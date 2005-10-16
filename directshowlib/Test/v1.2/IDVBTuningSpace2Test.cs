using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBTuningSpace2Test
	{
    private IDVBTuningSpace2 tuningSpace;
    
    public IDVBTuningSpace2Test()
		{
		}

    public void DoTests()
    {
      try
      {
        // No special HW required for this sample
        tuningSpace = (IDVBTuningSpace2) BDAUtils.GetTuningSpace(BDANetworkType.DVBT);

        TestNetworkID();
      }
      finally
      {
        Marshal.ReleaseComObject(tuningSpace);
      }
    }

    private void TestNetworkID()
    {
      int hr = 0;
      int nid;

      hr = tuningSpace.put_NetworkID(1234);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_NetworkID(out nid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(nid == 1234, "IDVBTuningSpace2.get_NetworkID / put_NetworkID");
    }

	}
}
