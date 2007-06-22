using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IGenericDescriptorTest
  {
    private BdaGraph graph;
#if ALLOW_UNTESTED_INTERFACES
    private IDvbSiParser parser;
#endif
    private IGenericDescriptor descriptor;

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

    // Methods tested : GetTag, GetLength, GetBody
    public void TestBatch1()
    {
      int hr = 0;

      byte tag = 0;
      hr = descriptor.GetTag(out tag);
      Debug.Assert((hr == 0) && (tag != 0), "IGenericDescriptor.GetTag failed");

      byte length = 0;
      hr = descriptor.GetLength(out length);
      Debug.Assert((hr == 0) && (length != 0), "IGenericDescriptor.GetLength failed");

      IntPtr body;
      hr = descriptor.GetBody(out body);
      Debug.Assert((hr == 0) && (body != IntPtr.Zero), "IGenericDescriptor.GetBody failed");

      Marshal.FreeCoTaskMem(body);
    }

    private void Config()
    {
#if ALLOW_UNTESTED_INTERFACES

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

      for (int i = 0; i < recordCount; i++)
      {
        hr = nit.GetRecordDescriptorByIndex(i, 0, out descriptor);
        if (descriptor != null)
          break;
      }

      Marshal.ReleaseComObject(nit);

      Debug.Assert(descriptor != null, "Can't get a GenericDescriptor object");
#endif
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(descriptor);
#if ALLOW_UNTESTED_INTERFACES
      Marshal.ReleaseComObject(parser);
#endif
      graph.DecomposeGraph();
    }
  }
}
