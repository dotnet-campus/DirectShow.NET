using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IDvbSatelliteDeliverySystemDescriptorTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDvbSatelliteDeliverySystemDescriptor dsdsDescriptor;

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
      hr = dsdsDescriptor.GetTag(out tag);
      Debug.Assert((hr == 0) && (tag == 0x43), "IDvbSatelliteDeliverySystemDescriptor.GetTag failed");

      byte length = 0;
      hr = dsdsDescriptor.GetLength(out length);
      Debug.Assert((hr == 0) && (length != 0), "IDvbSatelliteDeliverySystemDescriptor.GetLength failed");
    }

    // Methods tested : GetFECInner, GetFrequency, GetModulation, GetOrbitalPosition
    //                  GetPolarization, GetSymbolRate, GetWestEastFlag
    public void TestBatch2()
    {
      int hr = 0;

      byte innerFEC = 0;
      hr = dsdsDescriptor.GetFECInner(out innerFEC);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetFECInner failed");

      int freq = 0;
      hr = dsdsDescriptor.GetFrequency(out freq);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetFrequency failed");

      byte modulation = 0;
      hr = dsdsDescriptor.GetModulation(out modulation);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetModulation failed");

      short orbitalPos = 0;
      hr = dsdsDescriptor.GetOrbitalPosition(out orbitalPos);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetOrbitalPosition failed");

      byte polarization = 0;
      hr = dsdsDescriptor.GetPolarization(out polarization);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetPolarization failed");

      int symbolRate = 0;
      hr = dsdsDescriptor.GetSymbolRate(out symbolRate);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetSymbolRate failed");

      byte westEstFlag = 0;
      hr = dsdsDescriptor.GetWestEastFlag(out westEstFlag);
      Debug.Assert(hr == 0, "IDvbSatelliteDeliverySystemDescriptor.GetWestEastFlag failed");
    }

    private void Config()
    {
      int hr = 0;
      IDVB_NIT nit;
      IGenericDescriptor descriptor = null;

      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      for (int j = 0; j < 10; j++)
      {
        hr = parser.GetNIT(0x40, null, out nit);
        Debug.Assert(nit != null, "Can't get a NIT object");

        int recordCount = 0;
        hr = nit.GetCountOfRecords(out recordCount);

        descriptor = null;

        for (int i = 0; i < recordCount; i++)
        {
          // see the doc for the meaning of the 0x43
          hr = nit.GetRecordDescriptorByTag(i, 0x43, null, out descriptor);
          if (descriptor != null)
            break;
        }

        Marshal.ReleaseComObject(nit);

        if (descriptor != null)
          break;
      }

      dsdsDescriptor = descriptor as IDvbSatelliteDeliverySystemDescriptor;
      Debug.Assert(dsdsDescriptor != null, "Can't get a IDvbSatelliteDeliverySystemDescriptor object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(dsdsDescriptor);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }


  }
}
