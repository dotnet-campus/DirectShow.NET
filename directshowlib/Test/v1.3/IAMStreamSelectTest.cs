using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMStreamSelectTest
	{
        private IAMStreamSelect m_ss;
		public IAMStreamSelectTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestCount();
                TestEnable();
                TestInfo();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ss);
            }
        }

        private void TestInfo()
        {
            int hr;
            AMMediaType pmt;
            AMStreamSelectInfoFlags f;
            int id;
            int g;
            string name;
            object o1, o2;

            hr = m_ss.Info(0, out pmt, out f, out id, out g, out name, out o1, out o2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pmt != null, "Info");
        }

        private void TestCount()
        {
            int hr;
            int c;

            hr = m_ss.Count(out c);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(c == 2, "Count");
        }

        private void TestEnable()
        {
            int hr;

            hr = m_ss.Enable(0, AMStreamSelectEnableFlags.Enable);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            IGraphBuilder gb = (IGraphBuilder)new FilterGraph();
            IBaseFilter pFilter;

            int hr = gb.RenderFile(@"c:\nwn_1.mpg", null);
            hr = gb.FindFilterByName("MPEG-I Stream Splitter", out pFilter);

            m_ss = pFilter as IAMStreamSelect;

        }
	}
}
