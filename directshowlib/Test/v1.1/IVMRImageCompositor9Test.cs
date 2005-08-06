using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class IVMRImageCompositor9Test : IVMRImageCompositor9
  {
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9 = null;
    private DsROTEntry rot = null;

    private int InitCompositionDeviceCalledCount = 0;
    private int SetStreamMediaTypeCalledCount = 0;
    private int CompositeImageCalledCount = 0;
    private int TermCompositionDeviceCalledCount = 0;

    private int backgroundColorABGR = unchecked((int)0xff112233);
    private int backgroundColorARGB = unchecked((int)0xff332211);

    public IVMRImageCompositor9Test()
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

        Debug.Assert(InitCompositionDeviceCalledCount != 0, "IVMRImageCompositor9.InitCompositionDevice not called");
        Debug.Assert(SetStreamMediaTypeCalledCount != 0, "IVMRImageCompositor9.SetStreamMediaType not called");
        Debug.Assert(CompositeImageCalledCount != 0, "IVMRImageCompositor9.CompositeImageCalled not called");
        Debug.Assert(TermCompositionDeviceCalledCount != 0, "IVMRImageCompositor9.TermCompositionDevice not called");
      }
      finally
      {
        if (rot != null)
          rot.Dispose();

        Marshal.ReleaseComObject(vmr9);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    public void BuildGraph()
    {
      int hr  = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();

      rot = new DsROTEntry(graphBuilder);

      vmr9 = (IBaseFilter) new VideoMixingRenderer9();

      IVMRFilterConfig9 filterConfig = (IVMRFilterConfig9) vmr9;

      hr = filterConfig.SetNumberOfStreams(2);
      DsError.ThrowExceptionForHR(hr);

      // Set the custom compositor
      hr = filterConfig.SetImageCompositor(this);
      DsError.ThrowExceptionForHR(hr);

      IVMRMixerControl9 mixerControl = (IVMRMixerControl9) vmr9;

      // In COLORREF, colors are coded in ABGR format
      hr = mixerControl.SetBackgroundClr(backgroundColorABGR);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.AddFilter(vmr9, "VMR9");
      DsError.ThrowExceptionForHR(hr);

      // The 2 VMR pins must be connected...
      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    #region Membres de IVMRImageCompositor9

    public int CompositeImage(System.IntPtr pD3DDevice, System.IntPtr pddsRenderTarget, AMMediaType pmtRenderTarget, long rtStart, long rtEnd, int dwClrBkGnd, VMR9VideoStreamInfo[] pVideoStreamInfo, int cStreams)
    {
      Debug.Assert(pD3DDevice != IntPtr.Zero, "IVMRImageCompositor9.CompositeImage");
      Debug.Assert(pddsRenderTarget != IntPtr.Zero, "IVMRImageCompositor9.CompositeImage");
      Debug.Assert(pmtRenderTarget != null, "IVMRImageCompositor9.CompositeImage");
      // background color is coded in ARGB. A is always 0xff.
      Debug.Assert(dwClrBkGnd == backgroundColorARGB, "IVMRImageCompositor9.CompositeImage");
      Debug.Assert(pVideoStreamInfo.Length == 2, "IVMRImageCompositor9.CompositeImage");
      Debug.Assert(cStreams == 2, "IVMRImageCompositor9.CompositeImage");

      CompositeImageCalledCount++;
      return 0;
    }

    public int TermCompositionDevice(System.IntPtr pD3DDevice)
    {
      // this method is called before all other methods for cleanup purpose and pD3DDevice can be null or not...

      TermCompositionDeviceCalledCount++;
      return 0;
    }

    public int SetStreamMediaType(int dwStrmID, AMMediaType pmt, bool fTexture)
    {
      // Stream ID should be 0 or 1 for the two streams connected to the VMR
      Debug.Assert(dwStrmID == 0 || dwStrmID == 1, "IVMRImageCompositor9.SetStreamMediaType");

      // Note : This method is called serveral times but only one of them have pmt != null

      SetStreamMediaTypeCalledCount++;
      return 0;
    }

    public int InitCompositionDevice(System.IntPtr pD3DDevice)
    {
      // A valid pointer should be pass to this custom compositor. 
      Debug.Assert(pD3DDevice != IntPtr.Zero, "IVMRImageCompositor9.InitCompositionDevice");

      InitCompositionDeviceCalledCount++;
      return 0;
    }

    #endregion
  }



}
