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
using System.Runtime.InteropServices;

namespace DirectShowLib
{
	[ComVisible(false)]
	public class FilterCategory // uuids.h  :  CLSID_*
	{
		/// <summary> CLSID_AudioInputDeviceCategory, audio capture category </summary>
		public static readonly Guid AudioInputDevice = new Guid(0x33d9a762, 0x90c8, 0x11d0, 0xbd, 0x43, 0x00, 0xa0, 0xc9, 0x11, 0xce, 0x86);

		/// <summary> CLSID_VideoInputDeviceCategory, video capture category </summary>
		public static readonly Guid VideoInputDevice = new Guid(0x860BB310, 0x5D01, 0x11d0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

		/// <summary> CLSID_VideoCompressorCategory, video compressor category </summary>
		public static readonly Guid VideoCompressorCategory = new Guid(0x33d9a760, 0x90c8, 0x11d0, 0xbd, 0x43, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86);

		/// <summary> CLSID_AudioCompressorCategory, audio compressor category </summary>
		public static readonly Guid AudioCompressorCategory = new Guid(0x33d9a761, 0x90c8, 0x11d0, 0xbd, 0x43, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86);

		/// <summary> CLSID_LegacyAmFilterCategory, legacy filters </summary>
		public static readonly Guid LegacyAmFilterCategory = new Guid(0x083863F1, 0x70DE, 0x11d0, 0xBD, 0x40, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

		/// <summary> CLSID_AudioRendererCategory, Audio renderer category</summary>
		public static readonly Guid AudioRendererCategory = new Guid(0xe0f158e1, 0xcb04, 0x11d0, 0xbd, 0x4e, 0x0, 0xa0, 0xc9, 0x11, 0xce, 0x86);

		/// <summary> KSCATEGORY_BDA_RECEIVER_COMPONENT, BDA Receiver Components category</summary>
		public static readonly Guid BDAReceiverComponentsCategory = new Guid("FD0A5AF4-B41D-11d2-9C95-00C04F7971E0");

		/// <summary> KSCATEGORY_BDA_NETWORK_TUNER, BDA Source Filters category</summary>
		public static readonly Guid BDASourceFiltersCategory = new Guid("71985F48-1CA1-11d3-9CC8-00C04F7971E0");

		/// <summary> KSCATEGORY_BDA_IP_SINK, BDA Rendering Filters category</summary>
		public static readonly Guid BDARenderingFiltersCategory = new Guid("71985F4A-1CA1-11d3-9CC8-00C04F7971E0");

		/// <summary> KSCATEGORY_BDA_NETWORK_PROVIDER, BDA Network Providers category</summary>
		public static readonly Guid BDANetworkProvidersCategory = new Guid("71985F4B-1CA1-11d3-9CC8-00C04F7971E0");

		/// <summary> KSCATEGORY_BDA_TRANSPORT_INFORMATION, BDA Transport Information Renderers category</summary>
		public static readonly Guid BDATransportInformationRenderersCategory = new Guid("A2E3074F-6C3D-11d3-B653-00C04F79498E");

		public static readonly Guid DMOFilterCategory = new Guid(0xbcd5796c, 0xbd52, 0x4d30, 0xab, 0x76, 0x70, 0xf9, 0x75, 0xb8, 0x91, 0x99);
	}

	[ComVisible(false)]
	public class Clsid // uuids.h  :  CLSID_*
	{
		/// <summary> CLSID_NULL</summary>
		public static readonly Guid Null = Guid.Empty;

		/// <summary> CLSID_SystemDeviceEnum for ICreateDevEnum </summary>
		public static readonly Guid SystemDeviceEnum = new Guid(0x62BE5D10, 0x60EB, 0x11d0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

		/// <summary> CLSID_FilterGraph, filter Graph </summary>
		public static readonly Guid FilterGraph = new Guid(0xe436ebb3, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> CLSID_CaptureGraphBuilder2, new Capture graph building </summary>
		public static readonly Guid CaptureGraphBuilder2 = new Guid(0xBF87B6E1, 0x8C27, 0x11d0, 0xB3, 0xF0, 0x0, 0xAA, 0x00, 0x37, 0x61, 0xC5);

		/// <summary> CLSID_SampleGrabber, Sample Grabber filter </summary>
		public static readonly Guid SampleGrabber = new Guid(0xC1F400A0, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37);

		/// <summary> CLSID_DvdGraphBuilder, DVD graph builder </summary>
		public static readonly Guid DvdGraphBuilder = new Guid(0xFCC152B7, 0xF372, 0x11d0, 0x8E, 0x00, 0x00, 0xC0, 0x4F, 0xD7, 0xC0, 0x8B);

		/// <summary> CLSID_StreamBufferSink, stream buffer sink </summary>
		public static readonly Guid StreamBufferSink = new Guid("2db47ae5-cf39-43c2-b4d6-0cd8d90946f4");

		/// <summary> CLSID_StreamBufferSource, stream buffer sink </summary>
		public static readonly Guid StreamBufferSource = new Guid("c9f5fe02-f851-4eb5-99ee-ad602af1e619");

		/// <summary> CLSID_AviSplitter, split an AVI stream into separate video and audio streams </summary>
		public static readonly Guid AviSplitter = new Guid(0x1b544c20, 0xfd0b, 0x11ce, 0x8c, 0x63, 0x0, 0xaa, 0x00, 0x44, 0xb5, 0x1e);

		/// <summary> CLSID_SmartTee, create a preview stream when device only provides a capture stream. </summary>
		public static readonly Guid SmartTee = new Guid(0xcc58e280, 0x8aa1, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5);

		/// <summary> CLSID_AsyncReader, The Async File Source filter opens and reads local files of many different data formats and passes the data to a parser filter.</summary>
		public static readonly Guid AsyncReader = new Guid(0xe436ebb5, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> CLSID_DSoundRender, This filter renders audio using the Microsoft?DirectSound?API.</summary>
		public static readonly Guid DSoundRender = new Guid(0x79376820, 0x07D0, 0x11CF, 0xA2, 0x4D, 0x0, 0x20, 0xAF, 0xD7, 0x97, 0x67);

		/// <summary> CLSID_DMOWrapperFilter</summary>
		public static readonly Guid DMOWrapperFilter = new Guid(0x94297043, 0xbd82, 0x4dfd, 0xb0, 0xde, 0x81, 0x77, 0x73, 0x9c, 0x6d, 0x20);

		/// <summary> CLSID_MPEG2Demultiplexer </summary>
		public static readonly Guid MPEG2Demultiplexer = new Guid(0xafb6c280, 0x2c41, 0x11d3, 0x8a, 0x60, 0x00, 0x00, 0xf8, 0x1e, 0x0e, 0x4a);
	}

	[ComVisible(false)]
	public class KSProxyClsId // uuids.h  :  CLSID_*
	{
		// -------------------------------------------------------------------------
		// KSProxy GUIDS
		// -------------------------------------------------------------------------
		public static readonly Guid TVTunerFilterPropertyPage = new Guid(0x266eee41, 0x6c63, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);
		public static readonly Guid CrossbarFilterPropertyPage = new Guid(0x71f96461, 0x78f3, 0x11d0, 0xa1, 0x8c, 0x0, 0xa0, 0xc9, 0x11, 0x89, 0x56);
		public static readonly Guid TVAudioFilterPropertyPage = new Guid(0x71f96463, 0x78f3, 0x11d0, 0xa1, 0x8c, 0x0, 0xa0, 0xc9, 0x11, 0x89, 0x56);
		public static readonly Guid VideoProcAmpPropertyPage = new Guid(0x71f96464, 0x78f3, 0x11d0, 0xa1, 0x8c, 0x0, 0xa0, 0xc9, 0x11, 0x89, 0x56);
		public static readonly Guid CameraControlPropertyPage = new Guid(0x71f96465, 0x78f3, 0x11d0, 0xa1, 0x8c, 0x0, 0xa0, 0xc9, 0x11, 0x89, 0x56);
		public static readonly Guid AnalogVideoDecoderPropertyPage = new Guid(0x71f96466, 0x78f3, 0x11d0, 0xa1, 0x8c, 0x0, 0xa0, 0xc9, 0x11, 0x89, 0x56);
		public static readonly Guid VideoStreamConfigPropertyPage = new Guid(0x71f96467, 0x78f3, 0x11d0, 0xa1, 0x8c, 0x0, 0xa0, 0xc9, 0x11, 0x89, 0x56);
		public static readonly Guid AudioRendererAdvancedProperties = new Guid(0x37e92a92, 0xd9aa, 0x11d2, 0xbf, 0x84, 0x8e, 0xf2, 0xb1, 0x55, 0x5a, 0xed);
	}

	[ComVisible(false)]
	public class VMRClsId // uuids.h  :  CLSID_*
	{
		// -------------------------------------------------------------------------
		// VMR GUIDS
		// -------------------------------------------------------------------------
		public static readonly Guid VideoMixingRenderer = new Guid(0xB87BEB7B, 0x8D29, 0x423f, 0xAE, 0x4D, 0x65, 0x82, 0xC1, 0x01, 0x75, 0xAC);
		public static readonly Guid VideoRendererDefault = new Guid(0x6BC1CFFA, 0x8FC1, 0x4261, 0xAC, 0x22, 0xCF, 0xB4, 0xCC, 0x38, 0xDB, 0x50);
		public static readonly Guid AllocPresenter = new Guid(0x99d54f63, 0x1a69, 0x41ae, 0xaa, 0x4d, 0xc9, 0x76, 0xeb, 0x3f, 0x07, 0x13);
		public static readonly Guid AllocPresenterDDXclMode = new Guid(0x4444ac9e, 0x242e, 0x471b, 0xa3, 0xc7, 0x45, 0xdc, 0xd4, 0x63, 0x52, 0xbc);
		public static readonly Guid VideoPortManager = new Guid(0x6f26a6cd, 0x967b, 0x47fd, 0x87, 0x4a, 0x7a, 0xed, 0x2c, 0x9d, 0x25, 0xa2);

		// -------------------------------------------------------------------------
		// VMR GUIDS for DX9
		// -------------------------------------------------------------------------
		public static readonly Guid VideoMixingRenderer9 = new Guid(0x51b4abf3, 0x748f, 0x4e3b, 0xa2, 0x76, 0xc8, 0x28, 0x33, 0x0e, 0x92, 0x6a);
	}

	[ComVisible(false)]
	public class BDAClsId // uuids.h  :  CLSID_*
	{
		// -------------------------------------------------------------------------
		// BDA Network Provider GUIDS
		// -------------------------------------------------------------------------
		public static readonly Guid ATSCNetworkProvider = new Guid(0x0dad2fdd, 0x5fd7, 0x11d3, 0x8f, 0x50, 0x00, 0xc0, 0x4f, 0x79, 0x71, 0xe2);
		public static readonly Guid ATSCNetworkPropertyPage = new Guid(0xe3444d16, 0x5ac4, 0x4386, 0x88, 0xdf, 0x13, 0xfd, 0x23, 0x0e, 0x1d, 0xda);
		public static readonly Guid DVBSNetworkProvider = new Guid(0xfa4b375a, 0x45b4, 0x4d45, 0x84, 0x40, 0x26, 0x39, 0x57, 0xb1, 0x16, 0x23);
		public static readonly Guid DVBTNetworkProvider = new Guid(0x216c62df, 0x6d7f, 0x4e9a, 0x85, 0x71, 0x5, 0xf1, 0x4e, 0xdb, 0x76, 0x6a);
		public static readonly Guid DVBCNetworkProvider = new Guid(0xdc0c0fe7, 0x485, 0x4266, 0xb9, 0x3f, 0x68, 0xfb, 0xf8, 0xe, 0xd8, 0x34);
	}

	[ComVisible(false)]
	public class TVEClsId // uuids.h  :  CLSID_*
	{
		// -------------------------------------------------------------------------
		// TVE Receiver filter guids
		// -------------------------------------------------------------------------
		public static readonly Guid DShowTVEFilter = new Guid(0x05500280, 0xFAA5, 0x4DF9, 0x82, 0x46, 0xBF, 0xC2, 0x3A, 0xC5, 0xCE, 0xA8);
		public static readonly Guid TVEFilterTuneProperties = new Guid(0x05500281, 0xFAA5, 0x4DF9, 0x82, 0x46, 0xBF, 0xC2, 0x3A, 0xC5, 0xCE, 0xA8);
		public static readonly Guid TVEFilterCCProperties = new Guid(0x05500282, 0xFAA5, 0x4DF9, 0x82, 0x46, 0xBF, 0xC2, 0x3A, 0xC5, 0xCE, 0xA8);
		public static readonly Guid TVEFilterStatsProperties = new Guid(0x05500283, 0xFAA5, 0x4DF9, 0x82, 0x46, 0xBF, 0xC2, 0x3A, 0xC5, 0xCE, 0xA8);
	}

	[ComVisible(false)]
	public class ENCAPIClsId // uuids.h  :  CLSID_*
	{
		// -------------------------------------------------------------------------
		// Defined ENCAPI parameter GUIDs
		// -------------------------------------------------------------------------
		public static readonly Guid IVideoEncoderProxy = new Guid(0xb43c4eec, 0x8c32, 0x4791, 0x91, 0x2, 0x50, 0x8a, 0xda, 0x5e, 0xe8, 0xe7);
		public static readonly Guid ICodecAPIProxy = new Guid(0x7ff0997a, 0x1999, 0x4286, 0xa7, 0x3c, 0x62, 0x2b, 0x88, 0x14, 0xe7, 0xeb);
		public static readonly Guid IVideoEncoderCodecAPIProxy = new Guid(0xb05dabd9, 0x56e5, 0x4fdc, 0xaf, 0xa4, 0x8a, 0x47, 0xe9, 0x1f, 0x1c, 0x9c);

		public static readonly Guid DVDNavigator = new Guid(0x9b8c4620, 0x2c1a, 0x11d0, 0x84, 0x93, 0x0, 0xa0, 0x24, 0x38, 0xad, 0x48);
		public static readonly Guid Line21Decoder = new Guid(0x6e8d4a20, 0x310c, 0x11d0, 0xb7, 0x9a, 0x0, 0xaa, 0x0, 0x37, 0x67, 0xa7);
		public static readonly Guid Line21Decoder2 = new Guid(0xe4206432, 0x01a1, 0x4bee, 0xb3, 0xe1, 0x37, 0x02, 0xc8, 0xed, 0xc5, 0x74);
	}

	[ComVisible(false)]
	public class MediaType // MEDIATYPE_*
	{
		/// <summary> MEDIATYPE_Video 'vids' </summary>
		public static readonly Guid Video = new Guid(0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIATYPE_Interleaved 'iavs' </summary>
		public static readonly Guid Interleaved = new Guid(0x73766169, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIATYPE_Audio 'auds' </summary>
		public static readonly Guid Audio = new Guid(0x73647561, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIATYPE_Text 'txts' </summary>
		public static readonly Guid Text = new Guid(0x73747874, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIATYPE_Stream </summary>
		public static readonly Guid Stream = new Guid(0xe436eb83, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIATYPE_VBI </summary>
		public static readonly Guid VBI = new Guid(0xf72a76e1, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);
	}

	[ComVisible(false)]
	public class MediaSubType // MEDIASUBTYPE_*
	{
		/// <summary> MEDIASUBTYPE_YUYV 'YUYV' </summary>
		public static readonly Guid YUYV = new Guid(0x56595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_IYUV 'IYUV' </summary>
		public static readonly Guid IYUV = new Guid(0x56555949, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_DVSD 'DVSD' </summary>
		public static readonly Guid DVSD = new Guid(0x44535644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_RGB1 'RGB1' </summary>
		public static readonly Guid RGB1 = new Guid(0xe436eb78, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB4 'RGB4' </summary>
		public static readonly Guid RGB4 = new Guid(0xe436eb79, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB8 'RGB8' </summary>
		public static readonly Guid RGB8 = new Guid(0xe436eb7a, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB565 'RGB565' </summary>
		public static readonly Guid RGB565 = new Guid(0xe436eb7b, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB555 'RGB555' </summary>
		public static readonly Guid RGB555 = new Guid(0xe436eb7c, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB24 'RGB24' </summary>
		public static readonly Guid RGB24 = new Guid(0xe436eb7d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_RGB32 'RGB32' </summary>
		public static readonly Guid RGB32 = new Guid(0xe436eb7e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_Avi </summary>
		public static readonly Guid Avi = new Guid(0xe436eb88, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

		/// <summary> MEDIASUBTYPE_Asf </summary>
		public static readonly Guid Asf = new Guid(0x3db80f90, 0x9412, 0x11d1, 0xad, 0xed, 0x0, 0x0, 0xf8, 0x75, 0x4b, 0x99);

		/// <summary> MEDIASUBTYPE_CLPL </summary>
		public static readonly Guid CLPL = new Guid(0x4C504C43, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_YVU9 </summary>
		public static readonly Guid YVU9 = new Guid(0x39555659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_Y411 </summary>
		public static readonly Guid Y411 = new Guid(0x31313459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_Y41P </summary>
		public static readonly Guid Y41P = new Guid(0x50313459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_YUY2 </summary>
		public static readonly Guid YUY2 = new Guid(0x32595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_YVYU </summary>
		public static readonly Guid YVYU = new Guid(0x55595659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_UYVY </summary>
		public static readonly Guid UYVY = new Guid(0x59565955, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_Y211 </summary>
		public static readonly Guid Y211 = new Guid(0x31313259, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_YV12 </summary>
		public static readonly Guid YV12 = new Guid(0x32315659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_CLJR </summary>
		public static readonly Guid CLJR = new Guid(0x524a4c43, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_IF09 </summary>
		public static readonly Guid IF09 = new Guid(0x39304649, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_CPLA </summary>
		public static readonly Guid CPLA = new Guid(0x414c5043, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_MJPG </summary>
		public static readonly Guid MJPG = new Guid(0x47504A4D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_TVMJ </summary>
		public static readonly Guid TVMJ = new Guid(0x4A4D5654, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_WAKE </summary>
		public static readonly Guid WAKE = new Guid(0x454B4157, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_CFCC </summary>
		public static readonly Guid CFCC = new Guid(0x43434643, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_IJPG </summary>
		public static readonly Guid IJPG = new Guid(0x47504A49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_Plum </summary>
		public static readonly Guid PLUM = new Guid(0x6D756C50, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_DVCS </summary>
		public static readonly Guid DVCS = new Guid(0x53435644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_MDVF </summary>
		public static readonly Guid MDVF = new Guid(0x4656444D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_I420 </summary>
		public static readonly Guid I420 = new Guid(0x30323449, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_PCM </summary>
		public static readonly Guid PCM = new Guid(0x00000001, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

		/// <summary> MEDIASUBTYPE_ARGB1555 </summary>
		public static readonly Guid ARGB1555 = new Guid(0x297c55af, 0xe209, 0x4cb3, 0xb7, 0x57, 0xc7, 0x6d, 0x6b, 0x9c, 0x88, 0xa8);

		/// <summary> MEDIASUBTYPE_ARGB4444 </summary>
		public static readonly Guid ARGB4444 = new Guid(0x6e6415e6, 0x5c24, 0x425f, 0x93, 0xcd, 0x80, 0x10, 0x2b, 0x3d, 0x1c, 0xca);

		/// <summary> MEDIASUBTYPE_ARGB32 </summary>
		public static readonly Guid ARGB32 = new Guid(0x773c9ac0, 0x3274, 0x11d0, 0xb7, 0x24, 0x0, 0xaa, 0x0, 0x6c, 0x1a, 0x1);

		/// <summary> MEDIASUBTYPE_AYUV </summary>
		public static readonly Guid AYUV = new Guid(0x56555941, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
	}

	[ComVisible(false)]
	public class FormatType // FORMAT_*
	{
		/// <summary> FORMAT_None </summary>
		public static readonly Guid None = new Guid(0x0F6417D6, 0xc318, 0x11d0, 0xa4, 0x3f, 0x00, 0xa0, 0xc9, 0x22, 0x31, 0x96);

		/// <summary> FORMAT_VideoInfo </summary>
		public static readonly Guid VideoInfo = new Guid(0x05589f80, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

		/// <summary> FORMAT_VideoInfo2 </summary>
		public static readonly Guid VideoInfo2 = new Guid(0xf72a76A0, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> FORMAT_WaveFormatEx </summary>
		public static readonly Guid WaveEx = new Guid(0x05589f81, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

		/// <summary> FORMAT_MPEGVideo </summary>
		public static readonly Guid MpegVideo = new Guid(0x05589f82, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

		/// <summary> FORMAT_MPEGStreams </summary>
		public static readonly Guid MpegStreams = new Guid(0x05589f83, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);

		/// <summary> FORMAT_DvInfo </summary>
		public static readonly Guid DvInfo = new Guid(0x05589f84, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
	}

	[ComVisible(false)]
	public class Misc
	{
		/// <summary>AMPROPSETID_Pin</summary>
		public static readonly Guid PropsetIDPin = new Guid(0x9b00f101, 0x1567, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5);

		/// <summary> PROPSETID_VIDCAP_DROPPEDFRAMES </summary>
		public static readonly Guid DroppedFrames = new Guid(0xC6E13344, 0x30AC, 0x11D0, 0xA1, 0x8C, 0x0, 0xA0, 0xC9, 0x11, 0x89, 0x56);
	}

	[ComVisible(false)]
	public class PinCategory // PIN_CATEGORY_*
	{
		/// <summary> PIN_CATEGORY_CAPTURE </summary>
		public static readonly Guid Capture = new Guid(0xfb6c4281, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_PREVIEW </summary>
		public static readonly Guid Preview = new Guid(0xfb6c4282, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_ANALOGVIDEOIN </summary>
		public static readonly Guid AnalogVideoIn = new Guid(0xfb6c4283, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_VBI </summary>
		public static readonly Guid VBI = new Guid(0xfb6c4284, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_VIDEOPORT </summary>
		public static readonly Guid VideoPort = new Guid(0xfb6c4285, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_NABTS </summary>
		public static readonly Guid NABTS = new Guid(0xfb6c4286, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_EDS </summary>
		public static readonly Guid EDS = new Guid(0xfb6c4287, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_TELETEXT </summary>
		public static readonly Guid TeleText = new Guid(0xfb6c4288, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_CC </summary>
		public static readonly Guid CC = new Guid(0xfb6c4289, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_STILL </summary>
		public static readonly Guid Still = new Guid(0xfb6c428a, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_TIMECODE </summary>
		public static readonly Guid TimeCode = new Guid(0xfb6c428b, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

		/// <summary> PIN_CATEGORY_VIDEOPORT_VBI </summary>
		public static readonly Guid VideoPortVBI = new Guid(0xfb6c428c, 0x0353, 0x11d1, 0x90, 0x5f, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);

	}

	[ComVisible(false)]
	public class FindDirection
	{
		/// <summary> LOOK_UPSTREAM_ONLY </summary>
		public static readonly Guid UpstreamOnly = new Guid(0xac798be0, 0x98e3, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5);

		/// <summary> LOOK_DOWNSTREAM_ONLY </summary>
		public static readonly Guid DownstreamOnly = new Guid(0xac798be1, 0x98e3, 0x11d1, 0xb3, 0xf1, 0x0, 0xaa, 0x0, 0x37, 0x61, 0xc5);
	}


} // namespace DShowNET