using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineObjTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimelineObj m_pVideoGroupObj;
        private IAMTimelineObj m_pVideoGroupObj2;
        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;

        public IAMTimelineObjTest()
        {
        }

        public void DoTests()
        {
            Config();
            InitVideo();

            try
            {
                TestFix();
                TestFix2();
                TestLocked();
                TestMuted();
                TestDirty();
                TestUser();
                TestName();
                TestPropSet();
                TestType();
                TestGen();
                TestData();
                TestSub();
                TestTL();
                TestStartStop();
                TestStartStop2();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pVideoGroupObj);
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestType()
        {
            int hr;
            TimelineMajorType t;

            hr = m_pVideoGroupObj.GetTimelineType(out t);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(t == TimelineMajorType.Group, "TimelineType");

            hr = m_pVideoGroupObj.SetTimelineType(TimelineMajorType.Effect);
            Debug.Assert(hr == E_INVALIDARG, "Not supported");
        }

        private void TestPropSet()
        {
            int hr;
            IPropertySetter pVal = (IPropertySetter)new PropertySetter();
            IPropertySetter pVal2;

            hr = m_pVideoGroupObj.SetPropertySetter(pVal);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetPropertySetter(out pVal2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(pVal == pVal2, "PropertySetter");
        }

        private void TestStartStop()
        {
            const int START = 100000;
            const int END = 200000;
            int hr;
            long i1, i2;

            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);

            hr = pSource1Obj.SetStartStop(START, END);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.GetStartStop(out i1, out i2);

            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i1 == START && i2 == END, "StartStop");
        }

        private void TestStartStop2()
        {
            const double START = 123.456;
            const double END = 234.567;
            int hr;
            double i1, i2;

            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);

            hr = pSource1Obj.SetStartStop2(START, END);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.GetStartStop2(out i1, out i2);

            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i1 == START && i2 == END, "StartStop");
        }

        private void TestFix()
        {
            const int START = 100000001;
            const int END = 200000001;
            int hr;
            long i1, i2;

            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);

            hr = pSource1Obj.SetStartStop(START, END);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.GetStartStop(out i1, out i2);
            DESError.ThrowExceptionForHR(hr);

            hr = m_VideoTrack.SrcAdd( pSource1Obj );
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.FixTimes(ref i1, ref i2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i1 == START-1 && i2 == END-1, "fix");
        }

        private void TestFix2()
        {
            const double START = 123.456;
            const double END = 234.567;
            int hr;
            double i1, i2;

            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);

            hr = pSource1Obj.SetStartStop2(START, END);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.GetStartStop2(out i1, out i2);
            DESError.ThrowExceptionForHR(hr);

            hr = m_VideoTrack.SrcAdd( pSource1Obj );
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.FixTimes2(ref i1, ref i2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert((int)(i1 * 10) == 1234 && (int)(i2 * 10) == 2345, "fix2");
        }

        private void TestGen()
        {
            int hr;
            int pVal;

            hr = m_pVideoGroupObj.GetGenID(out pVal);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestData()
        {
            int hr;
            const int SIZE = 100;
            int i;

            IntPtr ip = Marshal.AllocCoTaskMem(SIZE);
            IntPtr ip2 = Marshal.AllocCoTaskMem(SIZE);
            Marshal.WriteInt64(ip, 123456789);

            hr = m_pVideoGroupObj.SetUserData(ip, SIZE);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetUserData(ip2, out i);
            DESError.ThrowExceptionForHR(hr);

            long j = Marshal.ReadInt64(ip2);
            Debug.Assert(i == SIZE && j == 123456789, "UserData");

            Marshal.FreeCoTaskMem(ip);
            Marshal.FreeCoTaskMem(ip2);
        }

        private void TestName()
        {
            int hr;
            string s;

            hr = m_pVideoGroupObj.SetUserName("AsDf");
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetUserName(out s);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(s == "AsDf", "UserName");
        }

        private void TestUser()
        {
            int hr;
            int i;

            hr = m_pVideoGroupObj.SetUserID(1234);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetUserID(out i);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1234, "UserID");
        }

        private void TestDirty()
        {
            int hr;
            long l1, l2;
            double d1, d2;

            // Not supported
            hr = m_pVideoGroupObj.SetDirtyRange(1, 2);
            Debug.Assert(hr == E_NOTIMPL, "SetDirtyRange");

            // Not supported
            hr = m_pVideoGroupObj.SetDirtyRange2(1.1, 2.2);
            Debug.Assert(hr == E_NOTIMPL, "SetDirtyRange2");

            // Not supported
            hr = m_pVideoGroupObj.GetDirtyRange(out l1, out l2);
            Debug.Assert(hr == 0 && l1 == -1 && l2 == -1, "GetDirtyRange");

            // Not supported
            hr = m_pVideoGroupObj.GetDirtyRange2(out d1, out d2);
            Debug.Assert(hr == 0 && d1 == -0.0000001 && d2 == -0.0000001, "GetDirtyRange2");

            // Not supported
            hr = m_pVideoGroupObj.ClearDirty();
            //Debug.Assert(hr == E_NOTIMPL, "ClearDirty");
        }

        private void TestLocked()
        {
            int hr;
            bool bLocked, bLocked2;

            hr = m_pVideoGroupObj.GetLocked(out bLocked);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.SetLocked(!bLocked);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetLocked(out bLocked2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(bLocked != bLocked2, "Locked");
        }

        private void TestMuted()
        {
            int hr;
            bool bLocked, bLocked2;

            hr = m_pVideoGroupObj.GetMuted(out bLocked);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.SetMuted(!bLocked);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetMuted(out bLocked2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(bLocked != bLocked2, "Muted");
        }

        private void TestSub()
        {
            int hr;
            object o;
            string s = "{"+MediaType.URLStream.ToString()+"}";
            string s2;
            Guid g;
            bool b;
            int iDepth;
            IAMTimelineGroup pGroup;

            s = s.ToUpper();

            hr = m_pVideoGroupObj.GetSubObjectLoaded(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(!b, "Loaded");

            hr = m_pVideoGroupObj.SetSubObject(m_pVideoGroupObj2);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetSubObject(out o);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(m_pVideoGroupObj2 == (IAMTimelineObj)o, "SubObject");

            hr = m_pVideoGroupObj.GetSubObjectLoaded(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b, "Loaded");

            hr = m_pVideoGroupObj2.GetGroupIBelongTo(out pGroup);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(pGroup == m_pVideoGroupObj2, "GetGroupIBelongTo");

            hr = m_pVideoGroupObj.SetSubObjectGUID(MediaType.URLStream);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetSubObjectGUID(out g);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaType.URLStream, "SubObjGuid");

            hr = m_pVideoGroupObj.SetSubObjectGUIDB(s);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.GetSubObjectGUIDB(out s2);
            DESError.ThrowExceptionForHR(hr);

            s2 = s2.ToUpper();

            Debug.Assert(s == s2, "SubObjGuidB");

            // Not supported
            hr = m_pVideoGroupObj2.GetEmbedDepth(out iDepth);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj2.Remove();
            DESError.ThrowExceptionForHR(hr);

            hr = m_pVideoGroupObj.RemoveAll();
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestTL()
        {
            int hr;
            IAMTimeline ppResult;

            // Not supported
            hr = m_pVideoGroupObj.GetTimelineNoRef(out ppResult);
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
