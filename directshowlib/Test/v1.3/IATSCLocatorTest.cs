using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IATSCLocatorTest
	{
    private IATSCLocator locator = null;
    
    public IATSCLocatorTest()
		{
		}

    public void DoTests()
    {
      locator = (IATSCLocator) new ATSCLocator();

      try
      {
        TestPhysicalChannel();
        TestTSID();
      }
      finally
      {
        Marshal.ReleaseComObject(locator);
      }
    }

    private void TestPhysicalChannel()
    {
      int hr = 0;
      int channel = 0;

      hr = locator.put_PhysicalChannel(10);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_PhysicalChannel(out channel);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(channel == 10, "IATSCLocator.get_PhysicalChannel / put_PhysicalChannel");

      hr = locator.put_PhysicalChannel(20);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_PhysicalChannel(out channel);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(channel == 20, "IATSCLocator.get_PhysicalChannel / put_PhysicalChannel");
    }

    private void TestTSID()
    {
      int hr = 0;
      int tsid = 0;

      hr = locator.put_TSID(10);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_TSID(out tsid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(tsid == 10, "IATSCLocator.get_TSID / put_TSID");

      hr = locator.put_TSID(20);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_TSID(out tsid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(tsid == 20, "IATSCLocator.get_TSID / put_TSID");
    }

	}
}
