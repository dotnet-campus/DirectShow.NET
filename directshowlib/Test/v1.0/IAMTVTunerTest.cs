using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IAMTVTunerTest : IAMTunerNotification
	{
    IFilterGraph2 graphBuilder = null;
    ICaptureGraphBuilder2 captureGraph = null;
    IBaseFilter filter = null;
    DsROTEntry rot = null;
    IAMTVTuner tuner = null;
    IBaseFilter vmr9 = null;
    IMediaControl mediaControl = null;

		public IAMTVTunerTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        // IAMTuner methods
        TestChannelMinMax();
        TestChannel();
        TestCountryCode();
        TestMode();
        TestTuningSpace();
        TestGetAvailableModes();
        TestLogonLogout();
        TestNotificationCallBack();
        TestSignalPresent();

        // IAMTVTuner methods
        TestAutoTune();
        TestFrequencies();
        TestAvailableTVFormats();
        TestConnectInput();
        TestInputType();
        TestNumInputConnections();
        TestTVFormat();
        TestStoreAutoTune();
      }
      finally
      {
        if (rot != null)
          rot.Dispose();
        Marshal.ReleaseComObject(this.filter);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    public void TestChannelMinMax()
    {
      int hr = 0;
      int chanMin = 0;
      int chanMax = 0;

      hr = tuner.ChannelMinMax(out chanMin, out chanMax);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IAMTVTuner.ChannelMinMax");
    }

    public void TestChannel()
    {
      int hr = 0;
      int chan1, chan2;
      AMTunerSubChannel vidSubChan, audSubChan;

      hr = tuner.get_Channel(out chan1, out vidSubChan, out audSubChan);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_Channel");

      hr = tuner.put_Channel(chan1 + 1, AMTunerSubChannel.Default, AMTunerSubChannel.Default);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.put_Channel");

      hr = tuner.get_Channel(out chan2, out vidSubChan, out audSubChan);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(chan2 == (chan1 + 1), "IAMTVTuner.get / put_Channel");
    }

    public void TestCountryCode()
    {
      int hr = 0;
      int code1, code2;

      hr = tuner.get_CountryCode(out code1);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_CountryCode");

      hr = tuner.put_CountryCode(code1 + 1);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.put_CountryCode");

      hr = tuner.get_CountryCode(out code2);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(code2 == (code1 + 1), "IAMTVTuner.get / put_CountryCode");
    }

    public void TestMode()
    {
      int hr = 0;
      AMTunerModeType mode;

      hr = tuner.get_Mode(out mode);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_Mode");

      // My hardware just return me AMTunerModeType.TV and forbidden me to set another value
      // So just try to set AMTunerModeType.TV and check the return value
      hr = tuner.put_Mode(AMTunerModeType.TV);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.put_Mode");
    }

    public void TestTuningSpace()
    {
      int hr = 0;
      int ts1, ts2;

      hr = tuner.get_TuningSpace(out ts1);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_TuningSpace");

      hr = tuner.put_TuningSpace(ts1 + 1);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.put_TuningSpace");

      hr = tuner.get_TuningSpace(out ts2);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(ts2 == (ts1 + 1), "IAMTVTuner.get / put_TuningSpace");
    }

    public void TestGetAvailableModes()
    {
      int hr = 0;
      AMTunerModeType modesAvailable;

      hr = tuner.GetAvailableModes(out modesAvailable);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.GetAvailableModes");
    }

    public void TestLogonLogout()
    {
      int hr = 0;

      hr = tuner.Logon(IntPtr.Zero);
      if (hr != -2147467263) //E_NOTIMPL
      {
        DsError.ThrowExceptionForHR(hr);
        Debug.Assert(hr == 0, "IAMTVTuner.Logon");

        hr = tuner.Logout();
        DsError.ThrowExceptionForHR(hr);
        Debug.Assert(hr == 0, "IAMTVTuner.Logout");
      }
    }

    public void TestNotificationCallBack()
    {
      int hr = 0;

      hr = tuner.RegisterNotificationCallBack(this, AMTunerEventType.Changed);
      Debug.Assert(hr == -2147467263, "IAMTVTuner.TestRegisterNotificationCallBack");

      hr = tuner.UnRegisterNotificationCallBack(this);
      Debug.Assert(hr == -2147467263, "IAMTVTuner.UnRegisterNotificationCallBack");
    }

    public void TestSignalPresent()
    {
      int hr = 0;
      AMTunerSignalStrength sig;

      hr = tuner.SignalPresent(out sig);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.SignalPresent");
    }

    public void TestAutoTune()
    {
      int hr = 0;
      int sig;

      hr = tuner.AutoTune(1, out sig);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.AutoTune");
    }

    public void TestFrequencies()
    {
      int hr = 0;
      int freq;

      hr = tuner.get_AudioFrequency(out freq);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_AudioFrequency");

      hr = tuner.get_VideoFrequency(out freq);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_VideoFrequency");
    }

    public void TestAvailableTVFormats()
    {
      int hr = 0;
      AnalogVideoStandard vidStandard;

      hr = tuner.get_AvailableTVFormats(out vidStandard);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_AvailableTVFormats");
    }

    public void TestConnectInput()
    {
      int hr = 0;
      int index;

      hr = tuner.get_ConnectInput(out index);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_ConnectInput");

      // With my hardware, i can only set what i get with get_ConnectInput (0)
      hr = tuner.put_ConnectInput(index);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.put_ConnectInput");
    }

    public void TestInputType()
    {
      int hr = 0;
      int index;
      TunerInputType inputType;

      hr = tuner.get_ConnectInput(out index);
      DsError.ThrowExceptionForHR(hr);

      hr = tuner.get_InputType(index, out inputType);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_InputType");

      inputType = (inputType == TunerInputType.Antenna) ? TunerInputType.Cable : TunerInputType.Antenna;

      hr = tuner.put_InputType(index, inputType);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.put_InputType");
    }

    public void TestNumInputConnections()
    {
      int hr = 0;
      int inputConn;

      hr = tuner.get_NumInputConnections(out inputConn);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_NumInputConnections");
    }

    public void TestTVFormat()
    {
      int hr = 0;
      AnalogVideoStandard vidStandard;

      hr = tuner.get_TVFormat(out vidStandard);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.get_TVFormat");
    }

    public void TestStoreAutoTune()
    {
      int hr = 0;

      hr = tuner.StoreAutoTune();
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IAMTVTuner.StoreAutoTune");
    }

    private void BuildGraph()
    {
      int hr = 0;
      DsDevice [] devs;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      vmr9 = (IBaseFilter) new VideoMixingRenderer9();
      hr = graphBuilder.AddFilter(vmr9, "VMR9");

      devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
      DsDevice dev = devs[0];

      hr = graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out filter);
      DsError.ThrowExceptionForHR(hr);

      captureGraph = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
      hr = captureGraph.SetFiltergraph(graphBuilder);
      DsError.ThrowExceptionForHR(hr);

      hr = captureGraph.RenderStream(PinCategory.Capture, MediaType.Video, filter, null, vmr9);
      DsError.ThrowExceptionForHR(hr);

      object tmp;
      hr = captureGraph.FindInterface(null, null, filter, typeof(IAMTVTuner).GUID, out tmp);
      DsError.ThrowExceptionForHR(hr);
      tuner = (IAMTVTuner) tmp;

      mediaControl = (IMediaControl) graphBuilder;
      mediaControl.Run();
    }
    #region Membres de IAMTunerNotification

    public int OnEvent(DirectShowLib.AMTunerEventType Event)
    {
      Debug.WriteLine(Event.ToString());
      return 0;
    }

    #endregion
  }
}
