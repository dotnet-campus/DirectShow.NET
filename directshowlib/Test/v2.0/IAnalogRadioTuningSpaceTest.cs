using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IAnalogRadioTuningSpaceTest
	{
        IAnalogRadioTuningSpace m_art;

        public void DoTests()
        {
            Config();

            TestStep();
            TestMaxFreq();
            TestMinFreq();
        }

        private void TestMaxFreq()
        {
            int hr;
            int m;

            hr = m_art.put_MaxFrequency(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_art.get_MaxFrequency(out m);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m == 123, "MaxFreq");
        }

        private void TestMinFreq()
        {
            int hr;
            int m;

            hr = m_art.put_MinFrequency(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_art.get_MinFrequency(out m);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m == 123, "MinFreq");
        }

        private void TestStep()
        {
            int hr;
            int s;

            hr = m_art.put_Step(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_art.get_Step(out s);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == 123, "Step");
        }

        private void Config()
        {
            m_art = new AnalogRadioTuningSpace() as IAnalogRadioTuningSpace;
        }
	}
}
