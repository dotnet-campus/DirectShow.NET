using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;
using System.Windows.Forms;

namespace v2_1
{
    public class IAMPluginControlTest
    {
        IAMPluginControl m_pi;

        public IAMPluginControlTest()
        {
        }

        public void DoTests()
        {
            int hr;
            Guid g, g2, g3, gA;

            m_pi = new DirectShowPluginControl() as IAMPluginControl;

            hr = m_pi.GetPreferredClsidByIndex(0, out g, out g2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pi.GetPreferredClsid(g, out g3);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(g3 == g2);

            hr = m_pi.IsDisabled(g2);
            Debug.Assert(hr < 0);

            hr = m_pi.SetDisabled(g2, true);
            DsError.ThrowExceptionForHR(hr);
            hr = m_pi.IsDisabled(g2);
            Debug.Assert(hr >= 0);

            hr = m_pi.GetDisabledByIndex(0, out gA);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(gA == g2);

            hr = m_pi.SetDisabled(g2, false);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pi.SetPreferredClsid(g, null);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pi.GetPreferredClsid(g, out g3);
            Debug.Assert(hr < 0);

            hr = m_pi.SetPreferredClsid(g, g2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pi.GetPreferredClsid(g, out g3);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g3 == g2);

            hr = m_pi.IsLegacyDisabled("asdf.acm"); // W7 has no disabled drivers
            Debug.Assert(hr < 0);                   // see HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectShow\DoNotUseDrivers32
        }
    }
}
