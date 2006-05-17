using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;

namespace v1._5
{
    public class IVPNotify2Test
    {
        IVPNotify2 m_vpn;

        public void DoTests()
        {
            Setup();

            TestSync();
        }

        private void TestSync()
        {
            int hr;
            bool b, b2 ;

            hr = m_vpn.GetVPSyncMaster(out b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_vpn.SetVPSyncMaster(!b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_vpn.GetVPSyncMaster(out b2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b != b2, "VPSyncMaster");
        }

        private void Setup()
        {
            // I've tried OverlayMixer, OverlayMixer2 and VideoPortManager, and their
            // input pins.  All return NotImplemented for Get/Set DeinterlaceMode.
            // Also, they don't support IVPBaseNotify (which makes no sense).
            m_vpn = new OverlayMixer2() as IVPNotify2;
        }
    }
}
