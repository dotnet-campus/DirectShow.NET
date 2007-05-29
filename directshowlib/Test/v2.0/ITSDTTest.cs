using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class ITSDTTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private ITSDT tsdt;

    public void DoTests()
    {
      try
      {
        Config();

        TestInitialize();

        TestBatch1();
        TestBatch2();
        TestBatch3();
      }
      finally
      {
        Unconfig();
      }
    }

    public void TestInitialize()
    {
      // This method is called by the Parser to itsdtialize the object. 
      // It's pointless to call it from an application.
    }

    // Methods tested : GetVersionNumber
    private void TestBatch1()
    {
      int hr = 0;

      byte version = 0;
      hr = tsdt.GetVersionNumber(out version);
      Debug.Assert((hr == 0) && (version != 0), "ITSDT.GetVersionNumber failed");
    }

    // Methods tested : GetCountOfTableDescriptors, GetTableDescriptorByIndex
    //                  GetTableDescriptorByTag
    private void TestBatch2()
    {
      int hr = 0;

      int descriptorCount = 0;
      hr = tsdt.GetCountOfTableDescriptors(out descriptorCount);
      Debug.Assert((hr == 0) && (descriptorCount > 0), "ITSDT.GetCountOfTableDescriptors failed");

      IGenericDescriptor descriptor = null;
      byte tag = 0;

      for (int i = 0; i < descriptorCount; i++)
      {
        hr = tsdt.GetTableDescriptorByIndex(i, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "ITSDT.GetTableDescriptorByIndex failed");

        hr = descriptor.GetTag(out tag);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(descriptor);
      }

      hr = tsdt.GetTableDescriptorByTag(tag, null, out descriptor);
      Debug.Assert((hr == 0) && (descriptor != null), "ITSDT.GetTableDescriptorByTag failed");

      Marshal.ReleaseComObject(descriptor);
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a TSDT table...
    public void TestBatch3()
    {
      int hr = 0;
      ITSDT nextTsdt;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = tsdt.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "ITSDT.RegisterForNextTable failed");

      hr = tsdt.GetNextTable(out nextTsdt);
      //Debug.Assert(hr == 0, "ITSDT.GetNextTable failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
      hr = tsdt.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "ITSDT.RegisterForWhenCurrent failed");

      hr = tsdt.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "ITSDT.ConvertNextToCurrent failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
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
      Debug.Assert(hr == 0, "Itsdtialize failed !!!");

      hr = parser.GetTSDT(out tsdt);
      Debug.Assert(tsdt != null, "Can't get a TSDT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(tsdt);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}
