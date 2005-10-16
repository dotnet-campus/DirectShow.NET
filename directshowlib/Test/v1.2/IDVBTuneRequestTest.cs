using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBTuneRequestTest
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
    
    public IDVBTuneRequestTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        TestONID();
        TestTSID();
        TestSID();
      }
      finally
      {
        if (rot != null)
          rot.Dispose();

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

    // Just basic tests...
    // This interface have already been massively used before.
    private void TestONID()
    {
      int hr = 0;
      int onid;

      hr = (tuneRequest as IDVBTuneRequest).put_ONID(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = (tuneRequest as IDVBTuneRequest).get_ONID(out onid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(onid == 1234, "IDVBTuneRequest.get_ONID / put_ONID");
    }

    private void TestTSID()
    {
      int hr = 0;
      int tsid;

      hr = (tuneRequest as IDVBTuneRequest).put_TSID(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = (tuneRequest as IDVBTuneRequest).get_TSID(out tsid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(tsid == 1234, "IDVBTuneRequest.get_TSID / put_TSID");
    }

    private void TestSID()
    {
      int hr = 0;
      int sid;

      hr = (tuneRequest as IDVBTuneRequest).put_SID(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = (tuneRequest as IDVBTuneRequest).get_SID(out sid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(sid == 1234, "IDVBTuneRequest.get_SID / put_SID");
    }

	}
}
