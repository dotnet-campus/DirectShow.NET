using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IBDA_TopologyTest
	{
    IFilterGraph2 graphBuilder;
    DsROTEntry rot;
    IBaseFilter bdaTuner;
    IBDA_Topology topology;

    int[] pinTypes;
    int[] pinIds;
    int[] nodeTypes;

    public IBDA_TopologyTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        topology = (IBDA_Topology) bdaTuner;

        TestGetPinTypes();
        TestGetNodeTypes();
        TestCreatePin();
        TestCreateTopology();
        TestGetControlNode();
        TestGetNodeInterfaces();
        TestGetTemplateConnections();
        TestGetNodeDescriptors();
        TestSetMediaType();
        TestSetMedium();

        TestDeletePin();

      }
      finally
      {
        Marshal.ReleaseComObject(bdaTuner);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      rot = new DsROTEntry(graphBuilder);

      // Assume that the first device in this category IS a BDA device...
      DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.BDASourceFiltersCategory);

      hr = graphBuilder.AddSourceFilterForMoniker(devices[0].Mon, null, devices[0].Name, out bdaTuner);
      DsError.ThrowExceptionForHR(hr);

    }

    private void TestGetPinTypes()
    {
      int hr = 0;
      int pinTypesCount;

      hr = topology.GetPinTypes(out pinTypesCount, 0, null);

      pinTypes = new int[pinTypesCount];

      hr = topology.GetPinTypes(out pinTypesCount, pinTypes.Length, pinTypes);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (pinTypesCount > 0), "IBDA_Topology.GetPinTypes");
    }

    private void TestGetNodeTypes()
    {
      int hr = 0;
      int nodeTypesCount;

      hr = topology.GetNodeTypes(out nodeTypesCount, 0, null);

      nodeTypes = new int[nodeTypesCount];

      hr = topology.GetNodeTypes(out nodeTypesCount, nodeTypes.Length, nodeTypes);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (nodeTypesCount > 0), "IBDA_Topology.GetNodeTypes");
    }

    private void TestCreatePin()
    {
      int hr = 0;

      pinIds = new int[pinTypes.Length];

      for(int i = 0; i < pinTypes.Length; i++)
      {
        hr = topology.CreatePin(pinTypes[i], out pinIds[i]);
        Debug.Assert((hr == 0), "IBDA_Topology.CreatePin");
      }
    }

    private void TestCreateTopology()
    {
      int hr = 0;

      hr = topology.CreateTopology(pinIds[0], pinIds[1]);
      Debug.Assert((hr == 0), "IBDA_Topology.CreateTopology");
    }

    private void TestGetControlNode()
    {
      int hr = 0;
      object controlNode;

      // This is one of the two really useful method of this interface
      hr = topology.GetControlNode(pinIds[0], pinIds[1], pinTypes[0], out controlNode);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (controlNode != null), "IBDA_Topology.GetControlNode");

      Marshal.ReleaseComObject(controlNode);
    }

    private void TestGetNodeInterfaces()
    {
      int hr = 0;
      int interfacesCount;
      Guid[] interfaces;

      // This is the second really useful method of this interface
      hr = topology.GetNodeInterfaces(pinTypes[1], out interfacesCount, 0, null);

      interfaces = new Guid[interfacesCount];

      hr = topology.GetNodeInterfaces(pinTypes[1], out interfacesCount, interfaces.Length, interfaces);

      Debug.Assert((hr == 0) && (interfacesCount > 0), "IBDA_Topology.GetNodeInterfaces");
    }

    private void TestGetTemplateConnections()
    {
      int hr = 0;
      int connectionsCount;
      BDATemplateConnection[] connections;

      // This method don't like to be called with a null param.
      // In this case, it didn't return the connection count...
      hr = topology.GetTemplateConnections(out connectionsCount, 0, null);

      // So i fix it to a size resonably high.
      connections = new BDATemplateConnection[10];
      hr = topology.GetTemplateConnections(out connectionsCount, connections.Length, connections);

      Debug.Assert((hr == 0), "IBDA_Topology.GetTemplateConnections");
    }

    private void TestGetNodeDescriptors()
    {
      int hr = 0;
      int nodeDescCount;
      BDANodeDescriptor[] nodeDesc;

      // This method is also very useful
      hr = topology.GetNodeDescriptors(out nodeDescCount, 0, null);

      nodeDesc = new BDANodeDescriptor[nodeDescCount];
      hr = topology.GetNodeDescriptors(out nodeDescCount, nodeDesc.Length, nodeDesc);

      Debug.Assert((hr == 0) && (nodeDescCount > 0), "IBDA_Topology.GetNodeDescriptors");
    }

    private void TestSetMediaType()
    {
      int hr = 0;
      AMMediaType[] mediaTypes;
      Guid KSDATAFORMAT_TYPE_BDA_ANTENNA = new Guid("71985F41-1CA1-11d3-9CC8-00C04F7971E0");

      mediaTypes = new AMMediaType[1];
      mediaTypes[0] = new AMMediaType();
      mediaTypes[0].formatType = KSDATAFORMAT_TYPE_BDA_ANTENNA;

      hr = topology.SetMediaType(pinIds[0], mediaTypes);

      Debug.Assert((hr == 0), "IBDA_Topology.SetMediaType");
    }

    private void TestSetMedium()
    {
      int hr = 0;
      RegPinMedium pinMedium = new RegPinMedium();;

      // Really don't know how to test this method...
      hr = topology.SetMedium(pinIds[0], pinMedium);

      Debug.Assert((hr == 0), "IBDA_Topology.SetMedium");
    }



    private void TestDeletePin()
    {
      int hr = 0;

      for(int i = 0; i < pinIds.Length; i++)
      {
        hr = topology.DeletePin(pinIds[i]);
        Debug.Assert((hr == 0), "IBDA_Topology.DeletePin");
      }
    }


	}
}
