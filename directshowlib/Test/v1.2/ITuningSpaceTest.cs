using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class ITuningSpaceTest
	{
    private ITuningSpace tuningSpace;

		public ITuningSpaceTest()
		{
		}

    public void DoTests()
    {
      try
      {
        Config();
        TestClone();
        TestCreateTuneRequest();
        TestEnumCategoryGUIDs();
        TestEnumDeviceMonikers();
        TestNetworkType();
        Testget_CLSID();
        TestDefaultLocator();
        TestDefaultPreferredComponentTypes();
        TestFrequencyMapping();
        TestFriendlyName();
        TestUniqueName();

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

      hr = tsContainer.get_Item(3, out tuningSpace);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(tsContainer);
    }

    private void TestClone()
    {
      int hr = 0;
      ITuningSpace newTS;
      string uName1, uName2;

      hr = tuningSpace.Clone(out newTS);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (newTS != null), "ITuningSpace.Clone");

      hr = tuningSpace.get_UniqueName(out uName1);
      DsError.ThrowExceptionForHR(hr);

      hr = newTS.get_UniqueName(out uName2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(uName1 == uName2, "ITuningSpace.Clone");

      Marshal.ReleaseComObject(newTS);
    }

    private void TestCreateTuneRequest()
    {
      int hr = 0;
      ITuneRequest tuneReq;

      hr = tuningSpace.CreateTuneRequest(out tuneReq);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (tuneReq != null), "ITuningSpace.CreateTuneRequest");

      // ATSC tuning spaces create ATSC tune request...
      Debug.Assert(tuneReq is IATSCChannelTuneRequest, "ITuningSpace.CreateTuneRequest");

      Marshal.ReleaseComObject(tuneReq);
    }

    private void TestEnumCategoryGUIDs()
    {
      int hr = 0;
      object enumG;

      hr = tuningSpace.EnumCategoryGUIDs(out enumG);
      
      // This method is not implemented
      Debug.Assert(hr == -2147467263, "ITuningSpace.EnumCategoryGUIDs");
    }

    private void TestEnumDeviceMonikers()
    {
      int hr = 0;
      IEnumMoniker enumM;

      hr = tuningSpace.EnumDeviceMonikers(out enumM);
      
      // This method is not implemented
      Debug.Assert(hr == -2147467263, "ITuningSpace.EnumDeviceMonikers");
    }

    private void TestNetworkType()
    {
      int hr = 0;
      Guid DVBTNetwork = typeof(DVBTNetworkProvider).GUID;
      Guid networkType1, networkType2;
      string networkTypeStr1, networkTypeStr2;

      hr = tuningSpace.get__NetworkType(out networkType1);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.put__NetworkType(DVBTNetwork);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get__NetworkType(out networkType2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(networkType2 == DVBTNetwork, "ITuningSpace.get__NetworkType / put__NetworkType");

      // Restore default
      hr = tuningSpace.put__NetworkType(networkType1);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_NetworkType(out networkTypeStr1);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.put_NetworkType(DVBTNetwork.ToString("B").ToUpper());
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_NetworkType(out networkTypeStr2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(networkTypeStr2 == DVBTNetwork.ToString("B").ToUpper(), "ITuningSpace.get_NetworkType / put_NetworkType");

      // Restore default
      hr = tuningSpace.put_NetworkType(networkTypeStr1);
      DsError.ThrowExceptionForHR(hr);
    }

    private void Testget_CLSID()
    {
      int hr = 0;
      string clsid;

      hr = tuningSpace.get_CLSID(out clsid);
      DsError.ThrowExceptionForHR(hr);

      // This is an ATSC Tuning Space...
      Debug.Assert(clsid == typeof(ATSCTuningSpace).GUID.ToString("B").ToUpper(), "ITuningSpace.get_CLSID");
    }

    private void TestDefaultLocator()
    {
      int hr = 0;
      ILocator locator;

      hr = tuningSpace.get_DefaultLocator(out locator);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.put_DefaultLocator(locator);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (locator != null), "ITuningSpace.get_DefaultLocator / put_DefaultLocator");

      Marshal.ReleaseComObject(locator);
    }

    private void TestDefaultPreferredComponentTypes()
    {
      int hr = 0;
      IComponentTypes compTypes;

      hr = tuningSpace.get_DefaultPreferredComponentTypes(out compTypes);
      DsError.ThrowExceptionForHR(hr);

      // Default ATSC Tuning Space has no Preferred Component Types so it return null
      Debug.Assert((hr == 0) && (compTypes == null), "ITuningSpace.get_DefaultPreferredComponentTypes");

      // Create a dummy ComponentTypes collection
      compTypes = (IComponentTypes) new ComponentTypes();

      hr = tuningSpace.put_DefaultPreferredComponentTypes(compTypes);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ITuningSpace.put_DefaultPreferredComponentTypes");

      Marshal.ReleaseComObject(compTypes);
    }

    private void TestFrequencyMapping()
    {
      int hr = 0;
      string freq;

      hr = tuningSpace.put_FrequencyMapping("azerty");
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_FrequencyMapping(out freq);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(freq == "azerty", "ITuningSpace.get_FrequencyMapping / put_FrequencyMapping");
    }

    private void TestFriendlyName()
    {
      int hr = 0;
      string name1, name2;

      hr = tuningSpace.get_FriendlyName(out name1);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.put_FriendlyName(name1 + "123456");
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_FriendlyName(out name2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(name2 == name1 + "123456", "ITuningSpace.get_FriendlyName / put_FriendlyName");
    }

    private void TestUniqueName()
    {
      int hr = 0;
      string name1, name2;

      hr = tuningSpace.get_UniqueName(out name1);
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.put_UniqueName(name1 + "123456");
      DsError.ThrowExceptionForHR(hr);

      hr = tuningSpace.get_UniqueName(out name2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(name2 == name1 + "123456", "ITuningSpace.get_UniqueName / put_UniqueName");
    }

	}
}
