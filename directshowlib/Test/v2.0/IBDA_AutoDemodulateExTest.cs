#if ALLOW_UNTESTED_INTERFACES

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  public class IBDA_AutoDemodulateExTest
  {
    private BdaGraph graph;
    private IBDA_AutoDemodulateEx autoDemodulateEx = null;

    private const int E_NOTIMPL = unchecked((int)0x80004001);

    public void DoTests()
    {
      Config();

      TestAuxInputCount();
      TestSupportedVideoFormats();
      TestSupportedDeviceNodeTypes();

      Unconfig();
    }

    public void TestAuxInputCount()
    {
      int hr = 0;
      int compositeCount, sVideoCount;

      hr = autoDemodulateEx.get_AuxInputCount(out compositeCount, out sVideoCount);
      Debug.Assert((hr == 0) || (hr == E_NOTIMPL), "IBDA_AutoDemodulateEx.get_AuxInputCount failed");
    }

    public void TestSupportedVideoFormats()
    {
      int hr = 0;
      AMTunerModeType modeType;
      AnalogVideoStandard videoStandards;

      hr = autoDemodulateEx.get_SupportedVideoFormats(out modeType, out videoStandards);
      Debug.Assert((hr == 0) || (hr == E_NOTIMPL), "IBDA_AutoDemodulateEx.get_SupportedVideoFormats failed");
    }

    public void TestSupportedDeviceNodeTypes()
    {
      int hr = 0;
      Guid[] networkTypes;
      int networkTypesWritten = 0;

      hr = autoDemodulateEx.get_SupportedDeviceNodeTypes(0, out networkTypesWritten, null);
      Debug.Assert((hr == 0), "IBDA_AutoDemodulateEx.get_SupportedNetworkTypes failed");

      networkTypes = new Guid[networkTypesWritten];

      hr = autoDemodulateEx.get_SupportedDeviceNodeTypes(networkTypes.Length, out networkTypesWritten, networkTypes);
      Debug.Assert((hr == 0) && (networkTypesWritten > 0) && (networkTypes[0] == typeof(DVBTNetworkProvider).GUID), "IBDA_AutoDemodulateEx.get_SupportedNetworkTypes failed");
    }

    private void Config()
    {
      int hr = 0;

      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      // graph.RunGraph();

      IBDA_Topology topo = (IBDA_Topology)graph.bdaTuner;
      int[] nodeTypes = new int[10];
      int nodeTypesCount;

      // Get all nodes in the BDA Tuner
      hr = topo.GetNodeTypes(out nodeTypesCount, nodeTypes.Length, nodeTypes);
      DsError.ThrowExceptionForHR(hr);

      // For each nodes types
      for (int i = 0; i < nodeTypesCount; i++)
      {
        Guid[] nodeGuid = new Guid[10];
        int nodeGuidCount;

        // Get its exposed interfaces
        hr = topo.GetNodeInterfaces(nodeTypes[i], out nodeGuidCount, nodeGuid.Length, nodeGuid);
        DsError.ThrowExceptionForHR(hr);

        // For each exposed interfaces
        for (int j = 0; j < nodeGuidCount; j++)
        {
          Debug.WriteLine(string.Format("node {0}/{1} : {2}", i, j, nodeGuid[j]));
          Console.WriteLine(string.Format("node {0}/{1} : {2}", i, j, nodeGuid[j]));

          // Is IBDA_AutoDemodulate supported by this node ?
          if (nodeGuid[j] == typeof(IBDA_AutoDemodulateEx).GUID)
          {
            Console.WriteLine("nodetype : " + nodeTypes[i]);
            // Yes, retrieve this node
            object ctrlNode;
            hr = topo.GetControlNode(0, 1, nodeTypes[i], out ctrlNode);
            DsError.ThrowExceptionForHR(hr);

            // Do the cast (it should not fail)
            autoDemodulateEx = ctrlNode as IBDA_AutoDemodulateEx;

            // Exit the for j loop if found a SignalStatistics object
            if (autoDemodulateEx != null)
              break;
          }

        }

        // If already found a SignalStatistics object, exit the for i loop
        if (autoDemodulateEx != null)
          break;
      }

      Debug.Assert(autoDemodulateEx != null, "Can't find a node supporting IBDA_AutoDemodulateEx");
    }

    private void Unconfig()
    {
      if (autoDemodulateEx != null) Marshal.ReleaseComObject(autoDemodulateEx);
      graph.DecomposeGraph();
    }

  }
}

#endif
