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

namespace DirectShowLib.BDA
{

	#region Declarations

#if ALLOW_UNTESTED_STRUCTS
	/// <summary>
	/// From KSPROPERTY_IPSINK
	/// </summary>
	[ComVisible(false)]
	public enum KSPropertyIPSink
	{
		MULTICASTLIST,
		ADAPTER_DESCRIPTION,
		ADAPTER_ADDRESS
	}

	/// <summary>
	/// From MEDIA_SAMPLE_CONTENT
	/// </summary>
	[ComVisible(false)]
	public enum MediaSampleContent
	{
		TRANSPORT_PACKET, //  complete TS packet e.g. pass-through mode
		ELEMENTARY_STREAM, //  PES payloads; audio/video only
		MPEG2_PSI, //  PAT, PMT, CAT, Private
		TRANSPORT_PAYLOAD //  gathered TS packet payloads (PES packets, etc...)
	}

	/// <summary>
	/// From PID_MAP
	/// </summary>
	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct PIDMap
	{
		public int ulPID;
		public MediaSampleContent MediaSampleContent;
	}

#endif

	#endregion

	#region Interfaces

#if ALLOW_UNTESTED_INTERFACES
	[ComVisible(true), ComImport,
		Guid("fd501041-8ebe-11ce-8183-00aa00577da2"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_NetworkProvider
	{
		[PreserveSig]
		int PutSignalSource([In] int ulSignalSource);

		[PreserveSig]
		int GetSignalSource([Out] out int pulSignalSource);

		[PreserveSig]
		int GetNetworkType([Out] out Guid pguidNetworkType);

		[PreserveSig]
		int PutTuningSpace([In] ref Guid guidTuningSpace);

		[PreserveSig]
		int GetTuningSpace([Out] out Guid pguidTuingSpace);

		[PreserveSig]
		int RegisterDeviceFilter(
			[In, MarshalAs(UnmanagedType.Interface)] object pUnkFilterControl,
			[Out] out int ppvRegisitrationContext
			);

		[PreserveSig]
		int UnRegisterDeviceFilter([In] int pvRegistrationContext);
	}

	[ComVisible(true), ComImport,
		Guid("71985F43-1CA1-11d3-9CC8-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_EthernetFilter
	{
		///TODO : Implemente this interface
	}

	[ComVisible(true), ComImport,
		Guid("71985F44-1CA1-11d3-9CC8-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_IPV4Filter
	{
		///TODO : Implemente this interface
	}

	[ComVisible(true), ComImport,
		Guid("E1785A74-2A23-4fb3-9245-A8F88017EF33"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_IPV6Filter
	{
		///TODO : Implemente this interface
	}

	[ComVisible(true), ComImport,
		Guid("FD0A5AF3-B41D-11d2-9C95-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_DeviceControl
	{
		[PreserveSig]
		int StartChanges();

		[PreserveSig]
		int CheckChanges();

		[PreserveSig]
		int CommitChanges();

		[PreserveSig]
		int GetChangeState([Out] out int pState);
	}

	[ComVisible(true), ComImport,
		Guid("FD0A5AF3-B41D-11d2-9C95-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_PinControl
	{
		[PreserveSig]
		int GetPinID([Out] out int pulPinID);

		[PreserveSig]
		int GetPinType([Out] out int pulPinType);

		[PreserveSig]
		int RegistrationContext([Out] out int pulRegistrationCtx);
	}

	[ComVisible(true), ComImport,
		Guid("D2F1644B-B409-11d2-BC69-00A0C9EE9E16"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_SignalProperties
	{
		[PreserveSig]
		int PutNetworkType([In] ref Guid guidNetworkType);

		[PreserveSig]
		int GetNetworkType([Out] out Guid pguidNetworkType);

		[PreserveSig]
		int PutSignalSource([In] int ulSignalSource);

		[PreserveSig]
		int GetSignalSource([Out] out int pulSignalSource);

		[PreserveSig]
		int PutTuningSpace([In] ref Guid guidTuningSpace);

		[PreserveSig]
		int GetTuningSpace([Out] out Guid pguidTuingSpace);
	}

	[ComVisible(true), ComImport,
		Guid("1347D106-CF3A-428a-A5CB-AC0D9A2A4338"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_SignalStatistics
	{
		[PreserveSig]
		int put_SignalStrength([In] int lDbStrength);

		[PreserveSig]
		int get_SignalStrength([Out] out int plDbStrength);

		[PreserveSig]
		int put_SignalQuality([In] int lPercentQuality);

		[PreserveSig]
		int get_SignalQuality([Out] out int plPercentQuality);

		[PreserveSig]
		int put_SignalPresent([In, MarshalAs(UnmanagedType.U1)] bool fPresent);

		[PreserveSig]
		int get_SignalPresent([Out, MarshalAs(UnmanagedType.U1)] out bool pfPresent);

		[PreserveSig]
		int put_SignalLocked([In, MarshalAs(UnmanagedType.U1)] bool fLocked);

		[PreserveSig]
		int get_SignalLocked([Out, MarshalAs(UnmanagedType.U1)] out bool pfLocked);

		[PreserveSig]
		int put_SampleTime([In] int lmsSampleTime);

		[PreserveSig]
		int get_SampleTime([Out] out int plmsSampleTime);
	}

	[ComVisible(true), ComImport,
		Guid("79B56888-7FEA-4690-B45D-38FD3C7849BE"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_Topology
	{
		///TODO : Implemente this interface
	}

	[ComVisible(true), ComImport,
		Guid("71985F46-1CA1-11d3-9CC8-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_VoidTransform
	{
		[PreserveSig]
		int Start();

		[PreserveSig]
		int Stop();
	}

	[ComVisible(true), ComImport,
		Guid("DDF15B0D-BD25-11d2-9CA0-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_NullTransform
	{
		[PreserveSig]
		int Start();

		[PreserveSig]
		int Stop();
	}

	[ComVisible(true), ComImport,
		Guid("71985F47-1CA1-11d3-9CC8-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_FrequencyFilter
	{
		[PreserveSig]
		int put_Autotune([In] int ulTransponder);

		[PreserveSig]
		int get_Autotune([Out] out int pulTransponder);

		[PreserveSig]
		int put_Frequency([In] int ulFrequency);

		[PreserveSig]
		int get_Frequency([Out] out int pulFrequency);

		[PreserveSig]
		int put_Polarity([In] Polarisation Polarity);

		[PreserveSig]
		int get_Polarity([Out] out Polarisation pPolarity);

		[PreserveSig]
		int put_Range([In] int ulRange);

		[PreserveSig]
		int get_Range([Out] out int pulRange);

		[PreserveSig]
		int put_Bandwidth([In] int ulBandwidth);

		[PreserveSig]
		int get_Bandwidth([Out] out int pulBandwidth);

		[PreserveSig]
		int put_FrequencyMultiplier([In] int ulMultiplier);

		[PreserveSig]
		int get_FrequencyMultiplier([Out] out int pulMultiplier);
	}

	[ComVisible(true), ComImport,
		Guid("992CF102-49F9-4719-A664-C4F23E2408F4"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_LNBInfo
	{
		[PreserveSig]
		int put_LocalOscilatorFrequencyLowBand([In] int ulLOFLow);

		[PreserveSig]
		int get_LocalOscilatorFrequencyLowBand([Out] out int pulLOFLow);

		[PreserveSig]
		int put_LocalOscilatorFrequencyHighBand([In] int ulLOFHigh);

		[PreserveSig]
		int get_LocalOscilatorFrequencyHighBand([Out] out int pulLOFHigh);

		[PreserveSig]
		int put_HighLowSwitchFrequency([In] int ulSwitchFrequency);

		[PreserveSig]
		int get_HighLowSwitchFrequency([Out] out int pulSwitchFrequency);
	}

	[ComVisible(true), ComImport,
		Guid("DDF15B12-BD25-11d2-9CA0-00C04F7971E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_AutoDemodulate
	{
		[PreserveSig]
		int put_AutoDemodulate();
	}

	[ComVisible(true), ComImport,
		Guid("EF30F379-985B-4d10-B640-A79D5E04E1E0"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_DigitalDemodulator
	{
		[PreserveSig]
		int put_ModulationType([In] ref ModulationType pModulationType);

		[PreserveSig]
		int get_ModulationType([Out] out ModulationType pModulationType);

		[PreserveSig]
		int put_InnerFECMethod([In] ref FECMethod pFECMethod);

		[PreserveSig]
		int get_InnerFECMethod([Out] out FECMethod pFECMethod);

		[PreserveSig]
		int put_InnerFECRate([In] ref BinaryConvolutionCodeRate pFECRate);

		[PreserveSig]
		int get_InnerFECRate([Out] out BinaryConvolutionCodeRate pFECRate);

		[PreserveSig]
		int put_OuterFECMethod([In] ref FECMethod pFECMethod);

		[PreserveSig]
		int get_OuterFECMethod([Out] out FECMethod pFECMethod);

		[PreserveSig]
		int put_OuterFECRate([In] ref BinaryConvolutionCodeRate pFECRate);

		[PreserveSig]
		int get_OuterFECRate([Out] out BinaryConvolutionCodeRate pFECRate);

		[PreserveSig]
		int put_SymbolRate([In] ref int pSymbolRate);

		[PreserveSig]
		int get_SymbolRate([Out] out int pSymbolRate);

		[PreserveSig]
		int put_SpectralInversion([In] ref SpectralInversion pSpectralInversion);

		[PreserveSig]
		int get_SpectralInversion([Out] out SpectralInversion pSpectralInversion);
	}

	[ComVisible(true), ComImport,
		Guid("3F4DC8E2-4050-11d3-8F4B-00C04F7971E2"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_IPSinkControl
	{
		///TODO : Implemente this interface
	}

	[ComVisible(true), ComImport,
		Guid("A750108F-492E-4d51-95F7-649B23FF7AD7"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBDA_IPSinkInfo
	{
		///TODO : Implemente this interface
	}


	[ComVisible(true), ComImport,
		Guid("afb6c2a2-2c41-11d3-8a60-0000f81e0e4a"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumPIDMap
	{
		[PreserveSig]
		int Next(
			[In] int cRequest,
			[Out] out PIDMap pPIDMap,
			[Out] out int pcReceived
			);

		[PreserveSig]
		int Skip([In] int cRecords);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out] out IEnumPIDMap ppIEnumPIDMap);
	}

	[ComVisible(true), ComImport,
		Guid("afb6c2a1-2c41-11d3-8a60-0000f81e0e4a"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IMPEG2PIDMap
	{
		[PreserveSig]
		int MapPID(
			[In] int culPID,
			[In] ref int pulPID,
			[In] MediaSampleContent MediaSampleContent
			);

		[PreserveSig]
		int UnmapPID(
			[In] int culPID,
			[In] ref int pulPID
			);

		[PreserveSig]
		int EnumPIDMap([Out] out IEnumPIDMap pIEnumPIDMap);
	}

	[ComVisible(true), ComImport,
		Guid("06FB45C1-693C-4ea7-B79F-7A6A54D8DEF2"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFrequencyMap
	{
		[PreserveSig]
		int get_FrequencyMapping(
			[Out] out int ulCount,
			[Out, MarshalAs(UnmanagedType.LPArray)] out int[] ppulList
			);

		[PreserveSig]
		int put_FrequencyMapping(
			[In] int ulCount,
			[In, MarshalAs(UnmanagedType.LPArray)] int[] pList
			);

		[PreserveSig]
		int get_CountryCode([Out] out int pulCountryCode);

		[PreserveSig]
		int put_CountryCode([In] int ulCountryCode);

		[PreserveSig]
		int get_DefaultFrequencyMapping(
			[In] int ulCountryCode,
			[Out] out int pulCount,
			[Out, MarshalAs(UnmanagedType.LPArray)] out int[] ppulList);

		[PreserveSig]
		int get_CountryCodeList(
			[Out] out int pulCount,
			[Out, MarshalAs(UnmanagedType.LPArray)] out int[] ppulList);

	}
#endif

	#endregion
}