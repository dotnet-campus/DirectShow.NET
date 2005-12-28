using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

// My DVB-T board doesn't support this interface which seem to be normal for a DVB-T driver 
// (accoding to elements found on the Web). My DVB-S board report it support this interface,
// but the driver refuse to give me a COM pointer with the GetControlNode method. I don't really see
// where the problem is...
//
// This interface is very simple. I don't see how it could be wrongly declarated...

namespace DirectShowLib.Test
{
	public class IBDA_AutoDemodulateTest
	{
    private IFilterGraph2 graphBuilder;
    private DsROTEntry rot;
    private IBaseFilter bdaTuner;
    private IBDA_AutoDemodulate autoDemodulate = null;

		public IBDA_AutoDemodulateTest()
		{
		}

    public void DoTests()
    {
      Config();

      try
      {
        TestAutoDemodulate();
      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(bdaTuner);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void Config()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      // Assume that the first device in this category IS a BDA device...
      DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

      hr = graphBuilder.AddSourceFilterForMoniker(devices[0].Mon, null, devices[0].Name, out bdaTuner);
      DsError.ThrowExceptionForHR(hr);

      IBDA_Topology topo = (IBDA_Topology) bdaTuner;
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
          if (nodeGuid[j] == typeof(IBDA_AutoDemodulate).GUID)
          {
            Console.WriteLine("nodetype : " + nodeTypes[i]);
            // Yes, retrieve this node
            object ctrlNode;
            hr = topo.GetControlNode(0, 1, nodeTypes[i], out ctrlNode);
            DsError.ThrowExceptionForHR(hr);

            // Do the cast (it should not fail)
            autoDemodulate = ctrlNode as IBDA_AutoDemodulate;

            // Exit the for j loop if found a SignalStatistics object
            if (autoDemodulate != null)
              break;
          }
          
        }

        // If already found a SignalStatistics object, exit the for i loop
        if (autoDemodulate != null)
          break;
      }
    }

    private void TestAutoDemodulate()
    {
      int hr = 0;

      hr = autoDemodulate.put_AutoDemodulate();
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBDA_AutoDemodulate.put_AutoDemodulate");
    }


	}
}
