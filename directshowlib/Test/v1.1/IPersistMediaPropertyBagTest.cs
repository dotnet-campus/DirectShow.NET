using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IPersistMediaPropertyBagTest : IErrorLog
    {
        IPersistMediaPropertyBag m_pPersist;
        IPersistMediaPropertyBag m_pPersist2;
        IMediaPropertyBag m_impb;
        IFilterGraph2 m_filterGraph;

        const string Author = "My Author String";
        const string Copyright = "My Copyright string";
        const string Title = "My Title String";
        const string FileName = @"a.avi";

        public IPersistMediaPropertyBagTest()
        {
        }

        public void DoTests()
        {
            EventCode ec;

            Configure();

            try
            {
                TestLoad();

                ((IMediaControl)m_filterGraph).Run();
                ((IMediaEvent)m_filterGraph).WaitForCompletion(-1, out ec);
                ((IMediaControl)m_filterGraph).Stop();

                Configure2();

                TestSave();
                TestInit();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pPersist);
                Marshal.ReleaseComObject(m_pPersist2);
                Marshal.ReleaseComObject(m_impb);
                Marshal.ReleaseComObject(m_filterGraph);
            }
        }

        private void TestLoad()
        {
            int hr;

            hr = m_pPersist.Load(m_impb, null);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pPersist.Load(m_impb, this);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestSave()
        {
            int hr;
            object o1, o2;
            IMediaPropertyBag pb = (IMediaPropertyBag)new MediaPropertyBag();

            hr = m_pPersist2.Save(pb, false, false);
            DsError.ThrowExceptionForHR(hr);

            hr = pb.EnumProperty(0, out o1, out o2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(TrimNull((string)o2) == Author, "Save");
        }

        private void TestInit()
        {
            int hr;

            hr = m_pPersist2.InitNew();
            DsError.ThrowExceptionForHR(hr);
        }

        string TrimNull(string s)
        {
            char [] c = new char[1];
            c[0] = '\0';
            return s.TrimEnd(c);
        }

        private void Configure()
        {
            int hr;
            IBaseFilter pFilter;
            IBaseFilter mux;
            IFileSinkFilter ifsf;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_filterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_filterGraph);

            hr = icgb.SetFiltergraph(m_filterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = m_filterGraph.AddSourceFilter("foo.avi", "foo", out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.SetOutputFileName(MediaSubType.Avi, FileName, out mux, out ifsf);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, pFilter, null, mux);
            DsError.ThrowExceptionForHR(hr);

            m_impb = (IMediaPropertyBag)new MediaPropertyBag();

            Load(m_impb, "IART", Author);
            Load(m_impb, "ICOP", Copyright);
            Load(m_impb, "INAM", Title);

            m_pPersist = (IPersistMediaPropertyBag)mux;

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(ifsf);
            Marshal.ReleaseComObject(icgb);
        }

        private void Configure2()
        {
            int hr;
            IBaseFilter pFilter;
            IBaseFilter ibf;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            IFilterGraph2 FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(FilterGraph);

            hr = icgb.SetFiltergraph(FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = FilterGraph.AddSourceFilter(FileName, "foo", out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, pFilter, null, null);
            DsError.ThrowExceptionForHR(hr);

            hr = FilterGraph.FindFilterByName("AVI Splitter", out ibf);
            DsError.ThrowExceptionForHR(hr);

            m_pPersist2 = (IPersistMediaPropertyBag)ibf;

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);
        }

        void Load(IMediaPropertyBag impb, string s1, string s2)
        {
            int hr;
            object o = s2;

            hr = impb.Write("INFO/" + s1, ref o);
            DsError.ThrowExceptionForHR(hr);
        }

        #region IErrorLog Members

        public int AddError(string pszPropName, System.Runtime.InteropServices.ComTypes.EXCEPINFO pExcepInfo)
        {
            // TODO:  Add IPersistMediaPropertyBagTest.AddError implementation
            return 0;
        }

        #endregion
    }
}
