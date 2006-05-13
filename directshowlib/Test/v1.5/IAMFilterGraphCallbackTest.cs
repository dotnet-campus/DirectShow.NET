using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace v1._5
{
    public class IAMFilterGraphCallbackTest : IAMFilterGraphCallback
	{
        public void DoTests()
        {
            Config();

            try
            {
            }
            finally
            {
            }
        }

        private void Config()
        {
            int hr;
            IBaseFilter pFilter;
            IFilterGraph2 ifg = new FilterGraph() as IFilterGraph2;

            IObjectWithSite iows = ifg as IObjectWithSite;
            //hr = iows.SetSite(this);

            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = ifg.AddSourceFilterForMoniker(devs[0].Mon, null, "asdf", out pFilter);
            IPin pPin = DsFindPin.ByDirection(pFilter, PinDirection.Output, 1);
            hr = ((IGraphBuilder)ifg).Render(pPin);
            DsError.ThrowExceptionForHR(hr);
        }

        #region IAMFilterGraphCallback Members

        public int UnableToRender(IPin pPin)
        {
            Debug.WriteLine("asdf");
            return 0;
        }

        #endregion
    }
}
