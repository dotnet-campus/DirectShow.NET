#region license

/*
WindowsMediaLib - Provide access to Windows Media interfaces via .NET
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
using System.Text;
using System.Runtime.InteropServices;
using WindowsMediaLib.Defs;

namespace WindowsMediaLib
{
    #region Declarations

    /// <summary>
    /// From unnamed enum
    /// </summary>
    [Flags]
    public enum WriteFlags
    {
        None = 0,
        CleanPoint	= 0x1,
        DisContinuity	= 0x2,
        DataLoss	= 0x4
    }

    /// <summary>
    /// From WMT_CREDENTIAL_FLAGS
    /// </summary>
    [Flags]
    public enum CredentialFlags
    {
        Save = 0x1,
        DontCache	= 0x2,
        ClearTextT	= 0x4,
        Proxy = 0x8,
        Encrypt	= 0x10
    }

    /// <summary>
    /// From WMT_RIGHTS
    /// </summary>
    [Flags]
    public enum Rights
    {
        Playback            = 0x00000001,
        CopyToNonSDMIDevice = 0x00000002,
        CopyToCD            = 0x00000008,
        CopyToSDMIDevice    = 0x00000010,
        OneTime             = 0x00000020,
        SaveStreamProtected = 0x00000040,
        SDMITrigger         = 0x00010000,
        SDMINoMoreCopies    = 0x00020000
    }

    /// <summary>
    /// From NETSOURCE_URLCREDPOLICY_SETTINGS
    /// </summary>
    public enum NetSourceURLCredPolicySettings
    {
        AnonymousOnly = 2,
        MustPromptUser = 1,
        SilentLogonOk = 0
    }


    /// <summary>
    /// From WMT_INDEXER_TYPE
    /// </summary>
    public enum IndexerType
    {
        // Fields
        FrameNumbers = 1,
        PresentationTime = 0,
        TimeCode = 2
    }


    /// <summary>
    /// From WMT_OFFSET_FORMAT
    /// </summary>
    public enum OffsetFormat
    {
        // Fields
        HundredNS = 0,
        FrameNumbers = 1,
        PlaylistOffset = 2,
        Timecode = 3
    }


    /// <summary>
    /// WMT_STORAGE_FORMAT
    /// </summary>
    public enum StorageFormat
    {
        // Fields
        MP3 = 0,
        V1 = 1
    }


    /// <summary>
    /// From WMT_TRANSPORT_TYPE
    /// </summary>
    public enum TransportType
    {
        // Fields
        Reliable = 1,
        Unreliable = 0
    }


    /// <summary>
    /// From WMT_WATERMARK_ENTRY_TYPE
    /// </summary>
    public enum WaterMarkEntryType
    {
        // Fields
        Audio = 1,
        Video = 2
    }


    /// <summary>
    /// From WM_AETYPE
    /// </summary>
    public enum AEType
    {
        Exclude = 0x65,
        Include = 0x69
    }


    /// <summary>
    /// From WMT_ATTR_DATATYPE
    /// </summary>
    public enum AttrDataType
    {
        DWORD   = 0,
        STRING  = 1,
        BINARY  = 2,
        BOOL    = 3,
        QWORD   = 4,
        WORD    = 5,
        GUID    = 6
    }


    /// <summary>
    /// From WMT_CODEC_INFO_TYPE
    /// </summary>
    public enum CodecInfoType
    {
        Audio   = 0,
        Video   = 1,
        Unknown = 0xffffff
    }


    /// <summary>
    /// From WMT_NET_PROTOCOL
    /// </summary>
    public enum NetProtocol
    {
        HTTP	= 0
    }


    /// <summary>
    /// From WMT_PLAY_MODE
    /// </summary>
    public enum PlayMode
    {
        // Fields
        AutoSelect = 0,
        Download = 2,
        Local = 1,
        Streaming = 3
    }


    /// <summary>
    /// From WMT_PROXY_SETTINGS
    /// </summary>
    public enum ProxySettings
    {
        // Fields
        Auto = 2,
        Browser = 3,
        Manual = 1,
        Max = 4,
        None = 0
    }


    /// <summary>
    /// From WMT_STATUS
    /// </summary>
    public enum Status
    {
        Error                       = 0,
        Opened                      = 1,
        BufferingStart             = 2,
        BufferingStop              = 3,
        EOF                         = 4,
        EndOfFile                 = 4,
        EndOfSegment              = 5,
        EndOfStreaming            = 6,
        Locating                    = 7,
        Connecting                  = 8,
        NoRights                   = 9,
        MissingCodec               = 10,
        Started                     = 11,
        Stopped                     = 12,
        Closed                      = 13,
        Striding                    = 14,
        Timer                       = 15,
        IndexProgress              = 16,
        SaveasStart                = 17,
        SaveasStop                = 18,
        NewSourceflags             = 19,
        NewMetadata                = 20,
        BackuprestoreBegin         = 21,
        SourceSwitch               = 22,
        AcquireLicense             = 23,
        Individualize               = 24,
        NeedsIndividualization     = 25,
        NoRightsEx                = 26,
        BackuprestoreEnd           = 27,
        BackuprestoreConnecting    = 28,
        BackuprestoreDisconnecting = 29,
        ErrorWithurl               = 30,
        RestrictedLicense          = 31,
        ClientConnect              = 32,
        ClientDisconnect           = 33,
        NativeOutputPropsChanged = 34,
        ReconnectStart             = 35,
        ReconnectEnd               = 36,
        ClientConnectEx           = 37,
        ClientDisconnectEx        = 38,
        SetFECSpan                = 39,
        PrerollReady               = 40,
        PrerollComplete            = 41,
        ClientProperties           = 42,
        LicenseURLSignatureState  = 43
    }


    /// <summary>
    /// From WMT_STREAM_SELECTION
    /// </summary>
    public enum StreamSelection
    {
        CleanPointOnly = 1,
        Off = 0,
        On = 2
    }


    /// <summary>
    /// From WMT_VERSION
    /// </summary>
    public enum WMVersion
    {
        V4_0  = 0x00040000,
        V7_0  = 0x00070000,
        V8_0  = 0x00080000,
        V9_0  = 0x00090000
    }	


    /// <summary>
    /// From WMT_WATERMARK_ENTRY
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct WaterMarkEntry
    {
        public WaterMarkEntryType wmetType;
        public Guid clsid;
        public int cbDisplayName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwszDisplayName;
    }


    /// <summary>
    /// From WM_ADDRESS_ACCESSENTRY
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct WMAddressAccessEntry
    {
        public int dwIPAddress;
        public int dwMask;
    }


    /// <summary>
    /// From WM_CLIENT_PROPERTIES
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct WMClientProperties
    {
        public int dwIPAddress;
        public int dwPort;
    }


    /// <summary>
    /// From WM_PORT_NUMBER_RANGE
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public struct WMPortNumberRange
    {
        public short wPortBegin;
        public short wPortEnd;
    }


    /// <summary>
    /// From WM_READER_CLIENTINFO
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct WMReaderClientInfo
    {
        public int cbSize;
        public string wszLang;
        public string wszBrowserUserAgent;
        public string wszBrowserWebPage;
        public long qwReserved;
        public IntPtr pReserved;
        public string wszHostExe;
        public long qwHostVersion;
        public string wszPlayerUserAgent;
    }


    /// <summary>
    /// From WM_READER_STATISTICS
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct WMReaderStatistics
    {
        public int cbSize;
        public int dwBandwidth;
        public int cPacketsReceived;
        public int cPacketsRecovered;
        public int cPacketsLost;
        public short wQuality;
    }


    /// <summary>
    /// From WM_STREAM_PRIORITY_RECORD
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public struct WMStreamPrioritizationRecord
    {
        public short wStreamNumber;
        public int fMandatory;
    }


    /// <summary>
    /// From WMT_BUFFER_SEGMENT
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct BufferSegment
    {
        public INSSBuffer pBuffer;
        public int cbOffset;
        public int cbLength;
    }


    /// <summary>
    /// From WMT_FILESINK_DATA_UNIT
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct FileSinkDataUnit
    {
        public BufferSegment packetHeaderBuffer;
        public int cPayloads;
        public IntPtr pPayloadHeaderBuffers;
        public int cPayloadDataFragments;
        public IntPtr pPayloadDataFragments;
    }


    /// <summary>
    /// From WMT_TIMECODE_EXTENSION_DATA
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public struct TimeCodeExtensionData
    {
        public short wRange;
        public int dwTimecode;
        public int dwUserbits;
        public int dwAmFlags;
    }


    /// <summary>
    /// From WM_WRITER_STATISTICS
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WriterStatistics
    {
        public long qwSampleCount;
        public long qwByteCount;
        public long qwDroppedSampleCount;
        public long qwDroppedByteCount;
        public int dwCurrentBitrate;
        public int dwAverageBitrate;
        public int dwExpectedBitrate;
        public int dwCurrentSampleRate;
        public int dwAverageSampleRate;
        public int dwExpectedSampleRate;
    }


    /// <summary>
    /// From WM_WRITER_STATISTICS_EX
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=4)]
    public struct WMWriterStatisticsEx
    {
        public int dwBitratePlusOverhead;
        public int dwCurrentSampleDropRateInQueue;
        public int dwCurrentSampleDropRateInCodec;
        public int dwCurrentSampleDropRateInMultiplexer;
        public int dwTotalSampleDropsInQueue;
        public int dwTotalSampleDropsInCodec;
        public int dwTotalSampleDropsInMultiplexer;
    }


    /// <summary>
    /// From STATSTG
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=8)]
    public struct STATSTG
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsName;
        public int Type;
        public long cbSize;
        public long mtime;
        public long ctime;
        public long atime;
        public int grfMode;
        public int grfLocksSupported;
        public Guid clsid;
        public int grfStateBits;
        public int reserved;
    }


    #endregion

    #region Interfaces

    [Guid("45086030-F7E4-486a-B504-826BB5792A3B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IConfigAsfWriter
    {
        [PreserveSig]
        int ConfigureFilterUsingProfileId([In] int dwProfileId);

        [PreserveSig]
        int GetCurrentProfileId([Out] out int pdwProfileId);

        [PreserveSig]
        int ConfigureFilterUsingProfileGuid([In, MarshalAs(UnmanagedType.LPStruct)] Guid guidProfile);

        [PreserveSig]
        int GetCurrentProfileGuid([Out] out Guid pProfileGuid);

        [PreserveSig]
        int ConfigureFilterUsingProfile([In] IWMProfile pProfile);

        [PreserveSig]
        int GetCurrentProfileGuid([Out] out IWMProfile ppProfile);

        [PreserveSig]
        int SetIndexMode([In, MarshalAs(UnmanagedType.Bool)] bool bIndexFile);

        [PreserveSig]
        int GetIndexMode([Out, MarshalAs(UnmanagedType.Bool)] out bool pbIndexFile);
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("E1CD3524-03D7-11D2-9EED-006097D2D7CF")]
    public interface INSSBuffer
    {
        [PreserveSig]
        int GetLength(
            out int pdwLength
            );

        [PreserveSig]
        int SetLength(
            [In] int dwLength
            );

        [PreserveSig]
        int GetMaxLength(
            out int pdwLength
            );

        [PreserveSig]
        int GetBuffer(
            out IntPtr ppdwBuffer
            );

        [PreserveSig]
        int GetBufferAndLength(
            out IntPtr ppdwBuffer,
            out int pdwLength
            );
    }


    [Guid("BB3C6389-1633-4E92-AF14-9F3173BA39D0"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMAddressAccess
    {
        [PreserveSig]
        int GetAccessEntryCount(
            [In] AEType aeType,
            out int pcEntries
            );

        [PreserveSig]
        int GetAccessEntry(
            [In] AEType aeType,
            [In] int dwEntryNum,
            out WMAddressAccessEntry pAddrAccessEntry
            );

        [PreserveSig]
        int AddAccessEntry(
            [In] AEType aeType,
            [In, MarshalAs(UnmanagedType.LPStruct)] WMAddressAccessEntry pAddrAccessEntry
            );

        [PreserveSig]
        int RemoveAccessEntry(
            [In] AEType aeType,
            [In] int dwEntryNum
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("65A83FC2-3E98-4D4D-81B5-2A742886B33D")]
    public interface IWMAddressAccess2 : IWMAddressAccess
    {
        #region IWMAddressAccess Methods

        [PreserveSig]
        new int GetAccessEntryCount(
            [In] AEType aeType,
            out int pcEntries
            );

        [PreserveSig]
        new int GetAccessEntry(
            [In] AEType aeType,
            [In] int dwEntryNum,
            out WMAddressAccessEntry pAddrAccessEntry
            );

        [PreserveSig]
        new int AddAccessEntry(
            [In] AEType aeType,
            [In, MarshalAs(UnmanagedType.LPStruct)] WMAddressAccessEntry pAddrAccessEntry
            );

        [PreserveSig]
        new int RemoveAccessEntry(
            [In] AEType aeType,
            [In] int dwEntryNum
            );

        #endregion

        [PreserveSig]
        int GetAccessEntryEx(
            [In] AEType aeType,
            [In] int dwEntryNum,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrAddress,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrMask
            );

        [PreserveSig]
        int AddAccessEntryEx(
            [In] AEType aeType,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrAddress,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrMask
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("3C8E0DA6-996F-4FF3-A1AF-4838F9377E2E")]
    public interface IWMBackupRestoreProps
    {
        [PreserveSig]
        int GetPropCount(
            out short pcProps
            );

        [PreserveSig]
        int GetPropByIndex(
            [In] short wIndex,
            [Out] StringBuilder pwszName,
            ref short pcchNameLen,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int GetPropByName(
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int SetProp(
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        int RemoveProp(
            [In] string pcwszName
            );

        [PreserveSig]
        int RemoveAllProps();
    }


    [Guid("AD694AF1-F8D9-42F8-BC47-70311B0C4F9E"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMBandwidthSharing : IWMStreamList
    {
        #region IWMStreamList Methods

        [PreserveSig]
        new int GetStreams(
            out short [] pwStreamNumArray,
            ref short pcStreams
            );

        [PreserveSig]
        new int AddStream(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int RemoveStream(
            [In] short wStreamNum
            );

        #endregion

        [PreserveSig]
        int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        int SetType(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType
            );

        [PreserveSig]
        int GetBandwidth(
            out int pdwBitrate,
            out int pmsBufferWindow
            );

        [PreserveSig]
        int SetBandwidth(
            [In] int dwBitrate,
            [In] int msBufferWindow
            );
    }


    [Guid("73C66010-A299-41DF-B1F0-CCF03B09C1C6"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMClientConnections
    {
        [PreserveSig]
        int GetClientCount(
            out int pcClients
            );

        [PreserveSig]
        int GetClientProperties(
            [In] int dwClientNum,
            out WMClientProperties pClientProperties
            );
    }


    [Guid("4091571E-4701-4593-BB3D-D5F5F0C74246"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMClientConnections2 : IWMClientConnections
    {
        #region IWMClientConnections Methods

        [PreserveSig]
        new int GetClientCount(
            out int pcClients
            );

        [PreserveSig]
        new int GetClientProperties(
            [In] int dwClientNum,
            out WMClientProperties pClientProperties
            );

        #endregion

        [PreserveSig]
        int GetClientInfo(
            [In] int dwClientNum,
            [Out] StringBuilder pwszNetworkAddress,
            ref int pcchNetworkAddress,
            [Out] StringBuilder pwszPort,
            ref int pcchPort,
            [Out] StringBuilder pwszDNSName,
            ref int pcchDNSName
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("A970F41E-34DE-4A98-B3BA-E4B3CA7528F0")]
    public interface IWMCodecInfo
    {
        [PreserveSig]
        int GetCodecInfoCount(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            out int pcCodecs
            );

        [PreserveSig]
        int GetCodecFormatCount(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            out int pcFormat
            );

        [PreserveSig]
        int GetCodecFormat(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] int dwFormatIndex,
            out IWMStreamConfig ppIStreamConfig
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("AA65E273-B686-4056-91EC-DD768D4DF710")]
    public interface IWMCodecInfo2 : IWMCodecInfo
    {
        #region IWMCodecInfo Methods

        [PreserveSig]
        new int GetCodecInfoCount(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            out int pcCodecs
            );

        [PreserveSig]
        new int GetCodecFormatCount(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            out int pcFormat
            );

        [PreserveSig]
        new int GetCodecFormat(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] int dwFormatIndex,
            out IWMStreamConfig ppIStreamConfig
            );

        #endregion

        [PreserveSig]
        int GetCodecName(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [Out] StringBuilder wszName,
            ref int pcchName
            );

        [PreserveSig]
        int GetCodecFormatDesc(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] int dwFormatIndex,
            out IWMStreamConfig ppIStreamConfig,
            [Out] StringBuilder wszDesc,
            ref int pcchDesc
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("7E51F487-4D93-4F98-8AB4-27D0565ADC51")]
    public interface IWMCodecInfo3 : IWMCodecInfo2
    {
        #region IWMCodecInfo Methods

        [PreserveSig]
        new int GetCodecInfoCount(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            out int pcCodecs
            );

        [PreserveSig]
        new int GetCodecFormatCount(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            out int pcFormat
            );

        [PreserveSig]
        new int GetCodecFormat(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] int dwFormatIndex,
            out IWMStreamConfig ppIStreamConfig
            );

        #endregion

        #region IWMCodecInfo2 Methods

        [PreserveSig]
        new int GetCodecName(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [Out] StringBuilder wszName,
            ref int pcchName
            );

        [PreserveSig]
        new int GetCodecFormatDesc(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] int dwFormatIndex,
            out IWMStreamConfig ppIStreamConfig,
            [Out] StringBuilder wszDesc,
            ref int pcchDesc
            );

        #endregion

        [PreserveSig]
        int GetCodecFormatProp(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] int dwFormatIndex,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref int pdwSize
            );

        [PreserveSig]
        int GetCodecProp(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref int pdwSize
            );

        [PreserveSig]
        int SetCodecEnumerationSetting(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] int dwSize
            );

        [PreserveSig]
        int GetCodecEnumerationSetting(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType,
            [In] int dwCodecIndex,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref int pdwSize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("342E0EB7-E651-450C-975B-2ACE2C90C48E")]
    public interface IWMCredentialCallback
    {
        [PreserveSig]
        int AcquireCredentials(
            [In] string pwszRealm,
            [In] string pwszSite,
            [Out] StringBuilder pwszUser,
            [In] int cchUser,
            [Out] StringBuilder pwszPassword,
            [In] int cchPassword,
            [In, MarshalAs(UnmanagedType.Error)] int hrStatus,
            out CredentialFlags pdwFlags
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("FF130EBC-A6C3-42A6-B401-C3382C3E08B3")]
    public interface IWMDRMEditor
    {
        [PreserveSig]
        int GetDRMProperty(
            [In] string pwstrName,
            out AttrDataType pdwType,
            out byte [] pValue,
            ref short pcbLength
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("D2827540-3EE7-432C-B14C-DC17F085D3B3")]
    public interface IWMDRMReader
    {
        [PreserveSig]
        int AcquireLicense(
            [In] int dwFlags
            );

        [PreserveSig]
        int CancelLicenseAcquisition();

        [PreserveSig]
        int Individualize(
            [In] int dwFlags
            );

        [PreserveSig]
        int CancelIndividualization();

        [PreserveSig]
        int MonitorLicenseAcquisition();

        [PreserveSig]
        int CancelMonitorLicenseAcquisition();

        [PreserveSig]
        int SetDRMProperty(
            [In] string pwstrName,
            [In] AttrDataType dwType,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        int GetDRMProperty(
            [In] string pwstrName,
            out AttrDataType pdwType,
            out byte [] pValue,
            ref short pcbLength
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("D6EA5DD0-12A0-43F4-90AB-A3FD451E6A07")]
    public interface IWMDRMWriter
    {
        [PreserveSig]
        int GenerateKeySeed(
            [Out] StringBuilder pwszKeySeed,
            ref int pcwchLength
            );

        [PreserveSig]
        int GenerateKeyID(
            [Out] StringBuilder pwszKeyID,
            ref int pcwchLength
            );

        [PreserveSig]
        int GenerateSigningKeyPair(
            [Out] StringBuilder pwszPrivKey,
            ref int pcwchPrivKeyLength,
            [Out] StringBuilder pwszPubKey,
            ref int pcwchPubKeyLength
            );

        [PreserveSig]
        int SetDRMAttribute(
            [In] short wStreamNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );
    }


    [Guid("96406BDA-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMHeaderInfo
    {
        [PreserveSig]
        int GetAttributeCount(
            [In] short wStreamNum,
            out short pcAttributes
            );

        [PreserveSig]
        int GetAttributeByIndex(
            [In] short wIndex,
            ref short pwStreamNum,
            [Out] StringBuilder pwszName,
            ref short pcchNameLen,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int GetAttributeByName(
            ref short pwStreamNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int SetAttribute(
            [In] short wStreamNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        int GetMarkerCount(
            out short pcMarkers
            );

        [PreserveSig]
        int GetMarker(
            [In] short wIndex,
            [Out] StringBuilder pwszMarkerName,
            ref short pcchMarkerNameLen,
            out long pcnsMarkerTime
            );

        [PreserveSig]
        int AddMarker(
            [In] string pwszMarkerName,
            [In] long cnsMarkerTime
            );

        [PreserveSig]
        int RemoveMarker(
            [In] short wIndex
            );

        [PreserveSig]
        int GetScriptCount(
            out short pcScripts
            );

        [PreserveSig]
        int GetScript(
            [In] short wIndex,
            [Out] StringBuilder pwszType,
            ref short pcchTypeLen,
            [Out] StringBuilder pwszCommand,
            ref short pcchCommandLen,
            out long pcnsScriptTime
            );

        [PreserveSig]
        int AddScript(
            [In] string pwszType,
            [In] string pwszCommand,
            [In] long cnsScriptTime
            );

        [PreserveSig]
        int RemoveScript(
            [In] short wIndex
            );
    }


    [Guid("15CF9781-454E-482E-B393-85FAE487A810"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMHeaderInfo2 : IWMHeaderInfo
    {
        #region IWMHeaderInfo Methods

        [PreserveSig]
        new int GetAttributeCount(
            [In] short wStreamNum,
            out short pcAttributes
            );

        [PreserveSig]
        new int GetAttributeByIndex(
            [In] short wIndex,
            ref short pwStreamNum,
            [Out] StringBuilder pwszName,
            ref short pcchNameLen,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int GetAttributeByName(
            ref short pwStreamNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int SetAttribute(
            [In] short wStreamNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        new int GetMarkerCount(
            out short pcMarkers
            );

        [PreserveSig]
        new int GetMarker(
            [In] short wIndex,
            [Out] StringBuilder pwszMarkerName,
            ref short pcchMarkerNameLen,
            out long pcnsMarkerTime
            );

        [PreserveSig]
        new int AddMarker(
            [In] string pwszMarkerName,
            [In] long cnsMarkerTime
            );

        [PreserveSig]
        new int RemoveMarker(
            [In] short wIndex
            );

        [PreserveSig]
        new int GetScriptCount(
            out short pcScripts
            );

        [PreserveSig]
        new int GetScript(
            [In] short wIndex,
            [Out] StringBuilder pwszType,
            ref short pcchTypeLen,
            [Out] StringBuilder pwszCommand,
            ref short pcchCommandLen,
            out long pcnsScriptTime
            );

        [PreserveSig]
        new int AddScript(
            [In] string pwszType,
            [In] string pwszCommand,
            [In] long cnsScriptTime
            );

        [PreserveSig]
        new int RemoveScript(
            [In] short wIndex
            );

        #endregion

        [PreserveSig]
        int GetCodecInfoCount(
            out int pcCodecInfos
            );

        [PreserveSig]
        int GetCodecInfo(
            [In] int wIndex,
            ref short pcchName,
            [Out] StringBuilder pwszName,
            ref short pcchDescription,
            [Out] StringBuilder pwszDescription,
            out CodecInfoType pCodecType,
            ref short pcbCodecInfo,
            out byte [] pbCodecInfo
            );
    }


    [Guid("15CC68E3-27CC-4ECD-B222-3F5D02D80BD5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMHeaderInfo3 : IWMHeaderInfo2
    {
        #region IWMHeaderInfo Methods

        [PreserveSig]
        new int GetAttributeCount(
            [In] short wStreamNum,
            out short pcAttributes
            );

        [PreserveSig]
        new int GetAttributeByIndex(
            [In] short wIndex,
            ref short pwStreamNum,
            [Out] StringBuilder pwszName,
            ref short pcchNameLen,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int GetAttributeByName(
            ref short pwStreamNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int SetAttribute(
            [In] short wStreamNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        new int GetMarkerCount(
            out short pcMarkers
            );

        [PreserveSig]
        new int GetMarker(
            [In] short wIndex,
            [Out] StringBuilder pwszMarkerName,
            ref short pcchMarkerNameLen,
            out long pcnsMarkerTime
            );

        [PreserveSig]
        new int AddMarker(
            [In] string pwszMarkerName,
            [In] long cnsMarkerTime
            );

        [PreserveSig]
        new int RemoveMarker(
            [In] short wIndex
            );

        [PreserveSig]
        new int GetScriptCount(
            out short pcScripts
            );

        [PreserveSig]
        new int GetScript(
            [In] short wIndex,
            [Out] StringBuilder pwszType,
            ref short pcchTypeLen,
            [Out] StringBuilder pwszCommand,
            ref short pcchCommandLen,
            out long pcnsScriptTime
            );

        [PreserveSig]
        new int AddScript(
            [In] string pwszType,
            [In] string pwszCommand,
            [In] long cnsScriptTime
            );

        [PreserveSig]
        new int RemoveScript(
            [In] short wIndex
            );

        #endregion

        #region IWMHeaderInfo2 Methods

        [PreserveSig]
        new int GetCodecInfoCount(
            out int pcCodecInfos
            );

        [PreserveSig]
        new int GetCodecInfo(
            [In] int wIndex,
            ref short pcchName,
            [Out] StringBuilder pwszName,
            ref short pcchDescription,
            [Out] StringBuilder pwszDescription,
            out CodecInfoType pCodecType,
            ref short pcbCodecInfo,
            out byte [] pbCodecInfo
            );

        #endregion

        [PreserveSig]
        int GetAttributeCountEx(
            [In] short wStreamNum,
            out short pcAttributes
            );

        [PreserveSig]
        int GetAttributeIndices(
            [In] short wStreamNum,
            [In] string pwszName,
            [In] ref short pwLangIndex,
            out short pwIndices,
            ref short pwCount
            );

        [PreserveSig]
        int GetAttributeByIndexEx(
            [In] short wStreamNum,
            [In] short wIndex,
            [Out] StringBuilder pwszName,
            ref short pwNameLen,
            out AttrDataType pType,
            out short pwLangIndex,
            out byte [] pValue,
            ref int pdwDataLength
            );

        [PreserveSig]
        int ModifyAttribute(
            [In] short wStreamNum,
            [In] short wIndex,
            [In] AttrDataType Type,
            [In] short wLangIndex,
            [In] byte [] pValue,
            [In] int dwLength
            );

        [PreserveSig]
        int AddAttribute(
            [In] short wStreamNum,
            [In] string pszName,
            out short pwIndex,
            [In] AttrDataType Type,
            [In] short wLangIndex,
            [In] byte [] pValue,
            [In] int dwLength
            );

        [PreserveSig]
        int DeleteAttribute(
            [In] short wStreamNum,
            [In] short wIndex
            );

        [PreserveSig]
        int AddCodecInfo(
            [In] string pwszName,
            [In] string pwszDescription,
            [In] CodecInfoType codecType,
            [In] short cbCodecInfo,
            [In] byte [] pbCodecInfo
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("9F0AA3B6-7267-4D89-88F2-BA915AA5C4C6")]
    public interface IWMImageInfo
    {
        [PreserveSig]
        int GetImageCount(
            out int pcImages
            );

        [PreserveSig]
        int GetImage(
            [In] int wIndex,
            ref short pcchMIMEType,
            [Out] StringBuilder pwszMIMEType,
            ref short pcchDescription,
            [Out] StringBuilder pwszDescription,
            out short pImageType,
            ref int pcbImageData,
            out byte [] pbImageData
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("6D7CDC71-9888-11D3-8EDC-00C04F6109CF")]
    public interface IWMIndexer
    {
        [PreserveSig]
        int StartIndexing(
            [In] string pwszURL,
            [In] IWMStatusCallback pCallback,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int Cancel();
    }


    [Guid("B70F1E42-6255-4DF0-A6B9-02B212D9E2BB"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMIndexer2 : IWMIndexer
    {
        #region IWMIndexer Methods

        [PreserveSig]
        new int StartIndexing(
            [In] string pwszURL,
            [In] IWMStatusCallback pCallback,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        new int Cancel();

        #endregion

        [PreserveSig]
        int Configure(
            [In] short wStreamNum,
            [In] IndexerType nIndexerType,
            [In] IntPtr pvInterval,
            [In] IntPtr pvIndexType
            );
    }


    [Guid("96406BD5-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMInputMediaProps : IWMMediaProps
    {
        #region IWMMediaProps Methods

        [PreserveSig]
        new int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        new int GetMediaType(
            out AMMediaType pType,
            ref int pcbType
            );

        [PreserveSig]
        new int SetMediaType(
            [In, MarshalAs(UnmanagedType.LPStruct)] AMMediaType pType
            );

        #endregion

        [PreserveSig]
        int GetConnectionName(
            [Out] StringBuilder pwszName,
            ref short pcchName
            );

        [PreserveSig]
        int GetGroupName(
            [Out] StringBuilder pwszName,
            ref short pcchName
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("6816DAD3-2B4B-4C8E-8149-874C3483A753")]
    public interface IWMIStreamProps
    {
        [PreserveSig]
        int GetProperty(
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref int pdwSize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("DF683F00-2D49-4D8E-92B7-FB19F6A0DC57")]
    public interface IWMLanguageList
    {
        [PreserveSig]
        int GetLanguageCount(
            out short pwCount
            );

        [PreserveSig]
        int GetLanguageDetails(
            [In] short wIndex,
            [Out] StringBuilder pwszLanguageString,
            ref short pcchLanguageStringLength
            );

        [PreserveSig]
        int AddLanguageByRFC1766String(
            [In] string pwszLanguageString,
            out short pwIndex
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("05E5AC9F-3FB6-4508-BB43-A4067BA1EBE8")]
    public interface IWMLicenseBackup
    {
        [PreserveSig]
        int BackupLicenses(
            [In] int dwFlags,
            [In] IWMStatusCallback pCallback
            );

        [PreserveSig]
        int CancelLicenseBackup();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("C70B6334-A22E-4EFB-A245-15E65A004A13")]
    public interface IWMLicenseRestore
    {
        [PreserveSig]
        int RestoreLicenses(
            [In] int dwFlags,
            [In] IWMStatusCallback pCallback
            );

        [PreserveSig]
        int CancelLicenseRestore();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BCE-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMMediaProps
    {
        [PreserveSig]
        int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        int GetMediaType(
            out AMMediaType pType,
            ref int pcbType
            );

        [PreserveSig]
        int SetMediaType(
            [In] AMMediaType pType
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BD9-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMMetadataEditor
    {
        [PreserveSig]
        int Open(
            [In] string pwszFilename
            );

        [PreserveSig]
        int Close();

        [PreserveSig]
        int Flush();
    }


    [Guid("203CFFE3-2E18-4FDF-B59D-6E71530534CF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMMetadataEditor2 : IWMMetadataEditor
    {
        #region IWMMetadataEditor Methods

        [PreserveSig]
        new int Open(
            [In] string pwszFilename
            );

        [PreserveSig]
        new int Close();

        [PreserveSig]
        new int Flush();

        #endregion

        [PreserveSig]
        int OpenEx(
            [In] string pwszFilename,
            [In] int dwDesiredAccess,
            [In] int dwShareMode
            );
    }


    [Guid("96406BDE-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMMutualExclusion : IWMStreamList
    {
        #region IWMStreamList Methods

        [PreserveSig]
        new int GetStreams(
            out short [] pwStreamNumArray,
            ref short pcStreams
            );

        [PreserveSig]
        new int AddStream(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int RemoveStream(
            [In] short wStreamNum
            );

        #endregion

        [PreserveSig]
        int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        int SetType(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType
            );
    }


    [Guid("0302B57D-89D1-4BA2-85C9-166F2C53EB91"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMMutualExclusion2 : IWMMutualExclusion
    {
        #region IWMStreamList Methods

        [PreserveSig]
        new int GetStreams(
            out short [] pwStreamNumArray,
            ref short pcStreams
            );

        [PreserveSig]
        new int AddStream(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int RemoveStream(
            [In] short wStreamNum
            );

        #endregion

        #region IWMMutualExclusion Methods

        [PreserveSig]
        new int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        new int SetType(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidType
            );

        #endregion

        [PreserveSig]
        int GetName(
            [Out] StringBuilder pwszName,
            ref short pcchName
            );

        [PreserveSig]
        int SetName(
            [In] string pwszName
            );

        [PreserveSig]
        int GetRecordCount(
            out short pwRecordCount
            );

        [PreserveSig]
        int AddRecord();

        [PreserveSig]
        int RemoveRecord(
            [In] short wRecordNumber
            );

        [PreserveSig]
        int GetRecordName(
            [In] short wRecordNumber,
            [Out] StringBuilder pwszRecordName,
            ref short pcchRecordName
            );

        [PreserveSig]
        int SetRecordName(
            [In] short wRecordNumber,
            [In] string pwszRecordName
            );

        [PreserveSig]
        int GetStreamsForRecord(
            [In] short wRecordNumber,
            out short [] pwStreamNumArray,
            ref short pcStreams
            );

        [PreserveSig]
        int AddStreamForRecord(
            [In] short wRecordNumber,
            [In] short wStreamNumber
            );

        [PreserveSig]
        int RemoveStreamForRecord(
            [In] short wRecordNumber,
            [In] short wStreamNumber
            );
    }


    [Guid("96406BD7-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMOutputMediaProps : IWMMediaProps
    {
        #region IWMMediaProps Methods

        [PreserveSig]
        new int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        new int GetMediaType(
            out AMMediaType pType,
            ref int pcbType
            );

        [PreserveSig]
        new int SetMediaType(
            [In] AMMediaType pType
            );

        #endregion

        [PreserveSig]
        int GetStreamGroupName(
            [Out] StringBuilder pwszName,
            ref short pcchName
            );

        [PreserveSig]
        int GetConnectionName(
            [Out] StringBuilder pwszName,
            ref short pcchName
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("CDFB97AB-188F-40B3-B643-5B7903975C59")]
    public interface IWMPacketSize
    {
        [PreserveSig]
        int GetMaxPacketSize(
            out int pdwMaxPacketSize
            );

        [PreserveSig]
        int SetMaxPacketSize(
            [In] int dwMaxPacketSize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("8BFC2B9E-B646-4233-A877-1C6A079669DC")]
    public interface IWMPacketSize2 : IWMPacketSize
    {
        #region IWMPacketSize Methods

        [PreserveSig]
        new int GetMaxPacketSize(
            out int pdwMaxPacketSize
            );

        [PreserveSig]
        new int SetMaxPacketSize(
            [In] int dwMaxPacketSize
            );

        #endregion

        [PreserveSig]
        int GetMinPacketSize(
            out int pdwMinPacketSize
            );

        [PreserveSig]
        int SetMinPacketSize(
            [In] int dwMinPacketSize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BDB-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMProfile
    {
        [PreserveSig]
        int GetVersion(
            out WMVersion pdwVersion
            );

        [PreserveSig]
        int GetName(
            [Out] StringBuilder pwszName,
            ref int pcchName
            );

        [PreserveSig]
        int SetName(
            [In] string pwszName
            );

        [PreserveSig]
        int GetDescription(
            [Out] StringBuilder pwszDescription,
            ref int pcchDescription
            );

        [PreserveSig]
        int SetDescription(
            [In] string pwszDescription
            );

        [PreserveSig]
        int GetStreamCount(
            out int pcStreams
            );

        [PreserveSig]
        int GetStream(
            [In] int dwStreamIndex,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        int GetStreamByNumber(
            [In] short wStreamNum,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        int RemoveStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        int RemoveStreamByNumber(
            [In] short wStreamNum
            );

        [PreserveSig]
        int AddStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        int ReconfigStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        int CreateNewStream(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidStreamType,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        int GetMutualExclusionCount(
            out int pcME
            );

        [PreserveSig]
        int GetMutualExclusion(
            [In] int dwMEIndex,
            out IWMMutualExclusion ppME
            );

        [PreserveSig]
        int RemoveMutualExclusion(
            [In] IWMMutualExclusion pME
            );

        [PreserveSig]
        int AddMutualExclusion(
            [In] IWMMutualExclusion pME
            );

        [PreserveSig]
        int CreateNewMutualExclusion(
            out IWMMutualExclusion ppME
            );
    }


    [Guid("07E72D33-D94E-4BE7-8843-60AE5FF7E5F5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMProfile2 : IWMProfile
    {
        #region IWMProfile Methods

        [PreserveSig]
        new int GetVersion(
            out WMVersion pdwVersion
            );

        [PreserveSig]
        new int GetName(
            [Out] StringBuilder pwszName,
            ref int pcchName
            );

        [PreserveSig]
        new int SetName(
            [In] string pwszName
            );

        [PreserveSig]
        new int GetDescription(
            [Out] StringBuilder pwszDescription,
            ref int pcchDescription
            );

        [PreserveSig]
        new int SetDescription(
            [In] string pwszDescription
            );

        [PreserveSig]
        new int GetStreamCount(
            out int pcStreams
            );

        [PreserveSig]
        new int GetStream(
            [In] int dwStreamIndex,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        new int GetStreamByNumber(
            [In] short wStreamNum,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        new int RemoveStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        new int RemoveStreamByNumber(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int AddStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        new int ReconfigStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        new int CreateNewStream(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidStreamType,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        new int GetMutualExclusionCount(
            out int pcME
            );

        [PreserveSig]
        new int GetMutualExclusion(
            [In] int dwMEIndex,
            out IWMMutualExclusion ppME
            );

        [PreserveSig]
        new int RemoveMutualExclusion(
            [In] IWMMutualExclusion pME
            );

        [PreserveSig]
        new int AddMutualExclusion(
            [In] IWMMutualExclusion pME
            );

        [PreserveSig]
        new int CreateNewMutualExclusion(
            out IWMMutualExclusion ppME
            );

        #endregion

        [PreserveSig]
        int GetProfileID(
            out Guid pguidID
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("00EF96CC-A461-4546-8BCD-C9A28F0E06F5")]
    public interface IWMProfile3 : IWMProfile2
    {
        #region IWMProfile Methods

        [PreserveSig]
        new int GetVersion(
            out WMVersion pdwVersion
            );

        [PreserveSig]
        new int GetName(
            [Out] StringBuilder pwszName,
            ref int pcchName
            );

        [PreserveSig]
        new int SetName(
            [In] string pwszName
            );

        [PreserveSig]
        new int GetDescription(
            [Out] StringBuilder pwszDescription,
            ref int pcchDescription
            );

        [PreserveSig]
        new int SetDescription(
            [In] string pwszDescription
            );

        [PreserveSig]
        new int GetStreamCount(
            out int pcStreams
            );

        [PreserveSig]
        new int GetStream(
            [In] int dwStreamIndex,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        new int GetStreamByNumber(
            [In] short wStreamNum,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        new int RemoveStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        new int RemoveStreamByNumber(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int AddStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        new int ReconfigStream(
            [In] IWMStreamConfig pConfig
            );

        [PreserveSig]
        new int CreateNewStream(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidStreamType,
            out IWMStreamConfig ppConfig
            );

        [PreserveSig]
        new int GetMutualExclusionCount(
            out int pcME
            );

        [PreserveSig]
        new int GetMutualExclusion(
            [In] int dwMEIndex,
            out IWMMutualExclusion ppME
            );

        [PreserveSig]
        new int RemoveMutualExclusion(
            [In] IWMMutualExclusion pME
            );

        [PreserveSig]
        new int AddMutualExclusion(
            [In] IWMMutualExclusion pME
            );

        [PreserveSig]
        new int CreateNewMutualExclusion(
            out IWMMutualExclusion ppME
            );

        #endregion

        #region IWMProfile2 Methods

        [PreserveSig]
        new int GetProfileID(
            out Guid pguidID
            );

        #endregion

        [PreserveSig]
        int GetStorageFormat(
            out StorageFormat pnStorageFormat
            );

        [PreserveSig]
        int SetStorageFormat(
            [In] StorageFormat nStorageFormat
            );

        [PreserveSig]
        int GetBandwidthSharingCount(
            out int pcBS
            );

        [PreserveSig]
        int GetBandwidthSharing(
            [In] int dwBSIndex,
            out IWMBandwidthSharing ppBS
            );

        [PreserveSig]
        int RemoveBandwidthSharing(
            [In] IWMBandwidthSharing pBS
            );

        [PreserveSig]
        int AddBandwidthSharing(
            [In] IWMBandwidthSharing pBS
            );

        [PreserveSig]
        int CreateNewBandwidthSharing(
            out IWMBandwidthSharing ppBS
            );

        [PreserveSig]
        int GetStreamPrioritization(
            out IWMStreamPrioritization ppSP
            );

        [PreserveSig]
        int SetStreamPrioritization(
            [In] IWMStreamPrioritization pSP
            );

        [PreserveSig]
        int RemoveStreamPrioritization();

        [PreserveSig]
        int CreateNewStreamPrioritization(
            out IWMStreamPrioritization ppSP
            );

        [PreserveSig]
        int GetExpectedPacketCount(
            [In] long msDuration,
            out long pcPackets
            );
    }


    [Guid("D16679F2-6CA0-472D-8D31-2F5D55AEE155"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMProfileManager
    {
        [PreserveSig]
        int CreateEmptyProfile(
            [In] WMVersion dwVersion,
            out IWMProfile ppProfile
            );

        [PreserveSig]
        int LoadProfileByID(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidProfile,
            out IWMProfile ppProfile
            );

        [PreserveSig]
        int LoadProfileByData(
            [In] string pwszProfile,
            out IWMProfile ppProfile
            );

        [PreserveSig]
        int SaveProfile(
            [In] IWMProfile pIWMProfile,
            [In] string pwszProfile,
            ref int pdwLength
            );

        [PreserveSig]
        int GetSystemProfileCount(
            out int pcProfiles
            );

        [PreserveSig]
        int LoadSystemProfile(
            [In] int dwProfileIndex,
            out IWMProfile ppProfile
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("7A924E51-73C1-494D-8019-23D37ED9B89A")]
    public interface IWMProfileManager2 : IWMProfileManager
    {
        #region IWMProfileManager Methods

        [PreserveSig]
        new int CreateEmptyProfile(
            [In] WMVersion dwVersion,
            out IWMProfile ppProfile
            );

        [PreserveSig]
        new int LoadProfileByID(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidProfile,
            out IWMProfile ppProfile
            );

        [PreserveSig]
        new int LoadProfileByData(
            [In] string pwszProfile,
            out IWMProfile ppProfile
            );

        [PreserveSig]
        new int SaveProfile(
            [In] IWMProfile pIWMProfile,
            [In] string pwszProfile,
            ref int pdwLength
            );

        [PreserveSig]
        new int GetSystemProfileCount(
            out int pcProfiles
            );

        [PreserveSig]
        new int LoadSystemProfile(
            [In] int dwProfileIndex,
            out IWMProfile ppProfile
            );

        #endregion

        [PreserveSig]
        int GetSystemProfileVersion(
            out WMVersion pdwVersion
            );

        [PreserveSig]
        int SetSystemProfileVersion(
            WMVersion dwVersion
            );
    }


    [Guid("BA4DCC78-7EE0-4AB8-B27A-DBCE8BC51454"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMProfileManagerLanguage
    {
        [PreserveSig]
        int GetUserLanguageID(
            out short wLangID
            );

        [PreserveSig]
        int SetUserLanguageID(
            short wLangID
            );
    }


    [Guid("72995A79-5090-42A4-9C8C-D9D0B6D34BE5"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMPropertyVault
    {
        [PreserveSig]
        int GetPropertyCount(
            out int pdwCount
            );

        [PreserveSig]
        int GetPropertyByName(
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref int pdwSize
            );

        [PreserveSig]
        int SetProperty(
            [In] string pszName,
            [In] AttrDataType pType,
            [In] byte [] pValue,
            [In] int dwSize
            );

        [PreserveSig]
        int GetPropertyByIndex(
            [In] int dwIndex,
            [Out] StringBuilder pszName,
            ref int pdwNameLen,
            out AttrDataType pType,
            out byte [] pValue,
            ref int pdwSize
            );

        [PreserveSig]
        int CopyPropertiesFrom(
            [In] IWMPropertyVault pIWMPropertyVault
            );

        [PreserveSig]
        int Clear();
    }


    [Guid("96406BD6-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReader
    {
        [PreserveSig]
        int Open(
            [In] string pwszURL,
            [In] IWMReaderCallback pCallback,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int Close();

        [PreserveSig]
        int GetOutputCount(
            out int pcOutputs
            );

        [PreserveSig]
        int GetOutputProps(
            [In] int dwOutputNum,
            out IWMOutputMediaProps ppOutput
            );

        [PreserveSig]
        int SetOutputProps(
            [In] int dwOutputNum,
            [In] IWMOutputMediaProps pOutput
            );

        [PreserveSig]
        int GetOutputFormatCount(
            [In] int dwOutputNumber,
            out int pcFormats
            );

        [PreserveSig]
        int GetOutputFormat(
            [In] int dwOutputNumber,
            [In] int dwFormatNumber,
            out IWMOutputMediaProps ppProps
            );

        [PreserveSig]
        int Start(
            [In] long cnsStart,
            [In] long cnsDuration,
            [In] float fRate,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int Stop();

        [PreserveSig]
        int Pause();

        [PreserveSig]
        int Resume();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("BDDC4D08-944D-4D52-A612-46C3FDA07DD4")]
    public interface IWMReaderAccelerator
    {
        [PreserveSig]
        int GetCodecInterface(
            [In] int dwOutputNum,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IntPtr ppvCodecInterface
            );

        [PreserveSig]
        int Notify(
            [In] int dwOutputNum,
            [In] AMMediaType pSubtype
            );
    }


    [Guid("96406BEA-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderAdvanced
    {
        [PreserveSig]
        int SetUserProvidedClock(
            [In, MarshalAs(UnmanagedType.Bool)] bool fUserClock
            );

        [PreserveSig]
        int GetUserProvidedClock(
            [MarshalAs(UnmanagedType.Bool)] out bool pfUserClock
            );

        [PreserveSig]
        int DeliverTime(
            [In] long cnsTime
            );

        [PreserveSig]
        int SetManualStreamSelection(
            [In, MarshalAs(UnmanagedType.Bool)] bool fSelection
            );

        [PreserveSig]
        int GetManualStreamSelection(
            [MarshalAs(UnmanagedType.Bool)] out bool pfSelection
            );

        [PreserveSig]
        int SetStreamsSelected(
            [In] short cStreamCount,
            [In] short [] pwStreamNumbers,
            [In] StreamSelection [] pSelections
            );

        [PreserveSig]
        int GetStreamSelected(
            [In] short wStreamNum,
            out StreamSelection pSelection
            );

        [PreserveSig]
        int SetReceiveSelectionCallbacks(
            [In, MarshalAs(UnmanagedType.Bool)] bool fGetCallbacks
            );

        [PreserveSig]
        int GetReceiveSelectionCallbacks(
            [MarshalAs(UnmanagedType.Bool)] out bool pfGetCallbacks
            );

        [PreserveSig]
        int SetReceiveStreamSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fReceiveStreamSamples
            );

        [PreserveSig]
        int GetReceiveStreamSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfReceiveStreamSamples
            );

        [PreserveSig]
        int SetAllocateForOutput(
            [In] int dwOutputNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        int GetAllocateForOutput(
            [In] int dwOutputNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        int SetAllocateForStream(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        int GetAllocateForStream(
            [In] short dwSreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        int GetStatistics(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WMReaderStatistics pStatistics
            );

        [PreserveSig]
        int SetClientInfo(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMReaderClientInfo pClientInfo
            );

        [PreserveSig]
        int GetMaxOutputSampleSize(
            [In] int dwOutput,
            out int pcbMax
            );

        [PreserveSig]
        int GetMaxStreamSampleSize(
            [In] short wStream,
            out int pcbMax
            );

        [PreserveSig]
        int NotifyLateDelivery(
            long cnsLateness
            );
    }


    [Guid("AE14A945-B90C-4D0D-9127-80D665F7D73E"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderAdvanced2 : IWMReaderAdvanced
    {
        #region IWMReaderAdvanced Methods

        [PreserveSig]
        new int SetUserProvidedClock(
            [In, MarshalAs(UnmanagedType.Bool)] bool fUserClock
            );

        [PreserveSig]
        new int GetUserProvidedClock(
            [MarshalAs(UnmanagedType.Bool)] out bool pfUserClock
            );

        [PreserveSig]
        new int DeliverTime(
            [In] long cnsTime
            );

        [PreserveSig]
        new int SetManualStreamSelection(
            [In, MarshalAs(UnmanagedType.Bool)] bool fSelection
            );

        [PreserveSig]
        new int GetManualStreamSelection(
            [MarshalAs(UnmanagedType.Bool)] out bool pfSelection
            );

        [PreserveSig]
        new int SetStreamsSelected(
            [In] short cStreamCount,
            [In] short [] pwStreamNumbers,
            [In] StreamSelection [] pSelections
            );

        [PreserveSig]
        new int GetStreamSelected(
            [In] short wStreamNum,
            out StreamSelection pSelection
            );

        [PreserveSig]
        new int SetReceiveSelectionCallbacks(
            [In, MarshalAs(UnmanagedType.Bool)] bool fGetCallbacks
            );

        [PreserveSig]
        new int GetReceiveSelectionCallbacks(
            [MarshalAs(UnmanagedType.Bool)] out bool pfGetCallbacks
            );

        [PreserveSig]
        new int SetReceiveStreamSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fReceiveStreamSamples
            );

        [PreserveSig]
        new int GetReceiveStreamSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfReceiveStreamSamples
            );

        [PreserveSig]
        new int SetAllocateForOutput(
            [In] int dwOutputNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        new int GetAllocateForOutput(
            [In] int dwOutputNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        new int SetAllocateForStream(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        new int GetAllocateForStream(
            [In] short dwSreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        new int GetStatistics(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WMReaderStatistics pStatistics
            );

        [PreserveSig]
        new int SetClientInfo(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMReaderClientInfo pClientInfo
            );

        [PreserveSig]
        new int GetMaxOutputSampleSize(
            [In] int dwOutput,
            out int pcbMax
            );

        [PreserveSig]
        new int GetMaxStreamSampleSize(
            [In] short wStream,
            out int pcbMax
            );

        [PreserveSig]
        new int NotifyLateDelivery(
            long cnsLateness
            );

        #endregion

        [PreserveSig]
        int SetPlayMode(
            [In] PlayMode Mode
            );

        [PreserveSig]
        int GetPlayMode(
            out PlayMode pMode
            );

        [PreserveSig]
        int GetBufferProgress(
            out int pdwPercent,
            out long pcnsBuffering
            );

        [PreserveSig]
        int GetDownloadProgress(
            out int pdwPercent,
            out long pqwBytesDownloaded,
            out long pcnsDownload
            );

        [PreserveSig]
        int GetSaveAsProgress(
            out int pdwPercent
            );

        [PreserveSig]
        int SaveFileAs(
            [In] string pwszFilename
            );

        [PreserveSig]
        int GetProtocolName(
            [Out] StringBuilder pwszProtocol,
            ref int pcchProtocol
            );

        [PreserveSig]
        int StartAtMarker(
            [In] short wMarkerIndex,
            [In] long cnsDuration,
            [In] float fRate,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int GetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int SetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        int Preroll(
            [In] long cnsStart,
            [In] long cnsDuration,
            [In] float fRate
            );

        [PreserveSig]
        int SetLogClientID(
            [In, MarshalAs(UnmanagedType.Bool)] bool fLogClientID
            );

        [PreserveSig]
        int GetLogClientID(
            [MarshalAs(UnmanagedType.Bool)] out bool pfLogClientID
            );

        [PreserveSig]
        int StopBuffering();

        [PreserveSig]
        int OpenStream(
            [In] UCOMIStream pStream,
            [In] IWMReaderCallback pCallback,
            [In] IntPtr pvContext
            );
    }


    [Guid("5DC0674B-F04B-4A4E-9F2A-B1AFDE2C8100"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderAdvanced3 : IWMReaderAdvanced2
    {
        #region IWMReaderAdvanced Methods

        [PreserveSig]
        new int SetUserProvidedClock(
            [In, MarshalAs(UnmanagedType.Bool)] bool fUserClock
            );

        [PreserveSig]
        new int GetUserProvidedClock(
            [MarshalAs(UnmanagedType.Bool)] out bool pfUserClock
            );

        [PreserveSig]
        new int DeliverTime(
            [In] long cnsTime
            );

        [PreserveSig]
        new int SetManualStreamSelection(
            [In, MarshalAs(UnmanagedType.Bool)] bool fSelection
            );

        [PreserveSig]
        new int GetManualStreamSelection(
            [MarshalAs(UnmanagedType.Bool)] out bool pfSelection
            );

        [PreserveSig]
        new int SetStreamsSelected(
            [In] short cStreamCount,
            [In] short [] pwStreamNumbers,
            [In] StreamSelection [] pSelections
            );

        [PreserveSig]
        new int GetStreamSelected(
            [In] short wStreamNum,
            out StreamSelection pSelection
            );

        [PreserveSig]
        new int SetReceiveSelectionCallbacks(
            [In, MarshalAs(UnmanagedType.Bool)] bool fGetCallbacks
            );

        [PreserveSig]
        new int GetReceiveSelectionCallbacks(
            [MarshalAs(UnmanagedType.Bool)] out bool pfGetCallbacks
            );

        [PreserveSig]
        new int SetReceiveStreamSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fReceiveStreamSamples
            );

        [PreserveSig]
        new int GetReceiveStreamSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfReceiveStreamSamples
            );

        [PreserveSig]
        new int SetAllocateForOutput(
            [In] int dwOutputNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        new int GetAllocateForOutput(
            [In] int dwOutputNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        new int SetAllocateForStream(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        new int GetAllocateForStream(
            [In] short dwSreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        new int GetStatistics(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WMReaderStatistics pStatistics
            );

        [PreserveSig]
        new int SetClientInfo(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMReaderClientInfo pClientInfo
            );

        [PreserveSig]
        new int GetMaxOutputSampleSize(
            [In] int dwOutput,
            out int pcbMax
            );

        [PreserveSig]
        new int GetMaxStreamSampleSize(
            [In] short wStream,
            out int pcbMax
            );

        [PreserveSig]
        new int NotifyLateDelivery(
            long cnsLateness
            );

        #endregion

        #region IWMReaderAdvanced2 Methods

        [PreserveSig]
        new int SetPlayMode(
            [In] PlayMode Mode
            );

        [PreserveSig]
        new int GetPlayMode(
            out PlayMode pMode
            );

        [PreserveSig]
        new int GetBufferProgress(
            out int pdwPercent,
            out long pcnsBuffering
            );

        [PreserveSig]
        new int GetDownloadProgress(
            out int pdwPercent,
            out long pqwBytesDownloaded,
            out long pcnsDownload
            );

        [PreserveSig]
        new int GetSaveAsProgress(
            out int pdwPercent
            );

        [PreserveSig]
        new int SaveFileAs(
            [In] string pwszFilename
            );

        [PreserveSig]
        new int GetProtocolName(
            [Out] StringBuilder pwszProtocol,
            ref int pcchProtocol
            );

        [PreserveSig]
        new int StartAtMarker(
            [In] short wMarkerIndex,
            [In] long cnsDuration,
            [In] float fRate,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        new int GetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int SetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        new int Preroll(
            [In] long cnsStart,
            [In] long cnsDuration,
            [In] float fRate
            );

        [PreserveSig]
        new int SetLogClientID(
            [In, MarshalAs(UnmanagedType.Bool)] bool fLogClientID
            );

        [PreserveSig]
        new int GetLogClientID(
            [MarshalAs(UnmanagedType.Bool)] out bool pfLogClientID
            );

        [PreserveSig]
        new int StopBuffering();

        [PreserveSig]
        new int OpenStream(
            [In] UCOMIStream pStream,
            [In] IWMReaderCallback pCallback,
            [In] IntPtr pvContext
            );

        #endregion

        [PreserveSig]
        int StopNetStreaming();

        [PreserveSig]
        int StartAtPosition(
            [In] short wStreamNum,
            [In] IntPtr pvOffsetStart,
            [In] IntPtr pvDuration,
            [In] OffsetFormat dwOffsetFormat,
            [In] float fRate,
            [In] IntPtr pvContext
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("945A76A2-12AE-4D48-BD3C-CD1D90399B85")]
    public interface IWMReaderAdvanced4 : IWMReaderAdvanced3
    {
        #region IWMReaderAdvanced Methods

        [PreserveSig]
        new int SetUserProvidedClock(
            [In, MarshalAs(UnmanagedType.Bool)] bool fUserClock
            );

        [PreserveSig]
        new int GetUserProvidedClock(
            [MarshalAs(UnmanagedType.Bool)] out bool pfUserClock
            );

        [PreserveSig]
        new int DeliverTime(
            [In] long cnsTime
            );

        [PreserveSig]
        new int SetManualStreamSelection(
            [In, MarshalAs(UnmanagedType.Bool)] bool fSelection
            );

        [PreserveSig]
        new int GetManualStreamSelection(
            [MarshalAs(UnmanagedType.Bool)] out bool pfSelection
            );

        [PreserveSig]
        new int SetStreamsSelected(
            [In] short cStreamCount,
            [In] short [] pwStreamNumbers,
            [In] StreamSelection [] pSelections
            );

        [PreserveSig]
        new int GetStreamSelected(
            [In] short wStreamNum,
            out StreamSelection pSelection
            );

        [PreserveSig]
        new int SetReceiveSelectionCallbacks(
            [In, MarshalAs(UnmanagedType.Bool)] bool fGetCallbacks
            );

        [PreserveSig]
        new int GetReceiveSelectionCallbacks(
            [MarshalAs(UnmanagedType.Bool)] out bool pfGetCallbacks
            );

        [PreserveSig]
        new int SetReceiveStreamSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fReceiveStreamSamples
            );

        [PreserveSig]
        new int GetReceiveStreamSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfReceiveStreamSamples
            );

        [PreserveSig]
        new int SetAllocateForOutput(
            [In] int dwOutputNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        new int GetAllocateForOutput(
            [In] int dwOutputNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        new int SetAllocateForStream(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        new int GetAllocateForStream(
            [In] short dwSreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );

        [PreserveSig]
        new int GetStatistics(
            [In, Out, MarshalAs(UnmanagedType.LPStruct)] WMReaderStatistics pStatistics
            );

        [PreserveSig]
        new int SetClientInfo(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMReaderClientInfo pClientInfo
            );

        [PreserveSig]
        new int GetMaxOutputSampleSize(
            [In] int dwOutput,
            out int pcbMax
            );

        [PreserveSig]
        new int GetMaxStreamSampleSize(
            [In] short wStream,
            out int pcbMax
            );

        [PreserveSig]
        new int NotifyLateDelivery(
            long cnsLateness
            );

        #endregion

        #region IWMReaderAdvanced2 Methods

        [PreserveSig]
        new int SetPlayMode(
            [In] PlayMode Mode
            );

        [PreserveSig]
        new int GetPlayMode(
            out PlayMode pMode
            );

        [PreserveSig]
        new int GetBufferProgress(
            out int pdwPercent,
            out long pcnsBuffering
            );

        [PreserveSig]
        new int GetDownloadProgress(
            out int pdwPercent,
            out long pqwBytesDownloaded,
            out long pcnsDownload
            );

        [PreserveSig]
        new int GetSaveAsProgress(
            out int pdwPercent
            );

        [PreserveSig]
        new int SaveFileAs(
            [In] string pwszFilename
            );

        [PreserveSig]
        new int GetProtocolName(
            [Out] StringBuilder pwszProtocol,
            ref int pcchProtocol
            );

        [PreserveSig]
        new int StartAtMarker(
            [In] short wMarkerIndex,
            [In] long cnsDuration,
            [In] float fRate,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        new int GetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int SetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        new int Preroll(
            [In] long cnsStart,
            [In] long cnsDuration,
            [In] float fRate
            );

        [PreserveSig]
        new int SetLogClientID(
            [In, MarshalAs(UnmanagedType.Bool)] bool fLogClientID
            );

        [PreserveSig]
        new int GetLogClientID(
            [MarshalAs(UnmanagedType.Bool)] out bool pfLogClientID
            );

        [PreserveSig]
        new int StopBuffering();

        [PreserveSig]
        new int OpenStream(
            [In] UCOMIStream pStream,
            [In] IWMReaderCallback pCallback,
            [In] IntPtr pvContext
            );

        #endregion

        #region IWMReaderAdvanced3

        [PreserveSig]
        new int StopNetStreaming();

        [PreserveSig]
        new int StartAtPosition(
            [In] short wStreamNum,
            [In] IntPtr pvOffsetStart,
            [In] IntPtr pvDuration,
            [In] OffsetFormat dwOffsetFormat,
            [In] float fRate,
            [In] IntPtr pvContext
            );

        #endregion

        [PreserveSig]
        int GetLanguageCount(
            [In] int dwOutputNum,
            out short pwLanguageCount
            );

        [PreserveSig]
        int GetLanguage(
            [In] int dwOutputNum,
            [In] short wLanguage,
            [Out] StringBuilder pwszLanguageString,
            ref short pcchLanguageStringLength
            );

        [PreserveSig]
        int GetMaxSpeedFactor(
            out double pdblFactor
            );

        [PreserveSig]
        int IsUsingFastCache(
            [MarshalAs(UnmanagedType.Bool)] out bool pfUsingFastCache
            );

        [PreserveSig]
        int AddLogParam(
            [In] string wszNameSpace,
            [In] string wszName,
            [In] string wszValue
            );

        [PreserveSig]
        int SendLogParams();

        [PreserveSig]
        int CanSaveFileAs(
            [MarshalAs(UnmanagedType.Bool)] out bool pfCanSave
            );

        [PreserveSig]
        int CancelSaveFileAs();

        [PreserveSig]
        int GetURL(
            [Out] StringBuilder pwszURL,
            ref int pcchURL
            );
    }


    [Guid("9F762FA7-A22E-428D-93C9-AC82F3AAFE5A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderAllocatorEx
    {
        [PreserveSig]
        int AllocateForStreamEx(
            [In] short wStreamNum,
            [In] int cbBuffer,
            out INSSBuffer ppBuffer,
            [In] int dwFlags,
            [In] long cnsSampleTime,
            [In] long cnsSampleDuration,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int AllocateForOutputEx(
            [In] int dwOutputNum,
            [In] int cbBuffer,
            out INSSBuffer ppBuffer,
            [In] int dwFlags,
            [In] long cnsSampleTime,
            [In] long cnsSampleDuration,
            [In] IntPtr pvContext
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BD8-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMReaderCallback : IWMStatusCallback
    {
        #region IWMStatusCallback Methods

        [PreserveSig]
        new int OnStatus(
            [In] Status Status,
            [In, MarshalAs(UnmanagedType.Error)] int hr,
            [In] AttrDataType dwType,
            [In] byte [] pValue,
            [In] IntPtr pvContext
            );

        #endregion

        [PreserveSig]
        int OnSample(
            [In] int dwOutputNum,
            [In] long cnsSampleTime,
            [In] long cnsSampleDuration,
            [In] int dwFlags,
            [In] INSSBuffer pSample,
            [In] IntPtr pvContext
            );
    }


    [Guid("96406BEB-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderCallbackAdvanced
    {
        [PreserveSig]
        int OnStreamSample(
            [In] short wStreamNum,
            [In] long cnsSampleTime,
            [In] long cnsSampleDuration,
            [In] int dwFlags,
            [In] INSSBuffer pSample,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int OnTime(
            [In] long cnsCurrentTime,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int OnStreamSelection(
            [In] short wStreamCount,
            [In] short [] pStreamNumbers,
            [In] StreamSelection [] pSelections,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int OnOutputPropsChanged(
            [In] int dwOutputNum,
            [In] AMMediaType pMediaType,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int AllocateForStream(
            [In] short wStreamNum,
            [In] int cbBuffer,
            out INSSBuffer ppBuffer,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int AllocateForOutput(
            [In] int dwOutputNum,
            [In] int cbBuffer,
            out INSSBuffer ppBuffer,
            [In] IntPtr pvContext
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BEC-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMReaderNetworkConfig
    {
        [PreserveSig]
        int GetBufferingTime(
            out long pcnsBufferingTime
            );

        [PreserveSig]
        int SetBufferingTime(
            [In] long cnsBufferingTime
            );

        [PreserveSig]
        int GetUDPPortRanges(
            out WMPortNumberRange pRangeArray,
            ref int pcRanges
            );

        [PreserveSig]
        int SetUDPPortRanges(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMPortNumberRange pRangeArray,
            [In] int cRanges
            );

        [PreserveSig]
        int GetProxySettings(
            [In] string pwszProtocol,
            out ProxySettings pProxySetting
            );

        [PreserveSig]
        int SetProxySettings(
            [In] string pwszProtocol,
            [In] ProxySettings ProxySetting
            );

        [PreserveSig]
        int GetProxyHostName(
            [In] string pwszProtocol,
            [Out] StringBuilder pwszHostName,
            ref int pcchHostName
            );

        [PreserveSig]
        int SetProxyHostName(
            [In] string pwszProtocol,
            [In] string pwszHostName
            );

        [PreserveSig]
        int GetProxyPort(
            [In] string pwszProtocol,
            out int pdwPort
            );

        [PreserveSig]
        int SetProxyPort(
            [In] string pwszProtocol,
            [In] int dwPort
            );

        [PreserveSig]
        int GetProxyExceptionList(
            [In] string pwszProtocol,
            [Out] StringBuilder pwszExceptionList,
            ref int pcchExceptionList
            );

        [PreserveSig]
        int SetProxyExceptionList(
            [In] string pwszProtocol,
            [In] string pwszExceptionList
            );

        [PreserveSig]
        int GetProxyBypassForLocal(
            [In] string pwszProtocol,
            [MarshalAs(UnmanagedType.Bool)] out bool pfBypassForLocal
            );

        [PreserveSig]
        int SetProxyBypassForLocal(
            [In] string pwszProtocol,
            [In, MarshalAs(UnmanagedType.Bool)] bool fBypassForLocal
            );

        [PreserveSig]
        int GetForceRerunAutoProxyDetection(
            [MarshalAs(UnmanagedType.Bool)] out bool pfForceRerunDetection
            );

        [PreserveSig]
        int SetForceRerunAutoProxyDetection(
            [In, MarshalAs(UnmanagedType.Bool)] bool fForceRerunDetection
            );

        [PreserveSig]
        int GetEnableMulticast(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableMulticast
            );

        [PreserveSig]
        int SetEnableMulticast(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableMulticast
            );

        [PreserveSig]
        int GetEnableHTTP(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableHTTP
            );

        [PreserveSig]
        int SetEnableHTTP(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableHTTP
            );

        [PreserveSig]
        int GetEnableUDP(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableUDP
            );

        [PreserveSig]
        int SetEnableUDP(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableUDP
            );

        [PreserveSig]
        int GetEnableTCP(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableTCP
            );

        [PreserveSig]
        int SetEnableTCP(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableTCP
            );

        [PreserveSig]
        int ResetProtocolRollover();

        [PreserveSig]
        int GetConnectionBandwidth(
            out int pdwConnectionBandwidth
            );

        [PreserveSig]
        int SetConnectionBandwidth(
            [In] int dwConnectionBandwidth
            );

        [PreserveSig]
        int GetNumProtocolsSupported(
            out int pcProtocols
            );

        [PreserveSig]
        int GetSupportedProtocolName(
            [In] int dwProtocolNum,
            [Out] StringBuilder pwszProtocolName,
            ref int pcchProtocolName
            );

        [PreserveSig]
        int AddLoggingUrl(
            [In] string pwszURL
            );

        [PreserveSig]
        int GetLoggingUrl(
            [In] int dwIndex,
            [Out] StringBuilder pwszURL,
            ref int pcchURL
            );

        [PreserveSig]
        int GetLoggingUrlCount(
            out int pdwUrlCount
            );

        [PreserveSig]
        int ResetLoggingUrlList();
    }


    [Guid("D979A853-042B-4050-8387-C939DB22013F"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderNetworkConfig2 : IWMReaderNetworkConfig
    {
        #region IWMReaderNetworkConfig Methods

        [PreserveSig]
        new int GetBufferingTime(
            out long pcnsBufferingTime
            );

        [PreserveSig]
        new int SetBufferingTime(
            [In] long cnsBufferingTime
            );

        [PreserveSig]
        new int GetUDPPortRanges(
            out WMPortNumberRange pRangeArray,
            ref int pcRanges
            );

        [PreserveSig]
        new int SetUDPPortRanges(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMPortNumberRange pRangeArray,
            [In] int cRanges
            );

        [PreserveSig]
        new int GetProxySettings(
            [In] string pwszProtocol,
            out ProxySettings pProxySetting
            );

        [PreserveSig]
        new int SetProxySettings(
            [In] string pwszProtocol,
            [In] ProxySettings ProxySetting
            );

        [PreserveSig]
        new int GetProxyHostName(
            [In] string pwszProtocol,
            [Out] StringBuilder pwszHostName,
            ref int pcchHostName
            );

        [PreserveSig]
        new int SetProxyHostName(
            [In] string pwszProtocol,
            [In] string pwszHostName
            );

        [PreserveSig]
        new int GetProxyPort(
            [In] string pwszProtocol,
            out int pdwPort
            );

        [PreserveSig]
        new int SetProxyPort(
            [In] string pwszProtocol,
            [In] int dwPort
            );

        [PreserveSig]
        new int GetProxyExceptionList(
            [In] string pwszProtocol,
            [Out] StringBuilder pwszExceptionList,
            ref int pcchExceptionList
            );

        [PreserveSig]
        new int SetProxyExceptionList(
            [In] string pwszProtocol,
            [In] string pwszExceptionList
            );

        [PreserveSig]
        new int GetProxyBypassForLocal(
            [In] string pwszProtocol,
            [MarshalAs(UnmanagedType.Bool)] out bool pfBypassForLocal
            );

        [PreserveSig]
        new int SetProxyBypassForLocal(
            [In] string pwszProtocol,
            [In, MarshalAs(UnmanagedType.Bool)] bool fBypassForLocal
            );

        [PreserveSig]
        new int GetForceRerunAutoProxyDetection(
            [MarshalAs(UnmanagedType.Bool)] out bool pfForceRerunDetection
            );

        [PreserveSig]
        new int SetForceRerunAutoProxyDetection(
            [In, MarshalAs(UnmanagedType.Bool)] bool fForceRerunDetection
            );

        [PreserveSig]
        new int GetEnableMulticast(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableMulticast
            );

        [PreserveSig]
        new int SetEnableMulticast(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableMulticast
            );

        [PreserveSig]
        new int GetEnableHTTP(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableHTTP
            );

        [PreserveSig]
        new int SetEnableHTTP(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableHTTP
            );

        [PreserveSig]
        new int GetEnableUDP(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableUDP
            );

        [PreserveSig]
        new int SetEnableUDP(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableUDP
            );

        [PreserveSig]
        new int GetEnableTCP(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableTCP
            );

        [PreserveSig]
        new int SetEnableTCP(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableTCP
            );

        [PreserveSig]
        new int ResetProtocolRollover();

        [PreserveSig]
        new int GetConnectionBandwidth(
            out int pdwConnectionBandwidth
            );

        [PreserveSig]
        new int SetConnectionBandwidth(
            [In] int dwConnectionBandwidth
            );

        [PreserveSig]
        new int GetNumProtocolsSupported(
            out int pcProtocols
            );

        [PreserveSig]
        new int GetSupportedProtocolName(
            [In] int dwProtocolNum,
            [Out] StringBuilder pwszProtocolName,
            ref int pcchProtocolName
            );

        [PreserveSig]
        new int AddLoggingUrl(
            [In] string pwszURL
            );

        [PreserveSig]
        new int GetLoggingUrl(
            [In] int dwIndex,
            [Out] StringBuilder pwszURL,
            ref int pcchURL
            );

        [PreserveSig]
        new int GetLoggingUrlCount(
            out int pdwUrlCount
            );

        [PreserveSig]
        new int ResetLoggingUrlList();

        #endregion

        [PreserveSig]
        int GetEnableContentCaching(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableContentCaching
            );

        [PreserveSig]
        int SetEnableContentCaching(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableContentCaching
            );

        [PreserveSig]
        int GetEnableFastCache(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableFastCache
            );

        [PreserveSig]
        int SetEnableFastCache(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableFastCache
            );

        [PreserveSig]
        int GetAcceleratedStreamingDuration(
            out long pcnsAccelDuration
            );

        [PreserveSig]
        int SetAcceleratedStreamingDuration(
            [In] long cnsAccelDuration
            );

        [PreserveSig]
        int GetAutoReconnectLimit(
            out int pdwAutoReconnectLimit
            );

        [PreserveSig]
        int SetAutoReconnectLimit(
            [In] int dwAutoReconnectLimit
            );

        [PreserveSig]
        int GetEnableResends(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableResends
            );

        [PreserveSig]
        int SetEnableResends(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableResends
            );

        [PreserveSig]
        int GetEnableThinning(
            [MarshalAs(UnmanagedType.Bool)] out bool pfEnableThinning
            );

        [PreserveSig]
        int SetEnableThinning(
            [In, MarshalAs(UnmanagedType.Bool)] bool fEnableThinning
            );

        [PreserveSig]
        int GetMaxNetPacketSize(
            out int pdwMaxNetPacketSize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BED-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMReaderStreamClock
    {
        [PreserveSig]
        int GetTime(
            [In] ref long pcnsNow
            );

        [PreserveSig]
        int SetTimer(
            [In] long cnsWhen,
            [In] IntPtr pvParam,
            out int pdwTimerId
            );

        [PreserveSig]
        int KillTimer(
            [In] int dwTimerId
            );
    }


    [Guid("F369E2F0-E081-4FE6-8450-B810B2F410D1"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMReaderTimecode
    {
        [PreserveSig]
        int GetTimecodeRangeCount(
            [In] short wStreamNum,
            out short pwRangeCount
            );

        [PreserveSig]
        int GetTimecodeRangeBounds(
            [In] short wStreamNum,
            [In] short wRangeNum,
            out int pStartTimecode,
            out int pEndTimecode
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("FDBE5592-81A1-41EA-93BD-735CAD1ADC05")]
    public interface IWMReaderTypeNegotiation
    {
        [PreserveSig]
        int TryOutputProps(
            [In] int dwOutputNum,
            [In] IWMOutputMediaProps pOutput
            );
    }


    [Guid("CF4B1F99-4DE2-4E49-A363-252740D99BC1"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMRegisterCallback
    {
        [PreserveSig]
        int Advise(
            [In] IWMStatusCallback pCallback,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int Unadvise(
            [In] IWMStatusCallback pCallback,
            [In] IntPtr pvContext
            );
    }


    [Guid("6D7CDC70-9888-11D3-8EDC-00C04F6109CF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMStatusCallback
    {
        [PreserveSig]
        int OnStatus(
            [In] Status Status,
            [In, MarshalAs(UnmanagedType.Error)] int hr,
            [In] AttrDataType dwType,
            [In] byte [] pValue,
            [In] IntPtr pvContext
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BDC-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMStreamConfig
    {
        [PreserveSig]
        int GetStreamType(
            out Guid pguidStreamType
            );

        [PreserveSig]
        int GetStreamNumber(
            out short pwStreamNum
            );

        [PreserveSig]
        int SetStreamNumber(
            [In] short wStreamNum
            );

        [PreserveSig]
        int GetStreamName(
            [Out] StringBuilder pwszStreamName,
            ref short pcchStreamName
            );

        [PreserveSig]
        int SetStreamName(
            [In] string pwszStreamName
            );

        [PreserveSig]
        int GetConnectionName(
            [Out] StringBuilder pwszInputName,
            ref short pcchInputName
            );

        [PreserveSig]
        int SetConnectionName(
            [In] string pwszInputName
            );

        [PreserveSig]
        int GetBitrate(
            out int pdwBitrate
            );

        [PreserveSig]
        int SetBitrate(
            [In] int pdwBitrate
            );

        [PreserveSig]
        int GetBufferWindow(
            out int pmsBufferWindow
            );

        [PreserveSig]
        int SetBufferWindow(
            [In] int msBufferWindow
            );
    }


    [Guid("7688D8CB-FC0D-43BD-9459-5A8DEC200CFA"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMStreamConfig2 : IWMStreamConfig
    {
        #region IWMStreamConfig Methods

        [PreserveSig]
        new int GetStreamType(
            out Guid pguidStreamType
            );

        [PreserveSig]
        new int GetStreamNumber(
            out short pwStreamNum
            );

        [PreserveSig]
        new int SetStreamNumber(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int GetStreamName(
            [Out] StringBuilder pwszStreamName,
            ref short pcchStreamName
            );

        [PreserveSig]
        new int SetStreamName(
            [In] string pwszStreamName
            );

        [PreserveSig]
        new int GetConnectionName(
            [Out] StringBuilder pwszInputName,
            ref short pcchInputName
            );

        [PreserveSig]
        new int SetConnectionName(
            [In] string pwszInputName
            );

        [PreserveSig]
        new int GetBitrate(
            out int pdwBitrate
            );

        [PreserveSig]
        new int SetBitrate(
            [In] int pdwBitrate
            );

        [PreserveSig]
        new int GetBufferWindow(
            out int pmsBufferWindow
            );

        [PreserveSig]
        new int SetBufferWindow(
            [In] int msBufferWindow
            );

        #endregion

        [PreserveSig]
        int GetTransportType(
            out TransportType pnTransportType
            );

        [PreserveSig]
        int SetTransportType(
            [In] TransportType nTransportType
            );

        [PreserveSig]
        int AddDataUnitExtension(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidExtensionSystemID,
            [In] short cbExtensionDataSize,
            [In] byte [] pbExtensionSystemInfo,
            [In] int cbExtensionSystemInfo
            );

        [PreserveSig]
        int GetDataUnitExtensionCount(
            out short pcDataUnitExtensions
            );

        [PreserveSig]
        int GetDataUnitExtension(
            [In] short wDataUnitExtensionNumber,
            out Guid pguidExtensionSystemID,
            out short pcbExtensionDataSize,
            out byte [] pbExtensionSystemInfo,
            ref int pcbExtensionSystemInfo
            );

        [PreserveSig]
        int RemoveAllDataUnitExtensions();
    }


    [Guid("CB164104-3AA9-45A7-9AC9-4DAEE131D6E1"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMStreamConfig3 : IWMStreamConfig2
    {
        #region IWMStreamConfig Methods

        [PreserveSig]
        new int GetStreamType(
            out Guid pguidStreamType
            );

        [PreserveSig]
        new int GetStreamNumber(
            out short pwStreamNum
            );

        [PreserveSig]
        new int SetStreamNumber(
            [In] short wStreamNum
            );

        [PreserveSig]
        new int GetStreamName(
            [Out] StringBuilder pwszStreamName,
            ref short pcchStreamName
            );

        [PreserveSig]
        new int SetStreamName(
            [In] string pwszStreamName
            );

        [PreserveSig]
        new int GetConnectionName(
            [Out] StringBuilder pwszInputName,
            ref short pcchInputName
            );

        [PreserveSig]
        new int SetConnectionName(
            [In] string pwszInputName
            );

        [PreserveSig]
        new int GetBitrate(
            out int pdwBitrate
            );

        [PreserveSig]
        new int SetBitrate(
            [In] int pdwBitrate
            );

        [PreserveSig]
        new int GetBufferWindow(
            out int pmsBufferWindow
            );

        [PreserveSig]
        new int SetBufferWindow(
            [In] int msBufferWindow
            );

        #endregion

        #region IWMStreamConfig2 Methods

        [PreserveSig]
        new int GetTransportType(
            out TransportType pnTransportType
            );

        [PreserveSig]
        new int SetTransportType(
            [In] TransportType nTransportType
            );

        [PreserveSig]
        new int AddDataUnitExtension(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidExtensionSystemID,
            [In] short cbExtensionDataSize,
            [In] byte [] pbExtensionSystemInfo,
            [In] int cbExtensionSystemInfo
            );

        [PreserveSig]
        new int GetDataUnitExtensionCount(
            out short pcDataUnitExtensions
            );

        [PreserveSig]
        new int GetDataUnitExtension(
            [In] short wDataUnitExtensionNumber,
            out Guid pguidExtensionSystemID,
            out short pcbExtensionDataSize,
            out byte [] pbExtensionSystemInfo,
            ref int pcbExtensionSystemInfo
            );

        [PreserveSig]
        new int RemoveAllDataUnitExtensions();

        #endregion

        [PreserveSig]
        int GetLanguage(
            [Out] StringBuilder pwszLanguageString,
            ref short pcchLanguageStringLength
            );

        [PreserveSig]
        int SetLanguage(
            [In] string pwszLanguageString
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BDD-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMStreamList
    {
        [PreserveSig]
        int GetStreams(
            out short [] pwStreamNumArray,
            ref short pcStreams
            );

        [PreserveSig]
        int AddStream(
            [In] short wStreamNum
            );

        [PreserveSig]
        int RemoveStream(
            [In] short wStreamNum
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("8C1C6090-F9A8-4748-8EC3-DD1108BA1E77")]
    public interface IWMStreamPrioritization
    {
        [PreserveSig]
        int GetPriorityRecords(
            out WMStreamPrioritizationRecord pRecordArray,
            ref short pcRecords
            );

        [PreserveSig]
        int SetPriorityRecords(
            [In, MarshalAs(UnmanagedType.LPStruct)] WMStreamPrioritizationRecord pRecordArray,
            [In] short cRecords
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("9397F121-7705-4DC9-B049-98B698188414")]
    public interface IWMSyncReader
    {
        [PreserveSig]
        int Open(
            [In] string pwszFilename
            );

        [PreserveSig]
        int Close();

        [PreserveSig]
        int SetRange(
            [In] long cnsStartTime,
            [In] long cnsDuration
            );

        [PreserveSig]
        int SetRangeByFrame(
            [In] short wStreamNum,
            [In] long qwFrameNumber,
            [In] long cFramesToRead
            );

        [PreserveSig]
        int GetNextSample(
            [In] short wStreamNum,
            out INSSBuffer ppSample,
            out long pcnsSampleTime,
            out long pcnsDuration,
            out int pdwFlags,
            out int pdwOutputNum,
            out short pwStreamNum
            );

        [PreserveSig]
        int SetStreamsSelected(
            [In] short cStreamCount,
            [In] short [] pwStreamNumbers,
            [In] StreamSelection [] pSelections
            );

        [PreserveSig]
        int GetStreamSelected(
            [In] short wStreamNum,
            out StreamSelection pSelection
            );

        [PreserveSig]
        int SetReadStreamSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fCompressed
            );

        [PreserveSig]
        int GetReadStreamSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfCompressed
            );

        [PreserveSig]
        int GetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int SetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        int GetOutputCount(
            out int pcOutputs
            );

        [PreserveSig]
        int GetOutputProps(
            [In] int dwOutputNum,
            out IWMOutputMediaProps ppOutput
            );

        [PreserveSig]
        int SetOutputProps(
            [In] int dwOutputNum,
            [In] IWMOutputMediaProps pOutput
            );

        [PreserveSig]
        int GetOutputFormatCount(
            [In] int dwOutputNum,
            out int pcFormats
            );

        [PreserveSig]
        int GetOutputFormat(
            [In] int dwOutputNum,
            [In] int dwFormatNum,
            out IWMOutputMediaProps ppProps
            );

        [PreserveSig]
        int GetOutputNumberForStream(
            [In] short wStreamNum,
            out int pdwOutputNum
            );

        [PreserveSig]
        int GetStreamNumberForOutput(
            [In] int dwOutputNum,
            out short pwStreamNum
            );

        [PreserveSig]
        int GetMaxOutputSampleSize(
            [In] int dwOutput,
            out int pcbMax
            );

        [PreserveSig]
        int GetMaxStreamSampleSize(
            [In] short wStream,
            out int pcbMax
            );

        [PreserveSig]
        int OpenStream(
            [In] UCOMIStream pStream
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("FAED3D21-1B6B-4AF7-8CB6-3E189BBC187B")]
    public interface IWMSyncReader2 : IWMSyncReader
    {
        #region IWMSyncReader Methods

        [PreserveSig]
        new int Open(
            [In] string pwszFilename
            );

        [PreserveSig]
        new int Close();

        [PreserveSig]
        new int SetRange(
            [In] long cnsStartTime,
            [In] long cnsDuration
            );

        [PreserveSig]
        new int SetRangeByFrame(
            [In] short wStreamNum,
            [In] long qwFrameNumber,
            [In] long cFramesToRead
            );

        [PreserveSig]
        new int GetNextSample(
            [In] short wStreamNum,
            out INSSBuffer ppSample,
            out long pcnsSampleTime,
            out long pcnsDuration,
            out int pdwFlags,
            out int pdwOutputNum,
            out short pwStreamNum
            );

        [PreserveSig]
        new int SetStreamsSelected(
            [In] short cStreamCount,
            [In] short [] pwStreamNumbers,
            [In] StreamSelection [] pSelections
            );

        [PreserveSig]
        new int GetStreamSelected(
            [In] short wStreamNum,
            out StreamSelection pSelection
            );

        [PreserveSig]
        new int SetReadStreamSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fCompressed
            );

        [PreserveSig]
        new int GetReadStreamSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfCompressed
            );

        [PreserveSig]
        new int GetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        new int SetOutputSetting(
            [In] int dwOutputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        [PreserveSig]
        new int GetOutputCount(
            out int pcOutputs
            );

        [PreserveSig]
        new int GetOutputProps(
            [In] int dwOutputNum,
            out IWMOutputMediaProps ppOutput
            );

        [PreserveSig]
        new int SetOutputProps(
            [In] int dwOutputNum,
            [In] IWMOutputMediaProps pOutput
            );

        [PreserveSig]
        new int GetOutputFormatCount(
            [In] int dwOutputNum,
            out int pcFormats
            );

        [PreserveSig]
        new int GetOutputFormat(
            [In] int dwOutputNum,
            [In] int dwFormatNum,
            out IWMOutputMediaProps ppProps
            );

        [PreserveSig]
        new int GetOutputNumberForStream(
            [In] short wStreamNum,
            out int pdwOutputNum
            );

        [PreserveSig]
        new int GetStreamNumberForOutput(
            [In] int dwOutputNum,
            out short pwStreamNum
            );

        [PreserveSig]
        new int GetMaxOutputSampleSize(
            [In] int dwOutput,
            out int pcbMax
            );

        [PreserveSig]
        new int GetMaxStreamSampleSize(
            [In] short wStream,
            out int pcbMax
            );

        [PreserveSig]
        new int OpenStream(
            [In] UCOMIStream pStream
            );

        #endregion

        [PreserveSig]
        int SetRangeByTimecode(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.LPStruct)] TimeCodeExtensionData pStart,
            [In, MarshalAs(UnmanagedType.LPStruct)] TimeCodeExtensionData pEnd
            );

        [PreserveSig]
        int SetRangeByFrameEx(
            [In] short wStreamNum,
            [In] long qwFrameNumber,
            [In] long cFramesToRead,
            out long pcnsStartTime
            );

        [PreserveSig]
        int SetAllocateForOutput(
            [In] int dwOutputNum,
            [In] IWMReaderAllocatorEx pAllocator
            );

        [PreserveSig]
        int GetAllocateForOutput(
            [In] int dwOutputNum,
            out IWMReaderAllocatorEx ppAllocator
            );

        [PreserveSig]
        int SetAllocateForStream(
            [In] short wStreamNum,
            [In] IWMReaderAllocatorEx pAllocator
            );

        [PreserveSig]
        int GetAllocateForStream(
            [In] short dwSreamNum,
            out IWMReaderAllocatorEx ppAllocator
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BCF-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMVideoMediaProps : IWMMediaProps
    {
        #region IWMMediaProps Methods

        [PreserveSig]
        new int GetType(
            out Guid pguidType
            );

        [PreserveSig]
        new int GetMediaType(
            out AMMediaType pType,
            ref int pcbType
            );

        [PreserveSig]
        new int SetMediaType(
            [In] AMMediaType pType
            );

        #endregion

        [PreserveSig]
        int GetMaxKeyFrameSpacing(
            out long pllTime
            );

        [PreserveSig]
        int SetMaxKeyFrameSpacing(
            [In] long llTime
            );

        [PreserveSig]
        int GetQuality(
            out int pdwQuality
            );

        [PreserveSig]
        int SetQuality(
            [In] int dwQuality
            );
    }


    [Guid("6F497062-F2E2-4624-8EA7-9DD40D81FC8D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWatermarkInfo
    {
        [PreserveSig]
        int GetWatermarkEntryCount(
            [In] WaterMarkEntryType wmetType,
            out int pdwCount
            );

        [PreserveSig]
        int GetWatermarkEntry(
            [In] WaterMarkEntryType wmetType,
            [In] int dwEntryNum,
            out WaterMarkEntry pEntry
            );
    }


    [Guid("96406BD4-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriter
    {
        [PreserveSig]
        int SetProfileByID(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidProfile
            );

        [PreserveSig]
        int SetProfile(
            [In] IWMProfile pProfile
            );

        [PreserveSig]
        int SetOutputFilename(
            [In] string pwszFilename
            );

        [PreserveSig]
        int GetInputCount(
            out int pcInputs
            );

        [PreserveSig]
        int GetInputProps(
            [In] int dwInputNum,
            out IWMInputMediaProps ppInput
            );

        [PreserveSig]
        int SetInputProps(
            [In] int dwInputNum,
            [In] IWMInputMediaProps pInput
            );

        [PreserveSig]
        int GetInputFormatCount(
            [In] int dwInputNumber,
            out int pcFormats
            );

        [PreserveSig]
        int GetInputFormat(
            [In] int dwInputNumber,
            [In] int dwFormatNumber,
            out IWMInputMediaProps pProps
            );

        [PreserveSig]
        int BeginWriting();

        [PreserveSig]
        int EndWriting();

        [PreserveSig]
        int AllocateSample(
            [In] int dwSampleSize,
            out INSSBuffer ppSample
            );

        [PreserveSig]
        int WriteSample(
            [In] int dwInputNum,
            [In] long cnsSampleTime,
            [In] WriteFlags dwFlags,
            [In] INSSBuffer pSample
            );

        [PreserveSig]
        int Flush();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BE3-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMWriterAdvanced
    {
        [PreserveSig]
        int GetSinkCount(
            out int pcSinks
            );

        [PreserveSig]
        int GetSink(
            [In] int dwSinkNum,
            out IWMWriterSink ppSink
            );

        [PreserveSig]
        int AddSink(
            [In] IWMWriterSink pSink
            );

        [PreserveSig]
        int RemoveSink(
            [In] IWMWriterSink pSink
            );

        [PreserveSig]
        int WriteStreamSample(
            [In] short wStreamNum,
            [In] long cnsSampleTime,
            [In] int msSampleSendTime,
            [In] long cnsSampleDuration,
            [In] WriteFlags dwFlags,
            [In] INSSBuffer pSample
            );

        [PreserveSig]
        int SetLiveSource(
            [In, MarshalAs(UnmanagedType.Bool)] bool fIsLiveSource
            );

        [PreserveSig]
        int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        int GetWriterTime(
            out long pcnsCurrentTime
            );

        [PreserveSig]
        int GetStatistics(
            [In] short wStreamNum,
            out WriterStatistics pStats
            );

        [PreserveSig]
        int SetSyncTolerance(
            [In] int msWindow
            );

        [PreserveSig]
        int GetSyncTolerance(
            out int pmsWindow
            );
    }


    [Guid("962DC1EC-C046-4DB8-9CC7-26CEAE500817"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterAdvanced2 : IWMWriterAdvanced
    {
        #region IWMWriterAdvanced Methods

        [PreserveSig]
        new int GetSinkCount(
            out int pcSinks
            );

        [PreserveSig]
        new int GetSink(
            [In] int dwSinkNum,
            out IWMWriterSink ppSink
            );

        [PreserveSig]
        new int AddSink(
            [In] IWMWriterSink pSink
            );

        [PreserveSig]
        new int RemoveSink(
            [In] IWMWriterSink pSink
            );

        [PreserveSig]
        new int WriteStreamSample(
            [In] short wStreamNum,
            [In] long cnsSampleTime,
            [In] int msSampleSendTime,
            [In] long cnsSampleDuration,
            [In] WriteFlags dwFlags,
            [In] INSSBuffer pSample
            );

        [PreserveSig]
        new int SetLiveSource(
            [In, MarshalAs(UnmanagedType.Bool)] bool fIsLiveSource
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int GetWriterTime(
            out long pcnsCurrentTime
            );

        [PreserveSig]
        new int GetStatistics(
            [In] short wStreamNum,
            out WriterStatistics pStats
            );

        [PreserveSig]
        new int SetSyncTolerance(
            [In] int msWindow
            );

        [PreserveSig]
        new int GetSyncTolerance(
            out int pmsWindow
            );

        #endregion

        [PreserveSig]
        int GetInputSetting(
            [In] int dwInputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        [PreserveSig]
        int SetInputSetting(
            [In] int dwInputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("2CD6492D-7C37-4E76-9D3B-59261183A22E")]
    public interface IWMWriterAdvanced3 : IWMWriterAdvanced2
    {
        #region IWMWriterAdvanced Methods

        [PreserveSig]
        new int GetSinkCount(
            out int pcSinks
            );

        [PreserveSig]
        new int GetSink(
            [In] int dwSinkNum,
            out IWMWriterSink ppSink
            );

        [PreserveSig]
        new int AddSink(
            [In] IWMWriterSink pSink
            );

        [PreserveSig]
        new int RemoveSink(
            [In] IWMWriterSink pSink
            );

        [PreserveSig]
        new int WriteStreamSample(
            [In] short wStreamNum,
            [In] long cnsSampleTime,
            [In] int msSampleSendTime,
            [In] long cnsSampleDuration,
            [In] WriteFlags dwFlags,
            [In] INSSBuffer pSample
            );

        [PreserveSig]
        new int SetLiveSource(
            [In, MarshalAs(UnmanagedType.Bool)] bool fIsLiveSource
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int GetWriterTime(
            out long pcnsCurrentTime
            );

        [PreserveSig]
        new int GetStatistics(
            [In] short wStreamNum,
            out WriterStatistics pStats
            );

        [PreserveSig]
        new int SetSyncTolerance(
            [In] int msWindow
            );

        [PreserveSig]
        new int GetSyncTolerance(
            out int pmsWindow
            );

        [PreserveSig]
        new int GetInputSetting(
            [In] int dwInputNum,
            [In] string pszName,
            out AttrDataType pType,
            out byte [] pValue,
            ref short pcbLength
            );

        #endregion

        #region IWMWriterAdvanced2 Methods

        [PreserveSig]
        new int SetInputSetting(
            [In] int dwInputNum,
            [In] string pszName,
            [In] AttrDataType Type,
            [In] byte [] pValue,
            [In] short cbLength
            );

        #endregion

        [PreserveSig]
        int GetStatisticsEx(
            [In] short wStreamNum,
            out WMWriterStatisticsEx pStats
            );

        [PreserveSig]
        int SetNonBlocking();
    }


    [Guid("96406BE5-2B2B-11D3-B36B-00C04F6108FF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterFileSink : IWMWriterSink
    {
        #region IWMWriterSink Methods

        [PreserveSig]
        new int OnHeader(
            [In] INSSBuffer pHeader
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int AllocateDataUnit(
            [In] int cbDataUnit,
            out INSSBuffer ppDataUnit
            );

        [PreserveSig]
        new int OnDataUnit(
            [In] INSSBuffer pDataUnit
            );

        [PreserveSig]
        new int OnEndWriting();

        #endregion

        [PreserveSig]
        int Open(
            [In] string pwszFilename
            );
    }


    [Guid("14282BA7-4AEF-4205-8CE5-C229035A05BC"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterFileSink2 : IWMWriterFileSink
    {
        #region IWMWriterSink Methods

        [PreserveSig]
        new int OnHeader(
            [In] INSSBuffer pHeader
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int AllocateDataUnit(
            [In] int cbDataUnit,
            out INSSBuffer ppDataUnit
            );

        [PreserveSig]
        new int OnDataUnit(
            [In] INSSBuffer pDataUnit
            );

        [PreserveSig]
        new int OnEndWriting();

        #endregion

        #region IWMWriterFileSink Methods

        [PreserveSig]
        new int Open(
            [In] string pwszFilename
            );

        #endregion

        [PreserveSig]
        int Start(
            [In] long cnsStartTime
            );

        [PreserveSig]
        int Stop(
            [In] long cnsStopTime
            );

        [PreserveSig]
        int IsStopped(
            [MarshalAs(UnmanagedType.Bool)] out bool pfStopped
            );

        [PreserveSig]
        int GetFileDuration(
            out long pcnsDuration
            );

        [PreserveSig]
        int GetFileSize(
            out long pcbFile
            );

        [PreserveSig]
        int Close();

        [PreserveSig]
        int IsClosed(
            [MarshalAs(UnmanagedType.Bool)] out bool pfClosed
            );
    }


    [Guid("3FEA4FEB-2945-47A7-A1DD-C53A8FC4C45C"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterFileSink3 : IWMWriterFileSink2
    {
        #region IWMWriterSink Methods

        [PreserveSig]
        new int OnHeader(
            [In] INSSBuffer pHeader
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int AllocateDataUnit(
            [In] int cbDataUnit,
            out INSSBuffer ppDataUnit
            );

        [PreserveSig]
        new int OnDataUnit(
            [In] INSSBuffer pDataUnit
            );

        [PreserveSig]
        new int OnEndWriting();

        #endregion

        #region IWMWriterFileSink Methods

        [PreserveSig]
        new int Open(
            [In] string pwszFilename
            );

        #endregion

        #region IWMWriterFileSink2

        [PreserveSig]
        new int Start(
            [In] long cnsStartTime
            );

        [PreserveSig]
        new int Stop(
            [In] long cnsStopTime
            );

        [PreserveSig]
        new int IsStopped(
            [MarshalAs(UnmanagedType.Bool)] out bool pfStopped
            );

        [PreserveSig]
        new int GetFileDuration(
            out long pcnsDuration
            );

        [PreserveSig]
        new int GetFileSize(
            out long pcbFile
            );

        [PreserveSig]
        new int Close();

        [PreserveSig]
        new int IsClosed(
            [MarshalAs(UnmanagedType.Bool)] out bool pfClosed
            );

        #endregion

        [PreserveSig]
        int SetAutoIndexing(
            [In, MarshalAs(UnmanagedType.Bool)] bool fDoAutoIndexing
            );

        [PreserveSig]
        int GetAutoIndexing(
            [MarshalAs(UnmanagedType.Bool)] out bool pfAutoIndexing
            );

        [PreserveSig]
        int SetControlStream(
            [In] short wStreamNumber,
            [In, MarshalAs(UnmanagedType.Bool)] bool fShouldControlStartAndStop
            );

        [PreserveSig]
        int GetMode(
            out int pdwFileSinkMode
            );

        [PreserveSig]
        int OnDataUnitEx(
            [In, MarshalAs(UnmanagedType.LPStruct)] FileSinkDataUnit pFileSinkDataUnit
            );

        [PreserveSig]
        int SetUnbufferedIO(
            [In, MarshalAs(UnmanagedType.Bool)] bool fUnbufferedIO,
            [In, MarshalAs(UnmanagedType.Bool)] bool fRestrictMemUsage
            );

        [PreserveSig]
        int GetUnbufferedIO(
            [MarshalAs(UnmanagedType.Bool)] out bool pfUnbufferedIO
            );

        [PreserveSig]
        int CompleteOperations();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BE7-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMWriterNetworkSink : IWMWriterSink
    {
        #region IWMWriterSink Methods

        [PreserveSig]
        new int OnHeader(
            [In] INSSBuffer pHeader
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int AllocateDataUnit(
            [In] int cbDataUnit,
            out INSSBuffer ppDataUnit
            );

        [PreserveSig]
        new int OnDataUnit(
            [In] INSSBuffer pDataUnit
            );

        [PreserveSig]
        new int OnEndWriting();

        #endregion

        [PreserveSig]
        int SetMaximumClients(
            [In] int dwMaxClients
            );

        [PreserveSig]
        int GetMaximumClients(
            out int pdwMaxClients
            );

        [PreserveSig]
        int SetNetworkProtocol(
            [In] NetProtocol protocol
            );

        [PreserveSig]
        int GetNetworkProtocol(
            out NetProtocol pProtocol
            );

        [PreserveSig]
        int GetHostURL(
            [Out] StringBuilder pwszURL,
            ref int pcchURL
            );

        [PreserveSig]
        int Open(
            ref int pdwPortNum
            );

        [PreserveSig]
        int Disconnect();

        [PreserveSig]
        int Close();
    }


    [Guid("81E20CE4-75EF-491A-8004-FC53C45BDC3E"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterPostView
    {
        [PreserveSig]
        int SetPostViewCallback(
            IWMWriterPostViewCallback pCallback, IntPtr pvContext
            );

        [PreserveSig]
        int SetReceivePostViewSamples(
            [In] short wStreamNum,
            [In, MarshalAs(UnmanagedType.Bool)] bool fReceivePostViewSamples
            );

        [PreserveSig]
        int GetReceivePostViewSamples(
            [In] short wStreamNum,
            [MarshalAs(UnmanagedType.Bool)] out bool pfReceivePostViewSamples
            );

        [PreserveSig]
        int GetPostViewProps(
            [In] short wStreamNumber,
            out IWMMediaProps ppOutput
            );

        [PreserveSig]
        int SetPostViewProps(
            [In] short wStreamNumber,
            [In] IWMMediaProps pOutput
            );

        [PreserveSig]
        int GetPostViewFormatCount(
            [In] short wStreamNumber,
            out int pcFormats
            );

        [PreserveSig]
        int GetPostViewFormat(
            [In] short wStreamNumber,
            [In] int dwFormatNumber,
            out IWMMediaProps ppProps
            );

        [PreserveSig]
        int SetAllocateForPostView(
            [In] short wStreamNumber,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAllocate
            );

        [PreserveSig]
        int GetAllocateForPostView(
            [In] short wStreamNumber,
            [MarshalAs(UnmanagedType.Bool)] out bool pfAllocate
            );
    }


    [Guid("D9D6549D-A193-4F24-B308-03123D9B7F8D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterPostViewCallback : IWMStatusCallback
    {
        #region IWMStatusCallback Methods

        [PreserveSig]
        new int OnStatus(
            [In] Status Status,
            [In, MarshalAs(UnmanagedType.Error)] int hr,
            [In] AttrDataType dwType,
            [In] byte [] pValue,
            [In] IntPtr pvContext
            );

        #endregion

        [PreserveSig]
        int OnPostViewSample(
            [In] short wStreamNumber,
            [In] long cnsSampleTime,
            [In] long cnsSampleDuration,
            [In] WriteFlags dwFlags,
            [In] INSSBuffer pSample,
            [In] IntPtr pvContext
            );

        [PreserveSig]
        int AllocateForPostView(
            [In] short wStreamNum,
            [In] int cbBuffer,
            out INSSBuffer ppBuffer,
            [In] IntPtr pvContext
            );
    }


    [Guid("FC54A285-38C4-45B5-AA23-85B9F7CB424B"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterPreprocess
    {
        [PreserveSig]
        int GetMaxPreprocessingPasses(
            [In] int dwInputNum,
            [In] int dwFlags,
            out int pdwMaxNumPasses
            );

        [PreserveSig]
        int SetNumPreprocessingPasses(
            [In] int dwInputNum,
            [In] int dwFlags,
            [In] int dwNumPasses
            );

        [PreserveSig]
        int BeginPreprocessingPass(
            [In] int dwInputNum,
            [In] int dwFlags
            );

        [PreserveSig]
        int PreprocessSample(
            [In] int dwInputNum,
            [In] long cnsSampleTime,
            [In] int dwFlags,
            [In] INSSBuffer pSample
            );

        [PreserveSig]
        int EndPreprocessingPass(
            [In] int dwInputNum,
            [In] int dwFlags
            );
    }


    [Guid("DC10E6A5-072C-467D-BF57-6330A9DDE12A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMWriterPushSink : IWMWriterSink
    {
        #region IWMWriterSink Methods

        [PreserveSig]
        new int OnHeader(
            [In] INSSBuffer pHeader
            );

        [PreserveSig]
        new int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        new int AllocateDataUnit(
            [In] int cbDataUnit,
            out INSSBuffer ppDataUnit
            );

        [PreserveSig]
        new int OnDataUnit(
            [In] INSSBuffer pDataUnit
            );

        [PreserveSig]
        new int OnEndWriting();

        #endregion

        [PreserveSig]
        int Connect(
            [In] string pwszURL,
            [In] string pwszTemplateURL,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAutoDestroy
            );

        [PreserveSig]
        int Disconnect();

        [PreserveSig]
        int EndSession();
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("96406BE4-2B2B-11D3-B36B-00C04F6108FF")]
    public interface IWMWriterSink
    {
        [PreserveSig]
        int OnHeader(
            [In] INSSBuffer pHeader
            );

        [PreserveSig]
        int IsRealTime(
            [MarshalAs(UnmanagedType.Bool)] out bool pfRealTime
            );

        [PreserveSig]
        int AllocateDataUnit(
            [In] int cbDataUnit,
            out INSSBuffer ppDataUnit
            );

        [PreserveSig]
        int OnDataUnit(
            [In] INSSBuffer pDataUnit
            );

        [PreserveSig]
        int OnEndWriting();
    }


    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa"),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IServiceProvider
    {
        [PreserveSig]
        int QueryService(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid guidService,
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out, MarshalAs(UnmanagedType.IUnknown) ] out object ppvObject
            );
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("0C0E4080-9081-11D2-BEEC-0060082F2054")]
    public interface INSNetSourceCreator
    {
        [PreserveSig]
        int Initialize();

        [PreserveSig]
        int CreateNetSource(
            [In] string pszStreamName,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pMonitor,
            [In] byte [] pData,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUserContext,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pCallback,
            [In] long qwContext
            );

        [PreserveSig]
        int GetNetSourceProperties(
            [In] string pszStreamName,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppPropertiesNode
            );

        [PreserveSig]
        int GetNetSourceSharedNamespace(
            [MarshalAs(UnmanagedType.IUnknown)] out object ppSharedNamespace
            );

        [PreserveSig]
        int GetNetSourceAdminInterface(
            [In] string pszStreamName,
            [MarshalAs(UnmanagedType.Struct)] out object pVal
            );

        [PreserveSig]
        int GetNumProtocolsSupported(
            out int pcProtocols
            );

        [PreserveSig]
        int GetProtocolName(
            [In] int dwProtocolNum,
            [Out] StringBuilder pwszProtocolName,
            ref short pcchProtocolName
            );

        [PreserveSig]
        int Shutdown();
    }


    [Guid("4F528693-1035-43FE-B428-757561AD3A68"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INSSBuffer2 : INSSBuffer
    {
        #region INSSBuffer Methods

        [PreserveSig]
        new int GetLength(
            out int pdwLength
            );

        [PreserveSig]
        new int SetLength(
            [In] int dwLength
            );

        [PreserveSig]
        new int GetMaxLength(
            out int pdwLength
            );

        [PreserveSig]
        new int GetBuffer(
            out IntPtr ppdwBuffer
            );

        [PreserveSig]
        new int GetBufferAndLength(
            out IntPtr ppdwBuffer,
            out int pdwLength
            );

        #endregion

        [PreserveSig]
        int GetSampleProperties(
            [In] int cbProperties,
            out byte [] pbProperties
            );

        [PreserveSig]
        int SetSampleProperties(
            [In] int cbProperties,
            [In] byte [] pbProperties
            );
    }


    [ComConversionLoss, Guid("C87CEAAF-75BE-4BC4-84EB-AC2798507672"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INSSBuffer3 : INSSBuffer2
    {
        #region INSSBuffer Methods

        [PreserveSig]
        new int GetLength(
            out int pdwLength
            );

        [PreserveSig]
        new int SetLength(
            [In] int dwLength
            );

        [PreserveSig]
        new int GetMaxLength(
            out int pdwLength
            );

        [PreserveSig]
        new int GetBuffer(
            out IntPtr ppdwBuffer
            );

        [PreserveSig]
        new int GetBufferAndLength(
            out IntPtr ppdwBuffer,
            out int pdwLength
            );

        #endregion

        #region INSSBuffer2 Methods

        [PreserveSig]
        new int GetSampleProperties(
            [In] int cbProperties,
            out byte [] pbProperties
            );

        [PreserveSig]
        new int SetSampleProperties(
            [In] int cbProperties,
            [In] byte [] pbProperties
            );

        #endregion

        [PreserveSig]
        int SetProperty(
            [In] Guid guidBufferProperty,
            [In] IntPtr pvBufferProperty,
            [In] int dwBufferPropertySize
            );

        [PreserveSig]
        int GetProperty(
            [In] Guid guidBufferProperty,
            [Out] IntPtr pvBufferProperty,
            ref int pdwBufferPropertySize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("B6B8FD5A-32E2-49D4-A910-C26CC85465ED")]
    public interface INSSBuffer4 : INSSBuffer3
    {
        #region INSSBuffer Methods

        [PreserveSig]
        new int GetLength(
            out int pdwLength
            );

        [PreserveSig]
        new int SetLength(
            [In] int dwLength
            );

        [PreserveSig]
        new int GetMaxLength(
            out int pdwLength
            );

        [PreserveSig]
        new int GetBuffer(
            out IntPtr ppdwBuffer
            );

        [PreserveSig]
        new int GetBufferAndLength(
            out IntPtr ppdwBuffer,
            out int pdwLength
            );

        #endregion

        #region INSSBuffer2 Methods

        [PreserveSig]
        new int GetSampleProperties(
            [In] int cbProperties,
            out byte [] pbProperties
            );

        [PreserveSig]
        new int SetSampleProperties(
            [In] int cbProperties,
            [In] byte [] pbProperties
            );

        #endregion

        #region INSSBuffer3 Methods

        [PreserveSig]
        new int SetProperty(
            [In] Guid guidBufferProperty,
            [In] IntPtr pvBufferProperty,
            [In] int dwBufferPropertySize
            );

        [PreserveSig]
        new int GetProperty(
            [In] Guid guidBufferProperty,
            [Out] IntPtr pvBufferProperty,
            ref int pdwBufferPropertySize
            );

        #endregion

        [PreserveSig]
        int GetPropertyCount(
            out int pcBufferProperties
            );

        [PreserveSig]
        int GetPropertyByIndex(
            [In] int dwBufferPropertyIndex,
            out Guid pguidBufferProperty,
            [Out] IntPtr pvBufferProperty,
            ref int pdwBufferPropertySize
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("D98EE251-34E0-4A2D-9312-9B4C788D9FA1")]
    public interface IWMCodecAMVideoAccelerator
    {
        [PreserveSig]
        int SetAcceleratorInterface(
            [In, MarshalAs(UnmanagedType.Interface)] object pIAMVA
            );

        [PreserveSig]
        int NegotiateConnection(
            [In] AMMediaType pMediaType
            );

        [PreserveSig]
        int SetPlayerNotify(
            [In] IWMPlayerTimestampHook pHook
            );
    }


    [Guid("990641B0-739F-4E94-A808-9888DA8F75AF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecVideoAccelerator
    {
        [PreserveSig]
        int NegotiateConnection(
            [In, MarshalAs(UnmanagedType.Interface)] object pIAMVA,
            [In] AMMediaType pMediaType
            );

        [PreserveSig]
        int SetPlayerNotify(
            [In] IWMPlayerTimestampHook pHook
            );
    }


    [Guid("28580DDA-D98E-48D0-B7AE-69E473A02825"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMPlayerTimestampHook
    {
        [PreserveSig]
        int MapTimestamp(
            [In] long rtIn, out long prtOut
            );
    }


    [Guid("61103CA4-2033-11D2-9EF1-006097D2D7CF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMSBufferAllocator
    {
        [PreserveSig]
        int AllocateBuffer(
            [In] int dwMaxBufferSize,
            out INSSBuffer ppBuffer
            );

        [PreserveSig]
        int AllocatePageSizeBuffer(
            [In] int dwMaxBufferSize,
            out INSSBuffer ppBuffer
            );
    }


    [Guid("8BB23E5F-D127-4AFB-8D02-AE5B66D54C78"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMSInternalAdminNetSource
    {
        [PreserveSig]
        int Initialize(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pSharedNamespace,
            [In, MarshalAs(UnmanagedType.IUnknown)] object pNamespaceNode,
            [In] INSNetSourceCreator pNetSourceCreator,
            [In] int fEmbeddedInServer
            );

        [PreserveSig]
        int GetNetSourceCreator(
            out INSNetSourceCreator ppNetSourceCreator
            );

        [PreserveSig]
        int SetCredentials(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrName,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrPassword,
            [In, MarshalAs(UnmanagedType.Bool)] bool fPersist,
            [In, MarshalAs(UnmanagedType.Bool)] bool fConfirmedGood
            );

        [PreserveSig]
        int GetCredentials(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrName,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrPassword,
            [MarshalAs(UnmanagedType.Bool)] out bool pfConfirmedGood
            );

        [PreserveSig]
        int DeleteCredentials(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm
            );

        [PreserveSig]
        int GetCredentialFlags(
            out int lpdwFlags
            );

        [PreserveSig]
        int SetCredentialFlags(
            [In] int dwFlags
            );

        [PreserveSig]
        int FindProxyForURL(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrProtocol,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrHost,
            [MarshalAs(UnmanagedType.Bool)] out bool pfProxyEnabled,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrProxyServer,
            out int pdwProxyPort,
            ref int pdwProxyContext
            );

        [PreserveSig]
        int RegisterProxyFailure(
            [In, MarshalAs(UnmanagedType.Error)] int hrParam,
            [In] int dwProxyContext
            );

        [PreserveSig]
        int ShutdownProxyContext(
            [In] int dwProxyContext
            );

        [PreserveSig]
        int IsUsingIE(
            [In] int dwProxyContext,
            [MarshalAs(UnmanagedType.Bool)] out bool pfIsUsingIE
            );
    }


    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("E74D58C3-CF77-4B51-AF17-744687C43EAE")]
    public interface IWMSInternalAdminNetSource2
    {
        [PreserveSig]
        int SetCredentialsEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrName,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrPassword,
            [In, MarshalAs(UnmanagedType.Bool)] bool fPersist,
            [In, MarshalAs(UnmanagedType.Bool)] bool fConfirmedGood
            );

        [PreserveSig]
        int GetCredentialsEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy,
            out NetSourceURLCredPolicySettings pdwUrlPolicy,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrName,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrPassword,
            [MarshalAs(UnmanagedType.Bool)] out bool pfConfirmedGood
            );

        [PreserveSig]
        int DeleteCredentialsEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy
            );

        [PreserveSig]
        int FindProxyForURLEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrProtocol,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrHost,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [MarshalAs(UnmanagedType.Bool)] out bool pfProxyEnabled,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrProxyServer,
            out int pdwProxyPort,
            ref int pdwProxyContext
            );
    }


    [Guid("6B63D08E-4590-44AF-9EB3-57FF1E73BF80"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMSInternalAdminNetSource3 : IWMSInternalAdminNetSource2
    {
        #region IWMSInternalAdminNetSource2 Methods

        [PreserveSig]
        new int SetCredentialsEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrName,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrPassword,
            [In, MarshalAs(UnmanagedType.Bool)] bool fPersist,
            [In, MarshalAs(UnmanagedType.Bool)] bool fConfirmedGood
            );

        [PreserveSig]
        new int GetCredentialsEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy,
            out NetSourceURLCredPolicySettings pdwUrlPolicy,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrName,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrPassword,
            [MarshalAs(UnmanagedType.Bool)] out bool pfConfirmedGood
            );

        [PreserveSig]
        new int DeleteCredentialsEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy
            );

        [PreserveSig]
        new int FindProxyForURLEx(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrProtocol,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrHost,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [MarshalAs(UnmanagedType.Bool)] out bool pfProxyEnabled,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrProxyServer,
            out int pdwProxyPort,
            ref int pdwProxyContext
            );

        #endregion

        [PreserveSig]
        int GetNetSourceCreator2(
            [MarshalAs(UnmanagedType.IUnknown)] out object ppNetSourceCreator
            );

        [PreserveSig]
        int FindProxyForURLEx2(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrProtocol,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrHost,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [MarshalAs(UnmanagedType.Bool)] out bool pfProxyEnabled,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrProxyServer,
            out int pdwProxyPort,
            ref long pqwProxyContext
            );

        [PreserveSig]
        int RegisterProxyFailure2(
            [In, MarshalAs(UnmanagedType.Error)] int hrParam,
            [In] long qwProxyContext
            );

        [PreserveSig]
        int ShutdownProxyContext2(
            [In] long qwProxyContext
            );

        [PreserveSig]
        int IsUsingIE2(
            [In] long qwProxyContext,
            [MarshalAs(UnmanagedType.Bool)] out bool pfIsUsingIE
            );

        [PreserveSig]
        int SetCredentialsEx2(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrName,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrPassword,
            [In, MarshalAs(UnmanagedType.Bool)] bool fPersist,
            [In, MarshalAs(UnmanagedType.Bool)] bool fConfirmedGood,
            [In, MarshalAs(UnmanagedType.Bool)] bool fClearTextAuthentication
            );

        [PreserveSig]
        int GetCredentialsEx2(
            [In, MarshalAs(UnmanagedType.BStr)] string bstrRealm,
            [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl,
            [In, MarshalAs(UnmanagedType.Bool)] bool fProxy,
            [In, MarshalAs(UnmanagedType.Bool)] bool fClearTextAuthentication,
            out NetSourceURLCredPolicySettings pdwUrlPolicy,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrName,
            [MarshalAs(UnmanagedType.BStr)] out string pbstrPassword,
            [MarshalAs(UnmanagedType.Bool)] out bool pfConfirmedGood
            );
    }


    #endregion
}
