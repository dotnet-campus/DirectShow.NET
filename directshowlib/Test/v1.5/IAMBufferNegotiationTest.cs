using System;
using System.Collections.Generic;
using System.Text;

using DirectShowLib;

namespace v1._5
{
    public class IAMBufferNegotiationTest
    {
        IAMBufferNegotiation m_ibn;
        IBaseFilter m_ibf;

        public void DoTests()
        {
            Setup();

            TestEm();
        }

        private void TestEm()
        {
            int hr;
            AllocatorProperties prop = new AllocatorProperties();
            Guid grf = typeof(IBaseFilter).GUID;

            prop.cbAlign = 1;
            prop.cbBuffer = 3000000;
            prop.cbPrefix = 0;
            prop.cBuffers = 12;

            hr = m_ibn.SuggestAllocatorProperties(prop);
            DsError.ThrowExceptionForHR(hr);

            IGraphBuilder ifg = new FilterGraph() as IGraphBuilder;
            DsROTEntry rot = new DsROTEntry(ifg);

            hr = ifg.AddFilter(m_ibf, "Device");
            DsError.ThrowExceptionForHR(hr);

            ICaptureGraphBuilder2 icgb2 = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = icgb2.SetFiltergraph(ifg);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb2.RenderStream(null, null, m_ibf, null, null);
            DsError.ThrowExceptionForHR(hr);

            // Returns E_FAIL for all my devices, so I wrote my own filter
            // that implements it for a test.  Note: You CANNOT use "out" here.
            hr = m_ibn.GetAllocatorProperties(prop);
            //DsError.ThrowExceptionForHR(hr);
            rot.Dispose();
        }

        private void Setup()
        {
            object o;
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            Guid grf = typeof(IBaseFilter).GUID;

            devs[0].Mon.BindToObject(null, null, ref grf, out o);
            IPin pPin = DsFindPin.ByDirection((IBaseFilter)o, PinDirection.Output, 0);

            m_ibn = pPin as IAMBufferNegotiation;
            m_ibf = o as IBaseFilter;
        }
    }
}
