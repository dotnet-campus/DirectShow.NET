using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IIPDVDecTest
    {
        IIPDVDec m_dvd;

        public IIPDVDecTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestIPD();
            }
            finally
            {
                Marshal.ReleaseComObject(m_dvd);
            }
        }

        private void TestIPD()
        {
            int hr;
            DVDecoderResolution dp;

            hr = m_dvd.put_IPDisplay(DVDecoderResolution.r88x60);
            DsError.ThrowExceptionForHR(hr);

            hr = m_dvd.get_IPDisplay(out dp);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dp == DVDecoderResolution.r88x60, "IPDisplay");
        }

        private void Configure()
        {
            m_dvd = (IIPDVDec) new DVVideoCodec();
        }
    }
}
