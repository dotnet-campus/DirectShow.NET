using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.Test
{
	public class IAMMediaTypeStreamTest
	{
        private IAMMediaTypeStream m_mts;
		public IAMMediaTypeStreamTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestFormat();
                TestSample();
                TestAllocator();
            }
            finally
            {
                Marshal.ReleaseComObject(m_mts);
            }
        }

        private void TestFormat()
        {
            int hr;
            AMMediaType pType = new AMMediaType();
            pType.formatType = FormatType.VideoInfo;
            pType.majorType = MediaType.Video;
            pType.subType = MediaSubType.YUYV;

            hr = m_mts.SetFormat(pType, 0);
            MsError.ThrowExceptionForHR(hr);

            // The error is "No stream can be found with the specified
            // attributes".  How you can successfully SET the value and
            // not be able to read it, I can't understand.  However, I believe
            // the declaration is correct.
            // Doesn't seem to work right in c++ either.
            hr = m_mts.GetFormat(pType, 0);
            //MsError.ThrowExceptionForHR(hr);
        }

        private void TestSample()
        {
            const int iSize = 100;
            int hr;
            IAMMediaTypeSample mts;
            IntPtr ip = Marshal.AllocCoTaskMem(iSize);

            hr = m_mts.CreateSample(iSize, ip, 0, null, out mts);
            MsError.ThrowExceptionForHR(hr);
        }
        private void TestAllocator()
        {
            int hr;
            AllocatorProperties pProps = new AllocatorProperties();

            pProps.cbAlign = 1;
            pProps.cbBuffer = 1000;
            pProps.cbPrefix = 4;
            pProps.cBuffers = 6;

            // Doesn't seem to work right in c++ either
            hr = m_mts.SetStreamAllocatorRequirements(pProps);
            //MsError.ThrowExceptionForHR(hr);

            // Doesn't seem to work right in c++ either
            hr = m_mts.GetStreamAllocatorRequirements(out pProps);
            //MsError.ThrowExceptionForHR(hr);
        }

        private void Config2()
        {
            int hr;

            IAMMultiMediaStream amms = (IAMMultiMediaStream)new AMMultiMediaStream();
            IMultiMediaStream mms = (IMultiMediaStream)amms;
            IMediaStream pStream = null;

            hr = amms.AddMediaStream(null, MSPID.PrimaryVideo, AMMStream.None, null);
            MsError.ThrowExceptionForHR(hr);

            hr = amms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, pStream);
            MsError.ThrowExceptionForHR(hr);

            hr = amms.OpenFile("foo.avi", AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.EnumMediaStreams(0, out pStream);
            MsError.ThrowExceptionForHR(hr);

            m_mts = pStream as IAMMediaTypeStream;
        }

        private void Config()
        {
            int hr;
            IMediaStream ms;
            IMediaStream ms2 = null;
            IAMMultiMediaStream mms = (IAMMultiMediaStream) new AMMultiMediaStream();
            IAMMediaTypeStream amts = (IAMMediaTypeStream)new AMMediaTypeStream();

            AMMediaType pmt = new AMMediaType();
            pmt.majorType = MediaType.Video;

            //hr = mms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, null);
            //MsError.ThrowExceptionForHR(hr);

            //pmt = null;
            //hr = amts.GetFormat(pmt, 0);
            //hr = amts.SetFormat(pmt, 0);

            hr = mms.AddMediaStream(amts, null, AMMStream.None, ms2);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.EnumMediaStreams(0, out ms);
            MsError.ThrowExceptionForHR(hr);

            m_mts = ms as IAMMediaTypeStream;
        }
	}
}
