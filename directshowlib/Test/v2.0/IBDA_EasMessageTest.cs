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
    class IBDA_EasMessageTest
    {
        IBDA_EasMessage m_eas;

        public void DoTests()
        {
            int i = 3;
            object o;

            Configure();

            int hr = m_eas.get_EasMessage(i, out o);
            DsError.ThrowExceptionForHR(hr);
        }

        public void Configure()
        {
            // BDA MPEG2 Transport Information Filter
            Guid g = new Guid("{fc772ab0-0c7f-11d3-8ff2-00a0c9224cf4}");
            Type type = Type.GetTypeFromCLSID(g);
            m_eas = Activator.CreateInstance(type) as IBDA_EasMessage;
        }
    }
}

