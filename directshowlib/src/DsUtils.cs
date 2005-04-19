// $Id: DsUtils.cs,v 1.3 2005-04-19 14:48:48 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.3 $

#region license
/* ====================================================================
 * The Apache Software License, Version 1.1
 *
 * Copyright (c) 2000 The Apache Software Foundation.  All rights
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

// DsUtils
// Original work from DirectShow .NET by netmaster@swissonline.ch
// DirectShow utility classes, partial from the SDK Common sources

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace DShowNET
{
	[ComVisible(false)]
	public class DsUtils
	{
		public static bool IsCorrectDirectXVersion()
		{
			return File.Exists(Path.Combine(Environment.SystemDirectory, @"dpnhpast.dll"));
		}


		public static bool ShowCapPinDialog(ICaptureGraphBuilder2 bld, IBaseFilter flt, IntPtr hwnd)
		{
			int hr;
			object comObj = null;
			ISpecifyPropertyPages spec = null;
			DsCAUUID cauuid = new DsCAUUID();

			try
			{
				Guid cat = PinCategory.Capture;
				Guid type = MediaType.Interleaved;
				Guid iid = typeof (IAMStreamConfig).GUID;
				hr = bld.FindInterface(ref cat, ref type, flt, ref iid, out comObj);
				if (hr != 0)
				{
					type = MediaType.Video;
					hr = bld.FindInterface(ref cat, ref type, flt, ref iid, out comObj);
					if (hr != 0)
						return false;
				}
				spec = comObj as ISpecifyPropertyPages;
				if (spec == null)
					return false;

				hr = spec.GetPages(out cauuid);
				hr = OleCreatePropertyFrame(hwnd, 30, 30, null, 1,
				                            ref comObj, cauuid.cElems, cauuid.pElems, 0, 0, IntPtr.Zero);
				return true;
			}
			catch (Exception ee)
			{
				Trace.WriteLine("!Ds.NET: ShowCapPinDialog " + ee.Message);
				return false;
			}
			finally
			{
				if (cauuid.pElems != IntPtr.Zero)
					Marshal.FreeCoTaskMem(cauuid.pElems);

				spec = null;
				if (comObj != null)
					Marshal.ReleaseComObject(comObj);
				comObj = null;
			}
		}

		public static bool ShowTunerPinDialog(ICaptureGraphBuilder2 bld, IBaseFilter flt, IntPtr hwnd)
		{
			int hr;
			object comObj = null;
			ISpecifyPropertyPages spec = null;
			DsCAUUID cauuid = new DsCAUUID();

			try
			{
				Guid cat = PinCategory.Capture;
				Guid type = MediaType.Interleaved;
				Guid iid = typeof (IAMTVTuner).GUID;
				hr = bld.FindInterface(ref cat, ref type, flt, ref iid, out comObj);
				if (hr != 0)
				{
					type = MediaType.Video;
					hr = bld.FindInterface(ref cat, ref type, flt, ref iid, out comObj);
					if (hr != 0)
						return false;
				}
				spec = comObj as ISpecifyPropertyPages;
				if (spec == null)
					return false;

				hr = spec.GetPages(out cauuid);
				hr = OleCreatePropertyFrame(hwnd, 30, 30, null, 1,
				                            ref comObj, cauuid.cElems, cauuid.pElems, 0, 0, IntPtr.Zero);
				return true;
			}
			catch (Exception ee)
			{
				Trace.WriteLine("!Ds.NET: ShowCapPinDialog " + ee.Message);
				return false;
			}
			finally
			{
				if (cauuid.pElems != IntPtr.Zero)
					Marshal.FreeCoTaskMem(cauuid.pElems);

				spec = null;
				if (comObj != null)
					Marshal.ReleaseComObject(comObj);
				comObj = null;
			}
		}


		// from 'DShowUtil.cpp'
		public int GetPin(IBaseFilter filter, PinDirection dirrequired, int num, out IPin ppPin)
		{
			ppPin = null;
			int hr;
			IEnumPins pinEnum;
			hr = filter.EnumPins(out pinEnum);
			if ((hr < 0) || (pinEnum == null))
				return hr;

			IPin[] pins = new IPin[1];
			int f;
			PinDirection dir;
			do
			{
				hr = pinEnum.Next(1, pins, out f);
				if ((hr != 0) || (pins[0] == null))
					break;
				dir = (PinDirection) 3;
				hr = pins[0].QueryDirection(out dir);
				if ((hr == 0) && (dir == dirrequired))
				{
					if (num == 0)
					{
						ppPin = pins[0];
						pins[0] = null;
						break;
					}
					num--;
				}
				Marshal.ReleaseComObject(pins[0]);
				pins[0] = null;
			} while (hr == 0);

			Marshal.ReleaseComObject(pinEnum);
			pinEnum = null;
			return hr;
		}

		/// <summary> 
		///  Free the nested structures and release any 
		///  COM objects within an AMMediaType struct.
		/// </summary>
		public static void FreeAMMediaType(AMMediaType mediaType)
		{
			if (mediaType.formatSize != 0)
				Marshal.FreeCoTaskMem(mediaType.formatPtr);
			if (mediaType.unkPtr != IntPtr.Zero)
				Marshal.Release(mediaType.unkPtr);
			mediaType.formatSize = 0;
			mediaType.formatPtr = IntPtr.Zero;
			mediaType.unkPtr = IntPtr.Zero;
		}

		[DllImport("olepro32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
		private static extern int OleCreatePropertyFrame(IntPtr hwndOwner, int x, int y,
		                                                 string lpszCaption, int cObjects,
		                                                 [In, MarshalAs(UnmanagedType.Interface)] ref object ppUnk,
		                                                 int cPages, IntPtr pPageClsID, int lcid, int dwReserved, IntPtr pvReserved);
	}


// ---------------------------------------------------------------------------------------

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct DsPOINT // POINT
	{
		public int X;
		public int Y;
	}


// ---------------------------------------------------------------------------------------

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct DsRECT // RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}


// ---------------------------------------------------------------------------------------

	[StructLayout(LayoutKind.Sequential, Pack=2), ComVisible(false)]
	public struct BitmapInfoHeader
	{
		public int Size;
		public int Width;
		public int Height;
		public short Planes;
		public short BitCount;
		public int Compression;
		public int ImageSize;
		public int XPelsPerMeter;
		public int YPelsPerMeter;
		public int ClrUsed;
		public int ClrImportant;
	}


// ---------------------------------------------------------------------------------------

	[ComVisible(false)]
	public class DsROT
	{
		public static bool AddGraphToRot(object graph, out int cookie)
		{
			cookie = 0;
			int hr = 0;
			UCOMIRunningObjectTable rot = null;
			UCOMIMoniker mk = null;
			try
			{
				hr = GetRunningObjectTable(0, out rot);
				if (hr < 0)
					Marshal.ThrowExceptionForHR(hr);

				int id = GetCurrentProcessId();
				IntPtr iuPtr = Marshal.GetIUnknownForObject(graph);
				int iuInt = (int) iuPtr;
				Marshal.Release(iuPtr);
				string item = string.Format("FilterGraph {0} pid {1}", iuInt.ToString("x8"), id.ToString("x8"));
				hr = CreateItemMoniker("!", item, out mk);
				if (hr < 0)
					Marshal.ThrowExceptionForHR(hr);

				rot.Register(ROTFLAGS_REGISTRATIONKEEPSALIVE, graph, mk, out cookie);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				if (mk != null)
					Marshal.ReleaseComObject(mk);
				mk = null;
				if (rot != null)
					Marshal.ReleaseComObject(rot);
				rot = null;
			}
		}

		public static bool RemoveGraphFromRot(ref int cookie)
		{
			UCOMIRunningObjectTable rot = null;
			try
			{
				int hr = GetRunningObjectTable(0, out rot);
				if (hr < 0)
					Marshal.ThrowExceptionForHR(hr);

				rot.Revoke(cookie);
				cookie = 0;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				if (rot != null)
					Marshal.ReleaseComObject(rot);
				rot = null;
			}
		}

		private const int ROTFLAGS_REGISTRATIONKEEPSALIVE = 1;

		[DllImport("ole32.dll", ExactSpelling=true)]
		private static extern int GetRunningObjectTable(int r,
		                                                out UCOMIRunningObjectTable pprot);

		[DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
		private static extern int CreateItemMoniker(string delim,
		                                            string item, out UCOMIMoniker ppmk);

		[DllImport("kernel32.dll", ExactSpelling=true)]
		private static extern int GetCurrentProcessId();
	}


// ---------------------------------- ocidl.idl ------------------------------------------------

	[ComVisible(true), ComImport,
		Guid("B196B28B-BAB4-101A-B69C-00AA00341D07"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISpecifyPropertyPages
	{
		[PreserveSig]
		int GetPages(out DsCAUUID pPages);
	}

	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public struct DsCAUUID // CAUUID
	{
		public int cElems;
		public IntPtr pElems;
	}

// ---------------------------------------------------------------------------------------


	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class DsOptInt64
	{
		public DsOptInt64(long Value)
		{
			this.Value = Value;
		}

		public long Value;
	}


	[StructLayout(LayoutKind.Sequential), ComVisible(false)]
	public class DsOptIntPtr
	{
		public IntPtr Pointer;
	}


} // namespace DShowNET