using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IBaseFilterTest
	{
    IBaseFilter filter = null;
    IGraphBuilder graphBuilder = null;

    public IBaseFilterTest()
		{
		}

    public void DoTests()
    {
      this.filter = (IBaseFilter) new VideoMixingRenderer9();
      this.graphBuilder = (IGraphBuilder) new FilterGraph();

      try
      {
        TestEnumPins();
        TestFindPin();
        TestJoinFilterGraph();
        TestQueryFilterInfo();
        TestQueryVendorInfo();
      }
      finally
      {
        while(Marshal.ReleaseComObject(this.filter) > 0);
      }
    }

    public void TestEnumPins()
    {
      int hr = 0;
      IEnumPins enumPins = null;

      hr = this.filter.EnumPins(out enumPins);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(enumPins != null, "IBaseFilter.EnumPins");

      Marshal.ReleaseComObject(enumPins);
    }

    public void TestFindPin()
    {
      int hr = 0;
      IPin pin = null;

      hr = this.filter.FindPin("VMR Input0", out pin);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(pin != null, "IBaseFilter.FindPin");

      Marshal.ReleaseComObject(pin);
    }

    public void TestJoinFilterGraph()
    {
      int hr = 0;

      hr = this.filter.JoinFilterGraph(this.graphBuilder, "VMR9");
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IBaseFilter.JoinFilterGraph");
    }

    public void TestQueryFilterInfo()
    {
      int hr = 0;
      FilterInfo filterInfo;

      hr = this.filter.QueryFilterInfo(out filterInfo);
      Marshal.ThrowExceptionForHR(hr);

      // QueryFilterInfo do an AddRef before returning. We have to release it.
      Marshal.ReleaseComObject(filterInfo.pGraph);

      // This value have been assigned in the TestJoinFilterGraph() method
      Debug.Assert(filterInfo.achName.Equals("VMR9"), "IBaseFilter.QueryVendorInfo");
    }

    public void TestQueryVendorInfo()
    {
      int hr = 0;
      string vendorInfo;

      hr = this.filter.QueryVendorInfo(out vendorInfo);
      //Marshal.ThrowExceptionForHR(hr);

      // Microsoft's CBaseFilter::QueryVendorInfo always return E_NOTIMPL
      // I assume we never had another value...
      Debug.Assert(hr == -2147467263, "IBaseFilter.QueryVendorInfo");
    }
	}
}
