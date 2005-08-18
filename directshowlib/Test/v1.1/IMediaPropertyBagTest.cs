using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IMediaPropertyBagTest
    {
        IMediaPropertyBag m_impb;

        const string Author = "My Author String";
        const string Copyright = "My Copyright string";
        const string Title = "My Title String";
        const string FileName = @"a.avi";

        public IMediaPropertyBagTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestEnum();
            }
            finally
            {
                Marshal.ReleaseComObject(m_impb);
            }
        }

        private void TestEnum()
        {
            int hr;
            object o1, o2;

            hr = m_impb.EnumProperty(0, out o1, out o2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert((string)o1 == "INFO/IART" && (string)o2 == Author, "EnumProperty");
        }

        private void Configure()
        {
            int hr;
            IBaseFilter pFilter;
            IBaseFilter mux;
            IFileSinkFilter ifsf;

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

            m_impb = (IMediaPropertyBag)new MediaPropertyBag();

            Load(m_impb, "IART", Author);
            Load(m_impb, "ICOP", Copyright);
            Load(m_impb, "INAM", Title);

            IPersistMediaPropertyBag pPersist = (IPersistMediaPropertyBag)mux;
            hr = pPersist.Load(m_impb, null);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(mux);
            Marshal.ReleaseComObject(filterGraph);
            Marshal.ReleaseComObject(ifsf);
            Marshal.ReleaseComObject(icgb);
        }

        void Load(IMediaPropertyBag impb, string s1, string s2)
        {
            int hr;
            object o = s2;

            hr = impb.Write("INFO/" + s1, ref o);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
