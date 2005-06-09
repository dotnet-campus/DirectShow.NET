using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Text;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	[TestFixture]
	public class IAMVideoCompressionTest
	{
        private const int E_NOTIMPLEMENTED = unchecked((int)0x80004001);
		private IAMVideoCompression m_ivc;

		public IAMVideoCompressionTest()
		{
		}

		[Test]
		public void DoTests()
		{
            // Get an audio device
			m_ivc = GetCompressionFilter();

            try
            {
                TestKeyFrameRate();
                TestPFrames();
                TestQuality();
                TestWindow();
                TestOverride();
                TestGetInfo();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ivc);
            }
		}

        void TestKeyFrameRate()
        {
            int hr;
            int pkfr;

            hr = m_ivc.put_KeyFrameRate(10);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ivc.get_KeyFrameRate(out pkfr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pkfr == 10, "Get/Set KeyFrameRate");
        }

        void TestPFrames()
        {
            int hr;
            int pkfr;

            hr = m_ivc.put_PFramesPerKeyFrame(10);

            if (hr != E_NOTIMPLEMENTED)
            {
                DsError.ThrowExceptionForHR(hr);

                hr = m_ivc.get_PFramesPerKeyFrame(out pkfr);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pkfr == 10, "Get/Set PFramesPerKeyFrame");
            }
        }

        void TestQuality()
        {
            int hr;
            double pkfr;

            hr = m_ivc.put_Quality(.50);

            if (hr != E_NOTIMPLEMENTED)
            {
                DsError.ThrowExceptionForHR(hr);

                hr = m_ivc.get_Quality(out pkfr);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pkfr > .49 && pkfr < .51, "Get/Set Quality");
            }
        }

        void TestWindow()
        {
            int hr;
            long pkfr;

            hr = m_ivc.put_WindowSize(1010101);

            // None of the default windows compressors use this
            if (hr != E_NOTIMPLEMENTED)
            {
                DsError.ThrowExceptionForHR(hr);

                hr = m_ivc.get_WindowSize(out pkfr);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pkfr == 1010101, "Get/Set WindowSize");
            }
        }

        void TestOverride()
        {
            int hr;

            // None of the default windows compressors use this
            hr = m_ivc.OverrideKeyFrame(12);
            if (hr != E_NOTIMPLEMENTED)
            {
                DsError.ThrowExceptionForHR(hr);
            }

            // None of the default windows compressors use this
            hr = m_ivc.OverrideFrameSize(13, 22222);
            if (hr != E_NOTIMPLEMENTED)
            {
                DsError.ThrowExceptionForHR(hr);
            }
        }

        void TestGetInfo()
        {
            int hr;
            int vSize;
            int dSize;
            int  kfr, ppk;
            double dq;
            CompressionCaps pcap;
            StringBuilder Version = null;
            StringBuilder Description = null;

            hr = m_ivc.GetInfo(null, out vSize, null, out dSize, out kfr, out ppk, out dq, out pcap);

            if (vSize > 0)
            {
                Version = new StringBuilder(vSize / 2, vSize / 2);
            }

            if (dSize > 0)
            {
                Description = new StringBuilder(dSize / 2, dSize / 2);
            }

            hr = m_ivc.GetInfo(Version, out vSize, Description, out dSize, out kfr, out ppk, out dq, out pcap);
        }
            
       // Find an video compression filter
        IAMVideoCompression GetCompressionFilter()
        {
            DsDevice [] capDevices;
            IAMVideoCompression ifcd = null;
            IBaseFilter ibf;
            int x;
            string s = "";

            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat( FilterCategory.VideoCompressorCategory );
            if( capDevices.Length == 0 )
            {
                throw new Exception("No video compressors found!");
            }

            for (x=0; x < capDevices.Length; x++)
            {
                DsDevice dev = capDevices[x];

                dev.Mon.GetDisplayName(null, null, out s);

                if (dev.Name == "Intel Indeo® Video 4.5") //"Microsoft Video 1")
                {
                    break;
                }
            }

            if (x >= capDevices.Length)
            {
                throw new Exception("can't find the video 1 filter");
            }

            ibf = Marshal.BindToMoniker( s ) as IBaseFilter;

            IPin outpin = DsFindPin.ByDirection(ibf, PinDirection.Output, 0);

            ifcd = outpin as IAMVideoCompression;

            return ifcd;
        }
	}
}