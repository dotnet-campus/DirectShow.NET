using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IDTFilterConfigTest
    {
        IDTFilterConfig m_ftc;

        public void DoTests()
        {
            Configure();

            try
            {
                TestSec();
            }
            finally
            {
            }
        }

        private void TestSec()
        {
            int hr;
            object o;

            hr = m_ftc.GetSecureChannelObject(out o);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(o != null, "GetSecureChannelObject");
        }

        private void Configure()
        {
            m_ftc = (IDTFilterConfig)new DTFilter();
        }
    }
}
