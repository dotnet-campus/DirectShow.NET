using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_IPSinkControlTest
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter bdaIPSink = null;
    private IBDA_IPSinkControl sinkControl = null;

		public IBDA_IPSinkControlTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();

      try
      {
        TestGetAdapterIPAddress();
        TestGetMulticastList();
      }
      finally
      {
        Marshal.ReleaseComObject(bdaIPSink);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();

      DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDARenderingFiltersCategory);

      foreach (DsDevice device in devices)
      {
        if (device.Name.Equals("BDA IP Sink"))
        {
          hr = graphBuilder.AddSourceFilterForMoniker(device.Mon, null, "BDA IP Sink", out bdaIPSink);
          DsError.ThrowExceptionForHR(hr);

          sinkControl = (IBDA_IPSinkControl) bdaIPSink;

          break;
        }
      }
    }

    private void TestGetAdapterIPAddress()
    {
      int hr = 0;
      int size = 0;
      IntPtr buffer = Marshal.AllocCoTaskMem(100);

      hr = sinkControl.GetAdapterIPAddress(out size, buffer);
      //DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == -2147023728, "IBDA_IPSinkControl.GetAdapterIPAddress");

      Marshal.FreeCoTaskMem(buffer);
    }

    private void TestGetMulticastList()
    {
      int hr = 0;
      int size = 0;
      IntPtr buffer = Marshal.AllocCoTaskMem(100);

      hr = sinkControl.GetMulticastList(out size, buffer);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_IPSinkControl.GetMulticastList");

      Marshal.FreeCoTaskMem(buffer);
    }

	}
}
