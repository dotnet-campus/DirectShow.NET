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

#define ALLOW_UNTESTED_CODE

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;

namespace DirectShowLib

{
#if ALLOW_UNTESTED_CODE
    #region COM Class Objects

    [ComVisible(false), ComImport,
    Guid("62BE5D10-60EB-11d0-BD3B-00A0C911CE86")]
    public class CreateDevEnum
    {
    }


    [ComVisible(false), ComImport,
    Guid("e436ebb3-524f-11ce-9f53-0020af0ba770")]
    public class FilterGraph
    {
    }


    [ComVisible(false), ComImport,
    Guid("BF87B6E1-8C27-11d0-B3F0-00AA003761C5")]
    public class CaptureGraphBuilder2
    {
    }


    [ComVisible(false), ComImport,
    Guid("C1F400A0-3F08-11d3-9F0B-006008039E37")]
    public class SampleGrabber
    {
    }


    [ComVisible(false), ComImport,
    Guid("FCC152B7-F372-11d0-8E00-00C04FD7C08B")]
    public class DvdGraphBuilder
    {
    }


    [ComVisible(false), ComImport,
    Guid("2DB47AE5-CF39-43c2-B4D6-0CD8D90946F4")]
    public class StreamBufferSink
    {
    }


    [ComVisible(false), ComImport,
    Guid("C9F5FE02-F851-4eb5-99EE-AD602AF1E619")]
    public class StreamBufferSource
    {
    }


    [ComVisible(false), ComImport,
    Guid("B87BEB7B-8D29-423f-AE4D-6582C10175AC")]
    public class VideoMixingRenderer
    {
    }


    [ComVisible(false), ComImport,
    Guid("51b4abf3-748f-4e3b-a276-c828330e926a")]
    public class VideoMixingRenderer9
    {
    }


    [ComVisible(false), ComImport,
    Guid("6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50")]
    public class VideoRendererDefault
    {
    }


    [ComVisible(false), ComImport,
    Guid("1b544c20-fd0b-11ce-8c63-00aa0044b51e")]
    public class AviSplitter
    {
    }


    [ComVisible(false), ComImport,
    Guid("CC58E280-8AA1-11d1-B3F1-00AA003761C5")]
    public class SmartTee
    {
    }


    [ComVisible(false), ComImport,
    Guid("C1F400A4-3F08-11d3-9F0B-006008039E37")]
    public class NullRenderer
    {
    }


    [ComVisible(false)]
    public class DsHlp
    {
        public const int OATRUE		= -1;
        public const int OAFALSE	= 0;

        [DllImport( "quartz.dll", CharSet=CharSet.Auto)]
        public static extern int AMGetErrorText( int hr, StringBuilder buf, int max );
    }


    #endregion

    #region Declarations
    [StructLayout(LayoutKind.Sequential, Pack=2), ComVisible(false)]
    public struct BitmapInfoHeader
    {
        public int      Size;
        public int      Width;
        public int      Height;
        public short    Planes;
        public short    BitCount;
        public int      Compression;
        public int      ImageSize;
        public int      XPelsPerMeter;
        public int      YPelsPerMeter;
        public int      ClrUsed;
        public int      ClrImportant;
    }

    [StructLayout(LayoutKind.Explicit), ComVisible(false)]
    public struct DDPixelFormat
    {
        [FieldOffset(0)]
        public int dwSize;
        [FieldOffset(4)]
        public int dwFlags;
        [FieldOffset(8)]
        public int dwFourCC;

        [FieldOffset(12)]
        public int   dwRGBBitCount;
        [FieldOffset(12)]
        public int   dwYUVBitCount;
        [FieldOffset(12)]
        public int   dwZBufferBitDepth;
        [FieldOffset(12)]
        public int   dwAlphaBitDepth;

        [FieldOffset(16)]
        public int dwRBitMask;
        [FieldOffset(16)]
        public int dwYBitMask;

        [FieldOffset(20)]
        public int dwGBitMask;
        [FieldOffset(20)]
        public int dwUBitMask;

        [FieldOffset(24)]
        public int dwBBitMask;
        [FieldOffset(24)]
        public int dwVBitMask;

        [FieldOffset(28)]
        public int dwRGBAlphaBitMask;
        [FieldOffset(28)]
        public int dwYUVAlphaBitMask;
        [FieldOffset(28)]
        public int dwRGBZBitMask;
        [FieldOffset(28)]
        public int dwYUVZBitMask;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct DsCAUUID		// CAUUID
    {
        public int		cElems;
        public IntPtr	pElems;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct DsOptInt64
    {
        public DsOptInt64( long Value )
        {
            this.Value = Value;
        }
        public long		Value;
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public struct DsOptIntPtr
    {
        public IntPtr	Pointer;
    }
    #endregion

    #region Utility Classes

    [ComVisible(false)]
    public class DsUtils
    {
        public static bool IsCorrectDirectXVersion()
        {
            return File.Exists( Path.Combine( Environment.SystemDirectory, @"dpnhpast.dll" ) );
        }


        public static bool ShowCapPinDialog( ICaptureGraphBuilder2 bld, IBaseFilter flt, IntPtr hwnd )
        {
            int hr;
            object comObj = null;
            ISpecifyPropertyPages	spec = null;
            DsCAUUID cauuid = new DsCAUUID();

            try 
            {
                Guid cat  = PinCategory.Capture;
                Guid type = MediaType.Interleaved;
                Guid iid = typeof(IAMStreamConfig).GUID;
                hr = bld.FindInterface( ref cat, ref type, flt, ref iid, out comObj );
                if( hr != 0 )
                {
                    type = MediaType.Video;
                    hr = bld.FindInterface( ref cat, ref type, flt, ref iid, out comObj );
                    if( hr != 0 )
                        return false;
                }
                spec = comObj as ISpecifyPropertyPages;
                if( spec == null )
                    return false;

                hr = spec.GetPages( out cauuid );
                hr = OleCreatePropertyFrame( hwnd, 30, 30, null, 1,
                    ref comObj, cauuid.cElems, cauuid.pElems, 0, 0, IntPtr.Zero );
                return true;
            }
            catch( Exception ee )
            {
                Trace.WriteLine( "!Ds.NET: ShowCapPinDialog " + ee.Message );
                return false;
            }
            finally
            {
                if( cauuid.pElems != IntPtr.Zero )
                    Marshal.FreeCoTaskMem( cauuid.pElems );
					
                spec = null;
                if( comObj != null )
                { Marshal.ReleaseComObject( comObj ); comObj = null; }
            }
        }

        public static bool ShowTunerPinDialog( ICaptureGraphBuilder2 bld, IBaseFilter flt, IntPtr hwnd )
        {
            int hr;
            object comObj = null;
            ISpecifyPropertyPages	spec = null;
            DsCAUUID cauuid = new DsCAUUID();

            try 
            {
                Guid cat  = PinCategory.Capture;
                Guid type = MediaType.Interleaved;
                Guid iid = typeof(IAMTVTuner).GUID;
                hr = bld.FindInterface( ref cat, ref type, flt, ref iid, out comObj );
                if( hr != 0 )
                {
                    type = MediaType.Video;
                    hr = bld.FindInterface( ref cat, ref type, flt, ref iid, out comObj );
                    if( hr != 0 )
                        return false;
                }
                spec = comObj as ISpecifyPropertyPages;
                if( spec == null )
                    return false;

                hr = spec.GetPages( out cauuid );
                hr = OleCreatePropertyFrame( hwnd, 30, 30, null, 1,
                    ref comObj, cauuid.cElems, cauuid.pElems, 0, 0, IntPtr.Zero );
                return true;
            }
            catch( Exception ee )
            {
                Trace.WriteLine( "!Ds.NET: ShowCapPinDialog " + ee.Message );
                return false;
            }
            finally
            {
                if( cauuid.pElems != IntPtr.Zero )
                    Marshal.FreeCoTaskMem( cauuid.pElems );
					
                spec = null;
                if( comObj != null )
                { Marshal.ReleaseComObject( comObj ); comObj = null; }
            }
        }


        [Obsolete("Use DsGetPin.ByDirectionAndIndex", false)]
        public static int GetPin( IBaseFilter filter, PinDirection dirrequired, int num, out IPin ppPin )
        {
            ppPin = null;
            int hr;
            IEnumPins pinEnum;
            hr = filter.EnumPins( out pinEnum );
            if( (hr < 0) || (pinEnum == null) )
                return hr;

            IPin[] pins = new IPin[1];
            int f;
            PinDirection dir;
            do
            {
                hr = pinEnum.Next( 1, pins, out f );
                if( (hr != 0) || (pins[0] == null) )
                    break;
                dir = (PinDirection) 3;
                hr = pins[0].QueryDirection( out dir );
                if( (hr == 0) && (dir == dirrequired) )
                {
                    if( num == 0 )
                    {
                        ppPin = pins[0];
                        pins[0] = null;
                        break;
                    }
                    num--;
                }
                Marshal.ReleaseComObject( pins[0] ); pins[0] = null;
            }
            while( hr == 0 );

            Marshal.ReleaseComObject( pinEnum ); pinEnum = null;
            return hr;
        }

        public static Guid GetPinCategory(IPin pPin)
        {
            Guid guidRet;

            int iSize = Marshal.SizeOf(typeof(Guid));
            IntPtr ipOut = Marshal.AllocCoTaskMem(iSize);

            try
            {
                int hr;
                int cbBytes;

                Guid g = Misc.PropsetIDPin;
                IntPtr ipIn = IntPtr.Zero;
                IKsPropertySet pKs = pPin as IKsPropertySet;

                hr = pKs.Get(ref g, AMPropertyPin.Category, ipIn, 0,  ipOut, iSize, out cbBytes);
                Marshal.ThrowExceptionForHR(hr);

                guidRet = (Guid)Marshal.PtrToStructure(ipOut, typeof(Guid));
            }
            finally
            {
                Marshal.FreeCoTaskMem(ipOut);
            }

            return guidRet;
        }

        /// <summary> 
        ///  Free the nested structures and release any 
        ///  COM objects within an AMMediaType struct.
        /// </summary>
        public static void FreeAMMediaType(ref AMMediaType mediaType)
        {
            if (mediaType != null)
            {
                if ( mediaType.formatSize != 0 )
                {
                    Marshal.FreeCoTaskMem( mediaType.formatPtr );
                    mediaType.formatSize = 0;
                    mediaType.formatPtr = IntPtr.Zero;
                }
                if ( mediaType.unkPtr != IntPtr.Zero ) 
                {
                    Marshal.Release( mediaType.unkPtr );
                    mediaType.unkPtr = IntPtr.Zero;
                }

                mediaType = null;
            }
        }

        /// <summary>
        ///  Free the nested interfaces within a PinInfo struct.
        /// </summary>
        public static void FreePinInfo(PinInfo pinInfo)
        {
            Marshal.ReleaseComObject(pinInfo.filter);
        }

        [DllImport("olepro32.dll", CharSet=CharSet.Unicode, ExactSpelling=true) ]
        private static extern int OleCreatePropertyFrame( IntPtr hwndOwner, int x, int y,
            string lpszCaption, int cObjects,
            [In, MarshalAs(UnmanagedType.Interface)] ref object ppUnk,
            int cPages,	IntPtr pPageClsID, int lcid, int dwReserved, IntPtr pvReserved );
    }


    // ---------------------------------------------------------------------------------------

    [ComVisible(false)]
    public class DsROT
    {
        public static bool AddGraphToRot( object graph, out int cookie )
        {
            cookie = 0;
            int hr = 0;
            UCOMIRunningObjectTable rot = null;
            UCOMIMoniker mk = null;
            try 
            {
                hr = GetRunningObjectTable( 0, out rot );
                Marshal.ThrowExceptionForHR( hr );

                int id = GetCurrentProcessId();
                IntPtr iuPtr = Marshal.GetIUnknownForObject( graph );
                int iuInt = (int) iuPtr;
                Marshal.Release( iuPtr );
                string item = string.Format( "FilterGraph {0} pid {1}", iuInt.ToString("x8"), id.ToString("x8") );
                hr = CreateItemMoniker( "!", item, out mk );
                Marshal.ThrowExceptionForHR( hr );
				
                rot.Register( ROTFLAGS_REGISTRATIONKEEPSALIVE, graph, mk, out cookie );
                return true;
            }
            catch( Exception )
            {
                return false;
            }
            finally
            {
                if( mk != null )
                { Marshal.ReleaseComObject( mk ); mk = null; }
                if( rot != null )
                { Marshal.ReleaseComObject( rot ); rot = null; }
            }
        }

        public static bool RemoveGraphFromRot( ref int cookie )
        {
            UCOMIRunningObjectTable rot = null;
            try 
            {
                int hr = GetRunningObjectTable( 0, out rot );
                Marshal.ThrowExceptionForHR( hr );

                rot.Revoke( cookie );
                cookie = 0;
                return true;
            }
            catch( Exception )
            {
                return false;
            }
            finally
            {
                if( rot != null )
                { Marshal.ReleaseComObject( rot ); rot = null; }
            }
        }

        private const int ROTFLAGS_REGISTRATIONKEEPSALIVE	= 1;

        [DllImport("ole32.dll", ExactSpelling=true) ]
        private static extern int GetRunningObjectTable( int r,
            out UCOMIRunningObjectTable pprot );

        [DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true) ]
        private static extern int CreateItemMoniker( string delim,
            string item, out UCOMIMoniker ppmk );

        [DllImport("kernel32.dll", ExactSpelling=true) ]
        private static extern int GetCurrentProcessId();
    }


    [ComVisible(false)]
    public class DsDev
    {
        public static bool GetDevicesOfCat( Guid cat, out ArrayList devs )
        {
            devs = null;
            int hr;
            ICreateDevEnum enumDev = null;
            UCOMIEnumMoniker enumMon = null;
            UCOMIMoniker[] mon = new UCOMIMoniker[1];
            int count = 0;
            try 
            {
                enumDev = (ICreateDevEnum) new CreateDevEnum();
                hr = enumDev.CreateClassEnumerator( ref cat, out enumMon, 0 );
                if( hr != 0 )
                {
                    throw new NotSupportedException( "No devices of the category" );
                }

                int f;
                do
                {
                    hr = enumMon.Next( 1, mon, out f );
                    Marshal.ThrowExceptionForHR( hr );

                    // End of enum
                    if( (hr != 0) || f == 0 )
                    {
                        break;
                    }
                    DsDevice dev = new DsDevice();
                    dev.Name = GetFriendlyName( mon[0] );
                    if( devs == null )
                    {
                        devs = new ArrayList();
                    }
                    dev.Mon = mon[0]; 
                    mon[0] = null;
                    devs.Add( dev ); 
                    dev = null;
                    count++;
                }
                while(true);
            }
            catch( Exception )
            {
                if( devs != null )
                {
                    foreach( DsDevice d in devs )
                        d.Dispose();
                    devs = null;
                }
                return false;
            }
            finally
            {
                enumDev = null;
                if( mon[0] != null )
                { 
                    Marshal.ReleaseComObject( mon[0] ); 
                    mon[0] = null; 
                }
                if( enumMon != null )
                { 
                    Marshal.ReleaseComObject( enumMon ); 
                    enumMon = null; 
                }
            }

            return count > 0;
        }

        private static string GetFriendlyName( UCOMIMoniker mon )
        {
            object bagObj = null;
            IPropertyBag bag = null;
            string ret = null;

            try 
            {
                Guid bagId = typeof( IPropertyBag ).GUID;
                mon.BindToStorage( null, null, ref bagId, out bagObj );
                bag = (IPropertyBag) bagObj;
                object val = "";
                int hr = bag.Read( "FriendlyName", ref val, IntPtr.Zero );
                Marshal.ThrowExceptionForHR( hr );
                ret = val as string;
                if( (ret == null) || (ret.Length < 1) )
                {
                    throw new NotImplementedException( "Device FriendlyName" );
                }
            }
            catch( Exception )
            {
                return null;
            }
            finally
            {
                bag = null;
                if( bagObj != null )
                { 
                    Marshal.ReleaseComObject( bagObj ); 
                    bagObj = null; 
                }
            }

            return ret;
        }
    }


    [ComVisible(false)]
    public class DsDevice : IDisposable
    {
        public string			Name;
        public UCOMIMoniker		Mon;

        public void Dispose()
        {
            if( Mon != null )
            { Marshal.ReleaseComObject( Mon ); Mon = null; }
        }
    }
    public class DsGetPin
    {
        public static IPin ByDirection(IBaseFilter vSource, PinDirection vDir)
        {
            int hr;
            int lFetched;
            IEnumPins ppEnum;
            PinDirection ppindir;
            IPin pRet = null;
            IPin[] pPins = new IPin[1];

            hr = vSource.EnumPins(out ppEnum);
            Marshal.ThrowExceptionForHR(hr);

            while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1) )
            {
                hr = pPins[0].QueryDirection(out ppindir);
                Marshal.ThrowExceptionForHR(hr);

                if (ppindir == vDir)
                {
                    pRet = pPins[0];
                    break;
                }
                Marshal.ReleaseComObject(pPins[0]);
            }
            Marshal.ReleaseComObject(ppEnum);

            return pRet;			 
        }

        public static IPin ByDirectionAndIndex(IBaseFilter vSource, PinDirection vDir, int iNum)
        {
            int hr;
            int lFetched;
            IEnumPins ppEnum;
            PinDirection ppindir;
            IPin pRet = null;
            IPin[] pPins = new IPin[1];

            hr = vSource.EnumPins(out ppEnum);
            Marshal.ThrowExceptionForHR(hr);

            while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1) )
            {
                hr = pPins[0].QueryDirection(out ppindir);
                Marshal.ThrowExceptionForHR(hr);

                if (ppindir == vDir)
                {
                    if (iNum == 0)
                    {
                        pRet = pPins[0];
                        break;
                    }
                    iNum--;
                    Marshal.ReleaseComObject(pPins[0]);
                }
            }
            Marshal.ReleaseComObject(ppEnum);

            return pRet;			 
        }

        public static IPin ByName(IBaseFilter vSource, string vPinName)
        {
            int hr;
            int lFetched;
            IEnumPins ppEnum;
            PinInfo ppinfo;
            IPin pRet = null;
            IPin[] pPins = new IPin[1];

            hr = vSource.EnumPins(out ppEnum);
            Marshal.ThrowExceptionForHR(hr);

            while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1) )
            {
                hr = pPins[0].QueryPinInfo(out ppinfo);
                Marshal.ThrowExceptionForHR(hr);

                if (ppinfo.name == vPinName)
                {
                    pRet = pPins[0];
                    break;
                }
                Marshal.ReleaseComObject(pPins[0]);
            }
            Marshal.ReleaseComObject(ppEnum);

            return pRet;			 
        }

        // Pass one of the members of PinCategory
        public static IPin ByCategory(IBaseFilter vSource, Guid guidPinCat)
        {
            int hr;
            int lFetched;
            IEnumPins ppEnum;
            IPin pRet = null;
            IPin[] pPins = new IPin[1];

            hr = vSource.EnumPins(out ppEnum);
            Marshal.ThrowExceptionForHR(hr);

            while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1) )
            {
                if ( DsUtils.GetPinCategory(pPins[0]) == guidPinCat )
                {
                    pRet = pPins[0];
                    break;
                }
                Marshal.ReleaseComObject(pPins[0]);
            }
            Marshal.ReleaseComObject(ppEnum);

            return pRet;			 
        }
    }


    public class DsToString
    {
        /// <summary>
        /// Produces a usable string that describes the MediaType object
        /// </summary>
        /// <returns>Concatenation of MajorType + SubType + FormatType + Fixed + Temporal + SampleSize.ToString</returns>
        static public string AMMediaTypeToString(ref AMMediaType pmt)
        {
            return MediaTypeToString(pmt.majorType) + " " + MediaSubTypeToString(pmt.subType) + " " + 
                MediaFormatTypeToString(pmt.formatType) + " " +
                (pmt.fixedSizeSamples ? "FixedSamples" : "NotFixedSamples") + " " +
                (pmt.temporalCompression ? "temporalCompression" : "NottemporalCompression") + " " +
                pmt.sampleSize.ToString();
        }

        /// <summary>
        /// Converts AMMediaType.MajorType Guid to a readable string
        /// </summary>
        /// <returns>MajorType Guid as a readable string or Guid if unrecognized</returns>
        static public string MediaTypeToString(Guid guid)
        {
            string sRet;

            if (guid == MediaType.Audio)
            {
                sRet = "Audio";
            }
            else if (guid == MediaType.Interleaved)
            {
                sRet = "Interleaved";
            }
            else if (guid == MediaType.Stream)
            {
                sRet = "Stream";
            }
            else if (guid == MediaType.Text)
            {
                sRet = "Text";
            }
            else if (guid == MediaType.Video)
            {
                sRet = "Video";
            }
            else
            {
                sRet = guid.ToString();
            }

            return sRet;
        }

        /// <summary>
        /// Converts the AMMediaType.SubType Guid to a readable string
        /// </summary>
        /// <returns>SubType Guid as a readable string or Guid if unrecognized</returns>
        static public string MediaSubTypeToString(Guid guid)
        {
            string sRet;

            // Video types
            if (guid == MediaSubType.CLPL)
            {
                sRet = "CLPL";
            }
            else if (guid == MediaSubType.YUYV)
            {
                sRet = "YUYV";
            }
            else if (guid == MediaSubType.IYUV)
            {
                sRet = "IYUV";
            }
            else if (guid == MediaSubType.YVU9)
            {
                sRet = "YVU9";
            }
            else if (guid == MediaSubType.Y411)
            {
                sRet = "Y411";
            }
            else if (guid == MediaSubType.Y41P)
            {
                sRet = "Y41P";
            }
            else if (guid == MediaSubType.YUY2)
            {
                sRet = "YUY2";
            }
            else if (guid == MediaSubType.YVYU)
            {
                sRet = "YVYU";
            }
            else if (guid == MediaSubType.UYVY)
            {
                sRet = "UYVY";
            }
            else if (guid == MediaSubType.Y211)
            {
                sRet = "Y211";
            }
            else if (guid == MediaSubType.YV12)
            {
                sRet = "YV12";
            }
            else if (guid == MediaSubType.IF09)
            {
                sRet = "IF09";
            }
            else if (guid == MediaSubType.MJPG)
            {
                sRet = "MJPG";
            }
            else if (guid == MediaSubType.TVMJ)
            {
                sRet = "TVMJ";
            }
            else if (guid == MediaSubType.WAKE)
            {
                sRet = "WAKE";
            }
            else if (guid == MediaSubType.CFCC)
            {
                sRet = "CFCC";
            }
            else if (guid == MediaSubType.IJPG)
            {
                sRet = "IJPG";
            }
            else if (guid == MediaSubType.CLPL)
            {
                sRet = "YUY2";
            }
            else if (guid == MediaSubType.PLUM)
            {
                sRet = "PLUM";
            }
            else if (guid == MediaSubType.DVCS)
            {
                sRet = "DVCS";
            }
            else if (guid == MediaSubType.DVSD)
            {
                sRet = "DVSD";
            }
            else if (guid == MediaSubType.MDVF)
            {
                sRet = "MDVF";
            }
            else if (guid == MediaSubType.RGB1)
            {
                sRet = "RGB1";
            }
            else if (guid == MediaSubType.RGB4)
            {
                sRet = "RGB4";
            }
            else if (guid == MediaSubType.RGB8)
            {
                sRet = "RGB8";
            }
            else if (guid == MediaSubType.RGB565)
            {
                sRet = "RGB565";
            }
            else if (guid == MediaSubType.RGB555)
            {
                sRet = "RGB555";
            }
            else if (guid == MediaSubType.RGB24)
            {
                sRet = "RGB24";
            }
            else if (guid == MediaSubType.RGB32)
            {
                sRet = "RGB32";
            }
            else if (guid == MediaSubType.CLJR)
            {
                sRet = "CLJR";
            }
            else if (guid == MediaSubType.I420)
            {
                sRet = "I420";
            }
            else if (guid == MediaSubType.PCM)
            {
                sRet = "PCM";
            }
            else
            {
                sRet = guid.ToString();
            }

            return sRet;
        }

        /// <summary>
        /// Converts the AMMediaType.FormatType Guid to a readable string
        /// </summary>
        /// <returns>FormatType Guid as a readable string or Guid if unrecognized</returns>
        static public string MediaFormatTypeToString(Guid guid)
        {
            string sRet;

            if (guid == FormatType.DvInfo)
            {
                sRet = "DvInfo";
            }
            else if (guid == FormatType.MpegStreams)
            {
                sRet = "MpegStreams";
            }
            else if (guid == FormatType.MpegVideo)
            {
                sRet = "MpegVideo";
            }
            else if (guid == FormatType.None)
            {
                sRet = "None";
            }
            else if (guid == FormatType.VideoInfo)
            {
                sRet = "VideoInfo";
            }
            else if (guid == FormatType.VideoInfo2)
            {
                sRet = "VideoInfo2";
            }
            else if (guid == FormatType.WaveEx)
            {
                sRet = "WaveEx";
            }
            else
            {
                sRet = guid.ToString();
            }

            return sRet;
        }
    }
    #endregion
#endif
} // namespace DShowNET
