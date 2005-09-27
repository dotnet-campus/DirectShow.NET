using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineTransableTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);
        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        IAMTimelineTransable m_itt;

        public IAMTimelineTransableTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestAdd();
                TestCount();
                TestNext();
                TestNext2();
                TestNextTime();
                TestNextTime2();
            }
            finally
            {
                Marshal.ReleaseComObject(m_itt);
                Marshal.ReleaseComObject(m_VideoTrack);
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestAdd()
        {
            int hr;

            hr = m_itt.TransAdd(GetTrans());
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestCount()
        {
            int hr;
            int i;

            hr = m_itt.TransGetCount(out i);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1, "TransGetCount");
        }

        private void TestNext()
        {
            int hr;
            long l = 100;
            IAMTimelineObj p;

            hr = m_itt.GetNextTrans(out p, ref l);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(p != null && l == 10000000000, "GetNextTrans");
        }

        private void TestNext2()
        {
            int hr;
            double d = 500.454;
            IAMTimelineObj p;

            hr = m_itt.GetNextTrans2(out p, ref d);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(p != null && d == 1000, "GetNextTrans2");
        }

        private void TestNextTime()
        {
            int hr;
            IAMTimelineObj p;

            hr = m_itt.GetTransAtTime(out p, 1000, DexterFTrackSearchFlags.Bounding);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(p != null, "GetTransAtTime");
        }

        private void TestNextTime2()
        {
            int hr;
            IAMTimelineObj p;

            hr = m_itt.GetTransAtTime2(out p, 500.433, DexterFTrackSearchFlags.Bounding);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(p != null, "GetTransAtTime2");
        }

        private IAMTimelineObj GetTrans()
        {
            int hr;

            // create the timeline source object
            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Transition);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.SetStartStop( 0, 10000000000 );
            DESError.ThrowExceptionForHR(hr);

            return pSource1Obj;
        }

        private void Config()
        {
            InitVideo();
            m_itt = m_VideoTrack as IAMTimelineTransable;
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

    }
}
