using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferRecCompTest
    {
        private IStreamBufferRecComp m_sbrc;
        private string FileName = @"..\..\..\Resources\foo.dvr-ms";

        private const long NANOSECONDS = (1000000000);
        private const long UNITS = (NANOSECONDS / 100);

        public IStreamBufferRecCompTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestInit();
                TestAppend();
                TestAppendEx();
                TestSize();
                TestCancel();
                TestClose();
            }
            finally
            {
                Marshal.ReleaseComObject(m_sbrc);
            }
        }

        private void TestAppend()
        {
            int hr;

            hr = m_sbrc.Append(FileName);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestCancel()
        {
            int hr;

            hr = m_sbrc.Append(FileName);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sbrc.Cancel();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestClose()
        {
            int hr;

            hr = m_sbrc.Close();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestAppendEx()
        {
            int hr;

            // Make sure any previous operation is complete
            System.Threading.Thread.Sleep(1000);

            hr = m_sbrc.AppendEx(FileName, 1 * UNITS, 4 * UNITS);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestSize()
        {
            int hr;
            int pSec;

            hr = m_sbrc.GetCurrentLength(out pSec);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestInit()
        {
            int hr;
            string FileNameOut = "delme.dvr-ms";

            File.Delete(FileNameOut);

            hr = m_sbrc.Initialize(FileNameOut, FileName);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_sbrc = (IStreamBufferRecComp)new StreamBufferComposeRecording();
        }
    }
}
