using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class ITunerTest
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

    public ITunerTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        TestEnumTuningSpaces();
        TestPreferredComponentTypes();
        TestTuningSpace();
        TestTuneRequestAndValidate();

        (graphBuilder as IMediaControl).Run();

        TestSignalStrength();
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

    private void TestEnumTuningSpaces()
    {
      int hr = 0;
      IEnumTuningSpaces tuningSpaces;

      hr = tuner.EnumTuningSpaces(out tuningSpaces);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (tuningSpaces != null), "ITuner.EnumTuningSpaces");

      Marshal.ReleaseComObject(tuningSpaces);
    }

    private void TestPreferredComponentTypes()
    {
      int hr = 0;
      IComponentTypes compTypes;

      hr = tuner.get_PreferredComponentTypes(out compTypes);
      DsError.ThrowExceptionForHR(hr);

      hr = tuner.put_PreferredComponentTypes(compTypes);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuner.get_PreferredComponentTypes / put_PreferredComponentTypes");

      Marshal.ReleaseComObject(compTypes);
    }

    private void TestTuningSpace()
    {
      int hr = 0;
      ITuningSpace tuningSpace;
      string name;

      hr = tuner.get_TuningSpace(out tuningSpace);
      hr = tuningSpace.get_FriendlyName(out name);
      DsError.ThrowExceptionForHR(hr);
      
      Debug.Assert(name.EndsWith("TuningSpace"), "ITuner.get_TuningSpace");

      // put_TuningSpace have been tested in BuildGraph...
    }

    private void TestTuneRequestAndValidate()
    {
      int hr = 0;
      ITuneRequest tuneRequest;
      ILocator locator;

      // I have this value as global varible but i have to test get_TuneRequest... 
      hr = tuner.get_TuneRequest(out tuneRequest);
      Debug.Assert(hr == 0, "ITuner.get_TuneRequest");

      hr = tuneRequest.get_Locator(out locator);
      DsError.ThrowExceptionForHR(hr);

      // Assume a DVBT hardware
      // A valid carrier frequency must be passed...
      hr = (locator as IDVBTLocator).put_CarrierFrequency(586000);
      hr = tuneRequest.put_Locator(locator);

      // Select the first channel in the first transport stream found...
      hr = (tuneRequest as IDVBTuneRequest).put_ONID(-1);
      hr = (tuneRequest as IDVBTuneRequest).put_TSID(-1);
      hr = (tuneRequest as IDVBTuneRequest).put_SID(-1);

      // Is it a valid tune request ?
      hr = tuner.Validate(tuneRequest);

      Debug.Assert(hr == 0, "ITuner.Validate");

      hr = tuner.put_TuneRequest(tuneRequest);

      Debug.Assert(hr == 0, "ITuner.put_TuneRequest");
    }

    private void TestSignalStrength()
    {
      int hr = 0;
      int strength;

      hr = tuner.get_SignalStrength(out strength);
      DsError.ThrowExceptionForHR(hr);

      // If the tunerequest is correct, strength should be greater then zero...
      Debug.Assert(strength > 0, "ITuner.get_SignalStrength");
    }


	}
}
