using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
    public class IAMAudioRendererStatsTest
    {
        IAMAudioRendererStats m_ibn;
        IMediaControl m_imc;

        public void DoTests()
        {
            Setup();

            TestEm();
        }

        private void TestEm()
        {
            int hr;
            int p1, p2;

            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);

            System.Threading.Thread.Sleep(3000);

            hr = m_ibn.GetStatParam(AMAudioRendererStatParam.SlaveMode, out p1, out p2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(p1 == 5, "see if we got something");
        }

        private void Setup()
        {
            int hr;

            IFilterGraph2 ifg = new FilterGraph() as IFilterGraph2;
            m_imc = ifg as IMediaControl;
            DsROTEntry rot = new DsROTEntry(ifg);
            IBaseFilter pFilter;

            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);

            hr = ifg.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            ICaptureGraphBuilder2 icgb2 = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = icgb2.SetFiltergraph(ifg);
            DsError.ThrowExceptionForHR(hr);

            //IPin pPin = DsFindPin.ByDirection((IBaseFilter)o, PinDirection.Output, 0);
            hr = icgb2.RenderStream(null, MediaType.Audio, pFilter, null, null);
            DsError.ThrowExceptionForHR(hr);

            IBaseFilter pAudio;
            hr = ifg.FindFilterByName("Audio Renderer", out pAudio);

            m_ibn = pAudio as IAMAudioRendererStats;
        }
    }
}
