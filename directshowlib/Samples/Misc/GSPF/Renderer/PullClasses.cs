/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Runtime.InteropServices;

using DirectShowLib;

namespace GenericSampleSourceFilterClasses
{
    [ComImport, Guid("84AC6CD2-EC96-4d89-BF70-7380003656C4")]
	public class GenericSamplePullFilter
	{
	}

    [ComImport, Guid("C883E766-38E0-47a8-ABDF-852232C59718"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IGenericPullConfig
	{
		[PreserveSig]
		int SetMediaType([MarshalAs(UnmanagedType.LPStruct)] AMMediaType amt);

        [PreserveSig]
        int SetEmbedded([MarshalAs(UnmanagedType.LPStruct)] Guid riid);
    }
}
