using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IMixerPinConfigTest
    {
        IMixerPinConfig m_imcPrimary;
        IFilterGraph2 m_FilterGraph;

        public IMixerPinConfigTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestColorKey();
                TestBlending();
                TestZOrder();
                TestAspectRatio();
                TestTransparent();
                TestPosition();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_imcPrimary);
            }
        }

        private void TestColorKey()
        {
            int hr;
            ColorKey j = new ColorKey();
            int c;

            hr = m_imcPrimary.GetColorKey(j, out c);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imcPrimary.SetColorKey(j);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestBlending()
        {
            int hr;
            int bp;

            hr = m_imcPrimary.GetBlendingParameter(out bp);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imcPrimary.SetBlendingParameter(1);
            //DsError.ThrowExceptionForHR(hr); not implemented
        }

        private void TestZOrder()
        {
            int hr;
            int bp;

            hr = m_imcPrimary.SetZOrder(1);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imcPrimary.GetZOrder(out bp);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(bp == 1, "SetZOrder");
        }

        private void TestAspectRatio()
        {
            int hr;
            AspectRatioMode bp;

            hr = m_imcPrimary.SetAspectRatioMode(AspectRatioMode.LetterBox);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imcPrimary.GetAspectRatioMode(out bp);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(bp == AspectRatioMode.LetterBox, "AspectRatio");
        }

        private void TestTransparent()
        {
            int hr;
            bool bp;

            hr = m_imcPrimary.GetStreamTransparent(out bp);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(bp == false, "Transparent");

            hr = m_imcPrimary.SetStreamTransparent(true);
            //DsError.ThrowExceptionForHR(hr); not implemented
        }

        private void TestPosition()
        {
            int hr;
            int l, t, r, b;

            hr = m_imcPrimary.SetRelativePosition(1, 2, 3, 4);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imcPrimary.GetRelativePosition(out l, out t, out r, out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l == 1 && t == 2 && r == 3 && b == 4, "RelativePosition");
        }

        private void Configure()
        {
            int hr;

            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry rot = new DsROTEntry(m_FilterGraph);
            IBaseFilter ibf = (IBaseFilter)new OverlayMixer();

            hr = m_FilterGraph.AddFilter((IBaseFilter)ibf, "asdf");
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.RenderFile("foo.avi", null);
            DsError.ThrowExceptionForHR(hr);

            IPin iPin = DsFindPin.ByDirection(ibf, PinDirection.Input, 0);
            m_imcPrimary = (IMixerPinConfig)iPin;

            Marshal.ReleaseComObject(ibf);
        }
    }
}
