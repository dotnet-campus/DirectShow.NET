// $Id: IEnumMediaTypesTest.cs,v 1.3 2005-05-24 21:57:09 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.3 $
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class IEnumMediaTypesTest
	{
		[Test]
		public void TestNext()
		{
			int hr;
			int lFetched;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
			hr = enumMediaTypes.Next(1, ppMediaTypes, out lFetched);
			DsError.ThrowExceptionForHR(hr);
		}

		[Test]
		public void TestReset()
		{
			int hr;
			int lFetched;
			AMMediaType[] ppMediaTypes = new AMMediaType[1];

			IEnumMediaTypes enumMediaTypes = GetEnumMediaTypes();
			hr = enumMediaTypes.Next(1, ppMediaTypes, out lFetched);
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
			IBaseFilter filter = new SmartTee() as IBaseFilter;
			int hr;
			int lFetched;
			IEnumPins ppEnum;
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

			hr = filter.EnumPins(out ppEnum);
			DsError.ThrowExceptionForHR(hr);

			while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
			{
				pRet = pPins[0];
				break;
			}
			Marshal.ReleaseComObject(ppEnum);

			IEnumMediaTypes enumMediaTypes;
			hr = pRet.EnumMediaTypes(out enumMediaTypes);
			DsError.ThrowExceptionForHR(hr);

			return enumMediaTypes;
		}

	}
}