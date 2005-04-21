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

#define ALLOW_UNTESTED_STRUCTS
#define ALLOW_UNTESTED_INTERFACES

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib
{
    #region Declarations

#if ALLOW_UNTESTED_STRUCTS
    /// <summary>
    /// From VIDEOINFOHEADER
    /// </summary>
    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public class VideoInfoHeader
    {
        public Rectangle	SrcRect;
        public Rectangle	TargetRect;
        public int		BitRate;
        public int		BitErrorRate;
        public long		AvgTimePerFrame;
        public BitmapInfoHeader	BmiHeader;
    }

    /// <summary>
    /// From VIDEOINFOHEADER2
    /// </summary>
    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public class VideoInfoHeader2
    {
        public Rectangle			SrcRect;
        public Rectangle			TargetRect;
        public int				BitRate;
        public int				BitErrorRate;
        public long				AvgTimePerFrame;
        public int				InterlaceFlags;
        public int				CopyProtectFlags;
        public int				PictAspectRatioX; 
        public int				PictAspectRatioY; 
        public int				ControlFlags;
        public int              Reserved2;
        public BitmapInfoHeader	BmiHeader;
    };

    /// <summary>
    /// From WAVEFORMATEX
    /// </summary>
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
#endif
    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [ComVisible(true), ComImport,
	Guid("6B652FFF-11FE-4fce-92AD-0266B5D7C78F"),
	InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
public interface ISampleGrabber
{
		[PreserveSig]
	int SetOneShot(
		[In, MarshalAs(UnmanagedType.Bool)]				bool	OneShot );

		[PreserveSig]
	int SetMediaType(
		[In, MarshalAs(UnmanagedType.LPStruct)]			AMMediaType	pmt );

		[PreserveSig]
	int GetConnectedMediaType(
		[Out, MarshalAs(UnmanagedType.LPStruct)]		AMMediaType	pmt );

		[PreserveSig]
	int SetBufferSamples(
		[In, MarshalAs(UnmanagedType.Bool)]				bool	BufferThem );

		[PreserveSig]
	int GetCurrentBuffer( ref int pBufferSize, IntPtr pBuffer ); // int *

		[PreserveSig]
	int GetCurrentSample( out IMediaSample ppSample );

		[PreserveSig]
	int SetCallback( ISampleGrabberCB pCallback, int WhichMethodToCallback );
}



	[ComVisible(true), ComImport,
	Guid("0579154A-2B53-4994-B0D0-E773148EFF85"),
	InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
public interface ISampleGrabberCB
{
        /// <summary>
        /// When called, callee must release pSample
        /// </summary>
		[PreserveSig]
	int SampleCB( double SampleTime, IMediaSample pSample );

		[PreserveSig]
	int BufferCB( double SampleTime, IntPtr pBuffer, int BufferLen );
}

#endif
    #endregion


} // namespace DShowNET
