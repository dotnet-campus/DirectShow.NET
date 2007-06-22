#if ALLOW_UNTESTED_INTERFACES

// The only filter I know that supports this interface returns E_NOTIMPL for all methods

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IBDA_TransportStreamInfoTest
    {
        IBDA_TransportStreamInfo m_tsi;

        public void DoTests()
        {
            int i;
            Configure();

            int hr = m_tsi.get_PatTableTickCount(out i);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            Guid g = new Guid("{fc772ab0-0c7f-11d3-8ff2-00a0c9224cf4}");
            Type type = Type.GetTypeFromCLSID(g);
            m_tsi = Activator.CreateInstance(type) as IBDA_TransportStreamInfo;
        }
    }
}

#endif
