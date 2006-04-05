using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

// Direct3D is required for this test.
// Microsoft.DirectX.dll and Microsoft.DirectX.Direct3D.dll
// must be in the references of the project
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace DirectShowLib.Test
{
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  public class IVMRSurfaceAllocatorNotify9Test : IVMRSurfaceAllocator9, IVMRImagePresenter9
  {
    // DirectShow Stuff
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9 = null;
    private IVMRSurfaceAllocatorNotify9 surfaceAllocatorNotify = null;
    private DsROTEntry rot = null;
    private IntPtr cookie = new IntPtr(0x12345678);

    // Direct3D Stuff
    private const int DxMagicNumber = -759872593;
    private Form form = null;
    private PresentParameters presentParam;
    private Device device1 = null;
    private Device device2 = null;
    private IntPtr[] unmanagedSurfaces = null;

    public IVMRSurfaceAllocatorNotify9Test()
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

        TestAdviseSurfaceAllocator();
        TestAllocateSurfaceHelper();
        //TestChangeD3DDevice();
        TestNotifyEvent();
        TestSetD3DDevice();

        // Nothing is shown in the video window since we present nothing...
        System.Threading.Thread.Sleep(2000);

        hr = (graphBuilder as IMediaControl).Stop();
        DsError.ThrowExceptionForHR(hr);
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

    private void TestAdviseSurfaceAllocator()
    {
      // This method is tested in the BuildGraph method
    }

    private void TestAllocateSurfaceHelper()
    {
      // This method is tested in the InitializeDevice method
    }

    private void TestChangeD3DDevice()
    {
      int hr = 0;

      // This method works (it return 0) but this break my poorly designed custum allocator presenter
      // and graph can't be stopped. VMR loop in the TerminateDevice method...

      IntPtr unmanagedDevice = device2.GetObjectByValue(DxMagicNumber);
      IntPtr hMonitor = Manager.GetAdapterMonitor(Manager.Adapters.Default.Adapter);

      hr = surfaceAllocatorNotify.ChangeD3DDevice(unmanagedDevice, hMonitor);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRSurfaceAllocatorNotify9.ChangeD3DDevice");
    }

    private void TestNotifyEvent()
    {
      int hr = 0;

      hr = surfaceAllocatorNotify.NotifyEvent(EventCode.Repaint, IntPtr.Zero, IntPtr.Zero);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRSurfaceAllocatorNotify9.NotifyEvent");
    }

    private void TestSetD3DDevice()
    {
      // This method is tested in AdviseNotify
    }

    // IVMRSurfaceAllocator9 methods need to pass valid Direct3D objects
    private void CreateDevice()
    {
      form = new Form();
      form.Show();

      presentParam = new PresentParameters();
      presentParam.Windowed = true;
      presentParam.PresentFlag = PresentFlag.Video;
      presentParam.SwapEffect = SwapEffect.Copy;
      presentParam.BackBufferFormat = Manager.Adapters.Default.CurrentDisplayMode.Format;

      device1 = new Device(
        0, 
        DeviceType.Hardware, 
        form, 
        CreateFlags.SoftwareVertexProcessing | CreateFlags.MultiThreaded,
        presentParam
        );

      device2 = new Device(
        0, 
        DeviceType.Hardware, 
        form, 
        CreateFlags.SoftwareVertexProcessing | CreateFlags.MultiThreaded,
        presentParam
        );
    }

    private void ReleaseDevice()
    {
      if (device1 != null)
        device1.Dispose();
      if (device2 != null)
        device2.Dispose();
      if (form != null)
        form.Dispose();
    }

    #region Membres de IVMRSurfaceAllocator9

    public int InitializeDevice(System.IntPtr dwUserID, ref VMR9AllocationInfo lpAllocInfo, ref int lpNumBuffers)
    {
      int hr = 0;
      int width = 1;
      int height = 1;

      Debug.Assert(dwUserID == cookie, "IVMRSurfaceAllocator9.InitializeDevice");

      try
      {
        if (device1.DeviceCaps.TextureCaps.SupportsPower2)
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
      //catch (DirectXException dxe)
      //{
      //  return dxe.ErrorCode;
      //}
      catch (COMException e)
      {
        return e.ErrorCode;
      }

      return 0;
    }

    // return the surface created in InitializeDevice
    public int GetSurface(System.IntPtr dwUserID, int SurfaceIndex, int SurfaceFlags, out System.IntPtr lplpSurface)
    {
      Debug.Assert(dwUserID == cookie, "IVMRSurfaceAllocator9.GetSurface");

      lplpSurface = unmanagedSurfaces[SurfaceIndex];
      return 0;
    }

    public int TerminateDevice(System.IntPtr dwID)
    {
      Debug.Assert(dwID == cookie, "IVMRSurfaceAllocator9.TerminateDevice");

      if (unmanagedSurfaces != null)
      {
        for (int i=0; i < unmanagedSurfaces.Length; i++)
        {
          Surface tmp = new Surface(unmanagedSurfaces[i]);
          tmp.Dispose();
        }
      }

      unmanagedSurfaces = null;

      return 0;
    }

    // The custom Allocator / Presenter is responsible of create the Direct3D device1 
    // and must pass it to the VMR9...
    public int AdviseNotify(IVMRSurfaceAllocatorNotify9 lpIVMRSurfAllocNotify)
    {
      IntPtr unmanagedDevice = device1.GetObjectByValue(DxMagicNumber);
      IntPtr hMonitor = Manager.GetAdapterMonitor(Manager.Adapters.Default.Adapter);

      return surfaceAllocatorNotify.SetD3DDevice(unmanagedDevice, hMonitor);
    }

    #endregion

    #region Membres de IVMRImagePresenter9

    public int StartPresenting(System.IntPtr dwUserID)
    {
      Debug.Assert(dwUserID == cookie, "IVMRImagePresenter9.StartPresenting");

      return 0;
    }

    public int StopPresenting(System.IntPtr dwUserID)
    {
      Debug.Assert(dwUserID == cookie, "IVMRImagePresenter9.StopPresenting");

      return 0;
    }

    public int PresentImage(System.IntPtr dwUserID, ref VMR9PresentationInfo lpPresInfo) 
    {
      Debug.Assert(dwUserID == cookie, "IVMRImagePresenter9.PresentImage");

      return 0;
    }

    #endregion
  }
}
