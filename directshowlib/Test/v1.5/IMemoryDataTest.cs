using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace v1._5
{
    public class IMemoryDataTest
    {
        IMemoryData m_imd;

        public void DoTests()
        {
            Setup();

            TestGetInfo();
        }

        private void TestGetInfo()
        {
            int hr;
            int iLen, ia;
            IntPtr ip = IntPtr.Zero;

            hr = m_imd.SetActual(3);

            hr = m_imd.GetInfo(out iLen, ip, out ia);
        }

        private void Setup()
        {
            IAudioData m_iad = new AMAudioData() as IAudioData;
            m_imd = m_iad as IMemoryData;
        }
    }
}
