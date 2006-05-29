using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v1._5
{
    public class IBDA_SignalPropertiesTest
    {
        IBDA_SignalProperties m_sp;

        public void DoTests()
        {
            try
            {
                BuildGraph();

                TestNetworkType();
                TestSignalSource();
                TestTuningSpace();
            }
            finally
            {
                if (m_sp != null)
                {
                    Marshal.ReleaseComObject(m_sp);
                    m_sp = null;
                }
            }
        }

        private void TestNetworkType()
        {
            int hr;
            Guid g;

            hr = m_sp.PutNetworkType(MediaType.Video);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sp.GetNetworkType(out g);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaType.Video, "NetworkType");
        }

        private void TestSignalSource()
        {
            int hr;
            int i;

            hr = m_sp.PutSignalSource(3);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sp.GetSignalSource(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 3, "SignalSource");
        }

        private void TestTuningSpace()
        {
            int hr;
            Guid g;

            hr = m_sp.PutTuningSpace(MediaType.VBI);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sp.GetTuningSpace(out g);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaType.VBI, "TuningSpace");
        }

        private void BuildGraph()
        {
            m_sp = (IBDA_SignalProperties)new MPEG2Demultiplexer();
        }
    }
}

