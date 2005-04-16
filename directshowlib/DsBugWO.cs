/******************************************************
                  DirectShow .NET
		      netmaster@swissonline.ch
*******************************************************/
//           WORKAROUND FOR DS BUGs

using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Services;

namespace DShowNET
{
	public class DsBugWO
	{
		/*
	works:
		CoCreateInstance( CLSID_CaptureGraphBuilder2, ..., IID_ICaptureGraphBuilder2, ...);
	doesn't (E_NOTIMPL):
		CoCreateInstance( CLSID_CaptureGraphBuilder2, ..., IID_IUnknown, ...);
	thus .NET 'Activator.CreateInstance' fails
	*/

		public static object CreateDsInstance(ref Guid clsid, ref Guid riid)
		{
			object ooo;
			try
			{
				// This works correctly in DX9.0c, but not in 8.1

				// Get the requested interface
				Type comType = Type.GetTypeFromCLSID(clsid);
				if (comType == null)
					throw new NotImplementedException(@"Interface not installed/registered!");
				ooo = Activator.CreateInstance(comType);
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

				ooo = EnterpriseServicesHelper.WrapIUnknownWithComObject(ptrIf);
				int ct = Marshal.Release(ptrIf);

				// CoCreateInstance increments the refcount to 1
				// QueryInstance increments the refcount to 2
				// WrapIUnknownWithComObject increments the refcount to 3

				// Marshal.Release( ptrIf ) decrements the refcount to 2
				// Releasing the returned object with Marshal.ReleaseComObject(o) decrements the refcount to 1
				// Leaving one refcount left to leak!  Unless you add:
				int ct2 = Marshal.Release(ptrXX); // ct2 will be 1
			}

			return ooo;
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


} // namespace DShowNET