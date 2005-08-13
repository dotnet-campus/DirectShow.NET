using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Direct3D is required for this test.
// Microsoft.DirectX.dll and Microsoft.DirectX.Direct3D.dll
// must be in the references of the project
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

/*
 * This is a test for both IVMRSurfaceAllocatorEx9 and IVMRImagePresenter9
 */

namespace DirectShowLib.Test
{
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class IVMRSurfaceAllocatorEx9Test : IVMRSurfaceAllocatorEx9, IVMRImagePresenter9
	{
    // DirectShow Stuff
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9 = null;
    private IVMRSurfaceAllocatorNotify9 surfaceAllocatorNotify = null;
    private DsROTEntry rot = null;
    private IntPtr cookie = new IntPtr(0x12345678);
    private DsRect dstRect;

    private int InitializeDeviceCount = 0;
    private int GetSurfaceCount = 0;
    private int TerminateDeviceCount = 0;
    private int AdviseNotifyCount = 0;
    private int GetSurfaceExCount = 0;
    private int StartPresentingCount = 0;
    private int StopPresentingCount = 0;
    private int PresentImageCount = 0;

    // Direct3D Stuff
    private const int DxMagicNumber = -759872593;
    private Form form = null;
    private PresentParameters presentParam;
    private Device device = null;
    private IntPtr[] unmanagedSurfaces = null;

		public IVMRSurfaceAllocatorEx9Test()
		{
      CreateDevice();
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

        // All methods need to be called at least once
        Debug.Assert(InitializeDeviceCount != 0, "IVMRSurfaceAllocatorEx9.InitializeDevice");
        Debug.Assert(GetSurfaceCount != 0, "IVMRSurfaceAllocatorEx9.GetSurface");
        Debug.Assert(TerminateDeviceCount != 0, "IVMRSurfaceAllocatorEx9.TerminateDevice");
        Debug.Assert(AdviseNotifyCount != 0, "IVMRSurfaceAllocatorEx9.AdviseNotify");
        Debug.Assert(GetSurfaceExCount != 0, "IVMRSurfaceAllocatorEx9.GetSurfaceEx");
        Debug.Assert(StartPresentingCount != 0, "IVMRImagePresenter9.StartPresenting");
        Debug.Assert(StopPresentingCount != 0, "IVMRImagePresenter9.StopPresenting");
        Debug.Assert(PresentImageCount != 0, "IVMRImagePresenter9.PresentImage");
      }
      finally
      {
        if (rot != null)
          rot.Dispose();

        ReleaseDevice();

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

      // Put the VMR9 in Renderless mode
      hr = filterConfig.SetRenderingMode(VMR9Mode.Renderless);
      DsError.ThrowExceptionForHR(hr);

      surfaceAllocatorNotify = (IVMRSurfaceAllocatorNotify9) vmr9;

      // Advise to VMR9 of our custom Allocator / Presenter
      hr = surfaceAllocatorNotify.AdviseSurfaceAllocator(cookie, this);
      DsError.ThrowExceptionForHR(hr);

      // Advise our custom Allocator / Presenter of the VMR9
      hr = this.AdviseNotify(surfaceAllocatorNotify);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.AddFilter(vmr9, "VMR9");
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);
    }

    // IVMRSurfaceAllocatorEx9 methods need to pass valid Direct3D objects
    private void CreateDevice()
    {
      form = new Form();
      form.Show();

      presentParam = new PresentParameters();
      presentParam.Windowed = true;
      presentParam.PresentFlag = PresentFlag.Video;
      presentParam.SwapEffect = SwapEffect.Copy;
      presentParam.BackBufferFormat = Manager.Adapters.Default.CurrentDisplayMode.Format;

      device = new Device(
        0, 
        DeviceType.Hardware, 
        form, 
        CreateFlags.SoftwareVertexProcessing | CreateFlags.MultiThreaded,
        presentParam
        );
    }

    private void ReleaseDevice()
    {
      if (device != null)
        device.Dispose();
      if (form != null)
        form.Dispose();
    }

    #region Membres de IVMRSurfaceAllocatorEx9

    public int InitializeDevice(System.IntPtr dwUserID, ref VMR9AllocationInfo lpAllocInfo, ref int lpNumBuffers)
    {
      int hr = 0;
      int width = 1;
      int height = 1;

      Debug.Assert(dwUserID == cookie, "IVMRSurfaceAllocatorEx9.InitializeDevice");

      InitializeDeviceCount++;

      dstRect = new DsRect(0, 0, lpAllocInfo.dwWidth, lpAllocInfo.dwHeight);

      try
      {
        if (device.DeviceCaps.TextureCaps.SupportsPower2)
        {
          while (width < lpAllocInfo.dwWidth)
            width = width << 1;
          while (height < lpAllocInfo.dwHeight)
            height = height << 1;

          lpAllocInfo.dwWidth = width;
          lpAllocInfo.dwHeight = height;
        }

        lpAllocInfo.dwFlags |= VMR9SurfaceAllocationFlags.TextureSurface;

        unmanagedSurfaces = new IntPtr[lpNumBuffers];

        hr = surfaceAllocatorNotify.AllocateSurfaceHelper(ref lpAllocInfo, ref lpNumBuffers, unmanagedSurfaces);
        DsError.ThrowExceptionForHR(hr);
        // Assume this call works (ie Hardware new enough to create a TextureSurface : dx7 video board or better)
        // This test also doesn't support YUV surfaces creation
      }
      catch (DirectXException dxe)
      {
        return dxe.ErrorCode;
      }
      catch (COMException e)
      {
        return e.ErrorCode;
      }

      return 0;
    }

    // return the surface created in InitializeDevice
    public int GetSurface(System.IntPtr dwUserID, int SurfaceIndex, int SurfaceFlags, out System.IntPtr lplpSurface)
    {
      Debug.Assert(dwUserID == cookie, "IVMRSurfaceAllocatorEx9.GetSurface");

      GetSurfaceCount++;
      lplpSurface = unmanagedSurfaces[SurfaceIndex];
      return 0;
    }

    public int TerminateDevice(System.IntPtr dwID)
    {
      Debug.Assert(dwID == cookie, "IVMRSurfaceAllocatorEx9.TerminateDevice");

      TerminateDeviceCount++;
      return 0;
    }

    // The custom Allocator / Presenter is responsible of create the Direct3D device 
    // and must pass it to the VMR9...
    public int AdviseNotify(IVMRSurfaceAllocatorNotify9 lpIVMRSurfAllocNotify)
    {
      AdviseNotifyCount++;

      IntPtr unmanagedDevice = device.GetObjectByValue(DxMagicNumber);
      IntPtr hMonitor = Manager.GetAdapterMonitor(Manager.Adapters.Default.Adapter);

      return surfaceAllocatorNotify.SetD3DDevice(unmanagedDevice, hMonitor);
    }

    // return the surface created in InitializeDevice and a dsRect with the video size
    public int GetSurfaceEx(System.IntPtr dwUserID, int SurfaceIndex, int SurfaceFlags, out System.IntPtr lplpSurface, out DsRect lprcDst)
    {
      Debug.Assert(dwUserID == cookie, "IVMRSurfaceAllocatorEx9.GetSurfaceEx");

      GetSurfaceExCount++;
      lplpSurface = unmanagedSurfaces[SurfaceIndex];
      lprcDst = dstRect;
      return 0;
    }

    #endregion

    #region Membres de IVMRImagePresenter9

    public int StartPresenting(System.IntPtr dwUserID)
    {
      Debug.Assert(dwUserID == cookie, "IVMRImagePresenter9.StartPresenting");

      StartPresentingCount++;
      return 0;
    }

    public int StopPresenting(System.IntPtr dwUserID)
    {
      Debug.Assert(dwUserID == cookie, "IVMRImagePresenter9.StopPresenting");

      StopPresentingCount++;
      return 0;
    }

    public int PresentImage(System.IntPtr dwUserID, ref VMR9PresentationInfo lpPresInfo) 
    {
      Debug.Assert(dwUserID == cookie, "IVMRImagePresenter9.PresentImage");

      PresentImageCount++;
      return 0;
    }

    #endregion
  }
}
