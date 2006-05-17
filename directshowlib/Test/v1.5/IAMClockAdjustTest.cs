using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
    public class IAMClockAdjustTest
    {
        public void DoTests()
        {
            TestEm();
        }

        private void TestEm()
        {
            const int offset = 3000;
            int hr;
            long l, l2;
            IAMClockAdjust ca = new SystemClock() as IAMClockAdjust;
            IReferenceClock rc = ca as IReferenceClock;

            hr = rc.GetTime(out l);
            DsError.ThrowExceptionForHR(hr);

            hr = ca.SetClockDelta(offset);
            DsError.ThrowExceptionForHR(hr);

            hr = rc.GetTime(out l2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l2 - l >= offset, "SetClockDelta");
        }
    }
}
