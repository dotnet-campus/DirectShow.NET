#if ALLOW_UNTESTED_INTERFACES

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDVB_STTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDVB_ST st;

    public void DoTests()
    {
      Config();

      TestInitialize();

      TestBatch1();

      Unconfig();
    }

    public void TestInitialize()
    {
      // This method is called by the Parser to initialize the object. 
      // It's pointless to call it from an application.
    }

    // Methods tested : GetDataLength, GetData
    public void TestBatch1()
    {
      int hr = 0;
      short dataLength = 0;

      hr = st.GetDataLength(out dataLength);
      Debug.Assert((hr == 0) && (dataLength > 0), "IDVB_ST.GetDataLength failed");

      IntPtr data;
      hr = st.GetData(out data);
      Debug.Assert((hr == 0) && (data != IntPtr.Zero), "IDVB_ST.GetData failed");

      Marshal.FreeCoTaskMem(data);
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

      for (short i = 0x10; i <= 0x14; i++)
      {
        hr = parser.GetST(i, 10 * 1000, out st);
        if (st != null)
          break;
      }

      Debug.Assert(st != null, "Can't get a ST object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(st);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}

#endif
