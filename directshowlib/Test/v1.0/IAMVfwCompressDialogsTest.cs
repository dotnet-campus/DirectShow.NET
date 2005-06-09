using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	[TestFixture]
	public class IAMVfwCompressDialogsTest
	{
		private IAMVfwCompressDialogs m_ivcd;

		public IAMVfwCompressDialogsTest()
		{
		}

		[Test]
		public void DoTests()
		{
            // Get an audio device
			m_ivcd = GetCompressionFilter();

            try
            {
                //TestState();
                //TestSendMessage();
                TestDialog();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ivcd);
            }
		}

        void TestState()
        {
            int hr;
            int iSize = 0;
            IntPtr ip = IntPtr.Zero;

            hr = m_ivcd.GetState(ip, ref iSize);
            DsError.ThrowExceptionForHR(hr);

            ip = Marshal.AllocCoTaskMem(iSize);
            hr = m_ivcd.GetState(ip, ref iSize);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ivcd.SetState(ip, iSize);
        }

        void TestSendMessage()
        {
            int hr;
            IntPtr ip = IntPtr.Zero;
            int iSize = 56;

            ip = Marshal.AllocCoTaskMem(iSize);
            hr = m_ivcd.SendDriverMessage(0x5000, ip.ToInt32(), iSize);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestDialog()
        {
            int hr;

            hr = m_ivcd.ShowDialog(VfwCompressDialogs.QueryAbout, IntPtr.Zero);
            Debug.Assert(hr == 0);

            hr = m_ivcd.ShowDialog(VfwCompressDialogs.QueryConfig, IntPtr.Zero);
            Debug.Assert(hr == 0);

            hr = m_ivcd.ShowDialog(VfwCompressDialogs.About, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ivcd.ShowDialog(VfwCompressDialogs.Config, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
        }

       // Find an video compression filter
        IAMVfwCompressDialogs GetCompressionFilter()
        {
            DsDevice [] capDevices;
            IAMVfwCompressDialogs ifcd = null;
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

            ifcd = Marshal.BindToMoniker( s ) as IAMVfwCompressDialogs;

            return ifcd;
        }
#if false
                //Debug.Write(s);
                //Debug.Write(" ");
                //Debug.WriteLine(dev.Name);
                try
                {
                    ibf = Marshal.BindToMoniker( s ) as IBaseFilter;
                }
                catch
                {
                }


                if (ifcd != null)
                {
                    hr = ifcd.ShowDialog(VfwCompressDialogs.QueryConfig, IntPtr.Zero);
                    if (hr == 0)
                    {
                        int iSize = 0;

                        hr = ifcd.GetState(IntPtr.Zero, ref iSize);

                        if (iSize > 0)
                        {
                            Debug.Write(s);
                            Debug.Write(" ");
                            Debug.WriteLine(dev.Name);
                        }
                    }

            if (hr != 0)
            {
                throw new Exception("No video compressors support getting the config");
            }
#endif
	}
}