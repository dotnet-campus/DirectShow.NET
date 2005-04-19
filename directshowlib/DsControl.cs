// $Id: DsControl.cs,v 1.3 2005-04-19 14:48:48 kawaic Exp $
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

// ---------------------------------------------------------------------------------
// basic Quartz control interfaces, ported from control.odl
// original work from DirectShow .NET by netmaster@swissonline.ch
// ---------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace DShowNET
{
	#region IMediaControl
	[ComVisible(true), ComImport,
		Guid("56a868b1-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaControl
	{
		[PreserveSig]
		int Run();

		[PreserveSig]
		int Pause();

		[PreserveSig]
		int Stop();

		[PreserveSig]
		int GetState(int msTimeout, out int pfs);

		[PreserveSig]
		int RenderFile(string strFilename);

		[PreserveSig]
		int AddSourceFilter(
			[In] string strFilename,
			[Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int get_FilterCollection(
			[Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int get_RegFilterCollection(
			[Out, MarshalAs(UnmanagedType.IDispatch)] out object ppUnk);

		[PreserveSig]
		int StopWhenReady();
	}

	#endregion

// ---------------------------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("56a868b6-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaEvent
	{
		[PreserveSig]
		int GetEventHandle(out IntPtr hEvent);

		[PreserveSig]
		int GetEvent(out DsEvCode lEventCode, out int lParam1, out int lParam2, int msTimeout);

		[PreserveSig]
		int WaitForCompletion(int msTimeout, out int pEvCode);

		[PreserveSig]
		int CancelDefaultHandling(int lEvCode);

		[PreserveSig]
		int RestoreDefaultHandling(int lEvCode);

		[PreserveSig]
		int FreeEventParams(DsEvCode lEvCode, int lParam1, int lParam2);
	}


// ---------------------------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("56a868c0-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaEventEx
	{
		#region "IMediaEvent Methods"

		[PreserveSig]
		int GetEventHandle(out IntPtr hEvent);

		[PreserveSig]
		int GetEvent(out DsEvCode lEventCode, out int lParam1, out int lParam2, int msTimeout);

		[PreserveSig]
		int WaitForCompletion(int msTimeout, [Out] out int pEvCode);

		[PreserveSig]
		int CancelDefaultHandling(int lEvCode);

		[PreserveSig]
		int RestoreDefaultHandling(int lEvCode);

		[PreserveSig]
		int FreeEventParams(DsEvCode lEvCode, int lParam1, int lParam2);

		#endregion

		[PreserveSig]
		int SetNotifyWindow(IntPtr hwnd, int lMsg, IntPtr lInstanceData);

		[PreserveSig]
		int SetNotifyFlags(int lNoNotifyFlags);

		[PreserveSig]
		int GetNotifyFlags(out int lplNoNotifyFlags);
	}


// ---------------------------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("329bb360-f6ea-11d1-9038-00a0c9697298"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBasicVideo2
	{
		[PreserveSig]
		int AvgTimePerFrame(out double pAvgTimePerFrame);

		[PreserveSig]
		int BitRate(out int pBitRate);

		[PreserveSig]
		int BitErrorRate(out int pBitRate);

		[PreserveSig]
		int VideoWidth(out int pVideoWidth);

		[PreserveSig]
		int VideoHeight(out int pVideoHeight);


		[PreserveSig]
		int put_SourceLeft(int SourceLeft);

		[PreserveSig]
		int get_SourceLeft(out int pSourceLeft);

		[PreserveSig]
		int put_SourceWidth(int SourceWidth);

		[PreserveSig]
		int get_SourceWidth(out int pSourceWidth);

		[PreserveSig]
		int put_SourceTop(int SourceTop);

		[PreserveSig]
		int get_SourceTop(out int pSourceTop);

		[PreserveSig]
		int put_SourceHeight(int SourceHeight);

		[PreserveSig]
		int get_SourceHeight(out int pSourceHeight);


		[PreserveSig]
		int put_DestinationLeft(int DestinationLeft);

		[PreserveSig]
		int get_DestinationLeft(out int pDestinationLeft);

		[PreserveSig]
		int put_DestinationWidth(int DestinationWidth);

		[PreserveSig]
		int get_DestinationWidth(out int pDestinationWidth);

		[PreserveSig]
		int put_DestinationTop(int DestinationTop);

		[PreserveSig]
		int get_DestinationTop(out int pDestinationTop);

		[PreserveSig]
		int put_DestinationHeight(int DestinationHeight);

		[PreserveSig]
		int get_DestinationHeight(out int pDestinationHeight);

		[PreserveSig]
		int SetSourcePosition(int left, int top, int width, int height);

		[PreserveSig]
		int GetSourcePosition(out int left, out int top, out int width, out int height);

		[PreserveSig]
		int SetDefaultSourcePosition();


		[PreserveSig]
		int SetDestinationPosition(int left, int top, int width, int height);

		[PreserveSig]
		int GetDestinationPosition(out int left, out int top, out int width, out int height);

		[PreserveSig]
		int SetDefaultDestinationPosition();


		[PreserveSig]
		int GetVideoSize(out int pWidth, out int pHeight);

		[PreserveSig]
		int GetVideoPaletteEntries(int StartIndex, int Entries, out int pRetrieved, IntPtr pPalette);

		[PreserveSig]
		int GetCurrentImage(ref int pBufferSize, IntPtr pDIBImage);

		[PreserveSig]
		int IsUsingDefaultSource();

		[PreserveSig]
		int IsUsingDefaultDestination();

		[PreserveSig]
		int GetPreferredAspectRatio(out int plAspectX, out int plAspectY);
	}


	[ComVisible(true), ComImport,
		Guid("56a868b4-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IVideoWindow
	{
		[PreserveSig]
		int put_Caption(string caption);

		[PreserveSig]
		int get_Caption([Out] out string caption);

		[PreserveSig]
		int put_WindowStyle(int windowStyle);

		[PreserveSig]
		int get_WindowStyle(out int windowStyle);

		[PreserveSig]
		int put_WindowStyleEx(int windowStyleEx);

		[PreserveSig]
		int get_WindowStyleEx(out int windowStyleEx);

		[PreserveSig]
		int put_AutoShow(int autoShow);

		[PreserveSig]
		int get_AutoShow(out int autoShow);

		[PreserveSig]
		int put_WindowState(int windowState);

		[PreserveSig]
		int get_WindowState(out int windowState);

		[PreserveSig]
		int put_BackgroundPalette(int backgroundPalette);

		[PreserveSig]
		int get_BackgroundPalette(out int backgroundPalette);

		[PreserveSig]
		int put_Visible(int visible);

		[PreserveSig]
		int get_Visible(out int visible);

		[PreserveSig]
		int put_Left(int left);

		[PreserveSig]
		int get_Left(out int left);

		[PreserveSig]
		int put_Width(int width);

		[PreserveSig]
		int get_Width(out int width);

		[PreserveSig]
		int put_Top(int top);

		[PreserveSig]
		int get_Top(out int top);

		[PreserveSig]
		int put_Height(int height);

		[PreserveSig]
		int get_Height(out int height);

		[PreserveSig]
		int put_Owner(IntPtr owner);

		[PreserveSig]
		int get_Owner(out IntPtr owner);

		[PreserveSig]
		int put_MessageDrain(IntPtr drain);

		[PreserveSig]
		int get_MessageDrain(out IntPtr drain);

		[PreserveSig]
		int get_BorderColor(out int color);

		[PreserveSig]
		int put_BorderColor(int color);

		[PreserveSig]
		int get_FullScreenMode(out int fullScreenMode);

		[PreserveSig]
		int put_FullScreenMode(int fullScreenMode);

		[PreserveSig]
		int SetWindowForeground(int focus);

		[PreserveSig]
		int NotifyOwnerMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

		[PreserveSig]
		int SetWindowPosition(int left, int top, int width, int height);

		[PreserveSig]
		int GetWindowPosition(out int left, out int top, out int width, out int height);

		[PreserveSig]
		int GetMinIdealImageSize(out int width, out int height);

		[PreserveSig]
		int GetMaxIdealImageSize(out int width, out int height);

		[PreserveSig]
		int GetRestorePosition(out int left, out int top, out int width, out int height);

		[PreserveSig]
		int HideCursor(int hideCursor);

		[PreserveSig]
		int IsCursorHidden(out int hideCursor);

	}


// ---------------------------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("56a868b2-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IMediaPosition
	{
		[PreserveSig]
		int get_Duration(out double pLength);

		[PreserveSig]
		int put_CurrentPosition(double llTime);

		[PreserveSig]
		int get_CurrentPosition(out double pllTime);

		[PreserveSig]
		int get_StopTime(out double pllTime);

		[PreserveSig]
		int put_StopTime(double llTime);

		[PreserveSig]
		int get_PrerollTime(out double pllTime);

		[PreserveSig]
		int put_PrerollTime(double llTime);

		[PreserveSig]
		int put_Rate(double dRate);

		[PreserveSig]
		int get_Rate(out double pdRate);

		[PreserveSig]
		int CanSeekForward(out int pCanSeekForward);

		[PreserveSig]
		int CanSeekBackward(out int pCanSeekBackward);
	}


// ---------------------------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("56a868b3-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBasicAudio
	{
		[PreserveSig]
		int put_Volume(int lVolume);

		[PreserveSig]
		int get_Volume(out int plVolume);

		[PreserveSig]
		int put_Balance(int lBalance);

		[PreserveSig]
		int get_Balance(out int plBalance);
	}


// ---------------------------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("56a868b9-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IAMCollection
	{
		[PreserveSig]
		int get_Count(out int plCount);

		[PreserveSig]
		int Item(int lItem,
		         [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

		[PreserveSig]
		int get_NewEnum(
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
	}


	public enum DsEvCode
	{
		None,
		Complete = 0x01, // EC_COMPLETE
		UserAbort = 0x02, // EC_USERABORT
		ErrorAbort = 0x03, // EC_ERRORABORT
		Time = 0x04, // EC_TIME
		Repaint = 0x05, // EC_REPAINT
		StErrStopped = 0x06, // EC_STREAM_ERROR_STOPPED
		StErrStPlaying = 0x07, // EC_STREAM_ERROR_STILLPLAYING
		ErrorStPlaying = 0x08, // EC_ERROR_STILLPLAYING
		PaletteChanged = 0x09, // EC_PALETTE_CHANGED
		VideoSizeChanged = 0x0a, // EC_VIDEO_SIZE_CHANGED
		QualityChange = 0x0b, // EC_QUALITY_CHANGE
		ShuttingDown = 0x0c, // EC_SHUTTING_DOWN
		ClockChanged = 0x0d, // EC_CLOCK_CHANGED
		Paused = 0x0e, // EC_PAUSED
		OpeningFile = 0x10, // EC_OPENING_FILE
		BufferingData = 0x11, // EC_BUFFERING_DATA
		FullScreenLost = 0x12, // EC_FULLSCREEN_LOST
		Activate = 0x13, // EC_ACTIVATE
		NeedRestart = 0x14, // EC_NEED_RESTART
		WindowDestroyed = 0x15, // EC_WINDOW_DESTROYED
		DisplayChanged = 0x16, // EC_DISPLAY_CHANGED
		Starvation = 0x17, // EC_STARVATION
		OleEvent = 0x18, // EC_OLE_EVENT
		NotifyWindow = 0x19, // EC_NOTIFY_WINDOW
		// EC_ ....

		// DVDevCod.h
		DvdDomChange = 0x101, // EC_DVD_DOMAIN_CHANGE
		DvdTitleChange = 0x102, // EC_DVD_TITLE_CHANGE
		DvdChaptStart = 0x103, // EC_DVD_CHAPTER_START
		DvdAudioStChange = 0x104, // EC_DVD_AUDIO_STREAM_CHANGE

		DvdSubPicStChange = 0x105, // EC_DVD_SUBPICTURE_STREAM_CHANGE
		DvdAngleChange = 0x106, // EC_DVD_ANGLE_CHANGE
		DvdButtonChange = 0x107, // EC_DVD_BUTTON_CHANGE
		DvdValidUopsChange = 0x108, // EC_DVD_VALID_UOPS_CHANGE
		DvdStillOn = 0x109, // EC_DVD_STILL_ON
		DvdStillOff = 0x10a, // EC_DVD_STILL_OFF
		DvdCurrentTime = 0x10b, // EC_DVD_CURRENT_TIME
		DvdError = 0x10c, // EC_DVD_ERROR
		DvdWarning = 0x10d, // EC_DVD_WARNING
		DvdChaptAutoStop = 0x10e, // EC_DVD_CHAPTER_AUTOSTOP
		DvdNoFpPgc = 0x10f, // EC_DVD_NO_FP_PGC
		DvdPlaybRateChange = 0x110, // EC_DVD_PLAYBACK_RATE_CHANGE
		DvdParentalLChange = 0x111, // EC_DVD_PARENTAL_LEVEL_CHANGE
		DvdPlaybStopped = 0x112, // EC_DVD_PLAYBACK_STOPPED
		DvdAnglesAvail = 0x113, // EC_DVD_ANGLES_AVAILABLE
		DvdPeriodAStop = 0x114, // EC_DVD_PLAYPERIOD_AUTOSTOP
		DvdButtonAActivated = 0x115, // EC_DVD_BUTTON_AUTO_ACTIVATED
		DvdCmdStart = 0x116, // EC_DVD_CMD_START
		DvdCmdEnd = 0x117, // EC_DVD_CMD_END
		DvdDiscEjected = 0x118, // EC_DVD_DISC_EJECTED
		DvdDiscInserted = 0x119, // EC_DVD_DISC_INSERTED
		DvdCurrentHmsfTime = 0x11a, // EC_DVD_CURRENT_HMSF_TIME
		DvdKaraokeMode = 0x11b // EC_DVD_KARAOKE_MODE
	}


} // namespace DShowNET