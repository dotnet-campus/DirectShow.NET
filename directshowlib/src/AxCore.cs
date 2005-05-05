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

#define  ALLOW_UNTESTED_STRUCTS
#define  ALLOW_UNTESTED_INTERFACES

using System;
using System.Runtime.InteropServices;

namespace DirectShowLib
{

	#region Declarations

#if ALLOW_UNTESTED_STRUCTS

    /// <summary>
	/// From FILTER_STATE
	/// </summary>
	[ComVisible(false)]
	public enum FilterState
	{
		Stopped,
		Paused,
		Running
	}

	/// <summary>
	/// From AM_SAMPLE_PROPERTY_FLAGS
	/// </summary>
	[ComVisible(false), Flags] // May not be flags?
		public enum AMSamplePropertyFlags
	{
		SplicePoint = 0x01,
		PreRoll = 0x02,
		DataDiscontinuity = 0x04,
		TypeChanged = 0x08,
		TimeValid = 0x10,
		TimeDiscontinuity = 0x40,
		FlushOnPause = 0x80,
		StopValid = 0x100,
		EndOfStream = 0x200,
		Media = 0,
		Control = 1
	}

	/// <summary>
	/// From AM_GBF_* defines
	/// </summary>
	[ComVisible(false), Flags]
	public enum AMGBF
	{
		PrevFrameSkipped = 1,
		NotAsyncPoint = 2,
		NoWait = 4,
		NoDDSurfaceLock = 8
	}

	/// <summary>
	/// From AM_SEEKING_SeekingFlags
	/// </summary>
	[ComVisible(false), Flags]
	public enum AMSeekingSeekingFlags
	{
		NoPositioning = 0x00,
		AbsolutePositioning = 0x01,
		RelativePositioning = 0x02,
		IncrementalPositioning = 0x03,
		PositioningBitsMask = 0x03,
		SeekToKeyFrame = 0x04,
		ReturnTime = 0x08,
		Segment = 0x10,
		NoFlush = 0x20
	}

	/// <summary>
	/// From AM_SEEKING_SeekingCapabilities
	/// </summary>
	[ComVisible(false), Flags]
	public enum AMSeekingSeekingCapabilities
	{
		CanSeekAbsolute = 0x001,
		CanSeekForwards = 0x002,
		CanSeekBackwards = 0x004,
		CanGetCurrentPos = 0x008,
		CanGetStopPos = 0x010,
		CanGetDuration = 0x020,
		CanPlayBackwards = 0x040,
		CanDoSegments = 0x080,
		Source = 0x100
	}

	/// <summary>
	/// From ALLOCATOR_PROPERTIES
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct AllocatorProperties
	{
		public int cBuffers;
		public int cbBuffer;
		public int cbAlign;
		public int cbPrefix;
	}

	/// <summary>
	/// From AM_SAMPLE2_PROPERTIES
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct AMSample2Properties
	{
		public int cbData;
		public int dwTypeSpecificFlags;
		public int dwSampleFlags;
		public int lActual;
		public long tStart;
		public long tStop;
		public int dwStreamId;
		[MarshalAs(UnmanagedType.LPStruct)] public AMMediaType pMediaType;
		public IntPtr pbBuffer; // BYTE *
		public int cbBuffer;
	}


	/// <summary>
	/// From FILTER_INFO
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode), ComVisible(false)]
	public class FilterInfo
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)] public string achName;
		[MarshalAs(UnmanagedType.IUnknown)] public IFilterGraph pGraph;
	}
#endif

    /// <summary>
    /// From PIN_INFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1, CharSet=CharSet.Unicode), ComVisible(false)]
    public struct PinInfo
    {
        public IBaseFilter filter;
        public PinDirection dir;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)] public string name;
    }

    /// <summary>
    /// From AM_MEDIA_TYPE - When you are done with an instance of this class,
    /// it should be released with FreeAMMediaType() to avoid leaking
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1), ComVisible(false)]
    public class AMMediaType
    {
        public Guid majorType;
        public Guid subType;
        [MarshalAs(UnmanagedType.Bool)] public bool fixedSizeSamples;
        [MarshalAs(UnmanagedType.Bool)] public bool temporalCompression;
        public int sampleSize;
        public Guid formatType;
        public IntPtr unkPtr; // IUnknown Pointer
        public int formatSize;
        public IntPtr formatPtr; // Pointer to a buff determined by formatType
    }

    /// <summary>
    /// From PIN_DIRECTION
    /// </summary>
    [ComVisible(false)]
    public enum PinDirection
    {
        Input,
        Output
    }

	#endregion

	#region Interfaces

#if ALLOW_UNTESTED_INTERFACES

	[ComVisible(true), ComImport,
		Guid("56a86892-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumPins
	{
		[PreserveSig]
		int Next(
			[In] int cPins,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] IPin[] ppPins,
			[Out] out int pcFetched
			);

		[PreserveSig]
		int Skip([In] int cPins);

		void Reset();
		void Clone([Out] out IEnumPins ppEnum);
	}


	[ComVisible(true), ComImport,
		Guid("89c31040-846b-11ce-97d3-00aa0055595a"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumMediaTypes
	{
		[PreserveSig]
		int Next(
			[In] int cMediaTypes,
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] AMMediaType[] ppMediaTypes,
			[Out] out int pcFetched
			);

		[PreserveSig]
		int Skip([In] int cMediaTypes);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out] out IEnumMediaTypes ppEnum);
	}


	[ComVisible(true), ComImport,
		Guid("56a8689f-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFilterGraph
	{
		[PreserveSig]
		int AddFilter(
			[In] IBaseFilter pFilter,
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName
			);

		[PreserveSig]
		int RemoveFilter([In] IBaseFilter pFilter);

		[PreserveSig]
		int EnumFilters([Out] out IEnumFilters ppEnum);

		[PreserveSig]
		int FindFilterByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName,
			[Out] out IBaseFilter ppFilter
			);

		[PreserveSig]
		int ConnectDirect(
			[In] IPin ppinOut,
			[In] IPin ppinIn,
			[In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
			);

		[PreserveSig]
		int Reconnect([In] IPin ppin);

		[PreserveSig]
		int Disconnect([In] IPin ppin);

		[PreserveSig]
		int SetDefaultSyncSource();
	}


	[ComVisible(true), ComImport,
		Guid("56a86893-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumFilters
	{
		/*
        [PreserveSig]
        int Next(
          [In] int cFilters,
          [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)]	IBaseFilter[]	ppFilter,
          [Out] out int	pcFetched
          );
        */

		// As written, this can only return 1 at a time.  Why is the code above
		// which looks correct commented out?
		[PreserveSig]
		int Next(
			[In] int cFilters,
			[Out] out IBaseFilter ppFilter,
			[Out] out int pcFetched
			);

		[PreserveSig]
		int Skip([In] int cFilters);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out] out IEnumFilters ppEnum);
	}


	[ComVisible(true), ComImport,
		Guid("56a86899-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaFilter : IPersist
	{
		#region IPersist Methods

		[PreserveSig]
		new int GetClassID(
			[Out] out Guid pClassID);

		#endregion

		[PreserveSig]
		int Stop();

		[PreserveSig]
		int Pause();

		[PreserveSig]
		int Run([In] long tStart);

		[PreserveSig]
		int GetState(
			[In] int dwMilliSecsTimeout,
			[Out] out FilterState filtState
			);

		[PreserveSig]
		int SetSyncSource([In] IReferenceClock pClock);

		[PreserveSig]
		int GetSyncSource([Out] out IReferenceClock pClock);
	}


	[ComVisible(true), ComImport,
		Guid("56a86895-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBaseFilter : IMediaFilter
	{
		#region "IPersist Methods"

		[PreserveSig]
		new int GetClassID(
			[Out] out Guid pClassID);

		#endregion

		#region "IMediaFilter Methods"

		[PreserveSig]
		new int Stop();

		[PreserveSig]
		new int Pause();

		[PreserveSig]
		new int Run(long tStart);

		[PreserveSig]
		new int GetState([In] int dwMilliSecsTimeout, [Out] out FilterState filtState);

		[PreserveSig]
		new int SetSyncSource([In] IReferenceClock pClock);

		[PreserveSig]
		new int GetSyncSource([Out] out IReferenceClock pClock);

		#endregion

		[PreserveSig]
		int EnumPins([Out] out IEnumPins ppEnum);

		[PreserveSig]
		int FindPin(
			[In, MarshalAs(UnmanagedType.LPWStr)] string Id,
			[Out] out IPin ppPin
			);

		[PreserveSig]
		int QueryFilterInfo([Out] FilterInfo pInfo);

		[PreserveSig]
		int JoinFilterGraph(
			[In] IFilterGraph pGraph,
			[In, MarshalAs(UnmanagedType.LPWStr)] string pName
			);

		[PreserveSig]
		int QueryVendorInfo([Out, MarshalAs(UnmanagedType.LPWStr)] out string pVendorInfo);
	}


	[ComVisible(true), ComImport,
		Guid("56a86897-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IReferenceClock
	{
		[PreserveSig]
		int GetTime([Out] out long pTime);

		[PreserveSig]
		int AdviseTime(
			[In] long baseTime,
			[In] long streamTime,
			[In] IntPtr hEvent, // System.Threading.WaitHandle?
			[Out] out int pdwAdviseCookie
			);

		[PreserveSig]
		int AdvisePeriodic(
			[In] long startTime,
			[In] long periodTime,
			[In] IntPtr hSemaphore, // System.Threading.WaitHandle?
			[Out] out int pdwAdviseCookie
			);

		[PreserveSig]
		int Unadvise([In] int dwAdviseCookie);
	}


	[ComVisible(true), ComImport,
		Guid("36b73885-c2c8-11cf-8b46-00805f6cef60"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IReferenceClock2 : IReferenceClock
	{
		#region IReferenceClock Methods

		[PreserveSig]
		new int GetTime([Out] out long pTime);

		[PreserveSig]
		new int AdviseTime(
			[In] long baseTime,
			[In] long streamTime,
			[In] IntPtr hEvent, // System.Threading.WaitHandle?
			[Out] out int pdwAdviseCookie
			);

		[PreserveSig]
		new int AdvisePeriodic(
			[In] long startTime,
			[In] long periodTime,
			[In] IntPtr hSemaphore, // System.Threading.WaitHandle?
			[Out] out int pdwAdviseCookie
			);

		[PreserveSig]
		new int Unadvise([In] int dwAdviseCookie);

		#endregion
	}


	[ComVisible(true), ComImport,
		Guid("36b73884-c2c8-11cf-8b46-00805f6cef60"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMediaSample2 : IMediaSample
	{
		#region IMediaSample Methods

		[PreserveSig]
		new int GetPointer([Out] out IntPtr ppBuffer); // BYTE **

		[PreserveSig]
		new int GetSize();

		[PreserveSig]
		new int GetTime(
			[Out] out long pTimeStart,
			[Out] out long pTimeEnd
			);

		[PreserveSig]
		new int SetTime(
			[In] long pTimeStart,
			[In] long pTimeEnd
			);

		[PreserveSig]
		new int IsSyncPoint();

		[PreserveSig]
		new int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);

		[PreserveSig]
		new int IsPreroll();

		[PreserveSig]
		new int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);

		[PreserveSig]
		new int GetActualDataLength();

		[PreserveSig]
		new int SetActualDataLength([In] int len);

		[PreserveSig]
		new int GetMediaType([Out] out AMMediaType ppMediaType);

		[PreserveSig]
		new int SetMediaType([In] AMMediaType pMediaType);

		[PreserveSig]
		new int IsDiscontinuity();

		[PreserveSig]
		new int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);

		[PreserveSig]
		new int GetMediaTime(
			[Out] out long pTimeStart,
			[Out] out long pTimeEnd
			);

		[PreserveSig]
		new int SetMediaTime(
			[In] long pTimeStart,
			[In] long pTimeEnd
			);

		#endregion

		[PreserveSig]
		int GetProperties(
			[In] int cbProperties,
			[Out] IntPtr pbProperties // BYTE *
			);

		[PreserveSig]
		int SetProperties(
			[In] int cbProperties,
			[In] IntPtr pbProperties // BYTE *
			);
	}


	[ComVisible(true), ComImport,
		Guid("56a8689c-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMemAllocator
	{
		[PreserveSig]
		int SetProperties(
			[In] AllocatorProperties pRequest,
			[Out] out AllocatorProperties pActual
			);

		[PreserveSig]
		int GetProperties([Out] AllocatorProperties pProps);

		[PreserveSig]
		int Commit();

		[PreserveSig]
		int Decommit();

		[PreserveSig]
		int GetBuffer(
			[Out] out IMediaSample ppBuffer,
			[In] long pStartTime,
			[In] long pEndTime,
			[In] AMGBF dwFlags
			);

		[PreserveSig]
		int ReleaseBuffer([In] IMediaSample pBuffer);
	}


	[ComVisible(true), ComImport,
		Guid("379a0cf0-c1de-11d2-abf5-00a0c905f375"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMemAllocatorCallbackTemp : IMemAllocator
	{
		#region IMemAllocator Methods

		[PreserveSig]
		new int SetProperties(
			[In] AllocatorProperties pRequest,
			[Out] out AllocatorProperties pActual
			);

		[PreserveSig]
		new int GetProperties([Out] AllocatorProperties pProps);

		[PreserveSig]
		new int Commit();

		[PreserveSig]
		new int Decommit();

		[PreserveSig]
		new int GetBuffer(
			[Out] out IMediaSample ppBuffer,
			[In] long pStartTime,
			[In] long pEndTime,
			[In] AMGBF dwFlags
			);

		[PreserveSig]
		new int ReleaseBuffer([In] IMediaSample pBuffer);

		#endregion

		[PreserveSig]
		int SetNotify([In] IMemAllocatorNotifyCallbackTemp pNotify);

		[PreserveSig]
		int GetFreeCount([Out] out int plBuffersFree);
	}


	[ComVisible(true), ComImport,
		Guid("92980b30-c1de-11d2-abf5-00a0c905f375"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMemAllocatorNotifyCallbackTemp
	{
		[PreserveSig]
		int NotifyRelease();
	}


	[ComVisible(true), ComImport,
		Guid("56a8689d-0ad4-11ce-b03a-0020af0ba770"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMemInputPin
	{
		[PreserveSig]
		int GetAllocator([Out] out IMemAllocator ppAllocator);

		[PreserveSig]
		int NotifyAllocator(
			[In] IMemAllocator pAllocator,
			[In, MarshalAs(UnmanagedType.Bool)] bool bReadOnly
			);

		[PreserveSig]
		int GetAllocatorRequirements([Out] out AllocatorProperties pProps);

		[PreserveSig]
		int Receive([In] IMediaSample pSample);

		[PreserveSig]
		int ReceiveMultiple(
			[In] IntPtr pSamples, // IMediaSample[]
			[In] int nSamples,
			[Out] out int nSamplesProcessed
			);

		[PreserveSig]
		int ReceiveCanBlock();
	}


	[ComVisible(true), ComImport,
		Guid("a3d8cec0-7e5a-11cf-bbc5-00805f6cef20"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IAMovieSetup
	{
		[PreserveSig]
		int Register();

		[PreserveSig]
		int Unregister();
	}

#endif

    [ComVisible(true), ComImport,
    Guid("56a86891-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPin
    {
        [PreserveSig]
        int Connect(
            [In] IPin pReceivePin,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        int ReceiveConnection(
            [In] IPin pReceivePin,
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt
            );

        [PreserveSig]
        int Disconnect();

        [PreserveSig]
        int ConnectedTo(
            [Out] out IPin ppPin);

        /// <summary>
        /// Release returned parameter with DsUtils.FreeAMMediaType
        /// </summary>
        [PreserveSig]
        int ConnectionMediaType(
            [Out, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        /// <summary>
        /// Release returned parameter with DsUtils.FreePinInfo
        /// </summary>
        [PreserveSig]
        int QueryPinInfo([Out] out PinInfo pInfo);

        [PreserveSig]
        int QueryDirection(out PinDirection pPinDir);

        [PreserveSig]
        int QueryId([Out, MarshalAs(UnmanagedType.LPWStr)] out string Id);

        [PreserveSig]
        int QueryAccept([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pmt);

        [PreserveSig]
        int EnumMediaTypes([Out] out IEnumMediaTypes ppEnum);

        [PreserveSig]
        int QueryInternalConnections(
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] IPin[] ppPins,
            [In, Out] ref int nPin
            );

        [PreserveSig]
        int EndOfStream();

        [PreserveSig]
        int BeginFlush();

        [PreserveSig]
        int EndFlush();

        [PreserveSig]
        int NewSegment(
            [In] long tStart,
            [In] long tStop,
            [In] double dRate
            );
    }


    [ComVisible(true), ComImport,
    Guid("36b73880-c2c8-11cf-8b46-00805f6cef60"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSeeking
    {
        [PreserveSig]
        int GetCapabilities([Out] out AMSeekingSeekingCapabilities pCapabilities);

        [PreserveSig]
        int CheckCapabilities([In, Out] ref AMSeekingSeekingCapabilities pCapabilities);

        [PreserveSig]
        int IsFormatSupported([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int QueryPreferredFormat([Out] out Guid pFormat);

        [PreserveSig]
        int GetTimeFormat([Out] out Guid pFormat);

        [PreserveSig]
        int IsUsingTimeFormat([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int SetTimeFormat([In, MarshalAs(UnmanagedType.LPStruct)] Guid pFormat);

        [PreserveSig]
        int GetDuration([Out] out long pDuration);

        [PreserveSig]
        int GetStopPosition([Out] out long pStop);

        [PreserveSig]
        int GetCurrentPosition([Out] out long pCurrent);

        [PreserveSig]
        int ConvertTimeFormat(
            [Out] out long pTarget,
            [In, MarshalAs(UnmanagedType.LPStruct)] GUID pTargetFormat,
            [In] long Source,
            [In, MarshalAs(UnmanagedType.LPStruct)] GUID pSourceFormat
            );

        [PreserveSig]
        int SetPositions(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] DsOptInt64 pCurrent,
            [In] AMSeekingSeekingFlags dwCurrentFlags,
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] DsOptInt64 pStop,
            [In] AMSeekingSeekingFlags dwStopFlags
            );

        [PreserveSig]
        int GetPositions(
            [Out] out long pCurrent,
            [Out] out long pStop
            );

        [PreserveSig]
        int GetAvailable(
            [Out] out long pEarliest,
            [Out] out long pLatest
            );

        [PreserveSig]
        int SetRate([In] double dRate);

        [PreserveSig]
        int GetRate([Out] out double pdRate);

        [PreserveSig]
        int GetPreroll([Out] out long pllPreroll);
    }


    [ComVisible(true), ComImport,
    Guid("56a8689a-0ad4-11ce-b03a-0020af0ba770"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaSample
    {
        [PreserveSig]
        int GetPointer([Out] out IntPtr ppBuffer); // BYTE **

        [PreserveSig]
        int GetSize();

        [PreserveSig]
        int GetTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        int SetTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsOptInt64 pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsOptInt64 pTimeEnd
            );

        [PreserveSig]
        int IsSyncPoint();

        [PreserveSig]
        int SetSyncPoint([In, MarshalAs(UnmanagedType.Bool)] bool bIsSyncPoint);

        [PreserveSig]
        int IsPreroll();

        [PreserveSig]
        int SetPreroll([In, MarshalAs(UnmanagedType.Bool)] bool bIsPreroll);

        [PreserveSig]
        int GetActualDataLength();

        [PreserveSig]
        int SetActualDataLength([In] int len);

        /// <summary>
        /// Returned object must be released with DsUtils.FreeAMMediaType()
        /// </summary>
        [PreserveSig]
        int GetMediaType([Out, MarshalAs(UnmanagedType.LPStruct)] out AMMediaType ppMediaType);

        [PreserveSig]
        int SetMediaType([In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pMediaType);

        [PreserveSig]
        int IsDiscontinuity();

        [PreserveSig]
        int SetDiscontinuity([In, MarshalAs(UnmanagedType.Bool)] bool bDiscontinuity);

        [PreserveSig]
        int GetMediaTime(
            [Out] out long pTimeStart,
            [Out] out long pTimeEnd
            );

        [PreserveSig]
        int SetMediaTime(
            [In, MarshalAs(UnmanagedType.LPStruct)] DsOptInt64 pTimeStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] DsOptInt64 pTimeEnd
            );
    }


	#endregion
}