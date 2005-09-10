using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class ILocatorTest
	{
    private ILocator locator;

		public ILocatorTest()
		{
		}

    public void DoTests()
    {
      try
      {
        locator = (ILocator) new DVBTLocator();

        TestClone();
        TestCarrierFrequency();
        TestFEC();
        TestFECRate();
        TestModulation();
        TestSymbolRate();
      }
      finally
      {
        Marshal.ReleaseComObject(locator);
      }
    }

    private void TestClone()
    {
      int hr = 0;
      ILocator newObj;

      hr = locator.Clone(out newObj);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((newObj != null), "ILocator.Clone");

      Marshal.ReleaseComObject(newObj);
    }

    private void TestCarrierFrequency()
    {
      int hr = 0;
      int freq = 0;

      hr = locator.put_CarrierFrequency(12345);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_CarrierFrequency(out freq);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((freq == 12345), "ILocator.get_CarrierFrequency / put_CarrierFrequency");
    }

    private void TestFEC()
    {
      int hr = 0;
      FECMethod fec;

      hr = locator.put_InnerFEC(FECMethod.VITERBI);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_InnerFEC(out fec);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((fec == FECMethod.VITERBI), "ILocator.get_InnerFEC / put_InnerFEC");

      hr = locator.put_OuterFEC(FECMethod.RS_204_188);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_OuterFEC(out fec);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((fec == FECMethod.RS_204_188), "ILocator.get_OuterFEC / put_OuterFEC");
    }

    private void TestFECRate()
    {
      int hr = 0;
      BinaryConvolutionCodeRate rate;

      hr = locator.put_InnerFECRate(BinaryConvolutionCodeRate.RATE_3_4);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_InnerFECRate(out rate);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((rate == BinaryConvolutionCodeRate.RATE_3_4), "ILocator.get_InnerFECRate / put_InnerFECRate");

      hr = locator.put_OuterFECRate(BinaryConvolutionCodeRate.RATE_4_5);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_OuterFECRate(out rate);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((rate == BinaryConvolutionCodeRate.RATE_4_5), "ILocator.get_OuterFECRate / put_OuterFECRate");
    }

    private void TestModulation()
    {
      int hr = 0;
      ModulationType mod;

      hr = locator.put_Modulation(ModulationType.MOD_96QAM);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_Modulation(out mod);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(mod == ModulationType.MOD_96QAM, "ILocator.get_Modulation / put_Modulation");
    }

    private void TestSymbolRate()
    {
      int hr = 0;
      int symb;

      hr = locator.put_SymbolRate(54321);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_SymbolRate(out symb);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(symb == 54321, "ILocator.get_SymbolRate / put_SymbolRate");
    }


	}
}
