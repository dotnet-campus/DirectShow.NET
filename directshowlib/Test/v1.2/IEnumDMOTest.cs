using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class IEnumDMOTest
    {
        IEnumDMO m_idmo;

        public IEnumDMOTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestDmoEnum();
                TestNext();
                TestSkip();
                TestClone();
                TestReset();
            }
            finally
            {
                Marshal.ReleaseComObject(m_idmo);
            }
        }

        private void TestDmoEnum()
        {
            int hr;
            IEnumDMO idmo;

            DMOPartialMediatype [] tIn = new DMOPartialMediatype[2];
            tIn[0].type = MediaType.Audio;
            tIn[0].subtype = Guid.Empty;

            tIn[1].type = MediaType.Video;
            tIn[1].subtype = Guid.Empty;

            hr = DMOUtils.DMOEnum(Guid.Empty, DMOEnumerator.IncludeKeyed, 2, tIn, 0, tIn, out idmo);
            DMOError.ThrowExceptionForHR(hr);

            int iCnt1 = CountEm(idmo);

            hr = DMOUtils.DMOEnum(Guid.Empty, DMOEnumerator.IncludeKeyed, 0, null, 0, null, out idmo);
            DMOError.ThrowExceptionForHR(hr);

            int iCnt2 = CountEm(idmo);

            Debug.Assert(iCnt1 == iCnt2, "Hopefully all DMOs are Video or Audio");

            // Looking for the MS Screen Encoder MSS1
            tIn[0].type = MediaType.Video;
            tIn[0].subtype = new Guid(0x3153534D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            tIn[0].subtype = new Guid("D990EE14-776C-4723-BE46-3DA2F56F10B9");

            hr = DMOUtils.DMOEnum(DMOCategory.VideoEncoder, DMOEnumerator.IncludeKeyed, 0, null, 1, tIn, out idmo);
            DMOError.ThrowExceptionForHR(hr);

            int iCnt3 = CountEm(idmo);

            Debug.Assert(iCnt3 == 1, "Test Category, and output partial media types");
        }

        private int CountEm(IEnumDMO idmo)
        {
            int hr;
            int iCnt = 0;
            Guid [] g = new Guid[1];
            string [] sn = new string[1];

            do
            {
                hr = idmo.Next(1, g, sn, IntPtr.Zero);
            } while (hr == 0 && iCnt++ < 100000);

            DMOError.ThrowExceptionForHR(hr);

            return iCnt;
        }

        private void TestNext()
        {
            int hr;
            IntPtr ip = Marshal.AllocCoTaskMem(4);
            Guid [] g = new Guid[3];
            string [] s = new string[3];

            hr = m_idmo.Next(3, g, s, ip);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(Marshal.ReadInt32(ip) == 3);

            Debug.Assert(s[2] != null && g[2] != Guid.Empty, "Next");
            Marshal.FreeCoTaskMem(ip);
        }

        private void TestSkip()
        {
            int hr;

            hr = m_idmo.Skip(1);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestClone()
        {
            int hr;
            IEnumDMO pEnum;

            hr = m_idmo.Clone(out pEnum);
            Debug.Assert(hr == unchecked((int)0x80004001), "Clone"); // Not implemented
        }

        private void TestReset()
        {
            int hr;

            hr = m_idmo.Reset();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            int hr;

            hr = DMOUtils.DMOEnum(Guid.Empty, DMOEnumerator.None, 0, null, 0, null, out m_idmo);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
