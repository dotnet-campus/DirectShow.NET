using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IEnumStreamBufferRecordingAttribTest
    {
        IEnumStreamBufferRecordingAttrib m_sbra;
        IFilterGraph2 m_FilterGraph;

        public IEnumStreamBufferRecordingAttribTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestNext();
                TestReset();
                TestSkip();
                TestClone();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sbra);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        public void TestNext()
        {
            int lFetched;
            IntPtr ip = Marshal.AllocCoTaskMem(4);
            int hr;
            StreamBufferAttribute[] pAttribs = new StreamBufferAttribute[2];

            hr = m_sbra.Next(2, pAttribs, ip);
            lFetched = Marshal.ReadInt32(ip);
            Marshal.FreeCoTaskMem(ip);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(lFetched == 2, "Next");

            Marshal.FreeCoTaskMem(pAttribs[0].pbAttribute);
            Marshal.FreeCoTaskMem(pAttribs[1].pbAttribute);
        }

        public void TestReset()
        {
            int hr;
            StreamBufferAttribute[] pAttribs = new StreamBufferAttribute[1];

            hr = m_sbra.Next(1, pAttribs, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(pAttribs[0].pbAttribute);

            hr = m_sbra.Reset();
            DsError.ThrowExceptionForHR(hr);

            hr = m_sbra.Next(1, pAttribs, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(pAttribs[0].pbAttribute);
        }

        public void TestSkip()
        {
            int hr;
            StreamBufferAttribute[] pAttribs = new StreamBufferAttribute[1];

            hr = m_sbra.Next(1, pAttribs, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(pAttribs[0].pbAttribute);

            hr = m_sbra.Skip(1);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sbra.Next(1, pAttribs, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(pAttribs[0].pbAttribute);
        }

        public void TestClone()
        {
            int hr;
            StreamBufferAttribute[] pAttribs = new StreamBufferAttribute[1];

            IEnumStreamBufferRecordingAttrib cloneEnum;
            hr = m_sbra.Clone(out cloneEnum);
            DsError.ThrowExceptionForHR(hr);

            hr = cloneEnum.Next(1, pAttribs, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(cloneEnum);
            Marshal.FreeCoTaskMem(pAttribs[0].pbAttribute);
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            const string FileName = "delme.out";

            int hr;
            IBaseFilter pFilter;
            object o;

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

            hr = isbc.CreateRecorder("delme.out", RecordingType.Content, out o);
            DsError.ThrowExceptionForHR(hr);

            // Make sure we really got a recorder object
            IStreamBufferRecordingAttribute sbra = (IStreamBufferRecordingAttribute)o;

            hr = sbra.EnumAttributes(out m_sbra);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
