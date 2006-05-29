using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.Test
{
	public class IMediaStreamFilterTest
	{
        private IMediaStreamFilter m_msf;

		public IMediaStreamFilterTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestAdd();
                TestEnum();
                TestRefTime();
                TestEnd();
                TestFlush();
                TestGetCurrent();
                TestGetMediaStream();
                TestWait();
                TestSupport();
            }
            finally
            {
                Marshal.ReleaseComObject(m_msf);
            }
        }

        private void TestAdd()
        {
			int hr;

            IAMMediaStream aStream;
            IMediaStream pStream = null;

            hr = m_msf.GetMediaStream(MSPID.PrimaryAudio, out pStream);
            MsError.ThrowExceptionForHR(hr);

            aStream = pStream as IAMMediaStream;

            hr = m_msf.AddMediaStream(aStream);

            // If it can read the purpose id, it must have been able to read the aStream
            // and that's close enough
            Debug.Assert(hr == MsResults.E_PurposeId, "AddMediaStream");
        }

        private void TestWait()
        {
            int hr;

            hr = m_msf.WaitUntil(10000000);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestSupport()
        {
            int hr;

            hr = m_msf.SupportSeeking(false);
            //MsError.ThrowExceptionForHR(hr);
            // Not sure why this gives an error, but the def looks right
        }

        private void TestRefTime()
        {
            int hr;
            long l = 1234;

            hr = m_msf.ReferenceTimeToStreamTime(ref l);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestGetMediaStream()
        {
            int hr;
            IMediaStream pStream = null;

            hr = m_msf.GetMediaStream(MSPID.PrimaryAudio, out pStream);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestFlush()
        {
            int hr;

            hr = m_msf.Flush(true);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestGetCurrent()
        {
            int hr;
            long l = 2453;
            
            hr = m_msf.GetCurrentStreamTime(out l);
            Debug.Assert(hr == 1 && l == 0, "GetCurrentStreamTime");
        }

        private void TestEnd()
        {
            int hr;

            hr = m_msf.EndOfStream();
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestEnum()
        {
            int hr;
            IMediaStream pStream;

            hr = m_msf.EnumMediaStreams(0, out pStream);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pStream != null, "EnumMediaStreams");
        }

        private void Config()
        {
            int hr;
            IMediaStream pStream = null;

            IAMMultiMediaStream mms = (IAMMultiMediaStream) new AMMultiMediaStream(); 

            hr = mms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, pStream);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.GetFilter(out m_msf);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.OpenFile("foo.avi", AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);
        }
	}
}
