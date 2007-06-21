// $Id: IEnumPinsTest.cs,v 1.4 2007-06-21 08:42:23 snarfle Exp $
// $Author: snarfle $
// $Revision: 1.4 $
using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Diagnostics;

namespace DirectShowLib.Test
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class IEnumPinsTest
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
			IEnumPins ppEnum = GetTestEnum();
			int hr;
			IPin[] pPins = new IPin[1];
            IntPtr ip = Marshal.AllocCoTaskMem(4);

			hr = ppEnum.Next(1, pPins, ip);

            Debug.Assert(Marshal.ReadInt32(ip) == 1, "Next");
            Marshal.FreeCoTaskMem(ip);

			DsError.ThrowExceptionForHR(hr);
			Marshal.ReleaseComObject(ppEnum);
		}

		[Test]
		public void TestReset()
		{
			int hr;
			IPin[] pPins = new IPin[1];

			IEnumPins ppEnum = GetTestEnum();

			hr = ppEnum.Next(1, pPins, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

			hr = ppEnum.Reset();
			DsError.ThrowExceptionForHR(hr);

            hr = ppEnum.Next(1, pPins, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);
			
			Marshal.ReleaseComObject(ppEnum);
		}

		[Test]
		public void TestSkip()
		{
			int hr;
			IPin[] pPins = new IPin[1];

			IEnumPins ppEnum = GetTestEnum();

            hr = ppEnum.Next(1, pPins, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

			hr = ppEnum.Skip(1);
			DsError.ThrowExceptionForHR(hr);

            hr = ppEnum.Next(1, pPins, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);
			
			Marshal.ReleaseComObject(ppEnum);
		}

		[Test]
		public void TestClone()
		{
			int hr;
			IPin[] pPins = new IPin[1];

			IEnumPins ppEnum = GetTestEnum();
			IEnumPins cloneEnum;
			hr = ppEnum.Clone(out cloneEnum);
			DsError.ThrowExceptionForHR(hr);

            hr = cloneEnum.Next(1, pPins, IntPtr.Zero);
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