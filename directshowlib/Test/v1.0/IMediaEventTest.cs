// WaitForCompletion -> EventCode
// CancelDefaultHandling -> EventCode
// RestoreDefaultHandling -> EventCode

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    public class IMediaEventTest
    {
        DsROTEntry m_ROT = null;
        IMediaEvent m_mediaEvent = null;
        IMediaControl m_imc = null;

        public IMediaEventTest()
        {
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        public void DoTests()
        {
            int hr;
            IntPtr hEvent;
            IntPtr p1, p2;
            EventCode ec;

            BuildGraph();

            hr = m_mediaEvent.GetEventHandle(out hEvent);
            DsError.ThrowExceptionForHR(hr);

            ManualResetEvent mre = new ManualResetEvent(false);
            mre.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(hEvent, true);

            // Should get an event before this
            bool b = mre.WaitOne(5000, true);
            Debug.Assert(b, "GetEventHandle");

            // I don't know what event I may get, so I don't know how to check it

            hr = m_mediaEvent.GetEvent(out ec, out p1, out p2, 0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaEvent.FreeEventParams(ec, p1, p2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaEvent.CancelDefaultHandling(EventCode.Repaint);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaEvent.RestoreDefaultHandling(EventCode.Repaint);
            DsError.ThrowExceptionForHR(hr);

            // The clip is 4 seconds long, so timeout in 5
            hr = m_mediaEvent.WaitForCompletion(5000, out ec);
            DsError.ThrowExceptionForHR(hr);

            // The video should have successfully played
            Debug.Assert(ec == EventCode.Complete, "WaitForCompletion");
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

            m_mediaEvent = graphBuilder as IMediaEvent;
        }
    }
}