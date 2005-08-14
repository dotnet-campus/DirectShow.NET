using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IMixerPinConfig2Test
    {
        IMixerPinConfig2 m_imcPrimary;
        IFilterGraph2 m_FilterGraph;

        public IMixerPinConfig2Test()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestControl();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                Marshal.ReleaseComObject(m_imcPrimary);
            }
        }

        private void TestControl()
        {
            int hr;

            DDColorControl p = new DDColorControl();
            p.dwSize = Marshal.SizeOf(typeof(DDColorControl));

            hr = m_imcPrimary.GetOverlaySurfaceColorControls(p);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imcPrimary.SetOverlaySurfaceColorControls(p);
            DsError.ThrowExceptionForHR(hr);
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
            m_imcPrimary = (IMixerPinConfig2)iPin;

            Marshal.ReleaseComObject(ibf);
        }
    }
}
