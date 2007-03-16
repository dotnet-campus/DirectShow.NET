using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IDvbSiParserTest
  {
    private IDvbSiParser parser;
    private BdaGraph graph;
    private IMPEG2PIDMap pidMapper;

    public void DoTests()
    {
      Config();

      TestInitialize();
      //TestGetBAT();
      //TestGetCAT();
      //TestGetDIT();

      TestGetPAT();
      TestGetPMT();

      //TestGetTDT();

      //TestGetTSDT();


      Unconfig();
    }

    private void TestInitialize()
    {
      int hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");
    }

    private void TestGetBAT()
    {
      int hr = 0;
      IDVB_BAT bat;

      hr = parser.GetBAT(null, out bat);
      //Debug.Assert((hr == 0) && (bat != null), "IDvbSiParser.GetBAT failed");
      if (bat != null) Marshal.ReleaseComObject(bat);

      IntPtr bouquetId = Marshal.AllocCoTaskMem(sizeof(short));
      Marshal.WriteInt16(bouquetId, 8442);

      hr = parser.GetBAT(null, out bat);
      //Debug.Assert((hr == 0) && (bat != null), "IDvbSiParser.GetBAT failed");
      Marshal.FreeCoTaskMem(bouquetId);
      if (bat != null) Marshal.ReleaseComObject(bat);
    }

    private void TestGetCAT()
    {
      int hr = 0;
      ICAT cat;

      hr = parser.GetCAT(1000 * 5, out cat);
      Debug.Assert((hr == 0) && (cat != null), "IDvbSiParser.GetCAT failed");
      if (cat != null) Marshal.ReleaseComObject(cat);
    }

    private void TestGetDIT()
    {
      int hr = 0;
      IDVB_DIT dit;

      hr = parser.GetDIT(1000 * 10, out dit);
      Debug.Assert((hr == 0) && (dit != null), "IDvbSiParser.GetDIT failed");
      if (dit != null) Marshal.ReleaseComObject(dit);
    }

    private void TestGetEIT()
    {
      int hr = 0;
      int[] pids = { 0x00 };
      IDVB_EIT eit;
      /*
            // Route PID 0x00 to the filter
            hr = pidMapper.MapPID(pids.Length, pids, MediaSampleContent.Mpeg2PSI);
            DsError.ThrowExceptionForHR(hr);
      */
      hr = parser.GetEIT(0x4e, null, out eit);
      Debug.Assert((hr == 0) && (eit != null), "IDvbSiParser.GetEIT failed (" + hr + ")");
      if (eit != null) Marshal.ReleaseComObject(eit);
    }

    private void TestGetNIT()
    {
      int hr = 0;
      int[] pids = { 0x00 };
      IDVB_NIT nit;
      /*
            // Route PID 0x00 to the filter
            hr = pidMapper.MapPID(pids.Length, pids, MediaSampleContent.Mpeg2PSI);
            DsError.ThrowExceptionForHR(hr);
      */
      hr = parser.GetNIT(0x40, null, out nit);
      Debug.Assert((hr == 0) && (nit != null), "IDvbSiParser.GetNIT failed (" + hr + ")");
      if (nit != null) Marshal.ReleaseComObject(nit);
    }

    private void TestGetPAT()
    {
      int hr = 0;
      int[] pids = { 0x00 };
      IPAT pat;
/*
      // Route PID 0x00 to the filter
      hr = pidMapper.MapPID(pids.Length, pids, MediaSampleContent.Mpeg2PSI);
      DsError.ThrowExceptionForHR(hr);
*/
      hr = parser.GetPAT(out pat);
      Debug.Assert((hr == 0) && (pat != null), "IDvbSiParser.GetPAT failed (" + hr + ")");

      int recordCount;
      hr = pat.GetCountOfRecords(out recordCount);

      short pmtPid;
      for (int i = 0; i < recordCount; i++)
      {
        hr = pat.GetRecordProgramMapPid(0, out pmtPid);
        Debug.WriteLine(string.Format("{0} : 0x{1:x2}", i, pmtPid));
      }
      if (pat != null) Marshal.ReleaseComObject(pat);
    }

    private void TestGetPMT()
    {
      int hr = 0;
      int[] pids = { 0x00 };
      IPMT pmt;
      /*
            // Route PID 0x00 to the filter
            hr = pidMapper.MapPID(pids.Length, pids, MediaSampleContent.Mpeg2PSI);
            DsError.ThrowExceptionForHR(hr);
      */

      hr = parser.GetPMT(0x10, null, out pmt);
      Debug.Assert((hr == 0) && (pmt != null), "IDvbSiParser.GetPMT failed (" + hr + ")");

      if (pmt != null) Marshal.ReleaseComObject(pmt);
    }


    private void TestGetTDT()
    {
      int hr = 0;
      int[] pids = { 0x14 };
      IDVB_TDT tdt;

      // Route PID 0x14 to the filter
      hr = pidMapper.MapPID(pids.Length, pids, MediaSampleContent.Mpeg2PSI);
      DsError.ThrowExceptionForHR(hr);

      hr = parser.GetTDT(out tdt);
      Debug.Assert((hr == 0) && (tdt != null), "IDvbSiParser.GetTDT failed (" + hr + ")");
      if (tdt != null) Marshal.ReleaseComObject(tdt);

    }

    private void TestGetTSDT()
    {
      int hr = 0;
      int[] pids = { 0x02 };
      ITSDT tsdt;

      hr = pidMapper.MapPID(pids.Length, pids, MediaSampleContent.ElementaryStream);
      DsError.ThrowExceptionForHR(hr);

      hr = parser.GetTSDT(out tsdt);
      Debug.Assert((hr == 0) && (tsdt != null), "IDvbSiParser.GetTSDT failed (" + hr + ")");
      if (tsdt != null) Marshal.ReleaseComObject(tsdt);

    }

    private void Config()
    {
      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
/*
      // The second pin go to the Mpeg-2 Tables and Sections filter
      IPin demuxPin = DsFindPin.ByDirection(graph.mpeg2Demux, PinDirection.Output, 1);
      pidMapper = (IMPEG2PIDMap)demuxPin;
*/

      int hr = 0;
      int regCookie;
      object mpeg2DataControl;

      IPin mpegPin = DsFindPin.ByDirection(graph.bdaSecTab, PinDirection.Input, 0);
      hr = (graph.networkProvider as IBDA_TIF_REGISTRATION).RegisterTIFEx(mpegPin, out regCookie, out mpeg2DataControl);
      DsError.ThrowExceptionForHR(hr);

      pidMapper = (IMPEG2PIDMap)mpeg2DataControl;

      graph.MakeTuneRequest();
      graph.RunGraph();
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(parser);
      Marshal.ReleaseComObject(pidMapper);
      graph.DecomposeGraph();
    }

    private void DumpPIDs()
    {
      int hr = 0;
      IEnumPIDMap enumPIDMap;
      IEnumPIDMap enumPIDMap2;
      PIDMap[] pids = new PIDMap[1];

      hr = pidMapper.EnumPIDMap(out enumPIDMap);
      DsError.ThrowExceptionForHR(hr);

      hr = enumPIDMap.Clone(out enumPIDMap2);

      while (enumPIDMap2.Next(pids.Length, pids, IntPtr.Zero) == 0)
      {
        Debug.WriteLine(string.Format("{0} : {1}", pids[0].ulPID, pids[0].MediaSampleContent));
      }

      Marshal.ReleaseComObject(enumPIDMap);
    }
  }
}
