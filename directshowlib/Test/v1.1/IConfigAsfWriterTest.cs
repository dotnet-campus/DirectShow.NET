using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IConfigAsfWriterTest
    {
        IConfigAsfWriter m_asfw;
        IFilterGraph2 m_FilterGraph;

        // This is a hacked and partial version of IWMProfile
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
            Guid("96406BDB-2B2B-11D3-B36B-00C04F6108FF")]
            public interface IWMProfile
        {
            [PreserveSig]
            int GetVersion(
                out int pdwVersion
                );

            [PreserveSig]
            int GetName(
                [Out] StringBuilder pwszName,
                ref int pcchName
                );
        }

        public IConfigAsfWriterTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestIndex();
                TestGuid();
                TestID();
                TestProfile();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestGuid()
        {
            int hr;

            // Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)
            Guid cat = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);
            Guid o;

            hr = m_asfw.ConfigureFilterUsingProfileGuid(cat);
            DsError.ThrowExceptionForHR(hr);

            hr = m_asfw.GetCurrentProfileGuid(out o);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(o == cat, "ProfileGuid");
        }

        private void TestIndex()
        {
            int hr;
            bool bIndex, bIndex2;

            hr = m_asfw.GetIndexMode(out bIndex);
            DsError.ThrowExceptionForHR(hr);

            hr = m_asfw.SetIndexMode(!bIndex);
            DsError.ThrowExceptionForHR(hr);

            hr = m_asfw.GetIndexMode(out bIndex2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(bIndex != bIndex2, "IndexMode");
        }

        private void TestID()
        {
            int hr;
            int id;

            hr = m_asfw.ConfigureFilterUsingProfileId(1);
            DsError.ThrowExceptionForHR(hr);

            hr = m_asfw.GetCurrentProfileId(out id);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(id == 1, "ProfileID");
        }

        private void TestProfile()
        {
            int hr;
            IntPtr prof;

            hr = m_asfw.GetCurrentProfile(out prof);
            DsError.ThrowExceptionForHR(hr);

#if false
            IWMProfile ipr = o as IWMProfile;
            int s = 255;
            StringBuilder sb = new StringBuilder(s, s);
            ipr.GetName(sb, ref s);
            Debug.Assert(sb.ToString() == "Intranet - High Speed LAN Multiple Bit Rate Video", "GetProfile");
#endif

#if false

            IntPtr ip = Marshal.GetIUnknownForObject(o);
            IntPtr ip2;
            Guid g = typeof(IWMProfile).GUID;
            Marshal.QueryInterface(ip, ref g, out ip2);
#endif

            hr = m_asfw.ConfigureFilterUsingProfile(prof);
            DsError.ThrowExceptionForHR(hr);

#if false
            Guid g2 = new Guid("00000000-0000-0000-C000-000000000046");
            IntPtr ip3;
            Marshal.QueryInterface(ip, ref g2, out ip3);

            hr = m_asfw.ConfigureFilterUsingProfile(ip3);
            DsError.ThrowExceptionForHR(hr);
            Marshal.Release(ip3);
#endif
#if false

            Marshal.Release(ip);
            Marshal.Release(ip2);
#endif
        }

        private void Configure()
        {
            int hr;

            m_FilterGraph = (IFilterGraph2)new FilterGraph();

            m_asfw = (IConfigAsfWriter)new WMAsfWriter();
            hr = m_FilterGraph.AddFilter((IBaseFilter)m_asfw, "asdf");
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
