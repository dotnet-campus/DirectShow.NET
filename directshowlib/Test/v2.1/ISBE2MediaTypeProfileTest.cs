using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;

namespace v2_1
{
    public class ISBE2MediaTypeProfileTest
    {
        ISBE2MediaTypeProfile m_pProfile;

        public ISBE2MediaTypeProfileTest()
        {
        }

        public void DoTests()
        {
            Config();

            TestStreams();
        }

        private void TestStreams()
        {
            int hr;
            int i, i2, i3;
            AMMediaType mt;

            hr = m_pProfile.GetStreamCount(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 2);

            hr = m_pProfile.GetStream(0, out mt);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pProfile.AddStream(mt);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pProfile.GetStreamCount(out i2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i2 == 3);

            hr = m_pProfile.DeleteStream(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pProfile.GetStreamCount(out i3);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == i3);
        }

        private void Config()
        {
            int hr;
            ISBE2Crossbar ISBE2Crossbar;
            IFilterGraph2 fg;

            fg = new FilterGraph() as IFilterGraph2;
            IBaseFilter streamBuffer = (IBaseFilter)new StreamBufferSource();

            hr = fg.AddFilter(streamBuffer, "SBS");

            IFileSourceFilter fs = streamBuffer as IFileSourceFilter;
            hr = fs.Load(@"C:\Users\Public\Recorded TV\Sample Media\win7_scenic-demoshort_raw.wtv", null);

            ISBE2Crossbar = streamBuffer as ISBE2Crossbar;

            hr = ISBE2Crossbar.EnableDefaultMode(CrossbarDefaultFlags.None);
            DsError.ThrowExceptionForHR(hr);

            hr = ISBE2Crossbar.GetInitialProfile(out m_pProfile);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
