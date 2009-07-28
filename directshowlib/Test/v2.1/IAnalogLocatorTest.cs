using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.SBE;

namespace v2_1
{
    public class IAnalogLocatorTest
    {
        public IAnalogLocatorTest()
        {
        }

        public void DoTests()
        {
            int hr;
            AnalogLocator dcts = new AnalogLocator();

            IAnalogLocator ts = dcts as IAnalogLocator;
            Debug.Assert(ts != null);

            AnalogVideoStandard avs;
            hr = ts.put_VideoStandard(AnalogVideoStandard.NTSC_433);
            DsError.ThrowExceptionForHR(hr);

            hr = ts.get_VideoStandard(out avs);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(avs == AnalogVideoStandard.NTSC_433);
        }
    }
}
