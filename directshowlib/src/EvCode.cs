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

using System.Runtime.InteropServices;

namespace DirectShowLib
{

	#region Declarations

	public enum EventCode
	{
		// EvCod.h
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
		StreamControlStopped = 0x1A, // EC_STREAM_CONTROL_STOPPED  
		StreamControlStarted = 0x1B, // EC_STREAM_CONTROL_STARTED  
		EndOfSegment = 0x1C, // EC_END_OF_SEGMENT          
		SegmentStarted = 0x1D, // EC_SEGMENT_STARTED         
		LenghtChanged = 0x1E, // EC_LENGTH_CHANGED          
		DeviceLost = 0x1f, // EC_DEVICE_LOST             
		StepComplete = 0x24, // EC_STEP_COMPLETE           
		TimeCodeAvailable = 0x30, // EC_TIMECODE_AVAILABLE      
		ExtDeviceModeChange = 0x31, // EC_EXTDEVICE_MODE_CHANGE   
		StateChange = 0x32, // EC_STATE_CHANGE            
		GraphChanged = 0x50, // EC_GRAPH_CHANGED           
		ClockUnset = 0x51, // EC_CLOCK_UNSET             
		VMRRenderDeviceSet = 0x53, // EC_VMR_RENDERDEVICE_SET    
		VMRSurfaceFlipped = 0x54, // EC_VMR_SURFACE_FLIPPED     
		VMRReconnectionFailed = 0x55, // EC_VMR_RECONNECTION_FAILED 
		PreprocessComplete = 0x56, // EC_PREPROCESS_COMPLETE     
		CodecApiEvent = 0x57, // EC_CODECAPI_EVENT          

		// DVDevCod.h
		DvdDomainChange = 0x101, // EC_DVD_DOMAIN_CHANGE
		DvdTitleChange = 0x102, // EC_DVD_TITLE_CHANGE
		DvdChapterStart = 0x103, // EC_DVD_CHAPTER_START
		DvdAudioStreamChange = 0x104, // EC_DVD_AUDIO_STREAM_CHANGE
		DvdSubPicictureStreamChange = 0x105, // EC_DVD_SUBPICTURE_STREAM_CHANGE
		DvdAngleChange = 0x106, // EC_DVD_ANGLE_CHANGE
		DvdButtonChange = 0x107, // EC_DVD_BUTTON_CHANGE
		DvdValidUopsChange = 0x108, // EC_DVD_VALID_UOPS_CHANGE
		DvdStillOn = 0x109, // EC_DVD_STILL_ON
		DvdStillOff = 0x10a, // EC_DVD_STILL_OFF
		DvdCurrentTime = 0x10b, // EC_DVD_CURRENT_TIME
		DvdError = 0x10c, // EC_DVD_ERROR
		DvdWarning = 0x10d, // EC_DVD_WARNING
		DvdChapterAutoStop = 0x10e, // EC_DVD_CHAPTER_AUTOSTOP
		DvdNoFpPgc = 0x10f, // EC_DVD_NO_FP_PGC
		DvdPlaybackRateChange = 0x110, // EC_DVD_PLAYBACK_RATE_CHANGE
		DvdParentalLevelChange = 0x111, // EC_DVD_PARENTAL_LEVEL_CHANGE
		DvdPlaybackStopped = 0x112, // EC_DVD_PLAYBACK_STOPPED
		DvdAnglesAvailable = 0x113, // EC_DVD_ANGLES_AVAILABLE
		DvdPlayPeriodAutoStop = 0x114, // EC_DVD_PLAYPERIOD_AUTOSTOP
		DvdButtonAutoActivated = 0x115, // EC_DVD_BUTTON_AUTO_ACTIVATED
		DvdCmdStart = 0x116, // EC_DVD_CMD_START
		DvdCmdEnd = 0x117, // EC_DVD_CMD_END
		DvdDiscEjected = 0x118, // EC_DVD_DISC_EJECTED
		DvdDiscInserted = 0x119, // EC_DVD_DISC_INSERTED
		DvdCurrentHmsfTime = 0x11a, // EC_DVD_CURRENT_HMSF_TIME
		DvdKaraokeMode = 0x11b, // EC_DVD_KARAOKE_MODE

		// AudEvCod.h
		SNDDEVInError = 0x200, // EC_SNDDEV_IN_ERROR 
		SNDDEVOutError = 0x201, // EC_SNDDEV_OUT_ERROR

		WMTIndexEvent = 0x0251, // EC_WMT_INDEX_EVENT
		WMTEvent = 0x0252, // EC_WMT_EVENT

		Built = 0x300, // EC_BUILT
		Unbuilt = 0x301, // EC_UNBUILT

		// Sbe.h
		StreamBufferTimeHole = 0x0326, // STREAMBUFFER_EC_TIMEHOLE
		StreamBufferStaleDataRead = 0x0327, // STREAMBUFFER_EC_STALE_DATA_READ
		StreamBufferStaleFileDeleted = 0x0328, // STREAMBUFFER_EC_STALE_FILE_DELETED
		StreamBufferContentBecomingStale = 0x0329, // STREAMBUFFER_EC_CONTENT_BECOMING_STALE
		StreamBufferWriteFailure = 0x032a, // STREAMBUFFER_EC_WRITE_FAILURE
		StreamBufferReadFailure = 0x032b, // STREAMBUFFER_EC_READ_FAILURE
		StreamBufferRateChanged = 0x032c, // STREAMBUFFER_EC_RATE_CHANGED
	}

	#endregion
}