using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace DirectShowLib.MultimediaStreaming
{
    #region Classes
    /// <summary>
    /// From CLSID_AMMultiMediaStream
    /// </summary>
    [ComImport, Guid("49c47ce5-9ba4-11d0-8212-00c04fc32c45")]
    public class AMMultiMediaStream
    {
    }

    /// <summary>
    /// From CLSID_AMMediaTypeStream
    /// </summary>
    [ComImport, Guid("CF0F2F7C-F7BF-11d0-900D-00C04FD9189D")]
    public class AMMediaTypeStream
    {
    }

    /// <summary>
    /// From CLSID_AMDirectDrawStream
    /// </summary>
    [ComImport, Guid("49c47ce4-9ba4-11d0-8212-00c04fc32c45")]
    public class AMDirectDrawStream
    {
    }

    /// <summary>
    /// From CLSID_AMAudioStream
    /// </summary>
    [ComImport, Guid("8496e040-af4c-11d0-8212-00c04fc32c45")]
    public class AMAudioStream
    {
    }

    /// <summary>
    /// From CLSID_AMAudioData
    /// </summary>
    [ComImport, Guid("f2468580-af8a-11d0-8212-00c04fc32c45")]
    public class AMAudioData
    {
    }
    #endregion

    #region GUIDS

    public class MSPID
    {
        /// <summary> MSPID_PrimaryVideo </summary>
        public static readonly Guid PrimaryVideo = new Guid(0xa35ff56a, 0x9fda, 0x11d0, 0x8f, 0xdf, 0x0, 0xc0, 0x4f, 0xd9, 0x18, 0x9d);

        /// <summary> MSPID_PrimaryAudio </summary>
        public static readonly Guid PrimaryAudio = new Guid(0xa35ff56b, 0x9fda, 0x11d0, 0x8f, 0xdf, 0x0, 0xc0, 0x4f, 0xd9, 0x18, 0x9d);
    }

    #endregion

}

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
