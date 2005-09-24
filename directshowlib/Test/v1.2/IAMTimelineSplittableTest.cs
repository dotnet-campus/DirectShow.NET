using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineSplittableTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private IAMTimelineSrc m_pSource1Src;
        private IAMTimelineSplittable m_Splittable;

        public IAMTimelineSplittableTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestSplitAt();
                TestSplitAt2();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestSplitAt()
        {
            int hr;
            int i1, i2;

            hr = m_VideoTrack.GetSourcesCount(out i1);
            DESError.ThrowExceptionForHR(hr);

            hr = m_Splittable.SplitAt(100);
            DESError.ThrowExceptionForHR(hr);

            hr = m_VideoTrack.GetSourcesCount(out i2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i2 == i1 + 1, "SplitAt");
        }

        private void TestSplitAt2()
        {
            int hr;
            int i1, i2;
            IAMTimelineObj pSrc;
            IAMTimelineSplittable pSplit;

            hr = m_VideoTrack.GetSourcesCount(out i1);
            DESError.ThrowExceptionForHR(hr);

            // Can't use m_Splittable.  Not sure why.  Find another object
            hr = m_VideoTrack.GetSrcAtTime2(out pSrc, 2.2, DexterFTrackSearchFlags.Bounding);
            DESError.ThrowExceptionForHR(hr);

            pSplit = (IAMTimelineSplittable)pSrc;

            hr = pSplit.SplitAt2(2.2);
            DESError.ThrowExceptionForHR(hr);

            hr = m_VideoTrack.GetSourcesCount(out i2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i2 == i1 + 1, "SplitAt2");
        }

        private void Config()
        {
            int hr;
            IAMTimelineObj pSource1Obj;

            m_pTimeline = (IAMTimeline)new AMTimeline();
            InitVideo();

            // create the timeline source object
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);
            m_pSource1Src = (IAMTimelineSrc)pSource1Obj;

            ////////////////////////////
            hr = m_pSource1Src.SetMediaName("foo.avi");
            DESError.ThrowExceptionForHR(hr);

            hr = ((IAMTimelineObj)pSource1Obj).SetStartStop(0, 1234563053945);
            DESError.ThrowExceptionForHR(hr);

            // Connect the track to the source
            hr = m_VideoTrack.SrcAdd( (IAMTimelineObj)pSource1Obj );
            DESError.ThrowExceptionForHR(hr);

            ////////////////////////////

            m_Splittable = (IAMTimelineSplittable)pSource1Obj;
        }

        private void InitVideo()
        {
            int hr;
            IAMTimelineObj pVideoGroupObj;

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

    }
}
