using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IEnumFiltersTest
	{
    IGraphBuilder graphBuilder = null;
    IEnumFilters enumFilters = null;
    
    public IEnumFiltersTest()
		{
		}

    public void DoTests()
    {
      BuildGraph(@"..\..\..\Resources\foo.avi");
      
      try
      {
        TestClone();
        TestNext();
        TestReset();
        TestSkip();
      }
      finally
      {
        while(Marshal.ReleaseComObject(this.enumFilters) > 0);
        while(Marshal.ReleaseComObject(this.graphBuilder) > 0);
      }
    }

    public void TestClone()
    {
      int hr = 0;
      IEnumFilters clone = null;
      IBaseFilter[] filters = new IBaseFilter[1];
      int filterCount1 = 0;
      int filterCount2 = 0;

      // Reset enumeration to be sure we are at the begining
      hr = this.enumFilters.Reset();
      DsError.ThrowExceptionForHR(hr);
      
      // Clone our enumeration object
      hr = this.enumFilters.Clone(out clone);
      DsError.ThrowExceptionForHR(hr);

      // Count how many filters are in this enumearation
      while (this.enumFilters.Next(1, filters, IntPtr.Zero) == 0)
      {
        Marshal.ReleaseComObject(filters[0]);
        filterCount1++;
      }

      // Count how many filters are in this enumearation
      while (clone.Next(1, filters, IntPtr.Zero) == 0)
      {
        Marshal.ReleaseComObject(filters[0]);
        filterCount2++;
      }

      // both enumeration should have equal items number
      // 6 filters for me
      Debug.Assert(filterCount1 == filterCount2, "IEnumFilters.Clone");
    }

    public void TestNext()
    {
      int hr = 0;
      IntPtr ip = Marshal.AllocCoTaskMem(4);
      IBaseFilter[] filters = new IBaseFilter[1];
      FilterInfo fi = new FilterInfo();

      // Reset enumeration to be sure we are at the begining
      hr = this.enumFilters.Reset();
      DsError.ThrowExceptionForHR(hr);

      // Get the first filter
      hr = this.enumFilters.Next(1, filters, ip);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(Marshal.ReadInt32(ip) == 1, "Next");

      hr = filters[0].QueryFilterInfo(out fi);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(fi.achName.Equals("Default DirectSound Device"), "IEnumFilters.Next");
      Marshal.ReleaseComObject(fi.pGraph);

      // Get the second filter
      hr = this.enumFilters.Next(1, filters, IntPtr.Zero);
      DsError.ThrowExceptionForHR(hr);

      hr = filters[0].QueryFilterInfo(out fi);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(fi.achName.Equals("Video Renderer"), "IEnumFilters.Next");
      Marshal.ReleaseComObject(fi.pGraph);
      Marshal.FreeCoTaskMem(ip);
    }

    public void TestReset()
    {
      int hr = 0;

      hr = this.enumFilters.Reset();
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IEnumFilters.Reset");
    }

    public void TestSkip()
    {
      int hr = 0;
      IBaseFilter[] filters = new IBaseFilter[1];
      int filterCount1 = 0;
      int filterCount2 = 0;

      // Count how many filters are in this enumearation
      while (this.enumFilters.Next(1, filters, IntPtr.Zero) == 0)
      {
        Marshal.ReleaseComObject(filters[0]);
        filterCount1++;
      }

      hr = this.enumFilters.Reset();
      DsError.ThrowExceptionForHR(hr);

      // Count how many filters are in this enumearation but skip on each time
      while (this.enumFilters.Next(1, filters, IntPtr.Zero) == 0)
      {
        // skip one filter
        hr = this.enumFilters.Skip(1);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(filters[0]);
        filterCount2++;
      }

      // second count shhould be half the first
      Debug.Assert(filterCount1 == filterCount2 * 2, "IEnumFilters.Skip");
    }

    private void BuildGraph(string fileName)
    {
      int hr = 0;
      this.graphBuilder = (IGraphBuilder) new FilterGraph();

      hr = this.graphBuilder.RenderFile(fileName, null);
      DsError.ThrowExceptionForHR(hr);

      hr = this.graphBuilder.EnumFilters(out this.enumFilters);
      DsError.ThrowExceptionForHR(hr);
    }
	}
}
