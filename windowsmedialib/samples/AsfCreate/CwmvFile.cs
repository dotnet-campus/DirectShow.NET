// ---------------------------------------------------------------------
//  CwmvFile
//  A .NET class library for creating asf files from a collection of bitmaps
//  
//    Copyright (c) 2005 David Wohlferd
//    david@LimeGreenSocks.com
// ---------------------------------------------------------------------
// 
// This code is released "free of charge for personal, educational and commercial// use." I only ask that you give me credit for my work.
//
// In that light, the original outline of this code came from a C++ project
// by P.GopalaKrishna (http://www.codeproject.com/bitmap/createmovie.asp).
// 
// Things to know when using this class:
//
// - All the bitmaps must have the same attributes: Height, Width, Pixelformat
// - The Constructor is very slow (something about working with profiles)
// - All errors will throw exceptions.
// - The speed at which you call AppendNewFrame has NOTHING to do with
// the speed at which the video file will play.  So you can bang them in
// at full speed, or slowly as frames arrive over the network.  They will
// play at the speed specified by the framerate specified in the constructor.
// - Normal tv video is ~30fps, so if your video is jerky, perhaps you need
// to have a higher fps (and more bitmap files).
// - Configuration parameters such as output frame size, compression ratios (bitrate)
// and the like are controlled by specifying the appropriate profile.  There are
// tools available (elsewhere) for creating profiles.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WindowsMediaLib;
using WindowsMediaLib.Defs;  // Contains defs also found in DirectShowLib

namespace AsfCreate
{
    public class CwmvFile
    {
        #region Member variables

        // Interface used to write to asf file
        private IWMWriter           m_pWMWriter;

        // Used to read and set the video properties
        private IWMInputMediaProps  m_pInputProps;

        private int m_iFrameRate; // # of Frames Per Second for video output
        private int m_dwVideoInput; // Which channel of the asf writer to write to
        private int m_dwCurrentVideoSample; // Count of current frame
        private long m_msVideoTime; // Timestamp of current frame
        private bool m_Init; // Has init been run yet?

        #endregion

        #region APIs
        [DllImport("Kernel32.dll", EntryPoint="RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion

        /// <summary>
        ///  Create filename from specified profile using specified framerate
        /// </summary>
        /// <param name="lpszFileName">File name to create</param>
        /// <param name="guidProfileID">WM Profile to use for compression</param>
        /// <param name="iFrameRate">Frames Per Second</param>
        public CwmvFile(string lpszFileName, ref Guid guidProfileID, int iFrameRate)
        {
            int hr;
            Guid    guidInputType;
            int    dwInputCount = 0;

            IWMProfileManager pWMProfileManager = null;
            IWMProfile pWMProfile = null;

            // Initialize all member variables
            m_iFrameRate = iFrameRate;
            m_dwVideoInput = -1;
            m_dwCurrentVideoSample = 0;
            m_msVideoTime = 0;

            m_pWMWriter = null;
            m_pInputProps = null;
            m_Init = false;

            try
            {
                // Open the profile manager
                hr = WMUtils.WMCreateProfileManager(out pWMProfileManager);
                Marshal.ThrowExceptionForHR(hr);

                // Convert pWMProfileManager to a IWMProfileManager2
                IWMProfileManager2 pProfileManager2 = (IWMProfileManager2)pWMProfileManager;

                // Specify the version number of the profiles to use
                hr = pProfileManager2.SetSystemProfileVersion(WMVersion.V8_0);
                Marshal.ThrowExceptionForHR(hr);

                // Load the profile specified by the caller
                hr = pProfileManager2.LoadProfileByID(guidProfileID, out pWMProfile);
                Marshal.ThrowExceptionForHR(hr);

                // Create a writer.  This is the interface we actually write with
                hr = WMUtils.WMCreateWriter(IntPtr.Zero, out m_pWMWriter);
                Marshal.ThrowExceptionForHR(hr);

                // Set the profile we got into the writer.  This controls compression, video
                // size, # of video channels, # of audio channels, etc
                hr = m_pWMWriter.SetProfile(pWMProfile);
                Marshal.ThrowExceptionForHR(hr);

                // Find out how many inputs are in the current profile
                hr = m_pWMWriter.GetInputCount(out dwInputCount);
                Marshal.ThrowExceptionForHR(hr);

                // Assume we won't find any video pins
                m_dwVideoInput = -1;

                // Find the first video input on the writer
                for (int i=0; i < dwInputCount; i++)
                {
                    // Get the properties of channel #i
                    hr = m_pWMWriter.GetInputProps(i, out m_pInputProps);
                    Marshal.ThrowExceptionForHR(hr);

                    // Read the type of the channel
                    hr = m_pInputProps.GetType(out guidInputType);
                    Marshal.ThrowExceptionForHR(hr);

                    // If it is video, we are done
                    if (guidInputType == MediaType.Video)
                    {
                        m_dwVideoInput = i;
                        break;
                    }
                }

                // Didn't find a video channel
                if (m_dwVideoInput == -1)
                {
                    throw new Exception("Profile does not accept video input");
                }

                // Specify the file name for the output
                hr = m_pWMWriter.SetOutputFilename(lpszFileName);
                Marshal.ThrowExceptionForHR(hr);
            }
            catch
            {
                Close();
                throw;
            }
            finally
            {
                // Release the locals
                if (pWMProfile != null)
                {
                    Marshal.ReleaseComObject(pWMProfile);
                    pWMProfile = null;
                }
                if (pWMProfileManager != null)
                {
                    Marshal.ReleaseComObject(pWMProfileManager);
                    pWMProfileManager = null;
                }
            }
        }


        /// <summary>
        /// Destructor
        /// </summary>
        ~CwmvFile()
        {
            Close();
        }


        /// <summary>
        /// Close the output and release the variables
        /// </summary>
        public void Close()
        {
            if (m_Init)   //We are currently writing
            {
                if (m_pWMWriter != null)
                {
                    // Close the file
                    int hr = m_pWMWriter.EndWriting();
                }
                m_Init = false;
            }

            if (m_pInputProps != null)
            {
                Marshal.ReleaseComObject(m_pInputProps);
                m_pInputProps = null;
            }
            if (m_pWMWriter != null)
            {
                Marshal.ReleaseComObject(m_pWMWriter);
                m_pWMWriter = null;
            }
        }


        /// <summary>
        /// Add a frame to the output file
        /// </summary>
        /// <param name="hBitmap">Frame to add</param>
        public void AppendNewFrame(Bitmap hBitmap)
        {
            int             hr = 0;
            INSSBuffer      pSample = null;
            Rectangle       r = new Rectangle(0, 0, hBitmap.Width, hBitmap.Height);
            BitmapData      bmd;

            if (!m_Init)
            {
                // Only call this for the first frame
                Initialize(hBitmap);
            }

            // Lock the bitmap, which gets us a pointer to the raw bitmap data
            bmd = hBitmap.LockBits(r, ImageLockMode.ReadOnly, hBitmap.PixelFormat);

            try
            {
                // Compute size of bitmap in bytes.  Strides may be negative.
                int iSize = Math.Abs(bmd.Stride * bmd.Height);
                IntPtr ip;

                // Get a sample interface
                hr = m_pWMWriter.AllocateSample(iSize, out pSample);
                Marshal.ThrowExceptionForHR(hr);

                // Get the buffer from the sample interface.  This is
                // where we copy the bitmap data to
                hr = pSample.GetBuffer(out ip);
                Marshal.ThrowExceptionForHR(hr);

                // Copy the bitmap data into the sample buffer
                LoadSample(bmd, ip, iSize);

                // Write the sample to the output - Sometimes, for reasons I can't explain,
                // writing a sample fails.  However, writing the same sample again
                // works.  Go figure.
                int iRetry = 0;
                do
                {
                    hr = m_pWMWriter.WriteSample(m_dwVideoInput, 10000 * m_msVideoTime, WriteFlags.CleanPoint, pSample);
                } while (hr == NSResults.E_InvalidData && iRetry++ < 3);

                Marshal.ThrowExceptionForHR(hr);

                // update the time based on the framerate
                m_msVideoTime = (++m_dwCurrentVideoSample*1000)/m_iFrameRate;
            }
            finally
            {
                // Release the locals
                if (pSample != null)
                {
                    Marshal.ReleaseComObject(pSample);
                    pSample = null;
                }

                hBitmap.UnlockBits(bmd);
            }
        }


        /// <summary>
        /// Read the properties of the first bitmap to finish initializing the writer.
        /// </summary>
        /// <param name="hBitmap">First bitmap</param>
        private void Initialize(Bitmap hBitmap)
        {
            int hr;
            WMMediaType mt = new WMMediaType();
            VideoInfoHeader videoInfo = new VideoInfoHeader();

            // Create the VideoInfoHeader using info from the bitmap
            videoInfo.BmiHeader.Size = Marshal.SizeOf(typeof(BitmapInfoHeader));
            videoInfo.BmiHeader.Width = hBitmap.Width;
            videoInfo.BmiHeader.Height = hBitmap.Height;
            videoInfo.BmiHeader.Planes = 1;                

            // compression thru clrimportant don't seem to be used. Init them anyway
            videoInfo.BmiHeader.Compression = 0;
            videoInfo.BmiHeader.ImageSize = 0;
            videoInfo.BmiHeader.XPelsPerMeter = 0;
            videoInfo.BmiHeader.YPelsPerMeter = 0;
            videoInfo.BmiHeader.ClrUsed = 0;
            videoInfo.BmiHeader.ClrImportant = 0;

            switch(hBitmap.PixelFormat)
            {
                case PixelFormat.Format32bppRgb:
                    mt.subType = MediaSubType.RGB32;
                    videoInfo.BmiHeader.BitCount = 32;
                    break;
                case PixelFormat.Format24bppRgb:
                    mt.subType = MediaSubType.RGB24;
                    videoInfo.BmiHeader.BitCount = 24;
                    break;
                case PixelFormat.Format16bppRgb555:
                    mt.subType = MediaSubType.RGB555;
                    videoInfo.BmiHeader.BitCount = 16;
                    break;
                default:
                    throw new Exception("Unrecognized Pixelformat in bitmap");
            }

            videoInfo.SrcRect = new Rectangle(0, 0, hBitmap.Width, hBitmap.Height);
            videoInfo.TargetRect = videoInfo.SrcRect;
            videoInfo.BmiHeader.ImageSize = hBitmap.Width * hBitmap.Height * (videoInfo.BmiHeader.BitCount / 8);
            videoInfo.BitRate = videoInfo.BmiHeader.ImageSize * m_iFrameRate;
            videoInfo.BitErrorRate = 0;
            videoInfo.AvgTimePerFrame = 10000 * 1000 / m_iFrameRate;

            mt.majorType = MediaType.Video;
            mt.fixedSizeSamples = true;
            mt.temporalCompression = false;
            mt.sampleSize = videoInfo.BmiHeader.ImageSize;
            mt.formatType = FormatType.VideoInfo;
            mt.unkPtr = IntPtr.Zero;
            mt.formatSize = Marshal.SizeOf(typeof(VideoInfoHeader));

            // Lock the videoInfo structure, and put the pointer
            // into the mediatype structure
            GCHandle gHan = GCHandle.Alloc(videoInfo, GCHandleType.Pinned);

            try
            {
                // Set the inputprops using the structures
                mt.formatPtr = gHan.AddrOfPinnedObject();

                hr = m_pInputProps.SetMediaType(mt);
                Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                gHan.Free();
            }

            // Now take the inputprops, and set them on the file writer
            hr = m_pWMWriter.SetInputProps(m_dwVideoInput, m_pInputProps);
            Marshal.ThrowExceptionForHR(hr);

            // Done with config, prepare to write
            hr = m_pWMWriter.BeginWriting();
            Marshal.ThrowExceptionForHR(hr);

            m_Init = true;
        }


        /// <summary>
        /// Copy the bitmap data to the sample buffer
        /// </summary>
        /// <param name="bmd">Source bytes</param>
        /// <param name="ip">Point to copy the data to</param>
        /// <param name="iSize">How many bytes to copy</param>
        private void LoadSample(BitmapData bmd, IntPtr ip, int iSize)
        {
            // If the bitmap is rightside up
            if (bmd.Stride < 0)
            {
                CopyMemory(ip, bmd.Scan0, iSize);
            }
            else
            {
                // Copy it line by line from bottom to top
                IntPtr ip2 = (IntPtr)(ip.ToInt32() + iSize - bmd.Stride);
                for (int x=0; x < bmd.Height; x++)
                {
                    CopyMemory(ip2, (IntPtr)(bmd.Scan0.ToInt32() + (bmd.Stride * x)), bmd.Stride);
                    ip2 = (IntPtr)(ip2.ToInt32() - bmd.Stride);
                }
            }
        }
    }
}
