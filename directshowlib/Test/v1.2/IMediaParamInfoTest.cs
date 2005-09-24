// out char [] - "Safe array of rank 87 has been passed to a method expecting an array of rank 1."
// out string - "Object reference not set to an instance of an object."
// ref string - "Object reference not set to an instance of an object."
// ref StringBuilder - only returns up to first null
// [MarshalAs(UnmanagedType.LPArray)] out char [] - returns 1 char

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class IMediaParamInfoTest
    {
        IMediaParamInfo m_impi;

        public IMediaParamInfoTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestGetParamCount();
                TestParamText();
                TestTimeFormats();
                TestParamInfo();
            }
            finally
            {
                Marshal.ReleaseComObject(m_impi);
            }
        }

        private void TestParamInfo()
        {
            int hr;
            ParamInfo pInfo = new ParamInfo();

            hr = m_impi.GetParamInfo(0, out pInfo);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(pInfo.szUnitText == "%", "GetParamInfo");
        }

        private void TestParamText()
        {
            int hr = 0;
            IntPtr ip;
            string sName, sUnits;
            string [] sEnum;

            for (int x=0; x < 7; x++)
            {
                hr = m_impi.GetParamText(x, out ip);
                DMOError.ThrowExceptionForHR(hr);

                ParseParamText(ip, out sName, out sUnits, out sEnum);
                Debug.WriteLine(string.Format("{0} {1} {2}", sName, sUnits, sEnum.Length));
                Marshal.FreeCoTaskMem(ip);
            }
        }

        private void ParseParamText(IntPtr ip, out string ParamName, out string ParamUnits, out string [] ParamEnum)
        {
            int iCount = 0;

            ParamName = Marshal.PtrToStringUni(ip);
            ip = (IntPtr)(ip.ToInt32() + ((ParamName.Length + 1) * 2));
            ParamUnits = Marshal.PtrToStringUni(ip);
            ip = (IntPtr)(ip.ToInt32() + ((ParamUnits.Length + 1) * 2));

            IntPtr ip2 = ip;
            while (Marshal.ReadInt16(ip2) != 0)
            {
                string s = Marshal.PtrToStringUni(ip2);
                ip2 = (IntPtr)(ip2.ToInt32() + ((s.Length + 1) * 2));
                iCount++;
            }
            ParamEnum = new string[iCount];
            for(int x=0; x < iCount; x++)
            {
                ParamEnum[x] = Marshal.PtrToStringUni(ip);
                ip = (IntPtr)(ip.ToInt32() + ((ParamEnum[x].Length + 1) * 2));
            }
        }

        private void TestGetParamCount()
        {
            int hr;
            int i;

            hr = m_impi.GetParamCount(out i);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 7, "GetParamCount");
        }

        private void TestTimeFormats()
        {
            int hr;
            int i;
            Guid g;
            Guid g2;
            int t;

            hr = m_impi.GetNumTimeFormats(out i);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1, "GetNumTimeFormats");

            hr = m_impi.GetCurrentTimeFormat(out g, out t);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaParamTimeFormat.Reference, "GetCurrentTimeFormat");

            for (int x=0; x < i; x++)
            {
                hr = m_impi.GetSupportedTimeFormat(x, out g2);
                DMOError.ThrowExceptionForHR(hr);

                Debug.Assert(g2 == MediaParamTimeFormat.Reference, "GetSupportedTimeFormat");
            }
        }
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

            m_impi = dmoWrapperFilter as IMediaParamInfo;
        }
        private void junk()
        {
            int hr;
            IEnumDMO idmo;
            Guid [] g = new Guid[1];
            string [] sn = new string[1];
            int i;
            int iCount = 0;

            hr = DMOUtils.DMOEnum(Guid.Empty, DMOEnumerator.None, 0, null, 0, null, out idmo);
            DMOError.ThrowExceptionForHR(hr);

            do
            {
                DMOWrapperFilter dmoFilter = new DMOWrapperFilter();
                IDMOWrapperFilter dmoWrapperFilter = (IDMOWrapperFilter) dmoFilter;

                hr = idmo.Next(1, g, sn, out i);
                DMOError.ThrowExceptionForHR(hr);

                if (hr > 0)
                    break;

                hr = dmoWrapperFilter.Init(g[0], Guid.Empty);

                if (hr >= 0)
                {
                    m_impi = dmoWrapperFilter as IMediaParamInfo;
                    if (m_impi != null)
                    {
                        hr = m_impi.GetParamCount(out iCount);
                    }
                    else
                    {
                        iCount = 0;
                    }

                    if (iCount > 0)
                        Debug.WriteLine(string.Format("{0} {1} {2}", sn[0], iCount, g[0]));
                }

            } while (iCount >= 0);
        }
    }
}
