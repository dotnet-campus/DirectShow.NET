using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_DeviceControlTest
	{
    private IFilterGraph2 graphBuilder;
    private DsROTEntry rot;
    private IBaseFilter bdaTuner;
    private IBDA_DeviceControl deviceControl = null;
    
    public IBDA_DeviceControlTest()
		{
		}

    public void DoTests()
    {
      Config();

      try
      {
        Tests();
      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(bdaTuner);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void Config()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      // Assume that the first device in this category IS a BDA device...
      DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

      hr = graphBuilder.AddSourceFilterForMoniker(devices[0].Mon, null, devices[0].Name, out bdaTuner);
      DsError.ThrowExceptionForHR(hr);

      deviceControl = (IBDA_DeviceControl) bdaTuner;
    }

    private void Tests()
    {
      int hr = 0;
      BDAChangeState state;

      hr = deviceControl.CheckChanges();
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_DeviceControl.CheckChanges");

      hr = deviceControl.CommitChanges();
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_DeviceControl.CommitChanges");

      hr = deviceControl.StartChanges();
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_DeviceControl.StartChanges");

      hr = deviceControl.GetChangeState(out state);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_DeviceControl.GetChangeState");
    }

	}
}
