using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineGroupTest
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimelineGroup m_itg;
        private IAMTimeline m_pTimeline;

        public IAMTimelineGroupTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestOutputFPS();
                TestGroupName();
                TestTimeline();
                TestPreview();
                TestOutputBuffering();
                TestPriority();
                TestSmartRecompress();
                TestVMMediaType();
                TestMediaType();
                TestRecompFormatFromSource();
            }
            finally
            {
                Marshal.ReleaseComObject(m_itg);
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestOutputFPS()
        {
            int hr;
            double f;

            hr = m_itg.SetOutputFPS(123.321);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.GetOutputFPS(out f);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(f == 123.321, "OutputFPS");
        }

        private void TestGroupName()
        {
            int hr;
            string s;

            hr = m_itg.SetGroupName("asdf");
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.GetGroupName(out s);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(s == "asdf", "GroupName");
        }

        private void TestTimeline()
        {
            int hr;
            IAMTimeline p;

            hr = m_itg.SetTimeline(m_pTimeline);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.GetTimeline(out p);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(p == m_pTimeline, "Timeline");
        }

        private void TestPreview()
        {
            int hr;
            bool b;

            hr = m_itg.SetPreviewMode(true);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.GetPreviewMode(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "preview");
        }

        private void TestOutputBuffering()
        {
            int hr;
            int b;

            hr = m_itg.SetOutputBuffering(6);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.GetOutputBuffering(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == 6, "buffering");
        }


        private void TestPriority()
        {
            int hr;
            int p;

            hr = m_itg.GetPriority(out p);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestSmartRecompress()
        {
            int hr;
            SCompFmt0 p = new SCompFmt0();
            bool b;

            hr = m_itg.GetSmartRecompressFormat(out p);
            DESError.ThrowExceptionForHR(hr);

            p.MediaType = new AMMediaType();
            p.MediaType.majorType = MediaType.Video;
            p.MediaType.subType = MediaSubType.MPEG2Video;

            hr = m_itg.SetSmartRecompressFormat(p);
            DESError.ThrowExceptionForHR(hr);

            // Unsupported
            hr = m_itg.IsRecompressFormatDirty(out b);
            DESError.ThrowExceptionForHR(hr);

            // Unsupported
            hr = m_itg.ClearRecompressFormatDirty();
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestVMMediaType()
        {
            int hr;

            hr = m_itg.SetMediaTypeForVB(0);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestMediaType()
        {
            int hr;
            AMMediaType p = new AMMediaType();

            hr = m_itg.GetMediaType(p);
            DESError.ThrowExceptionForHR(hr);

            // From SetMediaTypeForVB
            Debug.Assert(p.majorType == MediaType.Video, "mediatype1");

            p.majorType = MediaType.Video;

            hr = m_itg.SetMediaType(p);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestRecompFormatFromSource()
        {
            int hr;
            bool b;

            IAMTimelineObj pFirst;
            hr = m_pTimeline.CreateEmptyNode( out pFirst, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);

            hr = pFirst.SetStartStop( 0, 10000000000 );
            DESError.ThrowExceptionForHR(hr);

            IAMTimelineSrc pFirstSrc = (IAMTimelineSrc)pFirst;
            hr = pFirstSrc.SetMediaTimes(0, 10000000000);

            // Put in the file name
            hr = pFirstSrc.SetMediaName( "foo.avi" );
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.IsSmartRecompressFormatSet(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == false, "IsSmart");

            hr = m_itg.SetRecompFormatFromSource(pFirstSrc);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itg.IsSmartRecompressFormatSet(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "IsSmart2");
        }

        private void Config()
        {
            int hr;
            IAMTimelineObj m_pVideoGroupObj2;

            m_pTimeline = (IAMTimeline)new AMTimeline();

            // make the root group/composition
            hr = m_pTimeline.CreateEmptyNode( out m_pVideoGroupObj2, TimelineMajorType.Group);
            DESError.ThrowExceptionForHR(hr);

            m_itg = m_pVideoGroupObj2 as IAMTimelineGroup;

        }
    }
}
