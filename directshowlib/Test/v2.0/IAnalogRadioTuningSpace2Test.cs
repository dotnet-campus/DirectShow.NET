using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IAnalogRadioTuningSpace2Test
	{
        IAnalogRadioTuningSpace2 m_art;

        public void DoTests()
        {
            Config();

            TestCountry();
        }

        private void TestCountry()
        {
            int hr;
            int m;

            hr = m_art.put_CountryCode(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_art.get_CountryCode(out m);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m == 123, "Country");
        }

        private void Config()
        {
            m_art = new AnalogRadioTuningSpace() as IAnalogRadioTuningSpace2;
        }
	}
}
