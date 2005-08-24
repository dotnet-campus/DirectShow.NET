using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IAMStatsTest
    {
        IAMStats m_stat;
        int m_Index;
        IFilterGraph2 m_FilterGraph;

        public IAMStatsTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestGetCount();
                TestByName();
                TestByIndex();
                TestGetIndex();
                TestAddValue();
                TestReset();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_stat);
            }
        }

        private void TestAddValue()
        {
            int hr;
            int pIndex;
            string nam;
            int iCount;
            double dLast = 0, dAverage = 0, dStd = 0, dMin = 0, dMax = 0;

            hr = m_stat.GetIndex("TestAdd", true, out pIndex);
            DsError.ThrowExceptionForHR(hr);

            hr = m_stat.AddValue(pIndex, 3.1415927);
            DsError.ThrowExceptionForHR(hr);

            hr = m_stat.GetValueByIndex(pIndex, out nam, out iCount, out dLast, out dAverage, out dStd, out dMin, out dMax);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iCount == 1 && dLast == 3.1415927, "AddValue");
        }

        private void TestGetCount()
        {
            int hr;
            int pCount;

            hr = m_stat.get_Count(out pCount);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pCount > 0, "get_Count");
        }

        private void TestByName()
        {
            int hr;
            int iCount;
            double dLast, dAverage, dStd, dMin, dMax;

            hr = m_stat.GetValueByName("Create Filter", out m_Index, out iCount, out dLast, out dAverage, out dStd, out dMin, out dMax);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iCount > 0, "GetValueByName");
        }

        private void TestByIndex()
        {
            int hr;
            string nam;
            int iCount;
            double dLast, dAverage, dStd, dMin, dMax;

            hr = m_stat.GetValueByIndex(m_Index, out nam, out iCount, out dLast, out dAverage, out dStd, out dMin, out dMax);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(nam == "Create Filter", "GetValueByIndex");
        }

        private void TestGetIndex()
        {
            int hr;
            int pIndex;

            hr = m_stat.GetIndex("Create Filter", false, out pIndex);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pIndex == m_Index, "GetIndex");

            hr = m_stat.GetIndex("Create Filterx", true, out pIndex);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestReset()
        {
            int hr;
            string nam;
            int iCount;
            double dLast, dAverage, dStd, dMin, dMax;

            hr = m_stat.Reset();
            DsError.ThrowExceptionForHR(hr);

            hr = m_stat.GetValueByIndex(m_Index, out nam, out iCount, out dLast, out dAverage, out dStd, out dMin, out dMax);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iCount == 0, "Reset");
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            IBaseFilter pFilter;
            IBaseFilter pRender = (IBaseFilter)new VideoRendererDefault();

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = m_FilterGraph.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.AddFilter(pRender, "renderererer");
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, pFilter, null, pRender);
            DsError.ThrowExceptionForHR(hr);

            m_stat = (IAMStats)m_FilterGraph;
            //((IMediaControl)m_FilterGraph).Run();
            System.Threading.Thread.Sleep(1000);

            Marshal.ReleaseComObject(icgb);
        }
    }
}
