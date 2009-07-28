using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.SBE;

namespace v2_1
{
    public class IDigitalCableTuneRequestTest
    {
        public IDigitalCableTuneRequestTest()
        {
        }

        public void DoTests()
        {
            DigitalCableTuneRequest dcts = new DigitalCableTuneRequest();

            IDigitalCableTuneRequest ts = dcts as IDigitalCableTuneRequest;
            Debug.Assert(ts != null);
        }
    }
}
