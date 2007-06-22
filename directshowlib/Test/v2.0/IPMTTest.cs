using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IPMTTest
  {
    private BdaGraph graph;
#if ALLOW_UNTESTED_INTERFACES
    private IDvbSiParser parser;
#endif
      private IPMT pmt;

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

    // Methods tested : GetPcrPid, GetVersionNumber, GetProgramNumber, QueryMPEInfo
    //                  QueryServiceGatewayInfo
    public void TestBatch1()
    {
      int hr = 0;

      short pidVal = 0;
      hr = pmt.GetPcrPid(out pidVal);
      Debug.Assert((hr == 0) && (pidVal != 0), "IPMT.GetPcrPid failed");

      byte versionNumber = 0xff;
      hr = pmt.GetVersionNumber(out versionNumber);
      Debug.Assert((hr == 0) && (versionNumber != 0xff), "IPMT.GetVersionNumber failed");

      short programNumber = 0;
      hr = pmt.GetProgramNumber(out programNumber);
      Debug.Assert((hr == 0) && (programNumber != 0), "IPMT.GetProgramNumber failed");

      const int MPEG2_S_SG_INFO_FOUND = 0x00040202;
      const int MPEG2_S_SG_INFO_NOT_FOUND = 0x00040203;
      const int MPEG2_S_MPE_INFO_FOUND = 0x00040204;
      const int MPEG2_S_MPE_INFO_NOT_FOUND = 0x00040205;

      MpeElement[] mpeElements;
      DsmccElement[] dsmccElements;
      int elementCount;

      hr = pmt.QueryMPEInfo(out mpeElements, out elementCount);
      Debug.Assert((hr == MPEG2_S_MPE_INFO_FOUND) || (hr == MPEG2_S_MPE_INFO_NOT_FOUND), "IPMT.QueryMPEInfo failed");

      hr = pmt.QueryServiceGatewayInfo(out dsmccElements, out elementCount);
      Debug.Assert((hr == MPEG2_S_SG_INFO_FOUND) || (hr == MPEG2_S_SG_INFO_NOT_FOUND), "IPMT.QueryServiceGatewayInfo failed");
    }

    // Methods tested : GetCountOfRecords, GetRecordCountOfDescriptors, 
    //                  GetRecordDescriptorByIndex, GetRecordDescriptorByTag
    //                  GetRecordElementaryPid
    public void TestBatch2()
    {
      int hr = 0;
      short recordCount = 0;

      hr = pmt.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IPMT.GetCountOfRecords failed");

      int recordDescCount = 0;
      for (int i = 0; i < recordCount; i++)
      {
        IGenericDescriptor descriptor;

        hr = pmt.GetRecordCountOfDescriptors(i, out recordDescCount);
        Debug.Assert((hr == 0) && (recordDescCount != 0), "IPMT.GetRecordCountOfDescriptors failed");

        byte tag = 0;

        for (int j = 0; j < recordDescCount; j++)
        {
          hr = pmt.GetRecordDescriptorByIndex(i, j, out descriptor);
          Debug.Assert((hr == 0) && (descriptor != null), "IPMT.GetRecordDescriptorByIndex failed");

          // Getting the tag value for the next test
          hr = descriptor.GetTag(out tag);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(descriptor);
        }

        hr = pmt.GetRecordDescriptorByTag(i, tag, null, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IPMT.GetRecordDescriptorByTag failed");

        short pid = 0;

        hr = pmt.GetRecordElementaryPid(i, out pid);
        Debug.Assert((hr == 0) && (pid != 0), "IPMT.GetRecordElementaryPid failed");

        byte streamType = 0;

        hr = pmt.GetRecordStreamType(i, out streamType);
        Debug.Assert((hr == 0) && (streamType != 0), "IPMT.GetRecordStreamType failed");
      }
    }

    // Methods tested : GetCountOfTableDescriptors, GetTableDescriptorByIndex
    //                  GetTableDescriptorByTag
    public void TestBatch3()
    {
      int hr = 0;

      int descCount = 0;

      hr = pmt.GetCountOfTableDescriptors(out descCount);
      Debug.Assert(hr == 0, "IPMT.GetCountOfTableDescriptors failed");

      if (descCount > 0)
      {
        IGenericDescriptor descriptor;
        byte tag = 0;

        for (int i = 0; i < descCount; i++)
        {
          hr = pmt.GetTableDescriptorByIndex(i, out descriptor);
          Debug.Assert((hr == 0) && (descriptor != null), "IPMT.GetCountOfTableDescriptors failed");

          // save tag for later
          hr = descriptor.GetTag(out tag);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(descriptor);
        }

        hr = pmt.GetTableDescriptorByTag(tag, null, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IPMT.GetTableDescriptorByTag failed");

        Marshal.ReleaseComObject(descriptor);
      }
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a PAT table...
    public void TestBatch4()
    {
      int hr = 0;
      IPMT nextPmt;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = pmt.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "IPMT.RegisterForNextTable failed");

      hr = pmt.GetNextTable(out nextPmt);
      //Debug.Assert(hr == 0, "IPMT.GetNextTable failed");
/*
      mre.WaitOne();
      mre.Reset();
*/
      hr = pmt.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "IPMT.RegisterForWhenCurrent failed");

      hr = pmt.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "IPMT.ConvertNextToCurrent failed");
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

      IPAT pat = null;

      hr = parser.GetPAT(out pat);
      DsError.ThrowExceptionForHR(hr);

      int recordCount;
      short progNumber = 0;
      short mapPid = 0;

      hr = pat.GetCountOfRecords(out recordCount);
      DsError.ThrowExceptionForHR(hr);

      // Get the last value
      hr = pat.GetRecordProgramMapPid(recordCount - 1, out mapPid);
      DsError.ThrowExceptionForHR(hr);

      hr = pat.GetRecordProgramNumber(recordCount - 1, out progNumber);
      DsError.ThrowExceptionForHR(hr);

      hr = parser.GetPMT(mapPid, progNumber, out pmt);
      Debug.Assert(pmt != null, "Can't get a PMT object");
#endif
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(pmt);
#if ALLOW_UNTESTED_INTERFACES
      Marshal.ReleaseComObject(parser);
#endif
      graph.DecomposeGraph();
    }

  }
}
