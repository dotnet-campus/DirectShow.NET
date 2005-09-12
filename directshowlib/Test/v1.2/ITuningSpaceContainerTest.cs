using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

// Note : System Tuning Spaces are stored here : HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Tuning Spaces
// I use ASTC tuning space in my tests because they are installed with Direct3D 9.0.
// Other DVB-x tuning spaces are not present by default...

namespace DirectShowLib.Test
{
	public class ITuningSpaceContainerTest
	{
    ITuningSpaceContainer tsContainer;

		public ITuningSpaceContainerTest()
		{
		}

    public void DoTests()
    {
      try
      {
        tsContainer = (ITuningSpaceContainer) new SystemTuningSpaces();

        TestCount();
        TestFindID();
        TestMaxCount();
        TestTuningSpacesForCLSID();
        TestTuningSpacesForName();
        TestEnums();
        TestItem();
        TestAddRemove();
      }
      finally
      {
        Marshal.ReleaseComObject(tsContainer);
      }
    }

    private void TestCount()
    {
      int hr = 0;
      int count;

      hr = tsContainer.get_Count(out count);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuningSpaceContainer.get_Count");

    }

    private void TestFindID()
    {
      int hr = 0;
      int id;
      ITuningSpace ts;

      hr = tsContainer.get_Item(3, out ts);
      DsError.ThrowExceptionForHR(hr);

      hr = tsContainer.FindID(ts, out id);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(id == 3, "ITuningSpaceContainer.FindID");
    }

    private void TestMaxCount()
    {
      int hr = 0;
      int max1, max2;

      hr = tsContainer.get_MaxCount(out max1);
      DsError.ThrowExceptionForHR(hr);

      hr = tsContainer.put_MaxCount(max1 + 1);
      DsError.ThrowExceptionForHR(hr);

      hr = tsContainer.get_MaxCount(out max2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(max2 == max1 + 1, "ITuningSpaceContainer.get_MaxCount / put_MaxCount");

      hr = tsContainer.put_MaxCount(32); // Reset the default value
      DsError.ThrowExceptionForHR(hr);
    }
    
    private void TestTuningSpacesForCLSID()
    {
      int hr = 0;
      Guid searchClsid = typeof(ATSCTuningSpace).GUID;
      ITuningSpaces tsColl;
      int count1, count2;

      hr = tsContainer._TuningSpacesForCLSID(searchClsid, out tsColl);
      DsError.ThrowExceptionForHR(hr);

      hr = tsColl.get_Count(out count1);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(tsColl);

      // The guid string must have this form : {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
      hr = tsContainer.TuningSpacesForCLSID(searchClsid.ToString("B"), out tsColl);
      DsError.ThrowExceptionForHR(hr);

      hr = tsColl.get_Count(out count2);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(tsColl);

      Debug.Assert(count1 == count2, "ITuningSpaceContainer.TestTuningSpacesForCLSID");
    }

    private void TestTuningSpacesForName()
    {
      int hr = 0;
      ITuningSpace ts;
      string uniqueName;
      ITuningSpaces tsColl;
      int count;

      hr = tsContainer.get_Item(1, out ts);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.get_UniqueName(out uniqueName);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(ts);

      // Do a seach with this unique name. We should at least have 1 entry in the returned collection
      hr = tsContainer.TuningSpacesForName(uniqueName, out tsColl);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (tsColl != null), "ITuningSpaceContainer.TuningSpacesForName");

      hr = tsColl.get_Count(out count);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(count >= 1, "ITuningSpaceContainer.TuningSpacesForName");

      Marshal.ReleaseComObject(tsColl);
    }

    private void TestEnums()
    {
      int hr = 0;
      UCOMIEnumVARIANT enumTS1;
      IEnumTuningSpaces enumTS2;

      hr = tsContainer.get__NewEnum(out enumTS1);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (enumTS1 != null), "ITuningSpaceContainer.get__NewEnum");

      Marshal.ReleaseComObject(enumTS1);

      hr = tsContainer.get_EnumTuningSpaces(out enumTS2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (enumTS2 != null), "ITuningSpaceContainer.get_EnumTuningSpaces");

      Marshal.ReleaseComObject(enumTS2);
    }

    private void TestItem()
    {
      int hr = 0;
      ITuningSpace ts;

      hr = tsContainer.get_Item(1, out ts);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuningSpaceContainer.get_Item");

      hr = tsContainer.put_Item(1, ts);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuningSpaceContainer.put_Item");

      Marshal.ReleaseComObject(ts);
    }

    private void TestAddRemove()
    {
      int hr = 0;
      ITuningSpace ts;
      object id;

      ts = (ITuningSpace) new DVBTuningSpace();

      ts.put_UniqueName("DirectShowLib");
      DsError.ThrowExceptionForHR(hr);

      ts.put_FriendlyName("DirectShowLib TuningSpace test");
      DsError.ThrowExceptionForHR(hr);

      ts.put_NetworkType(typeof(DVBTNetworkProvider).GUID.ToString("B"));
      DsError.ThrowExceptionForHR(hr);

      (ts as IDVBTuningSpace2).put_SystemType(DVBSystemType.Terrestrial);
      DsError.ThrowExceptionForHR(hr);

      hr = tsContainer.Add(ts, out id);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuningSpaceContainer.Add");

      hr = tsContainer.Remove(id);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuningSpaceContainer.Remove");

      Marshal.ReleaseComObject(ts);
    }

	}
}
