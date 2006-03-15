using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Summary description for IAMExtTransportTest.
	/// </summary>
	public class IAMExtTransportTest
	{
		private DsDevice _dvDevice = null;
        private IAMExtTransport _extTransport = null;
		
		public IAMExtTransportTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public void DoTests()
		{
			Setup();
			TestGetCapability();
			TestMediaState();
			TestLocalControl();
			TestGetStatus();
			
			TestGetSetTransportBasicParameters();
			TestSetGetTransportVideoParameters();

			TestSetGetAudioParameters();

			TestPutGetMode();

			TestPutGetRate();
			TestSetGetChase();
			TestSetGetBump();
			TestAntiClogControl();

			TestSetGetEditPropertyAndSet();
			TestPutGetEditStart();

		}

		private void TestPutGetEditStart()
		{
			int start = 120;
			int hr = _extTransport.put_EditStart(start);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			start = 0;
			hr = _extTransport.get_EditStart(out start);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV
			
			//Debug.Assert(start == 120, "PutGetEditStart");
		}

		private void TestSetGetEditPropertyAndSet()
		{
			int EditId = 0;
			int hr = _extTransport.SetEditPropertySet(ref EditId, ExtTransportEdit.Register);  
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			hr = _extTransport.SetEditProperty(EditId, ExtTransportEdit.Mode, (int)ExtTransportEdit.ModeInsert);
			//DsError.ThrowExceptionForHR(hr);//Not implemented by MSDV

			int returnValue = 0;
			hr = _extTransport.GetEditProperty(EditId, ExtTransportEdit.Mode, out returnValue);
			//DsError.ThrowExceptionForHR(hr);//Not implemented by MSDV

			ExtTransportEdit mode;
			hr = _extTransport.GetEditPropertySet(EditId, out mode);
			//DsError.ThrowExceptionForHR(hr);//Not implemented by MSDV
		}

		private void TestAntiClogControl()
		{
			int hr = _extTransport.put_AntiClogControl(1);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			int clog = 0;
			hr = _extTransport.get_AntiClogControl(out clog);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			//Debug.Assert(clog == 1, "AntiClogControl");
		}

		private void TestSetGetChase()
		{
			int enabled = 1;
			int offset = 0;
			IntPtr hEvent = IntPtr.Zero;
			int hr = _extTransport.SetChase(enabled, offset, hEvent);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			hr = _extTransport.GetChase(out enabled, out offset, out hEvent);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			//Debug.Assert(enabled == 1, "SetGetChase");
		}

		private void TestSetGetBump()
		{
			int speed = 1;
			int duration = 20;
			int hr = _extTransport.SetBump(speed, duration);
			//DsError.ThrowExceptionForHR(hr);// Not implemented by MSDV

			hr = _extTransport.GetBump(out speed, out duration);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			//Debug.Assert(speed == 1, "SetGetBump");
		}

		private void TestPutGetRate()
		{
			int hr = _extTransport.put_Rate(0.5);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			double rate = 0;
			hr = _extTransport.get_Rate(out rate);
			//DsError.ThrowExceptionForHR(hr); // Not implemented by MSDV

			//Debug.Assert(rate == 0.5, "TestPutGetRate");
		}

		private void TestGetSetTransportBasicParameters()
		{
			string returnString = "";
			int returnValue = 0;

			int hr = _extTransport.SetTransportBasicParameters(ExtTransportParameters.TimeFormat, (int)ExtTransportParameters.TimeFormatFrames, "");
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV
			
			hr = _extTransport.GetTransportBasicParameters(ExtTransportParameters.TimeFormat, out returnValue, out returnString);
			DsError.ThrowExceptionForHR(hr);

			Debug.Assert(returnValue == (int)ExtTransportParameters.TimeFormatHmsf, "GetTransportBasicParameters");

		}

		private void TestSetGetTransportVideoParameters()
		{
			int hr = _extTransport.SetTransportVideoParameters(ExtTransportParameters.VideoSetOutput, (int)ExtTransportParameters.Playback);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			int returnValue = 0;
			hr = _extTransport.GetTransportVideoParameters(ExtTransportParameters.VideoSetOutput, out returnValue);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV
		}

		private void TestSetGetAudioParameters()
		{
			int hr = _extTransport.SetTransportAudioParameters(ExtTransportParameters.AudioEnableOutput, ExtTransportAudio.AudioAll);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV

			int returnValue = 0;
			hr = _extTransport.GetTransportAudioParameters(ExtTransportParameters.AudioSetMonitor, out returnValue);
			//DsError.ThrowExceptionForHR(hr); //Not implemented by MSDV
		}



		private void TestGetStatus()
		{
			int result = 0;
			int hr = _extTransport.GetStatus(ExtTransportStatus.MediaType , out result);
			DsError.ThrowExceptionForHR(hr);
			int wanted = (int)ExtTransportStatus.MediaDvc;

			Debug.Assert(result == wanted, "TestGetStatus");

			wanted = (int)ExtTransportModes.Stop;

			hr = _extTransport.GetStatus(ExtTransportStatus.Mode, out result);
			DsError.ThrowExceptionForHR(hr);

			Debug.Assert(result == wanted, "TestGetStatus2");

		}

		private void TestLocalControl()
		{
			int hr = _extTransport.put_LocalControl(1);
			//DsError.ThrowExceptionForHR(hr); //not implemented for MSDV

			int state = 0;
			hr = _extTransport.get_LocalControl(out state);
			//DsError.ThrowExceptionForHR(hr);//not implemented for MSDV

			//Debug.Assert(state == 1, "LocalControl");
		}

		private void TestMediaState()
		{
			int hr = _extTransport.put_MediaState(ExtTransportMediaStates.SpinUp);
			//DsError.ThrowExceptionForHR(hr); //Not implemented for MSDV

			ExtTransportMediaStates current_state;
			hr = _extTransport.get_MediaState(out current_state);
			//DsError.ThrowExceptionForHR(hr); //Not implemented for MSDV

			//Debug.Assert(current_state == ExtTransportMediaStates.SpinUp);
		}

		private void TestGetCapability()
		{
			int intval;
			double dbval;

			int hr = _extTransport.GetCapability(ExtTransportCaps.HasTimer, out intval, out dbval);
			DsError.ThrowExceptionForHR(hr); //Not supported by MSDV
		}

		private void TestPutGetMode()
		{

			int hr = _extTransport.put_Mode(ExtTransportModes.Play);
			DsError.ThrowExceptionForHR(hr);
			hr = _extTransport.put_Mode(ExtTransportModes.Freeze);
			DsError.ThrowExceptionForHR(hr);

			ExtTransportModes currentmode;
			hr = _extTransport.get_Mode(out currentmode);
			DsError.ThrowExceptionForHR(hr);

			Debug.Assert(currentmode == ExtTransportModes.Freeze, "TestPutGetMode");

			hr = _extTransport.put_Mode(ExtTransportModes.Stop);
			DsError.ThrowExceptionForHR(hr);
		}

		private void Setup()
		{
			_dvDevice = DsDevice.GetDevicesOfCat(FilterCategory.TransmitCategory)[0];
			//Query source for interface
			object o;
			Guid iid = typeof(IBaseFilter).GUID;

			_dvDevice.Mon.BindToObject(null, null, ref iid, out o);
			_extTransport = (IAMExtTransport) o; 
		}
	}
}
