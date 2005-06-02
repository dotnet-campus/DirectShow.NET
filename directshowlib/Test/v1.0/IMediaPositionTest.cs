// CanSeekForward -> OABool
// CanSeekBackward -> OABool

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IMediaPositionTest
    {
        DsROTEntry m_ROT = null;
        IMediaPosition m_mediaPosition = null;
        IMediaControl m_imc = null;

        public IMediaPositionTest()
        {
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            BuildGraph();

            TestCanSeek();
            TestDuration();
            TestPosition();
            TestRate();
            TestStopTime();
            TestPrerollTime();
        }

        void TestCanSeek()
        {
            int hr;
            OABool b, f;

            hr = m_mediaPosition.CanSeekBackward(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == OABool.True, "CanSeekBackward");

            hr = m_mediaPosition.CanSeekForward(out f);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(f == OABool.True, "CanSeekForward");
        }

        void TestDuration()
        {
            int hr;
            double len;

            hr = m_mediaPosition.get_Duration(out len);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(len > 4.31, "Get_Duration");
        }

        void TestPosition()
        {
            int hr;

            double pllTime;

            Thread.Sleep(1500);

            hr = m_mediaPosition.get_CurrentPosition(out pllTime);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pllTime > 1.0, "get_CurrentPosition");

            hr = m_mediaPosition.put_CurrentPosition(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaPosition.get_CurrentPosition(out pllTime);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pllTime < 1.0, "get_CurrentPosition");
        }

        void TestRate()
        {
            int hr;
            double rate;

            hr = m_mediaPosition.get_Rate(out rate);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(rate > 0 && rate < 2, "get_Rate");

            hr = m_mediaPosition.put_Rate(2.0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaPosition.get_Rate(out rate);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(rate > 1 && rate < 3, "get_Rate");

            hr = m_mediaPosition.put_Rate(1.0);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestStopTime()
        {
            int hr;
            double st;

            hr = m_mediaPosition.get_StopTime(out st);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(st > 4.3 && st < 4.33, "get_StopTime");

            hr = m_mediaPosition.put_StopTime(4.0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaPosition.get_StopTime(out st);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(st > 3.9 && st < 4.1, "get_StopTime");

            hr = m_mediaPosition.put_StopTime(6.0);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestPrerollTime()
        {
            int hr;
            double pr;

            hr = m_mediaPosition.get_PrerollTime(out pr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pr == 0, "get_PrerollTime");

            hr = m_mediaPosition.put_PrerollTime(4.0);

            // E_NOTIMPL - Nothing to test
            if (hr != -2147467263)
            {
                DsError.ThrowExceptionForHR(hr);

                hr = m_mediaPosition.get_PrerollTime(out pr);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pr > 3.9 && pr < 4.1, "get_PrerollTime");

                hr = m_mediaPosition.put_PrerollTime(6.0);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        void BuildGraph()
        {
            int hr;
            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;

            m_ROT = new DsROTEntry(graphBuilder);
            IFilterGraph2 ifg2 = graphBuilder as IFilterGraph2;

            hr = graphBuilder.RenderFile("foo.avi", null);
            DsError.ThrowExceptionForHR(hr);

            // Get a ICaptureGraphBuilder2
            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
            hr = icgb.SetFiltergraph( (IGraphBuilder)graphBuilder );
            DsError.ThrowExceptionForHR(hr);

            m_imc = graphBuilder as IMediaControl;
            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);

            m_mediaPosition = graphBuilder as IMediaPosition;
        }
    }
}