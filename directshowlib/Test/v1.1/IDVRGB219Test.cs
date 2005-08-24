using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IDVRGB219Test
    {
        IDVRGB219 m_dv219;

        public IDVRGB219Test()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                Test219();
            }
            finally
            {
                Marshal.ReleaseComObject(m_dv219);
            }
        }

        private void Test219()
        {
            int hr;

            hr = m_dv219.SetRGB219(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_dv219.SetRGB219(false);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_dv219 = (IDVRGB219) new DVVideoEnc();
        }
    }
}
