using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.Test
{
	public class IAMMediaTypeSampleTest
	{
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);
        private IAMMediaTypeSample m_ts;

		public IAMMediaTypeSampleTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestActualLength();
                TestMediaTime();
                TestMediaType();
                TestPointer();
                TestTime();
                TestDiscontinuity();
                TestPreroll();
                TestSync();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ts);
            }
        }

        private void TestMediaTime()
        {
            int hr;
            long ls, le;

            hr = m_ts.SetMediaTime(123, 432);
            Debug.Assert(hr == E_NOTIMPL);

            hr = m_ts.GetMediaTime(out ls, out le);
            Debug.Assert(hr == E_NOTIMPL);
        }

        private void TestMediaType()
        {
            int hr;
            AMMediaType mt2;
            AMMediaType mt = new AMMediaType();
            mt.majorType = MediaType.Timecode;

            hr = m_ts.SetMediaType(mt);
            MsError.ThrowExceptionForHR(hr);

            hr = m_ts.GetMediaType(out mt2);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(mt.majorType == mt2.majorType, "MediaType");
        }

        private void TestPointer()
        {
            const int iSize = 123;
            int hr;
            IntPtr ip = Marshal.AllocCoTaskMem(iSize);
            IntPtr ip2;
            int iNewSize;

            hr = m_ts.SetPointer(ip, iSize);
            MsError.ThrowExceptionForHR(hr);

            hr = m_ts.GetPointer(out ip2);
            MsError.ThrowExceptionForHR(hr);

            iNewSize = m_ts.GetSize();
            Debug.Assert(ip == ip2, "Pointer");
            Debug.Assert(iNewSize == iSize, "Pointer2");
        }

        private void TestTime()
        {
            int hr;
            long ls, le;

            hr = m_ts.SetTime(123, 345);
            MsError.ThrowExceptionForHR(hr);

            hr = m_ts.GetTime(out ls, out le);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(ls == 123 && le == 345, "Time");
        }

        private void TestDiscontinuity()
        {
            int hr;
            int iBefore, iAfter;

            hr = m_ts.SetDiscontinuity(false);
            MsError.ThrowExceptionForHR(hr);

            iBefore = m_ts.IsDiscontinuity();

            hr = m_ts.SetDiscontinuity(true);
            MsError.ThrowExceptionForHR(hr);

            iAfter = m_ts.IsDiscontinuity();

            Debug.Assert(iBefore != iAfter, "Discontinuity");
        }

        private void TestPreroll()
        {
            int hr;
            int iBefore, iAfter;

            hr = m_ts.SetPreroll(false);
            MsError.ThrowExceptionForHR(hr);

            iBefore = m_ts.IsPreroll();

            hr = m_ts.SetPreroll(true);
            MsError.ThrowExceptionForHR(hr);

            iAfter = m_ts.IsPreroll();
            Debug.Assert(iBefore != iAfter, "Preroll");
        }

        private void TestSync()
        {
            int hr;
            int iBefore, iAfter;

            hr = m_ts.SetSyncPoint(false);
            MsError.ThrowExceptionForHR(hr);

            iBefore = m_ts.IsSyncPoint();

            hr = m_ts.SetSyncPoint(true);
            MsError.ThrowExceptionForHR(hr);

            iAfter = m_ts.IsSyncPoint();

            Debug.Assert(iBefore != iAfter, "SyncPoint");
        }

        private void TestActualLength()
        {
            int hr;

            hr = m_ts.SetActualDataLength(99);
            MsError.ThrowExceptionForHR(hr);

            int i = m_ts.GetActualDataLength();
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 99, "ActualDataLength");
        }

        private void Config()
        {
            int hr;
            IMediaStream ms;
            IAMMediaTypeStream mts;
            IMediaStream ms2 = null;
            IAMMultiMediaStream mms = (IAMMultiMediaStream)new AMMultiMediaStream();
            IAMMediaTypeStream amts = (IAMMediaTypeStream) new AMMediaTypeStream();
            IAMMediaTypeSample mtp;

            hr = mms.Initialize(StreamType.Write, AMMMultiStream.None, null);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.AddMediaStream(amts, null, AMMStream.None, ms2);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.EnumMediaStreams(0, out ms);
            MsError.ThrowExceptionForHR(hr);

            mts = ms as IAMMediaTypeStream;
            hr = mts.CreateSample(100, IntPtr.Zero, 0, null, out mtp);
            MsError.ThrowExceptionForHR(hr);

            m_ts = mtp as IAMMediaTypeSample;

            hr = mms.SetState(StreamState.Run);
            MsError.ThrowExceptionForHR(hr);
        }
	}
}
