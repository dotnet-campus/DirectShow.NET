using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMVideoDecimationPropertiesTest
	{
        IAMVideoDecimationProperties m_ivdp;

		public IAMVideoDecimationPropertiesTest()
		{
        }

        public void DoTests()
        {
            Config();

            TestQuery();
        }

        private void TestUsage()
        {
            int hr;
            DecimationUsage u;

            hr = m_ivdp.SetDecimationUsage(DecimationUsage.UseOverlayOnly);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ivdp.QueryDecimationUsage(out u);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(u == DecimationUsage.UseOverlayOnly, "SetDecimationUsage");
        }

        private void Config()
        {
            m_ivdp = (IAMVideoDecimationProperties) new OverlayMixer();
        }
	}
}
