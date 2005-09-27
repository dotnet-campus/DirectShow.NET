using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineVirtualTrackTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimeline m_pTimeline;
        private IAMTimelineVirtualTrack m_vTrack;

        public IAMTimelineVirtualTrackTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestGetPriority();
                TestSetDirty();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
                Marshal.ReleaseComObject(m_vTrack);
            }
        }

        private void TestGetPriority()
        {
            int hr;
            int p;

            hr = m_vTrack.TrackGetPriority(out p);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(p == -1);
        }

        private void TestSetDirty()
        {
            int hr;

            hr = m_vTrack.SetTrackDirty();
            Debug.Assert(hr == E_NOTIMPL, "SetTrackDirty");
        }

        private void Config()
        {
            int hr;
            IAMTimelineObj pSource1Obj;

            m_pTimeline = (IAMTimeline)new AMTimeline();
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Track);

            m_vTrack = pSource1Obj as IAMTimelineVirtualTrack;
        }
    }
}
