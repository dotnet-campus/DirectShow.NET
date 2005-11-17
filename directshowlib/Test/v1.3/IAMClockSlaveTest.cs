using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMClockSlaveTest
	{
        private IAMClockSlave m_cs;

		public IAMClockSlaveTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestTolerance();
            }
            finally
            {
                Marshal.ReleaseComObject(m_cs);
            }
        }

        private void TestTolerance()
        {
            int hr;
            int it;
            
            hr = m_cs.SetErrorTolerance(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_cs.GetErrorTolerance(out it);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(it == 123, "ErrorTolerance");
        }

        private void Config()
        {
            m_cs = new AudioRender() as IAMClockSlave;
        }
	}
}
