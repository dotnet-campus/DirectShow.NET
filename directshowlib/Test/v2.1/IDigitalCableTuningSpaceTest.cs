using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.SBE;

namespace v2_1
{
    public class IDigitalCableTuningSpaceTest
    {
        public IDigitalCableTuningSpaceTest()
        {
        }

        public void DoTests()
        {
            DigitalCableTuningSpace dcts = new DigitalCableTuningSpace();

            IDigitalCableTuningSpace ts = dcts as IDigitalCableTuningSpace;
            Debug.Assert(ts != null);
        }
    }
}
