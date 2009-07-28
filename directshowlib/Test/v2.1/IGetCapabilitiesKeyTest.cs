using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.SBE;

namespace v2_1
{
    public class IGetCapabilitiesKeyTest
    {
        public IGetCapabilitiesKeyTest()
        {
        }

        public void DoTests()
        {
            int hr;
            IntPtr pk;
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.LegacyAmFilterCategory);

            foreach(DsDevice d in devs)
            {
                IGetCapabilitiesKey ts = d.Mon as IGetCapabilitiesKey;
                hr = ts.GetCapabilitiesKey(out pk);

                if (hr == 0)
                {
                    Debug.Assert(pk != IntPtr.Zero);
                    break;
                }
            }
        }
    }
}
