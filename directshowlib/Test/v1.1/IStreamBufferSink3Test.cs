using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferSink3Test
    {
        IStreamBufferSink3 m_isbc;
        IFilterGraph2 m_FilterGraph;

        public IStreamBufferSink3Test()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestAvail();
            }
            finally
            {
                Marshal.ReleaseComObject(m_isbc);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestAvail()
        {
            int hr;
            long l = -long.MaxValue;

            /// Start the graph so we have something to chop off
            IMediaControl imc = (IMediaControl)m_FilterGraph;
            hr = imc.Run();
            DsError.ThrowExceptionForHR(hr);
            System.Threading.Thread.Sleep(3000);

            hr = m_isbc.SetAvailableFilter(ref l);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l != -long.MaxValue, "Avail");

            hr = imc.Stop();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            IBaseFilter pFilter;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            StreamBufferSink sbk = new StreamBufferSink();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.AddFilter((IBaseFilter) sbk, "StreamBufferSink");
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = m_FilterGraph.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            DVVideoEnc dve = new DVVideoEnc();
            IBaseFilter ibfVideoEnc = (IBaseFilter) dve;
            hr = m_FilterGraph.AddFilter( ibfVideoEnc, "dvenc" );
            DsError.ThrowExceptionForHR( hr );

            hr = icgb.RenderStream(null, null, pFilter, ibfVideoEnc, (IBaseFilter)sbk);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);

            m_isbc = (IStreamBufferSink3)sbk;
        }
    }
}
