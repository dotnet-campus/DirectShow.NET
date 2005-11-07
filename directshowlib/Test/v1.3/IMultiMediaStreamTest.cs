using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;
using System.Threading;

namespace DirectShowLib.Test
{
	public class IMultiMediaStreamTest
	{
        private IMultiMediaStream m_mms;
		public IMultiMediaStreamTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestEnum();
                TestDuration();
                TestGetInfo();
                TestSeek();
                TestGetStream();
                TestStateAndEOS();
            }
            finally
            {
                Marshal.ReleaseComObject(m_mms);
            }
        }

        private void TestStateAndEOS()
        {
            int hr;
            IntPtr pEOS;
            StreamState pState;
            ManualResetEvent mre = new ManualResetEvent(false);

            ////////////////
            /// The AddMediaStream does something that prevents playback (I don't
            /// know what).  Create a new stream to do the actual run.
            IAMMultiMediaStream amms = (IAMMultiMediaStream)new AMMultiMediaStream();

            hr = amms.OpenFile("foo.avi", AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);
            IMultiMediaStream mms = amms as IMultiMediaStream;
            ////////////////

            hr = mms.GetEndOfStreamEventHandle(out pEOS);
            MsError.ThrowExceptionForHR(hr);

            mre.Handle = pEOS;

            hr = mms.Seek(0);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.GetState(out pState);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pState == StreamState.Stop, "GetState");

            hr = mms.SetState(StreamState.Run);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.GetState(out pState);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pState == StreamState.Run, "GetState2");

            bool b = mre.WaitOne(1000 * 6, false);

            Debug.Assert(b, "WaitOne");

            mre.Close();
            Marshal.ReleaseComObject(mms);
        }

        private void TestSeek()
        {
            int hr;
            long t;

            hr = m_mms.Seek(43200000 / 2);
            MsError.ThrowExceptionForHR(hr);

            hr = m_mms.GetTime(out t);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(t == 43200000 / 2, "Seek/GetTime");
        }

        private void TestGetStream()
        {
            int hr;
            IMediaStream pStream;

            hr = m_mms.GetMediaStream(MSPID.PrimaryAudio, out pStream);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pStream != null, "GetMediaStream");
            Marshal.ReleaseComObject(pStream);
        }

        private void TestGetInfo()
        {
            int hr;
            MMSSF f;
            StreamType t;

            hr = m_mms.GetInformation(out f, out t);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(f == (MMSSF.HasClock | MMSSF.Asynchronous | MMSSF.SupportSeek), "GetInformation");
            Debug.Assert(t == StreamType.Read, "GetInformation");
        }

        private void TestDuration()
        {
            int hr;
            long lDur;

            hr = m_mms.GetDuration(out lDur);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(lDur == 43200000, "GetDuration");
        }

        private void TestEnum()
        {
            int hr;
            IMediaStream pStream;

            hr = m_mms.EnumMediaStreams(0, out pStream);
            MsError.ThrowExceptionForHR(hr);
            Debug.Assert(hr == 0 && pStream != null, "EnumMediaStreams");

            hr = m_mms.EnumMediaStreams(1, out pStream);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr == 1 && pStream == null, "EnumMediaStreams2");
        }

        private void Config()
        {
            int hr;
            IMediaStream pStream = null;
            IAMMultiMediaStream amms = (IAMMultiMediaStream)new AMMultiMediaStream();

            hr = amms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, pStream);
            MsError.ThrowExceptionForHR(hr);

            hr = amms.OpenFile("foo.avi", AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);

            IGraphBuilder pGraphBuilder;
            hr = amms.GetFilterGraph(out pGraphBuilder);
            MsError.ThrowExceptionForHR(hr);
            DsROTEntry rot = new DsROTEntry(pGraphBuilder);

            m_mms = (IMultiMediaStream)amms;
        }
	}
}
