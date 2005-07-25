using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferSink2Test
    {
        IStreamBufferSink2 m_isbc;
        IFilterGraph2 m_FilterGraph;

        public IStreamBufferSink2Test()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestLock();
            }
            finally
            {
                Marshal.ReleaseComObject(m_isbc);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestLock()
        {
            int hr;

            hr = m_isbc.IsProfileLocked();
            Debug.Assert(hr == 1, "Locked 1");

            hr = m_isbc.LockProfile("delme.prf");
            DsError.ThrowExceptionForHR(hr);

            hr = m_isbc.IsProfileLocked();
            Debug.Assert(hr == 0, "Locked 2");

            hr = m_isbc.UnlockProfile();
            DsError.ThrowExceptionForHR(hr);

            hr = m_isbc.IsProfileLocked();
            Debug.Assert(hr == 1, "Locked 3");
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

            m_isbc = (IStreamBufferSink2)sbk;
        }
    }
}
