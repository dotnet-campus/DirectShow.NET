using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRMixerControlTest
	{
    IFilterGraph2 graphBuilder = null;
    IBaseFilter vmr = null;
    IVMRMixerControl vmrMixerControl = null;
    
    public IVMRMixerControlTest()
		{
		}
    public void DoTests()
    {
      try
      {
        BuildGraph();

        TestAlpha();
        TestMixingPrefs();
        TestOutputRect();

        ConnectAStreamToVMR();

        TestBackgroundClr();
        TestZOrder();
      }
      finally
      {
        Marshal.ReleaseComObject(vmr);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    public void BuildGraph()
    {
      int hr  = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      vmr = (IBaseFilter) new VideoMixingRenderer();

      hr = (vmr as IVMRFilterConfig).SetNumberOfStreams(4);
      DsError.ThrowExceptionForHR(hr);

      vmrMixerControl = (IVMRMixerControl) vmr;

      hr = graphBuilder.AddFilter(vmr, "VMR");
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
      hr = vmrMixerControl.SetAlpha(0, 0.5f);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetAlpha(0, out alpha);
      DsError.ThrowExceptionForHR(hr);

      // try to set alpha of pin 3 to 75%
      hr = vmrMixerControl.SetAlpha(3, 0.75f);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetAlpha(3, out alpha);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(alpha == 0.75f, "IVMRMixerControl.GetAlpha / SetAlpha");
    }

    public void TestBackgroundClr()
    {
      int hr = 0;
      int colorToSet = ColorTranslator.ToWin32(Color.BlueViolet);
      int colorRead = 0;

      hr = vmrMixerControl.SetBackgroundClr(colorToSet);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetBackgroundClr(out colorRead);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(colorRead == colorToSet, "IVMRMixerControl.GetBackgroundClr / SetBackgroundClr");
    }

    public void TestMixingPrefs()
    {
      int hr = 0;
      // On my hardware, VMR7 with YUV surfaces doesn't seem to be stable.
      // When set, some other tests doesn't works : ZOrder & BackgroundClr
//      VMRMixerPrefs flagsToSet = VMRMixerPrefs.NoDecimation | VMRMixerPrefs.RenderTargetYUV | VMRMixerPrefs.BiLinearFiltering;
      VMRMixerPrefs flagsToSet = VMRMixerPrefs.NoDecimation | VMRMixerPrefs.RenderTargetRGB | VMRMixerPrefs.BiLinearFiltering;
      VMRMixerPrefs flagsRead = VMRMixerPrefs.None;

      hr = vmrMixerControl.SetMixingPrefs(flagsToSet);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetMixingPrefs(out flagsRead);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(flagsRead == flagsToSet, "IVMRMixerControl.GetMixingPrefs / SetMixingPrefs");
    }

    public void TestOutputRect()
    {
      int hr = 0;
      NormalizedRect rect1 = new NormalizedRect(0.0f, 0.0f, 0.5f, 0.5f);
      NormalizedRect rect2 = new NormalizedRect();

      // Try to configure stream 0 to use upper left quarter screen
      hr = vmrMixerControl.SetOutputRect(0, ref rect1);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetOutputRect(0, out rect2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(rect1 == rect2, "IVMRMixerControl.GetOutputRect / SetOutputRect");

      rect1 = new NormalizedRect(0.5f, 0.5f, 1.0f, 1.0f);

      // Try to configure stream 3 to use lower right quarter screen
      hr = vmrMixerControl.SetOutputRect(3, ref rect1);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetOutputRect(3, out rect2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(rect1 == rect2, "IVMRMixerControl.GetOutputRect / SetOutputRect");
    }

    public void TestZOrder()
    {
      int hr = 0;
      int zValue = 0;

      // Try to change Z order of the stream 0
      hr = vmrMixerControl.SetZOrder(0, 10);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetZOrder(0, out zValue);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(zValue == 10, "IVMRMixerControl.GetZOrder / SetZOrder");

      // Try to change Z order of the stream 3
      hr = vmrMixerControl.SetZOrder(3, 100);
      DsError.ThrowExceptionForHR(hr);

      hr = vmrMixerControl.GetZOrder(3, out zValue);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(zValue == 100, "IVMRMixerControl.GetZOrder / SetZOrder");
    }

	}
}
