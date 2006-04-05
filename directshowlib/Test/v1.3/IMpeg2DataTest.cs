using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib.BDA;

// This is a test for IMpeg2Data, IMpeg2Stream and ISectionList
// IMpeg2Stream tests are called from TestGetStreamOfSection();
// ISectionList tests are called from TestGetSection();

namespace DirectShowLib.Test
{
  public class IMpeg2DataTest
  {
    private BDANetworkType networkType = BDANetworkType.DVBT;

    private IFilterGraph2 graphBuilder;
    private DsROTEntry rot;
    private IBaseFilter networkProvider;
    private ITuner tuner;

    private ITuningSpace tuningSpace;
    private ITuneRequest tuneRequest;

    private IBaseFilter bdaTuner, bdaCapture;
    private IBaseFilter mpeg2Demux;
    private IBaseFilter bdaTIF, bdaSecTab;

    private IMpeg2Data mpeg2Data;
    private IMpeg2Stream mpeg2Stream;
    private ISectionList sectionList;

    public IMpeg2DataTest()
    {
    }

    public void DoTests()
    {
      BuildGraph();
      MakeATuneRequestAndRunTheGraph();

      try
      {
        TestGetStreamOfSection();
        TestGetTable();
        TestGetSection();
      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(mpeg2Data);

        Marshal.ReleaseComObject(bdaSecTab);
        Marshal.ReleaseComObject(bdaTIF);
        Marshal.ReleaseComObject(mpeg2Demux);
        Marshal.ReleaseComObject(bdaCapture);
        Marshal.ReleaseComObject(bdaTuner);
        Marshal.ReleaseComObject(tuneRequest);
        Marshal.ReleaseComObject(tuningSpace);
        Marshal.ReleaseComObject(networkProvider);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      ICaptureGraphBuilder2 capBuilder = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
      capBuilder.SetFiltergraph(graphBuilder);

      // Get the BDA network provider specific for this given network type
      networkProvider = BDAUtils.GetNetworkProvider(networkType);

      hr = graphBuilder.AddFilter(networkProvider, "BDA Network Provider");
      DsError.ThrowExceptionForHR(hr);

      tuner = (ITuner) networkProvider;

      // Get a tuning space for this network type
      tuningSpace = BDAUtils.GetTuningSpace(networkType);

      hr = tuner.put_TuningSpace(tuningSpace);
      DsError.ThrowExceptionForHR(hr);

      // Create a tune request from this tuning space
      tuneRequest = BDAUtils.CreateTuneRequest(tuningSpace);

      // Is it okay ?
      hr = tuner.Validate(tuneRequest);
      if (hr == 0)
      {
        // Set it
        hr = tuner.put_TuneRequest(tuneRequest);
        DsError.ThrowExceptionForHR(hr);

        // found a BDA Tuner and a BDA Capture that can connect to this network provider
        BDAUtils.AddBDATunerAndDemodulatorToGraph(graphBuilder, networkProvider, out bdaTuner, out bdaCapture);

        if ((bdaTuner != null) && (bdaCapture != null)) 
        {
          // Create and add the mpeg2 demux
          mpeg2Demux = (IBaseFilter) new MPEG2Demultiplexer();

          hr = graphBuilder.AddFilter(mpeg2Demux, "MPEG2 Demultiplexer");
          DsError.ThrowExceptionForHR(hr);

          // connect it to the BDA Capture
          hr = capBuilder.RenderStream(null, null, bdaCapture, null, mpeg2Demux);
          DsError.ThrowExceptionForHR(hr);

          // Add the two mpeg2 transport stream helper filters
          BDAUtils.AddTransportStreamFiltersToGraph(graphBuilder, out bdaTIF, out bdaSecTab);

          if ((bdaTIF != null) && (bdaSecTab != null))
          {
            // Render all the output pins of the demux (missing filters are added)
            for (int i = 0; i < 5; i++)
            {
              IPin pin = DsFindPin.ByDirection(mpeg2Demux, PinDirection.Output, i);

              hr = graphBuilder.Render(pin);
              Marshal.ReleaseComObject(pin);
            }

            mpeg2Data = (IMpeg2Data) bdaSecTab;
          }

        }
      }
    }

    private void MakeATuneRequestAndRunTheGraph()
    {
      int hr = 0;
      ILocator locator;

      if (networkType == BDANetworkType.DVBT)
      {
        // Those values are valid for me but must be modified to be valid depending on your location
        hr = tuneRequest.get_Locator(out locator);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_CarrierFrequency(586000);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(1);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(260);
        DsError.ThrowExceptionForHR(hr);
      }

      if (networkType == BDANetworkType.DVBS)
      {
        // Those values are valid for me but must be modified to be valid depending on your Satellite dish
        hr = tuneRequest.get_Locator(out locator);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_CarrierFrequency(11591000);
        DsError.ThrowExceptionForHR(hr);

        hr = (locator as IDVBSLocator).put_SignalPolarisation(Polarisation.LinearV);
        DsError.ThrowExceptionForHR(hr);

        hr = locator.put_SymbolRate(20000);
        DsError.ThrowExceptionForHR(hr);

        hr = tuneRequest.put_Locator(locator);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(locator);

        hr = (tuneRequest as IDVBTuneRequest).put_ONID(8442);
        hr = (tuneRequest as IDVBTuneRequest).put_TSID(1);
        hr = (tuneRequest as IDVBTuneRequest).put_SID(260);
        DsError.ThrowExceptionForHR(hr);
      }

      if (tuner.Validate(tuneRequest) == 0)
      {
        hr = tuner.put_TuneRequest(tuneRequest);
        hr = (graphBuilder as IMediaControl).Run();
      }
      else
      {
        Debug.Fail("TuneRequest is not valid");
      }
    }

    // Tests for IMpeg2Data methods

    private void TestGetStreamOfSection()
    {
      int hr;
      ManualResetEvent mre = new ManualResetEvent(false);
      short PID = 0x12; // is the PID of a Event Information Table (EIT)
      byte TID = 0x4e; // is the TID for event_information_section - actual_transport_stream, present/following
      //short NID = 1;    // 1 is the Network ID of the broadcast selectionned

      MPEG2Filter mpeg2Filter = new MPEG2Filter();
      mpeg2Filter.bVersionNumber = 1;
      mpeg2Filter.wFilterSize = 124; // MPEG2_FILTER_VERSION_1_SIZE = Marshal.SizeOf(mpeg2Filter) = 124;
      //mpeg2Filter.fSpecifyTableIdExtension = true;
      //mpeg2Filter.TableIdExtension = NID;
      
      hr = mpeg2Data.GetStreamOfSections(PID, TID, mpeg2Filter, mre.SafeWaitHandle.DangerousGetHandle(), out mpeg2Stream);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) &&  (mpeg2Stream != null), "IMpeg2Data.GetStreamOfSections");

      TestIMpeg2Stream();

      Marshal.ReleaseComObject(mpeg2Stream);
    }

    private void TestGetTable()
    {
      int hr;
      ISectionList sectionList;

      short PID = 0x12; // is the PID of a Event Information Table (EIT)
      byte TID = 0x4e; // is the TID for event_information_section - actual_transport_stream, present/following

      MPEG2Filter mpeg2Filter = new MPEG2Filter();
      mpeg2Filter.bVersionNumber = 1;
      mpeg2Filter.wFilterSize = 124; // MPEG2_FILTER_VERSION_1_SIZE = Marshal.SizeOf(mpeg2Filter) = 124;

      hr = mpeg2Data.GetTable(PID, TID, mpeg2Filter, 10 * 1000, out sectionList);
      if (hr != unchecked((int)0x80040206)) //MPEG2_E_SECTION_NOT_FOUND
      {
        DsError.ThrowExceptionForHR(hr);

        Debug.Assert((hr == 0) &&  (sectionList != null), "IMpeg2Data.GetStreamOfSections");

        Marshal.ReleaseComObject(sectionList);
      }
    }

    private void TestGetSection()
    {
      int hr;
      short PID = 0x12; // is the PID of a Event Information Table (EIT)
      byte TID = 0x4e; // is the TID for event_information_section - actual_transport_stream, present/following

      MPEG2Filter mpeg2Filter = new MPEG2Filter();
      mpeg2Filter.bVersionNumber = 1;
      mpeg2Filter.wFilterSize = 124; // MPEG2_FILTER_VERSION_1_SIZE = Marshal.SizeOf(mpeg2Filter) = 124;

      hr = mpeg2Data.GetSection(PID, TID, mpeg2Filter, 10 * 1000, out sectionList);
      if (hr != unchecked((int)0x80040206)) //MPEG2_E_SECTION_NOT_FOUND
      {
        DsError.ThrowExceptionForHR(hr);

        Debug.Assert((hr == 0) &&  (sectionList != null), "IMpeg2Data.GetStreamOfSections");

        TestISectionList();

        Marshal.ReleaseComObject(sectionList);
      }
    }

    // Tests for IMpeg2Stream methods

    private void TestIMpeg2Stream()
    {
      TestInitialize();
      TestSupplyDataBuffer();
    }

    private void TestInitialize()
    {
      // Initialize method is normally only used by IMpeg2Data.GetStreamOfSections.
      // I don't see the purpose of testing it here...
    }

    private void TestSupplyDataBuffer()
    {
      int hr = 0;

      MPEGStreamBuffer buffer = new MPEGStreamBuffer();
      buffer.dwDataBufferSize = 4096;
      buffer.dwSizeOfDataRead = 0;
      buffer.pDataBuffer = Marshal.AllocCoTaskMem(buffer.dwDataBufferSize);

      for (int i=0; i<buffer.dwDataBufferSize; i++)
        Marshal.WriteByte(buffer.pDataBuffer, i, 0xaa);

      hr = mpeg2Stream.SupplyDataBuffer(buffer);
      DsError.ThrowExceptionForHR(hr);

      // It seem that data are not available immediatly so we have to wait a little.
      // I don't know if marshaling is involved.
      Thread.Sleep(500);

      Debug.Assert((hr == 0) &&  (buffer.dwSizeOfDataRead != 0), "IMpeg2Stream.SupplyDataBuffer");

      Marshal.FreeCoTaskMem(buffer.pDataBuffer);
    }

    // Tests for ISectionList methods

    private void TestISectionList()
    {
      TestGetNumberOfSections();
      TestGetProgramIdentifier();
      TestGetTableIdentifier();
      TestGetSectionData();
      TestCancelPendingRequest();
      TestInitialize2();
      TestInitializeWithRawSections();
    }

    private void TestGetNumberOfSections()
    {
      int hr = 0;
      short sectionCount = 0;

      hr = sectionList.GetNumberOfSections(out sectionCount);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(sectionCount != 0, "ISectionList.GetNumberOfSections");
    }

    private void TestGetProgramIdentifier()
    {
      int hr = 0;
      short pid = 0;

      hr = sectionList.GetProgramIdentifier(out pid);
      DsError.ThrowExceptionForHR(hr);

      // see TestGetSection()
      Debug.Assert(pid == 0x12, "ISectionList.GetNumberOfSections");
    }

    private void TestGetTableIdentifier()
    {
      int hr = 0;
      byte tid = 0;

      hr = sectionList.GetTableIdentifier(out tid);
      DsError.ThrowExceptionForHR(hr);

      // see TestGetSection()
      Debug.Assert(tid == 0x4e, "ISectionList.GetNumberOfSections");
    }

    private void TestGetSectionData()
    {
      int hr = 0;
      int packetLength;
      IntPtr sectionPtr;

      hr = sectionList.GetSectionData(0, out packetLength, out sectionPtr); 
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((packetLength > 0) && (sectionPtr != IntPtr.Zero), "ISectionList.GetSectionData");
    }

    private void TestCancelPendingRequest()
    {
      int hr = 0;

      hr = sectionList.CancelPendingRequest();
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "ISectionList.CancelPendingRequest");
    }

    private void TestInitialize2()
    {
      // Initialize method is normally only used by IMpeg2Data.GetSection or IMpeg2Data.GetTable.
      // I don't see the purpose of testing it here...
    }

    private void TestInitializeWithRawSections()
    {
      int hr = 0;
      MPEGPacketList packetList = new MPEGPacketList();
      packetList.wPacketCount = 0;
      packetList.PacketList = IntPtr.Zero;

      hr = sectionList.InitializeWithRawSections(ref packetList);

      // MPEG2_E_ALREADY_INITIALIZED
      Debug.Assert(hr == unchecked((int)0x80040201), "ISectionList.CancelPendingRequest");
    }

  }
}
