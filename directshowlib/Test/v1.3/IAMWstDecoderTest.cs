using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMWstDecoderTest
	{
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);

        private IAMWstDecoder m_iwd;

		public IAMWstDecoderTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                //TestService();
                //TestLevel();
                //TestAnswerMode();
                //TestBack();
                TestDraw();
                TestHold();
                TestOutput();
                TestRedraw();
                TestState();
                TestPage();
            }
            finally
            {
                Marshal.ReleaseComObject(m_iwd);
            }
        }

        private void TestService()
        {
            int hr;
            WSTService w;

            hr = m_iwd.GetCurrentService(out w);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(w == WSTService.Text, "GetCurrentService");
        }

        private void TestLevel()
        {
            int hr;
            WSTLevel w;

            hr = m_iwd.GetDecoderLevel(out w);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(w == WSTLevel.Level1_5, "GetDecoderLevel");
        }

        private void TestAnswerMode()
        {
            int hr;
            bool b;

            hr = m_iwd.SetAnswerMode(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetAnswerMode(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "AnswerMode");
        }

        private void TestBack()
        {
            int hr;
            int i;

            hr = m_iwd.SetBackgroundColor(12345678);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetBackgroundColor(out i);
            DsError.ThrowExceptionForHR(hr);

            // GetBackgroundColor always returns zero
            //Debug.Assert(i == 1234, "BackgroundColor");
        }

        private void TestPage()
        {
            int hr;
            WSTPage w = new WSTPage();
            w.dwPageNr = 256;
            w.dwSubPageNr = 0;
            w.pucPageData = Marshal.AllocCoTaskMem(100000);

            hr = m_iwd.SetCurrentPage(w);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetCurrentPage(w);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestDraw()
        {
            int hr;
            WSTDrawBGMode l;

            hr = m_iwd.SetDrawBackgroundMode(WSTDrawBGMode.Opaque);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetDrawBackgroundMode(out l);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l == WSTDrawBGMode.Opaque, "DrawBackgroundMode");
        }

        private void TestHold()
        {
            int hr;
            bool b;

            hr = m_iwd.SetHoldPage(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetHoldPage(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b, "GetHoldPage");
        }

        private void TestOutput()
        {
            int hr;
            BitmapInfoHeader b = new BitmapInfoHeader();

            hr = m_iwd.GetOutputFormat(b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.SetOutputFormat(b);
            Debug.Assert(E_NOTIMPL == hr, "SetOutputFormat");
        }

        private void TestRedraw()
        {
            int hr;
            bool r;

            hr = m_iwd.SetRedrawAlways(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetRedrawAlways(out r);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(r == true, "RedrawAlways");
        }

        private void TestState()
        {
            int hr;
            WSTState w;

            hr = m_iwd.SetServiceState(WSTState.Off);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iwd.GetServiceState(out w);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(w == WSTState.Off, "ServiceState");
        }

        private void Config()
        {
            m_iwd = (IAMWstDecoder)new WSTDecoder();
        }
	}
}
