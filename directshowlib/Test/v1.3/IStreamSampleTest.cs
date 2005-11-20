using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.Test
{
	public class IStreamSampleTest
	{
        private IStreamSample m_ss;
		public IStreamSampleTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestGetMediaStream();
                TestMediaTimes();
                TestUpdate();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ss);
            }
        }

        private void TestGetMediaStream()
        {
            int hr;
            IMediaStream pms;

            hr = m_ss.GetMediaStream(out pms);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pms != null, "GetMediaStream");
        }

        private void TestMediaTimes()
        {
            int hr;
            long ls, le, lc;

            // Despite having initialized the stream as StreamType.Write, apparently
            // DS still sees it as RO.  Not sure how to work around this.
            hr = m_ss.SetSampleTimes(123, 456);
            //MsError.ThrowExceptionForHR(hr);

            hr = m_ss.GetSampleTimes(out ls, out le, out lc);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestUpdate()
        {
            int hr;

            hr = m_ss.Update(SSUpdate.Continuous, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            MsError.ThrowExceptionForHR(hr);

            hr = m_ss.CompletionStatus(CompletionStatusFlags.None, 1);
            MsError.ThrowExceptionForHR(hr);
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

            //AMMediaType pmt = new AMMediaType();
            //pmt.majorType = MediaType.Video;

            // hr = amts.SetFormat(pmt, 0);
            // MsError.ThrowExceptionForHR(hr);

            hr = mms.Initialize(StreamType.Write, AMMMultiStream.None, null);

            hr = mms.AddMediaStream(amts, null, AMMStream.None, ms2);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.EnumMediaStreams(0, out ms);
            MsError.ThrowExceptionForHR(hr);

            mts = ms as IAMMediaTypeStream;
            hr = mts.CreateSample(100, IntPtr.Zero, 0, null, out mtp);
            MsError.ThrowExceptionForHR(hr);

            m_ss = mtp as IStreamSample;

            hr = mms.SetState(StreamState.Run);
            MsError.ThrowExceptionForHR(hr);
        }
	}
}
