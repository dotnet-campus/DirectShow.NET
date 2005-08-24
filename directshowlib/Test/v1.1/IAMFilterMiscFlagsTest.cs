using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IAMFilterMiscFlagsTest
    {
        IAMFilterMiscFlags m_fmf1;
        IAMFilterMiscFlags m_fmf2;
        IFilterGraph2 m_FilterGraph;

        public IAMFilterMiscFlagsTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestFlags();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_fmf1);
                Marshal.ReleaseComObject(m_fmf2);
            }
        }

        private void TestFlags()
        {
            int hr;

            hr = m_fmf1.GetMiscFlags();
            Debug.Assert(hr == 1, "renderer");

            hr = m_fmf2.GetMiscFlags();
            Debug.Assert(hr == 0, "video source");
        }

        private void Configure()
        {
            int hr;
            IBaseFilter pFilter;
            IBaseFilter pRender = (IBaseFilter)new VideoRendererDefault();

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = m_FilterGraph.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.AddFilter(pRender, "renderererer");
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, pFilter, null, pRender);
            DsError.ThrowExceptionForHR(hr);

            m_fmf1 = (IAMFilterMiscFlags)pRender;
            m_fmf2 = (IAMFilterMiscFlags)pFilter;

            Marshal.ReleaseComObject(icgb);
        }
    }
}
