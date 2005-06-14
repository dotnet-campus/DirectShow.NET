using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Drawing;

namespace DirectShowLib.Test
{
    internal class MyNotification : IAMTunerNotification
    {
        public int OnEvent(AMTunerEventType iEvent)
        {
            Debug.Assert(false, "This interfaces never gets called");
            return 0;
        }
    }

    [TestFixture]
    public class IAMTVAudioTest
    {
        IAMTVAudio m_itva = null;

        public IAMTVAudioTest()
        {
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            BuildGraph();

            try
            {
                TestAudioMode();
                TestModes();
                TestGetHW();
                TestCallBack();
            }
            finally
            {
            }
        }

        void TestAudioMode()
        {
            int hr;
            TVAudioMode pmode;

            hr = m_itva.get_TVAudioMode(out pmode);
            DsError.ThrowExceptionForHR(hr);

            hr = m_itva.put_TVAudioMode(TVAudioMode.LangA);
            DsError.ThrowExceptionForHR(hr);

            hr = m_itva.get_TVAudioMode(out pmode);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pmode == TVAudioMode.LangA, "Get/Set TVAudioMode");
        }

        void TestModes()
        {
            int hr;
            TVAudioMode modes;

            hr = m_itva.GetAvailableTVAudioModes(out modes);
            DsError.ThrowExceptionForHR(hr);

            // my card returns 17
            Debug.Assert(modes > 0, "GetAvailableTVAudioModes");
        }

        void TestGetHW()
        {
            int hr;
            TVAudioMode modes;

            hr = m_itva.GetHardwareSupportedTVAudioModes(out modes);
            DsError.ThrowExceptionForHR(hr);

            // my card returns 17
            Debug.Assert(modes > 0, "GetAvailableTVAudioModes");
        }

        void TestCallBack()
        {
            int hr;
            const int E_NOTIMPLEMENTED = unchecked((int)0x80004001);
            MyNotification myn = new MyNotification();

            // This function is documened to return E_NOTIMPLEMENTED
            hr = m_itva.RegisterNotificationCallBack(myn, AMTVAudioEventType.Changed);
            Debug.Assert(hr == E_NOTIMPLEMENTED, "RegisterNotification");

            // This function is documened to return E_NOTIMPLEMENTED
            hr = m_itva.UnRegisterNotificationCallBack(myn);
            Debug.Assert(hr == E_NOTIMPLEMENTED, "RegisterNotification");
        }

        void BuildGraph()
        {
            DsDevice [] devs;
            string s;

            devs = DsDevice.GetDevicesOfCat(FilterCategory.AMKSTVAudio);
            DsDevice dev = devs[0];

            dev.Mon.GetDisplayName(null, null, out s);
            m_itva = Marshal.BindToMoniker( s ) as IAMTVAudio;
        }
    }
}