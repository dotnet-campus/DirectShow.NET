using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferRecordingAttributeTest
    {
        IStreamBufferRecordingAttribute m_sbra;
        IFilterGraph2 m_FilterGraph;

        public IStreamBufferRecordingAttributeTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestCount();
                TestIndex();
                TestName();
                TestSet();
                TestEnum();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sbra);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestEnum()
        {
            int hr;
            IEnumStreamBufferRecordingAttrib pEnum;

            hr = m_sbra.EnumAttributes(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum != null);

            hr = pEnum.Reset();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestSet()
        {
            int hr;
            StreamBufferAttrDataType adt;
            short Len = 4;
            IntPtr pba = Marshal.AllocCoTaskMem(Len);

            try
            {
                Marshal.WriteInt32(pba, 501);

                hr = m_sbra.SetAttribute(0, "DVR Index Granularity", StreamBufferAttrDataType.DWord, pba, Len);
                DsError.ThrowExceptionForHR(hr);

                Marshal.WriteInt32(pba, 0);

                hr = m_sbra.GetAttributeByName("DVR Index Granularity", 0, out adt, pba, ref Len);
                DsError.ThrowExceptionForHR(hr);

                int v = Marshal.ReadInt32(pba);
                Debug.Assert(v == 501, "pba");
            }
            finally
            {
                Marshal.FreeCoTaskMem(pba);
            }
        }

        private void TestName()
        {
            int hr;
            StreamBufferAttrDataType adt;
            short Len = 0;
            IntPtr pba = IntPtr.Zero;

            hr = m_sbra.GetAttributeByName("DVR Index Granularity", 0, out adt, pba, ref Len);
            DsError.ThrowExceptionForHR(hr);

            pba = Marshal.AllocCoTaskMem(Len);
            try
            {
                hr = m_sbra.GetAttributeByName("DVR Index Granularity", 0, out adt, pba, ref Len);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(adt == StreamBufferAttrDataType.DWord, "adt");

                int v = Marshal.ReadInt32(pba);
                Debug.Assert(v == 500, "pba");
            }
            finally
            {
                Marshal.FreeCoTaskMem(pba);
            }
        }

        private void TestIndex()
        {
            int hr;
            StringBuilder Name;
            short NameLen;
            StreamBufferAttrDataType adt;
            IntPtr pba;
            short Len;
            short Index = 23;

            Name = null;
            NameLen = 0;
            pba = IntPtr.Zero;
            Len = 0;

            hr = m_sbra.GetAttributeByIndex(Index, 0, Name, ref NameLen, out adt, pba, ref Len);
            DsError.ThrowExceptionForHR(hr);

            Name = new StringBuilder(NameLen);
            pba = Marshal.AllocCoTaskMem(Len);

            try
            {
                hr = m_sbra.GetAttributeByIndex(Index, 0, Name, ref NameLen, out adt, pba, ref Len);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(Name.ToString() == "DVR Index Granularity", "Name");
                Debug.Assert(adt == StreamBufferAttrDataType.DWord, "adt");

                int v = Marshal.ReadInt32(pba);
                Debug.Assert(v == 500, "pba");
            }
            finally
            {
                Marshal.FreeCoTaskMem(pba);
            }
        }

        private void TestCount()
        {
            int hr;
            short c;

            hr = m_sbra.GetAttributeCount(0, out c);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(c > 0, "Count");
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
            m_sbra = (IStreamBufferRecordingAttribute)o;
        }
    }
}
