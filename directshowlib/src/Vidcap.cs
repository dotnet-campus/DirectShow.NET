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

#define ALLOW_UNTESTED_STRUCTS
#define ALLOW_UNTESTED_INTERFACES

using System;
using System.Runtime.InteropServices;

namespace DirectShowLib
{
    #region Declarations

#if ALLOW_UNTESTED_STRUCTS

    /// <summary>
    /// From KSTOPOLOGY_CONNECTION
    /// </summary>
    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
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

    [ComVisible(true), ComImport,
    Guid("720D4AC0-7533-11D0-A5D6-28DB04C10000"),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IKsTopologyInfo
    {
        [PreserveSig]
        int get_NumCategories([Out] int pdwNumCategories);

        [PreserveSig]
        int get_Category(
            [In] int dwIndex,
            [Out] out Guid pCategory
            );

        [PreserveSig]
        int get_NumConnections([Out] int pdwNumConnections);

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
        int get_NumNodes([Out] int pdwNumNodes);

        [PreserveSig]
        int get_NodeType(
            [In] int dwNodeId,
            [Out] out Guid pNodeType
            );

        [PreserveSig]
        int CreateNodeInstance(
            [In] int dwNodeId,
            [In] Guid iid,
            [Out] out IntPtr ppvObject // void **
            );
    }

    [ComVisible(true), ComImport,
    Guid("1ABDAECA-68B6-4F83-9371-B413907C7B9F"),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface ISelector
    {
        [PreserveSig]
        int get_NumSources([Out] out int pdwNumSources);

        [PreserveSig]
        int get_SourceNodeId([Out] out int pdwPinId);

        [PreserveSig]
        int put_SourceNodeId([In] int dwPinId);
    }

    [ComVisible(true), ComImport,
    Guid("11737C14-24A7-4bb5-81A0-0D003813B0C4"),
    InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
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
