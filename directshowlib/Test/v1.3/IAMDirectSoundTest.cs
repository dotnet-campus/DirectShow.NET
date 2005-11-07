using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMDirectSoundTest
	{
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);

        private IAMDirectSound m_ids;

		public IAMDirectSoundTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestUnimplemented();
                TestFocus();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ids);
            }
        }

        private void TestUnimplemented()
        {
            int hr;
            object o;

            hr = m_ids.GetDirectSoundInterface(out o);
            Debug.Assert(hr == E_NOTIMPL, "GetDirectSoundInterface");

            hr = m_ids.GetPrimaryBufferInterface(out o);
            Debug.Assert(hr == E_NOTIMPL, "GetPrimaryBufferInterface");

            hr = m_ids.GetSecondaryBufferInterface(out o);
            Debug.Assert(hr == E_NOTIMPL, "GetSecondaryBufferInterface");

            hr = m_ids.ReleaseDirectSoundInterface(null);
            Debug.Assert(hr == E_NOTIMPL, "ReleaseDirectSoundInterface");

            hr = m_ids.ReleasePrimaryBufferInterface(null);
            Debug.Assert(hr == E_NOTIMPL, "ReleasePrimaryBufferInterface");

            hr = m_ids.ReleaseSecondaryBufferInterface(null);
            Debug.Assert(hr == E_NOTIMPL, "ReleaseSecondaryBufferInterface");
        }

        private void TestFocus()
        {
            int hr;
            bool b, b2;
            IntPtr ip;

            hr = m_ids.GetFocusWindow(out ip, out b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ids.SetFocusWindow(ip, !b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ids.GetFocusWindow(out ip, out b2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == !b2, "GetFocusWindow");
        }

        private void Config()
        {
            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.AudioRendererCategory);

            for (int x=0; x < devs.Length; x++)
            {
                if (devs[x].Name == "Default DirectSound Device")
                {
                    object o;
                    Guid iid = typeof(IBaseFilter).GUID;

                    devs[x].Mon.BindToObject(null, null, ref iid, out o);
                    m_ids = (IAMDirectSound) o; 
                    break;
                }
            }
        }
	}
}
