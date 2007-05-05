using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IDTFilter2Test
    {
        IDTFilter2 m_ft;

        public void DoTests()
        {
            Configure();

            try
            {
                TestGetLastErrorCode();
                Testget_ChallengeUrl();
                TestGetCurrLicenseExpDate();
            }
            finally
            {
            }
        }

        private void Testget_ChallengeUrl()
        {
            int hr;
            string s;

            hr = m_ft.get_ChallengeUrl(out s);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetCurrLicenseExpDate()
        {
            int hr;
            ProtType p = ProtType.Once;
            int p2;

            // Not implemented (per docs)
            hr = m_ft.GetCurrLicenseExpDate(p, out p2);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetLastErrorCode()
        {
            int hr;

            hr = m_ft.GetLastErrorCode();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_ft = (IDTFilter2)new DTFilter();
        }
    }
}
