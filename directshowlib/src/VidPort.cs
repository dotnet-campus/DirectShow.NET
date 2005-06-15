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

    /// <summary>
    /// From AMVP_MODE
    /// </summary>
    public enum AMVP_Mode
    {   
        Weave,
        BobInterleaved,
        BobNonInterleaved,
        SkipEven,
        SkipOdd
    }

    /// <summary>
    /// From AMVPSIZE
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AMVPSize
    {
        int           dwWidth;                // the width
        int           dwHeight;               // the height
    }

    /// <summary>
    /// From DDVIDEOPORTCONNECT
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DDVideoPortConnect
    {
        int dwSize;
        int  dwPortWidth;
        Guid  guidTypeID;
        int  dwFlags;
        IntPtr dwReserved1;
    }

    /// <summary>
    /// From AMVPDATAINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VPDataInfo
    {
        int           dwSize;
        int           dwMicrosecondsPerField;
        AMVPDimInfo     amvpDimInfo;
        int           dwPictAspectRatioX;
        int           dwPictAspectRatioY;
        bool            bEnableDoubleClock;
        bool            bEnableVACT;
        bool            bDataIsInterlaced;
        int            lHalfLinesOdd;
        bool            bFieldPolarityInverted;
        int           dwNumLinesInVREF;
        int            lHalfLinesEven;
        int           dwReserved1;
    }

    /// <summary>
    /// From AMVPDIMINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AMVPDimInfo
    {
        int           dwFieldWidth;
        int           dwFieldHeight;
        int           dwVBIWidth;
        int           dwVBIHeight;
        Rectangle            rcValidRegion;
    }


#endif

    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPBaseConfig
    {
        [PreserveSig]
        int GetConnectInfo(
            out int pdwNumConnectInfo,
            out DDVideoPortConnect pddVPConnectInfo
            );

        [PreserveSig]
        int SetConnectInfo(
            int dwChosenEntry
            );

        [PreserveSig]
        int GetVPDataInfo(
            out VPDataInfo pamvpDataInfo
            );

        [PreserveSig]
        int GetMaxPixelRate(
            out AMVPSize pamvpSize,
            out int pdwMaxPixelsPerSecond
            );

        [PreserveSig]
        int InformVPInputFormats(
            int dwNumFormats,
            DDPixelFormat pDDPixelFormats
            );

        [PreserveSig]
        int GetVideoFormats(
            out int pdwNumFormats,
            out DDPixelFormat pddPixelFormats
            );

        [PreserveSig]
        int SetVideoFormat(
            int dwChosenEntry
            );

        [PreserveSig]
        int SetInvertPolarity(
            );

        [PreserveSig]
        int GetOverlaySurface(
            out IntPtr ppddOverlaySurface // IDirectDrawSurface
            );

        [PreserveSig]
        int SetDirectDrawKernelHandle(
            IntPtr dwDDKernelHandle
            );

        [PreserveSig]
        int SetVideoPortID(
            int dwVideoPortID
            );

        [PreserveSig]
        int SetDDSurfaceKernelHandles(
            int cHandles,
            IntPtr rgDDKernelHandles
            );

        [PreserveSig]
        int SetSurfaceParameters(
            int dwPitch,
            int dwXOrigin,
            int dwYOrigin
            );
    }

    [Guid("BC29A660-30E3-11d0-9E69-00C04FD7C15B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPConfig : IVPBaseConfig
    {
        #region IVPBaseConfig Methods

        [PreserveSig]
        new int GetConnectInfo(
            out int pdwNumConnectInfo,
            out DDVideoPortConnect pddVPConnectInfo
            );

        [PreserveSig]
        new int SetConnectInfo(
            int dwChosenEntry
            );

        [PreserveSig]
        new int GetVPDataInfo(
            out VPDataInfo pamvpDataInfo
            );

        [PreserveSig]
        new int GetMaxPixelRate(
            out AMVPSize pamvpSize,
            out int pdwMaxPixelsPerSecond
            );

        [PreserveSig]
        new int InformVPInputFormats(
            int dwNumFormats,
            DDPixelFormat pDDPixelFormats
            );

        [PreserveSig]
        new int GetVideoFormats(
            out int pdwNumFormats,
            out DDPixelFormat pddPixelFormats
            );

        [PreserveSig]
        new int SetVideoFormat(
            int dwChosenEntry
            );

        [PreserveSig]
        new int SetInvertPolarity(
            );

        [PreserveSig]
        new int GetOverlaySurface(
            out IntPtr ppddOverlaySurface // IDirectDrawSurface
            );

        [PreserveSig]
        new int SetDirectDrawKernelHandle(
            IntPtr dwDDKernelHandle
            );

        [PreserveSig]
        new int SetVideoPortID(
            int dwVideoPortID
            );

        [PreserveSig]
        new int SetDDSurfaceKernelHandles(
            int cHandles,
            IntPtr rgDDKernelHandles
            );

        [PreserveSig]
        new int SetSurfaceParameters(
            int dwPitch,
            int dwXOrigin,
            int dwYOrigin
            );

        #endregion

        [PreserveSig]
        int IsVPDecimationAllowed(
            [MarshalAs(UnmanagedType.Bool)] out bool pbIsDecimationAllowed
            );

        [PreserveSig]
        int SetScalingFactors(
            AMVPSize pamvpSize
            );
    }

    [Guid("EC529B00-1A1F-11D1-BAD9-00609744111A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPVBIConfig : IVPBaseConfig
    {
        #region IVPBaseConfig Methods

        [PreserveSig]
        new int GetConnectInfo(
            out int pdwNumConnectInfo,
            out DDVideoPortConnect pddVPConnectInfo
            );

        [PreserveSig]
        new int SetConnectInfo(
            int dwChosenEntry
            );

        [PreserveSig]
        new int GetVPDataInfo(
            out VPDataInfo pamvpDataInfo
            );

        [PreserveSig]
        new int GetMaxPixelRate(
            out AMVPSize pamvpSize,
            out int pdwMaxPixelsPerSecond
            );

        [PreserveSig]
        new int InformVPInputFormats(
            int dwNumFormats,
            DDPixelFormat pDDPixelFormats
            );

        [PreserveSig]
        new int GetVideoFormats(
            out int pdwNumFormats,
            out DDPixelFormat pddPixelFormats
            );

        [PreserveSig]
        new int SetVideoFormat(
            int dwChosenEntry
            );

        [PreserveSig]
        new int SetInvertPolarity(
            );

        [PreserveSig]
        new int GetOverlaySurface(
            out IntPtr ppddOverlaySurface // IDirectDrawSurface
            );

        [PreserveSig]
        new int SetDirectDrawKernelHandle(
            IntPtr dwDDKernelHandle
            );

        [PreserveSig]
        new int SetVideoPortID(
            int dwVideoPortID
            );

        [PreserveSig]
        new int SetDDSurfaceKernelHandles(
            int cHandles,
            IntPtr rgDDKernelHandles
            );

        [PreserveSig]
        new int SetSurfaceParameters(
            int dwPitch,
            int dwXOrigin,
            int dwYOrigin
            );

        #endregion

    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPBaseNotify
    {
        [PreserveSig]
        int RenegotiateVPParameters();
    }

    [Guid("C76794A1-D6C5-11d0-9E69-00C04FD7C15B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPNotify : IVPBaseNotify
    {
        #region IVPBaseNotify

        [PreserveSig]
        new int RenegotiateVPParameters();

        #endregion

        [PreserveSig]
        int SetDeinterlaceMode(
            AMVP_Mode mode
            );

        [PreserveSig]
        int GetDeinterlaceMode(
            out AMVP_Mode pMode
            );
    }

    [Guid("EBF47183-8764-11d1-9E69-00C04FD7C15B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPNotify2 : IVPNotify
    {
        #region IVPBaseNotify

        [PreserveSig]
        new int RenegotiateVPParameters();

        #endregion

        #region IVPNotify Methods

        [PreserveSig]
        new int SetDeinterlaceMode(
            AMVP_Mode mode
            );

        [PreserveSig]
        new int GetDeinterlaceMode(
            out AMVP_Mode pMode
            );

        #endregion

        [PreserveSig]
        int SetVPSyncMaster(
            [MarshalAs(UnmanagedType.Bool)] bool bVPSyncMaster
            );

        [PreserveSig]
        int GetVPSyncMaster(
            [MarshalAs(UnmanagedType.Bool)] out bool pbVPSyncMaster
            );

    }

    [Guid("EC529B01-1A1F-11D1-BAD9-00609744111A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IVPVBINotify : IVPBaseNotify
    {
        #region IVPBaseNotify

        [PreserveSig]
        new int RenegotiateVPParameters();

        #endregion

    }
#endif
    #endregion
}

