using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferSinkTest
    {
        IStreamBufferSink m_isbc;
        IFilterGraph2 m_FilterGraph;

        public IStreamBufferSinkTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestLock();
                TestRecorder();
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
        }

        private void TestRecorder()
        {
            const string FileName = "delme.out";
            int hr;
            object o;
            short c;

            File.Delete(FileName);

            hr = m_isbc.CreateRecorder("delme.out", RecordingType.Content, out o);
            DsError.ThrowExceptionForHR(hr);

            // Make sure we really got a recorder object
            IStreamBufferRecordingAttribute i = o as IStreamBufferRecordingAttribute;
            hr = i.GetAttributeCount(0, out c);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(c != 0, "CreateRecorder");
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

            m_isbc = (IStreamBufferSink)sbk;
        }
    }
}
