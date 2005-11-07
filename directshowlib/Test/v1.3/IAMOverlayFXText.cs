using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IAMOverlayFXTest
    {
        private IAMOverlayFX m_ofx;

        public IAMOverlayFXTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestQuery();
                TestOverlay();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ofx);
            }
        }

        private void TestOverlay()
        {
            int hr;
            AMOverlayFX c;

            hr = m_ofx.SetOverlayFX(AMOverlayFX.NoFX);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ofx.GetOverlayFX(out c);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(c == AMOverlayFX.NoFX, "OverlayFX");
        }

        private void TestQuery()
        {
            int hr;
            AMOverlayFX c;

            hr = m_ofx.QueryOverlayFXCaps(out c);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_ofx = (IAMOverlayFX)new OverlayMixer();
        }
    }
}
