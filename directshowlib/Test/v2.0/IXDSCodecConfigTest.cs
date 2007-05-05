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
    class IXDSCodecConfigTest
    {
        IXDSCodecConfig m_xcc;

        public void DoTests()
        {
            int hr;
            object o;

            Configure();

            hr = m_xcc.SetPauseBufferTime(100);
            DsError.ThrowExceptionForHR(hr);

            hr = m_xcc.GetSecureChannelObject(out o);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(o != null, "GetSecureChannelObject");
        }

        private void Configure()
        {
            // BDA MPEG2 Transport Information Filter
            Guid g = new Guid("{fc772ab0-0c7f-11d3-8ff2-00a0c9224cf4}");
            Type type = Type.GetTypeFromCLSID(g);
            m_xcc = Activator.CreateInstance(type) as IXDSCodecConfig;
        }
    }
}

