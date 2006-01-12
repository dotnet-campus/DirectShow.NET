using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IPinConnectionTest
	{
        private IPinConnection m_pc;
        private IMediaControl m_mc;

		public IPinConnectionTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestIsEnd();
                TestNotifyEndOfStream();
                TestDynamicQueryAccept();
                TestDynamicDisconnect();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pc);
            }
        }

        private void TestDynamicDisconnect()
        {
            int hr;

            hr = m_pc.DynamicDisconnect();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestDynamicQueryAccept()
        {
            int hr;

            IPin iPin = m_pc as IPin;
            AMMediaType pmt = new AMMediaType();

            hr = iPin.ConnectionMediaType(pmt);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pc.DynamicQueryAccept(pmt);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestNotifyEndOfStream()
        {
            int hr;
            IPin iPin = m_pc as IPin;
            System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);
            
            hr = m_pc.NotifyEndOfStream(mre.Handle);
            DsError.ThrowExceptionForHR(hr);

            hr = iPin.EndOfStream();
            DsError.ThrowExceptionForHR(hr);

            mre.WaitOne();
            mre.Close();

            // If NotifyEndOfStream wasn't working, we'd never get here.

            hr = m_pc.NotifyEndOfStream(IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestIsEnd()
        {
            int hr;

            hr = m_pc.IsEndPin();

            Debug.Assert(hr == 0, "IsEnd");
        }

        private void Config()
        {
            IBaseFilter pFilter;
            IGraphBuilder gb = (IGraphBuilder)new FilterGraph();
            m_mc = (IMediaControl)gb;

            int hr = gb.RenderFile(@"foo.avi", null);
            hr = gb.FindFilterByName("Video Renderer", out pFilter);
            IPin iPin = DsFindPin.ByDirection(pFilter, PinDirection.Input, 0);

            m_pc = iPin as IPinConnection;

        }
	}
}
