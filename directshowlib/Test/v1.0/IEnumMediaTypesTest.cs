// $Id: IEnumMediaTypesTest.cs,v 1.5 2005-06-23 23:44:51 snarfle Exp $
// $Author: snarfle $
// $Revision: 1.5 $
using System.Runtime.InteropServices;
using NUnit.Framework;
using System;

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
			int lFetched;
			AMMediaType[] ppMediaTypes = new AMMediaType[24];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();

            for (int x=0; x < 3; x++)
            {
                hr = enumMediaTypes.Next(ppMediaTypes.Length, ppMediaTypes, out lFetched);
                DsError.ThrowExceptionForHR(hr);
            }
		}

		[Test]
		public void TestReset()
		{
			int hr;
			int lFetched;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
			hr = enumMediaTypes.Next(ppMediaTypes.Length, ppMediaTypes, out lFetched);
			DsError.ThrowExceptionForHR(hr);

			hr = enumMediaTypes.Reset();
			DsError.ThrowExceptionForHR(hr);

			hr = enumMediaTypes.Next(1, ppMediaTypes, out lFetched);
			DsError.ThrowExceptionForHR(hr);
		}

		[Test]
		public void TestSkip()
		{
			int hr;
			int lFetched;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
			hr = enumMediaTypes.Next(1, ppMediaTypes, out lFetched);
			DsError.ThrowExceptionForHR(hr);

			hr = enumMediaTypes.Skip(1);
			DsError.ThrowExceptionForHR(hr);

			hr = enumMediaTypes.Next(1, ppMediaTypes, out lFetched);
			DsError.ThrowExceptionForHR(hr);

		}

		[Test]
		public void TestClone()
		{
			int hr;
			int lFetched;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
			IEnumMediaTypes cloneMediaType;
			hr = enumMediaTypes.Clone(out cloneMediaType);
			DsError.ThrowExceptionForHR(hr);

			hr = cloneMediaType.Next(1, ppMediaTypes, out lFetched);
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