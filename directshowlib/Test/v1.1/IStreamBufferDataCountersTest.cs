using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferDataCountersTest
    {
        IStreamBufferDataCounters m_isbdc;

        public IStreamBufferDataCountersTest()
        {
        }

        public void DoTests()
        {
            TestGetData();
            TestReset();
        }

        public void TestGetData()
        {
            int hr;
            IBaseFilter ibf = (IBaseFilter)new StreamBufferSink();
            IPin iPin = DsFindPin.ByDirection(ibf, PinDirection.Input, 0);

            m_isbdc = (IStreamBufferDataCounters)iPin;

            try
            {
                SBEPinData pPinData;
                pPinData.cDataBytes = 33;
                pPinData.cTimestamps = 43;

                hr = m_isbdc.GetData(out pPinData);
                DsError.ThrowExceptionForHR( hr );

                Debug.Assert(pPinData.cDataBytes == 0, "GetData");
                Debug.Assert(pPinData.cTimestamps == 0, "GetData2");
            }
            finally
            {
                Marshal.ReleaseComObject(ibf);
                Marshal.ReleaseComObject(iPin);
            }
        }

        public void TestReset()
        {
            int hr;

            hr = m_isbdc.ResetData();
            DsError.ThrowExceptionForHR( hr );
        }
    }
}
