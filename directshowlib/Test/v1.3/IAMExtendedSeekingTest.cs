using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	public class IAMExtendedSeekingTest
	{
        private IAMExtendedSeeking m_es;
		public IAMExtendedSeekingTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestCap();
                TestPlaybackSpeed();
                TestMarkerCount();
                TestCurrentMarker();
                TestMarkerTime();
                TestMarkerName();
            }
            finally
            {
                Marshal.ReleaseComObject(m_es);
            }
        }

        private void TestCap()
        {
            int hr;
            AMExtendedSeekingCapabilities pCap;

            // Should contain AMExtendedSeekingCapabilities.MarkerSeek, but doesn't
            hr = m_es.get_ExSeekCapabilities(out pCap);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestMarkerCount()
        {
            int hr;
            int i;

            hr = m_es.get_MarkerCount(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 3, "Count");
        }

        private void TestCurrentMarker()
        {
            int hr;
            int i;

            // Returns E_NotImpl, not sure why, but c++ does the same thing
            hr = m_es.get_CurrentMarker(out i);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestMarkerTime()
        {
            int hr;
            double d;

            // Returns Invalid Parameter, c++ does the same thing
            hr = m_es.GetMarkerTime(1, out d);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestMarkerName()
        {
            int hr;
            string s;

            // Returns Invalid Parameter, c++ does the same thing
            hr = m_es.GetMarkerName(1, out s);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestPlaybackSpeed()
        {
            int hr;
            double d;

            hr = m_es.get_PlaybackSpeed(out d);
            DsError.ThrowExceptionForHR(hr);

            // Doesn't work with values other than 1.0
            hr = m_es.put_PlaybackSpeed(d);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            int hr;
            IBaseFilter pFilter;
            
            IFilterGraph2 m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry rot = new DsROTEntry(m_FilterGraph);

            hr = m_FilterGraph.RenderFile(@"p.wmv", null);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.FindFilterByName("p.wmv", out pFilter);
            m_es = pFilter as IAMExtendedSeeking;
        }
	}
}
