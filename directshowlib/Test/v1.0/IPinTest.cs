using System;
using System.Collections;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	/// <summary>
	/// IPin Test is used to test all the calls for the IPin interface
	/// </summary>
	[TestFixture]
	public class IPinTest
	{

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestQueryDirection()
		{
			IPin testPin = GetTestPin();
			PinDirection pDir;
			int hr = testPin.QueryDirection(out pDir);			
			Marshal.ThrowExceptionForHR(hr);

			Assert.IsTrue(pDir == PinDirection.Input || pDir == PinDirection.Output);
		}


		[Test]
		public void TestQueryPinInfo()
		{
			IPin testPin = GetTestPin();
			PinInfo pinInfo;
			int hr = testPin.QueryPinInfo(out pinInfo);
			Marshal.ThrowExceptionForHR(hr);

			Assert.IsNotNull(pinInfo);
			Assert.IsNotNull(pinInfo.name);
		}

		[Test]
		public void TestQueryId()
		{
			IPin testPin = GetTestPin();
			string idStr;
			int hr = testPin.QueryId(out idStr);
			Marshal.ThrowExceptionForHR(hr);

			Assert.IsNotNull(idStr);
		}

		[Test]
		public void TestEnumMediaTypes()
		{
			IPin testPin = GetTestPin();
			IEnumMediaTypes enumMediaTypes;
			int hr = testPin.EnumMediaTypes(out enumMediaTypes);
			Marshal.ThrowExceptionForHR(hr);

			hr = enumMediaTypes.Reset();
			Marshal.ThrowExceptionForHR(hr);			
			Assert.IsNotNull(enumMediaTypes);
		}


		[Test]
		public void TestQueryInternalConnections()
		{
			IPin testPin = GetTestPin();
			int nPin = 0;
			IPin[] ppPins = null;
			int hr = testPin.QueryInternalConnections(ppPins, ref nPin);

			Marshal.ThrowExceptionForHR(hr);
			Console.Write(nPin);

			Assert.IsNotNull(ppPins);
		}




		private static IPin GetTestPin()
		{
			ArrayList audioInputDeviceList = new ArrayList();
			DsDev.GetDevicesOfCat(FilterCategory.AudioInputDevice, out audioInputDeviceList);
	
			Assert.IsTrue(audioInputDeviceList.Count > 0);
			DsDevice audioInputDevice = (DsDevice) audioInputDeviceList[0];
	
			Console.WriteLine("Testing with " + audioInputDevice.Name);
	
			string monikerName;
			audioInputDevice.Mon.GetDisplayName(null, null, out monikerName);
			IBaseFilter inputFilter = (IBaseFilter) Marshal.BindToMoniker(monikerName);
			
			
			int hr;
			int lFetched;
			IEnumPins ppEnum;
			PinInfo ppinfo;
			IPin pRet = null;
			IPin[] pPins = new IPin[1];

			hr = inputFilter.EnumPins(out ppEnum);
			Marshal.ThrowExceptionForHR(hr);

			while ((ppEnum.Next(1, pPins, out lFetched) >= 0) && (lFetched == 1))
			{
				hr = pPins[0].QueryPinInfo(out ppinfo);
				Marshal.ThrowExceptionForHR(hr);

				pRet = pPins[0];
				break;
			}
			Marshal.ReleaseComObject(ppEnum);

			return pRet;

		}
	}
}
