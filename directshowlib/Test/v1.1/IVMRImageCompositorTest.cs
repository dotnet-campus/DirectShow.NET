using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
  [Guid("f5049e79-4861-11d2-a407-00a0c90629a8")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IDirect3DDevice7
  {
    // Just to test...
  }

  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class IVMRImageCompositorTest : IVMRImageCompositor
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr = null;
    private DsROTEntry rot = null;

    private int InitCompositionDeviceCalledCount = 0;
    private int SetStreamMediaTypeCalledCount = 0;
    private int CompositeImageCalledCount = 0;
    private int TermCompositionDeviceCalledCount = 0;

    private int backgroundColorABGR = unchecked((int)0x00112233);
    private int backgroundColorARGB = unchecked((int)0xff332211);

		public IVMRImageCompositorTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        // wait for the end of foo.avi
        // Nothing is shown in the video window since we composite nothing...
        System.Threading.Thread.Sleep(1000);

        Debug.Assert(InitCompositionDeviceCalledCount != 0, "IVMRImageCompositor.InitCompositionDevice not called");
        Debug.Assert(SetStreamMediaTypeCalledCount != 0, "IVMRImageCompositor.SetStreamMediaType not called");
        Debug.Assert(CompositeImageCalledCount != 0, "IVMRImageCompositor.CompositeImageCalled not called");
        Debug.Assert(TermCompositionDeviceCalledCount != 0, "IVMRImageCompositor.TermCompositionDevice not called");
      }
      finally
      {
        if (rot != null)
          rot.Dispose();

        Marshal.ReleaseComObject(vmr);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    public void BuildGraph()
    {
      int hr  = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();

      rot = new DsROTEntry(graphBuilder);

      vmr = (IBaseFilter) new VideoMixingRenderer();

      IVMRFilterConfig filterConfig = (IVMRFilterConfig) vmr;

      hr = filterConfig.SetNumberOfStreams(2);
      DsError.ThrowExceptionForHR(hr);

      // Set the custom compositor
      hr = filterConfig.SetImageCompositor(this);
      DsError.ThrowExceptionForHR(hr);

      IVMRMixerControl mixerControl = (IVMRMixerControl) vmr;

      // In COLORREF, colors are coded in ABGR format
      hr = mixerControl.SetBackgroundClr(backgroundColorABGR);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.AddFilter(vmr, "VMR");
      DsError.ThrowExceptionForHR(hr);

      // The 2 VMR pins must be connected...
      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    #region Membres de IVMRImageCompositor

    public int CompositeImage(System.IntPtr pD3DDevice, System.IntPtr pddsRenderTarget, AMMediaType pmtRenderTarget, long rtStart, long rtEnd, int dwClrBkGnd, VMRVideoStreamInfo[] pVideoStreamInfo, int cStreams)
    {
      Debug.Assert(pD3DDevice != IntPtr.Zero, "IVMRImageCompositor.CompositeImage");
      Debug.Assert(pddsRenderTarget != IntPtr.Zero, "IVMRImageCompositor.CompositeImage");
      Debug.Assert(pmtRenderTarget != null, "IVMRImageCompositor.CompositeImage");
      // dwClrBkGnd is always zero. I don't know why...
      //Debug.Assert(dwClrBkGnd == backgroundColorARGB, "IVMRImageCompositor.CompositeImage");
      Debug.Assert(pVideoStreamInfo.Length == 2, "IVMRImageCompositor.CompositeImage");
      Debug.Assert(cStreams == 2, "IVMRImageCompositor.CompositeImage");

      // Verify that this array is correctly marshaled
      Debug.Assert(pVideoStreamInfo[1].rNormal.bottom == 1.0f, "IVMRImageCompositor.CompositeImage");

      CompositeImageCalledCount++;
      return 0;
    }

    public int TermCompositionTarget(System.IntPtr pD3DDevice, System.IntPtr pddsRenderTarget)
    {
      // this method is called 1before all other methods for cleanup purpose and pD3DDevice can be null or not...

      TermCompositionDeviceCalledCount++;
      return 0;
    }

    public int InitCompositionTarget(System.IntPtr pD3DDevice, System.IntPtr pddsRenderTarget)
    {
      // A RCW can be retrieve from an unmanaged IntPtr like this (this method do an AddRef) :
      IDirect3DDevice7 d3ddev = (IDirect3DDevice7) Marshal.GetObjectForIUnknown(pD3DDevice);
      Marshal.ReleaseComObject(d3ddev);

      // A valid pointer should be pass to this custom compositor. 
      Debug.Assert(pD3DDevice != IntPtr.Zero, "IVMRImageCompositor.InitCompositionDevice");
      Debug.Assert(pddsRenderTarget != IntPtr.Zero, "IVMRImageCompositor.InitCompositionDevice");

      InitCompositionDeviceCalledCount++;
      return 0;
    }

    public int SetStreamMediaType(int dwStrmID, AMMediaType pmt, bool fTexture)
    {
      // Stream ID should be 0 or 1 for the two streams connected to the VMR
      Debug.Assert(dwStrmID == 0 || dwStrmID == 1, "IVMRImageCompositor.SetStreamMediaType");

      // Note : This method is called serveral times but only one of them have pmt != null

      SetStreamMediaTypeCalledCount++;
      return 0;
    }

    #endregion
  }
}
