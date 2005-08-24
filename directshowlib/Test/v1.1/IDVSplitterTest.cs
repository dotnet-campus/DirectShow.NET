using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IDVSplitterTest
    {
        IDVSplitter m_dvs;

        public IDVSplitterTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestDisc();
            }
            finally
            {
                Marshal.ReleaseComObject(m_dvs);
            }
        }

        private void TestDisc()
        {
            int hr;

            hr = m_dvs.DiscardAlternateVideoFrames(false);
            DsError.ThrowExceptionForHR(hr);

            hr = m_dvs.DiscardAlternateVideoFrames(true);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_dvs = (IDVSplitter) new DVSplitter();
        }
    }
}
