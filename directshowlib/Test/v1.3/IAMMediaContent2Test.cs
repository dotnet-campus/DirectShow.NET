using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IAMMediaContent2Test
    {
        IAMMediaContent2 m_imc2;
        IFilterGraph2 m_FilterGraph;

        public IAMMediaContent2Test()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestCount();
                TestParmName();
                TestParm();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_imc2);
            }
        }

        private void TestCount()
        {
            int hr;
            int iCount;

            hr = m_imc2.get_PlaylistCount(out iCount);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iCount == 1, "get_PlaylistCount");
        }

        private void TestParm()
        {
            int hr;
            string sName;

            hr = m_imc2.get_MediaParameter(0, "moo", out sName);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sName.StartsWith("foo"), "get_MediaParameter");
        }

        private void TestParmName()
        {
            int hr;
            string sName;

            hr = m_imc2.get_MediaParameterName(0, 1, out sName);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sName.StartsWith("Moo"), "get_MediaParameterName");
        }
                
        private void Configure()
        {
            int hr;
            IBaseFilter pFilter;
            m_FilterGraph = (IFilterGraph2)new FilterGraph();

            hr = m_FilterGraph.RenderFile(@"France-Info.asx", null);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.FindFilterByName("XML Playlist", out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = ((IMediaControl)m_FilterGraph).Run();
            DsError.ThrowExceptionForHR(hr);

            m_imc2 = (IAMMediaContent2)pFilter;
        }
    }
}
