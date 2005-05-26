#region license

/* ====================================================================
 * The Apache Software License, Version 1.1
 *
 * Copyright (c) 2005 The Apache Software Foundation.  All rights
 * reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 *
 * 3. The end-user documentation included with the redistribution,
 *    if any, must include the following acknowledgment:
 *       "This product includes software developed by the
 *        Apache Software Foundation (http://www.apache.org/)."
 *    Alternately, this acknowledgment may appear in the software itself,
 *    if and wherever such third-party acknowledgments normally appear.
 *
 * 4. The names "Apache" and "Apache Software Foundation" must
 *    not be used to endorse or promote products derived from this
 *    software without prior written permission. For written
 *    permission, please contact apache@apache.org.
 *
 * 5. Products derived from this software may not be called "Apache",
 *    nor may "Apache" appear in their name, without prior written
 *    permission of the Apache Software Foundation.
 *
 * THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED.  IN NO EVENT SHALL THE APACHE SOFTWARE FOUNDATION OR
 * ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF
 * USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
 * OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 * ====================================================================
 *
 * This software consists of voluntary contributions made by many
 * individuals on behalf of the Apache Software Foundation.  For more
 * information on the Apache Software Foundation, please see
 * <http://www.apache.org/>.
 *
 * Portions of this software are based upon public domain software
 * originally written at the National Center for Supercomputing Applications,
 * University of Illinois, Urbana-Champaign.
 */

#endregion

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

namespace DirectShowLib

{
#if ALLOW_UNTESTED_CODE

    #region COM Class Objects

    [ComImport, Guid("62BE5D10-60EB-11d0-BD3B-00A0C911CE86")]
    public class CreateDevEnum
    {
    }


    [ComImport, Guid("e436ebb3-524f-11ce-9f53-0020af0ba770")]
    public class FilterGraph
    {
    }


    [ComImport, Guid("e436ebb8-524f-11ce-9f53-0020af0ba770")]
    public class FilterGraphNoThread
    {
    }


    [ComImport, Guid("BF87B6E1-8C27-11d0-B3F0-00AA003761C5")]
    public class CaptureGraphBuilder2
    {
    }


    [ComImport, Guid("FCC152B7-F372-11d0-8E00-00C04FD7C08B")]
    public class DvdGraphBuilder
    {
    }


    [ComImport, Guid("BF87B6E0-8C27-11d0-B3F0-00AA003761C5")]
    public class CaptureGraphBuilder
    {
    }


    [ComImport, Guid("FA8A68B2-C864-4ba2-AD53-D3876A87494B")]
    public class StreamBufferConfig
    {
    }


    [ComImport, Guid("D682C4BA-A90A-42fe-B9E1-03109849C423")]
    public class StreamBufferComposeRecording
    {
    }


    [ComImport, Guid("060AF76C-68DD-11d0-8FC1-00C04FD9189D")]
    public class SeekingPassThru
    {
    }


    [ComImport, Guid("CDA42200-BD88-11d0-BD4E-00A0C911CE86")]
    public class FilterMapper2
    {
    }


    [ComImport, Guid("1e651cc0-b199-11d0-8212-00c04fc32c45")]
    public class MemoryAllocator
    {
    }


    [ComImport, Guid("CDBD8D00-C193-11d0-BD4E-00A0C911CE86")]
    public class MediaPropertyBag
    {
    }


    [ComImport, Guid("f963c5cf-a659-4a93-9638-caf3cd277d13")]
    public class DVDState
    {
    }


    [ComImport, Guid("94297043-bd82-4dfd-b0de-8177739c6d20")]
    public class DMOWrapperFilter
    {
    }


    #endregion

    #region Filter Classes
    [ComImport, Guid("2DB47AE5-CF39-43c2-B4D6-0CD8D90946F4")]
    public class StreamBufferSink
    {
    }


    [ComImport, Guid("C1F400A0-3F08-11d3-9F0B-006008039E37")]
    public class SampleGrabber
    {
    }


    [ComImport, Guid("C9F5FE02-F851-4eb5-99EE-AD602AF1E619")]
    public class StreamBufferSource
    {
    }


    [ComImport, Guid("B87BEB7B-8D29-423f-AE4D-6582C10175AC")]
    public class VideoMixingRenderer
    {
    }


    [ComImport, Guid("51b4abf3-748f-4e3b-a276-c828330e926a")]
    public class VideoMixingRenderer9
    {
    }


    [ComImport, Guid("6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50")]
    public class VideoRendererDefault
    {
    }


    [ComImport, Guid("1b544c20-fd0b-11ce-8c63-00aa0044b51e")]
    public class AviSplitter
    {
    }


    [ComImport, Guid("CC58E280-8AA1-11d1-B3F1-00AA003761C5")]
    public class SmartTee
    {
    }


    [ComImport, Guid("C1F400A4-3F08-11d3-9F0B-006008039E37")]
    public class NullRenderer
    {
    }


    [ComImport, Guid("6a08cf80-0e18-11cf-a24d-0020afd79767")]
    public class ACMWrapper
    {
    }


    [ComImport, Guid("e30629d1-27e5-11ce-875d-00608cb78066")]
    public class AudioRender
    {
    }


    [ComImport, Guid("CF49D4E0-1115-11ce-B03A-0020AF0BA770")]
    public class AVIDec
    {
    }


    [ComImport, Guid("A888DF60-1E90-11cf-AC98-00AA004C0FA9")]
    public class AVIDraw
    {
    }


    [ComImport, Guid("E2510970-F137-11CE-8B67-00AA00A3F1A6")]
    public class AviDest
    {
    }


    [ComImport, Guid("0DAD2FDD-5FD7-11D3-8F50-00C04F7971E2")]
    public class ATSCNetworkProvider
    {
    }


    [ComImport, Guid("DC0C0FE7-0485-4266-B93F-68FBF80ED834")]
    public class DVBCNetworkProvider
    {
    }


    [ComImport, Guid("FA4B375A-45B4-4d45-8440-263957B11623")]
    public class DVBSNetworkProvider
    {
    }


    [ComImport, Guid("216C62DF-6D7F-4e9a-8571-05F14EDB766A")]
    public class DVBTNetworkProvider
    {
    }


    [ComImport, Guid("1643e180-90f5-11ce-97d5-00aa0055595a")]
    public class Colour
    {
    }


    [ComImport, Guid("79376820-07D0-11cf-A24D-0020AFD79767")]
    public class DSoundRender
    {
    }


    [ComImport, Guid("129D7E40-C10D-11d0-AFB9-00AA00B67A42")]
    public class DVMux
    {
    }


    [ComImport, Guid("4EB31670-9FC6-11cf-AF6E-00AA00B67A42")]
    public class DVSplitter
    {
    }


    [ComImport, Guid("B1B77C00-C3E4-11cf-AF79-00AA00B67A42")]
    public class DVVideoCodec
    {
    }


    [ComImport, Guid("13AA3650-BB6F-11d0-AFB9-00AA00B67A42")]
    public class DVVideoEnc
    {
    }


    [ComImport, Guid("9B8C4620-2C1A-11d0-8493-00A02438AD48")]
    public class DVDNavigator
    {
    }


    [ComImport, Guid("e436ebb5-524f-11ce-9f53-0020af0ba770")]
    public class AsyncReader
    {
    }


    [ComImport, Guid("e436ebb6-524f-11ce-9f53-0020af0ba770")]
    public class URLReader
    {
    }


    [ComImport, Guid("8596E5F0-0DA5-11d0-BD21-00A0C911CE86")]
    public class FileWriter
    {
    }


    [ComImport, Guid("07167665-5011-11cf-BF33-00AA0055595A")]
    public class ModexRenderer
    {
    }


    [ComImport, Guid("F8388A40-D5BB-11d0-BE5A-0080C706568E")]
    public class InfTee
    {
    }


    [ComImport, Guid("6E8D4A20-310C-11d0-B79A-00AA003767A7")]
    public class Line21Decoder
    {
    }


    [ComImport, Guid("E4206432-01A1-4BEE-B3E1-3702C8EDC574")]
    public class Line21Decoder2
    {
    }


    [ComImport, Guid("07b65360-c445-11ce-afde-00aa006c14f4")]
    public class AVIMIDIRender
    {
    }


    [ComImport, Guid("B80AB0A0-7416-11d2-9EEB-006008039E37")]
    public class MJPGEnc
    {
    }


    [ComImport, Guid("301056D0-6DFF-11d2-9EEB-006008039E37")]
    public class MjpegDec
    {
    }


    [ComImport, Guid("4a2286e0-7bef-11ce-9bd9-0000e202599c")]
    public class CMpegAudioCodec
    {
    }


    [ComImport, Guid("336475d0-942a-11ce-a870-00aa002feab5")]
    public class MPEG1Splitter
    {
    }


    [ComImport, Guid("feb50740-7bef-11ce-9bd9-0000e202599c")]
    public class CMpegVideoCodec
    {
    }


    [ComImport, Guid("afb6c280-2c41-11d3-8a60-0000f81e0e4a")]
    public class MPEG2Demultiplexer
    {
    }


    [ComImport, Guid("3ae86b20-7be8-11d1-abe6-00a0c905f375")]
    public class MMSPLITTER
    {
    }


    [ComImport, Guid("CD8743A1-3736-11d0-9E69-00C04FD7C15B")]
    public class OverlayMixer
    {
    }


    [ComImport, Guid("FDFE9681-74A3-11d0-AFA7-00AA00B67A42")]
    public class QTDec
    {
    }


    [ComImport, Guid("D51BD5A0-7548-11cf-A520-0080C77EF58A")]
    public class QuickTimeParser
    {
    }


    [ComImport, Guid("71F96463-78F3-11d0-A18C-00A0C9118956")]
    public class TVAudioFilterPropertyPage
    {
    }


    [ComImport, Guid("266EEE41-6C63-11cf-8A03-00AA006ECB65")]
    public class TVTunerFilterPropertyPage
    {
    }


    [ComImport, Guid("814B9800-1C88-11d1-BAD9-00609744111A")]
    public class VBISurfaces
    {
    }


    [ComImport, Guid("1b544c22-fd0b-11ce-8c63-00aa0044b51e")]
    public class VfwCapture
    {
    }


    [ComImport, Guid("1da08500-9edc-11cf-bc10-00aa00ac74f6")]
    public class Dither
    {
    }


    [ComImport, Guid("6f26a6cd-967b-47fd-874a-7aed2c9d25a2")]
    public class VideoPortManager
    {
    }


    [ComImport, Guid("70e102b0-5556-11ce-97c0-00aa0055595a")]
    public class VideoRenderer
    {
    }


    [ComImport, Guid("187463A0-5BB7-11d3-ACBE-0080C75E246E")]
    public class WMAsfReader
    {
    }

    [ComImport, Guid("e436ebb1-524f-11ce-9f53-0020af0ba770")]
    public class SystemClock
    {
    }

    [ComImport, Guid("7c23220e-55bb-11d3-8b16-00c04fb6bd3d")]
    public class WMAsfWriter
    {
    }


    [ComImport, Guid("70BC06E0-5666-11d3-A184-00105AEF9F33")]
    public class WSTDecoder
    {
    }


    [ComImport, Guid("6CFAD761-735D-4aa5-8AFC-AF91A7D61EBA")]
    public class Mpeg2VideoStreamAnalyzer
    {
    }


    #endregion

    #region Declarations

	[StructLayout(LayoutKind.Sequential, Pack=2)]
	public struct BitmapInfoHeader
	{
		public int Size;
		public int Width;
		public int Height;
		public short Planes;
		public short BitCount;
		public int Compression;
		public int ImageSize;
		public int XPelsPerMeter;
		public int YPelsPerMeter;
		public int ClrUsed;
		public int ClrImportant;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct DDPixelFormat
	{
		[FieldOffset(0)] public int dwSize;
		[FieldOffset(4)] public int dwFlags;
		[FieldOffset(8)] public int dwFourCC;

		[FieldOffset(12)] public int dwRGBBitCount;
		[FieldOffset(12)] public int dwYUVBitCount;
		[FieldOffset(12)] public int dwZBufferBitDepth;
		[FieldOffset(12)] public int dwAlphaBitDepth;

		[FieldOffset(16)] public int dwRBitMask;
		[FieldOffset(16)] public int dwYBitMask;

		[FieldOffset(20)] public int dwGBitMask;
		[FieldOffset(20)] public int dwUBitMask;

		[FieldOffset(24)] public int dwBBitMask;
		[FieldOffset(24)] public int dwVBitMask;

		[FieldOffset(28)] public int dwRGBAlphaBitMask;
		[FieldOffset(28)] public int dwYUVAlphaBitMask;
		[FieldOffset(28)] public int dwRGBZBitMask;
		[FieldOffset(28)] public int dwYUVZBitMask;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DsCAUUID // CAUUID
	{
		public int cElems;
		public IntPtr pElems;

    /// <summary>
    /// Perform a manual marshaling of pElems to retrieve an array of System.Guid.
    /// Assume this structure has been already filled by the ISpecifyPropertyPages.GetPages() method.
    /// </summary>
    /// <returns>A managed representation of pElems</returns>
    public Guid[] ToGuidArray()
    {
      Guid[] retval;

      if (this.cElems == 0)
        return null;
    
      retval = new Guid[this.cElems];
      for (int i=0; i<this.cElems; i++)
      {
        // In 32Bits OSs IntPtr constructor cast Int64 as Int32. 
        // It should work on 32Bits and 64 Bits OSs...
        IntPtr ptr = new IntPtr(this.pElems.ToInt64() + (IntPtr.Size * i));
        retval[i] = (Guid) Marshal.PtrToStructure(ptr, typeof(Guid));
      }
      return retval;
    }
	}

	[StructLayout(LayoutKind.Sequential)]
	public class DsOptInt64
	{
		public DsOptInt64(long Value)
		{
			this.Value = Value;
		}

		public long Value;
	}

    [StructLayout(LayoutKind.Explicit)]
    public class GUID
    {
        [FieldOffset(0)]
        private Guid guid;

        public static readonly GUID Empty;

        static GUID()
        {
            GUID.Empty = new GUID();
        }

        public GUID()
        {
            this.guid = Guid.Empty;
        }

        public GUID(string g)
        {
            this.guid = new Guid(g);
        }

        public GUID(Guid g)
        {
            this.guid = g;
        }

        public override string ToString()
        {
            return this.guid.ToString();
        }

        public string ToString(string format)
        {
            return this.guid.ToString(format);
        }

        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        public static implicit operator Guid(GUID g)
        {
            return g.guid;
        }

        public static implicit operator GUID(Guid g)
        {
            return new GUID(g);
        }
    }

    #endregion

	#region Utility Classes

    public class DsError
    {
        [DllImport("quartz.dll", CharSet=CharSet.Auto)]
        public static extern int AMGetErrorText(int hr, StringBuilder buf, int max);

        /// <summary>
        /// If hr has a "failed" status code (E_*), throw an exception.  Note that status
        /// messages (S_*) are not considered failure codes.  If DirectShow error text
        /// is available, it is used to build the exception, otherwise a generic com error
        /// is thrown.
        /// </summary>
        /// <param name="hr">The HRESULT to check</param>
        public static void ThrowExceptionForHR(int hr)
        {
            const int MAX_ERROR_TEXT_LEN = 160;

            // If a severe error has occurred
            if (hr < 0)
            {
                // Make a buffer to hold the string
                StringBuilder buf = new StringBuilder(MAX_ERROR_TEXT_LEN, MAX_ERROR_TEXT_LEN);

                // If a string is returned, build a com error from it
                if (AMGetErrorText(hr, buf, MAX_ERROR_TEXT_LEN) > 0)
                {
                    throw new COMException(buf.ToString(), hr);
                }
                else
                {
                    // No string, just use standard com error
                    Marshal.ThrowExceptionForHR(hr);
                }
            }
        }

    }

	public class DsUtils
	{
        private DsUtils()
        {
            // Prevent people from trying to instantiate this class
        }

        /// <summary>
        /// Returns the PinCategory of the specified pin.  Usually a member of PinCategory.  Not all pins have a category.
        /// </summary>
        /// <param name="pPin"></param>
        /// <returns>Guid indicating pin category or Guid.Empty on no category.  Usually a member of PinCategory</returns>
		public static Guid GetPinCategory(IPin pPin)
		{
			Guid guidRet = Guid.Empty;

            // Memory to hold the returned guid
			int iSize = Marshal.SizeOf(typeof (Guid));
			IntPtr ipOut = Marshal.AllocCoTaskMem(iSize);

			try
			{
				int hr;
				int cbBytes;
				Guid g = PropSetID.Pin;

                // Get an IKsPropertySet from the pin
				IKsPropertySet pKs = pPin as IKsPropertySet;

                if (pKs != null)
                {
                    // Query for the Category
                    hr = pKs.Get(ref g, AMPropertyPin.Category, IntPtr.Zero, 0, ipOut, iSize, out cbBytes);
                    DsError.ThrowExceptionForHR(hr);

                    // Marshal it to the return variable
                    guidRet = (Guid) Marshal.PtrToStructure(ipOut, typeof (Guid));
                }
			}
			finally
			{
				Marshal.FreeCoTaskMem(ipOut);
                ipOut = IntPtr.Zero;
			}

			return guidRet;
		}

		/// <summary>
		///  Free the nested structures and release any
		///  COM objects within an AMMediaType struct.
		/// </summary>
		public static void FreeAMMediaType(AMMediaType mediaType)
		{
			if (mediaType != null)
			{
				if (mediaType.formatSize != 0)
				{
					Marshal.FreeCoTaskMem(mediaType.formatPtr);
					mediaType.formatSize = 0;
					mediaType.formatPtr = IntPtr.Zero;
				}
				if (mediaType.unkPtr != IntPtr.Zero)
				{
					Marshal.Release(mediaType.unkPtr);
					mediaType.unkPtr = IntPtr.Zero;
				}
			}
		}

		/// <summary>
		///  Free the nested interfaces within a PinInfo struct.
		/// </summary>
		public static void FreePinInfo(PinInfo pinInfo)
		{
            if (pinInfo.filter != null)
            {
                Marshal.ReleaseComObject(pinInfo.filter);
                pinInfo.filter = null;
            }
		}

	}


    public class DsROTEntry : IDisposable
    {
        [Flags]
        private enum ROTFlags
        {
            RegistrationKeepsAlive = 0x1,
            AllowAnyClient = 0x2
        }

        private int m_cookie = 0;

        #region APIs
        [DllImport("ole32.dll", ExactSpelling=true)]
        private static extern int GetRunningObjectTable(int r,
            out UCOMIRunningObjectTable pprot);

        [DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
        private static extern int CreateItemMoniker(string delim,
            string item, out UCOMIMoniker ppmk);

        [DllImport("kernel32.dll", ExactSpelling=true)]
        private static extern int GetCurrentProcessId();
        #endregion

        public DsROTEntry(IFilterGraph graph)
        {
            int hr = 0;
            UCOMIRunningObjectTable rot = null;
            UCOMIMoniker mk = null;
            try
            {
                // First, get a pointer to the running object table
                hr = GetRunningObjectTable(0, out rot);
                DsError.ThrowExceptionForHR(hr);

                // Build up the object to add to the table
                int id = GetCurrentProcessId();
                IntPtr iuPtr = Marshal.GetIUnknownForObject(graph);
                int iuInt = (int) iuPtr;
                Marshal.Release(iuPtr);
                string item = string.Format("FilterGraph {0} pid {1}", iuInt.ToString("x8"), id.ToString("x8"));
                hr = CreateItemMoniker("!", item, out mk);
                DsError.ThrowExceptionForHR(hr);

                // Add the object to the table
                rot.Register((int)ROTFlags.RegistrationKeepsAlive, graph, mk, out m_cookie);
            }
            finally
            {
                if (mk != null)
                {
                    Marshal.ReleaseComObject(mk);
                    mk = null;
                }
                if (rot != null)
                {
                    Marshal.ReleaseComObject(rot);
                    rot = null;
                }
            }
        }

        ~DsROTEntry()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (m_cookie != 0)
            {
                UCOMIRunningObjectTable rot = null;
                try
                {
                    // Get a pointer to the running object table
                    int hr = GetRunningObjectTable(0, out rot);
                    DsError.ThrowExceptionForHR(hr);

                    // Remove our entry
                    rot.Revoke(m_cookie);
                    m_cookie = 0;
                }
                finally
                {
                    if (rot != null)
                    {
                        Marshal.ReleaseComObject(rot);
                        rot = null;
                    }
                }
            }
        }
    }


	public class DsDevice : IDisposable
	{
		private UCOMIMoniker m_Mon;
        private string m_Name;

        public DsDevice(UCOMIMoniker Mon)
        {
            m_Mon = Mon;
            m_Name = null;
        }

        public UCOMIMoniker Mon
        {
            get
            {
                return m_Mon;
            }
        }

        public string Name
        {
            get
            {
                if (m_Name == null)
                {
                    m_Name = GetFriendlyName();
                }
                return m_Name;
            }
        }

        /// <summary>
        /// Returns an array of DsDevices of type devcat.
        /// </summary>
        /// <param name="cat">Any one of FilterCategory</param>
        /// <param name="devs">An array of zero or more devices</param>
        public static void GetDevicesOfCat(Guid devcat, out ArrayList devs)
        {
            devs = new ArrayList();
            int hr;
            ICreateDevEnum enumDev = null;
            UCOMIEnumMoniker enumMon = null;
            UCOMIMoniker[] mon = new UCOMIMoniker[1];
            try
            {
                enumDev = (ICreateDevEnum) new CreateDevEnum();
                hr = enumDev.CreateClassEnumerator(devcat, out enumMon, 0);
                DsError.ThrowExceptionForHR(hr);

                // CreateClassEnumerator returns null for enumMon if there are no entries
                if (hr != 1)
                {
                    int lFetched;
                    while ((enumMon.Next(1, mon, out lFetched) == 0))
                    {
                        devs.Add(new DsDevice(mon[0]));
                        mon[0] = null;
                    }
                }
            }
            catch (Exception)
            {
                foreach (DsDevice d in devs)
                {
                    d.Dispose();
                }
                throw;
            }
            finally
            {
                enumDev = null;
                if (mon[0] != null)
                {
                    Marshal.ReleaseComObject(mon[0]);
                    mon[0] = null;
                }
                if (enumMon != null)
                {
                    Marshal.ReleaseComObject(enumMon);
                    enumMon = null;
                }
            }
        }

        /// <summary>
        /// Get the FriendlyName for a moniker
        /// </summary>
        /// <returns>String or null on error</returns>
        private string GetFriendlyName()
        {
            IPropertyBag bag = null;
            string ret = null;
            object bagObj = null;
            object val = null;

            try
            {
                Guid bagId = typeof (IPropertyBag).GUID;
                m_Mon.BindToStorage(null, null, ref bagId, out bagObj);

                bag = (IPropertyBag) bagObj;

                int hr = bag.Read("FriendlyName", ref val, IntPtr.Zero);
                DsError.ThrowExceptionForHR(hr);

                ret = val as string;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                bag = null;
                if (bagObj != null)
                {
                    Marshal.ReleaseComObject(bagObj);
                    bagObj = null;
                }
            }

            return ret;
        }

        public void Dispose()
		{
			if (Mon != null)
			{
				Marshal.ReleaseComObject(Mon);
				m_Mon = null;
			}
            m_Name = null;
		}
	}


    public class DsFindPin
	{
        private DsFindPin()
        {
            // Prevent people from trying to instantiate this class
        }

        /// <summary>
        /// Scans a filter's pins looking for a pin in the specified direction
        /// </summary>
        /// <param name="vSource">The filter to scan</param>
        /// <param name="vDir">The direction to find</param>
        /// <param name="iIndex">Zero based index (ie 2 will return the third pin in the specified direction)</param>
        /// <returns>The matching pin, or null if not found</returns>
		public static IPin ByDirection(IBaseFilter vSource, PinDirection vDir, int iIndex)
		{
			int hr;
			int lFetched;
			IEnumPins ppEnum;
			PinDirection ppindir;
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

            // Get the pin enumerator
			hr = vSource.EnumPins(out ppEnum);
			DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
                {
                    // Read the direction
                    hr = pPins[0].QueryDirection(out ppindir);
                    DsError.ThrowExceptionForHR(hr);

                    // Is it the right direction?
                    if (ppindir == vDir)
                    {
                        // Is is the right index?
                        if (iIndex == 0)
                        {
                            pRet = pPins[0];
                            break;
                        }
                        iIndex--;
                    }
                    Marshal.ReleaseComObject(pPins[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(ppEnum);
            }

			return pRet;
		}

        /// <summary>
        /// Scans a filter's pins looking for a pin with the specified name
        /// </summary>
        /// <param name="vSource">The filter to scan</param>
        /// <param name="vPinName">The pin name to find</param>
        /// <returns>The matching pin, or null if not found</returns>
        public static IPin ByName(IBaseFilter vSource, string vPinName)
		{
			int hr;
			int lFetched;
			IEnumPins ppEnum;
			PinInfo ppinfo;
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

            // Get the pin enumerator
            hr = vSource.EnumPins(out ppEnum);
			DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
                {
                    // Read the info
                    hr = pPins[0].QueryPinInfo(out ppinfo);
                    DsError.ThrowExceptionForHR(hr);

                    // Is it the right name?
                    if (ppinfo.name == vPinName)
                    {
                        DsUtils.FreePinInfo(ppinfo);
                        pRet = pPins[0];
                        break;
                    }
                    Marshal.ReleaseComObject(pPins[0]);
                    DsUtils.FreePinInfo(ppinfo);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(ppEnum);
            }

			return pRet;
		}

        /// <summary>
        /// Scan's a filter's pins looking for a pin with the specified category
        /// </summary>
        /// <param name="vSource">The filter to scan</param>
        /// <param name="guidPinCat">The guid from PinCategory to scan for</param>
        /// <param name="iIndex">Zero based index (ie 2 will return the third pin of the specified category)</param>
        /// <returns>The matching pin, or null if not found</returns>
        public static IPin ByCategory(IBaseFilter vSource, Guid guidPinCat, int iIndex)
		{
			int hr;
			int lFetched;
			IEnumPins ppEnum;
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

            // Get the pin enumerator
            hr = vSource.EnumPins(out ppEnum);
			DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
                {
                    // Is it the right category?
                    if (DsUtils.GetPinCategory(pPins[0]) == guidPinCat)
                    {
                        // Is is the right index?
                        if (iIndex == 0)
                        {
                            pRet = pPins[0];
                            break;
                        }
                        iIndex--;
                    }
                    Marshal.ReleaseComObject(pPins[0]);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(ppEnum);
            }

			return pRet;
		}
	}


    public class DsToString
    {
        private DsToString()
        {
            // Prevent people from trying to instantiate this class
        }

        /// <summary>
        /// Produces a usable string that describes the MediaType object
        /// </summary>
        /// <returns>Concatenation of MajorType + SubType + FormatType + Fixed + Temporal + SampleSize.ToString</returns>
        public static string AMMediaTypeToString(ref AMMediaType pmt)
        {
            return string.Format("{0} {1} {2} {3} {4} {5}",
                MediaTypeToString(pmt.majorType),
                MediaSubTypeToString(pmt.subType),
                MediaFormatTypeToString(pmt.formatType),
                (pmt.fixedSizeSamples ? "FixedSamples" : "NotFixedSamples"),
                (pmt.temporalCompression ? "temporalCompression" : "NottemporalCompression"),
                pmt.sampleSize.ToString());
        }

        /// <summary>
        /// Converts AMMediaType.MajorType Guid to a readable string
        /// </summary>
        /// <returns>MajorType Guid as a readable string or Guid if unrecognized</returns>
        public static string MediaTypeToString(Guid guid)
        {
            // Walk the MediaSubType class looking for a match
            return WalkClass(typeof(MediaType), guid);
        }

        /// <summary>
        /// Converts the AMMediaType.SubType Guid to a readable string
        /// </summary>
        /// <returns>SubType Guid as a readable string or Guid if unrecognized</returns>
        public static string MediaSubTypeToString(Guid guid)
        {
            // Walk the MediaSubType class looking for a match
            return WalkClass(typeof(MediaSubType), guid);
        }

        /// <summary>
        /// Converts the AMMediaType.FormatType Guid to a readable string
        /// </summary>
        /// <returns>FormatType Guid as a readable string or Guid if unrecognized</returns>
        public static string MediaFormatTypeToString(Guid guid)
        {
            // Walk the FormatType class looking for a match
            return WalkClass(typeof(FormatType), guid);

        }

        /// <summary>
        /// Use reflection to walk a class looking for a property containing a specified guid
        /// </summary>
        /// <param name="MyType">Class to scan</param>
        /// <param name="guid">Guid to scan for</param>
        /// <returns>String representing property name that matches, or Guid.ToString() for no match</returns>
        private static string WalkClass(Type MyType, Guid guid)
        {
            object o = null;
       
            // Read the fields from the class
            FieldInfo[] Fields = MyType.GetFields();

            // Walk the returned array
            foreach (FieldInfo m in Fields)
            {
                // Read the value of the property.  The parameter is ignored.
                o = m.GetValue(o);

                // Compare it with the sought value
                if ((Guid)o == guid)
                {
                    return m.Name;
                }
            }

            return guid.ToString();
        }
    }

	#endregion

#endif
} // namespace DShowNET