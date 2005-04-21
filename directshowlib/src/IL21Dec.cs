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
using System.Runtime.InteropServices;

namespace DirectShowLib
{
    #region Declarations

#if ALLOW_UNTESTED_STRUCTS
    /// <summary>
    /// From AM_LINE21_CCLEVEL
    /// </summary>
    [ComVisible(false)]
    public enum AMLine21CCLevel
    {
        TC2 = 0,
    }

    /// <summary>
    /// From AM_LINE21_CCSERVICE
    /// </summary>
    [ComVisible(false)]
    public enum AMLine21CCService
    {
        None = 0,
        Caption1,
        Caption2,
        Text1,
        Text2,
        XDS,
        DefChannel = 10,
        Invalid
    }

    /// <summary>
    /// From AM_LINE21_CCSTATE
    /// </summary>
    [ComVisible(false)]
    public enum AMLine21CCState
    {
        Off = 0,
        On
    }

    /// <summary>
    /// From AM_LINE21_CCSTYLE
    /// </summary>
    [ComVisible(false)]
    public enum AMLine21CCStyle
    {
        None = 0,
        PopOn,
        PaintOn,
        RollUp
    }

    /// <summary>
    /// From AM_LINE21_DRAWBGMODE
    /// </summary>
    [ComVisible(false)]
    public enum AMLine21DrawBGMode
    {
        Opaque,
        Transparent
    }

#endif
    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES
    [ComVisible(true), ComImport,
    Guid("6E8D4A21-310C-11d0-B79A-00AA003767A7"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAMLine21Decoder
    {
        [PreserveSig]
        int GetDecoderLevel([Out] out AMLine21CCLevel lpLevel);

        [PreserveSig]
        int GetCurrentService([Out] out AMLine21CCService lpService);

        [PreserveSig]
        int SetCurrentService([In] AMLine21CCService Service);

        [PreserveSig]
        int GetServiceState([Out] out AMLine21CCState lpState);

        [PreserveSig]
        int SetServiceState([In] AMLine21CCState State);

        [PreserveSig]
        int GetOutputFormat([Out] out BitmapInfoHeader lpbmih);

        [PreserveSig]
        int SetOutputFormat([In] BitmapInfoHeader lpbmih); //TODO: define BitmapInfo

        [PreserveSig]
        int GetBackgroundColor([Out] out int pdwPhysColor);

        [PreserveSig]
        int SetBackgroundColor([In] int dwPhysColor);

        [PreserveSig]
        int GetRedrawAlways([Out, MarshalAs(UnmanagedType.Bool)] out bool lpbOption);

        [PreserveSig]
        int SetRedrawAlways([In, MarshalAs(UnmanagedType.Bool)] bool bOption);

        [PreserveSig]
        int GetDrawBackgroundMode([Out] out AMLine21DrawBGMode lpMode);

        [PreserveSig]
        int SetDrawBackgroundMode([In] AMLine21DrawBGMode Mode);
    }

#endif
    #endregion
}
