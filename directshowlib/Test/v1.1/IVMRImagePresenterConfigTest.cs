using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRImagePresenterConfigTest
	{
    private IVMRImagePresenterConfig imagePresenterConfig = null;
    
    public IVMRImagePresenterConfigTest()
		{
		}

    public void DoTests()
    {
      try
      {
        Type comType = Type.GetTypeFromCLSID(VMRClsId.AllocPresenter);
        imagePresenterConfig = (IVMRImagePresenterConfig) Activator.CreateInstance(comType);

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
      VMRRenderPrefs renderPrefs;

      hr = imagePresenterConfig.SetRenderingPrefs(VMRRenderPrefs.ForceOffscreen);
      DsError.ThrowExceptionForHR(hr);

      hr = imagePresenterConfig.GetRenderingPrefs(out renderPrefs);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(renderPrefs == VMRRenderPrefs.ForceOffscreen, "IVMRImagePresenterConfig.GetRenderingPrefs / SetRenderingPrefs");

      hr = imagePresenterConfig.SetRenderingPrefs(VMRRenderPrefs.DoNotRenderColorKeyAndBorder);
      DsError.ThrowExceptionForHR(hr);

      hr = imagePresenterConfig.GetRenderingPrefs(out renderPrefs);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(renderPrefs == VMRRenderPrefs.DoNotRenderColorKeyAndBorder, "IVMRImagePresenterConfig.GetRenderingPrefs / SetRenderingPrefs");
    }
	}
}
