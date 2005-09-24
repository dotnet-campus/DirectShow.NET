using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class IMediaObjectTest
    {
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);

        IMediaObject m_imo;

        public IMediaObjectTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestGetStreamCount();

                TestInputType();
                TestOutputType();

                TestGetInputStreamInfo();
                TestGetInputSizeInfo();

                TestGetOutputStreamInfo();

                TestLatency();

                TestResources();
                TestFlush();
                TestDiscontinuity();
                TestGetInputStatus();

                TestProcess();
            }
            finally
            {
                Marshal.ReleaseComObject(m_imo);
            }
        }

        private void TestProcess()
        {
            int hr;
            int i;
            DMOOutputDataBuffer [] pOutBuf = new DMOOutputDataBuffer[1];

            hr = m_imo.AllocateStreamingResources();
            DMOError.ThrowExceptionForHR(hr);

            DoBuff d = new DoBuff(44100);
            d.SetLength(44100);

            hr = m_imo.ProcessInput(0, d, DMOInputDataBuffer.None, 0, 100);
            DMOError.ThrowExceptionForHR(hr);

            DoBuff d2 = new DoBuff(44100);
            pOutBuf[0].pBuffer = d2;

            hr = m_imo.ProcessOutput(DMOProcessOutput.None, 1, pOutBuf, out i);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestResources()
        {
            int hr;

            hr = m_imo.AllocateStreamingResources();
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.FreeStreamingResources();
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestFlush()
        {
            int hr;

            hr = m_imo.Flush();
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestDiscontinuity()
        {
            int hr;

            hr = m_imo.Discontinuity(0);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestGetInputStatus()
        {
            int hr;
            DMOInputStatusFlags f;

            hr = m_imo.GetInputStatus(0, out f);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(f == DMOInputStatusFlags.AcceptData, "GetInputStatus");
        }

        private void TestGetStreamCount()
        {
            int hr;
            int iIn, iOut;

            hr = m_imo.GetStreamCount(out iIn, out iOut);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(iIn == 1 && iOut == 1, "GetStreamCount");
        }

        private void TestInputType()
        {
            int hr;
            AMMediaType pmt = new AMMediaType();
            AMMediaType pmt2 = new AMMediaType();
            WaveFormatEx w = new WaveFormatEx();

            hr = m_imo.GetInputType(0, 0, pmt);
            Debug.WriteLine(DsToString.AMMediaTypeToString(pmt));

            Marshal.PtrToStructure(pmt.formatPtr, w);
            //pmt.sampleSize = 44100;
            //pmt.fixedSizeSamples = true;

            hr = m_imo.SetInputType(0, pmt, DMOSetType.Clear);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.SetInputType(0, pmt, DMOSetType.TestOnly);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.SetInputType(0, pmt, DMOSetType.None);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.GetInputCurrentType(0, pmt2);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(pmt2.majorType == pmt.majorType, "GetInputCurrentType");
        }

        private void TestOutputType()
        {
            int hr;
            AMMediaType pmt = new AMMediaType();
            AMMediaType pmt2 = new AMMediaType();
            WaveFormatEx w = new WaveFormatEx();

            hr = m_imo.GetOutputType(0, 0, pmt);
            Debug.WriteLine(DsToString.AMMediaTypeToString(pmt));

            Marshal.PtrToStructure(pmt.formatPtr, w);
            //pmt.sampleSize = 44100;
            //pmt.fixedSizeSamples = true;

            hr = m_imo.SetOutputType(0, pmt, DMOSetType.Clear);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.SetOutputType(0, pmt, DMOSetType.TestOnly);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.SetOutputType(0, pmt, DMOSetType.None);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.GetOutputCurrentType(0, pmt2);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(pmt2.majorType == pmt.majorType, "GetOutputCurrentType");
        }

        private void TestGetInputSizeInfo()
        {
            int hr;
            int iSize, iAlign, iLook;

            hr = m_imo.GetInputSizeInfo(0, out iSize, out iLook, out iAlign);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(iSize == 4 && iAlign == 1 && iLook == 0, "GetInputSizeInfo");
        }

        private void TestGetInputStreamInfo()
        {
            int hr;
            DMOInputStreamInfo f;

            hr = m_imo.GetInputStreamInfo(0, out f);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(f == DMOInputStreamInfo.None, "GetInputStreamInfo");
        }

        private void TestGetOutputStreamInfo()
        {
            int hr;
            int iSize, iAlign;

            hr = m_imo.GetOutputSizeInfo(0, out iSize, out iAlign);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(iSize == 4 && iAlign == 1, "GetOutputSizeInfo");
        }

        private void TestLatency()
        {
            int hr;
            long lat;

            hr = m_imo.SetInputMaxLatency(0, 100);
            Debug.Assert(hr == E_NOTIMPL, "Latency");

            hr = m_imo.GetInputMaxLatency(0, out lat);
            Debug.Assert(hr == E_NOTIMPL, "Latency");
        }

        private void TestLock()
        {
            int hr;

            hr = m_imo.Lock(false);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imo.Lock(true);
            DMOError.ThrowExceptionForHR(hr);
        }
#if false
        int ProcessOutput(
            DMOProcessOutput dwFlags, 
            int cOutputBufferCount, 
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] DMOOutputDataBuffer [] pOutputBuffers,
            out int pdwStatus
            );
#endif
        private void Configure()
        {
            int hr;

            DMOWrapperFilter dmoFilter = new DMOWrapperFilter();
            IDMOWrapperFilter dmoWrapperFilter = (IDMOWrapperFilter) dmoFilter;

            // Chorus - {efe6629c-81f7-4281-bd91-c9d604a95af6}
            // DmoFlip - {7EF28FD7-E88F-45bb-9CDD-8A62956F2D75}
            //hr = dmoWrapperFilter.Init(new Guid("{7EF28FD7-E88F-45bb-9CDD-8A62956F2D75}"), DMOCategory.AudioEffect);
            hr = dmoWrapperFilter.Init(new Guid("{efe6629c-81f7-4281-bd91-c9d604a95af6}"), DMOCategory.AudioEffect);
            DMOError.ThrowExceptionForHR(hr);

            m_imo = dmoWrapperFilter as IMediaObject;
        }
    }

    public class DoBuff : IMediaBuffer
    {
        IntPtr m_Buffer;
        int m_CurLength;
        int m_MaxLength;

        public DoBuff(int iBuffSize)
        {
            m_CurLength = 0;
            m_MaxLength = iBuffSize;
            m_Buffer = Marshal.AllocCoTaskMem(iBuffSize);
        }

        public int SetLength(int cbLength)
        {
            m_CurLength = cbLength;
            return 0;
        }

        public int GetBufferAndLength(out System.IntPtr ppBuffer, out int pcbLength)
        {
            ppBuffer = m_Buffer;
            pcbLength = m_CurLength;

            return 0;
        }

        public int GetMaxLength(out int pcbMaxLength)
        {
            pcbMaxLength = m_MaxLength;
            return 0;
        }

    }
}
