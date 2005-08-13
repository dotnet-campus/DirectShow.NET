using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/*
 * This is a test for both IVMRSurfaceAllocator and IVMRImagePresenter
 */

namespace DirectShowLib.Test
{
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class IVMRSurfaceAllocatorTest : IVMRSurfaceAllocator, IVMRImagePresenter
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr = null;
    private IVMRSurfaceAllocator defaultAllocatorPresenter = null;
    private IVMRSurfaceAllocatorNotify surfaceAllocatorNotify = null;
    private DsROTEntry rot = null;
    private IntPtr cookie = new IntPtr(0x12345678);

    private int PrepareSurfaceCount = 0;
    private int AllocateSurfaceCount = 0;
    private int FreeSurfaceCount = 0;
    private int AdviseNotifyCount = 0;
    private int StartPresentingCount = 0;
    private int StopPresentingCount = 0;
    private int PresentImageCount = 0;

    private Form form = null;

    public IVMRSurfaceAllocatorTest()
		{
      WeakReference wr = new WeakReference(this);
		}

    public void DoTests()
    {
      int hr = 0;

      try
      {
        BuildGraph();

        hr = (graphBuilder as IMediaControl).Run();
        DsError.ThrowExceptionForHR(hr);

        // Nothing is shown in the video window since we present nothing...
        System.Threading.Thread.Sleep(2000);

        hr = (graphBuilder as IMediaControl).Stop();
        DsError.ThrowExceptionForHR(hr);
      }
      finally
      {
        if (rot != null)
          rot.Dispose();

        Marshal.ReleaseComObject(vmr);
        if (defaultAllocatorPresenter != null)
          Marshal.ReleaseComObject(defaultAllocatorPresenter);
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

      // Put the VMR7 in Renderless mode
      hr = filterConfig.SetRenderingMode(VMRMode.Renderless);
      DsError.ThrowExceptionForHR(hr);

      surfaceAllocatorNotify = (IVMRSurfaceAllocatorNotify) vmr;

      hr = surfaceAllocatorNotify.AdviseSurfaceAllocator(cookie, this);
      DsError.ThrowExceptionForHR(hr);

      defaultAllocatorPresenter = (IVMRSurfaceAllocator) Activator.CreateInstance(Type.GetTypeFromCLSID(VMRClsId.AllocPresenter));

      form = new Form();
      form.Show();

      hr = (defaultAllocatorPresenter as IVMRWindowlessControl).SetVideoClippingWindow(form.Handle);
      DsError.ThrowExceptionForHR(hr);

      hr = this.AdviseNotify(surfaceAllocatorNotify);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.AddFilter(vmr, "VMR");

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);
    }
    #region Membres de IVMRSurfaceAllocator

    public int PrepareSurface(IntPtr dwUserID, IntPtr lplpSurface, int dwSurfaceFlags)
    {
      PrepareSurfaceCount++;

      try
      {
        int hr = defaultAllocatorPresenter.PrepareSurface(dwUserID, lplpSurface, dwSurfaceFlags);
        return hr;
      }
      catch
      {
        return 0;
      }
    }

    public int AllocateSurface(IntPtr dwUserID, ref VMRAllocationInfo lpAllocInfo, out int lpdwActualBuffers, ref IntPtr lplpSurface)
    {
      AllocateSurfaceCount++;

      int hr = defaultAllocatorPresenter.AllocateSurface(dwUserID, ref lpAllocInfo, out lpdwActualBuffers, ref lplpSurface);
      return hr;
    }

    public int FreeSurface(IntPtr dwID)
    {
      FreeSurfaceCount++;

      int hr = defaultAllocatorPresenter.FreeSurface(dwID);
      return hr;
    }

    public int AdviseNotify(IVMRSurfaceAllocatorNotify lpIVMRSurfAllocNotify)
    {
      AdviseNotifyCount++;

      int hr = defaultAllocatorPresenter.AdviseNotify(lpIVMRSurfAllocNotify);
      return hr;
    }

    #endregion

    #region Membres de IVMRImagePresenter

    public int StartPresenting(IntPtr dwUserID)
    {
      StartPresentingCount++;

      return (defaultAllocatorPresenter as IVMRImagePresenter).StartPresenting(dwUserID);
    }

    public int StopPresenting(IntPtr dwUserID)
    {
      StopPresentingCount++;

      return (defaultAllocatorPresenter as IVMRImagePresenter).StopPresenting(dwUserID);
    }

    public int PresentImage(IntPtr dwUserID, ref VMRPresentationInfo lpPresInfo)
    {
      PresentImageCount++;

      return (defaultAllocatorPresenter as IVMRImagePresenter).PresentImage(dwUserID, ref lpPresInfo);
    }

    #endregion
  }
}
