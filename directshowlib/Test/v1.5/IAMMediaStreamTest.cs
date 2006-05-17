using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace v1._5
{
	public class IAMMediaStreamTest
	{
        IAMMediaStream m_aStream;
        IFilterGraph m_pGraph;
        IMediaStreamFilter m_pStream;

        public void DoTests()
        {
            Config();

            try
            {
                TestGraph();
                TestFilter();
                TestState();
                TestJoinIAM();
                TestInit();
            }
            finally
            {
            }
        }

        private void TestGraph()
        {
            int hr;
            m_pGraph = new FilterGraph() as IFilterGraph;

            hr = m_aStream.JoinFilterGraph(m_pGraph);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestFilter()
        {
            int hr;

            hr = m_aStream.JoinFilter(m_pStream);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestState()
        {
            int hr;

            hr = m_aStream.SetState(FilterState.Paused);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestJoinIAM()
        {
            int hr;

            IAMMultiMediaStream mms = (IAMMultiMediaStream)new AMMultiMediaStream();

            hr = m_aStream.JoinAMMultiMediaStream(mms);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestInit()
        {
            int hr;

            hr = m_aStream.Initialize(m_pStream, AMMStream.None, MSPID.PrimaryAudio, StreamType.Read);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            int hr;
            IMediaStream pStream = null;
            IMediaStreamFilter msf;

            IAMMultiMediaStream mms = (IAMMultiMediaStream) new AMMultiMediaStream(); 

            hr = mms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.None, pStream);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.GetFilter(out msf);
            MsError.ThrowExceptionForHR(hr);

            hr = mms.OpenFile("foo.avi", AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);

            hr = msf.GetMediaStream(MSPID.PrimaryAudio, out pStream);
            MsError.ThrowExceptionForHR(hr);

            m_aStream = pStream as IAMMediaStream;
            hr = mms.GetFilter(out m_pStream);

        }
	}
}
