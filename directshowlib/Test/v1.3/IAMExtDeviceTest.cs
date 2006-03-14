using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Summary description for IAMExtDeviceTest.
	/// </summary>
	public class IAMExtDeviceTest
	{
		public IAMExtDeviceTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private DsDevice _dvDevice = null;
		private IAMExtDevice _extDevice = null;

		private void Setup()
		{
			_dvDevice = DsDevice.GetDevicesOfCat(FilterCategory.TransmitCategory)[0];
			//Query source for interface
			object o;
			Guid iid = typeof(IBaseFilter).GUID;

			_dvDevice.Mon.BindToObject(null, null, ref iid, out o);
			_extDevice= (IAMExtDevice) o; 


		}

		public void DoTests()
		{
			Setup();
			TestGetCapability();
			TestCalibrate();
			TestGetExternalDeviceID();
			TestGetExternalDeviceVersion();
			TestGetDevicePort();
			TestGetDevicePower();

		}

		private void TestGetDevicePower()
		{
			ExtDeviceCaps powermode = 0;
			int hr = _extDevice.get_DevicePower(out powermode);
			DsError.ThrowExceptionForHR(hr);

			Debug.Assert (powermode == ExtDeviceCaps.On, "TestGetDevicePower");

			hr = _extDevice.put_DevicePower(ExtDeviceCaps.Standby);
			//DsError.ThrowExceptionForHR(hr); //E_NOTIMPL

			Debug.Assert (powermode == ExtDeviceCaps.On, "TestGetDevicePower");

		}

		private void TestGetDevicePort()
		{
			ExtDevicePort port = 0;
			int hr = _extDevice.get_DevicePort(out port);
			DsError.ThrowExceptionForHR(hr);

			hr = _extDevice.put_DevicePort(ExtDevicePort.FireWire1394);
			//DsError.ThrowExceptionForHR(hr); //E_NOTIMPL;
		}

		private void TestGetExternalDeviceVersion()
		{
			string version = "";
			int hr = _extDevice.get_ExternalDeviceVersion(out version);
			DsError.ThrowExceptionForHR(hr);
		}
		
		private void TestGetExternalDeviceID()
		{
				string devId = "";
				int hr = _extDevice.get_ExternalDeviceID(out devId);
				DsError.ThrowExceptionForHR(hr);
		}

		private void TestGetCapability()
		{
			double ret = 0;
			ExtDeviceCaps devicetype = 0;
			int hr = _extDevice.GetCapability(ExtDeviceCaps.DeviceType, out devicetype, out ret);
			DsError.ThrowExceptionForHR(hr);

			Debug.Assert(devicetype == ExtDeviceCaps.VCR, "DeviceType");
		}

		private void TestCalibrate()
		{
			//No hardware
			IMediaEventSink mes;
			IFilterGraph2 FilterGraph;
			int hr;
			FilterGraph = (IFilterGraph2)new FilterGraph();

			hr = FilterGraph.RenderFile("foo.avi", null);
			DsError.ThrowExceptionForHR(hr);

			mes = (IMediaEventSink)FilterGraph;
			IMediaEvent pEvent = (IMediaEvent)FilterGraph;


			int ret = 0;

			IntPtr eventHandle = IntPtr.Zero;
			hr = pEvent.GetEventHandle(out eventHandle);
			DsError.ThrowExceptionForHR(hr);

			hr = _extDevice.Calibrate(eventHandle, ExtTransportEdit.Active, out ret);
			//DsError.ThrowExceptionForHR(hr); //E_NOTIMPL , but atleast it's called.

			hr = _extDevice.Calibrate(eventHandle, 0, out ret);
			//DsError.ThrowExceptionForHR(hr); //E_NOTIMPL , but atleast it's called.
		}
	}
}
