using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v1._6
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

            // Not implemented
            hr = m_ft.get_ChallengeUrl(out s);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetCurrLicenseExpDate()
        {
            int hr;
            int i = 0;
            ProtType p = ProtType.Once;
            IntPtr p1 = Marshal.AllocCoTaskMem(8);
            IntPtr p2 = Marshal.AllocCoTaskMem(8);

            // Not implemented
            hr = m_ft.GetCurrLicenseExpDate(out p1, out p2);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGetLastErrorCode()
        {
            int hr;

            hr = m_ft.GetLastErrorCode();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_ft = (IDTFilter2)new DecryptTag();
        }
    }
}
