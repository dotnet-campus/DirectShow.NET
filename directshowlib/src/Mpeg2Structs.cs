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

  ///...................

  /// <summary>
  /// From MPEG_TIME & MPEG_DURATION
  /// </summary>
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct MpegTime
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

#endif

  #endregion

  #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

#endif

  #endregion
}
