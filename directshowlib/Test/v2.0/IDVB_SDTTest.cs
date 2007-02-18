using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDVB_SDTTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDVB_SDT sdt;

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

    // Methods tested : GetOriginalNetworkId, GetTransportStreamId, 
    //                  GetVersionNumber, GetVersionHash
    public void TestBatch1()
    {
      int hr = 0;
      short networkId = 0;
      short transportStreamId = 0;
      byte versionNumber = 0;
      int versionHash = 0;

      hr = sdt.GetOriginalNetworkId(out networkId);
      Debug.Assert((hr == 0) && (networkId != 0), "IDVB_SDT.GetOriginalNetworkId failed");

      hr = sdt.GetTransportStreamId(out transportStreamId);
      Debug.Assert((hr == 0) && (transportStreamId != 0), "IDVB_SDT.GetTransportStreamId failed");

      hr = sdt.GetVersionNumber(out versionNumber);
      Debug.Assert((hr == 0) && (versionNumber != 0), "IDVB_SDT.GetVersionNumber failed");

      hr = sdt.GetVersionHash(out versionHash);
      Debug.Assert((hr == 0) && (versionHash != 0), "IDVB_SDT.GetVersionHash failed");
    }

    // Methods tested : GetCountOfRecords, GetRecordEITPresentFollowingFlag
    //                  GetRecordEITScheduleFlag, GetRecordFreeCAMode,
    //                  GetRecordRunningStatus, GetRecordServiceId, 
    //                  GetRecordCountOfDescriptors, GetRecordDescriptorByIndex
    //                  GetRecordDescriptorByTag
    public void TestBatch2()
    {
      int hr = 0;
      int recordCount = 0;

      hr = sdt.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IDVB_SDT.GetCountOfRecords failed");

      for (int i = 0; i < recordCount; i++)
      {
        // The results depend on too many things to test the returned values
        bool EITPresentFollowingFlag = false;
        hr = sdt.GetRecordEITPresentFollowingFlag(i, out EITPresentFollowingFlag);
        Debug.Assert((hr == 0), "IDVB_SDT.GetRecordEITPresentFollowingFlag failed");

        bool EIITScheduleFlag = false;
        hr = sdt.GetRecordEITScheduleFlag(i, out EIITScheduleFlag);
        Debug.Assert((hr == 0), "IDVB_SDT.GetRecordEITScheduleFlag failed");

        bool FreeCAMode = false;
        hr = sdt.GetRecordFreeCAMode(i, out FreeCAMode);
        Debug.Assert((hr == 0), "IDVB_SDT.GetRecordFreeCAMode failed");

        byte runningStatus = 0;
        hr = sdt.GetRecordRunningStatus(i, out runningStatus);
        Debug.Assert((hr == 0), "IDVB_SDT.GetRecordRunningStatus failed");

        short serviceId = 0;
        hr = sdt.GetRecordServiceId(i, out serviceId);
        Debug.Assert((hr == 0), "IDVB_SDT.GetRecordServiceId failed");

        int descriptorCount = 0;
        hr = sdt.GetRecordCountOfDescriptors(i, out descriptorCount);
        Debug.Assert((hr == 0) && (descriptorCount != 0), "IDVB_SDT.GetRecordServiceId failed");

        IGenericDescriptor descriptor = null;
        byte tag = 0;

        for (int j = 0; j < descriptorCount; j++)
        {
          hr = sdt.GetRecordDescriptorByIndex(i, j, out descriptor);
          Debug.Assert((hr == 0) && (descriptor != null), "IDVB_SDT.GetRecordDescriptorByIndex failed");

          // save it for later
          hr = descriptor.GetTag(out tag);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(descriptor);
        }

        hr = sdt.GetRecordDescriptorByTag(i, tag, null, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IDVB_SDT.GetRecordDescriptorByTag failed");

        Marshal.ReleaseComObject(descriptor);
      }
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a SDT table...
    public void TestBatch3()
    {
      int hr = 0;
      IDVB_SDT nextSdt;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = sdt.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "IDVB_SDT.RegisterForNextTable failed");

      hr = sdt.GetNextTable(out nextSdt);
      //Debug.Assert(hr == 0, "IDVB_SDT.GetNextTable failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
      hr = sdt.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "IDVB_SDT.RegisterForWhenCurrent failed");

      hr = sdt.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "IDVB_SDT.ConvertNextToCurrent failed");
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

      hr = parser.GetSDT(0x42, null, out sdt);
      Debug.Assert(sdt != null, "Can't get a SDT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(sdt);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }


  }
}
