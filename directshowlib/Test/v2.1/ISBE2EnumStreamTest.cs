using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;

namespace v2_1
{
    public class ISBE2EnumStreamTest
    {
        private ISBE2EnumStream m_es;

        public ISBE2EnumStreamTest()
        {
        }

        public void DoTests()
        {
            Config();

            TestEnum();
        }

        private void TestEnum()
        {
            int hr;
            ISBE2EnumStream es2;
            IntPtr ip = Marshal.AllocCoTaskMem(sizeof(int));

            SBE2_StreamDesc[] sd = new SBE2_StreamDesc[2];
            SBE2_StreamDesc[] sd2 = new SBE2_StreamDesc[1];

            hr = m_es.Reset();
            DsError.ThrowExceptionForHR(hr);

            hr = m_es.Clone(out es2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(es2 != null);

            hr = es2.Skip(1);
            DsError.ThrowExceptionForHR(hr);

            hr = es2.Next(1, sd2, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            hr = m_es.Next(2, sd, ip);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(Marshal.ReadInt32(ip) == 2);
            Debug.Assert(sd[1].StreamId == sd2[0].StreamId);
        }

        private void Config()
        {
            IFilterGraph2 fg;
            ISBE2Crossbar iSBE2Crossbar;

            fg = new FilterGraph() as IFilterGraph2;
            IBaseFilter streamBuffer = (IBaseFilter)new StreamBufferSource();
            int hr;

            hr = fg.AddFilter(streamBuffer, "SBS");
            DsError.ThrowExceptionForHR(hr);

            IFileSourceFilter fs = streamBuffer as IFileSourceFilter;
            hr = fs.Load(@"C:\Users\Public\Recorded TV\Sample Media\win7_scenic-demoshort_raw.wtv", null);
            DsError.ThrowExceptionForHR(hr);

            iSBE2Crossbar = streamBuffer as ISBE2Crossbar;
            hr = iSBE2Crossbar.EnableDefaultMode(CrossbarDefaultFlags.None);
            DsError.ThrowExceptionForHR(hr);

            IMediaControl mc = fg as IMediaControl;
            hr = mc.Run();
            DsError.ThrowExceptionForHR(hr);

            System.Threading.Thread.Sleep(10);

            hr = iSBE2Crossbar.EnumStreams(out m_es);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m_es != null);
        }
    }
}
