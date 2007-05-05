using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IBroadcastEventExTest
    {
        IBroadcastEventEx m_ibee;

        public void DoTests()
        {
            Configure();

            int hr = m_ibee.FireEx(new Guid(), 1, 2, 3, 4);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            // WST Page filter
            Guid g = new Guid("{ad6c8934-f31b-4f43-b5e4-0541c1452f6f}");
            Type type = Type.GetTypeFromCLSID(g);
            m_ibee = Activator.CreateInstance(type) as IBroadcastEventEx;
        }
    }
}

