using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRMonitorConfig9Test
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9 = null;
    private IVMRMonitorConfig9 vmrMonitorConfig = null;
    private VMR9MonitorInfo[] monitorInfo;
    
    public IVMRMonitorConfig9Test()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();
        TestGetAvailableMonitors();
        TestDefaultMonitor();
        TestMonitor();
      }
      finally
      {
        Marshal.ReleaseComObject(vmr9);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    public void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      vmr9 = (IBaseFilter) new VideoMixingRenderer9();

      hr = graphBuilder.AddFilter(vmr9, "VMR9");
      DsError.ThrowExceptionForHR(hr);

      vmrMonitorConfig = (IVMRMonitorConfig9) vmr9;
    }

    public void TestGetAvailableMonitors()
    {
      int hr = 0;
      int devicesNumber = 0;

      hr = vmrMonitorConfig.GetAvailableMonitors(null, 0, out devicesNumber);
      DsError.ThrowExceptionForHR(hr);

      monitorInfo = new VMR9MonitorInfo[devicesNumber];
      hr = vmrMonitorConfig.GetAvailableMonitors(monitorInfo, monitorInfo.Length, out devicesNumber);
      DsError.ThrowExceptionForHR(hr);

      // I just have 1 monitor connected to my PC so i can't test if this implementation is OK with 
      // an array greater than 1 item. It should but i can't be sure...
      Debug.Assert(hr == 0, "IVMRMonitorConfig9.GetAvailableMonitors");
    }

    public void TestDefaultMonitor()
    {
      int hr = 0;
      int monitor;

      hr = vmrMonitorConfig.GetDefaultMonitor(out monitor);
      DsError.ThrowExceptionForHR(hr);

      // Since we change nothing, this method should return the default monitor id
      Debug.Assert(monitor == 0, "IVMRMonitorConfig9.GetDefaultMonitor");

      if (monitorInfo.Length > 1)
      {
        // Get monitor id for the last monitor
        monitor = monitorInfo[monitorInfo.Length - 1].dwDeviceId;
      }
      else
      {
        // Get the defaut monitor id
        monitor = 0;
      }

      hr = vmrMonitorConfig.SetDefaultMonitor(monitor);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMonitorConfig9.SetDefaultMonitor");
    }

    public void TestMonitor()
    {
      int hr = 0;
      int monitor;

      hr = vmrMonitorConfig.GetMonitor(out monitor);
      DsError.ThrowExceptionForHR(hr);

      // Since we change nothing, this method should return the default monitor id
      Debug.Assert(monitor == 0, "IVMRMonitorConfig9.GetMonitor");

      if (monitorInfo.Length > 1)
      {
        // Get monitor id for the last monitor
        monitor = monitorInfo[monitorInfo.Length - 1].dwDeviceId;
      }
      else
      {
        // Get the defaut monitor id
        monitor = 0;
      }

      hr = vmrMonitorConfig.SetMonitor(monitor);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMonitorConfig9.SetMonitor");
    }
	}
}
