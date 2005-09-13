using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IXml2DexTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);
        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private IFilterGraph m_ifg;
        private IXml2Dex m_ixd;

        public IXml2DexTest()
        {
        }

        public void DoTests()
        {
            Config();
            InitVideo();

            try
            {
                TestWriteXML();
                TestWriteGRF();
                TestWrite();
                ReadXML();
                TestNotImplemented();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
                Marshal.ReleaseComObject(m_VideoTrack);
                Marshal.ReleaseComObject(m_ifg);
                Marshal.ReleaseComObject(m_ixd);
            }
        }

        private void TestWriteXML()
        {
            int hr;

            hr = m_ixd.WriteXMLFile(m_pTimeline, "foo.xml");
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(File.Exists("foo.xml"), "WriteXMLFile");
        }

        private void TestWriteGRF()
        {
            int hr;

            hr = m_ixd.WriteGrfFile(m_ifg, "foo.grf");
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(File.Exists("foo.grf"), "WriteGrfFile");
        }

        private void TestNotImplemented()
        {
            int hr;
            object o = "asdf";
            object o1 = m_pTimeline;
            object o2 = m_ixd;
            IXml2Dex ixd2 = (IXml2Dex)new Xml2Dex();

            // Supposed to be not implemented but returns 0
            hr = m_ixd.Reset();

            hr = m_ixd.CopyXML(o, 1.1, 2.2);
            Debug.Assert(hr == E_NOTIMPL, "CopyXML");

            hr = m_ixd.WriteXMLPart(o, 1.1, 2.2, "asdf");
            Debug.Assert(hr == E_NOTIMPL, "CopyXML");

            hr = m_ixd.PasteXML(o, 1.1);
            Debug.Assert(hr == E_NOTIMPL, "CopyXML");

            hr = m_ixd.PasteXMLFile(o, 1.1, "asdf");
            Debug.Assert(hr == E_NOTIMPL, "CopyXML");

            // Doesn't seem to like any of the parms I send it.
            // It is undoc'ed, so it is hard to get right.
            hr = m_ixd.ReadXML(m_pTimeline, ixd2);

            // Again supposed to be unimplemented, but seems to do something.
            hr = m_ixd.CreateGraphFromFile(out o2, m_pTimeline, @"foo.xml");

            // Again supposed to be unimplemented, but seems to do something.
            hr = m_ixd.Delete(m_pTimeline, 1.1, 2.2);
        }

        private void TestWrite()
        {
            int hr;
            string s;

            hr = m_ixd.WriteXML(m_pTimeline, out s);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(s.Length > 10, "WriteXML");
        }

        private void ReadXML()
        {
            int hr;

            hr = m_ixd.ReadXMLFile(m_pTimeline, "foo.xml");
            DESError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_ixd = (IXml2Dex)new Xml2Dex();
            m_pTimeline = (IAMTimeline)new AMTimeline();
            IRenderEngine ire = (IRenderEngine)new RenderEngine();
            m_ifg = (IFilterGraph)new FilterGraph();
            ire.SetFilterGraph((IGraphBuilder)m_ifg);
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

    }
}
