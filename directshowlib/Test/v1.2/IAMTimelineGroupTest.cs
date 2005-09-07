using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineGroupTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimelineObj m_pVideoGroupObj;
        private IAMTimelineObj m_pVideoGroupObj2;
        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;

        public IAMTimelineGroupTest()
        {
        }

        public void DoTests()
        {
            Config();
            InitVideo();

            try
            {
            }
            finally
            {
                Marshal.ReleaseComObject(m_pVideoGroupObj);
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void Config()
        {
            int hr;

            m_pTimeline = (IAMTimeline)new AMTimeline();

            // make the root group/composition
            hr = m_pTimeline.CreateEmptyNode( out m_pVideoGroupObj, TimelineMajorType.Group);
            DESError.ThrowExceptionForHR(hr);

            // make the root group/composition
            hr = m_pTimeline.CreateEmptyNode( out m_pVideoGroupObj2, TimelineMajorType.Group);
            DESError.ThrowExceptionForHR(hr);

        }

        private void InitVideo()
        {
            int hr;

            IAMTimelineGroup pVideoGroup = (IAMTimelineGroup)m_pVideoGroupObj;

            // all we set is the major type. The group will automatically use other defaults
            AMMediaType VideoGroupType = new AMMediaType();
            VideoGroupType.majorType = MediaType.Video;

            hr = pVideoGroup.SetMediaType( VideoGroupType );
            DESError.ThrowExceptionForHR(hr);
            DsUtils.FreeAMMediaType(VideoGroupType);

            // add the video group to the timeline
            hr = m_pTimeline.AddGroup( m_pVideoGroupObj );
            DESError.ThrowExceptionForHR(hr);

            IAMTimelineObj pTrack1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pTrack1Obj, TimelineMajorType.Track);
            DESError.ThrowExceptionForHR(hr);

            // tell the composition about the track
            IAMTimelineComp pRootComp = (IAMTimelineComp)m_pVideoGroupObj;
            hr = pRootComp.VTrackInsBefore( pTrack1Obj, -1 );
            DESError.ThrowExceptionForHR(hr);

            m_VideoTrack = (IAMTimelineTrack)pTrack1Obj;
        }
    }
}
