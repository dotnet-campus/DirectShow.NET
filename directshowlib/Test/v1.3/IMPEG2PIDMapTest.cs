using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IMPEG2PIDMapTest
    {
        IMPEG2PIDMap m_ipid;

        public IMPEG2PIDMapTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestMap();
                TestEnum();
                TestDelete();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ipid);
            }
        }

        private void TestMap()
        {
            int hr;
            int [] arr = new int[2];

            arr[0] = 0xc0;
            arr[1] = 0xc1;

            hr = m_ipid.MapPID(arr.Length, arr, MediaSampleContent.ElementaryStream);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestEnum()
        {
            int hr;
            IEnumPIDMap pEnum;

            hr = m_ipid.EnumPIDMap(out pEnum);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestDelete()
        {
            int hr;
            int [] elem = new int[2];

            elem[0] = 0xc0;
            elem[1] = 0xc1;
            
            hr = m_ipid.UnmapPID(elem.Length,  elem);
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

            m_ipid = (IMPEG2PIDMap)pPin;
        }
    }
}
