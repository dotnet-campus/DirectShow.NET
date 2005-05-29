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

    /// <summary>
    /// CLSID_SystemDeviceEnum
    /// </summary>
    [ComImport, Guid("62BE5D10-60EB-11d0-BD3B-00A0C911CE86")]
    public class CreateDevEnum
    {
    }


    /// <summary>
    /// CLSID_FilterGraph
    /// </summary>
    [ComImport, Guid("e436ebb3-524f-11ce-9f53-0020af0ba770")]
    public class FilterGraph
    {
    }


    /// <summary>
    /// CLSID_FilterGraphNoThread
    /// </summary>
    [ComImport, Guid("e436ebb8-524f-11ce-9f53-0020af0ba770")]
    public class FilterGraphNoThread
    {
    }


    /// <summary>
    /// CLSID_CaptureGraphBuilder2
    /// </summary>
    [ComImport, Guid("BF87B6E1-8C27-11d0-B3F0-00AA003761C5")]
    public class CaptureGraphBuilder2
    {
    }


    /// <summary>
    /// CLSID_DvdGraphBuilder
    /// </summary>
    [ComImport, Guid("FCC152B7-F372-11d0-8E00-00C04FD7C08B")]
    public class DvdGraphBuilder
    {
    }


    /// <summary>
    /// CLSID_CaptureGraphBuilder
    /// </summary>
    [ComImport, Guid("BF87B6E0-8C27-11d0-B3F0-00AA003761C5")]
    public class CaptureGraphBuilder
    {
    }


    /// <summary>
    /// CLSID_StreamBufferConfig
    /// </summary>
    [ComImport, Guid("FA8A68B2-C864-4ba2-AD53-D3876A87494B")]
    public class StreamBufferConfig
    {
    }


    /// <summary>
    /// CLSID_StreamBufferComposeRecording
    /// </summary>
    [ComImport, Guid("D682C4BA-A90A-42fe-B9E1-03109849C423")]
    public class StreamBufferComposeRecording
    {
    }


    /// <summary>
    /// CLSID_SeekingPassThru
    /// </summary>
    [ComImport, Guid("060AF76C-68DD-11d0-8FC1-00C04FD9189D")]
    public class SeekingPassThru
    {
    }


    /// <summary>
    /// CLSID_FilterMapper2
    /// </summary>
    [ComImport, Guid("CDA42200-BD88-11d0-BD4E-00A0C911CE86")]
    public class FilterMapper2
    {
    }


    /// <summary>
    /// CLSID_MemoryAllocator
    /// </summary>
    [ComImport, Guid("1e651cc0-b199-11d0-8212-00c04fc32c45")]
    public class MemoryAllocator
    {
    }


    /// <summary>
    /// CLSID_MediaPropertyBag
    /// </summary>
    [ComImport, Guid("CDBD8D00-C193-11d0-BD4E-00A0C911CE86")]
    public class MediaPropertyBag
    {
    }


    /// <summary>
    /// CLSID_DVDState
    /// </summary>
    [ComImport, Guid("f963c5cf-a659-4a93-9638-caf3cd277d13")]
    public class DVDState
    {
    }


    #endregion

    #region Filter Classes
    /// <summary>
    /// CLSID_DMOWrapperFilter
    /// </summary>
    [ComImport, Guid("94297043-bd82-4dfd-b0de-8177739c6d20")]
    public class DMOWrapperFilter
    {
    }


    /// <summary>
    /// CLSID_StreamBufferSink
    /// </summary>
    [ComImport, Guid("2DB47AE5-CF39-43c2-B4D6-0CD8D90946F4")]
    public class StreamBufferSink
    {
    }


    /// <summary>
    /// CLSID_SampleGrabber
    /// </summary>
    [ComImport, Guid("C1F400A0-3F08-11d3-9F0B-006008039E37")]
    public class SampleGrabber
    {
    }


    /// <summary>
    /// CLSID_StreamBufferSource
    /// </summary>
    [ComImport, Guid("C9F5FE02-F851-4eb5-99EE-AD602AF1E619")]
    public class StreamBufferSource
    {
    }


    /// <summary>
    /// CLSID_VideoMixingRenderer
    /// </summary>
    [ComImport, Guid("B87BEB7B-8D29-423f-AE4D-6582C10175AC")]
    public class VideoMixingRenderer
    {
    }


    /// <summary>
    /// CLSID_VideoMixingRenderer9
    /// </summary>
    [ComImport, Guid("51b4abf3-748f-4e3b-a276-c828330e926a")]
    public class VideoMixingRenderer9
    {
    }


    /// <summary>
    /// CLSID_VideoRendererDefault
    /// </summary>
    [ComImport, Guid("6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50")]
    public class VideoRendererDefault
    {
    }


    /// <summary>
    /// CLSID_AviSplitter
    /// </summary>
    [ComImport, Guid("1b544c20-fd0b-11ce-8c63-00aa0044b51e")]
    public class AviSplitter
    {
    }


    /// <summary>
    /// CLSID_SmartTee
    /// </summary>
    [ComImport, Guid("CC58E280-8AA1-11d1-B3F1-00AA003761C5")]
    public class SmartTee
    {
    }


    /// <summary>
    /// CLSID_NullRenderer
    /// </summary>
    [ComImport, Guid("C1F400A4-3F08-11d3-9F0B-006008039E37")]
    public class NullRenderer
    {
    }


    /// <summary>
    /// CLSID_ACMWrapper
    /// </summary>
    [ComImport, Guid("6a08cf80-0e18-11cf-a24d-0020afd79767")]
    public class ACMWrapper
    {
    }


    /// <summary>
    /// CLSID_AudioRender
    /// </summary>
    [ComImport, Guid("e30629d1-27e5-11ce-875d-00608cb78066")]
    public class AudioRender
    {
    }


    /// <summary>
    /// CLSID_AVIDec
    /// </summary>
    [ComImport, Guid("CF49D4E0-1115-11ce-B03A-0020AF0BA770")]
    public class AVIDec
    {
    }


    /// <summary>
    /// CLSID_AVIDraw
    /// </summary>
    [ComImport, Guid("A888DF60-1E90-11cf-AC98-00AA004C0FA9")]
    public class AVIDraw
    {
    }


    /// <summary>
    /// CLSID_AviDest
    /// </summary>
    [ComImport, Guid("E2510970-F137-11CE-8B67-00AA00A3F1A6")]
    public class AviDest
    {
    }


    /// <summary>
    /// CLSID_ATSCNetworkProvider
    /// </summary>
    [ComImport, Guid("0DAD2FDD-5FD7-11D3-8F50-00C04F7971E2")]
    public class ATSCNetworkProvider
    {
    }


    /// <summary>
    /// CLSID_DVBCNetworkProvider
    /// </summary>
    [ComImport, Guid("DC0C0FE7-0485-4266-B93F-68FBF80ED834")]
    public class DVBCNetworkProvider
    {
    }


    /// <summary>
    /// CLSID_DVBSNetworkProvider
    /// </summary>
    [ComImport, Guid("FA4B375A-45B4-4d45-8440-263957B11623")]
    public class DVBSNetworkProvider
    {
    }


    /// <summary>
    /// CLSID_DVBTNetworkProvider
    /// </summary>
    [ComImport, Guid("216C62DF-6D7F-4e9a-8571-05F14EDB766A")]

    public class DVBTNetworkProvider
    {
    }


    /// <summary>
    /// CLSID_Colour
    /// </summary>
    [ComImport, Guid("1643e180-90f5-11ce-97d5-00aa0055595a")]
    public class Colour
    {
    }


    /// <summary>
    /// CLSID_DSoundRender
    /// </summary>
    [ComImport, Guid("79376820-07D0-11cf-A24D-0020AFD79767")]
    public class DSoundRender
    {
    }


    /// <summary>
    /// CLSID_DVMux
    /// </summary>
    [ComImport, Guid("129D7E40-C10D-11d0-AFB9-00AA00B67A42")]
    public class DVMux
    {
    }


    /// <summary>
    /// CLSID_DVSplitter
    /// </summary>
    [ComImport, Guid("4EB31670-9FC6-11cf-AF6E-00AA00B67A42")]
    public class DVSplitter
    {
    }


    /// <summary>
    /// CLSID_DVVideoCodec
    /// </summary>
    [ComImport, Guid("B1B77C00-C3E4-11cf-AF79-00AA00B67A42")]
    public class DVVideoCodec
    {
    }


    /// <summary>
    /// CLSID_DVVideoEnc
    /// </summary>
    [ComImport, Guid("13AA3650-BB6F-11d0-AFB9-00AA00B67A42")]
    public class DVVideoEnc
    {
    }


    /// <summary>
    /// CLSID_DVDNavigator
    /// </summary>
    [ComImport, Guid("9B8C4620-2C1A-11d0-8493-00A02438AD48")]
    public class DVDNavigator
    {
    }


    /// <summary>
    /// CLSID_AsyncReader
    /// </summary>
    [ComImport, Guid("e436ebb5-524f-11ce-9f53-0020af0ba770")]
    public class AsyncReader
    {
    }


    /// <summary>
    /// CLSID_URLReader
    /// </summary>
    [ComImport, Guid("e436ebb6-524f-11ce-9f53-0020af0ba770")]
    public class URLReader
    {
    }


    /// <summary>
    /// CLSID_FileWriter
    /// </summary>
    [ComImport, Guid("8596E5F0-0DA5-11d0-BD21-00A0C911CE86")]
    public class FileWriter
    {
    }


    /// <summary>
    /// CLSID_ModexRenderer
    /// </summary>
    [ComImport, Guid("07167665-5011-11cf-BF33-00AA0055595A")]
    public class ModexRenderer
    {
    }


    /// <summary>
    /// CLSID_InfTee
    /// </summary>
    [ComImport, Guid("F8388A40-D5BB-11d0-BE5A-0080C706568E")]
    public class InfTee
    {
    }


    /// <summary>
    /// CLSID_Line21Decoder
    /// </summary>
    [ComImport, Guid("6E8D4A20-310C-11d0-B79A-00AA003767A7")]
    public class Line21Decoder
    {
    }


    /// <summary>
    /// CLSID_Line21Decoder2
    /// </summary>
    [ComImport, Guid("E4206432-01A1-4BEE-B3E1-3702C8EDC574")]
    public class Line21Decoder2
    {
    }


    /// <summary>
    /// CLSID_AVIMIDIRender
    /// </summary>
    [ComImport, Guid("07b65360-c445-11ce-afde-00aa006c14f4")]
    public class AVIMIDIRender
    {
    }


    /// <summary>
    /// CLSID_MJPGEnc
    /// </summary>
    [ComImport, Guid("B80AB0A0-7416-11d2-9EEB-006008039E37")]
    public class MJPGEnc
    {
    }


    /// <summary>
    /// CLSID_MjpegDec
    /// </summary>
    [ComImport, Guid("301056D0-6DFF-11d2-9EEB-006008039E37")]
    public class MjpegDec
    {
    }


    /// <summary>
    /// CLSID_CMpegAudioCodec
    /// </summary>
    [ComImport, Guid("4a2286e0-7bef-11ce-9bd9-0000e202599c")]
    public class CMpegAudioCodec
    {
    }


    /// <summary>
    /// CLSID_MPEG1Splitter
    /// </summary>
    [ComImport, Guid("336475d0-942a-11ce-a870-00aa002feab5")]
    public class MPEG1Splitter
    {
    }


    /// <summary>
    /// CLSID_CMpegVideoCodec
    /// </summary>
    [ComImport, Guid("feb50740-7bef-11ce-9bd9-0000e202599c")]
    public class CMpegVideoCodec
    {
    }


    /// <summary>
    /// CLSID_MPEG2Demultiplexer
    /// </summary>
    [ComImport, Guid("afb6c280-2c41-11d3-8a60-0000f81e0e4a")]
    public class MPEG2Demultiplexer
    {
    }


    /// <summary>
    /// CLSID_MMSPLITTER
    /// </summary>
    [ComImport, Guid("3ae86b20-7be8-11d1-abe6-00a0c905f375")]
    public class MMSPLITTER
    {
    }


    /// <summary>
    /// CLSID_OverlayMixer
    /// </summary>
    [ComImport, Guid("CD8743A1-3736-11d0-9E69-00C04FD7C15B")]
    public class OverlayMixer
    {
    }


    /// <summary>
    /// CLSID_QTDec
    /// </summary>
    [ComImport, Guid("FDFE9681-74A3-11d0-AFA7-00AA00B67A42")]
    public class QTDec
    {
    }


    /// <summary>
    /// CLSID_QuickTimeParser
    /// </summary>
    [ComImport, Guid("D51BD5A0-7548-11cf-A520-0080C77EF58A")]
    public class QuickTimeParser
    {
    }


    /// <summary>
    /// CLSID_VBISurfaces
    /// </summary>
    [ComImport, Guid("814B9800-1C88-11d1-BAD9-00609744111A")]
    public class VBISurfaces
    {
    }


    /// <summary>
    /// CLSID_VfwCapture
    /// </summary>
    [ComImport, Guid("1b544c22-fd0b-11ce-8c63-00aa0044b51e")]
    public class VfwCapture
    {
    }


    /// <summary>
    /// CLSID_Dither
    /// </summary>
    [ComImport, Guid("1da08500-9edc-11cf-bc10-00aa00ac74f6")]
    public class Dither
    {
    }


    /// <summary>
    /// CLSID_VideoPortManager
    /// </summary>
    [ComImport, Guid("6f26a6cd-967b-47fd-874a-7aed2c9d25a2")]
    public class VideoPortManager
    {
    }


    /// <summary>
    /// CLSID_VideoRenderer
    /// </summary>
    [ComImport, Guid("70e102b0-5556-11ce-97c0-00aa0055595a")]
    public class VideoRenderer
    {
    }


    /// <summary>
    /// CLSID_WMAsfReader
    /// </summary>
    [ComImport, Guid("187463A0-5BB7-11d3-ACBE-0080C75E246E")]
    public class WMAsfReader
    {
    }


    /// <summary>
    /// CLSID_SystemClock
    /// </summary>
    [ComImport, Guid("e436ebb1-524f-11ce-9f53-0020af0ba770")]
    public class SystemClock
    {
    }


    /// <summary>
    /// CLSID_WMAsfWriter
    /// </summary>
    [ComImport, Guid("7c23220e-55bb-11d3-8b16-00c04fb6bd3d")]
    public class WMAsfWriter
    {
    }


    /// <summary>
    /// CLSID_WSTDecoder
    /// </summary>
    [ComImport, Guid("70BC06E0-5666-11d3-A184-00105AEF9F33")]
    public class WSTDecoder
    {
    }


    /// <summary>
    /// CLSID_Mpeg2VideoStreamAnalyzer
    /// </summary>
    [ComImport, Guid("6CFAD761-735D-4aa5-8AFC-AF91A7D61EBA")]
    public class Mpeg2VideoStreamAnalyzer
    {
    }


    #endregion

    #region Declarations

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfo 
    { 
        BitmapInfoHeader bmiHeader; 
        int []         bmiColors;
    }

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
        public const int VFW_E_INVALIDMEDIATYPE = -2147220992;   // 0x80040200
        public const int VFW_E_INVALIDSUBTYPE = -2147220991;   // 0x80040201
        public const int VFW_E_NEED_OWNER = -2147220990;   // 0x80040202
        public const int VFW_E_ENUM_OUT_OF_SYNC = -2147220989;   // 0x80040203
        public const int VFW_E_ALREADY_CONNECTED = -2147220988;   // 0x80040204
        public const int VFW_E_FILTER_ACTIVE = -2147220987;   // 0x80040205
        public const int VFW_E_NO_TYPES = -2147220986;   // 0x80040206
        public const int VFW_E_NO_ACCEPTABLE_TYPES = -2147220985;   // 0x80040207
        public const int VFW_E_INVALID_DIRECTION = -2147220984;   // 0x80040208
        public const int VFW_E_NOT_CONNECTED = -2147220983;   // 0x80040209
        public const int VFW_E_NO_ALLOCATOR = -2147220982;   // 0x8004020A
        public const int VFW_E_RUNTIME_ERROR = -2147220981;   // 0x8004020B
        public const int VFW_E_BUFFER_NOTSET = -2147220980;   // 0x8004020C
        public const int VFW_E_BUFFER_OVERFLOW = -2147220979;   // 0x8004020D
        public const int VFW_E_BADALIGN = -2147220978;   // 0x8004020E
        public const int VFW_E_ALREADY_COMMITTED = -2147220977;   // 0x8004020F
        public const int VFW_E_BUFFERS_OUTSTANDING = -2147220976;   // 0x80040210
        public const int VFW_E_NOT_COMMITTED = -2147220975;   // 0x80040211
        public const int VFW_E_SIZENOTSET = -2147220974;   // 0x80040212
        public const int VFW_E_NO_CLOCK = -2147220973;   // 0x80040213
        public const int VFW_E_NO_SINK = -2147220972;   // 0x80040214
        public const int VFW_E_NO_INTERFACE = -2147220971;   // 0x80040215
        public const int VFW_E_NOT_FOUND = -2147220970;   // 0x80040216
        public const int VFW_E_CANNOT_CONNECT = -2147220969;   // 0x80040217
        public const int VFW_E_CANNOT_RENDER = -2147220968;   // 0x80040218
        public const int VFW_E_CHANGING_FORMAT = -2147220967;   // 0x80040219
        public const int VFW_E_NO_COLOR_KEY_SET = -2147220966;   // 0x8004021A
        public const int VFW_E_NOT_OVERLAY_CONNECTION = -2147220965;   // 0x8004021B
        public const int VFW_E_NOT_SAMPLE_CONNECTION = -2147220964;   // 0x8004021C
        public const int VFW_E_PALETTE_SET = -2147220963;   // 0x8004021D
        public const int VFW_E_COLOR_KEY_SET = -2147220962;   // 0x8004021E
        public const int VFW_E_NO_COLOR_KEY_FOUND = -2147220961;   // 0x8004021F
        public const int VFW_E_NO_PALETTE_AVAILABLE = -2147220960;   // 0x80040220
        public const int VFW_E_NO_DISPLAY_PALETTE = -2147220959;   // 0x80040221
        public const int VFW_E_TOO_MANY_COLORS = -2147220958;   // 0x80040222
        public const int VFW_E_STATE_CHANGED = -2147220957;   // 0x80040223
        public const int VFW_E_NOT_STOPPED = -2147220956;   // 0x80040224
        public const int VFW_E_NOT_PAUSED = -2147220955;   // 0x80040225
        public const int VFW_E_NOT_RUNNING = -2147220954;   // 0x80040226
        public const int VFW_E_WRONG_STATE = -2147220953;   // 0x80040227
        public const int VFW_E_START_TIME_AFTER_END = -2147220952;   // 0x80040228
        public const int VFW_E_INVALID_RECT = -2147220951;   // 0x80040229
        public const int VFW_E_TYPE_NOT_ACCEPTED = -2147220950;   // 0x8004022A
        public const int VFW_E_SAMPLE_REJECTED = -2147220949;   // 0x8004022B
        public const int VFW_E_SAMPLE_REJECTED_EOS = -2147220948;   // 0x8004022C
        public const int VFW_E_DUPLICATE_NAME = -2147220947;   // 0x8004022D
        public const int VFW_S_DUPLICATE_NAME = 262701;   // 0x0004022D
        public const int VFW_E_TIMEOUT = -2147220946;   // 0x8004022E
        public const int VFW_E_INVALID_FILE_FORMAT = -2147220945;   // 0x8004022F
        public const int VFW_E_ENUM_OUT_OF_RANGE = -2147220944;   // 0x80040230
        public const int VFW_E_CIRCULAR_GRAPH = -2147220943;   // 0x80040231
        public const int VFW_E_NOT_ALLOWED_TO_SAVE = -2147220942;   // 0x80040232
        public const int VFW_E_TIME_ALREADY_PASSED = -2147220941;   // 0x80040233
        public const int VFW_E_ALREADY_CANCELLED = -2147220940;   // 0x80040234
        public const int VFW_E_CORRUPT_GRAPH_FILE = -2147220939;   // 0x80040235
        public const int VFW_E_ADVISE_ALREADY_SET = -2147220938;   // 0x80040236
        public const int VFW_S_STATE_INTERMEDIATE = 262711;   // 0x00040237
        public const int VFW_E_NO_MODEX_AVAILABLE = -2147220936;   // 0x80040238
        public const int VFW_E_NO_ADVISE_SET = -2147220935;   // 0x80040239
        public const int VFW_E_NO_FULLSCREEN = -2147220934;   // 0x8004023A
        public const int VFW_E_IN_FULLSCREEN_MODE = -2147220933;   // 0x8004023B
        public const int VFW_E_UNKNOWN_FILE_TYPE = -2147220928;   // 0x80040240
        public const int VFW_E_CANNOT_LOAD_SOURCE_FILTER = -2147220927;   // 0x80040241
        public const int VFW_S_PARTIAL_RENDER = 262722;   // 0x00040242
        public const int VFW_E_FILE_TOO_SHORT = -2147220925;   // 0x80040243
        public const int VFW_E_INVALID_FILE_VERSION = -2147220924;   // 0x80040244
        public const int VFW_S_SOME_DATA_IGNORED = 262725;   // 0x00040245
        public const int VFW_S_CONNECTIONS_DEFERRED = 262726;   // 0x00040246
        public const int VFW_E_INVALID_CLSID = -2147220921;   // 0x80040247
        public const int VFW_E_INVALID_MEDIA_TYPE = -2147220920;   // 0x80040248
        public const int VFW_E_BAD_KEY = -2147220494;   // 0x800403F2
        public const int VFW_S_NO_MORE_ITEMS = 262403;   // 0x00040103
        public const int VFW_E_SAMPLE_TIME_NOT_SET = -2147220919;   // 0x80040249
        public const int VFW_S_RESOURCE_NOT_NEEDED = 262736;   // 0x00040250
        public const int VFW_E_MEDIA_TIME_NOT_SET = -2147220911;   // 0x80040251
        public const int VFW_E_NO_TIME_FORMAT_SET = -2147220910;   // 0x80040252
        public const int VFW_E_MONO_AUDIO_HW = -2147220909;   // 0x80040253
        public const int VFW_S_MEDIA_TYPE_IGNORED = 262740;   // 0x00040254
        public const int VFW_E_NO_DECOMPRESSOR = -2147220907;   // 0x80040255
        public const int VFW_E_NO_AUDIO_HARDWARE = -2147220906;   // 0x80040256
        public const int VFW_S_VIDEO_NOT_RENDERED = 262743;   // 0x00040257
        public const int VFW_S_AUDIO_NOT_RENDERED = 262744;   // 0x00040258
        public const int VFW_E_RPZA = -2147220903;   // 0x80040259
        public const int VFW_S_RPZA = 262746;   // 0x0004025A
        public const int VFW_E_PROCESSOR_NOT_SUITABLE = -2147220901;   // 0x8004025B
        public const int VFW_E_UNSUPPORTED_AUDIO = -2147220900;   // 0x8004025C
        public const int VFW_E_UNSUPPORTED_VIDEO = -2147220899;   // 0x8004025D
        public const int VFW_E_MPEG_NOT_CONSTRAINED = -2147220898;   // 0x8004025E
        public const int VFW_E_NOT_IN_GRAPH = -2147220897;   // 0x8004025F
        public const int VFW_S_ESTIMATED = 262752;   // 0x00040260
        public const int VFW_E_NO_TIME_FORMAT = -2147220895;   // 0x80040261
        public const int VFW_E_READ_ONLY = -2147220894;   // 0x80040262
        public const int VFW_S_RESERVED = 262755;   // 0x00040263
        public const int VFW_E_BUFFER_UNDERFLOW = -2147220892;   // 0x80040264
        public const int VFW_E_UNSUPPORTED_STREAM = -2147220891;   // 0x80040265
        public const int VFW_E_NO_TRANSPORT = -2147220890;   // 0x80040266
        public const int VFW_S_STREAM_OFF = 262759;   // 0x00040267
        public const int VFW_S_CANT_CUE = 262760;   // 0x00040268
        public const int VFW_E_BAD_VIDEOCD = -2147220887;   // 0x80040269
        public const int VFW_S_NO_STOP_TIME = 262768;   // 0x00040270
        public const int VFW_E_OUT_OF_VIDEO_MEMORY = -2147220879;   // 0x80040271
        public const int VFW_E_VP_NEGOTIATION_FAILED = -2147220878;   // 0x80040272
        public const int VFW_E_DDRAW_CAPS_NOT_SUITABLE = -2147220877;   // 0x80040273
        public const int VFW_E_NO_VP_HARDWARE = -2147220876;   // 0x80040274
        public const int VFW_E_NO_CAPTURE_HARDWARE = -2147220875;   // 0x80040275
        public const int VFW_E_DVD_OPERATION_INHIBITED = -2147220874;   // 0x80040276
        public const int VFW_E_DVD_INVALIDDOMAIN = -2147220873;   // 0x80040277
        public const int VFW_E_DVD_NO_BUTTON = -2147220872;   // 0x80040278
        public const int VFW_E_DVD_GRAPHNOTREADY = -2147220871;   // 0x80040279
        public const int VFW_E_DVD_RENDERFAIL = -2147220870;   // 0x8004027A
        public const int VFW_E_DVD_DECNOTENOUGH = -2147220869;   // 0x8004027B
        public const int VFW_E_DDRAW_VERSION_NOT_SUITABLE = -2147220868;   // 0x8004027C
        public const int VFW_E_COPYPROT_FAILED = -2147220867;   // 0x8004027D
        public const int VFW_S_NOPREVIEWPIN = 262782;   // 0x0004027E
        public const int VFW_E_TIME_EXPIRED = -2147220865;   // 0x8004027F
        public const int VFW_S_DVD_NON_ONE_SEQUENTIAL = 262784;   // 0x00040280
        public const int VFW_E_DVD_WRONG_SPEED = -2147220863;   // 0x80040281
        public const int VFW_E_DVD_MENU_DOES_NOT_EXIST = -2147220862;   // 0x80040282
        public const int VFW_E_DVD_CMD_CANCELLED = -2147220861;   // 0x80040283
        public const int VFW_E_DVD_STATE_WRONG_VERSION = -2147220860;   // 0x80040284
        public const int VFW_E_DVD_STATE_CORRUPT = -2147220859;   // 0x80040285
        public const int VFW_E_DVD_STATE_WRONG_DISC = -2147220858;   // 0x80040286
        public const int VFW_E_DVD_INCOMPATIBLE_REGION = -2147220857;   // 0x80040287
        public const int VFW_E_DVD_NO_ATTRIBUTES = -2147220856;   // 0x80040288
        public const int VFW_E_DVD_NO_GOUP_PGC = -2147220855;   // 0x80040289
        public const int VFW_E_DVD_LOW_PARENTAL_LEVEL = -2147220854;   // 0x8004028A
        public const int VFW_E_DVD_NOT_IN_KARAOKE_MODE = -2147220853;   // 0x8004028B
        public const int VFW_S_DVD_CHANNEL_CONTENTS_NOT_AVAILABLE = 262796;   // 0x0004028C
        public const int VFW_S_DVD_NOT_ACCURATE = 262797;   // 0x0004028D
        public const int VFW_E_FRAME_STEP_UNSUPPORTED = -2147220850;   // 0x8004028E
        public const int VFW_E_DVD_STREAM_DISABLED = -2147220849;   // 0x8004028F
        public const int VFW_E_DVD_TITLE_UNKNOWN = -2147220848;   // 0x80040290
        public const int VFW_E_DVD_INVALID_DISC = -2147220847;   // 0x80040291
        public const int VFW_E_DVD_NO_RESUME_INFORMATION = -2147220846;   // 0x80040292
        public const int VFW_E_PIN_ALREADY_BLOCKED_ON_THIS_THREAD = -2147220845;   // 0x80040293
        public const int VFW_E_PIN_ALREADY_BLOCKED = -2147220844;   // 0x80040294
        public const int VFW_E_CERTIFICATION_FAILURE = -2147220843;   // 0x80040295
        public const int VFW_E_VMR_NOT_IN_MIXER_MODE = -2147220842;   // 0x80040296
        public const int VFW_E_VMR_NO_AP_SUPPLIED = -2147220841;   // 0x80040297
        public const int VFW_E_VMR_NO_DEINTERLACE_HW = -2147220840;   // 0x80040298
        public const int VFW_E_VMR_NO_PROCAMP_HW = -2147220839;   // 0x80040299
        public const int VFW_E_DVD_VMR9_INCOMPATIBLEDEC = -2147220838;   // 0x8004029A
        public const int VFW_E_NO_COPP_HW = -2147220837;   // 0x8004029B

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
                    hr = pKs.Get(g, (int)AMPropertyPin.Category, IntPtr.Zero, 0, ipOut, iSize, out cbBytes);
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
        public static ArrayList GetDevicesOfCat(Guid devcat)
        {
            ArrayList devs = new ArrayList();
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

            return devs;
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

                int hr = bag.Read("FriendlyName", out val, null);
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