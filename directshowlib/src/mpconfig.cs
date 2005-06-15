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

#if ALLOW_UNTESTED_INTERFACES

    public enum AspectRatioMode
    {
        Stretched,
        LetterBox,
        Crop,
        StretchedAsPrimary
    }

    /// <summary>
    /// From DDCOLOR_* defines
    /// </summary>
    [Flags]
    public enum DDColor
    {
        Brightness =              0x00000001,
        Contrast =               0x00000002,
        Hue =                    0x00000004,
        Saturation =            0x00000008,
        Sharpness =             0x00000010,
        Gamma =                 0x00000020,
        ColorEnable =           0x00000040
    }

    /// <summary>
    /// From DDCOLORCONTROL
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DDColorControl
    {
        int  dwSize;
        DDColor  dwFlags;
        int  lBrightness;
        int  lContrast;
        int  lHue;
        int  lSaturation;
        int  lSharpness;
        int  lGamma;
        int  lColorEnable;
        int  dwReserved1;
    }
    
    #endif

    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [Guid("593CDDE1-0759-11d1-9E69-00C04FD7C15B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMixerPinConfig
    {
        [PreserveSig]
        int SetRelativePosition(
            int dwLeft,
            int dwTop,
            int dwRight,
            int dwBottom);    

        [PreserveSig]
        int GetRelativePosition(
            out int pdwLeft,
            out int pdwTop,
            out int pdwRight,
            out int pdwBottom
            );

        [PreserveSig]
        int SetZOrder(
            int dwZOrder
            );

        [PreserveSig]
        int GetZOrder(
            out int pdwZOrder
            );

        [PreserveSig]
        int SetColorKey(
            ColorKey pColorKey
            );

        [PreserveSig]
        int GetColorKey(
            out ColorKey pColorKey,
            out int pColor
            );

        [PreserveSig]
        int SetBlendingParameter(
            int dwBlendingParameter
            );

        [PreserveSig]
        int GetBlendingParameter(
            out int pdwBlendingParameter
            );

        [PreserveSig]
        int SetAspectRatioMode(
            AspectRatioMode amAspectRatioMode
            );

        [PreserveSig]
        int GetAspectRatioMode(
            out AspectRatioMode pamAspectRatioMode
            );

        [PreserveSig]
        int SetStreamTransparent(
            [In, MarshalAs(UnmanagedType.Bool)] bool bStreamTransparent
            );

        [PreserveSig]
        int GetStreamTransparent(
            [Out, MarshalAs(UnmanagedType.Bool)] out bool pbStreamTransparent
            );
    }


    [Guid("EBF47182-8764-11d1-9E69-00C04FD7C15B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMixerPinConfig2 : IMixerPinConfig
    {
        #region IMixerPinConfig Methods

        [PreserveSig]
        new int SetRelativePosition(
            int dwLeft,
            int dwTop,
            int dwRight,
            int dwBottom);    

        [PreserveSig]
        new int GetRelativePosition(
            out int pdwLeft,
            out int pdwTop,
            out int pdwRight,
            out int pdwBottom
            );

        [PreserveSig]
        new int SetZOrder(
            int dwZOrder
            );

        [PreserveSig]
        new int GetZOrder(
            out int pdwZOrder
            );

        [PreserveSig]
        new int SetColorKey(
            ColorKey pColorKey
            );

        [PreserveSig]
        new int GetColorKey(
            out ColorKey pColorKey,
            out int pColor
            );

        [PreserveSig]
        new int SetBlendingParameter(
            int dwBlendingParameter
            );

        [PreserveSig]
        new int GetBlendingParameter(
            out int pdwBlendingParameter
            );

        [PreserveSig]
        new int SetAspectRatioMode(
            AspectRatioMode amAspectRatioMode
            );

        [PreserveSig]
        new int GetAspectRatioMode(
            out AspectRatioMode pamAspectRatioMode
            );

        [PreserveSig]
        new int SetStreamTransparent(
            [In, MarshalAs(UnmanagedType.Bool)] bool bStreamTransparent
            );

        [PreserveSig]
        new int GetStreamTransparent(
            [Out, MarshalAs(UnmanagedType.Bool)] out bool pbStreamTransparent
            );

        #endregion

        [PreserveSig]
        int SetOverlaySurfaceColorControls(
            DDColorControl pColorControl
            );

        [PreserveSig]
        int GetOverlaySurfaceColorControls(
            out DDColorControl pColorControl
            );
    };
#endif

    #endregion
}
