using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

// This test don't work on my DVB-T board (Hauppauge) but does on my DVB-S board (Twinhan)
//
// This interface is a bridge between application and driver (user mode to kernel mode).
// It use KS properties to set values on the demux node.
//
// All put_ methods return -2147023728 (E_PROP_ID_UNSUPPORTED) on DVB-T and 0 (S_OK) on DVB-S
// All get_ methods return -2147467262 (E_NOINTERFACE) on both boards

namespace DirectShowLib.Test
{
	public class IBDA_DigitalDemodulatorTest
	{
    // Use this field to change the network type...
    private BDANetworkType networkType = BDANetworkType.DVBT;

    private IFilterGraph2 graphBuilder;
    private DsROTEntry rot;
    private IBaseFilter networkProvider;
    private ITuner tuner;

    private ITuningSpace tuningSpace;
    private ITuneRequest tuneRequest;

    private IBaseFilter bdaTuner, bdaCapture;
    private IBaseFilter mpeg2Demux;
    private IBaseFilter bdaTIF, bdaSecTab;

    private IBDA_DigitalDemodulator digitalDemux = null;

    public IBDA_DigitalDemodulatorTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();
      MakeATuneRequestAndRunTheGraph();

      try
      {
        TestFEC();
        TestModulationType();
        TestSpectralInversion();
        TestSymbolRate();

      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(digitalDemux);

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
          
              // Is IBDA_AutoDemodulate supported by this node ?
              if (nodeGuid[j] == typeof(IBDA_DigitalDemodulator).GUID)
              {
                // Yes, retrieve this node
                object ctrlNode;
                hr = topo.GetControlNode(0, 1, nodeTypes[i], out ctrlNode);
                DsError.ThrowExceptionForHR(hr);

                // Do the cast (it should not fail)
                digitalDemux = ctrlNode as IBDA_DigitalDemodulator;

                // Exit the for j loop if found a SignalStatistics object
                if (digitalDemux != null)
                  break;
              }
            }

            // If already found a SignalStatistics object, exit the for i loop
            if (digitalDemux != null)
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

    private void TestFEC()
    {
      int hr = 0;
      FECMethod fecMethod2, fecMethod1 = FECMethod.Viterbi;
      BinaryConvolutionCodeRate fecRate2, fecRate1 = BinaryConvolutionCodeRate.Rate2_3;

      hr = digitalDemux.put_InnerFECMethod(ref fecMethod1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_InnerFECMethod(out fecMethod2);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.put_InnerFECRate(ref fecRate1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_InnerFECRate(out fecRate2);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.put_OuterFECMethod(ref fecMethod1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_OuterFECMethod(out fecMethod2);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.put_OuterFECRate(ref fecRate1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_OuterFECRate(out fecRate2);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestModulationType()
    {
      int hr = 0;
      ModulationType modType2, modType1 = ModulationType.Mod64Qam;

      hr = digitalDemux.put_ModulationType(ref modType1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_ModulationType(out modType2);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestSpectralInversion()
    {
      int hr = 0;
      SpectralInversion spectral2, spectral1 = SpectralInversion.Inverted;

      hr = digitalDemux.put_SpectralInversion(ref spectral1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_SpectralInversion(out spectral2);
      //DsError.ThrowExceptionForHR(hr);
    }

    private void TestSymbolRate()
    {
      int hr = 0;
      int rate2, rate1 = 20000;

      hr = digitalDemux.put_SymbolRate(ref rate1);
      //DsError.ThrowExceptionForHR(hr);

      hr = digitalDemux.get_SymbolRate(out rate2);
      //DsError.ThrowExceptionForHR(hr);
    }

	}
}
