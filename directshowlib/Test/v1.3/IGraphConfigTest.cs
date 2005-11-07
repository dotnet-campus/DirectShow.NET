using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IGraphConfigTest : IGraphConfigCallback
    {
        IGraphConfig m_igc;
        IFilterGraph2 m_FilterGraph;
        IBaseFilter m_ibf;
        IBaseFilter m_pNero;

        public IGraphConfigTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestFlags();
                TestCache();
                TestRemove();
                TestStartTime();
                TestPush();
                TestReconfig();
                TestReconnect();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_igc);
            }
        }


        private void TestCache()
        {
            int hr;
            int f;
            IEnumFilters pEnum;
            IBaseFilter [] ibf = new IBaseFilter[1];

            hr = m_igc.EnumCacheFilter(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum.Next(1, ibf, out f) == 1, "EnumCacheFilter1");

            hr = m_igc.AddFilterToCache(m_ibf);
            DsError.ThrowExceptionForHR(hr);

            hr = m_igc.EnumCacheFilter(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum.Next(1, ibf, out f) == 0, "EnumCacheFilter2");

            Debug.Assert(ibf[0] == m_ibf, "AddFilterToCache");

            hr = m_igc.RemoveFilterFromCache(m_ibf);
            DsError.ThrowExceptionForHR(hr);

            hr = m_igc.EnumCacheFilter(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum.Next(1, ibf, out f) == 1, "EnumCacheFilter3");
        }

        private void TestRemove()
        {
            int hr;

            hr = m_igc.RemoveFilterEx(m_ibf, RemFilterFlags.LeaveConnected);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestStartTime()
        {
            int hr;
            long l1, l2;

            hr = ((IMediaControl)m_FilterGraph).Run();
            DsError.ThrowExceptionForHR(hr);

            hr = m_igc.GetStartTime(out l1);
            DsError.ThrowExceptionForHR(hr);

            System.Threading.Thread.Sleep(3234);

            hr = ((IMediaControl)m_FilterGraph).Pause();
            DsError.ThrowExceptionForHR(hr);

            hr = ((IMediaControl)m_FilterGraph).Run();
            DsError.ThrowExceptionForHR(hr);

            hr = m_igc.GetStartTime(out l2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l2 > l1, "GetStartTime");
        }

        private void TestFlags()
        {
            int hr;
            AMFilterFlags f;

            hr = m_FilterGraph.AddFilter(m_ibf, "SG");
            DsError.ThrowExceptionForHR(hr);

            hr = m_igc.GetFilterFlags(m_ibf, out f);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(f == AMFilterFlags.None, "GetFilterFlags1");

            hr = m_igc.SetFilterFlags(m_ibf, AMFilterFlags.Removable);
            DsError.ThrowExceptionForHR(hr);

            hr = m_igc.GetFilterFlags(m_ibf, out f);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(f == AMFilterFlags.Removable, "GetFilterFlags2");
        }

        private IBaseFilter FindNero()
        {
            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.LegacyAmFilterCategory);
            IBaseFilter ibf = null;

            for (int x=0; x < devs.Length; x++)
            {
                if (devs[x].Name == "Nero Digital Audio Decoder")
                {
                    object o;
                    Guid iid = typeof(IBaseFilter).GUID;
                    devs[x].Mon.BindToObject(null, null, ref iid, out o);
                    ibf = o as IBaseFilter;
                    break;
                }
            }

            return ibf;
        }

        private void TestPush()
        {
            int hr;
            DsROTEntry rot = new DsROTEntry(m_FilterGraph);

            hr = m_FilterGraph.AddFilter(m_pNero, "Nero");
            DsError.ThrowExceptionForHR(hr);

            hr = ((IGraphBuilder)m_FilterGraph).RenderFile("foo.avi", null);
            DsError.ThrowExceptionForHR(hr);

            IPin pPin2 = DsFindPin.ByDirection(m_pNero, PinDirection.Output, 0);

            IPinFlowControl pfc = pPin2 as IPinFlowControl;

            hr = m_igc.PushThroughData(pPin2, null, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            rot.Dispose();
        }

        private void TestReconfig()
        {
            int hr;

            hr = m_igc.Reconfigure(this, (IntPtr)7, 8, IntPtr.Zero);
            Debug.Assert(hr == 3, "Reconfigure");
        }

        private void TestReconnect()
        {
            int hr;
            IPin pPin2 = DsFindPin.ByDirection(m_pNero, PinDirection.Output, 0);

            hr = m_igc.Reconnect(pPin2, null, null, null, IntPtr.Zero, AMGraphConfigReconnect.None);
            DsError.ThrowExceptionForHR(hr);
        }


        private void Configure()
        {
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            m_pNero = FindNero();

            m_igc = (IGraphConfig)m_FilterGraph;
            m_ibf = (IBaseFilter)new SampleGrabber();
        }
        #region IGraphConfigCallback Members

        public int Reconfigure(System.IntPtr pvContext, int dwFlags)
        {
            Debug.Assert(pvContext.ToInt32() == 7, "IGraphConfigCallback1");
            Debug.Assert(dwFlags == 8, "IGraphConfigCallback2");

            return 3;
        }

        #endregion
    }
}
