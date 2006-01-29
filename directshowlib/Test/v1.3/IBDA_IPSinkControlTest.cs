using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_IPSinkControlTest
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
    private IBDA_IPSinkControl sinkControl = null;

    private const int E_PROP_ID_UNSUPPORTED = -2147023728;

		public IBDA_IPSinkControlTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();

      try
      {
        TestGetAdapterIPAddress();
        TestGetMulticastList();
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

          sinkControl = (IBDA_IPSinkControl) bdaIPSink;

          hr = capBuilder.RenderStream(null, null, mpeg2Demux, null, bdaMPE);
          DsError.ThrowExceptionForHR(hr);

          hr = capBuilder.RenderStream(null, null, bdaMPE, null, bdaIPSink);
          DsError.ThrowExceptionForHR(hr);

          Marshal.ReleaseComObject(capBuilder);
        }
      }
    }

    private void TestGetAdapterIPAddress()
    {
      int hr = 0;
      int size = 0;
      IntPtr buffer;

      hr = sinkControl.GetAdapterIPAddress(out size, out buffer);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_IPSinkControl.GetAdapterIPAddress");

      // this methods seem to always return size = 16.
      // 16 is the largest string an IPv4 can fill : aaa.bbb.ccc.ddd\0
      // but returned string can be shorter
      // In my case, the returned IP is 0.1.0.4 (confirmed by ipconfig)

      if (hr == 0)
      {
        int i = 0;
        byte[] charBuffer = new byte[size];

        for(i = 0; i < size; i++)
        {
          byte val = Marshal.ReadByte(buffer, i);
          if (val == 0) break;
          charBuffer[i] = val;
        }

        string ipAddress = Encoding.ASCII.GetString(charBuffer, 0, i);
        Debug.WriteLine("BDA IP Sink address is : " + ipAddress);
      }

      Marshal.FreeCoTaskMem(buffer);
    }

    private void TestGetMulticastList()
    {
      int hr = 0;
      int size = 0;
      IntPtr buffer;

      hr = sinkControl.GetMulticastList(out size, out buffer);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_IPSinkControl.GetMulticastList");

      // the returned address list is binary encodered (too much zero are returned to be a string)
      // this method return me size = 12 so since IPv4 addresses are 4 bytes long, 
      // this method return me 3 "multicast" addresses
      for(int i = 0; i < size / 4; i++)
      {
        Debug.WriteLine(string.Format("{0}.{1}.{2}.{3}", Marshal.ReadByte(buffer, (i * 4) + 0),
                                                         Marshal.ReadByte(buffer, (i * 4) + 1),
                                                         Marshal.ReadByte(buffer, (i * 4) + 2),
                                                         Marshal.ReadByte(buffer, (i * 4) + 3)));
      }

      // On my computer (the stanger IP addresses i seen since a long time) :
      // 1.0.94.0
      // 0.1.1.128
      // 194.0.0.3

      Marshal.FreeCoTaskMem(buffer);
    }

	}
}
