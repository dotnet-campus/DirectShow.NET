using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class BdaGraph
  {
    // Use this field to change the network type...
    public BDANetworkType networkType = BDANetworkType.DVBS;

    public IFilterGraph2 graphBuilder;
    public DsROTEntry rot;
    public IBaseFilter networkProvider;
    public ITuner tuner;

    public ITuningSpace tuningSpace;
    public ITuneRequest tuneRequest;

    public IBaseFilter bdaTuner, bdaCapture;
    public IBaseFilter mpeg2Demux;
    public IBaseFilter bdaTIF, bdaSecTab;

    public void InitializeGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2)new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      ICaptureGraphBuilder2 capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
      capBuilder.SetFiltergraph(graphBuilder);

      // Get the BDA network provider specific for this given network type
      networkProvider = BDAUtils.GetNetworkProvider(networkType);

      hr = graphBuilder.AddFilter(networkProvider, "BDA Network Provider");
      DsError.ThrowExceptionForHR(hr);

      tuner = (ITuner)networkProvider;


      // found a BDA Tuner and a BDA Capture that can connect to this network provider
      BDAUtils.AddBDATunerAndDemodulatorToGraph(graphBuilder, networkProvider, out bdaTuner, out bdaCapture);

      if ((bdaTuner != null) && (bdaCapture != null))
      {
        // Create and add the mpeg2 demux
        mpeg2Demux = (IBaseFilter)new MPEG2Demultiplexer();

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
        else
          throw new ApplicationException("Can't add the BDA support filters");
      }
      else
        throw new ApplicationException("Can't add a tuner and a demodulator");
    }

    public void MakeTuneRequest()
    {
      int hr = 0;
      ILocator locator;

      // Get a tuning space for this network type
      tuningSpace = BDAUtils.GetTuningSpace(networkType);

      hr = tuner.put_TuningSpace(tuningSpace);
      DsError.ThrowExceptionForHR(hr);

      // Create a tune request from this tuning space
      tuneRequest = BDAUtils.CreateTuneRequest(tuningSpace);

      // Is it okay ?
      hr = tuner.Validate(tuneRequest);
      //DsError.ThrowExceptionForHR(hr);

      if (networkType == BDANetworkType.DVBT)
      {
        // Those values are valid for me but must be modified to be valid depending on your location
        hr = tuneRequest.get_Locator(out locator);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_CarrierFrequency(586166);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(1);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(259);
        DsError.ThrowExceptionForHR(hr);

/*
        hr = locator.put_CarrierFrequency(522166);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(3);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(769);
        DsError.ThrowExceptionForHR(hr);
*/
/*
        hr = locator.put_CarrierFrequency(562166);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(6);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(1537);
        DsError.ThrowExceptionForHR(hr);
*/
      }

      if (networkType == BDANetworkType.DVBS)
      {
        // Those values are valid for me but must be modified to be valid depending on your Satellite dish
/*
        hr = tuneRequest.get_Locator(out locator);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_CarrierFrequency(11591000);
        DsError.ThrowExceptionForHR(hr);

        hr = (locator as IDVBSLocator).put_SignalPolarisation(Polarisation.LinearV);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_SymbolRate(20000);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(1);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(260);
        DsError.ThrowExceptionForHR(hr);
*/

        hr = tuneRequest.get_Locator(out locator);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_CarrierFrequency(11607000);
        DsError.ThrowExceptionForHR(hr);

        hr = (locator as IDVBSLocator).put_SignalPolarisation(Polarisation.LinearV);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_SymbolRate(6944);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(144);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(100);
        DsError.ThrowExceptionForHR(hr);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(289);
        DsError.ThrowExceptionForHR(hr);
      }

      hr = tuner.put_TuneRequest(tuneRequest);
      if (hr < 0)
      {
        Debug.Fail("TuneRequest is not valid");
      }
    }

    public void RunGraph()
    {
      int hr = 0;

      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    public void DecomposeGraph()
    {
      int hr = 0;

      hr = (graphBuilder as IMediaControl).Stop();
      rot.Dispose();

      Marshal.ReleaseComObject(bdaTIF);
      Marshal.ReleaseComObject(bdaSecTab);
      Marshal.ReleaseComObject(mpeg2Demux);
      Marshal.ReleaseComObject(bdaTuner);
      Marshal.ReleaseComObject(bdaCapture);
      Marshal.ReleaseComObject(tuneRequest);
      Marshal.ReleaseComObject(bdaCapture);
      Marshal.ReleaseComObject(networkProvider);
      Marshal.ReleaseComObject(graphBuilder);
    }

  }
}
