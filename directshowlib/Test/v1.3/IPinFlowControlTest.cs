using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IPinFlowControlTest
    {
        IFilterGraph2 m_FilterGraph;
        IBaseFilter m_pNero;

        public IPinFlowControlTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestBlock();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }


        private void TestBlock()
        {
            int hr;

            hr = m_FilterGraph.AddFilter(m_pNero, "Nero");
            DsError.ThrowExceptionForHR(hr);

            hr = ((IGraphBuilder)m_FilterGraph).RenderFile("foo.avi", null);
            DsError.ThrowExceptionForHR(hr);

            IPin pPin2 = DsFindPin.ByDirection(m_pNero, PinDirection.Output, 0);

            IPinFlowControl pfc = pPin2 as IPinFlowControl;

            hr = pfc.Block(AMPinFlowControl.Block, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
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

        private void Configure()
        {
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            m_pNero = FindNero();
        }
    }
}
