// $Id: IEnumPinsTest.cs,v 1.2 2005-05-24 17:27:22 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.2 $
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class IEnumPinsTest
	{
		[Test]
		public void TestNext()
		{
			IEnumPins ppEnum = GetTestEnum();
			int lFetched;
			int hr;
			IPin[] pPins = new IPin[1];

			hr = ppEnum.Next(1, pPins, out lFetched);
			DsError.ThrowExceptionForHR(hr);
			Marshal.ReleaseComObject(ppEnum);
		}

		[Test]
		public void TestReset()
		{
			int hr;
			int lFetched;
			IPin[] pPins = new IPin[1];

			IEnumPins ppEnum = GetTestEnum();

			hr = ppEnum.Next(1, pPins, out lFetched);
			DsError.ThrowExceptionForHR(hr);

			hr = ppEnum.Reset();
			DsError.ThrowExceptionForHR(hr);

			hr = ppEnum.Next(1, pPins, out lFetched);
			DsError.ThrowExceptionForHR(hr);
			
			Marshal.ReleaseComObject(ppEnum);
		}

		[Test]
		public void TestSkip()
		{
			int hr;
			int lFetched;
			IPin[] pPins = new IPin[1];

			IEnumPins ppEnum = GetTestEnum();

			hr = ppEnum.Next(1, pPins, out lFetched);
			DsError.ThrowExceptionForHR(hr);

			hr = ppEnum.Skip(1);
			DsError.ThrowExceptionForHR(hr);

			hr = ppEnum.Next(1, pPins, out lFetched);
			DsError.ThrowExceptionForHR(hr);
			
			Marshal.ReleaseComObject(ppEnum);
		}

		[Test]
		public void TestClone()
		{
			int hr;
			int lFetched;
			IPin[] pPins = new IPin[1];

			IEnumPins ppEnum = GetTestEnum();
			IEnumPins cloneEnum;
			hr = ppEnum.Clone(out cloneEnum);
			DsError.ThrowExceptionForHR(hr);

			hr = cloneEnum.Next(1, pPins, out lFetched);
			DsError.ThrowExceptionForHR(hr);

			Marshal.ReleaseComObject(ppEnum);
			Marshal.ReleaseComObject(cloneEnum);
		}


		private IEnumPins GetTestEnum()
		{
			
			IBaseFilter filter = new SmartTee() as IBaseFilter;
			int hr;
			
			IEnumPins ppEnum;

			hr = filter.EnumPins(out ppEnum);
			DsError.ThrowExceptionForHR(hr);

			return ppEnum;
		}

	}
}