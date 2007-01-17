using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IETFilterTest
    {
        IETFilter m_ft;

        public void DoTests()
        {
            Configure();

            try
            {
                TestOK();
                TestCurRate();
                TestGetCurrLicenseExpDate();
                TestGetLastErrorCode();
                TestRecord();
            }
            finally
            {
            }
        }

        private void TestOK()
        {
            int hr;
            int hr2;

            hr = m_ft.get_EvalRatObjOK(out hr2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr2 == 0, "get_EvalRatObjOK");
        }

        private void TestCurRate()
        {
            int hr;
            EnTvRat_System s;
            EnTvRat_GenericLevel g;
            BfEnTvRat_GenericAttributes i;

            hr = m_ft.GetCurrRating(out s, out g, out i);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetCurrLicenseExpDate()
        {
            int hr;
            int t;

            hr = m_ft.GetCurrLicenseExpDate(ProtType.Free, out t);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetLastErrorCode()
        {
            int hr;

            hr = m_ft.GetLastErrorCode();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestRecord()
        {
            int hr;

            hr = m_ft.SetRecordingOn(true);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_ft = (IETFilter)new ETFilter();
        }
    }
}
