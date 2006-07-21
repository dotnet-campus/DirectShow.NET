using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace v1._5
{
    public class IAudioDataTest
    {
        IAudioData m_iad;

        public void DoTests()
        {
            Setup();

            TestFormat();
        }

        private void TestFormat()
        {
            int hr;
            WaveFormatEx wf = new WaveFormatEx();
            WaveFormatEx wf2 = new WaveFormatEx();
            wf.cbSize = (short)Marshal.SizeOf(wf);

            hr = m_iad.GetFormat(wf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(wf.nBlockAlign == 2, "GetFormat");

            wf.nAvgBytesPerSec = wf.nAvgBytesPerSec * 2;

            hr = m_iad.SetFormat(wf);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iad.GetFormat(wf2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(wf2.nAvgBytesPerSec == wf.nAvgBytesPerSec, "SetFormat");
        }

        private void Setup()
        {
            m_iad = new AMAudioData() as IAudioData;
        }
    }
}
