using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRImagePresenterConfig9Test
	{
    private IVMRImagePresenterConfig9 imagePresenterConfig = null;

    // This CLSID is normally hidden because there is no need to access this COM object with VMR9
    // but for the purpose of this test, we need an object implementing IVMRImagePresenterConfig9 
    // and this is the only known built-in object doing it...
    private Guid vmr9AllocatorPresenterGuid = new Guid("2D2E24CB-0CD5-458F-86EA-3E6FA22C8E64"); 
    
    public IVMRImagePresenterConfig9Test()
		{
		}

    public void DoTests()
    {
      try
      {
        Type comType = Type.GetTypeFromCLSID(vmr9AllocatorPresenterGuid);
        imagePresenterConfig = (IVMRImagePresenterConfig9) Activator.CreateInstance(comType);

        TestRenderingPrefs();
      }
      finally
      {
        Marshal.ReleaseComObject(imagePresenterConfig);
      }
    }

    public void TestRenderingPrefs()
    {
      int hr = 0;
      VMR9RenderPrefs renderPrefs;

      hr = imagePresenterConfig.SetRenderingPrefs(VMR9RenderPrefs.DoNotRenderBorder);
      DsError.ThrowExceptionForHR(hr);

      hr = imagePresenterConfig.GetRenderingPrefs(out renderPrefs);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(renderPrefs == VMR9RenderPrefs.DoNotRenderBorder, "IVMRImagePresenterConfig9.GetRenderingPrefs / SetRenderingPrefs");
    }
  }
}
