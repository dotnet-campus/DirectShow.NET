using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IAMGraphStreamsTest
    {
        IAMGraphStreams m_igs;
        IFilterGraph2 m_FilterGraph;
        IPin m_pin;

        public IAMGraphStreamsTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestLatency();
                TestSync();
                TestFind();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_pin);
            }
        }

        private void TestLatency()
        {
            int hr;

            hr = m_igs.SyncUsingStreamOffset(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_igs.SetMaxGraphLatency(100);
            Debug.Assert(hr == 0, "SetMaxGraphLatency2");

            hr = m_igs.SyncUsingStreamOffset(false);
            DsError.ThrowExceptionForHR(hr);

            hr = m_igs.SetMaxGraphLatency(100);
            Debug.Assert(hr < 0, "SetMaxGraphLatency1");
        }

        private void TestSync()
        {
            int hr;

            hr = m_igs.SyncUsingStreamOffset(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_igs.SyncUsingStreamOffset(false);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestFind()
        {
            int hr;
            object o;
            IBaseFilter ibf;

            hr = m_igs.FindUpstreamInterface(m_pin, typeof(IBaseFilter).GUID, out o, AMIntfSearchFlags.None);
            DsError.ThrowExceptionForHR(hr);

            ibf = o as IBaseFilter;
            Debug.Assert(ibf != null, "FindUpstreamInterface");
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

            m_pin = DsFindPin.ByDirection(pRender, PinDirection.Input, 0);
            m_igs = (IAMGraphStreams)m_FilterGraph;

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);
            Marshal.ReleaseComObject(pRender);
        }
    }
}
