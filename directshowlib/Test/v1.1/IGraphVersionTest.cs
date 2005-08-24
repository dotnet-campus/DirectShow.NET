using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IGraphVersionTest
    {
        IFilterGraph2 m_FilterGraph;

        public IGraphVersionTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            int v;
            int oldv;

            IBaseFilter pFilter;
            IBaseFilter pRender = (IBaseFilter)new VideoRendererDefault();

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);
            IGraphVersion igv = (IGraphVersion)m_FilterGraph;

            hr = igv.QueryVersion(out v);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(v == 0, "Version1");
            oldv = v;

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = m_FilterGraph.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = igv.QueryVersion(out v);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(v > oldv, "Version2");
            oldv = v;

            hr = m_FilterGraph.AddFilter(pRender, "renderererer");
            DsError.ThrowExceptionForHR(hr);

            hr = igv.QueryVersion(out v);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(v > oldv, "Version2");
            oldv = v;

            hr = icgb.RenderStream(null, null, pFilter, null, pRender);
            DsError.ThrowExceptionForHR(hr);

            hr = igv.QueryVersion(out v);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(v > oldv, "Version4");
            oldv = v;

            Marshal.ReleaseComObject(icgb);
        }
    }
}
