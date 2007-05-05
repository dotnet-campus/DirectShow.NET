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
    class IATSCLocator2Test
    {
        IATSCLocator2 m_al;

        public void DoTests()
        {
            int hr;
            int p;

            Configure();

            hr = m_al.put_ProgramNumber(213);
            DsError.ThrowExceptionForHR(hr);

            hr = m_al.get_ProgramNumber(out p);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(p == 213, "ProgramNumber");
        }

        private void Configure()
        {
            m_al = new ATSCLocator() as IATSCLocator2;
        }
    }
}

