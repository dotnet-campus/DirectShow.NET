using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IChannelTuneRequestTest
	{
    private IChannelTuneRequest channelTR = null;
    
    public IChannelTuneRequestTest()
		{
		}

    public void DoTests()
    {
      Config();

      try
      {
        TestChannel();
      }
      finally
      {
        Marshal.ReleaseComObject(channelTR);
      }
    }

    private void Config()
    {
      int hr = 0;
      IAnalogTVTuningSpace ts = (IAnalogTVTuningSpace) new AnalogTVTuningSpace();
      ITuneRequest tr = null;

      hr = ts.put_CountryCode(33);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.put_InputType(TunerInputType.Cable);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.put_MaxChannel(50);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.put_MinChannel(5);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.CreateTuneRequest(out tr);
      DsError.ThrowExceptionForHR(hr);

      channelTR = (IChannelTuneRequest) tr;

      Marshal.ReleaseComObject(ts);
    }

    private void TestChannel()
    {
      int hr = 0;
      int channel = 0;

      hr = channelTR.put_Channel(10);
      DsError.ThrowExceptionForHR(hr);

      hr = channelTR.get_Channel(out channel);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(channel == 10, "IChannelTuneRequest.get_Channel / put_Channel");

      hr = channelTR.put_Channel(20);
      DsError.ThrowExceptionForHR(hr);

      hr = channelTR.get_Channel(out channel);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(channel == 20, "IChannelTuneRequest.get_Channel / put_Channel");
    }


	}
}
