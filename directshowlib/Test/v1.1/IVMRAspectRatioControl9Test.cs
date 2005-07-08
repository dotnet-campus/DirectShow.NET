using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
  public class IVMRAspectRatioControl9Test
  {
    IVMRAspectRatioControl9 arControl = null;

    public IVMRAspectRatioControl9Test()
    {
    }

    public void DoTests()
    {
      arControl = (IVMRAspectRatioControl9) new VideoMixingRenderer9();

      try
      {
        TestAspectRatioMode();
      }
      finally
      {
        Marshal.ReleaseComObject(arControl);
      }
    }

    public void TestAspectRatioMode()
    {
      int hr = 0;
      VMRAspectRatioMode ar;

      hr = arControl.SetAspectRatioMode(VMRAspectRatioMode.LetterBox);
      DsError.ThrowExceptionForHR(hr);

      hr = arControl.GetAspectRatioMode(out ar);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(ar == VMRAspectRatioMode.LetterBox,"IVMRAspectRatioControl9.GetAspectRatioMode");

      hr = arControl.SetAspectRatioMode(VMRAspectRatioMode.None);
      DsError.ThrowExceptionForHR(hr);

      hr = arControl.GetAspectRatioMode(out ar);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(ar == VMRAspectRatioMode.None,"IVMRAspectRatioControl9.GetAspectRatioMode");
    }

  }
}
