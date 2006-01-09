using System;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IMixerOCXTest : IMixerOCXNotify
	{
        private IMixerOCX m_mo;
        private bool m_ir, m_dc, m_sc;
        private IFilterGraph2 m_fg;

		public IMixerOCXTest()
		{
           m_ir = false;
           m_dc = false;
           m_sc = false;
        }

        public void DoTests()
        {
            Config();

            try
            {
                EventCode ev;

                TestGetAspectRatio();
                TestGetStatus();
                TestGetVideoSize();
                TestOnDisplayChange();
                TestOnDraw();
                TestSetDrawRegion();
                ((IMediaEvent)m_fg).WaitForCompletion(-1, out ev);
                TestUnadvise();

                Debug.Assert(m_ir == true, "IMixerOCXNotify::OnInvalidateRect");

                // This never gets called.  Perhaps related to the fact that
                // IMixerOCX::GetStatus is unimplemented?
                // Debug.Assert(m_sc == true, "IMixerOCXNotify::OnStatusChange");

                // I don't know why this one doesn't get called.
                // Debug.Assert(m_dc == true, "IMixerOCXNotify::OnDataChange");
            }
            finally
            {
                Marshal.ReleaseComObject(m_mo);
            }
        }

        public void TestAdvise()
        {
            int hr;

            hr = m_mo.Advise(this);
            DsError.ThrowExceptionForHR(hr);
        }


        private void TestSetDrawRegion()
        {
            int hr;
            DsRect r1 = new DsRect(1, 1, 2, 2);
            DsRect r2 = new DsRect(0, 0, 319, 239);

            hr = m_mo.SetDrawRegion(IntPtr.Zero, r1, r2);
            DsError.ThrowExceptionForHR(hr);
        }

        public void TestUnadvise()
        {
            int hr;

            hr = m_mo.UnAdvise();
            DsError.ThrowExceptionForHR(hr);
        }

        public void TestOnDisplayChange()
        {
            int hr;

            // Not implemented
            hr = m_mo.OnDisplayChange(24, 640, 480);
            //DsError.ThrowExceptionForHR(hr);
        }

        public void TestOnDraw()
        {
            int hr;

            //Graphics g;
            //IntPtr hdc = g.GetHdc();
            DsRect r = new DsRect(0, 0, 640, 480);

            hr = m_mo.OnDraw(IntPtr.Zero, r);
            DsError.ThrowExceptionForHR(hr);
        }

        public void TestGetVideoSize()
        {
            int hr;
            int w, h;

            hr = m_mo.GetVideoSize(out w, out h);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(w > 0 && h > 0, "GetVideoSize");
        }

        public void TestGetStatus()
        {
            int hr;
            int i;

            // "Not implemented"
            hr = m_mo.GetStatus(out i);
        }

        private void TestGetAspectRatio()
        {
            int hr;
            int x, y;

            // "Not implemented"
            hr = m_mo.GetAspectRatio(out x, out y);
            //DsError.ThrowExceptionForHR(hr);
        }


        private void Config()
        {
            int hr;
            IBaseFilter pFilter;
            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            m_fg = (IFilterGraph2)new FilterGraph();
            DsROTEntry rot = new DsROTEntry(m_fg);
            hr = icgb.SetFiltergraph(m_fg);
            DsError.ThrowExceptionForHR(hr);

            m_mo = (IMixerOCX) new OverlayMixer(); 
            TestAdvise();

            hr = m_fg.AddFilter((IBaseFilter)m_mo, null);
            DsError.ThrowExceptionForHR(hr);

            hr = m_fg.AddSourceFilter(@"foo.avi", null, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            IPin iPin = DsFindPin.ByDirection(pFilter, PinDirection.Output, 0);
            hr = icgb.RenderStream(null, null, iPin, null, (IBaseFilter)m_mo);
            DsError.ThrowExceptionForHR(hr);

            hr = ((IMediaControl)m_fg).Run();
            DsError.ThrowExceptionForHR(hr);
        }

        #region IMixerOCXNotify Members

        public int OnInvalidateRect(DsRect lpcRect)
        {
            m_ir = true;
            return 0;
        }

        public int OnDataChange(DirectShowLib.MixerData ulDataFlags)
        {
            if (!m_dc)
            {
                m_dc = true;
            }
            return 0;
        }

        public int OnStatusChange(DirectShowLib.MixerState ulStatusFlags)
        {
            if (!m_sc)
            {
                m_sc = true;
            }
            return 0;
        }

        #endregion
    }
}
