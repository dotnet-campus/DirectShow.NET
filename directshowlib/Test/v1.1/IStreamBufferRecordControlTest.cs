using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferRecordControlTest
    {
        IStreamBufferRecordControl m_sbrc;
        IFilterGraph2 m_FilterGraph;

        public IStreamBufferRecordControlTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestStart();

                // Wait long enough for the start to start
                System.Threading.Thread.Sleep(3000);
                TestEnd();
                TestStat();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sbrc);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestStart()
        {
            int hr;
            long lStart = 3000000;

            hr = m_sbrc.Start(ref lStart);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestEnd()
        {
            int hr;

            hr = m_sbrc.Stop(2000000);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestStat()
        {
            int hr;
            int rethr = -1;
            bool bStart = false;
            bool bEnd = false;
            
            hr = m_sbrc.GetRecordingStatus(out rethr, out bStart, out bEnd);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(rethr == 0 && bStart == true && bEnd == false, "Status");
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            const string FileName = "delme.out";

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

            IStreamBufferSink isbc = (IStreamBufferSink)sbk;
            hr = isbc.LockProfile("delme.prf");
            DsError.ThrowExceptionForHR(hr);

            File.Delete(FileName);

            object o;
            hr = isbc.CreateRecorder("delme.out", RecordingType.Reference, out o);
            DsError.ThrowExceptionForHR(hr);

            m_sbrc = (IStreamBufferRecordControl)o;

            hr = ((IMediaControl)m_FilterGraph).Run();
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
