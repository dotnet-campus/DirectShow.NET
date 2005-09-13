using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineEffectableTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);
        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        IAMTimelineEffectable m_ite;

        public IAMTimelineEffectableTest()
        {
        }

        public void DoTests()
        {
            InitVideo();
            Config();

            try
            {
                TestInsBefore();
                TestCount();
                TestSwap();
                TestGetEffect();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ite);
                Marshal.ReleaseComObject(m_VideoTrack);
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestCount()
        {
            int hr;
            int i;

            hr = m_ite.EffectGetCount(out i);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1, "EffectGetCount");
        }

        private void TestInsBefore()
        {
            int hr;
            IAMTimelineObj pSource1Obj;

            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Effect);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.SetStartStop( 0, 100000000 );
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.SetSubObjectGUIDB("{7EF28FD7-E88F-45bb-9CDD-8A62956F2D75}");
            DESError.ThrowExceptionForHR(hr);

            hr = m_ite.EffectInsBefore(pSource1Obj, 0);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestSwap()
        {
            int hr;

            IAMTimelineObj pSource1Obj;

            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Effect);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.SetStartStop( 0, 100000000 );
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.SetSubObjectGUIDB("{8EF28FD7-E88F-45bb-9CDD-8A62956F2D75}");
            DESError.ThrowExceptionForHR(hr);

            hr = m_ite.EffectInsBefore(pSource1Obj, -1);
            DESError.ThrowExceptionForHR(hr);

            hr = m_ite.EffectSwapPriorities(0, 1);
        }

        private void TestGetEffect()
        {
            int hr;
            IAMTimelineObj pObj;
            string s;
            int i;

            hr = m_ite.GetEffect(out pObj, 1);
            DESError.ThrowExceptionForHR(hr);

            hr = pObj.GetSubObjectGUIDB(out s);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert("{7EF28FD7-E88F-45BB-9CDD-8A62956F2D75}" == s, "GetEffect");

            IAMTimelineEffect itfx = pObj as IAMTimelineEffect;
            hr = itfx.EffectGetPriority(out i);
            Debug.Assert(i == 1, "EffectGetPriority");
        }

        private void Config()
        {
            m_ite = m_VideoTrack as IAMTimelineEffectable;
        }

        private void InitVideo()
        {
            int hr;
            IAMTimelineObj pVideoGroupObj;
            m_pTimeline = (IAMTimeline)new AMTimeline();

            // make the root group/composition
            hr = m_pTimeline.CreateEmptyNode( out pVideoGroupObj, TimelineMajorType.Group);
            DESError.ThrowExceptionForHR(hr);

            try
            {
                IAMTimelineGroup pVideoGroup = (IAMTimelineGroup)pVideoGroupObj;

                // all we set is the major type. The group will automatically use other defaults
                AMMediaType VideoGroupType = new AMMediaType();
                VideoGroupType.majorType = MediaType.Video;

                hr = pVideoGroup.SetMediaType( VideoGroupType );
                DESError.ThrowExceptionForHR(hr);
                DsUtils.FreeAMMediaType(VideoGroupType);

                // add the video group to the timeline
                hr = m_pTimeline.AddGroup( pVideoGroupObj );
                DESError.ThrowExceptionForHR(hr);

                IAMTimelineObj pTrack1Obj;
                hr = m_pTimeline.CreateEmptyNode( out pTrack1Obj, TimelineMajorType.Track);
                DESError.ThrowExceptionForHR(hr);

                // tell the composition about the track
                IAMTimelineComp pRootComp = (IAMTimelineComp)pVideoGroupObj;
                hr = pRootComp.VTrackInsBefore( pTrack1Obj, -1 );
                DESError.ThrowExceptionForHR(hr);

                m_VideoTrack = (IAMTimelineTrack)pTrack1Obj;
            }
            finally
            {
                Marshal.ReleaseComObject(pVideoGroupObj);
            }
        }

        private long AddVideo(string VideoFile)
        {
            int hr;
            long dur;

            // create the timeline source object
            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);

            try
            {
                // set up source length (Why doesn't this default to the length of the file!??!?)
                hr = this.m_pTimeline.GetDuration(out dur);

                hr = pSource1Obj.SetStartStop( dur, dur + 10000000000 );
                DESError.ThrowExceptionForHR(hr);

                IAMTimelineSrc pSource1Src = (IAMTimelineSrc)pSource1Obj;

                // Put in the file name
                hr = pSource1Src.SetMediaName( VideoFile );
                DESError.ThrowExceptionForHR(hr);

                // Connect the track to the source
                hr = m_VideoTrack.SrcAdd( pSource1Obj );
                DESError.ThrowExceptionForHR(hr);
            }
            finally
            {
                Marshal.ReleaseComObject(pSource1Obj);
            }

            return 0;
        }

    }
}
