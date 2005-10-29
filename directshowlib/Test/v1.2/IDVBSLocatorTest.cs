using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBSLocatorTest
	{
    private IDVBSLocator locator;
    
    public IDVBSLocatorTest()
		{
		}

    public void DoTests()
    {
      try
      {
        locator = (IDVBSLocator) new DVBSLocator();

        TestAzimuth();
        TestElevation();
        TestOrbitalPosition();
        TestSignalPolarisation();
        TestWestPosition();
      }
      finally
      {
        Marshal.ReleaseComObject(locator);
      }
    }

    private void TestAzimuth()
    {
      int hr = 0;
      int az;

      hr = locator.put_Azimuth(900);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_Azimuth(out az);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(az == 900, "IDVBSLocator.get_Azimuth / put_Azimuth");
    }

    private void TestElevation()
    {
      int hr = 0;
      int elev;

      hr = locator.put_Elevation(450);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_Elevation(out elev);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(elev == 450, "IDVBSLocator.get_Elevation / put_Elevation");
    }

    private void TestOrbitalPosition()
    {
      int hr = 0;
      int longitude;

      hr = locator.put_OrbitalPosition(1800);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_OrbitalPosition(out longitude);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(longitude == 1800, "IDVBSLocator.get_OrbitalPosition / put_OrbitalPosition");
    }

    private void TestSignalPolarisation()
    {
      int hr = 0;
      Polarisation pol;

      hr = locator.put_SignalPolarisation(Polarisation.LinearH);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_SignalPolarisation(out pol);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(pol == Polarisation.LinearH, "IDVBSLocator.get_SignalPolarisation / put_SignalPolarisation");
    }

    private void TestWestPosition()
    {
      int hr = 0;
      bool westPos;

      hr = locator.put_WestPosition(true);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_WestPosition(out westPos);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(westPos == true, "IDVBSLocator.get_WestPosition / put_WestPosition");

      hr = locator.put_WestPosition(false);
      DsError.ThrowExceptionForHR(hr);

      hr = locator.get_WestPosition(out westPos);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(westPos == false, "IDVBSLocator.get_WestPosition / put_WestPosition");
    }
  
  }
}
