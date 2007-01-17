using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IETFilterConfigTest
    {
        IETFilterConfig m_ftc;

        public void DoTests()
        {
            Configure();

            try
            {
                TestInit();
                TestSec();
            }
            finally
            {
            }
        }

        private void TestInit()
        {
            int hr;

            hr = m_ftc.InitLicense(0);  // Not implemented
            //DsError.ThrowExceptionForHR(hr);
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
            m_ftc = (IETFilterConfig)new ETFilter();
        }
    }
}
