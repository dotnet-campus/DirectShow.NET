using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

// Note : this interface is supposed to be implemented just by the DVBT Network Provider...

namespace DirectShowLib.Test
{
	public class IFrequencyMapTest
	{
    IFrequencyMap freqMap;

		public IFrequencyMapTest()
		{
		}

    public void DoTests()
    {
      try
      {
        freqMap = (IFrequencyMap) new DVBTNetworkProvider();

        TestCountryCode();
        Testget_CountryCodeList();
        Testget_DefaultFrequencyMapping();
        TestFrequencyMapping();


      }
      finally
      {
        Marshal.ReleaseComObject(freqMap);
      }
    }

    private void TestCountryCode()
    {
      int hr = 0;
      int countryCode;

      // This method is not documented to not been implemented but this the case on my machine
      hr = freqMap.get_CountryCode(out countryCode);
      
      Debug.Assert(hr == -2147467263, "IFrequencyMap.get_CountryCode");

      // Vive la France !!!
      hr = freqMap.put_CountryCode(33);

      // This method is not documented to not been implemented but this the case on my machine
      Debug.Assert(hr == -2147467263, "IFrequencyMap.get_CountryCode");
    }

    private void Testget_CountryCodeList()
    {
      int hr = 0;
      int entryCount;
      int[] entries;

      hr = freqMap.get_CountryCodeList(out entryCount, null);

      entries = new int[entryCount];
      hr = freqMap.get_CountryCodeList(out entryCount, entries);

      // This method is not documented to not been implemented but this the case on my machine
      Debug.Assert(hr == -2147467263, "IFrequencyMap.get_CountryCodeList");
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
