using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBSTuningSpaceTest
	{
    private IDVBSTuningSpace tuningSpace;
    
    public IDVBSTuningSpaceTest()
		{
		}

    public void DoTests()
    {
      try
      {
        // No special HW required for this sample
        tuningSpace = (IDVBSTuningSpace) BDAUtils.GetTuningSpace(BDANetworkType.DVBS);

        TestHighOscillator();
        TestInputRange();
        TestLNBSwitch();
        TestLowOscillator();
        TestSpectralInversion();
      }
      finally
      {
        Marshal.ReleaseComObject(tuningSpace);
      }
    }

    private void TestHighOscillator()
    {
      int hr = 0;
      int val;

      hr = tuningSpace.put_HighOscillator(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_HighOscillator(out val);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(val == 1234, "IDVBSTuningSpace.get_HighOscillator / put_HighOscillator");
    }

    private void TestInputRange()
    {
      int hr = 0;
      string val;

      // Documented as int in MSDN but as BSTR in idl & h files...
      hr = tuningSpace.put_InputRange("1234");
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_InputRange(out val);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(val == "1234", "IDVBSTuningSpace.get_InputRange / put_InputRange");
    }

    private void TestLNBSwitch()
    {
      int hr = 0;
      int val;

      hr = tuningSpace.put_LNBSwitch(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_LNBSwitch(out val);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(val == 1234, "IDVBSTuningSpace.get_LNBSwitch / put_LNBSwitch");
    }

    private void TestLowOscillator()
    {
      int hr = 0;
      int val;

      hr = tuningSpace.put_LowOscillator(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_LowOscillator(out val);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(val == 1234, "IDVBSTuningSpace.get_LowOscillator / put_LowOscillator");
    }

    private void TestSpectralInversion()
    {
      int hr = 0;
      SpectralInversion val;

      hr = tuningSpace.put_SpectralInversion(SpectralInversion.NORMAL);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_SpectralInversion(out val);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(val == SpectralInversion.NORMAL, "IDVBSTuningSpace.get_SpectralInversion / put_SpectralInversion");
    }
  }
}
