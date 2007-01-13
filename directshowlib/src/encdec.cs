#region license

/*
DirectShowLib - Provide access to DirectShow interfaces via .NET
Copyright (C) 2006
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
    /// From ProtType
    /// </summary>
    public enum ProtType
    {
        None = 0,
        Free = 1,
        Once = 2,
        Never = 3,
        NeverReally = 4,
        NoMoare = 5,
        FreeCit = 6,
        BF = 7,
        CnRecordingStop = 8,
        FreeSecure = 9,
        Invalid = 50
    }

#endif

    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [ComImport,
    Guid("C4C4C4B2-0049-4E2B-98FB-9537F6CE516D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDTFilter
    {
        [PreserveSig]
        int get_EvalRatObjOK(
            out int pHrCoCreateRetVal
            );

        [PreserveSig]
        int GetCurrRating(
            out EnTvRat_System pEnSystem,
            out EnTvRat_GenericLevel pEnRating,
            out int plbfEnAttr
            );

        [PreserveSig]
        int get_BlockedRatingAttributes(
            EnTvRat_System enSystem,
            EnTvRat_GenericLevel enLevel,
            out int plbfEnAttr
            );

        [PreserveSig]
        int put_BlockedRatingAttributes(
            EnTvRat_System enSystem,
            EnTvRat_GenericLevel enLevel,
            int lbfAttrs
            );

        [PreserveSig]
        int get_BlockUnRated(
            [MarshalAs(UnmanagedType.Bool)] out bool pfBlockUnRatedShows
            );

        [PreserveSig]
        int put_BlockUnRated(
            [MarshalAs(UnmanagedType.Bool)] bool fBlockUnRatedShows
            );

        [PreserveSig]
        int get_BlockUnRatedDelay(
            out int pmsecsDelayBeforeBlock
            );

        [PreserveSig]
        int put_BlockUnRatedDelay(
            int msecsDelayBeforeBlock
            );
    }

    [ComImport,
    Guid("C4C4C4D2-0049-4E2B-98FB-9537F6CE516D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDTFilterConfig
    {
        int GetSecureChannelObject(
            [MarshalAs(UnmanagedType.IUnknown)] out object ppUnkDRMSecureChannel
            );
    }

    [ComImport,
    Guid("C4C4C4B1-0049-4E2B-98FB-9537F6CE516D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IETFilter
    {
        int get_EvalRatObjOK(
            out int pHrCoCreateRetVal
            );

        int GetCurrRating(
            out EnTvRat_System pEnSystem,
            out EnTvRat_GenericLevel pEnRating,
            out int plbfEnAttr
            );

        int GetCurrLicenseExpDate(
            ProtType protType,
            out int lpDateTime
                );

        int GetLastErrorCode();

        int SetRecordingOn(
            [MarshalAs(UnmanagedType.Bool)] bool fRecState
            );
    }

    [ComImport,
    Guid("C4C4C4D1-0049-4E2B-98FB-9537F6CE516D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IETFilterConfig
    {
        int InitLicense(
            int LicenseId
            );

        int GetSecureChannelObject(
            out object ppUnkDRMSecureChannel
            );
    }

    [ComImport,
    Guid("C4C4C4B3-0049-4E2B-98FB-9537F6CE516D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IXDSCodec
    {
        int get_XDSToRatObjOK(
            out int pHrCoCreateRetVal
                );

        int put_CCSubstreamService(
            int SubstreamMask
            );

        int get_CCSubstreamService(
            out int pSubstreamMask
            );

        int GetContentAdvisoryRating(
            out int pRat,
            out int pPktSeqID,
            out int pCallSeqID,
            out long pTimeStart,
            out long pTimeEnd
            );

        int GetXDSPacket(
            out int pXDSClassPkt,
            out int pXDSTypePkt,
            [MarshalAs(UnmanagedType.BStr)] out string pBstrXDSPkt,
            out int pPktSeqID,
            out int pCallSeqID,
            out long pTimeStart,
            out long pTimeEnd
            );

        int GetCurrLicenseExpDate(
            ProtType protType,
            out int lpDateTime
            );

        int GetLastErrorCode();
    }

#endif

    #endregion

}
