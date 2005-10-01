using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

// Note : this interface is supposed to be implemented just by the DVBT Network Provider...

namespace DirectShowLib.Test
{
	public class IFrequencyMapTest
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

    IFrequencyMap freqMap;

		public IFrequencyMapTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        TestCountryCode();
        Testget_CountryCodeList();
//        Testget_DefaultFrequencyMapping();
//        TestFrequencyMapping();


      }
      finally
      {
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

      // Currently only DVBT Network Provider support this interface...
      freqMap = (IFrequencyMap) networkProvider;
    }

    private void TestCountryCode()
    {
      int hr = 0;
      int countryCode;

      // Vive la France !!!
      hr = freqMap.put_CountryCode(33);
      DsError.ThrowExceptionForHR(hr);

      hr = freqMap.get_CountryCode(out countryCode);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(countryCode == 33, "IFrequencyMap.get_CountryCode / put_CountryCode");
    }

    private void Testget_CountryCodeList()
    {
      int hr = 0;
      int entryCount;
      IntPtr entriesPtr;
      int[] entries;

      hr = freqMap.get_CountryCodeList(out entryCount, out entriesPtr);
      DsError.ThrowExceptionForHR(hr);

      entries = new int[entryCount];
      Marshal.Copy(entriesPtr, entries, 0, entryCount);
      Marshal.FreeCoTaskMem(entriesPtr);

      Debug.Assert(entryCount > 0, "IFrequencyMap.get_CountryCodeList");
    }

    private void Testget_DefaultFrequencyMapping()
    {
      int hr = 0;
      int entryCount;
      int[] entries;

      hr = freqMap.get_DefaultFrequencyMapping(33, out entryCount, null);

      entries = new int[entryCount];
      hr = freqMap.get_DefaultFrequencyMapping(33, out entryCount, entries);

      // This method is not documented to not been implemented but this the case on my machine
      Debug.Assert(hr == -2147467263, "IFrequencyMap.get_CountryCodeList");
    }

    private void TestFrequencyMapping()
    {
      int hr = 0;
      // Frequencies for the Paris Eiffel Tower emitter...
      int[] localFreq = {586000, //R1
                         475000, //R2
                         522000, //R3
                         498000, //R4
                         562000, //R6 (R5 not assigned)
                        };
      int entryCount;
      int[] entries;

      hr = freqMap.put_FrequencyMapping(localFreq.Length, localFreq);
      DsError.ThrowExceptionForHR(hr);

      hr = freqMap.get_FrequencyMapping(out entryCount, null);

      entries = new int[entryCount];
      hr = freqMap.get_FrequencyMapping(out entryCount, null);

      // This method is not documented to not been implemented but this the case on my machine
      Debug.Assert(hr == -2147467263, "IFrequencyMap.get_CountryCodeList");
    }


	}
}
