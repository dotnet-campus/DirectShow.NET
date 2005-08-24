using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IFilterChainTest
    {
        IFilterChain m_fc;
        IFilterGraph2 m_FilterGraph;
        IBaseFilter m_pFilter;
        IBaseFilter m_pRender;

        public IFilterChainTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestPause();
                TestStop();
                TestStart();
                TestRemove();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_fc);
            }
        }

        private void TestPause()
        {
            int hr;

            ((IMediaControl)m_FilterGraph).Run();
            ((IMediaControl)m_FilterGraph).Pause();
            hr = m_fc.PauseChain(m_pFilter, m_pRender);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestStop()
        {
            int hr;

            hr = m_fc.StopChain(m_pFilter, null);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestStart()
        {
            int hr;

            ((IMediaControl)m_FilterGraph).Run();
            hr = m_fc.StartChain(m_pFilter, null);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestRemove()
        {
            int hr;

            hr = m_fc.RemoveChain(m_pFilter, m_pRender);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            m_pRender = (IBaseFilter)new VideoRendererDefault();

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = m_FilterGraph.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out m_pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.AddFilter(m_pRender, "renderererer");
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, m_pFilter, null, m_pRender);
            DsError.ThrowExceptionForHR(hr);

            m_fc = (IFilterChain)m_FilterGraph;

            Marshal.ReleaseComObject(icgb);
        }
    }
}
