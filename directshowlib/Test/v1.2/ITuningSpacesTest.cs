using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class ITuningSpacesTest
	{
    ITuningSpaceContainer tsContainer;
    ITuningSpaces tuningSpaces;

		public ITuningSpacesTest()
		{
		}

    public void DoTests()
    {
      int hr = 0;

      try
      {
        Guid searchClsid = typeof(ATSCTuningSpace).GUID;
        tsContainer = (ITuningSpaceContainer) new SystemTuningSpaces();

        hr = tsContainer._TuningSpacesForCLSID(searchClsid, out tuningSpaces);
        DsError.ThrowExceptionForHR(hr);

        Testget__NewEnum();
        Testget_Count();
        Testget_EnumTuningSpaces();
        Testget_Item();
      }
      finally
      {
        Marshal.ReleaseComObject(tsContainer);
        Marshal.ReleaseComObject(tuningSpaces);
      }
    }

    private void Testget__NewEnum()
    {
      int hr = 0;
      IEnumVARIANT enumv;

      hr = tuningSpaces.get__NewEnum(out enumv);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (enumv != null), "ITuningSpaces.get__NewEnum");

      Marshal.ReleaseComObject(enumv);
    }

    private void Testget_Count()
    {
      int hr = 0;
      int count;

      hr = tuningSpaces.get_Count(out count);
      DsError.ThrowExceptionForHR(hr);

      // There is normaly at least 1 ATSC system tuning space...
      Debug.Assert((hr == 0) && (count > 0), "ITuningSpaces.get_Count");
    }

    private void Testget_EnumTuningSpaces()
    {
      int hr = 0;
      IEnumTuningSpaces enumTS;

      hr = tuningSpaces.get_EnumTuningSpaces(out enumTS);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (enumTS != null), "ITuningSpaces.get_EnumTuningSpaces");

      Marshal.ReleaseComObject(enumTS);
    }

    private void Testget_Item()
    {
      int hr = 0;
      ITuningSpace ts;

      // 3rd system tuning space is the ATSC one...
      hr = tuningSpaces.get_Item(3, out ts);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (ts != null), "ITuningSpaces.get_EnumTuningSpaces");

      Marshal.ReleaseComObject(ts);
    }

	}
}
