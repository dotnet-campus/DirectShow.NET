#if ALLOW_UNTESTED_INTERFACES

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class ITuneRequestInfoTest
    {
        ITuneRequestInfo m_tri;

        public void DoTests()
        {
            Configure();
        }

        private void Configure()
        {
            // BDA MPEG2 Transport Information Filter
            Guid g = new Guid("{fc772ab0-0c7f-11d3-8ff2-00a0c9224cf4}");
            Type type = Type.GetTypeFromCLSID(g);
            m_tri = Activator.CreateInstance(type) as ITuneRequestInfo;
        }
    }
}

#endif
