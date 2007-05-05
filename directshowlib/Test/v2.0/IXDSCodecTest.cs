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
    class IXDSCodecTest
    {
        IXDSCodec m_xc;

        public void DoTests()
        {
            int hr;
            object o;

            Configure();
        }

        private void Configure()
        {
            // BDA MPEG2 Transport Information Filter
            Guid g = new Guid("{fc772ab0-0c7f-11d3-8ff2-00a0c9224cf4}");
            Type type = Type.GetTypeFromCLSID(g);
            m_xc = Activator.CreateInstance(type) as IXDSCodec;
        }
    }
}

