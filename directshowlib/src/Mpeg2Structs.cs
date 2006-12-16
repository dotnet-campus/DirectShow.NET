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

  // From Mpeg2Bits.h

  /// <summary>
  /// From PID_BITS & PID_BITS_MIDL
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct PidBits
  {
    public short Bits;

    public short Reserved
    {
      get { return (short)((int)Bits & 0x0007); }
    }

    public short ProgramId
    {
      get { return (short)(((int)Bits & 0xfff8) >> 3); }
    }
  }

  /// <summary>
  /// From MPEG_HEADER_BITS & MPEG_HEADER_BITS_MIDL
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct MpegHeaderBits
  {
    public short Bits;

    public short SectionLength
    {
      get { return (short)((int)Bits & 0x0fff); }
    }

    public short Reserved
    {
      get { return (short)(((int)Bits & 0x3000) >> 12); }
    }

    public short PrivateIndicator
    {
      get { return (short)(((int)Bits & 0x4000) >> 14); }
    }

    public short SectionSyntaxIndicator
    {
      get { return (short)(((int)Bits & 0x8000) >> 15); }
    }
  }

  /// <summary>
  /// From MPEG_HEADER_VERSION_BITS & MPEG_HEADER_VERSION_BITS_MIDL
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MpegHeaderVersionBits
  {
    public byte Bits;

    public byte CurrentNextIndicator
    {
      get { return (byte)((int)Bits & 0x1); }
    }

    public byte VersionNumber
    {
      get { return (byte)(((int)Bits & 0x3e) >> 1); }
    }

    public byte Reserved
    {
      get { return (byte)(((int)Bits & 0xc0) >> 6); }
    }
  }

#if ALLOW_UNTESTED_INTERFACES

  /// <summary>
  /// From MPEG_CURRENT_NEXT_BIT, MPEG_SECTION_IS_*
  /// </summary>
  public enum MpegSectionIs
  {
    Next = 0,
    Current = 1,
  }

  /// <summary>
  /// From TID_EXTENSION
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct TidExtension
  {
    public short wTidExt;
    public short wCount;
  }

  /// <summary>
  /// From SECTION
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct Section
  {
    public short TableId;
    public MpegHeaderBits Header;
    public byte SectionData; // Must be marshalled manually
  }

  /// <summary>
  /// From LONG_SECTION
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct LongSection
  {
    public short TableId;
    public MpegHeaderBits Header;
    public short TableIdExtension;
    public MpegHeaderVersionBits Version;
    public byte SectionNumber;
    public byte LastSectionNumber;
    public byte RemainingData; // Must be marshalled manually
  }

  /// <summary>
  /// From DSMCC_SECTION
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct DsmccSection
  {
    public short TableId;
    public MpegHeaderBits Header;
    public short TableIdExtension;
    public MpegHeaderVersionBits Version;
    public byte SectionNumber;
    public byte LastSectionNumber;
    public byte ProtocolDiscriminator;
    public byte DsmccType;
    public short MessageId;
    public int TransactionId;
    public byte Reserved;
    public byte AdaptationLength;
    public short MessageLength;
    public byte RemainingData; // Must be marshalled manually
  }

  /// <summary>
  /// From MPEG_RQST_PACKET
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MpegRqstPacket
  {
    public int dwLength;
    [MarshalAs(UnmanagedType.LPStruct)]
    public Section pSection;
  }

  /// <summary>
  /// From MPEG_PACKET_LIST
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public struct MpegPacketList
  {
    public short wPacketCount;
    public IntPtr PacketList; // MPEG_RQST_PACKET array
  }

  /// <summary>
  /// From DSMCC_FILTER_OPTIONS
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct DsmccFilterOptions
  {
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyProtocol;       
    public byte Protocol;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyType;           
    public byte Type;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyMessageId;      
    public short MessageId;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyTransactionId;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fUseTrxIdMessageIdMask; 
    public int TransactionId;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyModuleVersion;  
    public byte ModuleVersion;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyBlockNumber;    
    public short BlockNumber;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fGetModuleCall;         
    public short NumberOfBlocksInModule;
  }

  /// <summary>
  /// From ATSC_FILTER_OPTIONS
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct AtscFilterOptions
  {
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyEtmId;
    public int EtmId;
  }

  /// <summary>
  /// From MPEG2_FILTER
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct Mpeg2Filter
  {
    public byte bVersionNumber;
    public short wFilterSize;
    [MarshalAs(UnmanagedType.Bool)]
    public bool  fUseRawFilteringBits;
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType=UnmanagedType.U1, SizeConst=16)]
    public byte[] Filter;
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
    public byte[] Mask;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyTableIdExtension; 
    public short TableIdExtension;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyVersion;          
    public byte Version;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifySectionNumber;    
    public byte  SectionNumber;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyCurrentNext;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fNext;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyDsmccOptions;
    public DsmccFilterOptions Dsmcc;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fSpecifyAtscOptions;      
    public AtscFilterOptions Atsc;
  }

  /// <summary>
  /// From MPEG_STREAM_BUFFER
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MpegStreamBuffer
  {
    public int hr;
    public int dwDataBufferSize;
    public int dwSizeOfDataRead;
    public IntPtr pDataBuffer;
  }

  /// <summary>
  /// From MPEG_DURATION & MPEG_TIME
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MpegDuration
  {
    public byte Hours;
    public byte Minutes;
    public byte Seconds;
  }

  /// <summary>
  /// From MPEG_DATE
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MpegDate
  {
    public byte Date;
    public byte Month;
    public byte Year;
  }

  /// <summary>
  /// From MPEG_DATE_AND_TIME
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MpegDateAndTime
  {
    //public MpegDate D;
    //public MpegTime T;
    // Marshaling is faster like that...
    public byte Date;
    public byte Month;
    public byte Year;
    public byte Hours;
    public byte Minutes;
    public byte Seconds;
  }

  /// <summary>
  /// From MPEG_CONTEXT_TYPE
  /// </summary>
  public enum MpegContextType
  {
    BcsDemux,
    Winsock
  }

  /// <summary>
  /// From MPEG_BCS_DEMUX
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MpegBcsDemux
  {
    public int AVMGraphId;
  }

  /// <summary>
  /// From MPEG_WINSOCK
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MpegWinsock
  {
    public int AVMGraphId;
  }

  /// <summary>
  /// From MPEG_CONTEXT
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct MpegContext
  {
    public MpegContextType Type;
    public int AVMGraphId; // easier to marshal...
  }

  /// <summary>
  /// From MPEG_REQUEST_TYPE
  /// </summary>
  public enum MpegRequestType
  {
    Unknown = 0,
    GetSection,
    GetSectionAsync,
    GetTable,
    GetTableAsync,
    GetSectionsStream,
    GetPesStream,
    GetTsStream,
    StartMpeStream,
  }

  /// <summary>
  /// From MPEG_SERVICE_REQUEST
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public class MpegServiceRequest
  {
    public MpegRequestType Type;
    public MpegContext Context;
    public short Pid;
    public byte TableId;
    public MPEG2Filter Filter;
    public int Flags;
  }

  /// <summary>
  /// From MPEG_SERVICE_RESPONSE
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  public class MpegServiceResponse
  {
    public int IPAddress;
    public short Port;
  }

  /// <summary>
  /// From DSMCC_ELEMENT
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public class DsmccElement
  {
    public short pid;
    public byte bComponentTag;
    public int dwCarouselId;
    public int dwTransactionId;
    public DsmccElement pNext;
  }

  /// <summary>
  /// From MPE_ELEMENT
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public class MpeElement
  {
    public short pid;
    public byte bComponentTag;
    public MpeElement pNext;
  }

  /// <summary>
  /// From MPEG_STREAM_FILTER
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MpegStreamFilter
  {
    public short wPidValue;
    public int dwFilterSize;
    [MarshalAs(UnmanagedType.Bool)]
    public bool fCrcEnabled;
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
    public byte[] rgchFilter;
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
    public byte[] rgchMask;
  }
#endif

  #endregion

  #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

#endif

  #endregion
}
