using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

// This interface is a bridge between application and driver (user mode to kernel mode).
// It use KS properties to set values on the tuner node.
//
// All methods return -2147467262 (E_NOINTERFACE) on both boards except :
//    - put_Bandwidth, put_Frequency & put_FrequencyMultiplier return 0 (S_OK) on my DVB-T board
//    - All put_ methods (minus put_Autotune) return 0 (S_OK) on DVB-S board

namespace DirectShowLib.Test
{
	public class IBDA_FrequencyFilterTest
	{
    // Use this field to change the network type...
    private BDANetworkType networkType = BDANetworkType.DVBS;

    private IFilterGraph2 graphBuilder;
    private DsROTEntry rot;
    private IBaseFilter networkProvider;
    private ITuner tuner;

    private ITuningSpace tuningSpace;
    private ITuneRequest tuneRequest;

    private IBaseFilter bdaTuner, bdaCapture;
    private IBaseFilter mpeg2Demux;
    private IBaseFilter bdaTIF, bdaSecTab;

    private IBDA_FrequencyFilter freqFilter = null;
    
    public IBDA_FrequencyFilterTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();
      MakeATuneRequestAndRunTheGraph();

      try
      {
        TestAutotune();
        TestBandwidth();
        TestFrequency();
        TestFrequencyMultiplier();
        TestPolarity();
        TestRange();
      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(freqFilter);

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
              Debug.WriteLine(string.Format("node {0}/{1} : {2}", i, j, nodeGuid[j]));
              Console.WriteLine(string.Format("node {0}/{1} : {2}", i, j, nodeGuid[j]));
          
              // Is IBDA_FrequencyFilter supported by this node ?
              if (nodeGuid[j] == typeof(IBDA_FrequencyFilter).GUID)
              {
                // Yes, retrieve this node
                object ctrlNode;
                hr = topo.GetControlNode(0, 1, nodeTypes[i], out ctrlNode);
                DsError.ThrowExceptionForHR(hr);

                // Do the cast (it should not fail)
                freqFilter = ctrlNode as IBDA_FrequencyFilter;

                // Exit the for j loop if found a SignalStatistics object
                if (freqFilter != null)
                  break;
              }
            }

            // If already found a SignalStatistics object, exit the for i loop
            if (freqFilter != null)
              break;
          }
        }
      }
    }

    private void MakeATuneRequestAndRunTheGraph()
    {
      int hr = 0;
      ILocator locator;

      if (networkType == BDANetworkType.DVBT)
      {
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
      }

      if (networkType == BDANetworkType.DVBS)
      {
        // Those values are valid for me but must be modified to be valid depending on your Satellite dish
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
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(1);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(260);
        DsError.ThrowExceptionForHR(hr);
      }

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

    private void TestAutotune()
    {
      int hr = 0;
      int freq2, freq1 = 0;

      if (networkType == BDANetworkType.DVBT)
      {
        freq1 = 586166;
      }
      if (networkType == BDANetworkType.DVBS)
      {
        freq1 = 11591050;
      }

      hr = freqFilter.put_Autotune(freq1);
      //DsError.ThrowExceptionForHR(hr);

      hr = freqFilter.get_Autotune(out freq2);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestBandwidth()
    {
      int hr = 0;
      int bandwidth;

      hr = freqFilter.put_Bandwidth(8);
      //DsError.ThrowExceptionForHR(hr);

      hr = freqFilter.get_Bandwidth(out bandwidth);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestFrequency()
    {
      int hr = 0;
      int freq2, freq1 = 0;

      if (networkType == BDANetworkType.DVBT)
      {
        freq1 = 586166;
      }
      if (networkType == BDANetworkType.DVBS)
      {
        freq1 = 11591050;
      }

      hr = freqFilter.put_Frequency(freq1);
      //DsError.ThrowExceptionForHR(hr);

      hr = freqFilter.get_Frequency(out freq2);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestFrequencyMultiplier()
    {
      int hr = 0;
      int freqMul;

      hr = freqFilter.put_FrequencyMultiplier(1000);
      //DsError.ThrowExceptionForHR(hr);

      hr = freqFilter.get_FrequencyMultiplier(out freqMul);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestPolarity()
    {
      int hr = 0;
      Polarisation pol;

      hr = freqFilter.put_Polarity(Polarisation.LinearV);
      //DsError.ThrowExceptionForHR(hr);

      hr = freqFilter.get_Polarity(out pol);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestRange()
    {
      int hr = 0;
      int range;

      hr = freqFilter.put_Range(1234);
      //DsError.ThrowExceptionForHR(hr);

      hr = freqFilter.get_Range(out range);
      //DsError.ThrowExceptionForHR(hr);
    }

	}
}
