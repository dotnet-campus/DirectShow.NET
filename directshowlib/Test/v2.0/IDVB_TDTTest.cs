using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDVB_TDTTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDVB_TDT tdt;

    public void DoTests()
    {
      try
      {
        Config();

        TestInitialize();

        TestBatch1();
      }
      finally
      {
        Unconfig();
      }
    }

    public void TestInitialize()
    {
      // This method is called by the Parser to initialize the object. 
      // It's pointless to call it from an application.
    }

    // Methods tested : GetUTCTime
    public void TestBatch1()
    {
      int hr = 0;

      MpegDateAndTime dateAndTime = new MpegDateAndTime();

      hr = tdt.GetUTCTime(out dateAndTime);
      Debug.Assert((hr == 0 && (dateAndTime.ToDateTime() > DateTime.MinValue)), "IDVB_TDT.GetUTCTime failed");
    }

    private void Config()
    {
      int hr = 0;

      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      for (int i = 0; i < 10; i++)
      {
        hr = parser.GetTDT(out tdt);
        if (tdt != null)
          break;
      }

      Debug.Assert(tdt != null, "Can't get a TDT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(tdt);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}
