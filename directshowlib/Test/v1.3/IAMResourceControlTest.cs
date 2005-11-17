using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMResourceControlTest
	{
        IAMResourceControl m_irc;

		public IAMResourceControlTest()
		{
        }

        public void DoTests()
        {
            Config();

            TestReserve();
        }

        private void TestReserve()
        {
            int hr;

            hr = m_irc.Reserve(AMResCtlReserveFlags.Reserve, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_irc = (IAMResourceControl) new AudioRender();
        }
	}
}
