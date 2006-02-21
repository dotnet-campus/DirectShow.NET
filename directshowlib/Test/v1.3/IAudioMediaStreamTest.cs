using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

// c r lp
// s r lp
// s   lp
// c   lp
// s r    
// c r
// s
// c

namespace DirectShowLib.Test
{
	public class IAudioMediaStreamTest
	{
        private IAudioMediaStream m_ams;

		public IAudioMediaStreamTest()
		{
        }

        public void DoTests()
        {
            Config2();

            try
            {
                TestCreate();
                Config();
                TestFormat();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ams);
            }
        }

        private void TestCreate()
        {
#if ALLOW_UNTESTED_INTERFACES
            int hr;

            IAudioStreamSample pSample = null;
            IAudioData pData = new foo() as IAudioData;

            hr = m_ams.CreateSample(pData, 0, out pSample);
            MsError.ThrowExceptionForHR(hr);
#endif
        }

        private void TestFormat()
        {
            int hr;
            WaveFormatEx pCurrent = new WaveFormatEx();

            hr = m_ams.GetFormat(pCurrent);
            MsError.ThrowExceptionForHR(hr);

            hr = m_ams.SetFormat(pCurrent);
            MsError.ThrowExceptionForHR(hr);
        }

        private void Config2()
        {
            int hr;
            IAMMultiMediaStream mms = (IAMMultiMediaStream)new AMMultiMediaStream();
            IMediaStream pStream = null;

            hr = mms.Initialize(StreamType.Read, AMMMultiStream.None, null);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, pStream);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.EnumMediaStreams(0, out pStream);
            MsError.ThrowExceptionForHR(hr);

            m_ams = pStream as IAudioMediaStream;
        }

        private void Config3()
        {
            m_ams = (IAudioMediaStream)new AMAudioStream();
        }

        private void Config()
        {
            int hr;

            IAMMultiMediaStream amms = (IAMMultiMediaStream)new AMMultiMediaStream();
            IMultiMediaStream mms = (IMultiMediaStream)amms;
            IMediaStream pStream = null;

            hr = amms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, pStream);
            MsError.ThrowExceptionForHR(hr);

            hr = amms.OpenFile("foo.avi", AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.EnumMediaStreams(0, out pStream);
            MsError.ThrowExceptionForHR(hr);

            m_ams = pStream as IAudioMediaStream;
        }
	}

#if ALLOW_UNTESTED_INTERFACES
    class foo : IAudioData
    {
        #region IAudioData Members

        public int SetActual(int cbDataValid)
        {
            // TODO:  Add foo.SetActual implementation
            return 0;
        }

        public int GetInfo(out int pdwLength, System.IntPtr ppbData, out int pcbActualData)
        {
            // TODO:  Add foo.GetInfo implementation
            pdwLength = 0;
            pcbActualData = 0;
            return 0;
        }

        public int SetBuffer(int cbSize, IntPtr pbData, int dwFlags)
        {
            // TODO:  Add foo.SetBuffer implementation
            return 0;
        }

        public int GetFormat(WaveFormatEx pWaveFormatCurrent)
        {
            //pWaveFormatCurrent = new WaveFormatEx();

            pWaveFormatCurrent.cbSize = 0;
            pWaveFormatCurrent.wFormatTag = 1;
            pWaveFormatCurrent.nChannels = 2;
            pWaveFormatCurrent.nSamplesPerSec = 64000;
            pWaveFormatCurrent.wBitsPerSample = 16;
            pWaveFormatCurrent.nBlockAlign = (short)(pWaveFormatCurrent.nChannels * (pWaveFormatCurrent.wBitsPerSample / 8));
            pWaveFormatCurrent.nAvgBytesPerSec = (pWaveFormatCurrent.nSamplesPerSec * pWaveFormatCurrent.nBlockAlign);

            return 0;
        }

        public int SetFormat(WaveFormatEx lpWaveFormat)
        {
            // TODO:  Add foo.SetFormat implementation
            return 0;
        }

        #endregion

    }
#endif


}
