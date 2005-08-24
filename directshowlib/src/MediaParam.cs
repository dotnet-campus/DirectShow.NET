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
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib.MultimediaStreaming
{
    #region Declarations

#if ALLOW_UNTESTED_INTERFACES

    /// <summary>
    /// From MP_ENVELOPE_SEGMENT
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct MPEnvelopeSegment
    {
        public long rtStart;
        public long rtEnd;
        public float valStart;
        public float valEnd;
        public MPCaps iCurve;
        public MPFlags flags;
    }

    /// <summary>
    /// From MPF_ENVLP_* defines
    /// </summary>
    [Flags]
    public enum MPFlags
    {
        Standard = 0x0,
        BeginCurrentVal = 0x1,
        BeginNeutralVal = 0x2
    }

    /// <summary>
    /// From MP_TYPE
    /// </summary>
    public enum MPType
    {
        // Fields
        BOOL = 2,
        ENUM = 3,
        FLOAT = 1,
        INT = 0,
        MAX = 4
    }

    /// <summary>
    /// From MP_CAPS_CURVE* defines
    /// </summary>
    [Flags]
    public enum MPCaps
    {
        Jump = 0x1,
        Linear = 0x2,
        Square = 0x4,
        InvSquare	= 0x8,
        Sine = 0x10
    }

    /// <summary>
    /// From MP_PARAMINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct MP_PARAMINFO
    {
        public MPType mpType;
        public MPCaps mopCaps;
        public float mpdMinValue;
        public float mpdMaxValue;
        public float mpdNeutralValue;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x20)]
        public short[] szUnitText;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x20)]
        public short[] szLabel;
    }

#endif
    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
    Guid("6D6CBB60-A223-44AA-842F-A2F06750BE6D")]
    public interface IMediaParamInfo
    {
        [PreserveSig]
        int GetParamCount(out int pdwParams);

        [PreserveSig]
        int GetParamInfo(
            [In] int dwParamIndex, 
            out MP_PARAMINFO pInfo
            );

        [PreserveSig]
        int GetParamText(
            [In] int dwParamIndex, 
            [Out] IntPtr ppwchText
            );

        [PreserveSig]
        int GetNumTimeFormats(
            out int pdwNumTimeFormats
            );

        [PreserveSig]
        int GetSupportedTimeFormat(
            [In] int dwFormatIndex, 
            out Guid pguidTimeFormat
            );

        [PreserveSig]
        int GetCurrentTimeFormat(
            out Guid pguidTimeFormat, 
            out int pTimeData
            );
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
    Guid("6D6CBB61-A223-44AA-842F-A2F06750BE6E")]
    public interface IMediaParams
    {
        [PreserveSig]
        int GetParam(
            [In] int dwParamIndex, 
            out float pValue
            );

        [PreserveSig]
        int SetParam(
            [In] int dwParamIndex, 
            [In] float value
            );

        [PreserveSig]
        int AddEnvelope(
            [In] int dwParamIndex, 
            [In] int cSegments, 
            [In, MarshalAs(UnmanagedType.LPStruct)] MPEnvelopeSegment pEnvelopeSegments);

        [PreserveSig]
        int FlushEnvelope(
            [In] int dwParamIndex, 
            [In] long refTimeStart, 
            [In] long refTimeEnd
            );

        [PreserveSig]
        int SetTimeFormat(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidTimeFormat, 
            [In] int mpTimeData
            );
    }

#endif

    #endregion
}
