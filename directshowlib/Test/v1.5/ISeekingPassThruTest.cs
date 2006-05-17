using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
    public class ISeekingPassThruTest
    {
        public void DoTests()
        {
            TestEm();
        }

        private void TestEm()
        {
            int hr;

            ISeekingPassThru spt = new SeekingPassThru() as ISeekingPassThru;

            IBaseFilter ibf = new VideoRenderer() as IBaseFilter;

            IPin pPin = DsFindPin.ByDirection(ibf, PinDirection.Input, 0);

            hr = spt.Init(true, pPin);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
