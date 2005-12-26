using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IAnalogTVTuningSpaceTest
	{
    private IAnalogTVTuningSpace analogTVTS = null;

		public IAnalogTVTuningSpaceTest()
		{
		}

    public void DoTests()
    {
      analogTVTS = (IAnalogTVTuningSpace) new AnalogTVTuningSpace();

      try
      {
        TestCountryCode();
        TestInputType();
        TestMaxChannel();
        TestMinChannel();
      }
      finally
      {
        Marshal.ReleaseComObject(analogTVTS);
      }
    }

    private void TestCountryCode()
    {
      int hr = 0;
      int countryCode = 0;

      hr = analogTVTS.put_CountryCode(33);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_CountryCode(out countryCode);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(countryCode == 33, "IAnalogTVTuningSpace.get_CountryCode / put_CountryCode");

      hr = analogTVTS.put_CountryCode(1);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_CountryCode(out countryCode);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(countryCode == 1, "IAnalogTVTuningSpace.get_CountryCode / put_CountryCode");
    }

    private void TestInputType()
    {
      int hr = 0;
      TunerInputType inputType;

      hr = analogTVTS.put_InputType(TunerInputType.Antenna);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_InputType(out inputType);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(inputType == TunerInputType.Antenna, "IAnalogTVTuningSpace.get_InputType / put_InputType");

      hr = analogTVTS.put_InputType(TunerInputType.Cable);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_InputType(out inputType);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(inputType == TunerInputType.Cable, "IAnalogTVTuningSpace.get_InputType / put_InputType");
    }

    private void TestMaxChannel()
    {
      int hr = 0;
      int maxCh = 0;

      hr = analogTVTS.put_MaxChannel(50);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_MaxChannel(out maxCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(maxCh == 50, "IAnalogTVTuningSpace.get_MaxChannel / put_MaxChannel");

      hr = analogTVTS.put_MaxChannel(25);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_MaxChannel(out maxCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(maxCh == 25, "IAnalogTVTuningSpace.get_MaxChannel / put_MaxChannel");
    }

    private void TestMinChannel()
    {
      int hr = 0;
      int minCh = 0;

      hr = analogTVTS.put_MinChannel(5);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_MinChannel(out minCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(minCh == 5, "IAnalogTVTuningSpace.get_MinChannel / put_MinChannel");

      hr = analogTVTS.put_MinChannel(10);
      DsError.ThrowExceptionForHR(hr);

      hr = analogTVTS.get_MinChannel(out minCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(minCh == 10, "IAnalogTVTuningSpace.get_MinChannel / put_MinChannel");
    }
	}
}
