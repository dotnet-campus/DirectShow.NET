// $Id: DsBugWO.cs,v 1.3 2005-04-22 20:41:58 kawaic Exp $
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

using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;

namespace DShowNET
{
	/// <summary>
	/// This class is used for as a workaround for direct show bugs in previous versions older 9.0c
	/// Original author is from the code project under DirectShow.NET by netmaster@swissonline.ch	
	/// </summary>
	public class DsBugWO
	{
		/*
	works:
		CoCreateInstance( CLSID_CaptureGraphBuilder2, ..., IID_ICaptureGraphBuilder2, ...);
	doesn't (E_NOTIMPL):
		CoCreateInstance( CLSID_CaptureGraphBuilder2, ..., IID_IUnknown, ...);
	thus .NET 'Activator.CreateInstance' fails
	*/

		/// <summary>
		/// Creates a directshow instance
		/// </summary>
		/// <param name="clsid"></param>
		/// <param name="riid"></param>
		/// <returns></returns>
		public static object CreateDsInstance(ref Guid clsid, ref Guid riid)
		{
			object result;
			try
			{
				// This works correctly in DX9.0c, but not in 8.1

				// Get the requested interface
				Type comType = Type.GetTypeFromCLSID(clsid);
				if (comType == null)
					throw new NotImplementedException(@"Interface not installed/registered!");
				result = Activator.CreateInstance(comType);
			}
			catch
			{
				IntPtr ptrIf;
				int hr = CoCreateInstance(ref clsid, IntPtr.Zero, CLSCTX.Inproc, ref riid, out ptrIf);
				if ((hr != 0) || (ptrIf == IntPtr.Zero))
					Marshal.ThrowExceptionForHR(hr);

				Guid iu = new Guid("00000000-0000-0000-C000-000000000046");
				IntPtr ptrXX;
				hr = Marshal.QueryInterface(ptrIf, ref iu, out ptrXX);

				result = EnterpriseServicesHelper.WrapIUnknownWithComObject(ptrIf);
				int ct = Marshal.Release(ptrIf);

				// CoCreateInstance increments the refcount to 1
				// QueryInstance increments the refcount to 2
				// WrapIUnknownWithComObject increments the refcount to 3

				// Marshal.Release( ptrIf ) decrements the refcount to 2
				// Releasing the returned object with Marshal.ReleaseComObject(o) decrements the refcount to 1
				// Leaving one refcount left to leak!  Unless you add:
				int ct2 = Marshal.Release(ptrXX); // ct2 will be 1
			}

			return result;
		}

		[DllImport("ole32.dll")]
		private static extern int CoCreateInstance(ref Guid clsid, IntPtr pUnkOuter, CLSCTX dwClsContext, ref Guid iid, out IntPtr ptrIf);
	}

	[Flags]
	internal enum CLSCTX
	{
		Inproc = 0x03,
		Server = 0x15,
		All = 0x17,
	}


}