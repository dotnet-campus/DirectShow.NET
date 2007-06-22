#if ALLOW_UNTESTED_INTERFACES

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IGetCapabilitiesKeyTest
    {
        IGetCapabilitiesKey m_ck;

        public void DoTests()
        {
            Configure();

            try
            {
                int hr;
                IntPtr ip;

                hr = m_ck.GetCapabilitiesKey(out ip);
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
            }
        }

        private void Configure()
        {
            m_ck = (IGetCapabilitiesKey)new CDeviceMoniker();
        }
    }
}

#endif
