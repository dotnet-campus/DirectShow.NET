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

using System;
using System.Runtime.InteropServices;

namespace DirectShowLib
{

	#region Declarations

#if ALLOW_UNTESTED_STRUCTS
	/// <summary>
	/// From KSMULTIPLE_ITEM - Note that data is returned in the memory IMMEDIATELY following this struct.
	/// The Size parm indicates ths size of the KSMultipleItem plus the extra bytes.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class KSMultipleItem
	{
		public int Size;
		public int Count;
	}
#endif

    /// <summary>
    /// From REGPINMEDIUM
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PinMedium
    {
        public Guid clsMedium;
        public int dw1;
        public int dw2;
    }

    #endregion

	#region Interfaces

#if ALLOW_UNTESTED_INTERFACES
	[Guid("00000109-0000-0000-C000-000000000046"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistStream : IPersist
	{
        #region IPersist Methods

		[PreserveSig]
		new int GetClassID([Out] out Guid pClassID);

        #endregion

        [PreserveSig]
        int IsDirty();
        
        [PreserveSig]
        int Load([In] IStream pStm);
        
        [PreserveSig]
        int Save([In] IStream pStm,
                [In] bool fClearDirty);
        
        [PreserveSig]
        int GetSizeMax([Out] long pcbSize);
    }

	[Guid("55272A00-42CB-11CE-8135-00AA004BB851"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyBag
	{
		[PreserveSig]
		int Read(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[In, Out] ref object pVar,
			[In] IntPtr pErrorLog
			);

		[PreserveSig]
		int Write(
			[In, MarshalAs(UnmanagedType.LPWStr)] string pszPropName,
			[In] ref object pVar
			);
	}


	public interface IStream
	{
		//TODO:
	}

	[Guid("B196B28B-BAB4-101A-B69C-00AA00341D07"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISpecifyPropertyPages
	{
		[PreserveSig]
		int GetPages(out DsCAUUID pPages);
	}


	[Guid("b61178d1-a2d9-11cf-9e53-00aa00a216a1"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IKsPin
	{
		/// <summary>
		/// The caller must free the returned structures, using the CoTaskMemFree function
		/// </summary> 
		[PreserveSig]
		int KsQueryMediums(
			out IntPtr ip);
	}

#endif

    [Guid("0000010c-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPersist
    {
        [PreserveSig]
        int GetClassID([Out] out Guid pClassID);
    }

    #endregion
}