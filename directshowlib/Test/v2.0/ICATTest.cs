using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  // This need a multiplex with CAT tables (Conditional Access = Pay TV)...
  class ICATTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private ICAT cat;

    public void DoTests()
    {
      Config();

      TestInitialize();

      TestBatch1();
      TestBatch2();
      TestBatch3();

      Unconfig();
    }

    public void TestInitialize()
    {
      // This method is called by the Parser to initialize the object. 
      // It's pointless to call it from an application.
    }

    // Methods tested : GetVersionNumber
    public void TestBatch1()
    {
      int hr = 0;

      byte versionNum = 0;
      hr = cat.GetVersionNumber(out versionNum);
      Debug.Assert((hr == 0 && (versionNum != 0)), "ICAT.GetVersionNumber failed");
    }

    // Methods tested : GetCountOfTableDescriptors, GetTableDescriptorByIndex
    //                  GetTableDescriptorByTag
    public void TestBatch2()
    {
      int hr = 0;

      int descriptorCount = 0;
      hr = cat.GetCountOfTableDescriptors(out descriptorCount);
      Debug.Assert((hr == 0 && (descriptorCount != 0)), "ICAT.GetCountOfTableDescriptors failed");

      IGenericDescriptor descriptor = null;
      byte tag = 0;

      for (int i = 0; i < descriptorCount; i++)
      {
        hr = cat.GetTableDescriptorByIndex(i, out descriptor);
        Debug.Assert((hr == 0 && (descriptor != null)), "ICAT.GetTableDescriptorByIndex failed");

        hr = descriptor.GetTag(out tag);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(descriptor);
      }

      hr = cat.GetTableDescriptorByTag(tag, null, out descriptor);
      Debug.Assert((hr == 0 && (descriptor != null)), "ICAT.GetTableDescriptorByTag failed");

      Marshal.ReleaseComObject(descriptor);
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a NIT table...
    public void TestBatch3()
    {
      int hr = 0;
      ICAT nextCat;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = cat.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "ICAT.RegisterForNextTable failed");

      hr = cat.GetNextTable(5 * 1000, out nextCat);
      //Debug.Assert(hr == 0, "ICAT.GetNextTable failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
      hr = cat.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "ICAT.RegisterForWhenCurrent failed");

      hr = cat.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "ICAT.ConvertNextToCurrent failed");
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
      Debug.Assert(hr == 0, "Initialize failed !!!");

      hr = parser.GetCAT(5*1000, out cat);
      Debug.Assert(cat != null, "Can't get a CAT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(cat);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}
