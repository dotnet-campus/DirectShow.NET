using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IRenderEngineTest : IMediaLocator
#if ALLOW_UNTESTED_INTERFACES
        , IGrfCache
#endif
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);
        private IRenderEngine m_ire;
        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private DsROTEntry m_rot;
        private IFilterGraph m_ifg;

        public IRenderEngineTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestUnimplemented();
                TestScrap();
                TestDyn();
                TestRange();
                TestVendor();

                Config();
                TestTimeLine();
                InitVideo();
                TestValidate();
                TestFilterGraph();
                AddVideo("foo.avi");
                TestConnect();
                TestGetGroup();
                TestRender();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ire);
            }
        }

        private void TestUnimplemented()
        {
            int hr;
            int i1;

            hr = m_ire.Commit();
            Debug.Assert(hr == E_NOTIMPL, "Commit");

            hr = m_ire.Decommit();
            Debug.Assert(hr == E_NOTIMPL, "DeCommit");

            // Doc'ed as "not supported", but it seems to do something
            hr = m_ire.DoSmartRecompression();
            //Debug.Assert(hr == E_NOTIMPL, "DoSmartRecompression");

            hr = m_ire.GetCaps(1, out i1);
            Debug.Assert(hr == E_NOTIMPL, "GetCaps");

            hr = m_ire.SetInterestRange(1, 2);
            Debug.Assert(hr == E_INVALIDARG, "GetCaps");

            hr = m_ire.SetInterestRange2(1.1, 2.2);
            Debug.Assert(hr == E_INVALIDARG, "GetCaps");

            // Doc'ed as "not supported", but it seems to do something
            hr = m_ire.SetSourceConnectCallback(this);
            //Debug.Assert(hr == E_NOTIMPL, "GetCaps");

            // Doc'ed as "not supported", but it seems to do something
            hr = m_ire.UseInSmartRecompressionGraph();
            //Debug.Assert(hr == E_NOTIMPL, "GetCaps");
        }

        private void TestTimeLine()
        {
            int hr;
            IAMTimeline tl2;

            m_pTimeline = (IAMTimeline)new AMTimeline();

            hr = m_ire.SetTimelineObject(m_pTimeline);
            DESError.ThrowExceptionForHR(hr);

            hr = m_ire.GetTimelineObject(out tl2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(m_pTimeline == tl2, "TimeLine");
        }

        private void TestFilterGraph()
        {
            int hr;

            m_ifg = (IFilterGraph)new FilterGraph();
            IGraphBuilder ifg2;

            hr = m_ire.SetFilterGraph((IGraphBuilder)m_ifg);
            DESError.ThrowExceptionForHR(hr);

            m_rot = new DsROTEntry(m_ifg);

            hr = m_ire.GetFilterGraph(out ifg2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(m_ifg == ifg2, "FilterGraph");

            hr = m_ire.GetFilterGraph(out ifg2);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestScrap()
        {
            int hr;

            hr = m_ire.ScrapIt();
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestRange()
        {
            int hr;

            hr = m_ire.SetRenderRange(1, 2);
            DESError.ThrowExceptionForHR(hr);

            hr = m_ire.SetRenderRange2(1.1, 2.2);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestVendor()
        {
            int hr;
            string sVendor;

            hr = m_ire.GetVendorString(out sVendor);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(sVendor == "Microsoft Corporation", "Vendor");
        }

        private void TestDyn()
        {
            int hr;

            hr = m_ire.SetDynamicReconnectLevel(ConnectFDynamic.Sources);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestConnect()
        {
            int hr;

            hr = m_ire.ConnectFrontEnd();
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestRender()
        {
            int hr;

            hr = m_ire.RenderOutputPins();
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestGetGroup()
        {
            int hr;
            IPin pPin;

            hr = m_ire.GetGroupOutputPin(0, out pPin);
            DESError.ThrowExceptionForHR(hr);
            Debug.Assert(pPin != null, "GetGroupOutputPin");
            Marshal.ReleaseComObject(pPin);
        }

        private void TestValidate()
        {
            int hr;
            char c = (char)0;

            string sParam = "asdf" + c + "*.asd" + c + c + c + c;

            hr = m_ire.SetSourceNameValidation(sParam, this, SFNValidateFlags.NoFind | SFNValidateFlags.Check | SFNValidateFlags.Popup );
            DESError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_ire = (IRenderEngine)new RenderEngine();
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

#if ALLOW_UNTESTED_INTERFACES
        #region IGrfCache Members

        public int AddFilter(IGrfCache ChainedCache, long Id, IBaseFilter pFilter, string pName)
        {
            // TODO:  Add IRenderEngineTest.AddFilter implementation
            return 0;
        }

        public int DoConnectionsNow()
        {
            // TODO:  Add IRenderEngineTest.DoConnectionsNow implementation
            return 0;
        }

        public int ConnectPins(IGrfCache ChainedCache, long PinID1, IPin pPin1, long PinID2, IPin pPin2)
        {
            // TODO:  Add IRenderEngineTest.ConnectPins implementation
            return 0;
        }

        public int SetGraph(IGraphBuilder pGraph)
        {
            // TODO:  Add IRenderEngineTest.SetGraph implementation
            return 0;
        }

        #endregion
#endif

        #region IMediaLocator Members

        public int AddFoundLocation(string DirectoryName)
        {
            // TODO:  Add IRenderEngineTest.AddFoundLocation implementation
            return 0;
        }

        public int FindMediaFile(string Input, string FilterString, out string pOutput, DirectShowLib.DES.SFNValidateFlags Flags)
        {
            // TODO:  Add IRenderEngineTest.FindMediaFile implementation
            pOutput = null;
            return 0;
        }

        #endregion
    }
}
