using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDVB_NITTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDVB_NIT nit;

    public void DoTests()
    {
      Config();

      TestInitialize();

      TestBatch1();
      TestBatch2();
      TestBatch3();
      TestBatch4();

      Unconfig();
    }

    public void TestInitialize()
    {
      // This method is called by the Parser to initialize the object. 
      // It's pointless to call it from an application.
    }

    // Methods tested : GetNetworkId, GetVersionNumber, GetVersionHash
    public void TestBatch1()
    {
      int hr = 0;
      short networkId = 0;
      byte versionNumber = 0;
      int versionHash = 0;

      hr = nit.GetNetworkId(out networkId);
      Debug.Assert((hr == 0) && (networkId != 0), "IDVB_NIT.GetNetworkId failed");

      hr = nit.GetVersionNumber(out versionNumber);
      Debug.Assert((hr == 0) && (versionNumber != 0), "IDVB_NIT.GetVersionNumber failed");

      hr = nit.GetVersionHash(out versionHash);
      Debug.Assert((hr == 0) && (versionHash != 0), "IDVB_NIT.GetVersionHash failed");
    }

    // Methods tested : GetCountOfRecords, GetRecordOriginalNetworkId,
    //                  GetRecordTransportStreamId, GetRecordCountOfDescriptors
    //                  GetRecordDescriptorByIndex, GetRecordDescriptorByTag
    public void TestBatch2()
    {
      int hr = 0;
      int recordCount = 0;

      hr = nit.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IDVB_NIT.GetCountOfRecords failed");

      for (int i = 0; i < recordCount; i++)
      {
        short originalNetworkId = 0;
        hr = nit.GetRecordOriginalNetworkId(i, out originalNetworkId);
        Debug.Assert((hr == 0) && (originalNetworkId != 0), "IDVB_NIT.GetRecordOriginalNetworkId failed");

        short transportStreamId = 0;
        hr = nit.GetRecordTransportStreamId(i, out transportStreamId);
        Debug.Assert((hr == 0) && (transportStreamId != 0), "IDVB_NIT.GetRecordTransportStreamId failed");

        int descriptorCount = 0;
        hr = nit.GetRecordCountOfDescriptors(i, out descriptorCount);
        Debug.Assert((hr == 0) && (descriptorCount != 0), "IDVB_NIT.GetRecordCountOfDescriptors failed");

        IGenericDescriptor descriptor = null;
        byte tag = 0;

        for (int j = 0; j < descriptorCount; j++)
        {
          hr = nit.GetRecordDescriptorByIndex(i, j, out descriptor);
          Debug.Assert((hr == 0) && (descriptor != null), "IDVB_NIT.GetRecordDescriptorByIndex failed");

          // save it for later...
          hr = descriptor.GetTag(out tag);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(descriptor);
        }

        hr = nit.GetRecordDescriptorByTag(i, tag, null, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IDVB_NIT.GetRecordDescriptorByTag failed");

        Marshal.ReleaseComObject(descriptor);
      }

    }

    // Methods tested : GetCountOfTableDescriptors, GetTableDescriptorByIndex
    //                  GetTableDescriptorByTag
    public void TestBatch3()
    {
      int hr = 0;
      int tableDescriptorsCount = 0;

      hr = nit.GetCountOfTableDescriptors(out tableDescriptorsCount);
      Debug.Assert((hr == 0) && (tableDescriptorsCount != 0), "IDVB_NIT.GetCountOfTableDescriptors failed");

      IGenericDescriptor descriptor = null;
      byte tag = 0;

      for (int i = 0; i < tableDescriptorsCount; i++)
      {
        hr = nit.GetTableDescriptorByIndex(i, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IDVB_NIT.GetTableDescriptorByIndex failed");

        // save it for later...
        hr = descriptor.GetTag(out tag);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(descriptor);
      }

      hr = nit.GetTableDescriptorByTag(tag, null, out descriptor);
      Debug.Assert((hr == 0) && (descriptor != null), "IDVB_NIT.GetTableDescriptorByTag failed");

      Marshal.ReleaseComObject(descriptor);
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a NIT table...
    public void TestBatch4()
    {
      int hr = 0;
      IDVB_NIT nextNit;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = nit.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "IDVB_NIT.RegisterForNextTable failed");

      hr = nit.GetNextTable(out nextNit);
      //Debug.Assert(hr == 0, "IDVB_NIT.GetNextTable failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
      hr = nit.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "IDVB_NIT.RegisterForWhenCurrent failed");

      hr = nit.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "IDVB_NIT.ConvertNextToCurrent failed");
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

      hr = parser.GetNIT(0x40, null, out nit);
      Debug.Assert(nit != null, "Can't get a NIT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(nit);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}
