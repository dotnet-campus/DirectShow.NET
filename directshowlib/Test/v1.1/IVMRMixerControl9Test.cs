using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
  public class IVMRMixerControl9Test
  {
    IFilterGraph2 graphBuilder = null;
    IBaseFilter vmr9 = null;
    IVMRMixerControl9 vmr9MixerControl = null;
    VMR9ProcAmpControlRange brightnessRange = new VMR9ProcAmpControlRange();

    public IVMRMixerControl9Test()
    {
    }
    
    public void DoTests()
    {
      try
      {
        BuildGraph();

        TestAlpha();
        TestBackgroundClr();
        TestMixingPrefs();
        TestOutputRect();

        ConnectAStreamToVMR();

        TestGetProcAmpControlRange();
        TestProcAmpControl();
        TestZOrder();
      }
      finally
      {
        Marshal.ReleaseComObject(vmr9);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    public void BuildGraph()
    {
      int hr  = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      vmr9 = (IBaseFilter) new VideoMixingRenderer9();

      hr = (vmr9 as IVMRFilterConfig9).SetNumberOfStreams(4);
      DsError.ThrowExceptionForHR(hr);

      vmr9MixerControl = (IVMRMixerControl9) vmr9;

      hr = graphBuilder.AddFilter(vmr9, "VMR9");
      DsError.ThrowExceptionForHR(hr);
    }

    public void ConnectAStreamToVMR()
    {
      int hr = 0;

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);
    }

    public void TestAlpha()
    {
      int hr = 0;
      float alpha = 0.0f;

      // try to set alpha of pin 0 to 50%
      hr = vmr9MixerControl.SetAlpha(0, 0.5f);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetAlpha(0, out alpha);
      DsError.ThrowExceptionForHR(hr);

      // try to set alpha of pin 3 to 75%
      hr = vmr9MixerControl.SetAlpha(3, 0.75f);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetAlpha(3, out alpha);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(alpha == 0.75f, "IVMRMixerControl9.GetAlpha / SetAlpha");
    }

    public void TestBackgroundClr()
    {
      int hr = 0;
      int colorToSet = ColorTranslator.ToWin32(Color.BlueViolet);
      int colorRead = 0;

      hr = vmr9MixerControl.SetBackgroundClr(colorToSet);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetBackgroundClr(out colorRead);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(colorRead == colorToSet, "IVMRMixerControl9.GetBackgroundClr / SetBackgroundClr");
    }

    public void TestMixingPrefs()
    {
      int hr = 0;
      VMR9MixerPrefs flagsToSet = VMR9MixerPrefs.NoDecimation | VMR9MixerPrefs.RenderTargetYUV | VMR9MixerPrefs.BiLinearFiltering;
      VMR9MixerPrefs flagsRead = VMR9MixerPrefs.None;

      hr = vmr9MixerControl.SetMixingPrefs(flagsToSet);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetMixingPrefs(out flagsRead);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(flagsRead == flagsToSet, "IVMRMixerControl9.GetMixingPrefs / SetMixingPrefs");
    }

    public void TestOutputRect()
    {
      int hr = 0;
      NormalizedRect rect1 = new NormalizedRect(0.0f, 0.0f, 0.5f, 0.5f);
      NormalizedRect rect2 = new NormalizedRect();

      // Try to configure stream 0 to use upper left quarter screen
      hr = vmr9MixerControl.SetOutputRect(0, ref rect1);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetOutputRect(0, out rect2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(rect1 == rect2, "IVMRMixerControl9.GetOutputRect / SetOutputRect");

      rect1 = new NormalizedRect(0.5f, 0.5f, 1.0f, 1.0f);

      // Try to configure stream 3 to use lower right quarter screen
      hr = vmr9MixerControl.SetOutputRect(3, ref rect1);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetOutputRect(3, out rect2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(rect1 == rect2, "IVMRMixerControl9.GetOutputRect / SetOutputRect");
    }

    public void TestGetProcAmpControlRange()
    {
      int hr = 0;

      // Get the brightness ranges for the next test
      brightnessRange.dwSize = Marshal.SizeOf(typeof(VMR9ProcAmpControlRange));
      brightnessRange.dwProperty = VMR9ProcAmpControlFlags.Brightness;

      hr = vmr9MixerControl.GetProcAmpControlRange(0, ref brightnessRange);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMixerControl9.GetProcAmpControlRange");
    }

    public void TestProcAmpControl()
    {
      int hr = 0;

      // Assuming the preceding test have works, Trying to set brightness to maximum value
      VMR9ProcAmpControl procAmpControl1 = new VMR9ProcAmpControl();
      procAmpControl1.dwSize = Marshal.SizeOf(typeof(VMR9ProcAmpControl));
      procAmpControl1.dwFlags = VMR9ProcAmpControlFlags.Brightness;
      procAmpControl1.Brightness = brightnessRange.MaxValue;

      hr = vmr9MixerControl.SetProcAmpControl(0, ref procAmpControl1);
      // A VFW_E_VMR_NO_PROCAMP_HW can be return for old devices
      DsError.ThrowExceptionForHR(hr);

      VMR9ProcAmpControl procAmpControl2 = new VMR9ProcAmpControl();
      procAmpControl2.dwSize = Marshal.SizeOf(typeof(VMR9ProcAmpControl));

      hr = vmr9MixerControl.GetProcAmpControl(0, ref procAmpControl2);
      // A VFW_E_VMR_NO_PROCAMP_HW can be return for old devices
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(procAmpControl2.Brightness == brightnessRange.MaxValue, "IVMRMixerControl9.GetProcAmpControl / SetProcAmpControl");
    }

    public void TestZOrder()
    {
      int hr = 0;
      int zValue = 0;

      // Try to change Z order of the stream 0
      hr = vmr9MixerControl.SetZOrder(0, 10);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetZOrder(0, out zValue);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(zValue == 10, "IVMRMixerControl9.GetZOrder / SetZOrder");

      // Try to change Z order of the stream 3
      hr = vmr9MixerControl.SetZOrder(3, 100);
      DsError.ThrowExceptionForHR(hr);

      hr = vmr9MixerControl.GetZOrder(3, out zValue);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(zValue == 100, "IVMRMixerControl9.GetZOrder / SetZOrder");
    }


  }
}
