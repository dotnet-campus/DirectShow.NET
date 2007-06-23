using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IMediaEventSinkTest
    {
        internal const EventCode EC_FileComplete = (EventCode)0x8000;

        IMediaEventSink m_mes;
        IFilterGraph2 m_FilterGraph;

        public IMediaEventSinkTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestNotify();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestNotify()
        {
            int hr;
            IntPtr p1, p2;
            IntPtr p3 = IntPtr.Zero, p4 = IntPtr.Zero;
            EventCode ec;
            bool bPassed = false;
            bool bDone = false;

            IMediaEvent pEvent = (IMediaEvent)m_FilterGraph;

            hr = m_mes.Notify(EC_FileComplete, new IntPtr(1), new IntPtr(2));
            DsError.ThrowExceptionForHR(hr);

            ((IMediaControl)m_FilterGraph).Run();

            // Read the event
            for (
                hr = pEvent.GetEvent(out ec, out p1, out p2, 5000);
                hr >= 0 && !bDone;
                hr = pEvent.GetEvent(out ec, out p1, out p2, 5000)
                )
            {
                Debug.WriteLine(ec);

                if (ec == EC_FileComplete)
                {
                    bPassed = true;
                    p3 = p1;
                    p4 = p2;
                }
                else if (ec == EventCode.Complete)
                {
                    break;
                }
                else
                {
                    hr = pEvent.FreeEventParams(ec, p1, p2);
                }
            }

            Debug.Assert(bPassed && p3.ToInt64() == 1 && p4.ToInt64() == 2, "Notify");
        }

        private void Configure()
        {
            int hr;
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry rot = new DsROTEntry(m_FilterGraph);

            hr = m_FilterGraph.RenderFile("foo.avi", null);
            DsError.ThrowExceptionForHR(hr);

            m_mes = (IMediaEventSink)m_FilterGraph;
        }
    }
}
