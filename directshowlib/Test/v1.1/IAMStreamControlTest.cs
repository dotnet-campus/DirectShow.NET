using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IAMStreamControlTest
    {
        IFilterGraph2 m_FilterGraph;
        IAMStreamControl m_sc;

        public IAMStreamControlTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestStart();
                TestStop();
                TestInfo();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sc);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestStart()
        {
            int hr;
            AMStreamInfo pInfo;

            hr = m_sc.StartAt(123, 456);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sc.GetInfo(out pInfo);
            Debug.Assert(pInfo.tStart == 123 && pInfo.dwStartCookie == 456, "start");
        }

        private void TestStop()
        {
            int hr;
            AMStreamInfo pInfo;

            hr = m_sc.StopAt(321, true, 654);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sc.GetInfo(out pInfo);
            Debug.Assert(pInfo.tStop == 321 && pInfo.dwStopCookie == 654, "stop");
        }

        private void TestInfo()
        {
            int hr;
            AMStreamInfo pInfo;

            hr = m_sc.GetInfo(out pInfo);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pInfo.dwFlags == (AMStreamInfoFlags.StopSendExtra | AMStreamInfoFlags.StartDefined | AMStreamInfoFlags.StopDefined | AMStreamInfoFlags.Discarding), "Info");
        }

        private void Configure()
        {
            int hr;
            m_FilterGraph = (IFilterGraph2)new FilterGraph();

            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            Guid iid = typeof(IBaseFilter).GUID;
            object o;
            devs[0].Mon.BindToObject(null, null, ref iid, out o);

            IBaseFilter dev = (IBaseFilter)o;
            hr = m_FilterGraph.AddFilter(dev, "device");
            DsError.ThrowExceptionForHR(hr);

            IBaseFilter mux = (IBaseFilter)new AviDest();

            hr = m_FilterGraph.AddFilter(mux, "mux");
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, dev, null, mux);
            DsError.ThrowExceptionForHR(hr);

            IPin pPin = DsFindPin.ByDirection(mux, PinDirection.Input, 0);
            m_sc = (IAMStreamControl)pPin;

            Marshal.ReleaseComObject(icgb);
            Marshal.ReleaseComObject(mux);
            Marshal.ReleaseComObject(dev);
        }
    }
}
