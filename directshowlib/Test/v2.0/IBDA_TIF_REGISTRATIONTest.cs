// IBDA_TIF_REGISTRATION					2	"Applications do not use this interface."

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IBDA_TIF_REGISTRATIONTest
	{
        IBDA_TIF_REGISTRATION m_tr;

        public void DoTests()
        {
            Config();

            TestReg();
        }

        private void TestReg()
        {
            int hr;
            int i;
            object o;

            hr = m_tr.RegisterTIFEx(null, out i, out o);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_tr = new ATSCNetworkProvider() as IBDA_TIF_REGISTRATION;
        }
	}
}
