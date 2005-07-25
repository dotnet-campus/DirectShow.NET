using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace DirectShowLib.Test
{
	public class IVMRImagePresenterExclModeConfigTest : Form
	{
    private IVMRImagePresenterExclModeConfig imagePresenterExclModeConfig = null;
    private Device device = null;
    private Surface primarySurface = null;
    IntPtr unmanagedDevice = IntPtr.Zero;
    IntPtr unmanagedSurface = IntPtr.Zero;
    
    public IVMRImagePresenterExclModeConfigTest()
		{
		}

    public void DoTests()
    {
      try
      {
        this.Show();

        Type comType = Type.GetTypeFromCLSID(VMRClsId.AllocPresenterDDXclMode);
        imagePresenterExclModeConfig = (IVMRImagePresenterExclModeConfig) Activator.CreateInstance(comType);

        TestRenderingPrefs();

//        CreateDirectDrawDeviceAndPrimarySurface();

        TestXlcModeDDObjAndPrimarySurface();
      }
      finally
      {
        if (primarySurface != null)
          primarySurface.Dispose();

        if (device != null)
          device.Dispose();

        Marshal.ReleaseComObject(imagePresenterExclModeConfig);

        this.Close();
      }
    }

    private unsafe void CreateDirectDrawDeviceAndPrimarySurface()
    {
      Type type;
      FieldInfo fieldInfo;
      IDirectDraw7 directDraw;

      SurfaceDescription surfaceDesc = new SurfaceDescription();

      device = new Device();
      device.SetCooperativeLevel(this, CooperativeLevelFlags.FullscreenExclusive);
      device.SetDisplayMode(1280, 1024, 16, 0, false);

      type = device.GetType();
      directDraw = (IDirectDraw7) type.InvokeMember ("m_lpUM", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, device, new object [] {});

//      fieldInfo = type.GetField("m_lpUM", BindingFlags.NonPublic | BindingFlags.GetField);
//      unmanagedDevice = (IntPtr) fieldInfo.GetValue(device);

      surfaceDesc.SurfaceCaps.PrimarySurface = true;
      surfaceDesc.SurfaceCaps.Flip = true;
      surfaceDesc.SurfaceCaps.Complex = true;
      surfaceDesc.BackBufferCount = 1;

      primarySurface = new Surface(surfaceDesc, device);

      type = primarySurface.GetType();
      fieldInfo = type.GetField("m_lpUM");
      unmanagedSurface = (IntPtr) fieldInfo.GetValue(primarySurface);
    }

    public void TestRenderingPrefs()
    {
      int hr = 0;
      VMRRenderPrefs renderPrefs;

      hr = imagePresenterExclModeConfig.SetRenderingPrefs(VMRRenderPrefs.ForceOffscreen);
      DsError.ThrowExceptionForHR(hr);

      hr = imagePresenterExclModeConfig.GetRenderingPrefs(out renderPrefs);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(renderPrefs == VMRRenderPrefs.ForceOffscreen, "IVMRImagePresenterExclModeConfig.GetRenderingPrefs / SetRenderingPrefs");

      hr = imagePresenterExclModeConfig.SetRenderingPrefs(VMRRenderPrefs.DoNotRenderColorKeyAndBorder);
      DsError.ThrowExceptionForHR(hr);

      hr = imagePresenterExclModeConfig.GetRenderingPrefs(out renderPrefs);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(renderPrefs == VMRRenderPrefs.DoNotRenderColorKeyAndBorder, "IVMRImagePresenterExclModeConfig.GetRenderingPrefs / SetRenderingPrefs");
    }

    public void TestXlcModeDDObjAndPrimarySurface()
    {
      int hr = 0;

      hr = imagePresenterExclModeConfig.SetXlcModeDDObjAndPrimarySurface(unmanagedDevice, unmanagedSurface);
//      DsError.ThrowExceptionForHR(hr);

      IntPtr dev, surf;

//      hr = imagePresenterExclModeConfig.GetXlcModeDDObjAndPrimarySurface(out dev, out surf);
//      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRImagePresenterExclModeConfig.GetRenderingPrefs / SetRenderingPrefs");
    }

	}
}
