using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
	public class IAuxInTuningSpace2Test
	{
        public void DoTests()
        {
            int hr;
            int i;
            IAuxInTuningSpace2 loc = new AuxInTuningSpace() as IAuxInTuningSpace2;

            hr = loc.put_CountryCode(123);
            DsError.ThrowExceptionForHR(hr);

            hr = loc.get_CountryCode(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 123, "CountryCode");
        }
	}
}
