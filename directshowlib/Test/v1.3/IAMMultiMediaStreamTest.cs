using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.Test
{
	public class IAMMultiMediaStreamTest
	{
        private IAMMultiMediaStream m_mms;
		public IAMMultiMediaStreamTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestInit();
                TestGetFilter();
                TestAdd();
                TestOpenFile();
                TestOpenMoniker();
                TestRender();
            }
            finally
            {
                Marshal.ReleaseComObject(m_mms);
            }
        }

        private void TestInit()
        {
            int hr;
            IGraphBuilder igb, igb2;

            hr = m_mms.Initialize(StreamType.Read, AMMMultiStream.None, null);
            MsError.ThrowExceptionForHR(hr);

            hr = m_mms.GetFilterGraph(out igb2);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(igb2 == null, "GetFilterGraph");

            igb = (IGraphBuilder)new FilterGraph();
            hr = m_mms.Initialize(StreamType.Read, AMMMultiStream.None, igb);
            MsError.ThrowExceptionForHR(hr);

            hr = m_mms.GetFilterGraph(out igb2);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(igb == igb2, "GetFilterGraph");
        }

        private void TestGetFilter()
        {
            int hr;
            IMediaStreamFilter pFilter;

            hr = m_mms.GetFilter(out pFilter);
            MsError.ThrowExceptionForHR(hr);

            Debug.Assert(pFilter != null, "GetFilter");
            Marshal.ReleaseComObject(pFilter);
        }

        private void TestOpenFile()
        {
            int hr;

            hr = m_mms.OpenFile("foo.avi", AMOpenModes.NoRender);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestRender()
        {
            int hr;

            hr = m_mms.Render(AMOpenModes.RenderAllStreams);
            MsError.ThrowExceptionForHR(hr);
        }

        private void TestAdd()
        {
            int hr;
            IMediaStream pStream = null;

            hr = m_mms.AddMediaStream(null, MSPID.PrimaryAudio, AMMStream.AddDefaultRenderer, pStream);
            MsError.ThrowExceptionForHR(hr);
        }


        private void TestOpenMoniker()
        {
            int hr;
            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            hr = m_mms.OpenMoniker(null, devs[0].Mon, AMOpenModes.NoRender);
            MsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_mms = (IAMMultiMediaStream) new AMMultiMediaStream(); 
        }
	}
}
