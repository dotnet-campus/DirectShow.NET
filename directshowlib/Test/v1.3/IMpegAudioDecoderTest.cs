using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IMpegAudioDecoderTest
    {
        IMpegAudioDecoder m_mad;

        public IMpegAudioDecoderTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestAudioFormat();
                TestFrequencyDivider();
                TestAccuracy();
                TestStereo();
                TestDecoderWordSize();
                TestIntegerDecode();
                TestDualMode();
            }
            finally
            {
                Marshal.ReleaseComObject(m_mad);
            }
        }

        private void TestAudioFormat()
        {
            int hr;
            MPEG1WaveFormat pFmt;

            // I'd feel better about this test if pFmt didn't come back
            // as all zeros
            hr = m_mad.get_AudioFormat(out pFmt);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestFrequencyDivider()
        {
            int hr;
            MPEGAudioDivider i;

            hr = m_mad.put_FrequencyDivider(MPEGAudioDivider.CDAudio);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mad.get_FrequencyDivider(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == MPEGAudioDivider.CDAudio, "FrequencyDivider");
        }

        private void TestAccuracy()
        {
            int hr;
            MPEGAudioAccuracy i;

            hr = m_mad.put_DecoderAccuracy(MPEGAudioAccuracy.Best);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mad.get_DecoderAccuracy(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == MPEGAudioAccuracy.Best, "Accuracy");
        }

        private void TestStereo()
        {
            int hr;
            MPEGAudioChannels i;

            hr = m_mad.put_Stereo(MPEGAudioChannels.Stereo);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mad.get_Stereo(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == MPEGAudioChannels.Stereo, "Stereo");
        }

        private void TestDecoderWordSize()
        {
            int hr;
            int i;

            hr = m_mad.put_DecoderWordSize(16);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mad.get_DecoderWordSize(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 16, "DecoderWordSize");
        }

        private void TestIntegerDecode()
        {
            int hr;
            int i;

            hr = m_mad.put_IntegerDecode(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mad.get_IntegerDecode(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 0, "IntegerDecode");
        }

        private void TestDualMode()
        {
            int hr;
            MPEGAudioDual i;

            hr = m_mad.put_DualMode(MPEGAudioDual.Merge);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mad.get_DualMode(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == MPEGAudioDual.Merge, "DualMode");
        }

        private void Configure()
        {
            m_mad = (IMpegAudioDecoder)new CMpegAudioCodec();
        }
    }
}
