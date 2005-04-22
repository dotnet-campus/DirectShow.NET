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

#define   ALLOW_UNTESTED_STRUCTS
#define   ALLOW_UNTESTED_INTERFACES

using System;
using System.Runtime.InteropServices;

namespace DirectShowLib
{

	#region Declarations

#if ALLOW_UNTESTED_STRUCTS
	/// <summary>
	/// From _AM_PIN_FLOW_CONTROL_BLOCK_FLAGS
	/// </summary>
	[ComVisible(false), Flags]
	public enum AMPinFlowControl
	{
		Block = 0x00000001
	}

	/// <summary>
	/// From AM_GRAPH_CONFIG_RECONNECT_FLAGS
	/// </summary>
	[ComVisible(false), Flags]
	public enum AMGraphConfigReconnect
	{
		DirectConnect = 0x00000001,
		CacheRemovedFilters = 0x00000002,
		UseOnlyCachedFilters = 0x00000004
	}

	/// <summary>
	/// From _REM_FILTER_FLAGS
	/// </summary>
	[ComVisible(false), Flags]
	public enum RemFilterFlags
	{
		LeaveConnected = 0x00000001
	}

	/// <summary>
	/// From _AM_FILTER_FLAGS
	/// </summary>
	[ComVisible(false), Flags]
	public enum AMFilterFlags
	{
		Removable = 0x00000001
	}

#endif

	#endregion

	#region Interfaces

#if ALLOW_UNTESTED_INTERFACES
	//--------------------------------------------------------------------
	//
	//  IPinConnection - supported by input pins
	//
	//--------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("4a9a62d3-27d4-403d-91e9-89f540e55534"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPinConnection
	{
		[PreserveSig]
		int DynamicQueryAccept([In] ref AMMediaType pmt);

		[PreserveSig]
		int NotifyEndOfStream([In] IntPtr hNotifyEvent); // HEVENT

		[PreserveSig]
		int IsEndPin();

		[PreserveSig]
		int DynamicDisconnect();
	}

	//--------------------------------------------------------------------
	//
	//  IPinFlowControl - supported by output pins
	//
	//--------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("c56e9858-dbf3-4f6b-8119-384af2060deb"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPinFlowControl
	{
		[PreserveSig]
		int Block(
			[In] int dwBlockFlags,
			[In] IntPtr hEvent // HEVENT
			);
	}

	//--------------------------------------------------------------------
	//
	//  IGraphConfig
	//
	//--------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("03A1EB8E-32BF-4245-8502-114D08A9CB88"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGraphConfig
	{
		[PreserveSig]
		int Reconnect(
			[In] IPin pOutputPin,
			[In] IPin pInputPin,
			[In] ref AMMediaType pmtFirstConnection,
			[In] IBaseFilter pUsingFilter, // can be NULL
			[In] IntPtr hAbortEvent, // HANDLE 
			[In] AMGraphConfigReconnect dwFlags
			);

		[PreserveSig]
		int Reconfigure(
			[In] IGraphConfigCallback pCallback,
			[In] IntPtr pvContext, // PVOID
			[In] int dwFlags,
			[In] IntPtr hAbortEvent // HANDLE
			);

		[PreserveSig]
		int AddFilterToCache([In] IBaseFilter pFilter);

		[PreserveSig]
		int EnumCacheFilter([Out] out IEnumFilters pEnum);

		[PreserveSig]
		int RemoveFilterFromCache([In] IBaseFilter pFilter);

		[PreserveSig]
		int GetStartTime([Out] out long prtStart);

		[PreserveSig]
		int PushThroughData(
			[In] IPin pOutputPin,
			[In] IPinConnection pConnection,
			[In] IntPtr hEventAbort // HANDLE
			);

		[PreserveSig]
		int SetFilterFlags(
			[In] IBaseFilter pFilter,
			[In] int dwFlags
			);

		[PreserveSig]
		int GetFilterFlags(
			[In] IBaseFilter pFilter,
			[Out] out int pdwFlags
			);

		[PreserveSig]
		int RemoveFilterEx(
			[In] IBaseFilter pFilter,
			RemFilterFlags Flags
			);
	}

	//--------------------------------------------------------------------
	//
	//  IGraphConfigCallback
	//
	//--------------------------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("ade0fd60-d19d-11d2-abf6-00a0c905f375"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGraphConfigCallback
	{
		[PreserveSig]
		int Reconfigure(
			IntPtr pvContext, // PVOID
			int dwFlags
			);

	}

	[ComVisible(true), ComImport,
		Guid("DCFBDCF6-0DC2-45f5-9AB2-7C330EA09C29"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFilterChain
	{
		[PreserveSig]
		int StartChain(
			[In] IBaseFilter pStartFilter,
			[In] IBaseFilter pEndFilter
			);

		[PreserveSig]
		int PauseChain(
			[In] IBaseFilter pStartFilter,
			[In] IBaseFilter pEndFilter
			);

		[PreserveSig]
		int StopChain(
			[In] IBaseFilter pStartFilter,
			[In] IBaseFilter pEndFilter
			);

		[PreserveSig]
		int RemoveChain(
			[In] IBaseFilter pStartFilter,
			[In] IBaseFilter pEndFilter
			);
	}

#endif

	#endregion
}