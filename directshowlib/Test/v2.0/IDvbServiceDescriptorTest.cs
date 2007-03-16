using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDvbServiceDescriptorTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDvbServiceDescriptor dsDescriptor;

    public void DoTests()
    {
      Config();

//      TestBatch1();
//      TestBatch2();

      Unconfig();
    }

    private void Config()
    {
      int hr = 0;
      IDVB_BAT bat;
      IGenericDescriptor descriptor = null;

      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      while (true)
      {
        hr = parser.GetBAT(null, out bat);
        Debug.Assert(bat != null, "Can't get a BAT object");

        int recordCount = 0;
        hr = bat.GetCountOfRecords(out recordCount);

        for (int i = 0; i < recordCount; i++)
        {
          // see the doc for the meaning of the 0x47
          hr = bat.GetRecordDescriptorByTag(i, 0x47, null, out descriptor);
          if (descriptor != null)
            break;
        }

        Marshal.ReleaseComObject(bat);

        if (descriptor != null)
          break;
      }

      dsDescriptor = descriptor as IDvbServiceDescriptor;
      Debug.Assert(dsDescriptor != null, "Can't get a DvbServiceDescriptor object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(dsDescriptor);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}