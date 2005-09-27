using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class ISmartRenderEngineTest : IFindCompressorCB
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);

        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private ISmartRenderEngine m_pRenderEngine;
        private IAMTimelineGroup m_pVideoGroup;

        public ISmartRenderEngineTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestCompressor();
                TestSetFindCompressorCB();
            }
            finally
            {
                if (m_pTimeline != null)
                {
                    Marshal.ReleaseComObject(m_pTimeline);
                }
                if (m_VideoTrack != null)
                {
                    Marshal.ReleaseComObject(m_VideoTrack);
                }
                if (m_pRenderEngine != null)
                {
                    Marshal.ReleaseComObject(m_pRenderEngine);
                }
                if (m_pVideoGroup != null)
                {
                    Marshal.ReleaseComObject(m_pVideoGroup);
                }
            }

        }

        private void TestCompressor()
        {
            int hr;
            IBaseFilter pComp;

            hr = m_pRenderEngine.GetGroupCompressor(0, out pComp);
            DESError.ThrowExceptionForHR(hr);

            hr = m_pRenderEngine.SetGroupCompressor(0, pComp);
            DESError.ThrowExceptionForHR(hr);
        }
        
        private void TestSetFindCompressorCB()
        {
            int hr;

            hr = m_pRenderEngine.SetFindCompressorCB(this);
            Debug.Assert(hr == E_NOTIMPL, "SetFindCompressorCB");
        }

        private void Config()
        {
            // Initialize the data members
            m_pTimeline = (IAMTimeline)new AMTimeline();

            InitVideo();
            AddVideo(@"foo2.avi");
            RenderCommon();
        }

        private void InitVideo()
        {
            int hr;
            IAMTimelineObj pVideoGroupObj;

            // make the root group/composition
            hr = m_pTimeline.CreateEmptyNode( out pVideoGroupObj, TimelineMajorType.Group);
            DESError.ThrowExceptionForHR(hr);

            m_pVideoGroup = (IAMTimelineGroup)pVideoGroupObj;

            // all we set is the major type. The group will automatically use other defaults
            AMMediaType VideoGroupType = new AMMediaType();
            VideoGroupType.majorType = MediaType.Video;

            hr = m_pVideoGroup.SetMediaType( VideoGroupType );
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

        private void AddVideo(string VideoFile)
        {
            int hr;

            // create the timeline source object
            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);

            try
            {
                hr = pSource1Obj.SetStartStop( 0, 1333340 );
                DESError.ThrowExceptionForHR(hr);

                IAMTimelineSrc pSource1Src = (IAMTimelineSrc)pSource1Obj;

                // Put in the file name
                hr = pSource1Src.SetMediaName( VideoFile );
                DESError.ThrowExceptionForHR(hr);

                // Connect the track to the source
                hr = m_VideoTrack.SrcAdd( pSource1Obj );
                DESError.ThrowExceptionForHR(hr);

                hr = m_pVideoGroup.SetRecompFormatFromSource(pSource1Src);
                DESError.ThrowExceptionForHR(hr);
            }
            finally
            {
                Marshal.ReleaseComObject(pSource1Obj);
            }
        }

        private void RenderCommon()
        {
            int hr;

            // create the render engine
            IRenderEngine pRenderEngine = (IRenderEngine)new SmartRenderEngine();

            m_pRenderEngine = pRenderEngine as ISmartRenderEngine;

            // tell the render engine about the timeline it should use
            hr = pRenderEngine.SetTimelineObject( m_pTimeline );
            DESError.ThrowExceptionForHR(hr);

            // connect up the front end
            hr = pRenderEngine.ConnectFrontEnd( );
            DESError.ThrowExceptionForHR(hr);
        }

        #region IFindCompressorCB Members

        public int GetCompressor(AMMediaType pType, AMMediaType pCompType, out IBaseFilter ppFilter)
        {
            ppFilter = null;
            return 0;
        }

        #endregion
    }
}
