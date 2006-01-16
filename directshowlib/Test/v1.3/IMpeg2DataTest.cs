using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IMpeg2DataTest
    {
        private IMpeg2Data m_m2d = null;

        public IMpeg2DataTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestGetStreamOfSection();
                TestGetSection();
                TestGetTable();
            }
            finally
            {
                Marshal.ReleaseComObject(m_m2d);
            }
        }

        private void TestGetStreamOfSection()
        {
            int hr;
            IMpeg2Stream pStream;
            ManualResetEvent mre = new ManualResetEvent(false);

            hr = m_m2d.GetStreamOfSections(1, 2, null, mre.Handle, out pStream);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetTable()
        {
            int hr;
            ISectionList psl;

            hr = m_m2d.GetTable(1, 2, null, 1, out psl);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetSection()
        {
            int hr;
            ISectionList isl;

            hr = m_m2d.GetSection(1, 2, null, 1, out isl);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_m2d = (IMpeg2Data) new Mpeg2Data();
        }
    }
}
