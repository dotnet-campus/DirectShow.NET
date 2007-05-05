using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IFilterGraph3Test
    {
        IFilterGraph3 m_ifg3;
        IBaseFilter m_ibf;

        public void DoTests()
        {
            Configure();

            IReferenceClock systemClock = (IReferenceClock) new SystemClock();

            int hr = m_ifg3.SetSyncSourceEx(systemClock, systemClock, m_ibf);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(m_ibf);
            Marshal.ReleaseComObject(m_ifg3);
        }

        public void Configure()
        {
            m_ifg3 = new FilterGraph() as IFilterGraph3;

             m_ibf = new SampleGrabber() as IBaseFilter;
            int hr = m_ifg3.AddFilter(m_ibf, "SG");
            DsError.ThrowExceptionForHR(hr);
        }
    }
}

