using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IMPEG2StreamIdMapTest
    {
        IMPEG2StreamIdMap m_sim;

        public IMPEG2StreamIdMapTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestMap();
                //TestEnum(); Broken until MS fixes the QI for IEnumStreamIdMap
                TestDelete();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sim);
            }
        }

        private void TestMap()
        {
            int hr;

            hr = m_sim.MapStreamId(0xc0, MPEG2Program.ElementaryStream, 0, 0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sim.MapStreamId(0xc1, MPEG2Program.ElementaryStream, 0, 0);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestEnum()
        {
            int hr;
            IEnumStreamIdMap pEnum;

            hr = m_sim.EnumStreamIdMap(out pEnum);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestDelete()
        {
            int hr;
            int [] elem = new int[2];

            elem[0] = 0xc0;
            elem[1] = 0xc1;
            
            hr = m_sim.UnmapStreamId(elem.Length,  elem);
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

            IMpeg2Demultiplexer ism = (IMpeg2Demultiplexer)pFilter;

            hr = ism.CreateOutputPin(amt, "Pin1", out pPin);
            DsError.ThrowExceptionForHR(hr);

            m_sim = (IMPEG2StreamIdMap)pPin;
        }
    }
}
