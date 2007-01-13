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

namespace DirectShowLib
{
    #region Declarations

#if ALLOW_UNTESTED_INTERFACES

    /// <summary>
    /// From KSTOPOLOGY_CONNECTION
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KSTopologyConnection
    {
        public int FromNode;
        public int FromNodePin;
        public int ToNode;
        public int ToNodePin;
    }

#endif

    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [ComImport,
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("2BA1785D-4D1B-44EF-85E8-C7F1D3F20184")]
    public interface ICameraControl
    {
        [PreserveSig]
        int get_Exposure(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Exposure(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Exposure(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_Focus(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Focus(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Focus(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_Iris(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Iris(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Iris(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_Zoom(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Zoom(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Zoom(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_FocalLengths(
            out int plOcularFocalLength,
            out int plObjectiveFocalLengthMin,
            out int plObjectiveFocalLengthMax
            );

        [PreserveSig]
        int get_Pan(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Pan(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Pan(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_Tilt(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Tilt(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Tilt(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_PanTilt(
            out int pPanValue,
            out int pTiltValue,
            out int pFlags
            );

        [PreserveSig]
        int put_PanTilt(
            [In] int PanValue,
            [In] int TiltValue,
            [In] int Flags
            );

        [PreserveSig]
        int get_Roll(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_Roll(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_Roll(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_ExposureRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_ExposureRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_ExposureRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_FocusRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_FocusRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_FocusRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_IrisRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_IrisRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_IrisRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_ZoomRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_ZoomRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_ZoomRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_PanRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_PanRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int get_TiltRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_TiltRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_TiltRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_PanTiltRelative(
            out int pPanValue,
            out int pTiltValue,
            out int pFlags
            );

        [PreserveSig]
        int put_PanTiltRelative(
            [In] int PanValue,
            [In] int TiltValue,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_PanRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_RollRelative(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_RollRelative(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int getRange_RollRelative(
            out int pMin,
            out int pMax,
            out int pSteppingDelta,
            out int pDefault,
            out int pCapsFlag
            );

        [PreserveSig]
        int get_ScanMode(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_ScanMode(
            [In] int Value,
            [In] int Flags
            );

        [PreserveSig]
        int get_PrivacyMode(
            out int pValue,
            out int pFlags
            );

        [PreserveSig]
        int put_PrivacyMode(
            [In] int Value,
            [In] int Flags
            );
    }

    [ComImport,
    Guid("720D4AC0-7533-11D0-A5D6-28DB04C10000"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKsTopologyInfo
    {
        [PreserveSig]
        int get_NumCategories(
            [Out] out int pdwNumCategories
            );

        [PreserveSig]
        int get_Category(
            [In] int dwIndex,
            [Out] out Guid pCategory
            );

        [PreserveSig]
        int get_NumConnections(
            [Out] out int pdwNumConnections
            );

        [PreserveSig]
        int get_ConnectionInfo(
            [In] int dwIndex,
            [Out] out KSTopologyConnection pConnectionInfo
            );

        [PreserveSig]
        int get_NodeName(
            [In] int dwNodeId,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string pwchNodeName,
            [In] int dwBufSize,
            [Out] out int pdwNameLen
            );

        [PreserveSig]
        int get_NumNodes(
            [Out] out int pdwNumNodes
            );

        [PreserveSig]
        int get_NodeType(
            [In] int dwNodeId,
            [Out] out Guid pNodeType
            );

        [PreserveSig]
        int CreateNodeInstance( 
            [In] int dwNodeId, 
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid iid, 
            [Out, MarshalAs(UnmanagedType.IUnknown)] out Object ppvObject  
            ); 
    }

    [ComImport,
    Guid("1ABDAECA-68B6-4F83-9371-B413907C7B9F"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISelector
    {
        [PreserveSig]
        int get_NumSources([Out] out int pdwNumSources);

        [PreserveSig]
        int get_SourceNodeId([Out] out int pdwPinId);

        [PreserveSig]
        int put_SourceNodeId([In] int dwPinId);
    }

    [ComImport,
    Guid("11737C14-24A7-4bb5-81A0-0D003813B0C4"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKsNodeControl
    {
        [PreserveSig]
        int put_NodeId([In] int dwNodeId);

        [PreserveSig]
        int put_KsControl([In] IntPtr pKsControl); // PVOID
    }

#endif

    #endregion
}
