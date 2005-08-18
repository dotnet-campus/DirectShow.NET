using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IAMMediaContentTest
    {
        IAMMediaContent m_imc;
        IFilterGraph2 m_FilterGraph;
        const string Author = "My Author String";
        const string Copyright = "My Copyright string";
        const string Title = "My Title String";
        const string FileName = @"a.avi";

        public IAMMediaContentTest()
        {
        }

        public void DoTests()
        {
            Configure();
            Configure2();

            try
            {
                TestAll();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_imc);
            }
        }

        void TestAll()
        {
            const int E_NOTIMPLE = unchecked((int)0x80004001);
            string s;
            int hr;

            hr = m_imc.get_AuthorName(out s);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(TrimNull(s) == Author, "Get_AuthorName");

            hr = m_imc.get_Copyright(out s);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(TrimNull(s) == Copyright, "get_Copyright");

            hr = m_imc.get_Title(out s);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(TrimNull(s) == Title, "get_Title");

            hr = m_imc.get_BaseURL(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_BaseURL");

            hr = m_imc.get_Description(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_Description");

            hr = m_imc.get_LogoIconURL(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_LogoIconURL");

            hr = m_imc.get_LogoURL(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_LogoURL");

            hr = m_imc.get_MoreInfoBannerImage(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_MoreInfoBannerImage");

            hr = m_imc.get_MoreInfoBannerURL(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_MoreInfoBannerURL");

            hr = m_imc.get_MoreInfoText(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_MoreInfoText");

            hr = m_imc.get_MoreInfoURL(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_MoreInfoURL");

            hr = m_imc.get_Rating(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_Rating");

            hr = m_imc.get_WatermarkURL(out s);
            Debug.Assert(hr == E_NOTIMPLE, "get_WatermarkURL");
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            IBaseFilter pFilter;
            IBaseFilter mux;
            IFileSinkFilter ifsf;
            EventCode ec;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            IFilterGraph2 filterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(filterGraph);

            hr = icgb.SetFiltergraph(filterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = filterGraph.AddSourceFilter("foo.avi", "foo", out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.SetOutputFileName(MediaSubType.Avi, FileName, out mux, out ifsf);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, pFilter, null, mux);
            DsError.ThrowExceptionForHR(hr);

            IMediaPropertyBag impb = (IMediaPropertyBag)new MediaPropertyBag();

            Load(impb, "IART", Author);
            Load(impb, "ICOP", Copyright);
            Load(impb, "INAM", Title);

            IPersistMediaPropertyBag pPersist = (IPersistMediaPropertyBag)mux;
            hr = pPersist.Load(impb, null);
            DsError.ThrowExceptionForHR(hr);

            ((IMediaControl)filterGraph).Run();
            ((IMediaEvent)filterGraph).WaitForCompletion(-1, out ec);
            ((IMediaControl)filterGraph).Stop();

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(mux);
            Marshal.ReleaseComObject(filterGraph);
            Marshal.ReleaseComObject(ifsf);
            Marshal.ReleaseComObject(impb);
            Marshal.ReleaseComObject(icgb);
        }

        void Load(IMediaPropertyBag impb, string s1, string s2)
        {
            int hr;
            object o = s2;

            hr = impb.Write("INFO/" + s1, ref o);
            DsError.ThrowExceptionForHR(hr);
        }

        string TrimNull(string s)
        {
            char [] c = new char[1];
            c[0] = '\0';
            return s.TrimEnd(c);
        }

        private void Configure2()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            IBaseFilter pFilter;
            IBaseFilter ibf;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.AddSourceFilter(FileName, "foo", out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, pFilter, null, null);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.FindFilterByName("AVI Splitter", out ibf);
            DsError.ThrowExceptionForHR(hr);

            m_imc = (IAMMediaContent)ibf;

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);
        }
    }
}
