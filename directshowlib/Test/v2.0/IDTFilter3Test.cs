using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IDTFilter3Test
    {
        IDTFilter3 m_ft;

        public void DoTests()
        {
            int hr;
            ProtType pProtectionType;
            bool pfLicenseHasExpirationDate;

            Configure();

            try
            {
                hr = m_ft.GetProtectionType(out pProtectionType);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pProtectionType == ProtType.Never, "GetProtectionType");

                hr = m_ft.LicenseHasExpirationDate(out pfLicenseHasExpirationDate);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pfLicenseHasExpirationDate == true, "LicenseHasExpirationDate");

                // Not implemented
                hr = m_ft.SetRights("asdf");
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
            }
        }

        private void Configure()
        {
            m_ft = (IDTFilter3)new DTFilter();
        }
    }
}
