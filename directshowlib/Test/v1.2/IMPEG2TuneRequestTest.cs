using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IMPEG2TuneRequestTest
	{
    BDANetworkType networkType = BDANetworkType.DVBT;

    IFilterGraph2 graphBuilder;
    DsROTEntry rot;

    IBaseFilter networkProvider;
    ITuner tuner;

    ITuningSpace tuningSpace;
    ITuneRequest tuneRequest;

    IBaseFilter bdaTuner, bdaCapture;
    IBaseFilter mpeg2Demux;
    IBaseFilter bdaTIF, bdaSecTab;

    IMPEG2TuneRequest mpeg2TuneRequest;
    
    public IMPEG2TuneRequestTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        MakeATuneRequestAndRunTheGraph();

        // This is also a test for the IMPEG2TuneRequestFactory interface... 
        TestIMPEG2TuneRequestFactory();

        TestTSID();
        TestProgNo();
        
      }
      finally
      {
        if (rot != null)
          rot.Dispose();

        Marshal.ReleaseComObject(mpeg2TuneRequest);

        Marshal.ReleaseComObject(bdaSecTab);
        Marshal.ReleaseComObject(bdaTIF);
        Marshal.ReleaseComObject(mpeg2Demux);
        Marshal.ReleaseComObject(bdaCapture);
        Marshal.ReleaseComObject(bdaTuner);
        Marshal.ReleaseComObject(tuneRequest);
        Marshal.ReleaseComObject(tuningSpace);
        Marshal.ReleaseComObject(networkProvider);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      ICaptureGraphBuilder2 capBuilder = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
      capBuilder.SetFiltergraph(graphBuilder);

      // Get the BDA network provider specific for this given network type
      networkProvider = BDAUtils.GetNetworkProvider(networkType);

      hr = graphBuilder.AddFilter(networkProvider, "BDA Network Provider");
      DsError.ThrowExceptionForHR(hr);

      tuner = (ITuner) networkProvider;

      // Get a tuning space for this network type
      tuningSpace = BDAUtils.GetTuningSpace(networkType);

      hr = tuner.put_TuningSpace(tuningSpace);
      DsError.ThrowExceptionForHR(hr);

      // Create a tune request from this tuning space
      tuneRequest = BDAUtils.CreateTuneRequest(tuningSpace);

      // Is it okay ?
      hr = tuner.Validate(tuneRequest);
      if (hr == 0)
      {
        // Set it
        hr = tuner.put_TuneRequest(tuneRequest);
        DsError.ThrowExceptionForHR(hr);

        // found a BDA Tuner and a BDA Capture that can connect to this network provider
        BDAUtils.AddBDATunerAndDemodulatorToGraph(graphBuilder, networkProvider, out bdaTuner, out bdaCapture);

        if ((bdaTuner != null) && (bdaCapture != null)) 
        {
          // Create and add the mpeg2 demux
          mpeg2Demux = (IBaseFilter) new MPEG2Demultiplexer();

          hr = graphBuilder.AddFilter(mpeg2Demux, "MPEG2 Demultiplexer");
          DsError.ThrowExceptionForHR(hr);

          // connect it to the BDA Capture
          hr = capBuilder.RenderStream(null, null, bdaCapture, null, mpeg2Demux);
          DsError.ThrowExceptionForHR(hr);

          // Add the two mpeg2 transport stream helper filters
          BDAUtils.AddTransportStreamFiltersToGraph(graphBuilder, out bdaTIF, out bdaSecTab);

          if ((bdaTIF != null) && (bdaSecTab != null))
          {
            // Render all the output pins of the demux (missing filters are added)
            for (int i = 0; i < 5; i++)
            {
              IPin pin = DsFindPin.ByDirection(mpeg2Demux, PinDirection.Output, i);

              hr = graphBuilder.Render(pin);
              Marshal.ReleaseComObject(pin);
            }
          }
        }
      }
    }

    private void MakeATuneRequestAndRunTheGraph()
    {
      int hr = 0;
      ILocator locator;

      // Assume a DVBT hardware.
      // Those values are valid for me but must be modified to be valid depending on your location
      hr = tuneRequest.get_Locator(out locator);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.put_CarrierFrequency(586000);
      DsError.ThrowExceptionForHR(hr);

      hr = tuneRequest.put_Locator(locator);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(locator);

      hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
      hr = (tuneRequest as IDVBTuneRequest).put_TSID(1);
      hr = (tuneRequest as IDVBTuneRequest).put_SID(260);
      DsError.ThrowExceptionForHR(hr);

      if (tuner.Validate(tuneRequest) == 0)
      {
        hr = tuner.put_TuneRequest(tuneRequest);
        hr = (graphBuilder as IMediaControl).Run();
      }
      else
      {
        Debug.Fail("TuneRequest is not valid");
      }
    }

    private void TestIMPEG2TuneRequestFactory()
    {
      int hr = 0;
      IMPEG2TuneRequestFactory mpeg2TRFactory;

      mpeg2TRFactory = (new MPEG2TuneRequestFactory() as IMPEG2TuneRequestFactory);
      Debug.Assert(mpeg2TRFactory != null, "MPEG2TuneRequestFactory constructor failed");

      hr = mpeg2TRFactory.CreateTuneRequest(tuningSpace, out mpeg2TuneRequest);
      Debug.Assert((hr == 0) && (mpeg2TuneRequest != null), "IMPEG2TuneRequestFactory.CreateTuneRequest");

      Marshal.ReleaseComObject(mpeg2TRFactory);
    }

    private void TestTSID()
    {
      int hr = 0;
      int tsid;

      hr = mpeg2TuneRequest.put_TSID(261);
      DsError.ThrowExceptionForHR(hr);

      hr = mpeg2TuneRequest.get_TSID(out tsid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(tsid == 261, "IMPEG2TuneRequest.get_TSID / put_TSID");
    }

    private void TestProgNo()
    {
      int hr = 0;
      int progNo;

      hr = mpeg2TuneRequest.put_ProgNo(1);
      DsError.ThrowExceptionForHR(hr);

      hr = mpeg2TuneRequest.get_ProgNo(out progNo);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(progNo == 1, "IMPEG2TuneRequest.get_ProgNo / put_ProgNo");
    }

	}
}
