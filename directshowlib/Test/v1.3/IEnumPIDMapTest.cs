using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IEnumPIDMapTest
    {
        IEnumPIDMap m_pm;

        public IEnumPIDMapTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestNext();
                TestSkip();
                TestClone();
                TestReset();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pm);
            }
        }

        private void TestNext()
        {
            int hr;
            int pf;
            PIDMap [] m = new PIDMap[3];

            hr = m_pm.Next(1, m, out pf);
            DsError.ThrowExceptionForHR(hr);

            //Debug.Assert(m[2] != null, "Next");
        }

        private void TestSkip()
        {
            int hr;

            hr = m_pm.Skip(1);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestClone()
        {
            int hr;
            IEnumPIDMap pEnum;

            hr = m_pm.Clone(out pEnum);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestReset()
        {
            int hr;

            hr = m_pm.Reset();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            int hr;
            IFilterGraph gb = (IFilterGraph )new FilterGraph();
            DsROTEntry rot = new DsROTEntry(gb);
            IBaseFilter pFilter = (IBaseFilter)new MPEG2Demultiplexer();
            AMMediaType amt = new AMMediaType();
            IPin pPin;

            hr = gb.AddFilter(pFilter, "fdsa");
            DsError.ThrowExceptionForHR(hr);

            IMpeg2Demultiplexer ism = (IMpeg2Demultiplexer)pFilter;

            hr = ism.CreateOutputPin(amt, "Pin1", out pPin);
            DsError.ThrowExceptionForHR(hr);

            IMPEG2PIDMap pmap = pPin as IMPEG2PIDMap;
            hr = pmap.EnumPIDMap(out m_pm);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
