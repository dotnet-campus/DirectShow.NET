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
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib
{

	#region Declarations

#if ALLOW_UNTESTED_STRUCTS

	/// <summary>
	/// From MIXER_DATA_* defines
	/// </summary>
	[Flags]
	public enum MixerData
	{
		AspectRatio = 0x00000001, // picture aspect ratio changed
		NativeSize = 0x00000002, // native size of video changed
		Palette = 0x00000004 // palette of video changed
	}

	/// <summary>
	/// #define MIXER_STATE_* defines
	/// </summary>
	public enum MixerState
	{
		Mask = 0x00000003, // use this mask with state status bits
		Unconnected = 0x00000000, // mixer is unconnected and stopped
		ConnectedStopped = 0x00000001, // mixer is connected and stopped
		ConnectedPaused = 0x00000002, // mixer is connected and paused
		ConnectedPlaying = 0x00000003 // mixer is connected and playing
	}

#endif

	#endregion

	#region Interfaces

#if ALLOW_UNTESTED_INTERFACES

	[Guid("81A3BD31-DEE1-11d1-8508-00A0C91F9CA0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMixerOCXNotify
	{
		[PreserveSig]
		int OnInvalidateRect([In] Rectangle lpcRect);

		[PreserveSig]
		int OnStatusChange([In] int ulStatusFlags);

		[PreserveSig]
		int OnDataChange([In] int ulDataFlags);
	}

	[Guid("81A3BD32-DEE1-11d1-8508-00A0C91F9CA0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMixerOCX
	{
		[PreserveSig]
		int OnDisplayChange(
			[In] int ulBitsPerPixel,
			[In] int ulScreenWidth,
			[In] int ulScreenHeight
			);

		[PreserveSig]
		int GetAspectRatio(
			[Out] out int pdwPictAspectRatioX,
			[Out] out int pdwPictAspectRatioY
			);

		[PreserveSig]
		int GetVideoSize(
			[Out] out int pdwVideoWidth,
			[Out] out int pdwVideoHeight
			);

		[PreserveSig]
		int GetStatus([Out] out int pdwStatus);

		[PreserveSig]
		int OnDraw(
			[In] IntPtr hdcDraw, // HDC
			[In] Rectangle prcDraw
			);

		[PreserveSig]
		int SetDrawRegion(
			[In] Point lpptTopLeftSC,
			[In] Rectangle prcDrawCC,
			[In] Rectangle lprcClip
			);

		[PreserveSig]
		int Advise([In] IMixerOCXNotify pmdns);

		[PreserveSig]
		int UnAdvise();
	}
#endif

	#endregion
}