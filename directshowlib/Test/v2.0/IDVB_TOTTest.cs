using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
  class IDVB_TOTTest
  {
    private BdaGraph graph;
#if ALLOW_UNTESTED_INTERFACES
    private IDvbSiParser parser;
#endif
      private IDVB_TOT tot;

    public void DoTests()
    {
      try
      {
        Config();

        TestInitialize();

        TestBatch1();
        TestBatch2();
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

    // Methods tested : GetUTCTime
    private void TestBatch1()
    {
      int hr = 0;

      MpegDateAndTime dateAndTime = new MpegDateAndTime();
      hr = tot.GetUTCTime(out dateAndTime);
      Debug.Assert((hr == 0) && (dateAndTime.ToDateTime() > DateTime.MinValue), "IDVB_TOT.GetUTCTime failed");

      Debug.WriteLine("Current UTC time is " + dateAndTime.ToDateTime().ToString());
    }

    // Methods tested : GetCountOfTableDescriptors, GetTableDescriptorByIndex
    //                  GetTableDescriptorByTag
    private void TestBatch2()
    {
      int hr = 0;

      int descriptorCount = 0;
      hr = tot.GetCountOfTableDescriptors(out descriptorCount);
      Debug.Assert((hr == 0) && (descriptorCount > 0), "IDVB_TOT.GetCountOfTableDescriptors failed");

      IGenericDescriptor descriptor = null;
      byte tag = 0;

      for (int i = 0; i < descriptorCount; i++)
      {
        hr = tot.GetTableDescriptorByIndex(i, out descriptor);
        Debug.Assert((hr == 0) && (descriptor != null), "IDVB_TOT.GetTableDescriptorByIndex failed");

        hr = descriptor.GetTag(out tag);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(descriptor);
      }

      hr = tot.GetTableDescriptorByTag(tag, null, out descriptor);
      Debug.Assert((hr == 0) && (descriptor != null), "IDVB_TOT.GetTableDescriptorByTag failed");

      Marshal.ReleaseComObject(descriptor);
    }

    private void Config()
    {
#if ALLOW_UNTESTED_INTERFACES

      int hr = 0;

      parser = (IDvbSiParser)new DvbSiParser();
      graph = new BdaGraph();
      graph.InitializeGraph();
      graph.MakeTuneRequest();
      graph.RunGraph();

      hr = parser.Initialize(graph.bdaSecTab as IMpeg2Data);
      Debug.Assert(hr == 0, "Initialize failed !!!");

      hr = parser.GetTOT(out tot);
      Debug.Assert(tot != null, "Can't get a TOT object");
#endif
    }

    private void Unconfig()
    {
      Marshal.ReleaseComObject(tot);
#if ALLOW_UNTESTED_INTERFACES
      Marshal.ReleaseComObject(parser);
#endif
      graph.DecomposeGraph();
    }
  }
}
