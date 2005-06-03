using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IAMVideoProcAmpTest
	{
    IFilterGraph2 graphBuilder = null;
    IAMVideoProcAmp videoProcAmp = null;
    
    public IAMVideoProcAmpTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();

      try
      {
        TestAllInOne();
      }
      finally
      {
        while(Marshal.ReleaseComObject(this.videoProcAmp) > 0);
        while(Marshal.ReleaseComObject(this.graphBuilder) > 0);
      }
    }

    public void TestAllInOne()
    {
      int hr = 0;
      int minValue, maxValue, defaultValue;
      int stepping;
      VideoProcAmpFlags caps;
      int newValue;
      VideoProcAmpFlags newCaps;

      //This test is very hardware dependent. Video ProcAmp Property tested can be changed here.
      VideoProcAmpProperty propertyToCheck = VideoProcAmpProperty.Saturation;

      // Get values for this property
      hr = this.videoProcAmp.GetRange(propertyToCheck, out minValue, out maxValue, out stepping, out defaultValue, out caps);
      DsError.ThrowExceptionForHR(hr);

      // try to increment this value
      hr = this.videoProcAmp.Set(propertyToCheck, defaultValue + stepping, VideoProcAmpFlags.Manual);
      DsError.ThrowExceptionForHR(hr);

      // Read this new value
      hr = this.videoProcAmp.Get(propertyToCheck,out newValue, out newCaps);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(newValue == defaultValue + stepping, "IAMVideoProcAmp.Get / Set");
      Debug.Assert(newCaps == VideoProcAmpFlags.Manual, "IAMVideoProcAmp.Get / Set");

      // try to set it as Auto
      hr = this.videoProcAmp.Set(propertyToCheck, defaultValue, VideoProcAmpFlags.Auto);
      DsError.ThrowExceptionForHR(hr);

      // Read this value
      hr = this.videoProcAmp.Get(propertyToCheck,out newValue, out newCaps);
      DsError.ThrowExceptionForHR(hr);

      // With my hardware, Contrast & Gamma can't be set to Auto
      // But Saturation works
      Debug.Assert(newCaps == VideoProcAmpFlags.Auto, "IAMVideoProcAmp.Get / Set");
    }

    void BuildGraph()
    {
      int hr = 0;
      IBaseFilter filter;

      this.graphBuilder = (IFilterGraph2) new FilterGraph();

      ArrayList devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
      DsDevice dev = devs[0] as DsDevice;

      hr = this.graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out filter);
      DsError.ThrowExceptionForHR(hr);

      this.videoProcAmp = (IAMVideoProcAmp) filter;
    }
	}
}
