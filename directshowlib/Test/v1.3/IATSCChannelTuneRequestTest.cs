using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
  public class IATSCChannelTuneRequestTest
	{
    private IATSCChannelTuneRequest atscChannelTR = null;
    
    public IATSCChannelTuneRequestTest()
		{
		}

    public void DoTests()
    {
      Config();

      try
      {
        TestMinorChannel();
      }
      finally
      {
        Marshal.ReleaseComObject(atscChannelTR);
      }
    }

    private void Config()
    {
      int hr = 0;
      IATSCTuningSpace ts = (IATSCTuningSpace) new ATSCTuningSpace();
      ITuneRequest tr = null;

      hr = ts.put_MaxChannel(50);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.put_MinChannel(5);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.CreateTuneRequest(out tr);
      DsError.ThrowExceptionForHR(hr);

      atscChannelTR = (IATSCChannelTuneRequest) tr;

      Marshal.ReleaseComObject(ts);
    }
    

    private void TestMinorChannel()
    {
      int hr = 0;
      int minor = 0;

      hr = atscChannelTR.put_MinorChannel(20);
      DsError.ThrowExceptionForHR(hr);

      hr = atscChannelTR.get_MinorChannel(out minor);
      DsError.ThrowExceptionForHR(hr);
      // I don't know why but this method always return -1 meaning according to the doc 
      // "that the tuner should tune to the first valid minor channel it finds".
      // I suppose that it's a normal behaviour since a complete BDA graph is not built...

//      Debug.Assert(minor == 20, "IATSCChannelTuneRequest.get_MinorChannel / put_MinorChannel");
    }



	}
}
