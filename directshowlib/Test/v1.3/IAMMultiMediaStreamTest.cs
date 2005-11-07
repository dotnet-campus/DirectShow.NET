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

    #region Utility classes
    sealed public class MsResults
    {
        private MsResults()
        {
            // Prevent people from trying to instantiate this class
        }

        public const int S_Pending                  = unchecked((int)0x00040001);
        public const int S_NoUpdate                 = unchecked((int)0x00040002);
        public const int S_EndOfStream              = unchecked((int)0x00040003);
        public const int E_SampleAlloc              = unchecked((int)0x80040401);
        public const int E_PurposeId                = unchecked((int)0x80040402);
        public const int E_NoStream                 = unchecked((int)0x80040403);
        public const int E_NoSeeking                = unchecked((int)0x80040404);
        public const int E_Incompatible             = unchecked((int)0x80040405);
        public const int E_Busy                     = unchecked((int)0x80040406);
        public const int E_NotInit                  = unchecked((int)0x80040407);
        public const int E_SourceAlreadyDefined     = unchecked((int)0x80040408);
        public const int E_InvalidStreamType        = unchecked((int)0x80040409);
        public const int E_NotRunning               = unchecked((int)0x8004040a);
    }

    sealed public class MsError
    {
        private MsError()
        {
            // Prevent people from trying to instantiate this class
        }

        public static string GetErrorText(int hr)
        {
            string sRet = null;

            switch(hr)
            {
                case MsResults.S_Pending:
                    sRet = "Sample update is not yet complete.";
                    break;
                case MsResults.S_NoUpdate:
                    sRet = "Sample was not updated after forced completion.";
                    break;
                case MsResults.S_EndOfStream:
                    sRet = "End of stream. Sample not updated.";
                    break;
                case MsResults.E_SampleAlloc:
                    sRet = "An IMediaStream object could not be removed from an IMultiMediaStream object because it still contains at least one allocated sample.";
                    break;
                case MsResults.E_PurposeId:
                    sRet = "The specified purpose ID can't be used for the call.";
                    break;
                case MsResults.E_NoStream:
                    sRet = "No stream can be found with the specified attributes.";
                    break;
                case MsResults.E_NoSeeking:
                    sRet = "Seeking not supported for this IMultiMediaStream object.";
                    break;
                case MsResults.E_Incompatible:
                    sRet = "The stream formats are not compatible.";
                    break;
                case MsResults.E_Busy:
                    sRet = "The sample is busy.";
                    break;
                case MsResults.E_NotInit:
                    sRet = "The object can't accept the call because its initialize function or equivalent has not been called.";
                    break;
                case MsResults.E_SourceAlreadyDefined:
                    sRet = "Source already defined.";
                    break;
                case MsResults.E_InvalidStreamType:
                    sRet = "The stream type is not valid for this operation.";
                    break;
                case MsResults.E_NotRunning:
                    sRet = "The IMultiMediaStream object is not in running state.";
                    break;
                default:
                    sRet = DsError.GetErrorText(hr);
                    break;
            }

            return sRet;
        }

        /// <summary>
        /// If hr has a "failed" status code (E_*), throw an exception.  Note that status
        /// messages (S_*) are not considered failure codes.  If DES or DShow error text
        /// is available, it is used to build the exception, otherwise a generic com error
        /// is thrown.
        /// </summary>
        /// <param name="hr">The HRESULT to check</param>
        public static void ThrowExceptionForHR(int hr)
        {
            // If an error has occurred
            if (hr < 0)
            {
                // If a string is returned, build a com error from it
                string buf = GetErrorText(hr);

                if (buf != null)
                {
                    throw new COMException(buf, hr);
                }
                else
                {
                    // No string, just use standard com error
                    Marshal.ThrowExceptionForHR(hr);
                }
            }
        }
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
