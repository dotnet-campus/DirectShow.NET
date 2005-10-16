using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBTuningSpaceTest
	{
    private IDVBTuningSpace tuningSpace;
    
    public IDVBTuningSpaceTest()
		{
		}

    public void DoTests()
    {
      try
      {
        // No special HW required for this sample
        tuningSpace = (IDVBTuningSpace) BDAUtils.GetTuningSpace(BDANetworkType.DVBT);

        TestSystemType();

      }
      finally
      {
        Marshal.ReleaseComObject(tuningSpace);
      }
    }

    private void TestSystemType()
    {
      int hr = 0;
      DVBSystemType sysType;

      hr = tuningSpace.get_SystemType(out sysType);
      // Since we have created a DVBT tuning space, system type should be Terrestrial
      Debug.Assert(sysType == DVBSystemType.Terrestrial, "IDVBTuningSpace.get_SystemType");

      // Trying to change it (purposeless)
      hr = tuningSpace.put_SystemType(DVBSystemType.Satellite);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_SystemType(out sysType);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(sysType == DVBSystemType.Satellite, "IDVBTuningSpace.get_SystemType / put_SystemType");
    }


	}
}
