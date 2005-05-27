using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

// Remove an out to GetCurFile method for AMMediaType

namespace DirectShowLib.Test
{
	public class IFileSourceFilterTest
	{
    IGraphBuilder graphBuilder = null;
    IFileSourceFilter sourceFilter = null;

    string sampleFileName = @"..\..\..\Resources\foo.avi";

		public IFileSourceFilterTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();

      try
      {
        TestLoad();
        TestGetCurFile();
      }
      finally
      {
        while(Marshal.ReleaseComObject(this.sourceFilter) > 0);
        while(Marshal.ReleaseComObject(this.graphBuilder) > 0);
      }
    }

    public void TestLoad()
    {
      int hr = 0;

      // Try to load our sample file
      hr = this.sourceFilter.Load(sampleFileName, null);
      DsError.ThrowExceptionForHR(hr);

      Debug.WriteLine(hr == 0, "IFileSourceFilter.Load");
    }

    public void TestGetCurFile()
    {
      int hr = 0;
      string filename;
      AMMediaType mediaType = new AMMediaType();

      hr = this.sourceFilter.GetCurFile(out filename, mediaType);
      DsError.ThrowExceptionForHR(hr);

      Debug.WriteLine(filename.Equals(this.sampleFileName), "IFileSourceFilter.GetCurFile");
      Debug.WriteLine(hr == 0, "IFileSourceFilter.GetCurFile");
    }

    private void BuildGraph()
    {
      int hr = 0;
      IBaseFilter filter = null;

      // Get a GraphBuilder
      this.graphBuilder = (IGraphBuilder) new FilterGraph();

      // And add it a File Source (Async) Filter.
      filter = (IBaseFilter) new AsyncReader();
      this.graphBuilder.AddFilter(filter, "Source");
      DsError.ThrowExceptionForHR(hr);

      // Get a IFileSourceFilter from it
      this.sourceFilter = (IFileSourceFilter) filter;
    }

	}
}
