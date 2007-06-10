using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IDvbFrequencyListDescriptorTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDvbFrequencyListDescriptor dflDescriptor;

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
      hr = dflDescriptor.GetTag(out tag);
      Debug.Assert((hr == 0) && (tag == 0x62), "IDvbFrequencyListDescriptor.GetTag failed");

      byte length = 0;
      hr = dflDescriptor.GetLength(out length);
      Debug.Assert((hr == 0) && (length != 0), "IDvbFrequencyListDescriptor.GetLength failed");
    }

    // Methods tested : GetCountOfRecords, GetCodingType, GetRecordCentreFrequency
    public void TestBatch2()
    {
      int hr = 0;

      byte codingType = 0;
      hr = dflDescriptor.GetCodingType(out codingType);
      Debug.Assert((hr == 0) && (codingType != 0), "IDvbFrequencyListDescriptor.GetCodingType failed");

      byte recordCount = 0;
      hr = dflDescriptor.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IDvbFrequencyListDescriptor.GetCountOfRecords failed");

      for (byte i = 0; i < recordCount; i++)
      {
        int centerFreq = 0;
        hr = dflDescriptor.GetRecordCentreFrequency(i, out centerFreq);
        Debug.Assert((hr == 0), "IDvbFrequencyListDescriptor.GetRecordCentreFrequency failed");
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
          // see the doc for the meaning of the 0x62
          hr = nit.GetRecordDescriptorByTag(i, 0x62, null, out descriptor);
          if (descriptor != null)
            break;
        }

        Marshal.ReleaseComObject(nit);

        if (descriptor != null)
          break;
      }

      dflDescriptor = descriptor as IDvbFrequencyListDescriptor;
      Debug.Assert(dflDescriptor != null, "Can't get a IDvbFrequencyListDescriptor object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(dflDescriptor);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }

  }
}
