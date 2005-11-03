using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineSrcTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private IAMTimelineSrc m_pSource1Src;

        public IAMTimelineSrcTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestIsNormal(true);
                TestSplice();
                TestFixMedia();
                TestFixMedia2();
                TestMediaTimes();
                TestMediaTimes2();
                TestFPS();
                TestMediaName();
                TestStreamNumber();
                TestStretchMode();
                TestMediaLength();
                TestMediaLength2();
                TestModifyStop();
                TestModifyStop2();
                TestIsNormal(false);
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestIsNormal(bool c)
        {
            int hr;
            bool b;

            hr = m_pSource1Src.IsNormalRate(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == c, "IsNormalRate");
        }

        private void TestSplice()
        {
            int hr;
            long pStart, pStop;

            IAMTimelineSplittable ps;
            IAMTimelineObj pFirst, pSecond;
            hr = m_pTimeline.CreateEmptyNode( out pFirst, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);

            hr = pFirst.SetStartStop( 0, 10000000000 );
            DESError.ThrowExceptionForHR(hr);

            IAMTimelineSrc pFirstSrc = (IAMTimelineSrc)pFirst;
            hr = pFirstSrc.SetMediaTimes(0, 10000000000);

            // Put in the file name
            hr = pFirstSrc.SetMediaName( "foo.avi" );
            DESError.ThrowExceptionForHR(hr);

            // Connect the track to the source
            hr = m_VideoTrack.SrcAdd( pFirst );
            DESError.ThrowExceptionForHR(hr);

            // Split the source
            ps = pFirst as IAMTimelineSplittable;

            long l = 500000000;
            hr = ps.SplitAt(l);
            DESError.ThrowExceptionForHR(hr);

            // Get the new chunk
            hr = m_VideoTrack.GetNextSrc(out pSecond, ref l);
            DESError.ThrowExceptionForHR(hr);

            // Re-join the two
            hr = pFirstSrc.SpliceWithNext(pSecond);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestFixMedia()
        {
            int hr;

            long start, stop;

            start = 1243;
            stop =  10000000000;

            hr = m_pSource1Src.SetMediaName("foo.avi");
            DESError.ThrowExceptionForHR(hr);

            hr = ((IAMTimelineObj)m_pSource1Src).SetStartStop(0, 1234563053945);
            DESError.ThrowExceptionForHR(hr);

            // Connect the track to the source
            hr = m_VideoTrack.SrcAdd( (IAMTimelineObj)m_pSource1Src );
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.FixMediaTimes(ref start, ref stop);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(stop != 10000000000, "FixMediaTimes");
        }

        private void TestFixMedia2()
        {
            int hr;

            double start, stop;

            start = 1243.124;
            stop =  10000000000.6457;

            hr = m_pSource1Src.SetMediaName("foo.avi");
            DESError.ThrowExceptionForHR(hr);

            hr = ((IAMTimelineObj)m_pSource1Src).SetStartStop(0, 1234563053945);
            DESError.ThrowExceptionForHR(hr);

            // Connect the track to the source
            hr = m_VideoTrack.SrcAdd( (IAMTimelineObj)m_pSource1Src );
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.FixMediaTimes2(ref start, ref stop);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(stop != 10000000000.6457, "FixMediaTimes2");
        }
        private void TestFPS()
        {
            int hr;
            const double FPS = 13.14;
            double d;

            hr = m_pSource1Src.SetDefaultFPS(FPS);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetDefaultFPS(out d);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(d == FPS, "TestFPS");
        }

        private void TestMediaName()
        {
            int hr;
            const string fn = "foo.avi";
            string s;

            hr = m_pSource1Src.SetMediaName(fn);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetMediaName(out s);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(fn == s, "MediaName");
        }

        private void TestStreamNumber()
        {
            int hr;
            int i;

            hr = m_pSource1Src.SetStreamNumber(1);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetStreamNumber(out i);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1, "StreamNumber");
        }

        private void TestStretchMode()
        {
            int hr;
            ResizeFlags sm;

            hr = m_pSource1Src.SetStretchMode(ResizeFlags.Crop);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetStretchMode(out sm);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(sm == ResizeFlags.Crop, "StretchMode");
        }

        private void TestMediaLength()
        {
            int hr;
            long ml;
            const long LEN = 1000;

            hr = m_pSource1Src.SetMediaLength(LEN);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetMediaLength(out ml);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(ml == LEN, "MediaLength");
        }

        private void TestMediaLength2()
        {
            int hr;
            double ml;
            const double LEN = 1000.1234;

            hr = m_pSource1Src.SetMediaLength2(LEN);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetMediaLength2(out ml);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(ml == LEN, "MediaLength2");
        }

        private void TestMediaTimes()
        {
            int hr;
            long i, i2;
            const long start = 1000;
            const long stop = 2000;

            hr = m_pSource1Src.SetMediaTimes(start, stop);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetMediaTimes(out i, out i2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == start && i2 == stop, "MediaTimes");
        }

        private void TestMediaTimes2()
        {
            int hr;
            double i, i2;
            const double start = 1000.4321;
            const double stop = 2000.5678;

            hr = m_pSource1Src.SetMediaTimes2(start, stop);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pSource1Src.GetMediaTimes2(out i, out i2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == start && i2 == stop, "MediaTimes2");
        }

        private void TestModifyStop()
        {
            int hr;
            long l1, l2;
            IAMTimelineObj pObj = (IAMTimelineObj)m_pSource1Src;

            hr = m_pSource1Src.ModifyStopTime(1200);
            DESError.ThrowExceptionForHR(hr);

            hr = pObj.GetStartStop(out l1, out l2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(l2 == 1200, "ModifyStopTime");
        }

        private void TestModifyStop2()
        {
            int hr;
            double l1, l2;
            IAMTimelineObj pObj = (IAMTimelineObj)m_pSource1Src;

            hr = m_pSource1Src.ModifyStopTime2(1200.56);
            DESError.ThrowExceptionForHR(hr);

            hr = pObj.GetStartStop2(out l1, out l2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(l2 == 1200.56, "ModifyStopTime2");
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
