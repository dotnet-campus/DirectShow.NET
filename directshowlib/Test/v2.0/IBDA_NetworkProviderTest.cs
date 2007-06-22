#if ALLOW_UNTESTED_INTERFACES

// IBDA_NetworkProvider					7	????NetworkProvider doesn't seem to work

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IBDA_NetworkProviderTest
    {
        public void DoTests()
        {
            Guid g = Guid.NewGuid();
            int i = 3;
            int hr;

            IBDA_NetworkProvider idl = new DVBTNetworkProvider() as IBDA_NetworkProvider;

            hr = idl.PutSignalSource(4);
            hr = idl.GetSignalSource(out i);

            hr = idl.GetNetworkType(out g);
            hr = idl.GetTuningSpace(out g);
        }
    }
}

#endif
