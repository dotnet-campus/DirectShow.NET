using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IQualPropTest
    {
        IQualProp m_iqp;
        IFilterGraph2 m_FilterGraph;

        public IQualPropTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                System.Threading.Thread.Sleep(1000);
                GetAll();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void GetAll()
        {
            int hr;

            int afr, aso, dso, fd, pdr, jit;

            hr = m_iqp.get_AvgFrameRate(out afr);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iqp.get_AvgSyncOffset(out aso);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iqp.get_DevSyncOffset(out dso);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iqp.get_FramesDrawn(out fd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iqp.get_FramesDroppedInRenderer(out pdr);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iqp.get_Jitter(out jit);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(afr != 0, "afr");
            Debug.Assert(aso != 0, "aso");
            Debug.Assert(dso != 0, "dso");
            Debug.Assert(fd != 0, "fd");
            //Debug.Assert(pdr != 0, "pdr");
            Debug.Assert(jit != 0, "jit");
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
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

            ((IMediaControl)m_FilterGraph).Run();

            m_iqp = (IQualProp)pRender;

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);
        }
    }
}
