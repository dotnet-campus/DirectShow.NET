using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace v1._5
{
    public class IOverlayTest : IOverlayNotify
    {
        IOverlay m_io;

        public void DoTests()
        {
            Setup();

            TestAdvise();
            TestPalette();
        }

        private void TestPalette()
        {
            int hr;
            int i;
            IntPtr ip;

            hr = m_io.GetPalette(out i, out ip);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestAdvise()
        {
            int hr;

            hr = m_io.Advise(this, Advise.All2);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Setup()
        {
            //IBaseFilter ibf = new VideoRendererDefault() as IBaseFilter;
            //IBaseFilter ibf = new VideoMixingRenderer() as IBaseFilter;
            //IBaseFilter ibf = new VideoMixingRenderer9() as IBaseFilter;
            IBaseFilter ibf = new OverlayMixer() as IBaseFilter;
            
            IPin pPin;
            pPin = DsFindPin.ByDirection(ibf, PinDirection.Input, 0);
            m_io = pPin as IOverlay;
        }

        #region IOverlayNotify Members

        int IOverlayNotify.OnPaletteChange(int dwColors, IntPtr pPalette)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IOverlayNotify.OnClipChange(System.Drawing.Rectangle pSourceRect, System.Drawing.Rectangle pDestinationRect, RgnData pRgnData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IOverlayNotify.OnColorKeyChange(ColorKey pColorKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IOverlayNotify.OnPositionChange(System.Drawing.Rectangle pSourceRect, System.Drawing.Rectangle pDestinationRect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
