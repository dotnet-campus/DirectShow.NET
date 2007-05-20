using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class ITunerCapTest
  {
    private BdaGraph graph;
    private ITunerCap tunerCap;

    private const int E_NOTIMPL = unchecked((int)0x80004001);

    // NOTE : This interface (new to MCE2005 & Vista) seem to only be supported by the Generic Network 
    // Provider and not by the old network specific ones.
    public void DoTests()
    {
      Config();

      TestAuxInputCount();
      TestSupportedVideoFormats();
      TestSupportedNetworkTypes();

      Unconfig();
    }

    public void TestAuxInputCount()
    {
      int hr = 0;
      int compositeCount, sVideoCount;

      // This method is not implemented with my Hardware (an hybrid analog / digital board).
      hr = tunerCap.get_AuxInputCount(out compositeCount, out sVideoCount);
      Debug.Assert((hr == 0) || (hr == E_NOTIMPL), "ITunerCap.get_AuxInputCount failed");
    }

    public void TestSupportedVideoFormats()
    {
      int hr = 0;
      AMTunerModeType modeType;
      AnalogVideoStandard videoStandards;

      // This method is not implemented with my Hardware (an hybrid analog / digital board).
      hr = tunerCap.get_SupportedVideoFormats(out modeType, out videoStandards);
      Debug.Assert((hr == 0) || (hr == E_NOTIMPL), "ITunerCap.get_SupportedVideoFormats failed");
    }

    public void TestSupportedNetworkTypes()
    {
      int hr = 0;
      Guid[] networkTypes = new Guid[10];
      int networkTypesWritten = 0;

      // This method is IMPLEMENTED with my Hardware.
      // Network type should at least be the type currently tuned
      hr = tunerCap.get_SupportedNetworkTypes(networkTypes.Length, out networkTypesWritten, networkTypes);
      Debug.Assert((hr == 0) && (networkTypesWritten > 0) && (networkTypes[0] == typeof(DVBTNetworkProvider).GUID), "ITunerCap.get_SupportedNetworkTypes failed");
    }

    private void Config()
    {
      int hr = 0;

      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      //graph.RunGraph();

      tunerCap = (ITunerCap)graph.networkProvider;
    }

    private void Unconfig()
    {
      graph.DecomposeGraph();
    }
  }
}
