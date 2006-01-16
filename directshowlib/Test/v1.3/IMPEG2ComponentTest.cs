using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IMPEG2ComponentTest
    {
        private IMPEG2Component m_m2c = null;

        public IMPEG2ComponentTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestPID();
                TestPCRPID();
                TestProgramNumber();
            }
            finally
            {
                Marshal.ReleaseComObject(m_m2c);
            }
        }

        private void TestPID()
        {
            int hr;
            int pid;

            hr = m_m2c.put_PID(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_m2c.get_PID(out pid);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pid == 123, "PID");
        }

        private void TestPCRPID()
        {
            int hr;
            int pcrpid;

            hr = m_m2c.put_PCRPID(321);
            DsError.ThrowExceptionForHR(hr);

            hr = m_m2c.get_PCRPID(out pcrpid);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pcrpid == 321, "PCRPID");
        }

        private void TestProgramNumber()
        {
            int hr;
            int pn;

            hr = m_m2c.put_ProgramNumber(456);
            DsError.ThrowExceptionForHR(hr);

            hr = m_m2c.get_ProgramNumber(out pn);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pn == 456, "ProgramNumber");
        }

        private void Config()
        {
            m_m2c = (IMPEG2Component) new MPEG2Component();
        }
    }
}
