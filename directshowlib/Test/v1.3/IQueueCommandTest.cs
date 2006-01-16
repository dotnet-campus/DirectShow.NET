using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using Microsoft.Win32;

namespace DirectShowLib.Test
{
	public class IQueueCommandTest
	{
        private IQueueCommand m_qc;
        private IMediaControl m_mc;

		public IQueueCommandTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestStream();
                Marshal.ReleaseComObject(m_mc);

                Config();
                TestPresentation();
            }
            finally
            {
                Marshal.ReleaseComObject(m_qc);
                Marshal.ReleaseComObject(m_mc);
            }
        }

        private void TestPresentation()
        {
            int hr;
            int hr2;
            IDeferredCommand defc;
            short e;
            object o1 = 3;
            int p;
            GCHandle gch;
            gch = GCHandle.Alloc(o1, GCHandleType.Pinned);

            hr = m_qc.InvokeAtPresentationTime(
                out defc,
                3,
                typeof(IMediaControl).GUID,
                0x60020002,
                DispatchFlags.Method,
                0,
                null,
                gch.AddrOfPinnedObject(),
                out e);
            DsError.ThrowExceptionForHR(hr);

            hr = defc.Confidence(out p);
            Debug.Assert(hr == -2147467263, "Confidence not implemented");

            hr = defc.Postpone(2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mc.Run();
            DsError.ThrowExceptionForHR(hr);

            for (int x=0; x < 5000; x++)
            {
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }

            hr = defc.GetHResult(out hr2);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(hr2 == 0, "GetHResult");

            gch.Free();
            Debug.Assert((int)o1 == 0);
        }

        private void TestStream()
        {
            int hr;
            int hr2;
            IDeferredCommand defc;
            short e;
            int p;

            hr = m_qc.InvokeAtStreamTime(
                out defc,
                3,
                typeof(IMediaControl).GUID,
                0x60020002,
                DispatchFlags.Method,
                0,
                null,
                IntPtr.Zero,
                out e);
            DsError.ThrowExceptionForHR(hr);

            hr = defc.Postpone(200);
            DsError.ThrowExceptionForHR(hr);

            hr = m_mc.Run();
            DsError.ThrowExceptionForHR(hr);

            for (int x=0; x < 5000; x++)
            {
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }

            hr = defc.GetHResult(out hr2);
            DsError.ThrowExceptionForHR(hr2);

            // Operation aborted - since the execution time is 
            // past the end of the graph, this makes sense.
            Debug.Assert(hr == -2147467260, "GetHResult");
        }

        private void Config()
        {
            FilterGraph fg = new FilterGraph();

            m_qc = fg as IQueueCommand;
            m_mc = (IMediaControl)fg;

            int hr = ((IGraphBuilder)fg).RenderFile(@"foo.avi", null);
        }
	}
}
