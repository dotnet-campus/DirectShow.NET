using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;
using System.Windows.Forms;

namespace v2_1
{
    public class IAMAsyncReaderTimestampScalingTest
    {
        IAMAsyncReaderTimestampScaling m_pi;

        public IAMAsyncReaderTimestampScalingTest()
        {
        }

        public void DoTests()
        {
            int hr;
            bool b = false;
            bool b2;
            IFileSourceFilter fsf;

            IBaseFilter ibf = new AsyncReader() as IBaseFilter;
            fsf = ibf as IFileSourceFilter;
            hr = fsf.Load(@"C:\Users\Public\Recorded TV\Sample Media\win7_scenic-demoshort_raw.wtv", null);

            IPin ppin = DsFindPin.ByDirection(ibf, PinDirection.Output, 0);

            m_pi = ppin as IAMAsyncReaderTimestampScaling;

            hr = m_pi.SetTimestampMode(b);
            DsError.ThrowExceptionForHR(hr);
            hr = m_pi.GetTimestampMode(out b2);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(b == b2);

            hr = m_pi.SetTimestampMode(!b);
            DsError.ThrowExceptionForHR(hr);
            hr = m_pi.GetTimestampMode(out b2);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(!b == b2);

        }
    }
}
