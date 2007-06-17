using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IDVB_DITTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDVB_DIT dit;

    public void DoTests()
    {
      try
      {
        Config();

        TestInitialize();
        TestTransitionFlag();
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

    public void TestTransitionFlag()
    {
      int hr = 0;

      bool transFlag = false;
      hr = dit.GetTransitionFlag(out transFlag);
      Debug.Assert((hr == 0), "IDVB_DIT.GetTransitionFlag failed");
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

      hr = parser.GetDIT(10 * 1000, out dit);
      Debug.Assert(dit != null, "Can't get a IDVB_DIT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(dit);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }

  }
}
