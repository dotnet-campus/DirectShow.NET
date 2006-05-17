using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;

namespace v1._5
{
    public class IVPNotifyTest
    {
        IVPNotify m_vpn;

        public void DoTests()
        {
            Setup();

            TestNeg();
            TestDeinterlace();
        }

        private void TestNeg()
        {
            int hr;

            hr = m_vpn.RenegotiateVPParameters();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestDeinterlace()
        {
            int hr;
            AMVP_Mode pMode;

            hr = m_vpn.SetDeinterlaceMode(AMVP_Mode.SkipEven);
            //DsError.ThrowExceptionForHR(hr);

            hr = m_vpn.GetDeinterlaceMode(out pMode);
            //DsError.ThrowExceptionForHR(hr);

            //Debug.Assert(pMode == AMVP_Mode.SkipEven, "DeinterlaceMode");
        }

        private void Setup()
        {
            // I've tried OverlayMixer, OverlayMixer2 and VideoPortManager, and their
            // input pins.  All return NotImplemented for Get/Set DeinterlaceMode.
            // Also, they don't support IVPBaseNotify (which makes no sense).
            m_vpn = new OverlayMixer2() as IVPNotify;
        }
    }
}
