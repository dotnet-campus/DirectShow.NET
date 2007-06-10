using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDvbTerrestrialDeliverySystemDescriptorTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDvbTerrestrialDeliverySystemDescriptor dtdsDescriptor;

    public void DoTests()
    {
      try
      {
        Config();

        TestBatch1();
        TestBatch2();
      }
      finally
      {
        Unconfig();
      }
    }

    // Methods tested : GetTag, GetLength
    public void TestBatch1()
    {
      int hr = 0;

      byte tag = 0;
      hr = dtdsDescriptor.GetTag(out tag);
      Debug.Assert((hr == 0) && (tag == 0x5a), "IDvbTerrestrialDeliverySystemDescriptor.GetTag failed");

      byte length = 0;
      hr = dtdsDescriptor.GetLength(out length);
      Debug.Assert((hr == 0) && (length != 0), "IDvbTerrestrialDeliverySystemDescriptor.GetLength failed");
    }

    // Methods tested : GetBandwidth, GetCentreFrequency, GetCodeRateHPStream
    //                  GetCodeRateLPStream, GetConstellation, GetGuardInterval
    //                  GetHierarchyInformation, GetOtherFrequencyFlag
    //                  GetTransmissionMode
    public void TestBatch2()
    {
      int hr = 0;

      byte bandwidth = 0;
      hr = dtdsDescriptor.GetBandwidth(out bandwidth);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetBandwidth failed");

      int centralFreq = 0;
      hr = dtdsDescriptor.GetCentreFrequency(out centralFreq);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetCentreFrequency failed");

      byte hpRate = 0;
      hr = dtdsDescriptor.GetCodeRateHPStream(out hpRate);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetCodeRateHPStream failed");

      byte lpRate = 0;
      hr = dtdsDescriptor.GetCodeRateLPStream(out lpRate);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetCodeRateLPStream failed");

      byte constellation = 0;
      hr = dtdsDescriptor.GetConstellation(out constellation);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetConstellation failed");

      byte guardInterval = 0;
      hr = dtdsDescriptor.GetGuardInterval(out guardInterval);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetGuardInterval failed");

      byte hierarchyInformation = 0;
      hr = dtdsDescriptor.GetHierarchyInformation(out hierarchyInformation);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetHierarchyInformation failed");

      byte otherFreq = 0;
      hr = dtdsDescriptor.GetOtherFrequencyFlag(out otherFreq);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetOtherFrequencyFlag failed");

      byte transMode = 0;
      hr = dtdsDescriptor.GetTransmissionMode(out transMode);
      Debug.Assert(hr == 0, "IDvbTerrestrialDeliverySystemDescriptor.GetTransmissionMode failed");
    }

    private void Config()
    {
      int hr = 0;
      IDVB_NIT nit;

      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      hr = parser.GetNIT(0x40, null, out nit);
      Debug.Assert(nit != null, "Can't get a NIT object");

      int recordCount = 0;
      hr = nit.GetCountOfRecords(out recordCount);

      IGenericDescriptor descriptor = null;

      for (int i = 0; i < recordCount; i++)
      {
        // see the doc for the meaning of the 0x5a
        hr = nit.GetRecordDescriptorByTag(i, 0x5a, null, out descriptor);
        if (descriptor != null)
          break;
      }

      Marshal.ReleaseComObject(nit);

      dtdsDescriptor = descriptor as IDvbTerrestrialDeliverySystemDescriptor;
      Debug.Assert(dtdsDescriptor != null, "Can't get a DvbTerrestrialDeliverySystemDescriptor object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(dtdsDescriptor);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}
