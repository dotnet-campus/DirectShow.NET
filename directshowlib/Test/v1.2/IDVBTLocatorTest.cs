using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBTLocatorTest
	{
    private IDVBTLocator locator;

		public IDVBTLocatorTest()
		{
		}

    public void DoTests()
    {
      try
      {
        locator = (IDVBTLocator) new DVBTLocator();

        TestBandwidth();
        TestGuard();
        TestHAlpha();
        TestLPInnerFEC();
        TestMode();
        TestOtherFrequencyInUse();

      }
      finally
      {
        Marshal.ReleaseComObject(locator);
      }
    }

    private void TestBandwidth()
    {
      int hr = 0;
      int band;

      hr = locator.put_Bandwidth(8);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_Bandwidth(out band);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(band == 8, "IDVBTLocator.get_Bandwidth / put_Bandwidth");
    }

    private void TestGuard()
    {
      int hr = 0;
      GuardInterval guard;

      hr = locator.put_Guard(GuardInterval.GUARD_1_32);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_Guard(out guard);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(guard == GuardInterval.GUARD_1_32, "IDVBTLocator.get_Guard / put_Guard");
    }

    private void TestHAlpha()
    {
      int hr = 0;
      HierarchyAlpha alpha;

      hr = locator.put_HAlpha(HierarchyAlpha.HALPHA_4);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_HAlpha(out alpha);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(alpha == HierarchyAlpha.HALPHA_4, "IDVBTLocator.get_HAlpha / put_HAlpha");
    }

    private void TestLPInnerFEC()
    {
      int hr = 0;
      FECMethod fec;
      BinaryConvolutionCodeRate rate;

      hr = locator.put_LPInnerFEC(FECMethod.VITERBI);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_LPInnerFEC(out fec);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((fec == FECMethod.VITERBI), "IDVBTLocator.get_LPInnerFEC / put_LPInnerFEC");

      hr = locator.put_LPInnerFECRate(BinaryConvolutionCodeRate.RATE_5_11);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_LPInnerFECRate(out rate);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((rate == BinaryConvolutionCodeRate.RATE_5_11), "IDVBTLocator.get_LPInnerFECRate / put_LPInnerFECRate");
    }

    private void TestMode()
    {
      int hr = 0;
      TransmissionMode mode;

      hr = locator.put_Mode(TransmissionMode.MODE_8K);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_Mode(out mode);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(mode == TransmissionMode.MODE_8K, "IDVBTLocator.get_Mode / put_Mode");
    }

    private void TestOtherFrequencyInUse()
    {
      int hr = 0;
      bool inUse;

      hr = locator.put_OtherFrequencyInUse(true);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_OtherFrequencyInUse(out inUse);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(inUse == true, "IDVBTLocator.gett_OtherFrequencyInUse / put_OtherFrequencyInUse");

      hr = locator.put_OtherFrequencyInUse(false);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_OtherFrequencyInUse(out inUse);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(inUse == false, "IDVBTLocator.gett_OtherFrequencyInUse / put_OtherFrequencyInUse");
    }

	}
}
