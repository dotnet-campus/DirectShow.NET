using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IMediaLocatorTest : IMediaLocator
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private IMediaLocator m_loc;
        private int m_LCalled = 0;
        private int m_FCalled = 0;

        public IMediaLocatorTest()
        {
            m_loc = (IMediaLocator)new MediaLocator();
        }

        public void DoTests()
        {
            Config();
            InitVideo();

            try
            {
                TestIt();
                Debug.Assert(m_LCalled > 0 && m_FCalled > 0, "Called");
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestIt()
        {
            int hr;

            AddVideo("foo.avi");
            AddVideo("foxo.avi");

            hr = AddFoundLocation(@"c:\");
            DESError.ThrowExceptionForHR(hr);

            hr = m_pTimeline.ValidateSourceNames(SFNValidateFlags.Replace|SFNValidateFlags.Check|SFNValidateFlags.Popup, this, IntPtr.Zero);
            DESError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_pTimeline = (IAMTimeline)new AMTimeline();
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
            m_LCalled++;

            return m_loc.AddFoundLocation(DirectoryName);
        }

        public int FindMediaFile(string Input, string FilterString, out string pOutput, DirectShowLib.DES.SFNValidateFlags Flags)
        {
            Debug.Assert(Input == "foxo.avi", "FindMediaFile");
            Debug.Assert(Flags == (SFNValidateFlags.Replace|SFNValidateFlags.Check|SFNValidateFlags.Popup), "FindMediaFile2");
            m_FCalled++;

            return m_loc.FindMediaFile(Input, FilterString, out pOutput, Flags);
        }

        #endregion
    }
}
