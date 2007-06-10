using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IDvbLogicalChannelDescriptorTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDvbLogicalChannelDescriptor dlcDescriptor;

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
      hr = dlcDescriptor.GetTag(out tag);
      Debug.Assert((hr == 0) && (tag == 0x83), "IDvbLogicalChannelDescriptor.GetTag failed");

      byte length = 0;
      hr = dlcDescriptor.GetLength(out length);
      Debug.Assert((hr == 0) && (length != 0), "IDvbLogicalChannelDescriptor.GetLength failed");
    }

    // Methods tested : GetCountOfRecords, GetRecordLogicalChannelNumber, GetRecordServiceId
    public void TestBatch2()
    {
      int hr = 0;

      byte recordCount = 0;
      hr = dlcDescriptor.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IDvbLogicalChannelDescriptor.GetCountOfRecords failed");

      for (byte i = 0; i < recordCount; i++)
      {
        short logicalChannelNumber = 0;
        hr = dlcDescriptor.GetRecordLogicalChannelNumber(i, out logicalChannelNumber);
        Debug.Assert((hr == 0) && (logicalChannelNumber != 0), "IDvbLogicalChannelDescriptor.GetRecordLogicalChannelNumber failed");

        short serviceId = 0;
        hr = dlcDescriptor.GetRecordServiceId(i, out serviceId);
        Debug.Assert((hr == 0) && (serviceId != 0), "IDvbLogicalChannelDescriptor.GetRecordServiceId failed");
      }
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
          // see the doc for the meaning of the 0x83
          hr = nit.GetRecordDescriptorByTag(i, 0x83, null, out descriptor);
          if (descriptor != null)
            break;
        }

        Marshal.ReleaseComObject(nit);

        if (descriptor != null)
          break;
      }

      dlcDescriptor = descriptor as IDvbLogicalChannelDescriptor;
      Debug.Assert(dlcDescriptor != null, "Can't get a IDvbLogicalChannelDescriptor object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(dlcDescriptor);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }

  }
}
