#region license

/*
DirectShowLib - Provide access to DirectShow interfaces via .NET
Copyright (C) 2005
http://sourceforge.net/projects/directshownet/

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#endregion

using System;
using System.Runtime.InteropServices;

namespace DirectShowLib.BDA
{

	#region Declarations

#if ALLOW_UNTESTED_INTERFACES

	/// <summary>
	/// From BDA_EVENT_ID
	/// </summary>
	public enum BDAEventID
	{
		SIGNAL_LOSS = 0,
		SIGNAL_LOCK,
		DATA_START,
		DATA_STOP,
		CHANNEL_ACQUIRED,
		CHANNEL_LOST,
		CHANNEL_SOURCE_CHANGED,
		CHANNEL_ACTIVATED,
		CHANNEL_DEACTIVATED,
		SUBCHANNEL_ACQUIRED,
		SUBCHANNEL_LOST,
		SUBCHANNEL_SOURCE_CHANGED,
		SUBCHANNEL_ACTIVATED,
		SUBCHANNEL_DEACTIVATED,
		ACCESS_GRANTED,
		ACCESS_DENIED,
		OFFER_EXTENDED,
		PURCHASE_COMPLETED,
		SMART_CARD_INSERTED,
		SMART_CARD_REMOVED
	}

	/// <summary>
	/// From MPEG2StreamType
	/// </summary>
	public enum MPEG2StreamType
	{
		BDA_UNITIALIZED_MPEG2STREAMTYPE = -1,
		Reserved1 = 0x0,
		ISO_IEC_11172_2_VIDEO = Reserved1 + 1,
		ISO_IEC_13818_2_VIDEO = ISO_IEC_11172_2_VIDEO + 1,
		ISO_IEC_11172_3_AUDIO = ISO_IEC_13818_2_VIDEO + 1,
		ISO_IEC_13818_3_AUDIO = ISO_IEC_11172_3_AUDIO + 1,
		ISO_IEC_13818_1_PRIVATE_SECTION = ISO_IEC_13818_3_AUDIO + 1,
		ISO_IEC_13818_1_PES = ISO_IEC_13818_1_PRIVATE_SECTION + 1,
		ISO_IEC_13522_MHEG = ISO_IEC_13818_1_PES + 1,
		ANNEX_A_DSM_CC = ISO_IEC_13522_MHEG + 1,
		ITU_T_REC_H_222_1 = ANNEX_A_DSM_CC + 1,
		ISO_IEC_13818_6_TYPE_A = ITU_T_REC_H_222_1 + 1,
		ISO_IEC_13818_6_TYPE_B = ISO_IEC_13818_6_TYPE_A + 1,
		ISO_IEC_13818_6_TYPE_C = ISO_IEC_13818_6_TYPE_B + 1,
		ISO_IEC_13818_6_TYPE_D = ISO_IEC_13818_6_TYPE_C + 1,
		ISO_IEC_13818_1_AUXILIARY = ISO_IEC_13818_6_TYPE_D + 1,
		ISO_IEC_13818_1_RESERVED = ISO_IEC_13818_1_AUXILIARY + 1,
		USER_PRIVATE = ISO_IEC_13818_1_RESERVED + 1
	}

	/// <summary>
	/// From ATSCComponentTypeFlags
	/// </summary>
	[Flags]
	public enum ATSCComponentTypeFlags
	{
		ATSCCT_AC3 = 0x00000001
	}

	/// <summary>
	/// From BDA_TEMPLATE_CONNECTION
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct BDATemplateConnection
	{
		public int FromNodeType;
		public int FromNodePinType;
		public int ToNodeType;
		public int ToNodePinType;
	}

	/// <summary>
	/// From BDA_TEMPLATE_PIN_JOINT
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct BDATemplatePinJoint
	{
		public int uliTemplateConnection;
		public int ulcInstancesMax;
	}

	/// <summary>
	/// From KS_BDA_FRAME_INFO
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct KSBDAFrameInfo
	{
		public int ExtendedHeaderSize; // Size of this extended header
		public int dwFrameFlags; //
		public int ulEvent; //
		public int ulChannelNumber; //
		public int ulSubchannelNumber; //
		public int ulReason; //
	}

	/// <summary>
	/// From MPEG2_TRANSPORT_STRIDE
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MPEG2TransportStride
	{
		public int dwOffset;
		public int dwPacketLength;
		public int dwStride;
	}

#endif

    /// <summary>
    /// From FECMethod
    /// </summary>
    public enum FECMethod
    {
        METHOD_NOT_SET = -1,
        METHOD_NOT_DEFINED = 0,
        VITERBI = 1, // FEC is a Viterbi Binary Convolution.
        RS_204_188, // The FEC is Reed-Solomon 204/188 (outer FEC)
        MAX,
    }

    /// <summary>
    /// From BinaryConvolutionCodeRate
    /// </summary>
    public enum BinaryConvolutionCodeRate
    {
        RATE_NOT_SET = -1,
        RATE_NOT_DEFINED = 0,
        RATE_1_2 = 1, // 1/2
        RATE_2_3, // 2/3
        RATE_3_4, // 3/4
        RATE_3_5,
        RATE_4_5,
        RATE_5_6, // 5/6
        RATE_5_11,
        RATE_7_8, // 7/8
        RATE_MAX
    }

    /// <summary>
    /// From Polarisation
    /// </summary>
    public enum Polarisation
    {
        NOT_SET = -1,
        NOT_DEFINED = 0,
        LINEAR_H = 1, // Linear horizontal polarisation
        LINEAR_V, // Linear vertical polarisation
        CIRCULAR_L, // Circular left polarisation
        CIRCULAR_R, // Circular right polarisation
        MAX,
    }

    /// <summary>
    /// From SpectralInversion
    /// </summary>
    public enum SpectralInversion
    {
        NOT_SET = -1,
        NOT_DEFINED = 0,
        AUTOMATIC = 1,
        NORMAL,
        INVERTED,
        MAX
    }

    /// <summary>
    /// From ModulationType
    /// </summary>
    public enum ModulationType
    {
        MOD_NOT_SET = -1,
        MOD_NOT_DEFINED = 0,
        MOD_16QAM = 1,
        MOD_32QAM,
        MOD_64QAM,
        MOD_80QAM,
        MOD_96QAM,
        MOD_112QAM,
        MOD_128QAM,
        MOD_160QAM,
        MOD_192QAM,
        MOD_224QAM,
        MOD_256QAM,
        MOD_320QAM,
        MOD_384QAM,
        MOD_448QAM,
        MOD_512QAM,
        MOD_640QAM,
        MOD_768QAM,
        MOD_896QAM,
        MOD_1024QAM,
        MOD_QPSK,
        MOD_BPSK,
        MOD_OQPSK,
        MOD_8VSB,
        MOD_16VSB,
        MOD_ANALOG_AMPLITUDE, // std am
        MOD_ANALOG_FREQUENCY, // std fm
        MOD_MAX
    }

    /// <summary>
    /// From DVBSystemType
    /// </summary>
    public enum DVBSystemType
    {
        Cable,
        Terrestrial,
        Satellite,
    }

    /// <summary>
    /// From HierarchyAlpha
    /// </summary>
    public enum HierarchyAlpha
    {
        HALPHA_NOT_SET = -1,
        HALPHA_NOT_DEFINED = 0,
        HALPHA_1 = 1, // Hierarchy alpha is 1.
        HALPHA_2, // Hierarchy alpha is 2.
        HALPHA_4, // Hierarchy alpha is 4.
        HALPHA_MAX,
    }

    /// <summary>
    /// From GuardInterval
    /// </summary>
    public enum GuardInterval
    {
        GUARD_NOT_SET = -1,
        GUARD_NOT_DEFINED = 0,
        GUARD_1_32 = 1, // Guard interval is 1/32
        GUARD_1_16, // Guard interval is 1/16
        GUARD_1_8, // Guard interval is 1/8
        GUARD_1_4, // Guard interval is 1/4
        GUARD_MAX,
    }

    /// <summary>
    /// From TransmissionMode
    /// </summary>
    public enum TransmissionMode
    {
        MODE_NOT_SET = -1,
        MODE_NOT_DEFINED = 0,
        MODE_2K = 1, // Transmission uses 1705 carriers (use a 2K FFT)
        MODE_8K, // Transmission uses 6817 carriers (use an 8K FFT)
        MODE_MAX,
    }

    /// <summary>
    /// From ComponentStatus
    /// </summary>
    public enum ComponentStatus
    {
        Active,
        Inactive,
        Unavailable
    }

    /// <summary>
    /// From ComponentCategory
    /// </summary>
    public enum ComponentCategory
    {
        NotSet = -1,
        Other = 0,
        Video,
        Audio,
        Text,
        Data
    }

    #endregion
}