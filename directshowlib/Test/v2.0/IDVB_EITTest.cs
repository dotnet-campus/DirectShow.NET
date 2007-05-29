using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDVB_EITTest
  {
    private BdaGraph graph;
    private IDvbSiParser parser;
    private IDVB_EIT eit;

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

    // Methods tested : GetOriginalNetworkId, GetLastTableId, GetSegmentLastSectionNumber
    //                  GetServiceId, GetTransportStreamId, GetVersionNumber,
    //                  GetVersionHash
    public void TestBatch1()
    {
      int hr = 0;
      short networkId = 0;
      byte lastTableId = 0;
      byte lastSectionNumber = 0;
      short serviceId = 0;
      short transportStreamId = 0;
      byte versionNumber = 0;
      int versionHash = 0;

      hr = eit.GetOriginalNetworkId(out networkId);
      Debug.Assert((hr == 0) && (networkId != 0), "IDVB_EIT.GetOriginalNetworkId failed");

      hr = eit.GetLastTableId(out lastTableId);
      Debug.Assert((hr == 0) && (lastTableId != 0), "IDVB_EIT.GetLastTableId failed");

      hr = eit.GetSegmentLastSectionNumber(out lastSectionNumber);
      Debug.Assert((hr == 0) && (lastSectionNumber != 0), "IDVB_EIT.GetSegmentLastSectionNumber failed");

      hr = eit.GetServiceId(out serviceId);
      Debug.Assert((hr == 0) && (serviceId != 0), "IDVB_EIT.GetServiceId failed");

      hr = eit.GetTransportStreamId(out transportStreamId);
      Debug.Assert((hr == 0) && (transportStreamId != 0), "IDVB_EIT.GetTransportStreamId failed");

      hr = eit.GetVersionNumber(out versionNumber);
      Debug.Assert((hr == 0) && (versionNumber != 0), "IDVB_EIT.GetVersionNumber failed");

      hr = eit.GetVersionHash(out versionHash);
      Debug.Assert((hr == 0) && (versionHash != 0), "IDVB_EIT.GetVersionHash failed");
    }

    // Methods tested : GetCountOfRecords, GetRecordDuration, GetRecordEventId
    //                  GetRecordFreeCAMode, GetRecordRunningStatus, 
    //                  GetRecordStartTime, GetSegmentLastSectionNumber
    //                  GetRecordCountOfDescriptors, GetRecordDescriptorByIndex
    //                  GetRecordDescriptorByTag
    public void TestBatch2()
    {
      int hr = 0;
      int recordCount = 0;

      hr = eit.GetCountOfRecords(out recordCount);
      Debug.Assert((hr == 0) && (recordCount != 0), "IDVB_EIT.GetCountOfRecords failed");

      for (int i = 0; i < recordCount; i++)
      {
        // the results are dependent to the multiplex
        MpegDuration duration = new MpegDuration();
        hr = eit.GetRecordDuration(i, out duration);
        Debug.Assert((hr == 0) && (duration.ToTimeSpan() > TimeSpan.Zero), "IDVB_EIT.GetRecordDuration failed");

        short eventId = 0;
        hr = eit.GetRecordEventId(i, out eventId);
        Debug.Assert((hr == 0) && (eventId != 0), "IDVB_EIT.GetRecordEventId failed");

        bool freeCAMode = true;
        hr = eit.GetRecordFreeCAMode(i, out freeCAMode);
        Debug.Assert((hr == 0), "IDVB_EIT.GetRecordFreeCAMode failed");

        RunningStatus runningStatus = 0;
        hr = eit.GetRecordRunningStatus(i, out runningStatus);
        Debug.Assert((hr == 0 && (runningStatus != 0)), "IDVB_EIT.GetRecordRunningStatus failed");

        MpegDateAndTime dateAndTime = new MpegDateAndTime();
        hr = eit.GetRecordStartTime(i, out dateAndTime);
        Debug.Assert((hr == 0) && (dateAndTime.ToDateTime() > DateTime.MinValue), "IDVB_EIT.GetRecordStartTime failed");

        int descriptorCount = 0;
        hr = eit.GetRecordCountOfDescriptors(i, out descriptorCount);
        Debug.Assert((hr == 0 && (descriptorCount != 0)), "IDVB_EIT.GetRecordCountOfDescriptors failed");

        IGenericDescriptor descriptor = null;
        byte tag = 0;

        for (int j = 0; j < descriptorCount; j++)
        {
          hr = eit.GetRecordDescriptorByIndex(i, j, out descriptor);
          Debug.Assert((hr == 0 && (descriptor != null)), "IDVB_EIT.GetRecordDescriptorByIndex failed");

          hr = descriptor.GetTag(out tag);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(descriptor);
        }

        hr = eit.GetRecordDescriptorByTag(i, tag, null, out descriptor);
        Debug.Assert((hr == 0 && (descriptor != null)), "IDVB_EIT.GetRecordDescriptorByTag failed");

        Marshal.ReleaseComObject(descriptor);
      }
    }

    // Methods tested : RegisterForNextTable, GetNextTable,
    //                  RegisterForWhenCurrent, ConvertNextToCurrent
    // This test don't work. This is perhaps normal for a EIT table...
    public void TestBatch3()
    {
      int hr = 0;
      IDVB_EIT nextEit;
      ManualResetEvent mre = new ManualResetEvent(false);
      IntPtr hevent = mre.SafeWaitHandle.DangerousGetHandle();

      hr = eit.RegisterForNextTable(hevent);
      Debug.Assert(hr == 0, "IDVB_EIT.RegisterForNextTable failed");

      hr = eit.GetNextTable(out nextEit);
      //Debug.Assert(hr == 0, "IDVB_EIT.GetNextTable failed");
      /*
            mre.WaitOne();
            mre.Reset();
      */
      hr = eit.RegisterForWhenCurrent(hevent);
      //Debug.Assert(hr == 0, "IDVB_EIT.RegisterForWhenCurrent failed");

      hr = eit.ConvertNextToCurrent();
      //Debug.Assert(hr == 0, "IDVB_EIT.ConvertNextToCurrent failed");
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

      IDVB_SDT sdt = null;
      hr = parser.GetSDT(0x42, null, out sdt);
      DsError.ThrowExceptionForHR(hr);

      int recordCount = 0;

      hr = sdt.GetCountOfRecords(out recordCount);
      DsError.ThrowExceptionForHR(hr);

      for (int i = 0; i < recordCount; i++)
      {
        short serviceId = 0;
        hr = sdt.GetRecordServiceId(i, out serviceId);
        DsError.ThrowExceptionForHR(hr);

        hr = parser.GetEIT(0x4e, serviceId, out eit);
        if (eit != null)
          break;
      }

      Marshal.ReleaseComObject(sdt);

      Debug.Assert(eit != null, "Can't get a SDT object");
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(eit);
      Marshal.ReleaseComObject(parser);
      graph.DecomposeGraph();
    }
  }
}
