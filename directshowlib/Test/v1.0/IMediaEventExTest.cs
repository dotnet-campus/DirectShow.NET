// WaitForCompletion -> EventCode
// CancelDefaultHandling -> EventCode
// RestoreDefaultHandling -> EventCode
// SetNotifyFlags -> NotifyFlags

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IMediaEventExTest : Form
    {
        const int WM_GRAPHNOTIFY = 0x8000 + 1;

        DsROTEntry m_ROT = null;
        IMediaEventEx m_mediaEventEx = null;
        IMediaControl m_imc = null;
        bool m_bCalled = false;

        public IMediaEventExTest()
        {
            // Set the window properties.  The window isn't displayed, but does receive events.
            this.Text = "IMediaEventExTest window";
            this.ClientSize = new Size(800, 450); // 16/9 aspect ratio to see colored borders
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        // Media events are sent to use as windows messages.
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                    // If this is a windows media message
                case WM_GRAPHNOTIFY:
                    m_bCalled = true;
                    break;

                    // All other messages
                default:
                    // unhandled window message
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            BuildGraph();

            //TestNotifyFlags();
            TestNotifyWindow();
        }

        void TestNotifyFlags()
        {
            int hr;
            NotifyFlags nf;

            hr = m_mediaEventEx.GetNotifyFlags(out nf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(nf == NotifyFlags.None, "GetNotifyFlags");

            hr = m_mediaEventEx.SetNotifyFlags(NotifyFlags.NoNotify);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mediaEventEx.GetNotifyFlags(out nf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(nf == NotifyFlags.NoNotify, "GetNotifyFlags");

            hr = m_mediaEventEx.SetNotifyFlags(NotifyFlags.None);
            DsError.ThrowExceptionForHR(hr);
        }


        void TestNotifyWindow()
        {
            int hr;
            EventCode ec;

            // Allow windows messages to be processed
            do
            {
                Application.DoEvents();
                hr = m_mediaEventEx.WaitForCompletion(100, out ec);
            } while (hr == -2147467260); // timeout errors

            DsError.ThrowExceptionForHR(hr);

            // m_bCalled should get set in the WndProc routine
            Debug.Assert(m_bCalled, "SetNotifyWindow");
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

            m_mediaEventEx = graphBuilder as IMediaEventEx;
            hr = m_mediaEventEx.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            m_imc = graphBuilder as IMediaControl;
            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);


        }
    }
}