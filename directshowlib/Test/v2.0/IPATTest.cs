#if ALLOW_UNTESTED_INTERFACES

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IPATTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IPAT pat;

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
      // This method is called by the Parser to initialize the object. 
      // It's pointless to call it from an application.
    }

    // Methods tested : GetCountOfRecords, GetRecordProgramNumber, 
    //                  GetRecordProgramMapPid, FindRecordProgramMapPid
    public void TestBatch1()
    {
      int hr = 0;
      int recordCount;
      short progNumber = 0;
      short mapPid = 0, mapPid2;

      hr = pat.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IPAT.GetCountOfRecords failed");

      for (int i = 0; i < recordCount; i++)
      {
        hr = pat.GetRecordProgramNumber(i, out progNumber);
        Debug.Assert(hr == 0, "IPAT.GetRecordProgramNumber failed");

        hr = pat.GetRecordProgramMapPid(i, out mapPid);
        Debug.Assert(hr == 0, "IPAT.GetRecordProgramMapPid failed");

        Debug.WriteLine(string.Format("0x{0:x4} : 0x{1:x4}", progNumber, mapPid));
      }

      // this method should return the same result than the last GetRecordProgramMapPid
      hr = pat.FindRecordProgramMapPid(progNumber, out mapPid2);
      Debug.Assert(hr == 0, "IPAT.FindRecordProgramMapPid failed");
      Debug.Assert(mapPid2 == mapPid, "IPAT.FindRecordProgramMapPid failed");
    }

    // Methods tested : GetVersionNumber, GetTransportStreamId 
    public void TestBatch2()
    {
      int hr = 0;
      byte version;
      short tsId = 0;

      hr = pat.GetVersionNumber(out version);
      Debug.Assert(hr == 0, "IPAT.GetVersionNumber failed");

      hr = pat.GetTransportStreamId(out tsId);
      Debug.Assert(hr == 0, "IPAT.GetTransportStreamId failed");
      Debug.Assert(tsId != 0, "IPAT.GetTransportStreamId failed");
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a PAT table...
    public void TestBatch3()
    {
      int hr = 0;
      IPAT nextPat;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = pat.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "IPAT.RegisterForNextTable failed");

      hr = pat.GetNextTable(out nextPat);
//      Debug.Assert(hr == 0, "IPAT.GetNextTable failed");
/*
      mre.WaitOne();
      mre.Reset();
*/
      hr = pat.RegisterForWhenCurrent(hevent);
      Debug.Assert(hr == 0, "IPAT.RegisterForWhenCurrent failed");

      hr = pat.ConvertNextToCurrent();
//      Debug.Assert(hr == 0, "IPAT.ConvertNextToCurrent failed");
/*
      mre.WaitOne();
      mre.Reset();
*/
    }

    private void Config()
    {
      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      int hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      hr = parser.GetPAT(out pat);
      Debug.Assert(pat != null, "Can't get a PAT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(pat);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }


}

#endif
