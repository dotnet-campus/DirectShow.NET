// $Id: IEnumMediaTypesTest.cs,v 1.6 2007-06-21 21:37:07 snarfle Exp $
// $Author: snarfle $
// $Revision: 1.6 $
using System.Runtime.InteropServices;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace DirectShowLib.Test
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class IEnumMediaTypesTest
	{
        public void DoTests()
        {
            TestNext();
            TestReset();
            TestSkip();
            TestClone();
        }

		[Test]
		public void TestNext()
		{
			int hr;
            IntPtr ip = Marshal.AllocCoTaskMem(4);
			AMMediaType[] ppMediaTypes = new AMMediaType[10];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
            //IntPtr ip2 = Marshal.AllocCoTaskMem(ppMediaTypes.Length * IntPtr.Size);

            for (int x=0; x < 3; x++)
            {
                hr = enumMediaTypes.Next(ppMediaTypes.Length, ppMediaTypes, ip);

                //for (int y = 0; y < Marshal.ReadInt32(ip); y++)
                //{
                //    ppMediaTypes[y] = new AMMediaType();
                //    IntPtr ip3 = Marshal.ReadIntPtr(ip2, y * IntPtr.Size); // new IntPtr(ip2.ToInt64() + (y * IntPtr.Size));
                //    Marshal.PtrToStructure(ip3, ppMediaTypes[y]);
                //}

                DsError.ThrowExceptionForHR(hr);
            }

            Marshal.FreeCoTaskMem(ip);
		}

		[Test]
		public void TestReset()
		{
            int hr;
            IntPtr ip = Marshal.AllocCoTaskMem(4);
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
            hr = enumMediaTypes.Next(ppMediaTypes.Length, ppMediaTypes, ip);
			DsError.ThrowExceptionForHR(hr);

            Debug.Assert(Marshal.ReadInt32(ip) == 1, "Next");

			hr = enumMediaTypes.Reset();
			DsError.ThrowExceptionForHR(hr);

            hr = enumMediaTypes.Next(1, ppMediaTypes, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(ip);
		}

		[Test]
		public void TestSkip()
		{
            int hr;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
            hr = enumMediaTypes.Next(1, ppMediaTypes, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

			hr = enumMediaTypes.Skip(1);
			DsError.ThrowExceptionForHR(hr);

            hr = enumMediaTypes.Next(1, ppMediaTypes, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

		}

		[Test]
		public void TestClone()
		{
			int hr;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
			IEnumMediaTypes cloneMediaType;
			hr = enumMediaTypes.Clone(out cloneMediaType);
			DsError.ThrowExceptionForHR(hr);

            hr = cloneMediaType.Next(1, ppMediaTypes, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

		}

		private static IEnumMediaTypes GetEnumMediaTypes()
		{
			IBaseFilter filter;
			int hr;
			IPin pRet = null;

            DsDevice [] capDevices;

            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat( FilterCategory.VideoInputDevice );
            if( capDevices.Length == 0 )
            {
                throw new Exception("No video capture devices found!");
            }

            DsDevice dev = capDevices[0];
            string s;

            dev.Mon.GetDisplayName(null, null, out s);
            filter = Marshal.BindToMoniker( s ) as IBaseFilter;

			pRet = DsFindPin.ByDirection(filter, PinDirection.Output, 0);

			IEnumMediaTypes enumMediaTypes;
			hr = pRet.EnumMediaTypes(out enumMediaTypes);
			DsError.ThrowExceptionForHR(hr);

			return enumMediaTypes;
		}

	}
}