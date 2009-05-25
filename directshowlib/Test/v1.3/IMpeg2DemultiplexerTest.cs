using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IMpeg2DemultiplexerTest
    {
        IMpeg2Demultiplexer m_ism;

        public IMpeg2DemultiplexerTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestCreate();
                TestSet();
                TestDelete();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ism);
            }
        }

        private void TestCreate()
        {
            int hr;
            IPin pPin;
            AMMediaType amt = new AMMediaType();

            hr = m_ism.CreateOutputPin(amt, "Pin1", out pPin);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestSet()
        {
            int hr;
            AMMediaType amt = new AMMediaType();
            amt.majorType = MediaType.Texts;

            hr = m_ism.SetOutputPinMediaType("Pin1", amt);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestDelete()
        {
            int hr;

            hr = m_ism.DeleteOutputPin("Pin1");
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            int hr;
            IGraphBuilder gb = (IGraphBuilder )new FilterGraph();
            DsROTEntry rot = new DsROTEntry(gb);
            IBaseFilter pFilter = (IBaseFilter)new MPEG2Demultiplexer();

            hr = gb.AddFilter(pFilter, "mpeg demux");

            m_ism = (IMpeg2Demultiplexer)pFilter;
        }
    }
}
