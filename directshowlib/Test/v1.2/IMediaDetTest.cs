using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
	public class IMediaDetTest
	{
        IMediaDet m_imd;

        public IMediaDetTest()
		{
		}

        public void DoTests()
        {
            Config();

            try
            {
                TestFileName();
                TestGetOStreams();
                TestStream();
                TestStreamType();
                TestStreamTypeB();
                TestStreamMediaType();
                TestStreamLen();
                TestGetBitmaps();
                TestSampleGrabber();
                TestEnter();
                TestFilter();
                TestFRate();
                TestWrite();
            }
            finally
            {
                Marshal.ReleaseComObject(m_imd);
            }
        }

        private void TestEnter()
        {
            int hr;

            hr = m_imd.EnterBitmapGrabMode(1.3);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestStreamMediaType()
        {
            int hr;
            AMMediaType pmt = new AMMediaType();

            hr = m_imd.get_StreamMediaType(pmt);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pmt.majorType == MediaType.Audio, "StreamMediaType");
        }

        private void TestSampleGrabber()
        {
            int hr;
            ISampleGrabber isamp;

            hr = m_imd.GetSampleGrabber(out isamp);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestStreamTypeB()
        {
            int hr;
            string s;

            hr = m_imd.get_StreamTypeB(out s);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s.ToUpper() == "{" + MediaType.Audio.ToString().ToUpper() + "}", "StreamTypeB");
        }

        private void TestGetOStreams()
        {
            int hr;
            int i;

            hr = m_imd.get_OutputStreams(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 2, "get_OutputStreams");
        }

        private void TestStreamType()
        {
            int hr;
            Guid g;

            hr = m_imd.get_StreamType(out g);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaType.Audio, "StreamType");
        }

        private void TestStreamLen()
        {
            int hr;
            double d;

            hr = m_imd.get_StreamLength(out d);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(d == 4.32, "StreamLen");
        }

        private void TestWrite()
        {
            int hr;

            System.IO.File.Delete("foo2.bmp");

            hr = m_imd.WriteBitmapBits(1.1, 320, 240, "foo2.bmp");
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(System.IO.File.Exists("foo2.bmp"), "WriteBitmapBits");
        }

        private void TestGetBitmaps()
        {
            int hr;
            int iSize;
            int iWidth = 320;
            int iHeight = 240;
            IntPtr ip = IntPtr.Zero;
            IntPtr ip2;
            BitmapInfoHeader bmh = new BitmapInfoHeader();

            hr = m_imd.GetBitmapBits(1.1, out iSize, ip, iWidth, iHeight);
            DsError.ThrowExceptionForHR(hr);

            ip = Marshal.AllocCoTaskMem(iSize);
            hr = m_imd.GetBitmapBits(1.1, out iSize, ip, iWidth, iHeight);
            DsError.ThrowExceptionForHR(hr);

            Marshal.PtrToStructure(ip, bmh);
            ip2 = (IntPtr)(ip.ToInt32() + bmh.Size);

            Bitmap b = new Bitmap(iWidth, iHeight, iWidth * (bmh.BitCount / 8), PixelFormat.Format24bppRgb, ip2);
            b.RotateFlip(RotateFlipType.Rotate180FlipX);
            b.Save("foo.bmp", ImageFormat.Bmp);

            Marshal.FreeCoTaskMem(ip);
        }

        private void TestFilter()
        {
            int hr;
            object o;
            IBaseFilter ibf;

            hr = m_imd.get_Filter(out o);
            DsError.ThrowExceptionForHR(hr);

            ibf = o as IBaseFilter;
            Debug.Assert(ibf != null, "get_Filter");

            hr = m_imd.put_Filter(ibf);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(ibf);
        }

        private void TestFRate()
        {
            int hr;
            double d;

            hr = m_imd.get_FrameRate(out d);
            DsError.ThrowExceptionForHR(hr);

            // Deal with the inaccuracy in floating point
            Debug.Assert( (int)(d * 100) == 2997, "FrameRate");
        }

        private void TestStream()
        {
            int hr;
            const int STREAM = 1;
            int i;

            hr = m_imd.put_CurrentStream(STREAM);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imd.get_CurrentStream(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == STREAM, "Stream");
        }

        private void TestFileName()
        {
            int hr;
            string sFileName;
            const string FILENAME = @"foo.avi";

            hr = m_imd.put_Filename(FILENAME);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imd.get_Filename(out sFileName);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sFileName == FILENAME, "FileName");
        }

        private void Config()
        {
            m_imd = (IMediaDet)new MediaDet();;
        }
	}
}
