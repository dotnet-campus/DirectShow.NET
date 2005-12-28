using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IATSCTuningSpaceTest
	{
    private IATSCTuningSpace tuningSpace = null;

		public IATSCTuningSpaceTest()
		{
		}

    public void DoTests()
    {
      Config();

      try
      {
        TestMinorChannel();
        TestPhysicalChannel();
      }
      finally
      {
        Marshal.ReleaseComObject(tuningSpace);
      }
    }

    private void Config()
    {
      int hr = 0;
      ITuningSpaceContainer tsContainer = (ITuningSpaceContainer) new SystemTuningSpaces();
      ITuningSpace ts = null;

      // The third System TuningSpace is an ATSC one...
      // see HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Tuning Spaces\3
      hr = tsContainer.get_Item(3, out ts);
      DsError.ThrowExceptionForHR(hr);

      tuningSpace = (IATSCTuningSpace) ts;

      Marshal.ReleaseComObject(tsContainer);
    }

    private void TestMinorChannel()
    {
      int hr = 0;
      int minCh, maxCh;

      hr = tuningSpace.put_MinMinorChannel(5);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MinMinorChannel(out minCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(minCh == 5, "IATSCTuningSpace.get_MinMinorChannel / put_MinMinorChannel");

      hr = tuningSpace.put_MinMinorChannel(10);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MinMinorChannel(out minCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(minCh == 10, "IATSCTuningSpace.get_MinMinorChannel / put_MinMinorChannel");

      hr = tuningSpace.put_MaxMinorChannel(50);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MaxMinorChannel(out maxCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(maxCh == 50, "IATSCTuningSpace.get_MaxMinorChannel / put_MaxMinorChannel");

      hr = tuningSpace.put_MaxMinorChannel(45);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MaxMinorChannel(out maxCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(maxCh == 45, "IATSCTuningSpace.get_MaxMinorChannel / put_MaxMinorChannel");
    }

    private void TestPhysicalChannel()
    {
      int hr = 0;

      int minCh, maxCh;

      hr = tuningSpace.put_MinPhysicalChannel(5);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MinPhysicalChannel(out minCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(minCh == 5, "IATSCTuningSpace.get_MinPhysicalChannel / put_MinPhysicalChannel");

      hr = tuningSpace.put_MinPhysicalChannel(10);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MinPhysicalChannel(out minCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(minCh == 10, "IATSCTuningSpace.get_MinPhysicalChannel / put_MinPhysicalChannel");

      hr = tuningSpace.put_MaxPhysicalChannel(50);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MaxPhysicalChannel(out maxCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(maxCh == 50, "IATSCTuningSpace.get_MaxPhysicalChannel / put_MaxPhysicalChannel");

      hr = tuningSpace.put_MaxPhysicalChannel(45);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_MaxPhysicalChannel(out maxCh);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(maxCh == 45, "IATSCTuningSpace.get_MaxPhysicalChannel / put_MaxPhysicalChannel");
    }

	}
}
