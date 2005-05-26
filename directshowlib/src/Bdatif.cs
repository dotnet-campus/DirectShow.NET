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

namespace DirectShowLib.BDA
{

	#region Declarations

#if ALLOW_UNTESTED_STRUCTS
	///TODO: A verifier !!!
	[Guid("14EB8748-1753-4393-95AE-4F7E7A87AAD6")]
	public class TIFLoad
	{
	}
#endif

	#endregion

	#region Interfaces

#if ALLOW_UNTESTED_INTERFACES

	[Guid("DFEF4A68-EE61-415f-9CCB-CD95F2F98A3A"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_TIF_REGISTRATION
	{
		[PreserveSig]
		int RegisterTIFEx(
			[In] IPin pTIFInputPin,
			[In, Out] ref int ppvRegistrationContext,
			[In, Out, MarshalAs(UnmanagedType.Interface)] ref object ppMpeg2DataControl
			);

		[PreserveSig]
		int UnregisterTIF([In] int pvRegistrationContext);
	}

	[Guid("F9BAC2F9-4149-4916-B2EF-FAA202326862"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMPEG2_TIF_CONTROL
	{
		[PreserveSig]
		int RegisterTIF(
			[In, MarshalAs(UnmanagedType.Interface)] object pUnkTIF,
			[In, Out] ref int ppvRegistrationContext
			);

		[PreserveSig]
		int UnregisterTIF([In] int pvRegistrationContext);

		[PreserveSig]
		int AddPIDs(
			[In] int ulcPIDs,
			[In] ref int pulPIDs
			);

		[PreserveSig]
		int DeletePIDs(
			[In] int ulcPIDs,
			[In] ref int pulPIDs
			);

		[PreserveSig]
		int GetPIDCount([Out] out int pulcPIDs);

		[PreserveSig]
		int GetPIDs(
			[Out] out int pulcPIDs,
			[Out] out int pulPIDs
			);
	}

	[Guid("A3B152DF-7A90-4218-AC54-9830BEE8C0B6"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ITuneRequestInfo
	{
		[PreserveSig]
		int GetLocatorData([In] ITuneRequest Request);

		[PreserveSig]
		int GetComponentData([In] ITuneRequest CurrentRequest);

		[PreserveSig]
		int CreateComponentList([In] ITuneRequest CurrentRequest);

		[PreserveSig]
		int GetNextProgram(
			[In] ITuneRequest CurrentRequest,
			[Out] out ITuneRequest TuneRequest
			);

		[PreserveSig]
		int GetPreviousProgram(
			[In] ITuneRequest CurrentRequest,
			[Out] out ITuneRequest TuneRequest
			);

		[PreserveSig]
		int GetNextLocator(
			[In] ITuneRequest CurrentRequest,
			[Out] out ITuneRequest TuneRequest
			);

		[PreserveSig]
		int GetPreviousLocator(
			[In] ITuneRequest CurrentRequest,
			[Out] out ITuneRequest TuneRequest
			);
	}

	[Guid("EFDA0C80-F395-42c3-9B3C-56B37DEC7BB7"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGuideDataEvent
	{
		[PreserveSig]
		int GuideDataAcquired();

		[PreserveSig]
		int ProgramChanged([In] object varProgramDescriptionID);

		[PreserveSig]
		int ServiceChanged([In] object varProgramDescriptionID);

		[PreserveSig]
		int ScheduleEntryChanged([In] object varProgramDescriptionID);

		[PreserveSig]
		int ProgramDeleted([In] object varProgramDescriptionID);

		[PreserveSig]
		int ServiceDeleted([In] object varProgramDescriptionID);

		[PreserveSig]
		int ScheduleDeleted([In] object varProgramDescriptionID);
	}

	[Guid("88EC5E58-BB73-41d6-99CE-66C524B8B591"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGuideDataProperty
	{
		[PreserveSig]
		int get_Name([Out, MarshalAs(UnmanagedType.BStr)] out string pbstrName);

		[PreserveSig]
		int get_Language([Out] out int idLang);

		[PreserveSig]
		int get_Value([Out] out object pvar);
	}

	[Guid("AE44423B-4571-475c-AD2C-F40A771D80EF"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumGuideDataProperties
	{
		[PreserveSig]
		int Next(
			[In] int celt,
			[Out] out IGuideDataProperty ppprop,
			[Out] out int pcelt
			);

		[PreserveSig]
		int Skip([In] int celt);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out] out IEnumGuideDataProperties ppenum);
	}

	[Guid("1993299C-CED6-4788-87A3-420067DCE0C7"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumTuneRequests
	{
		[PreserveSig]
		int Next(
			[In] int celt,
			[Out] out ITuneRequest ppprop,
			[Out] out int pcelt
			);

		[PreserveSig]
		int Skip([In] int celt);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out] out IEnumTuneRequests ppenum);
	}

	[Guid("61571138-5B01-43cd-AEAF-60B784A0BF93"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGuideData
	{
		[PreserveSig]
		int GetServices([Out] out IEnumTuneRequests ppEnumTuneRequests);

		[PreserveSig]
		int GetServiceProperties(
			[In] ITuneRequest pTuneRequest,
			[Out] out IEnumGuideDataProperties ppEnumProperties
			);

		[PreserveSig]
		int GetGuideProgramIDs([Out] out UCOMIEnumVARIANT pEnumPrograms);

		[PreserveSig]
		int GetProgramProperties(
			[In] object varProgramDescriptionID,
			[Out] out IEnumGuideDataProperties ppEnumProperties
			);

		[PreserveSig]
		int GetScheduleEntryIDs([Out] out UCOMIEnumVARIANT pEnumScheduleEntries);

		[PreserveSig]
		int GetScheduleEntryProperties(
			[In] object varScheduleEntryDescriptionID,
			[Out] out IEnumGuideDataProperties ppEnumProperties
			);
	}

	[Guid("4764ff7c-fa95-4525-af4d-d32236db9e38"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGuideDataLoader
	{
		[PreserveSig]
		int Init([In] IGuideData pGuideStore);

		[PreserveSig]
		int Terminate();
	}

#endif

	#endregion
}