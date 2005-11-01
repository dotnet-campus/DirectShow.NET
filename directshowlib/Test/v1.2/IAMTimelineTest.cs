using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineTest : IMediaLocator
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        IAMTimelineGroup m_pVideoGroup;

        public IAMTimelineTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestDefaultFPS();

                TestEffects();
                TestDefaultEffect();
                TestDefaultEffectB();

                TestTransitions();
                TestDefaultTransition();
                TestDefaultTransitionB();

                TestInsertMode();
                TestCreateNode();
                TestAddGroup();
                TestGroupCount();
                TestCountOfType();
                TestUnimplemented();
                TestDuration();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestDefaultFPS()
        {
            int hr;
            double f;
            const double DFPS = 29.997;

            hr = m_pTimeline.SetDefaultFPS(DFPS);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetDefaultFPS(out f);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(DFPS == f, "GetDefaultFPS");
        }

        private void TestEffects()
        {
            int hr;
            bool b;

            hr = m_pTimeline.EnableEffects(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.EffectsEnabled(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b, "EffectsEnabled");
        }

        private void TestDefaultEffect()
        {
            int hr;
            Guid g = typeof(DxtAlphaSetter).GUID;
            Guid g2;

            hr = m_pTimeline.SetDefaultEffect(g);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetDefaultEffect(out g2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == g2, "GetDefaultEffect");
        }

        private void TestDefaultEffectB()
        {
            int hr;
            string g = "{" + typeof(DxtCompositor).GUID + "}";
            string g2;

            g = g.ToLower();

            hr = m_pTimeline.SetDefaultEffectB(g);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetDefaultEffectB(out g2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == g2.ToLower(), "GetDefaultEffectB");
        }

        private void TestTransitions()
        {
            int hr;
            bool b;

            hr = m_pTimeline.EnableTransitions(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.TransitionsEnabled(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b, "EnableTransitions");
        }

        private void TestDefaultTransition()
        {
            int hr;
            Guid g = typeof(DxtAlphaSetter).GUID;
            Guid g2;

            hr = m_pTimeline.SetDefaultTransition(g);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetDefaultTransition(out g2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == g2, "GetDefaultTransition");
        }

        private void TestDefaultTransitionB()
        {
            int hr;
            string g = "{" + typeof(DxtCompositor).GUID + "}";
            string g2;

            g = g.ToLower();

            hr = m_pTimeline.SetDefaultTransitionB(g);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetDefaultTransitionB(out g2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == g2.ToLower(), "GetDefaultTransitionB");
        }

        private void TestInsertMode()
        {
            int hr;
            TimelineInsertMode m;

            // E_NOTIMPL
            hr = m_pTimeline.SetInsertMode(TimelineInsertMode.Insert);
            //DsError.ThrowExceptionForHR(hr); 

            hr = m_pTimeline.GetInsertMode(out m);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(TimelineInsertMode.Overlay == m, "TimelineInsertMode");
        }

        private void TestCreateNode()
        {
            int hr;
            IAMTimelineObj pVideoGroupObj;

            // make the root group/composition
            hr = m_pTimeline.CreateEmptyNode( out pVideoGroupObj, TimelineMajorType.Group);
            DESError.ThrowExceptionForHR(hr);

            m_pVideoGroup = (IAMTimelineGroup)pVideoGroupObj;
        }

        private void TestAddGroup()
        {
            int hr;

            hr = m_pTimeline.AddGroup((IAMTimelineObj)m_pVideoGroup);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestGroupCount()
        {
            int hr;
            int c;
            IAMTimelineObj pGroup;

            hr = m_pTimeline.GetGroupCount(out c);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(c == 1, "GetGroupCount");

            hr = m_pTimeline.ClearAllGroups();
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetGroupCount(out c);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(c == 0, "ClearAllGroups");

            hr = m_pTimeline.AddGroup((IAMTimelineObj)m_pVideoGroup);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetGroup(out pGroup, 0);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.RemGroupFromList((IAMTimelineObj)pGroup);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetGroupCount(out c);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(c == 0, "RemGroupFromList");
        }

        private void TestCountOfType()
        {
            int hr;
            int c, c1;
            IAMTimelineObj pTrack1Obj;

            hr = m_pTimeline.AddGroup((IAMTimelineObj)m_pVideoGroup);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.CreateEmptyNode( out pTrack1Obj, TimelineMajorType.Track);
            DESError.ThrowExceptionForHR(hr);

            // tell the composition about the track
            IAMTimelineComp pRootComp = (IAMTimelineComp)m_pVideoGroup;
            hr = pRootComp.VTrackInsBefore( pTrack1Obj, -1 );
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.GetCountOfType(0, out c, out c1, TimelineMajorType.Track);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(c == 1 && c1 == 2, "GetCountOfType");
        }

        private void TestUnimplemented()
        {
            int hr;
            bool b;
            long l1, l2;

            hr = m_pTimeline.IsDirty(out b);
            hr = m_pTimeline.SetInterestRange(1, 2);
            hr = m_pTimeline.GetDirtyRange(out l1, out l2);
        }

        private void TestDuration()
        {
            int hr;
            long l;
            double d;

            IAMTimelineObj pTrack1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pTrack1Obj, TimelineMajorType.Track);
            DESError.ThrowExceptionForHR(hr);

            // tell the composition about the track
            IAMTimelineComp pRootComp = (IAMTimelineComp)m_pVideoGroup;
            hr = pRootComp.VTrackInsBefore( pTrack1Obj, -1 );
            DESError.ThrowExceptionForHR(hr);

            m_VideoTrack = (IAMTimelineTrack)pTrack1Obj;

            AddVideo("foo.avi");
            AddVideo("foxo.avi");

            hr = m_pTimeline.GetDuration(out l);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(l == 10000000000, "Duration");

            hr = m_pTimeline.GetDuration2(out d);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(d == 1000.0, "GetDuration2");

            hr = m_pTimeline.ValidateSourceNames(SFNValidateFlags.Replace|SFNValidateFlags.Check|SFNValidateFlags.Popup, this, IntPtr.Zero);
            DESError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_pTimeline = (IAMTimeline)new AMTimeline();
        }

        private long AddVideo(string VideoFile)
        {
            int hr;

            // create the timeline source object
            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);

            try
            {
                hr = pSource1Obj.SetStartStop( 0, 10000000000 );
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
        #region IMediaLocator Members

        public int AddFoundLocation(string DirectoryName)
        {
            // TODO:  Add IAMTimelineTest.AddFoundLocation implementation
            Debug.Assert(false, "AddFoundLocation");
            return 0;
        }

        public int FindMediaFile(string Input, string FilterString, out string pOutput, DirectShowLib.DES.SFNValidateFlags Flags)
        {
            // TODO:  Add IAMTimelineTest.FindMediaFile implementation
            pOutput = "foo.avi";
            return 0;
        }

        #endregion
    }
}
