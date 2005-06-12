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

    /// <summary>
    /// Not from DirectShow
    /// </summary>
    public enum PinConnectedStatus
    {
        Unconnected,
        Connected
    }

    /// <summary>
    /// From BITMAPINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfo 
    { 
        BitmapInfoHeader bmiHeader; 
        int []         bmiColors;
    }

    /// <summary>
    /// From BITMAPINFOHEADER
    /// </summary>
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

    /// <summary>
    /// From DDPIXELFORMAT
    /// </summary>
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

    /// <summary>
    /// From CAUUID
    /// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DsCAUUID
	{
		public int cElems;
		public IntPtr pElems;

    /// <summary>
    /// Perform a manual marshaling of pElems to retrieve an array of System.Guid.
    /// Assume this structure has been already filled by the ISpecifyPropertyPages.GetPages() method.
    /// </summary>
    /// <returns>A managed representation of pElems (cElems items)</returns>
    public Guid[] ToGuidArray()
    {
      Guid[] retval = new Guid[this.cElems];

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
	public class DsLong
	{
		public DsLong(long Value)
		{
			this.Value = Value;
		}

		public long Value;
	}

    [StructLayout(LayoutKind.Explicit)]
    public class DsGuid
    {
        [FieldOffset(0)]
        private Guid guid;

        public static readonly DsGuid Empty = Guid.Empty;

        public DsGuid()
        {
            this.guid = Guid.Empty;
        }

        public DsGuid(string g)
        {
            this.guid = new Guid(g);
        }

        public DsGuid(Guid g)
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

        public static implicit operator Guid(DsGuid g)
        {
            return g.guid;
        }

        public static implicit operator DsGuid(Guid g)
        {
            return new DsGuid(g);
        }
    }

    #endregion

	#region Utility Classes

    public class DsResults
    {
        public const int E_InvalidMediaType = unchecked((int)0x80040200);
        public const int E_InvalidSubType = unchecked((int)0x80040201);
        public const int E_NeedOwner = unchecked((int)0x80040202);
        public const int E_EnumOutOfSync = unchecked((int)0x80040203);
        public const int E_AlreadyConnected = unchecked((int)0x80040204);
        public const int E_FilterActive = unchecked((int)0x80040205);
        public const int E_NoTypes = unchecked((int)0x80040206);
        public const int E_NoAcceptableTypes = unchecked((int)0x80040207);
        public const int E_InvalidDirection = unchecked((int)0x80040208);
        public const int E_NotConnected = unchecked((int)0x80040209);
        public const int E_NoAllocator = unchecked((int)0x8004020A);
        public const int E_RunTimeError = unchecked((int)0x8004020B);
        public const int E_BufferNotSet = unchecked((int)0x8004020C);
        public const int E_BufferOverflow = unchecked((int)0x8004020D);
        public const int E_BadAlign = unchecked((int)0x8004020E);
        public const int E_AlreadyCommitted = unchecked((int)0x8004020F);
        public const int E_BuffersOutstanding = unchecked((int)0x80040210);
        public const int E_NotCommitted = unchecked((int)0x80040211);
        public const int E_SizeNotSet = unchecked((int)0x80040212);
        public const int E_NoClock = unchecked((int)0x80040213);
        public const int E_NoSink = unchecked((int)0x80040214);
        public const int E_NoInterface = unchecked((int)0x80040215);
        public const int E_NotFound = unchecked((int)0x80040216);
        public const int E_CannotConnect = unchecked((int)0x80040217);
        public const int E_CannotRender = unchecked((int)0x80040218);
        public const int E_ChangingFormat = unchecked((int)0x80040219);
        public const int E_NoColorKeySet = unchecked((int)0x8004021A);
        public const int E_NotOverlayConnection = unchecked((int)0x8004021B);
        public const int E_NotSampleConnection = unchecked((int)0x8004021C);
        public const int E_PaletteSet = unchecked((int)0x8004021D);
        public const int E_ColorKeySet = unchecked((int)0x8004021E);
        public const int E_NoColorKeyFound = unchecked((int)0x8004021F);
        public const int E_NoPaletteAvailable = unchecked((int)0x80040220);
        public const int E_NoDisplayPalette = unchecked((int)0x80040221);
        public const int E_TooManyColors = unchecked((int)0x80040222);
        public const int E_StateChanged = unchecked((int)0x80040223);
        public const int E_NotStopped = unchecked((int)0x80040224);
        public const int E_NotPaused = unchecked((int)0x80040225);
        public const int E_NotRunning = unchecked((int)0x80040226);
        public const int E_WrongState = unchecked((int)0x80040227);
        public const int E_StartTimeAfterEnd = unchecked((int)0x80040228);
        public const int E_InvalidRect = unchecked((int)0x80040229);
        public const int E_TypeNotAccepted = unchecked((int)0x8004022A);
        public const int E_SampleRejected = unchecked((int)0x8004022B);
        public const int E_SampleRejectedEOS = unchecked((int)0x8004022C);
        public const int E_DuplicateName = unchecked((int)0x8004022D);
        public const int S_DuplicateName = unchecked((int)0x0004022D);
        public const int E_Timeout = unchecked((int)0x8004022E);
        public const int E_InvalidFileFormat = unchecked((int)0x8004022F);
        public const int E_EnumOutOfRange = unchecked((int)0x80040230);
        public const int E_CircularGraph = unchecked((int)0x80040231);
        public const int E_NotAllowedToSave = unchecked((int)0x80040232);
        public const int E_TimeAlreadyPassed = unchecked((int)0x80040233);
        public const int E_AlreadyCancelled = unchecked((int)0x80040234);
        public const int E_CorruptGraphFile = unchecked((int)0x80040235);
        public const int E_AdviseAlreadySet = unchecked((int)0x80040236);
        public const int S_StateIntermediate = unchecked((int)0x00040237);
        public const int E_NoModexAvailable = unchecked((int)0x80040238);
        public const int E_NoAdviseSet = unchecked((int)0x80040239);
        public const int E_NoFullScreen = unchecked((int)0x8004023A);
        public const int E_InFullScreenMode = unchecked((int)0x8004023B);
        public const int E_UnknownFileType = unchecked((int)0x80040240);
        public const int E_CannotLoadSourceFilter = unchecked((int)0x80040241);
        public const int S_PartialRender = unchecked((int)0x00040242);
        public const int E_FileTooShort = unchecked((int)0x80040243);
        public const int E_InvalidFileVersion = unchecked((int)0x80040244);
        public const int S_SomeDataIgnored = unchecked((int)0x00040245);
        public const int S_ConnectionsDeferred = unchecked((int)0x00040246);
        public const int E_InvalidCLSID = unchecked((int)0x80040247);
        public const int E_InvalidMediaType2 = unchecked((int)0x80040248);
        public const int E_BabKey = unchecked((int)0x800403F2);
        public const int S_NoMoreItems = unchecked((int)0x00040103);
        public const int E_SampleTimeNotSet = unchecked((int)0x80040249);
        public const int S_ResourceNotNeeded = unchecked((int)0x00040250);
        public const int E_MediaTimeNotSet = unchecked((int)0x80040251);
        public const int E_NoTimeFormatSet = unchecked((int)0x80040252);
        public const int E_MonoAudioHW = unchecked((int)0x80040253);
        public const int S_MediaTypeIgnored = unchecked((int)0x00040254);
        public const int E_NoDecompressor = unchecked((int)0x80040255);
        public const int E_NoAudioHardware = unchecked((int)0x80040256);
        public const int S_VideoNotRendered = unchecked((int)0x00040257);
        public const int S_AudioNotRendered = unchecked((int)0x00040258);
        public const int E_RPZA = unchecked((int)0x80040259);
        public const int S_RPZA = unchecked((int)0x0004025A);
        public const int E_ProcessorNotSuitable = unchecked((int)0x8004025B);
        public const int E_UnsupportedAudio = unchecked((int)0x8004025C);
        public const int E_UnsupportedVideo = unchecked((int)0x8004025D);
        public const int E_MPEGNotConstrained = unchecked((int)0x8004025E);
        public const int E_NotInGraph = unchecked((int)0x8004025F);
        public const int S_Estimated = unchecked((int)0x00040260);
        public const int E_NoTimeFormat = unchecked((int)0x80040261);
        public const int E_ReadOnly = unchecked((int)0x80040262);
        public const int S_Reserved = unchecked((int)0x00040263);
        public const int E_BufferUnderflow = unchecked((int)0x80040264);
        public const int E_UnsupportedStream = unchecked((int)0x80040265);
        public const int E_NoTransport = unchecked((int)0x80040266);
        public const int S_StreamOff = unchecked((int)0x00040267);
        public const int S_CantCue = unchecked((int)0x00040268);
        public const int E_BadVideoCD = unchecked((int)0x80040269);
        public const int S_NoStopTime = unchecked((int)0x00040270);
        public const int E_OutOfVideoMemory = unchecked((int)0x80040271);
        public const int E_VPNegotiationFailed = unchecked((int)0x80040272);
        public const int E_DDrawCapsNotSuitable = unchecked((int)0x80040273);
        public const int E_NoVPHardware = unchecked((int)0x80040274);
        public const int E_NoCaptureHardware = unchecked((int)0x80040275);
        public const int E_DVDOperationInhibited = unchecked((int)0x80040276);
        public const int E_DVDInvalidDomain = unchecked((int)0x80040277);
        public const int E_DVDNoButton = unchecked((int)0x80040278);
        public const int E_DVDGraphNotReady = unchecked((int)0x80040279);
        public const int E_DVDRenderFail = unchecked((int)0x8004027A);
        public const int E_DVDDecNotEnough = unchecked((int)0x8004027B);
        public const int E_DDrawVersionNotSuitable = unchecked((int)0x8004027C);
        public const int E_CopyProtFailed = unchecked((int)0x8004027D);
        public const int S_NoPreviewPin = unchecked((int)0x0004027E);
        public const int E_TimeExpired = unchecked((int)0x8004027F);
        public const int S_DVDNonOneSequential = unchecked((int)0x00040280);
        public const int E_DVDWrongSpeed = unchecked((int)0x80040281);
        public const int E_DVDMenuDoesNotExist = unchecked((int)0x80040282);
        public const int E_DVDCmdCancelled = unchecked((int)0x80040283);
        public const int E_DVDStateWrongVersion = unchecked((int)0x80040284);
        public const int E_DVDStateCorrupt = unchecked((int)0x80040285);
        public const int E_DVDStateWrongDisc = unchecked((int)0x80040286);
        public const int E_DVDIncompatibleRegion = unchecked((int)0x80040287);
        public const int E_DVDNoAttributes = unchecked((int)0x80040288);
        public const int E_DVDNoGoupPGC = unchecked((int)0x80040289);
        public const int E_DVDLowParentalLevel = unchecked((int)0x8004028A);
        public const int E_DVDNotInKaraokeMode = unchecked((int)0x8004028B);
        public const int S_DVDChannelContentsNotAvailable = unchecked((int)0x0004028C);
        public const int S_DVDNotAccurate = unchecked((int)0x0004028D);
        public const int E_FrameStepUnsupported = unchecked((int)0x8004028E);
        public const int E_DVDStreamDisabled = unchecked((int)0x8004028F);
        public const int E_DVDTitleUnknown = unchecked((int)0x80040290);
        public const int E_DVDInvalidDisc = unchecked((int)0x80040291);
        public const int E_DVDNoResumeInformation = unchecked((int)0x80040292);
        public const int E_PinAlreadyBlockedOnThisThread = unchecked((int)0x80040293);
        public const int E_PinAlreadyBlocked = unchecked((int)0x80040294);
        public const int E_CertificationFailure = unchecked((int)0x80040295);
        public const int E_VMRNotInMixerMode = unchecked((int)0x80040296);
        public const int E_VMRNoApSupplied = unchecked((int)0x80040297);
        public const int E_VMRNoDeinterlace_HW = unchecked((int)0x80040298);
        public const int E_VMRNoProcAMPHW = unchecked((int)0x80040299);
        public const int E_DVDVMR9IncompatibleDec = unchecked((int)0x8004029A);
        public const int E_NoCOPPHW = unchecked((int)0x8004029B);
    }


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
                int id = System.Diagnostics.Process.GetCurrentProcess().Id;
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
        public static DsDevice[] GetDevicesOfCat(Guid devcat)
        {
            // Use arrayList to build the retun list since it is easily resizable
            ArrayList devs = new ArrayList();
            DsDevice [] devret;
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

                // Copy the ArrayList to the DsDevicep[]
                devret = new DsDevice[devs.Count];
                devs.CopyTo(devret, 0);
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

            return devret;
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
        /// <summary>
        /// Scans a filter's pins looking for a pin with the specified connection status
        /// </summary>
        /// <param name="vSource">The filter to scan</param>
        /// <param name="vStat">The status to find (connected/unconnected)</param>
        /// <param name="iIndex">Zero based index (ie 2 will return the third pin with the specified status)</param>
        /// <returns>The matching pin, or null if not found</returns>
        public static IPin ByConnectionStatus(IBaseFilter vSource, PinConnectedStatus vStat, int iIndex)
        {
            int hr;
            int lFetched;
            IEnumPins ppEnum;
            IPin pRet = null;
            IPin pOutPin;
            IPin[] pPins = new IPin[1];

            // Get the pin enumerator
            hr = vSource.EnumPins(out ppEnum);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // Walk the pins looking for a match
                while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
                {
                    // Read the connected status
                    hr = pPins[0].ConnectedTo(out pOutPin);

                    // Check for VFW_E_NOT_CONNECTED.  Anything else is bad.
                    if (hr != DsResults.E_NotConnected)
                    {
                        DsError.ThrowExceptionForHR(hr);

                        // The ConnectedTo call succeeded, release the interface
                        Marshal.ReleaseComObject(pOutPin);
                    }

                    // Is it the right status?
                    if (
                        (hr == 0 && vStat == PinConnectedStatus.Connected) ||
                        (hr == DsResults.E_NotConnected && vStat == PinConnectedStatus.Unconnected)
                        )
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


    // This abstract class contains definitions for use in implementing a custom marshaler.
    //
    // MarshalManagedToNative() gets called before the COM method, and MarshalNativeToManaged() gets
    // called after.  This allows for allocating a correctly sized memory block for the COM call,
    // then to break up the memory block and build an object that c# can digest.

    abstract internal class DsMarshaler : ICustomMarshaler
    {
        #region Data Members
        // The cookie isn't currently being used.
        protected string m_cookie;

        // The managed object passed in to MarshalManagedToNative, and modified in MarshalNativeToManaged
        protected object m_obj;
        #endregion

        // The constructor.  This is called from GetInstance (below)
        public DsMarshaler(string cookie)
        {
            // If we get a cookie, save it.
            m_cookie = cookie;
        }

        // Called just before invoking the COM method.  The returned IntPtr is what goes on the stack
        // for the COM call.  The input arg is the parameter that was passed to the method.
        virtual public IntPtr MarshalManagedToNative(object managedObj)
        {
            // Save off the passed-in value.  Safe since we just checked the type.
            m_obj = managedObj;

            // Create an appropriately sized buffer, and send it to the marshaler to
            // make the COM call with.
            IntPtr p = Marshal.AllocCoTaskMem(GetNativeDataSize());

            return p;
        }

        // Called just after invoking the COM method.  The IntPtr is the same one that just got returned
        // from MarshalManagedToNative.  The return value is unused.
        virtual public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return m_obj;
        }

        // Release the (now unused) buffer
        virtual public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (pNativeData != IntPtr.Zero)
            {
                Marshal.Release(pNativeData);
            }
        }

        // Release the (now unused) managed object
        virtual public void CleanUpManagedData(object managedObj)
        {
            m_obj = null;
        }

        // This routine is (apparently) never called by the marshaler.  However it can be useful.
        abstract public int GetNativeDataSize();

        // GetInstance is called by the marshaler in preparation to doing custom marshaling.  The (optional)
        // cookie is the value specified in MarshalCookie="asdf", or "" is none is specified.

        // It is commented out in this abstract class, but MUST be implemented in derived classes
        //public static ICustomMarshaler GetInstance(string cookie)
    }

    #endregion
}