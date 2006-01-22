using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_IPSinkInfoTest
	{
    // Use this field to change the network type...
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

    private IBaseFilter bdaMPE, bdaIPSink;
    private IBDA_IPSinkInfo sinkInfo = null;
    
    public IBDA_IPSinkInfoTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();

      try
      {
        TestAdapterDescription();
        TestAdapterIPAddress();
        TestMulticastList();
      }
      finally
      {
        Marshal.ReleaseComObject(bdaIPSink);
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
          }

          // Add network related filters
          BDAUtils.AddNetworkFiltersToGraph(graphBuilder, out bdaMPE, out bdaIPSink);

          sinkInfo = (IBDA_IPSinkInfo) bdaIPSink;

          hr = capBuilder.RenderStream(null, null, mpeg2Demux, null, bdaMPE);
          DsError.ThrowExceptionForHR(hr);

          hr = capBuilder.RenderStream(null, null, bdaMPE, null, bdaIPSink);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(capBuilder);
        }
      }
    }

    private void TestAdapterDescription()
    {
      int hr = 0;
      string desc;

      hr = sinkInfo.get_AdapterDescription(out desc);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(desc == "Microsoft TV/Video Connection", "IBDA_IPSinkInfo.get_AdapterDescription");
    }

    private void TestAdapterIPAddress()
    {
      int hr = 0;
      string address;

      hr = sinkInfo.get_AdapterIPAddress(out address);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_IPSinkInfo.get_AdapterIPAddress");
    }

    private void TestMulticastList()
    {
      int hr = 0;
      int addressCount;
      IntPtr addressList;

      hr = sinkInfo.get_MulticastList(out addressCount, out addressList);
      DsError.ThrowExceptionForHR(hr);

      // I don't know how the use thoses 6 bytes addresses...
      for (int i = 0; i < addressCount; i++)
      {

        Debug.WriteLine(string.Format("{0:x}-{1:x}-{2:x}-{3:x}-{4:x}-{5:x}", Marshal.ReadByte(addressList, i*6 + 0), 
                                                                             Marshal.ReadByte(addressList, i*6 + 1),
                                                                             Marshal.ReadByte(addressList, i*6 + 2),
                                                                             Marshal.ReadByte(addressList, i*6 + 3),
                                                                             Marshal.ReadByte(addressList, i*6 + 4),
                                                                             Marshal.ReadByte(addressList, i*6 + 5)));
/*
        Debug.WriteLine(string.Format("{0}.{1}.{2}.{3}:{4}", Marshal.ReadByte(addressList, i*6 + 0), 
                                                             Marshal.ReadByte(addressList, i*6 + 1),
                                                             Marshal.ReadByte(addressList, i*6 + 2),
                                                             Marshal.ReadByte(addressList, i*6 + 3),
                                                             Marshal.ReadInt16(addressList, i*6 + 4)));
*/
        Marshal.FreeCoTaskMem(addressList);
      }

      Debug.Assert(addressCount > 0, "IBDA_IPSinkInfo.get_MulticastList");
    }

	}
}
