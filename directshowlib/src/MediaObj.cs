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
    /// From DMO_PROCESS_OUTPUT_FLAGS
    /// </summary>
    [Flags]
    public enum DMOProcessOutput
    {
        None = 0x0,
        DiscardWhenNoBuffer = 0x00000001
    }

    /// <summary>
    /// From DMO_INPUT_DATA_BUFFER_FLAGS
    /// </summary>
    [Flags]
    public enum DMOInputDataBuffer
    {
        SyncPoint = 0x1,
        Time = 0x2,
        TimeLength = 0x4
    }

    /// <summary>
    /// From DMO_INPUT_STATUS_FLAGS
    /// </summary>
    [Flags]
    public enum DMOInputStatusFlags
    {
        None = 0x0,
        AcceptData = 0x1
    }

    /// <summary>
    /// From _DMO_SET_TYPE_FLAGS
    /// </summary>
    [Flags]
    public enum DMOSetType
    {
        None = 0x0,
        TestOnly = 0x1,
        Clear = 0x2
    }

    /// <summary>
    /// DMO_OUTPUT_STREAM_INFO_FLAGS
    /// </summary>
    [Flags]
    public enum DMOOutputStreamInfo
    {
        None = 0x0,
        WholeSamples	= 0x1,
        SingleSamplePerBuffer	= 0x2,
        FixedSampleSize	= 0x4,
        Discardable	= 0x8,
        Optional = 0x10
    }

    /// <summary>
    /// From DMO_INPUT_STREAM_INFO_FLAGS
    /// </summary>
    public enum DMOInputStreamInfo
    {
        None = 0x0,
        WholeSamples = 0x1,
        SingleSamplePerBuffer = 0x2,
        FixedSampleSize	= 0x4,
        HoldsBuffers = 0x8
    }

    /// <summary>
    /// From DMO_INPLACE_PROCESS_FLAGS
    /// </summary>
    [Flags]
    public enum DMOInplaceProcess
    {
        Normal	= 0,
        Zero	= 0x1
    }

    /// <summary>
    /// From DMO_QUALITY_STATUS_FLAGS
    /// </summary>
    [Flags]
    public enum DMOQualityStatus
    {
        None = 0x0,
        Enabled	= 0x1
    }

    /// <summary>
    /// From DMO_VIDEO_OUTPUT_STREAM_FLAGS
    /// </summary>
    [Flags]
    public enum DMOVideoOutputStream
    {
        None = 0x0,
        NeedsPreviousSample	= 0x1
    }


    /// <summary>
    /// From DMO_MEDIA_TYPE
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4), ComConversionLoss]
    public struct DMOMediaType
    {
        public Guid majortype;
        public Guid subtype;
        public int bFixedSizeSamples;
        public int bTemporalCompression;
        public int lSampleSize;
        public Guid formattype;
        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnk;
        public int cbFormat;
        public IntPtr pbFormat;
    }

    /// <summary>
    /// From DMO_OUTPUT_DATA_BUFFER
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct DMOOutputDataBuffer
    {
        [MarshalAs(UnmanagedType.Interface)]
        public IMediaBuffer pBuffer;
        public int dwStatus;
        public long rtTimestamp;
        public long rtTimelength;
    }

#endif
    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [Guid("65ABEA96-CF36-453F-AF8A-705E98F16260"), 
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDMOQualityControl
    {
        [PreserveSig]
        int SetNow(
            [In] long rtNow
            );

        [PreserveSig]
        int SetStatus(
            [In] DMOQualityStatus dwFlags
            );

        [PreserveSig]
        int GetStatus(
            out DMOQualityStatus pdwFlags
            );
    }


    [Guid("BE8F4F4E-5B16-4D29-B350-7F6B5D9298AC"), 
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDMOVideoOutputOptimizations
    {
        [PreserveSig]
        int QueryOperationModePreferences(
            int ulOutputStreamIndex, 
            out DMOVideoOutputStream pdwRequestedCapabilities
            );

        [PreserveSig]
        int SetOperationMode(
            int ulOutputStreamIndex, 
            DMOVideoOutputStream dwEnabledFeatures
            );

        [PreserveSig]
        int GetCurrentOperationMode(
            int ulOutputStreamIndex, 
            out DMOVideoOutputStream pdwEnabledFeatures
            );

        [PreserveSig]
        int GetCurrentSampleRequirements(
            int ulOutputStreamIndex, 
            out DMOVideoOutputStream pdwRequestedFeatures
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
    Guid("2C3CD98A-2BFA-4A53-9C27-5249BA64BA0F")]
    public interface IEnumDMO
    {
        [PreserveSig]
        int Next(int cItemsToFetch, 
            [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.Struct)]
            Guid[] pCLSID, 
            [Out, MarshalAs(UnmanagedType.LPArray)] 
            string[] Names, 
            out int pcItemsFetched
            );

        [PreserveSig]
        int Skip(
            int cItemsToSkip
            );

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone(
            [MarshalAs(UnmanagedType.Interface)] out IEnumDMO ppEnum
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
    Guid("651B9AD0-0FC7-4AA9-9538-D89931010741")]
    public interface IMediaObjectInPlace
    {
        [PreserveSig]
        int Process(
            [In] int ulSize, 
            [In, Out] ref byte pData, 
            [In] long refTimeStart, 
            [In] DMOInplaceProcess dwFlags
            );

        [PreserveSig]
        int Clone(
            [MarshalAs(UnmanagedType.Interface)] out IMediaObjectInPlace ppMediaObject
            );

        [PreserveSig]
        int GetLatency(
            out long pLatencyTime
            );
    }


    [Guid("59EFF8B9-938C-4A26-82F2-95CB84CDC837"), 
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaBuffer
    {
        [PreserveSig]
        int SetLength(
            int cbLength
            );

        [PreserveSig]
        int GetMaxLength(
            out int pcbMaxLength
            );

        [PreserveSig]
        int GetBufferAndLength(
            [Out] IntPtr ppBuffer, 
            out int pcbLength
            );
    }


    [Guid("D8AD0F58-5494-4102-97C5-EC798E59BCF4"), 
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMediaObject
    {
        [PreserveSig]
        int GetStreamCount(
            out int pcInputStreams, 
            out int pcOutputStreams
            );

        [PreserveSig]
        int GetInputStreamInfo(
            int dwInputStreamIndex, 
            out DMOInputStreamInfo pdwFlags
            );

        [PreserveSig]
        int GetOutputStreamInfo(
            int dwOutputStreamIndex, 
            out DMOOutputStreamInfo pdwFlags
            );

        [PreserveSig]
        int GetInputType(
            int dwInputStreamIndex, 
            int dwTypeIndex, 
            out DMOMediaType pmt
            );

        [PreserveSig]
        int GetOutputType(
            int dwOutputStreamIndex, 
            int dwTypeIndex, 
            out DMOMediaType pmt
            );

        [PreserveSig]
        int SetInputType(
            int dwInputStreamIndex, 
            [In, MarshalAs(UnmanagedType.LPStruct)] DMOMediaType pmt, 
            DMOSetType dwFlags
            );

        [PreserveSig]
        int SetOutputType(
            int dwOutputStreamIndex, 
            [In, MarshalAs(UnmanagedType.LPStruct)] DMOMediaType pmt, 
            DMOSetType dwFlags
            );

        [PreserveSig]
        int GetInputCurrentType(
            int dwInputStreamIndex, 
            out DMOMediaType pmt
            );

        [PreserveSig]
        int GetOutputCurrentType(
            int dwOutputStreamIndex, 
            out DMOMediaType pmt
            );

        [PreserveSig]
        int GetInputSizeInfo(
            int dwInputStreamIndex, 
            out int pcbSize, 
            out int pcbMaxLookahead, 
            out int pcbAlignment
            );

        [PreserveSig]
        int GetOutputSizeInfo(
            int dwOutputStreamIndex, 
            out int pcbSize, 
            out int pcbAlignment
            );

        [PreserveSig]
        int GetInputMaxLatency(
            int dwInputStreamIndex, 
            out long prtMaxLatency
            );

        [PreserveSig]
        int SetInputMaxLatency(
            int dwInputStreamIndex, 
            long rtMaxLatency
            );

        [PreserveSig]
        int Flush();

        [PreserveSig]
        int Discontinuity(
            int dwInputStreamIndex
            );

        [PreserveSig]
        int AllocateStreamingResources();

        [PreserveSig]
        int FreeStreamingResources();

        [PreserveSig]
        int GetInputStatus(
            int dwInputStreamIndex, 
            out DMOInputStatusFlags dwFlags
            );

        [PreserveSig]
        int ProcessInput(
            int dwInputStreamIndex, 
            [MarshalAs(UnmanagedType.Interface)] IMediaBuffer pBuffer, 
            DMOInputDataBuffer dwFlags, 
            long rtTimestamp, 
            long rtTimelength
            );

        [PreserveSig]
        int ProcessOutput(
            DMOProcessOutput dwFlags, 
            int cOutputBufferCount, 
            [In, Out] ref DMOOutputDataBuffer pOutputBuffers, 
            out int pdwStatus);

        [PreserveSig]
        int Lock(
            [MarshalAs(UnmanagedType.Bool)] bool bLock
            );
    }

#endif

    #endregion
}
