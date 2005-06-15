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
    /// From AMDDS_* defines
    /// </summary>
    [Flags]
    public enum DirectDrawSwitches
    {
        None = 0x00,
        DCIPS = 0x01,
        PS = 0x02,
        RGBOVR = 0x04,
        YUVOVR = 0x08,
        RGBOFF = 0x10,
        YUVOFF = 0x20,
        RGBFLP = 0x40,
        YUVFLP = 0x80,
        All = 0xFF,
        YUV = (YUVOFF | YUVOVR | YUVFLP),
        RGB = (RGBOFF | RGBOVR | RGBFLP),
        Primary = (DCIPS | PS)
    }

    /// <summary>
    /// From AM_PROPERTY_FRAMESTEP
    /// </summary>
    public enum PropertyFrameStep
    {
        Step   = 0x01,
        Cancel = 0x02,
        CanStep = 0x03,
        CanStepMultiple = 0x04
    }

    /// <summary>
    /// From AM_FRAMESTEP_STEP
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FrameStepStep
    {
        public int dwFramesToStep;
    }

    /// <summary>
    /// From MPEG1VIDEOINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MPEG1VideoInfo 
    {
        public VideoInfoHeader hdr;
        public int dwStartTimeCode;        
        public int           cbSequenceHeader;
        public byte            bSequenceHeader;
    }

    /// <summary>
    /// From ANALOGVIDEOINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AnalogVideoInfo 
    {
        public Rectangle            rcSource;
        public Rectangle            rcTarget;
        public int            dwActiveWidth;
        public int            dwActiveHeight;
        public long  AvgTimePerFrame;
    }

#endif

    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [Guid("36d39eb0-dd75-11ce-bf0e-00aa0055595a"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDirectDrawVideo
    {
        [PreserveSig]
        int GetSwitches(out int pSwitches);

        [PreserveSig]
        int SetSwitches(int Switches);

        [PreserveSig]
        int GetCaps(out IntPtr pCaps); // DDCAPS

        [PreserveSig]
        int GetEmulatedCaps(out IntPtr pCaps); // DDCAPS

        [PreserveSig]
        int GetSurfaceDesc(out IntPtr pSurfaceDesc); // DDSURFACEDESC

        [PreserveSig]
        int GetFourCCCodes(out int pCount,out int pCodes);

        [PreserveSig]
        int SetDirectDraw(IntPtr pDirectDraw); // LPDIRECTDRAW

        [PreserveSig]
        int GetDirectDraw(out IntPtr ppDirectDraw); // LPDIRECTDRAW

        [PreserveSig]
        int GetSurfaceType(out int pSurfaceType);

        [PreserveSig]
        int SetDefault();

        [PreserveSig]
        int UseScanLine(int UseScanLine);

        [PreserveSig]
        int CanUseScanLine(out int UseScanLine);

        [PreserveSig]
        int UseOverlayStretch(int UseOverlayStretch);

        [PreserveSig]
        int CanUseOverlayStretch(out int UseOverlayStretch);

        [PreserveSig]
        int UseWhenFullScreen(int UseWhenFullScreen);

        [PreserveSig]
        int WillUseFullScreen(out int UseWhenFullScreen);
    }


    [Guid("1bd0ecb0-f8e2-11ce-aac6-0020af0b99a3"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IQualProp
    {
        [PreserveSig]
        int get_FramesDroppedInRenderer(out int pcFrames);

        [PreserveSig]
        int get_FramesDrawn(out int pcFramesDrawn);

        [PreserveSig]
        int get_AvgFrameRate(out int piAvgFrameRate);

        [PreserveSig]
        int get_Jitter(out int iJitter);

        [PreserveSig]
        int get_AvgSyncOffset(out int piAvg);

        [PreserveSig]
        int get_DevSyncOffset(out int piDev);

    }


    [Guid("dd1d7110-7836-11cf-bf47-00aa0055595a"),
    Obsolete("This interface has been deprecated.", false),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFullScreenVideo
    {
        [PreserveSig]
        int CountModes(out int pModes);

        [PreserveSig]
        int GetModeInfo(int Mode,out int pWidth,out int pHeight,out int pDepth);

        [PreserveSig]
        int GetCurrentMode(out int pMode);

        [PreserveSig]
        int IsModeAvailable(int Mode);

        [PreserveSig]
        int IsModeEnabled(int Mode);

        [PreserveSig]
        int SetEnabled(int Mode,int bEnabled);

        [PreserveSig]
        int GetClipFactor(out int pClipFactor);

        [PreserveSig]
        int SetClipFactor(int ClipFactor);

        [PreserveSig]
        int SetMessageDrain(IntPtr hwnd);

        [PreserveSig]
        int GetMessageDrain(out IntPtr hwnd);

        [PreserveSig]
        int SetMonitor(int Monitor);

        [PreserveSig]
        int GetMonitor(out int Monitor);

        [PreserveSig]
        int HideOnDeactivate(int Hide);

        [PreserveSig]
        int IsHideOnDeactivate();

        [PreserveSig]
        int SetCaption([MarshalAs(UnmanagedType.BStr)] string strCaption);

        [PreserveSig]
        int GetCaption([MarshalAs(UnmanagedType.BStr)] out string pstrCaption);

        [PreserveSig]
        int SetDefault();

    }


    [Guid("53479470-f1dd-11cf-bc42-00aa00ac74f6"),
    Obsolete("This interface has been deprecated.", false),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFullScreenVideoEx : IFullScreenVideo
    {
#region IFullScreenVideo methods

        [PreserveSig]
        new int CountModes(out int pModes);

        [PreserveSig]
        new int GetModeInfo(int Mode,out int pWidth,out int pHeight,out int pDepth);

        [PreserveSig]
        new int GetCurrentMode(out int pMode);

        [PreserveSig]
        new int IsModeAvailable(int Mode);

        [PreserveSig]
        new int IsModeEnabled(int Mode);

        [PreserveSig]
        new int SetEnabled(int Mode,int bEnabled);

        [PreserveSig]
        new int GetClipFactor(out int pClipFactor);

        [PreserveSig]
        new int SetClipFactor(int ClipFactor);

        [PreserveSig]
        new int SetMessageDrain(IntPtr hwnd);

        [PreserveSig]
        new int GetMessageDrain(out IntPtr hwnd);

        [PreserveSig]
        new int SetMonitor(int Monitor);

        [PreserveSig]
        new int GetMonitor(out int Monitor);

        [PreserveSig]
        new int HideOnDeactivate(int Hide);

        [PreserveSig]
        new int IsHideOnDeactivate();

        [PreserveSig]
        new int SetCaption([MarshalAs(UnmanagedType.BStr)] string strCaption);

        [PreserveSig]
        new int GetCaption([MarshalAs(UnmanagedType.BStr)] out string pstrCaption);

        [PreserveSig]
        new int SetDefault();
        #endregion

        [PreserveSig]
        int SetAcceleratorTable(IntPtr hwnd,IntPtr hAccel); // HACCEL

        [PreserveSig]
        int GetAcceleratorTable(out IntPtr phwnd,out IntPtr phAccel); // HACCEL

        [PreserveSig]
        int KeepPixelAspectRatio(int KeepAspect);

        [PreserveSig]
        int IsKeepPixelAspectRatio(out int pKeepAspect);

    }


    [Guid("61ded640-e912-11ce-a099-00aa00479a58"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBaseVideoMixer
    {
        [PreserveSig]
        int SetLeadPin(int iPin);

        [PreserveSig]
        int GetLeadPin(out int piPin);

        [PreserveSig]
        int GetInputPinCount(out int piPinCount);

        [PreserveSig]
        int IsUsingClock(out int pbValue);

        [PreserveSig]
        int SetUsingClock(int bValue);

        [PreserveSig]
        int GetClockPeriod(out int pbValue);

        [PreserveSig]
        int SetClockPeriod(int bValue);
    }

#endif
    #endregion
}