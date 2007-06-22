using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  // This need a multiplex with BAT tables...
  class IDVB_BATTest
  {
    private BdaGraph graph;
#if ALLOW_UNTESTED_INTERFACES
    private IDvbSiParser parser;
#endif
    private IDVB_BAT bat;

    public void DoTests()
    {
      try
      {
        Config();

        TestInitialize();

        TestBatch1();
        TestBatch2();
        TestBatch3();
        TestBatch4();
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

    // Methods tested : GetBouquetId, GetVersionNumber
    public void TestBatch1()
    {
      int hr = 0;

      short bouquetId = 0;
      hr = bat.GetBouquetId(out bouquetId);
      Debug.Assert((hr == 0 && (bouquetId != 0)), "IDVB_BAT.GetBouquetId failed");

      byte versionNum = 0;
      hr = bat.GetVersionNumber(out versionNum);
      Debug.Assert((hr == 0 && (versionNum != 0)), "IDVB_BAT.GetVersionNumber failed");
    }

    // Methods tested : GetCountOfRecords, GetRecordTransportStreamId
    //                  GetRecordOriginalNetworkId, GetRecordCountOfDescriptors
    //                  GetRecordDescriptorByIndex, GetRecordDescriptorByTag
    public void TestBatch2()
    {
      int hr = 0;

      int recordCount = 0;
      hr = bat.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0 && (recordCount != 0)), "IDVB_BAT.GetCountOfRecords failed");

      for (int i = 0; i < recordCount; i++)
      {
        short transportStreamId = 0;
        hr = bat.GetRecordTransportStreamId(i, out transportStreamId);
        Debug.Assert((hr == 0 && (transportStreamId != 0)), "IDVB_BAT.GetRecordTransportStreamId failed");

        short originalNetworkId = 0;
        hr = bat.GetRecordOriginalNetworkId(i, out originalNetworkId);
        Debug.Assert((hr == 0 && (originalNetworkId != 0)), "IDVB_BAT.GetRecordOriginalNetworkId failed");

        int descriptorCount = 0;
        hr = bat.GetRecordCountOfDescriptors(i, out descriptorCount);
        Debug.Assert((hr == 0 && (descriptorCount != 0)), "IDVB_BAT.GetRecordCountOfDescriptors failed");

        IGenericDescriptor descriptor = null;
        byte tag = 0;

        for (int j = 0; j < descriptorCount; j++)
        {
          hr = bat.GetRecordDescriptorByIndex(i, j, out descriptor);
          Debug.Assert((hr == 0 && (descriptor != null)), "IDVB_BAT.GetRecordDescriptorByIndex failed");

          hr = descriptor.GetTag(out tag);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(descriptor);
        }

        hr = bat.GetRecordDescriptorByTag(i, tag, null, out descriptor);
        Debug.Assert((hr == 0 && (descriptor != null)), "IDVB_BAT.GetRecordDescriptorByTag failed");

        Marshal.ReleaseComObject(descriptor);
      }

    }

    // Methods tested : GetCountOfTableDescriptors, GetTableDescriptorByIndex
    //                  GetTableDescriptorByTag
    public void TestBatch3()
    {
      int hr = 0;
      int tableDescriptorsCount = 0;

      hr = bat.GetCountOfTableDescriptors(out tableDescriptorsCount);
      Debug.Assert((hr == 0) && (tableDescriptorsCount != 0), "IDVB_BAT.GetCountOfTableDescriptors failed");

      IGenericDescriptor descriptor = null;
      byte tag = 0;

      for (int i = 0; i < tableDescriptorsCount; i++)
      {
        hr = bat.GetTableDescriptorByIndex(i, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IDVB_BAT.GetTableDescriptorByIndex failed");

        // save it for later...
        hr = descriptor.GetTag(out tag);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(descriptor);
      }

      hr = bat.GetTableDescriptorByTag(tag, null, out descriptor);
      Debug.Assert((hr == 0) && (descriptor != null), "IDVB_BAT.GetTableDescriptorByTag failed");

      Marshal.ReleaseComObject(descriptor);
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a NIT table...
    public void TestBatch4()
    {
      int hr = 0;
      IDVB_BAT nextBat;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = bat.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "IDVB_BAT.RegisterForNextTable failed");

      hr = bat.GetNextTable(out nextBat);
      //Debug.Assert(hr == 0, "IDVB_BAT.GetNextTable failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
      hr = bat.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "IDVB_BAT.RegisterForWhenCurrent failed");

      hr = bat.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "IDVB_BAT.ConvertNextToCurrent failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
    }

    private void Config()
    {
#if ALLOW_UNTESTED_INTERFACES

      int hr = 0;

      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      hr = parser.GetBAT(null, out bat);
      Debug.Assert(bat != null, "Can't get a BAT object");
#endif
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(bat);
#if ALLOW_UNTESTED_INTERFACES
      Marshal.ReleaseComObject(parser);
#endif
      graph.DecomposeGraph();
    }

  }
}
