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
    class IReferenceClockTimerControlTest
    {
        IReferenceClockTimerControl m_rctc;

        public void DoTests()
        {
            int hr;
            long l;

            Configure();

            hr = m_rctc.SetDefaultTimerResolution(20000);
            DsError.ThrowExceptionForHR(hr);

            hr = m_rctc.GetDefaultTimerResolution(out l);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l == 20000, "DefaultTimerResolution");
        }

        private void Configure()
        {
            m_rctc = new SystemClock() as IReferenceClockTimerControl;
        }
    }
}

