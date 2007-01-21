using System;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public enum BDANetworkType
  {
    ATSC,
    DVBT,
    DVBS,
    DVBC,
    OpenCable
  }

	public sealed class BDAUtils
	{
    private BDAUtils(){}

    public static IBaseFilter GetNetworkProvider(BDANetworkType type)
    {
      IBaseFilter networkProvider = null;

      try
      {
        // Using Generic Network Provider (available in MCE 2005 & Vista)
        networkProvider = (IBaseFilter)new NetworkProvider();
      }
      catch{}

      if (networkProvider != null)
        return networkProvider;

      switch (type)
      {
        case (BDANetworkType.ATSC):
          return (IBaseFilter) new ATSCNetworkProvider(); 
        case (BDANetworkType.DVBT):
          return (IBaseFilter) new DVBTNetworkProvider(); 
        case (BDANetworkType.DVBS):
          return (IBaseFilter) new DVBSNetworkProvider();
        case (BDANetworkType.DVBC):
          return (IBaseFilter)new DVBCNetworkProvider();
        default:
          return null;
      }
    }

    public static ITuningSpace GetTuningSpace(BDANetworkType type)
    {
      int hr = 0;
      ITuningSpace ts = null;

      switch (type)
      {
        case BDANetworkType.ATSC :
        {
          ts = (ITuningSpace) new ATSCTuningSpace();
          hr = ts.put_UniqueName(type.ToString() + "TuningSpace");
          hr = ts.put_FriendlyName(type.ToString() + "TuningSpace");
          hr = ts.put__NetworkType(typeof(ATSCNetworkProvider).GUID);
          break;
        }

        case BDANetworkType.DVBT :
        {
          ts = (ITuningSpace) new DVBTuningSpace();
          hr = ts.put_UniqueName(type.ToString() + "TuningSpace");
          hr = ts.put_FriendlyName(type.ToString() + "TuningSpace");
          hr = ts.put__NetworkType(typeof(DVBTNetworkProvider).GUID);
          hr = (ts as IDVBTuningSpace).put_SystemType(DVBSystemType.Terrestrial);
          break;
        }

        case BDANetworkType.DVBS :
        {
          ts = (ITuningSpace) new DVBSTuningSpace();
          hr = ts.put_UniqueName(type.ToString() + "TuningSpace");
          hr = ts.put_FriendlyName(type.ToString() + "TuningSpace");
          hr = ts.put__NetworkType(typeof(DVBSNetworkProvider).GUID);
          hr = (ts as IDVBTuningSpace).put_SystemType(DVBSystemType.Satellite);
          break;
        }

        case BDANetworkType.DVBC :
        {
          throw new NotImplementedException();
        }

        case BDANetworkType.OpenCable :
        {
          throw new NotImplementedException();
        }
      }

      return ts;
    }

    public static ITuneRequest CreateTuneRequest(ITuningSpace ts)
    {
      int hr = 0;
      ITuneRequest tr = null;
      Guid networkType;

      hr = ts.CreateTuneRequest(out tr);
      DsError.ThrowExceptionForHR(hr);

      hr = ts.get__NetworkType(out networkType);

      if (networkType == typeof(ATSCNetworkProvider).GUID)
      {
        // I know nothing about ATSC so thoses lines are pure speculation
        hr = (tr as IATSCChannelTuneRequest).put_Channel(-1);

        IATSCLocator locator = (IATSCLocator) new ATSCLocator();
        hr = locator.put_CarrierFrequency(-1);
        hr = tr.put_Locator(locator as ILocator);
      }
      else if (networkType == typeof(DVBTNetworkProvider).GUID)
      {
        hr = (tr as IDVBTuneRequest).put_ONID(-1);
        hr = (tr as IDVBTuneRequest).put_TSID(-1);
        hr = (tr as IDVBTuneRequest).put_SID(-1);

        IDVBTLocator locator = (IDVBTLocator) new DVBTLocator();
        hr = locator.put_CarrierFrequency(-1);
        hr = tr.put_Locator(locator as ILocator);
      }
      else if (networkType == typeof(DVBSNetworkProvider).GUID)
      {
        hr = (tr as IDVBTuneRequest).put_ONID(-1);
        hr = (tr as IDVBTuneRequest).put_TSID(-1);
        hr = (tr as IDVBTuneRequest).put_SID(-1);

        IDVBSLocator locator = (IDVBSLocator) new DVBSLocator();
        hr = locator.put_CarrierFrequency(-1);
        hr = locator.put_SymbolRate(-1);
        hr = tr.put_Locator(locator as ILocator);
      }

      return tr;
    }

    public static void AddBDATunerAndDemodulatorToGraph(IFilterGraph2 graphBuilder, IBaseFilter networkProvider, out IBaseFilter tuner, out IBaseFilter capture)
    {
      int hr = 0;
      DsDevice[] devices;
      ICaptureGraphBuilder2 capBuilder = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
      capBuilder.SetFiltergraph(graphBuilder);

      try
      {
        tuner = null;
        capture = null;

        devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

        for(int i = 0; i < devices.Length; i++)
        {
          IBaseFilter tmp;

          hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);
          DsError.ThrowExceptionForHR(hr);

          hr = capBuilder.RenderStream(null, null, networkProvider, null, tmp);
          if (hr == 0)
          {
            tuner = tmp;
            break;
          }
          else
          {
            hr = graphBuilder.RemoveFilter(tmp);
            Marshal.ReleaseComObject(tmp);
          }
        }

        devices = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);

        for(int i = 0; i < devices.Length; i++)
        {
          IBaseFilter tmp;

          hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);
          DsError.ThrowExceptionForHR(hr);

          hr = capBuilder.RenderStream(null, null, tuner, null, tmp);
          if (hr == 0)
          {
            capture = tmp;
            break;
          }
          else
          {
            hr = graphBuilder.RemoveFilter(tmp);
            Marshal.ReleaseComObject(tmp);
          }
        }
      }
      finally
      {
        Marshal.ReleaseComObject(capBuilder);
      }
    }

    public static void AddTransportStreamFiltersToGraph(IFilterGraph2 graphBuilder, out IBaseFilter bdaTIF, out IBaseFilter bdaSecTab)
    {
      int hr = 0;
      DsDevice[] devices;

      bdaTIF = null;
      bdaSecTab = null;

      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDATransportInformationRenderersCategory);
      for(int i = 0; i < devices.Length; i++)
      {
        if (devices[i].Name.Equals("BDA MPEG2 Transport Information Filter"))
        {
          hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out bdaTIF);
          DsError.ThrowExceptionForHR(hr);
          continue;
        }

        if (devices[i].Name.Equals("MPEG-2 Sections and Tables"))
        {
          hr = graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out bdaSecTab);
          DsError.ThrowExceptionForHR(hr);
          continue;
        }
      }
    }

    public static void AddNetworkFiltersToGraph(IFilterGraph2 graphBuilder, out IBaseFilter bdaMPE, out IBaseFilter bdaIPSink)
    {
      int hr = 0;
      DsDevice[] devices;

      bdaMPE = null;
      bdaIPSink = null;

      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDAReceiverComponentsCategory);
      foreach (DsDevice device in devices)
      {
        if (device.Name.IndexOf("BDA MPE") != -1)
        {
          hr = graphBuilder.AddSourceFilterForMoniker(device.Mon, null, "BDA MPE Filter", out bdaMPE);
          DsError.ThrowExceptionForHR(hr);
          break;
        }
      }
      
      devices = DsDevice.GetDevicesOfCat(FilterCategory.BDARenderingFiltersCategory);
      foreach (DsDevice device in devices)
      {
        if (device.Name.Equals("BDA IP Sink"))
        {
          hr = graphBuilder.AddSourceFilterForMoniker(device.Mon, null, "BDA IP Sink", out bdaIPSink);
          DsError.ThrowExceptionForHR(hr);
          break;
        }
      }
    }

	}
}
