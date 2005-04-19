// $Id: QEdit.cs,v 1.3 2005-04-19 14:50:47 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.3 $

#region license
/* ====================================================================
 * The Apache Software License, Version 1.1
 *
 * Copyright (c) 2000 The Apache Software Foundation.  All rights
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

// QEdit
// Original work from DirectShow .NET by netmaster@swissonline.ch
// Extended streaming interfaces, ported from qedit.idl

using System;
using System.Runtime.InteropServices;

namespace DShowNET
{
	[ComVisible(true), ComImport,
		Guid("6B652FFF-11FE-4fce-92AD-0266B5D7C78F"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISampleGrabber
	{
		[PreserveSig]
		int SetOneShot(
			[In, MarshalAs(UnmanagedType.Bool)] bool OneShot);

		[PreserveSig]
		int SetMediaType(
			[In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		[PreserveSig]
		int GetConnectedMediaType(
			[Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

		[PreserveSig]
		int SetBufferSamples(
			[In, MarshalAs(UnmanagedType.Bool)] bool BufferThem);

		[PreserveSig]
		int GetCurrentBuffer(ref int pBufferSize, IntPtr pBuffer);

		[PreserveSig]
		int GetCurrentSample(IntPtr ppSample);

		[PreserveSig]
		int SetCallback(ISampleGrabberCB pCallback, int WhichMethodToCallback);
	}


	[ComVisible(true), ComImport,
		Guid("0579154A-2B53-4994-B0D0-E773148EFF85"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISampleGrabberCB
	{
		[PreserveSig]
		int SampleCB(double SampleTime, IMediaSample pSample);

		[PreserveSig]
		int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen);
	}


	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class VideoInfoHeader // VIDEOINFOHEADER
	{
		public DsRECT SrcRect;
		public DsRECT TargetRect;
		public int BitRate;
		public int BitErrorRate;
		public long AvgTimePerFrame;
		public BitmapInfoHeader BmiHeader;
	}

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class VideoInfoHeader2 // VIDEOINFOHEADER2
	{
		public DsRECT SrcRect;
		public DsRECT TargetRect;
		public int BitRate;
		public int BitErrorRate;
		public long AvgTimePerFrame;
		public int InterlaceFlags;
		public int CopyProtectFlags;
		public int PictAspectRatioX;
		public int PictAspectRatioY;
		public int ControlFlags;
		public int Reserved2;
		public BitmapInfoHeader BmiHeader;
	} ;


	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class WaveFormatEx
	{
		public short wFormatTag;
		public short nChannels;
		public int nSamplesPerSec;
		public int nAvgBytesPerSec;
		public short nBlockAlign;
		public short wBitsPerSample;
		public short cbSize;
	}

} // namespace DShowNET