using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class ITuneRequestTest
	{
    ITuneRequest tuneRequest;

		public ITuneRequestTest()
		{
		}

    public void DoTests()
    {
      try
      {
        Config();

        TestClone();
        Testget_Components();
        TestLocator();
        Testget_TuningSpace();

      }
      finally
      {
        Marshal.ReleaseComObject(tuneRequest);
      }
    }

    private void Config()
    {
      int hr = 0;

      ITuningSpaceContainer tsContainer = (ITuningSpaceContainer) new SystemTuningSpaces();
      ITuningSpace tuningSpace;

      // ATSC System Tuning Space
      hr = tsContainer.get_Item(3, out tuningSpace);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(tsContainer);

      hr = tuningSpace.CreateTuneRequest(out tuneRequest);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(tuningSpace);
    }

    private void TestClone()
    {
      int hr = 0;
      ITuneRequest newTR;

      hr = tuneRequest.Clone(out newTR);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(newTR != null, "ITuneRequest.Clone");

      Marshal.ReleaseComObject(newTR);
    }

    private void Testget_Components()
    {
      int hr = 0;
      IComponents comp;

      hr = tuneRequest.get_Components(out comp);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(comp != null, "ITuneRequest.get_Components");

      Marshal.ReleaseComObject(comp);
    }

    private void TestLocator()
    {
      int hr = 0;
      ILocator locator;

      hr = tuneRequest.get_Locator(out locator);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(locator != null, "ITuneRequest.get_Locator");

      hr = tuneRequest.put_Locator(locator);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuneRequest.put_Locator");

      Marshal.ReleaseComObject(locator);
    }

    private void Testget_TuningSpace()
    {
      int hr = 0;
      ITuningSpace ts;

      hr = tuneRequest.get_TuningSpace(out ts);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(ts != null, "ITuneRequest.get_TuningSpace");

      Marshal.ReleaseComObject(ts);
    }


	}
}
