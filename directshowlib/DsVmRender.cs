// $Id: DsVmRender.cs,v 1.4 2005-04-22 20:41:58 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.4 $

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

// DsVmRender
// Original work from DirectShow .NET by brian.low@shaw.ca
// Video Mixer Renderer, ported from VmRender.idl

using System;
using System.Runtime.InteropServices;

namespace DShowNET
{
	[ComVisible(false)]
	public enum VMRMode : uint
	{
		Windowed = 0x00000001,
		Windowless = 0x00000002,
		Renderless = 0x00000004,
	}

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct RECT
	{
		private int left;
		private int top;
		private int right;
		private int bottom;
	}

	[ComVisible(true), ComImport,
		Guid("0eb1088c-4dcd-46f0-878f-39dae86a51b7"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRWindowlessControl
	{
		//
		//////////////////////////////////////////////////////////
		// Video size and position information
		//////////////////////////////////////////////////////////
		//
		int GetNativeVideoSize(
			[Out] out int lpWidth,
			[Out] out int lpHeight,
			[Out] out int lpARWidth,
			[Out] out int lpARHeight
			);

		int GetMinIdealVideoSize(
			[Out] out int lpHeight
			);

		int GetMaxIdealVideoSize(
			[Out] out int lpWidth,
			[Out] out int lpHeight
			);

		int SetVideoPosition(
			[In, MarshalAs(UnmanagedType.LPStruct)] RECT lpSRCRect,
			[In, MarshalAs(UnmanagedType.LPStruct)] RECT lpDSTRect
			);

		int GetVideoPosition(
			[Out, MarshalAs(UnmanagedType.LPStruct)] out RECT lpSRCRect,
			[Out, MarshalAs(UnmanagedType.LPStruct)] out RECT lpDSTRect
			);

		int GetAspectRatioMode([Out] out uint lpAspectRatioMode);

		int SetAspectRatioMode([In] uint AspectRatioMode);

		//
		//////////////////////////////////////////////////////////
		// Display and clipping management
		//////////////////////////////////////////////////////////
		//
		int SetVideoClippingWindow([In] IntPtr hwnd);

		int RepaintVideo(
			[In] IntPtr hwnd,
			[In] IntPtr hdc
			);

		int DisplayModeChanged();


		//
		//////////////////////////////////////////////////////////
		// GetCurrentImage
		//
		// Returns the current image being displayed.  This images
		// is returned in the form of packed Windows DIB.
		//
		// GetCurrentImage can be called at any time, also
		// the caller is responsible for free the returned memory
		// by calling CoTaskMemFree.
		//
		// Excessive use of this function will degrade video
		// playback performed.
		//////////////////////////////////////////////////////////
		//
		int GetCurrentImage([Out] out IntPtr lpDib);

		//
		//////////////////////////////////////////////////////////
		// Border Color control
		//
		// The border color is color used to fill any area of the
		// the destination rectangle that does not contain video.
		// It is typically used in two instances.  When the video
		// straddles two monitors and when the VMR is trying
		// to maintain the aspect ratio of the movies by letter
		// boxing the video to fit within the specified destination
		// rectangle. See SetAspectRatioMode above.
		//////////////////////////////////////////////////////////
		//
		int SetBorderColor([In] uint Clr);

		int GetBorderColor([Out] out uint lpClr);

		//
		//////////////////////////////////////////////////////////
		// Color key control only meaningful when the VMR is using
		// and overlay
		//////////////////////////////////////////////////////////
		//
		int SetColorKey([In] uint Clr);

		int GetColorKey([Out] out uint lpClr);

	}

	[ComVisible(true), ComImport,
		Guid("9e5530c5-7034-48b4-bb46-0b8a6efc8e36"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVMRFilterConfig
	{
		[PreserveSig]
		int SetImageCompositor([In] IntPtr lpVMRImgCompositor);

		[PreserveSig]
		int SetNumberOfStreams([In] uint dwMaxStreams);

		[PreserveSig]
		int GetNumberOfStreams([Out] out uint pdwMaxStreams);

		[PreserveSig]
		int SetRenderingPrefs([In] uint dwRenderFlags);

		[PreserveSig]
		int GetRenderingPrefs([Out] out uint pdwRenderFlags);

		[PreserveSig]
		int SetRenderingMode([In] uint Mode);

		[PreserveSig]
		int GetRenderingMode([Out] out VMRMode Mode);
	}

}