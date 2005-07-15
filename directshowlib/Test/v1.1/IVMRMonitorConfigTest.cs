using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
  public class IVMRMonitorConfigTest
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr = null;
    private IVMRMonitorConfig vmrMonitorConfig = null;
    private VMRMonitorInfo[] monitorInfo;
  
    public IVMRMonitorConfigTest()
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
        Marshal.ReleaseComObject(vmr);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    public void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      vmr = (IBaseFilter) new VideoMixingRenderer();

      hr = graphBuilder.AddFilter(vmr, "VMR");
      DsError.ThrowExceptionForHR(hr);

      vmrMonitorConfig = (IVMRMonitorConfig) vmr;
    }

    public void TestGetAvailableMonitors()
    {
      int hr = 0;
      int devicesNumber = 0;

      hr = vmrMonitorConfig.GetAvailableMonitors(null, 0, out devicesNumber);
      DsError.ThrowExceptionForHR(hr);

      monitorInfo = new VMRMonitorInfo[devicesNumber];
      hr = vmrMonitorConfig.GetAvailableMonitors(monitorInfo, monitorInfo.Length, out devicesNumber);
      DsError.ThrowExceptionForHR(hr);

      // I just have 1 monitor connected to my PC so i can't test if this implementation is OK with 
      // an array greater than 1 item. It should but i can't be sure...
      Debug.Assert(hr == 0, "IVMRMonitorConfig.GetAvailableMonitors");
    }

    public void TestDefaultMonitor()
    {
      int hr = 0;
      VMRGuid vmrGuid;

      hr = vmrMonitorConfig.GetDefaultMonitor(out vmrGuid);
      DsError.ThrowExceptionForHR(hr);

      // Since we change nothing, this method should return a VMRGuid for default DD device
      // GUID = Guid.Empty & pGUID = null
      Debug.Assert((vmrGuid.GUID == Guid.Empty) && (vmrGuid.pGUID == IntPtr.Zero), "IVMRMonitorConfig.GetDefaultMonitor");

      if (monitorInfo.Length > 1)
      {
        // Get VMRGuid for the last monitor
        vmrGuid = monitorInfo[monitorInfo.Length - 1].guid;
      }
      else
      {
        // Get the defaut VMRGuid
        vmrGuid.GUID = Guid.Empty;
        vmrGuid.pGUID = IntPtr.Zero;
      }

      hr = vmrMonitorConfig.SetDefaultMonitor(ref vmrGuid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMonitorConfig.SetDefaultMonitor");
    }

    public void TestMonitor()
    {
      int hr = 0;
      VMRGuid vmrGuid;

      hr = vmrMonitorConfig.GetMonitor(out vmrGuid);
      DsError.ThrowExceptionForHR(hr);

      // Since we change nothing, this method should return a VMRGuid for default DD device
      // GUID = Guid.Empty & pGUID = null
      Debug.Assert((vmrGuid.GUID == Guid.Empty) && (vmrGuid.pGUID == IntPtr.Zero), "IVMRMonitorConfig.GetMonitor");

      if (monitorInfo.Length > 1)
      {
        // Get VMRGuid for the last monitor
        vmrGuid = monitorInfo[monitorInfo.Length - 1].guid;
      }
      else
      {
        // Get the defaut VMRGuid
        vmrGuid.GUID = Guid.Empty;
        vmrGuid.pGUID = IntPtr.Zero;
      }

      hr = vmrMonitorConfig.SetMonitor(ref vmrGuid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMonitorConfig.SetMonitor");
    }


  }
}
