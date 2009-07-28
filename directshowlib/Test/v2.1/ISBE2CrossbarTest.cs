using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;

namespace v2_1
{
    public class ISBE2CrossbarTest
    {
        private ISBE2Crossbar m_ISBE2Crossbar;
        private IFilterGraph2 m_fg;

        public ISBE2CrossbarTest()
        {
        }

        public void DoTests()
        {
            Config();

            TestMode();
            TestProf();
            TestEnum();
        }

        private void TestEnum()
        {
            int hr;
            ISBE2EnumStream pStr;

            IMediaControl mc = m_fg as IMediaControl;
            hr = mc.Run();
            DsError.ThrowExceptionForHR(hr);

            System.Threading.Thread.Sleep(10);

            hr = m_ISBE2Crossbar.EnumStreams(out pStr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pStr != null);
        }

        private void TestMode()
        {
            int hr;

            hr = m_ISBE2Crossbar.EnableDefaultMode(CrossbarDefaultFlags.None);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestProf()
        {
            int hr;
            ISBE2MediaTypeProfile pProfile;

            hr = m_ISBE2Crossbar.GetInitialProfile(out pProfile);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pProfile != null);

            int i = 0;
            IPin[] op = null;
            hr = m_ISBE2Crossbar.SetOutputProfile(pProfile, ref i, op);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 2);

            op = new IPin[i];
            hr = m_ISBE2Crossbar.SetOutputProfile(pProfile, ref i, op);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(op[0] != null && op[1] != null);

            ICaptureGraphBuilder2 cgb = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = cgb.SetFiltergraph(m_fg as IGraphBuilder);

            hr = cgb.RenderStream(null, MediaType.Video, op[0], null, null);
        }

        private void Config()
        {
            m_fg = new FilterGraph() as IFilterGraph2;
            IBaseFilter streamBuffer = (IBaseFilter)new StreamBufferSource();
            int hr;

            hr = m_fg.AddFilter(streamBuffer, "SBS");

            IFileSourceFilter fs = streamBuffer as IFileSourceFilter;
            hr = fs.Load(@"C:\Users\Public\Recorded TV\Sample Media\win7_scenic-demoshort_raw.wtv", null);

            m_ISBE2Crossbar = streamBuffer as ISBE2Crossbar;
        }
    }
}
