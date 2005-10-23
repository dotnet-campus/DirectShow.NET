using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_SignalStatisticsTest
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

    IBDA_SignalStatistics signalStats;
    
    public IBDA_SignalStatisticsTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();
        DoAValidTuneRequest();

        (graphBuilder as IMediaControl).Run();
        System.Threading.Thread.Sleep(1000);

        TestSignalPresent();
        TestSignalLocked();
        TestSignalQuality();
        TestSignalStrength();
        TestSampleTime();


      }
      finally
      {
        if (rot != null)
          rot.Dispose();

        (graphBuilder as IMediaControl).Stop();

        Marshal.ReleaseComObject(signalStats);
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

          IBDA_Topology topo = (IBDA_Topology) bdaTuner;
          int[] nodeTypes = new int[10];
          int nodeTypesCount;

          // Get all nodes in the BDA Tuner
          hr = topo.GetNodeTypes(out nodeTypesCount, nodeTypes.Length, nodeTypes);
          DsError.ThrowExceptionForHR(hr);

          // For each nodes types
          for (int i = 0; i < nodeTypesCount; i++)
          {
            Guid[] nodeGuid = new Guid[10];
            int nodeGuidCount;

            // Get its exposed interfaces
            hr = topo.GetNodeInterfaces(nodeTypes[i], out nodeGuidCount, nodeGuid.Length, nodeGuid);
            DsError.ThrowExceptionForHR(hr);

            // For each exposed interfaces
            for (int j = 0; j < nodeGuidCount; j++)
            {
              // Is IBDA_SignalStatistics supported by this node ?
              if (nodeGuid[j] == typeof(IBDA_SignalStatistics).GUID)
              {
                // Yes, retrieve this node
                object ctrlNode;
                hr = topo.GetControlNode(0, 1, nodeTypes[i], out ctrlNode);
                DsError.ThrowExceptionForHR(hr);

                // Do the cast (it should not fail)
                signalStats = ctrlNode as IBDA_SignalStatistics;

                // Exit the for j loop if found a SignalStatistics object
                if (signalStats != null)
                  break;
              }
            }

            // If already found a SignalStatistics object, exit the for i loop
            if (signalStats != null)
              break;
          }

        }
      }
    }

    private void DoAValidTuneRequest()
    {
      int hr = 0;
      ILocator locator;

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

      if (tuner.Validate(tuneRequest) == 0)
      {
        hr = tuner.put_TuneRequest(tuneRequest);
      }    
    }

    private void TestSignalPresent()
    {
      int hr = 0;
      bool sigPresent;

      // Since a valid tune request had been done, Signal should be present
      hr = signalStats.get_SignalPresent(out sigPresent);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(sigPresent == true, "IBDA_SignalStatistics.get_SignalPresent");

      hr = signalStats.put_SignalPresent(false);
      //DsError.ThrowExceptionForHR(hr);
      // on my HW, this method always return -2147467262

      hr = signalStats.get_SignalPresent(out sigPresent);
      //DsError.ThrowExceptionForHR(hr);

      //Debug.Assert(sigPresent == false, "IBDA_SignalStatistics.get_SignalPresent / put_SignalPresent");
    }

    private void TestSignalLocked()
    {
      int hr = 0;
      bool sigLocked;

      // Since a valid tune request had been done, Signal should be locked
      hr = signalStats.get_SignalLocked(out sigLocked);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(sigLocked == true, "IBDA_SignalStatistics.get_SignalLocked");

      hr = signalStats.put_SignalLocked(false);
      //DsError.ThrowExceptionForHR(hr);
      // on my HW, this method always return -2147467262

      hr = signalStats.get_SignalLocked(out sigLocked);
      //DsError.ThrowExceptionForHR(hr);

      //Debug.Assert(sigLocked == false, "IBDA_SignalStatistics.get_SignalLocked / put_SignalLocked");
    }

    private void TestSignalStrength()
    {
      int hr = 0;
      int sigStrength; //in mDb

      // This method seem to be badly documented in the SDK.
      // According to the SDK, this method should return signal strength in Db -> so a positive number
      // BUT the DDK say about KSPROPERTY_BDA_SIGNAL_STRENGTH (this kernel property is used by get_SignalStrength)
      // "A strength of 0 is nominal strength as expected for the given type of
      // broadcast network. Sub-nominal strengths are reported as positive mDb.
      // Super-nominal strengths are reported as negative mDb."
      // 
      // http://discuss.microsoft.com/SCRIPTS/WA-MSD.EXE?A2=ind0306&L=DIRECTXAV&P=R19999&I=-3&X=050DEC74A7F238772F&Y=eric.nowinski%40mail.com
      // With my Hardware i get -41000 -> +41Db Better-than-nominal strengths

      // Since a valid tune request had been done, Signal Strength should be set
      hr = signalStats.get_SignalStrength(out sigStrength);
      DsError.ThrowExceptionForHR(hr);

      // This test is very HW dependant...
      Debug.Assert(sigStrength <= 0, "IBDA_SignalStatistics.get_SignalStrength");

      hr = signalStats.put_SignalStrength(0);
      //DsError.ThrowExceptionForHR(hr);
      // on my HW, this method always return -2147467262

      hr = signalStats.get_SignalStrength(out sigStrength);
      //DsError.ThrowExceptionForHR(hr);

      //Debug.Assert(sigLocked == false, "IBDA_SignalStatistics.get_SignalLocked / put_SignalLocked");
    }

    private void TestSignalQuality()
    {
      int hr = 0;
      int sigQuality; //in %

      // Since a valid tune request had been done, Signal Quality should be set
      hr = signalStats.get_SignalQuality(out sigQuality);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(sigQuality > 0, "IBDA_SignalStatistics.get_SignalQuality");

      hr = signalStats.put_SignalQuality(0);
      //DsError.ThrowExceptionForHR(hr);
      // on my HW, this method always return -2147467262

      hr = signalStats.get_SignalQuality(out sigQuality);
      //DsError.ThrowExceptionForHR(hr);

      //Debug.Assert(sigLocked == false, "IBDA_SignalStatistics.get_SignalLocked / put_SignalLocked");
    }

    private void TestSampleTime()
    {
      int hr = 0;
      int sampleTime;

      hr = signalStats.get_SampleTime(out sampleTime);
      //DsError.ThrowExceptionForHR(hr);
      // on my HW, this method always return -2147467262

      hr = signalStats.put_SampleTime(0);
      //DsError.ThrowExceptionForHR(hr);
      // on my HW, this method always return -2147467262

      hr = signalStats.get_SampleTime(out sampleTime);
      //DsError.ThrowExceptionForHR(hr);

    }

	}
}
