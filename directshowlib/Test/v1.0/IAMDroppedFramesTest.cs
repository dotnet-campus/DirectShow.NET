using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IAMDroppedFramesTest
    {
        const int E_PROP_ID_UNSUPPORTED = unchecked((int)0x80070490);

        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null;
        IAMDroppedFrames m_idf = null;

        public IAMDroppedFramesTest()
        {
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            BuildGraph();

            try
            {
                TestAverage();
                TestDropped();
                TestNumDropped();
                TestNumNotDropped();
            }
            finally
            {
                if (m_imc != null)
                {
                    m_imc.Stop();
                    Marshal.ReleaseComObject(m_imc);
                    m_imc = null;
                }
            }
        }

        void TestAverage()
        {
            int hr;
            int ave;

            hr = m_idf.GetAverageFrameSize(out ave);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(ave > 0, "GetAverageFrameSize");
        }

        void TestDropped()
        {
            int hr;
            int cop;
            int[] di = new int[16];

            hr = m_idf.GetDroppedInfo(di.Length, out di, out cop);

            // Not supported by this device
            if (hr != E_PROP_ID_UNSUPPORTED)
            {
                DsError.ThrowExceptionForHR(hr);
            }
        }

        void TestNumDropped()
        {
            int hr;
            int drop;

            hr = m_idf.GetNumDropped(out drop);
            DsError.ThrowExceptionForHR(hr);

            // Not really much we can test since we may not have dropped anything
            Debug.Assert(drop >= 0, "GetNumDropped");
        }

        void TestNumNotDropped()
        {
            int hr;
            int notdrop;

            hr = m_idf.GetNumNotDropped(out notdrop);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(notdrop > 0, "GetNumNotDropped");
        }

        void BuildGraph()
        {
            int hr;
            IBaseFilter ppFilter;
            DsDevice [] devs;
            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;

            m_ROT = new DsROTEntry(graphBuilder);
            IFilterGraph2 ifg2 = graphBuilder as IFilterGraph2;

            devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = devs[0];

            hr = ifg2.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out ppFilter);
            DsError.ThrowExceptionForHR(hr);

            m_idf = ppFilter as IAMDroppedFrames;

            IPin IPinOut = DsFindPin.ByDirection(ppFilter, PinDirection.Output, 0);

            hr = ifg2.Render(IPinOut);
            DsError.ThrowExceptionForHR(hr);

            m_imc = graphBuilder as IMediaControl;
            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);
        }
    }
}