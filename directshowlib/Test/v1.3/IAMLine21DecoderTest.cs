using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMLine21DecoderTest
	{
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);

        private IAMLine21Decoder m_ild;

		public IAMLine21DecoderTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestDecoderLevel();
                TestBack();
                TestService();
                TestMode();
                TestFormat();
                TestRedraw();
                TestState();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ild);
            }
        }

        private void TestDecoderLevel()
        {
            int hr;
            AMLine21CCLevel l;

            hr = m_ild.GetDecoderLevel(out l);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l == AMLine21CCLevel.TC2, "GetDecoderLevel");
        }

        private void TestBack()
        {
            int hr;
            int i;

            hr = m_ild.SetBackgroundColor(1234);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ild.GetBackgroundColor(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1234, "BackgroundColor");
        }

        private void TestService()
        {
            int hr;
            AMLine21CCService s;

            hr = m_ild.SetCurrentService(AMLine21CCService.Caption2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ild.GetCurrentService(out s);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == AMLine21CCService.Caption2, "CurrentService");
        }

        private void TestMode()
        {
            int hr;
            AMLine21DrawBGMode m;

            hr = m_ild.SetDrawBackgroundMode(AMLine21DrawBGMode.Opaque);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ild.GetDrawBackgroundMode(out m);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m == AMLine21DrawBGMode.Opaque, "DrawBackgroundMode");
        }

        private void TestFormat()
        {
            int hr;
            BitmapInfoHeader b = new BitmapInfoHeader();

            hr = m_ild.GetOutputFormat(b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ild.SetOutputFormat(b);
            Debug.Assert(E_NOTIMPL == hr, "SetOutputFormat");
        }

        private void TestRedraw()
        {
            int hr;
            bool r;

            hr = m_ild.SetRedrawAlways(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ild.GetRedrawAlways(out r);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(r == true, "RedrawAlways");
        }

        private void TestState()
        {
            int hr;
            AMLine21CCState s;

            hr = m_ild.SetServiceState(AMLine21CCState.Off);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ild.GetServiceState(out s);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == AMLine21CCState.Off, "ServiceState");
        }

        private void Config()
        {
            m_ild = (IAMLine21Decoder)new Line21Decoder();
        }
	}
}
