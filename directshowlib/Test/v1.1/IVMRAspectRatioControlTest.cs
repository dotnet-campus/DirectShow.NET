using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRAspectRatioControlTest
	{
    IVMRAspectRatioControl arControl = null;

		public IVMRAspectRatioControlTest()
		{
		}

    public void DoTests()
    {
      arControl = (IVMRAspectRatioControl) new VideoMixingRenderer();

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

      Debug.Assert(ar == VMRAspectRatioMode.LetterBox,"IVMRAspectRatioControl.GetAspectRatioMode");

      hr = arControl.SetAspectRatioMode(VMRAspectRatioMode.None);
      DsError.ThrowExceptionForHR(hr);

      hr = arControl.GetAspectRatioMode(out ar);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(ar == VMRAspectRatioMode.None,"IVMRAspectRatioControl.GetAspectRatioMode");
    }

	}
}
