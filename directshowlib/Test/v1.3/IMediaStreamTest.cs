using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.Test
{
	public class IMediaStreamTest
	{
        private IMediaStream m_ms;
        private IMediaStream m_ms2;
        private IAMMultiMediaStream m_mms;

		public IMediaStreamTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestGetInfo();
                TestAllocateSample();
                TestGetMMStream();
                TestSetSame();
                TestEOS();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ms);
            }
        }

        private void TestEOS()
        {
            int hr;

            // Not supposed to call this from apps
            hr = m_ms2.SendEndOfStream(0);
            Debug.Assert(hr == MsResults.E_InvalidStreamType, "SendEndOfStream");
        }

        private void TestAllocateSample()
        {
            int hr;
            IStreamSample pSample, pNewSample;

            hr = m_ms.AllocateSample(0, out pSample);
            MsError.ThrowExceptionForHR(hr);

            hr = m_ms.CreateSharedSample(pSample, 0, out pNewSample);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pSample != pNewSample && pNewSample != null, "CreateSharedSample");

            Marshal.ReleaseComObject(pSample);
            Marshal.ReleaseComObject(pNewSample);
        }

        private void TestSetSame()
        {
            int hr;

            hr = m_ms.SetSameFormat(m_ms2, 0);
            
            // The fact that they are incompatible means that it was
            // able to read the m_ms2 param, so that's good.
            Debug.Assert(hr == MsResults.E_Incompatible, "SetSameFormat");
        }

        private void TestGetMMStream()
        {
            int hr;
            IMultiMediaStream mms;

            hr = m_ms.GetMultiMediaStream(out mms);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(m_mms == mms, "GetMultiMediaStream");
        }

        private void TestGetInfo()
        {
            int hr;
            Guid pPurpose;
            StreamType pType;

            hr = m_ms.GetInformation(out pPurpose, out pType);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pType == StreamType.Read && pPurpose == MSPID.PrimaryAudio, "GetInformation");
        }

        private void Config()
        {
            int hr;
            m_mms = (IAMMultiMediaStream) new AMMultiMediaStream();

            hr = m_mms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, null);
            MsError.ThrowExceptionForHR(hr);

            hr = m_mms.AddMediaStream(null, MSPID.PrimaryVideo, AMMStream.None, null);
            MsError.ThrowExceptionForHR(hr);

            hr = m_mms.EnumMediaStreams(0, out m_ms);
            MsError.ThrowExceptionForHR(hr);

            hr = m_mms.EnumMediaStreams(1, out m_ms2);
            MsError.ThrowExceptionForHR(hr);
        }
	}
}
