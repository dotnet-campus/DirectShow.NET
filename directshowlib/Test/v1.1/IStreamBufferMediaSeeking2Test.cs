using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferMediaSeeking2Test
    {
        IFilterGraph2 m_FilterGraph;
        IFilterGraph2 m_FilterGraph2;
        IStreamBufferMediaSeeking2 m_sbms;

        public IStreamBufferMediaSeeking2Test()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestSetRateEx();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sbms);
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_FilterGraph2);
            }
        }

        private void TestSetRateEx()
        {
            int hr;

            hr = m_sbms.SetRateEx(1.0, 22);
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

            IStreamBufferSink isbc = (IStreamBufferSink)sbk;
            hr = isbc.LockProfile(null);
            DsError.ThrowExceptionForHR(hr);

            IPin i = DsFindPin.ByDirection((IBaseFilter)sbk, PinDirection.Input, 0);

            ((IMediaControl)m_FilterGraph).Run();
            // --------------------------
            IBaseFilter streamBuffer = null;
            m_FilterGraph2 = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds2 = new DsROTEntry(m_FilterGraph2);

            streamBuffer = (IBaseFilter) new StreamBufferSource();

            hr = m_FilterGraph2.AddFilter( streamBuffer, "Stream buffer sink" );
            DsError.ThrowExceptionForHR( hr );

            IStreamBufferSource sbsrc = (IStreamBufferSource) streamBuffer;

            hr = sbsrc.SetStreamSink(isbc);
            DsError.ThrowExceptionForHR(hr);

            IPin i2 = DsFindPin.ByDirection((IBaseFilter)streamBuffer, PinDirection.Output, 0);

            ((IMediaControl)m_FilterGraph2).Run();

            m_sbms = (IStreamBufferMediaSeeking2)sbsrc;
            // --------------------------

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);

        }
    }
}
