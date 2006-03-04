using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_PinControlTest
	{
    private IFilterGraph2 graphBuilder;
    private DsROTEntry rot;
    private IBDA_PinControl pinControl = null;
    
    public IBDA_PinControlTest()
		{
		}

    public void DoTests()
    {
      Config();

      try
      {
      }
      finally
      {
        rot.Dispose();
        if (pinControl != null) Marshal.ReleaseComObject(pinControl);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void Config()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      DsDevice[] devices;
      IBaseFilter filter;
      IPin pin;

      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);
//      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDARenderingFiltersCategory);
//      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);
//      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDANetworkProvidersCategory);

      foreach(DsDevice device in devices)
      {
        hr = graphBuilder.AddSourceFilterForMoniker(device.Mon, null, device.Name, out filter);
        DsError.ThrowExceptionForHR(hr);

        pin = DsFindPin.ByDirection(filter, PinDirection.Input, 0);
        if (pin is IBDA_PinControl)
        {
          pinControl = (IBDA_PinControl) pin;
          return;
        }
        Marshal.ReleaseComObject(pin);

        pin = DsFindPin.ByDirection(filter, PinDirection.Output, 0);
        if (pin is IBDA_PinControl)
        {
          pinControl = (IBDA_PinControl) pin;
          return;
        }
        Marshal.ReleaseComObject(pin);

        hr = graphBuilder.RemoveFilter(filter);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(filter);
      }

  }

}
}
