using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IATSCComponentTypeTest
    {
        private IATSCComponentType m_ct = null;

        public IATSCComponentTypeTest()
        {
        }

        public void DoTests()
        {
            m_ct = (IATSCComponentType) new ATSCComponentType();

            try
            {
                TestFlags();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ct);
            }
        }

        private void TestFlags()
        {
            int hr;
            ATSCComponentTypeFlags f;

            hr = m_ct.put_Flags(ATSCComponentTypeFlags.ATSCCT_AC3);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ct.get_Flags(out f);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(f == ATSCComponentTypeFlags.ATSCCT_AC3, "Flags");
        }

    }
}
