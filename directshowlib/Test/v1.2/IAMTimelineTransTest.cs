using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IAMTimelineTransTest
    {
        private IAMTimeline m_pTimeline;
        IAMTimelineTrans m_itt;

        public IAMTimelineTransTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestCutPoint();
                TestCutPoint2();
                TestCutsOnly();
                TestSwapInputs();
            }
            finally
            {
                Marshal.ReleaseComObject(m_itt);
                Marshal.ReleaseComObject(m_pTimeline);
            }
        }

        private void TestCutPoint()
        {
            int hr;
            long l;

            hr = m_itt.SetCutPoint(5000);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itt.GetCutPoint(out l);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(l == 5000, "CutPoint");
        }

        private void TestCutPoint2()
        {
            int hr;
            double d;

            hr = m_itt.SetCutPoint2(500.456);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itt.GetCutPoint2(out d);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(d == 500.456, "CutPoint2");
        }

        private void TestCutsOnly()
        {
            int hr;
            bool b;

            hr = m_itt.SetCutsOnly(true);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itt.GetCutsOnly(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "CutsOnly");
        }

        private void TestSwapInputs()
        {
            int hr;
            bool b;

            hr = m_itt.SetSwapInputs(true);
            DESError.ThrowExceptionForHR(hr);

            hr = m_itt.GetSwapInputs(out b);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "SwapInputs");
        }

        private void Config()
        {
            m_pTimeline = (IAMTimeline)new AMTimeline();
            AddTrans();
        }

        private void AddTrans()
        {
            int hr;

            // create the timeline source object
            IAMTimelineObj pSource1Obj;
            hr = m_pTimeline.CreateEmptyNode( out pSource1Obj, TimelineMajorType.Transition);
            DESError.ThrowExceptionForHR(hr);

            hr = pSource1Obj.SetStartStop( 0, 10000000000 );
            DESError.ThrowExceptionForHR(hr);

            m_itt = pSource1Obj as IAMTimelineTrans;
        }

    }
}
