using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMSetErrorLogTest : IAMErrorLog
    {
        const int E_NOTIMPL = unchecked((int)0x80004001);
        const int E_INVALIDARG = unchecked((int)0x80070057);

        private IAMTimeline m_pTimeline;
        private IAMTimelineTrack m_VideoTrack;
        private IAMTimelineSrc m_pSource1Src;
        private bool m_Called = false;

        public IAMSetErrorLogTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestErrorLog();
                TestLogError();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestErrorLog()
        {
            int hr;
            IAMSetErrorLog sel;
            IAMErrorLog iel;

            sel = m_pTimeline as IAMSetErrorLog;

            hr = sel.put_ErrorLog(this);
            DESError.ThrowExceptionForHR(hr);

            hr = sel.get_ErrorLog(out iel);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(iel == this, "ErrorLog");
        }

        private void TestLogError()
        {
            int hr;
            IGraphBuilder fg;
            IRenderEngine ire = new RenderEngine() as IRenderEngine;

            hr = ire.SetTimelineObject(m_pTimeline);
            DESError.ThrowExceptionForHR(hr);

            hr = ire.ConnectFrontEnd();
            DESError.ThrowExceptionForHR(hr);

            hr = ire.RenderOutputPins();
            DESError.ThrowExceptionForHR(hr);

            hr = ire.GetFilterGraph(out fg);
            DESError.ThrowExceptionForHR(hr);

            hr = ((IMediaControl)fg).Run();
            DESError.ThrowExceptionForHR(hr);

            IMediaEvent ime = fg as IMediaEvent;
            EventCode evCode;
            const int E_Abort = unchecked((int)0x80004004);
            do
            {
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                hr = ime.WaitForCompletion(1000, out evCode);

            } while (evCode == (EventCode)E_Abort);

            Debug.Assert(m_Called == true, "LogError");
        }

        private void Config()
        {
            int hr = 0;
            IAMTimelineObj pSource1Obj;

            m_pTimeline = (IAMTimeline)new AMTimeline();
            InitVideo();

            // create the timeline source object
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Source);
            DESError.ThrowExceptionForHR(hr);
            m_pSource1Src = (IAMTimelineSrc)pSource1Obj;

            ////////////////////////////
            hr = m_pSource1Src.SetMediaName("fsoo.avi");
            DESError.ThrowExceptionForHR(hr);

            hr = ((IAMTimelineObj)pSource1Obj).SetStartStop(0, 1234563053945);
            DESError.ThrowExceptionForHR(hr);

            // Connect the track to the source
            hr = m_VideoTrack.SrcAdd( (IAMTimelineObj)pSource1Obj );
            DESError.ThrowExceptionForHR(hr);
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
        #region IAMErrorLog Members

        public int LogError(int Severity, string pErrorString, int ErrorCode, int hresult, IntPtr pExtraInfo)
        {
            m_Called = true;
            Debug.Write(string.Format("ErrorLog sev: {0} ErrorCode: {1} HResult: {2} String: {3} Extra: ", Severity, ErrorCode, hresult.ToString("x"), pErrorString));

            try
            {
                switch(Marshal.ReadInt16(pExtraInfo))
                {
                    case 8: // VT_BSTR
                        IntPtr ip = Marshal.ReadIntPtr((IntPtr)(pExtraInfo.ToInt32() + 8));
                        string s = Marshal.PtrToStringUni(ip);
                        Debug.WriteLine(s);
                        break;
                    case 3: // VT_I4
                        Debug.WriteLine(Marshal.ReadInt32((IntPtr)(pExtraInfo.ToInt32() + 8)));
                        break;
                    case 1: // VT_NULL
                        Debug.WriteLine("<null>");
                        break;
                    default:
                        Debug.WriteLine("Unrecognized variant type!");
                        break;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("{0}", e.Message));
            }
            return 0;
        }

        #endregion
    }
}
