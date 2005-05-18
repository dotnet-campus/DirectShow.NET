using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IGraphBuilderTest
	{
    private IGraphBuilder graphBuilder = null;
    private IBaseFilter sourceFilter = null;
    private IBaseFilter aviSplitter = null;
    private FileStream logFile = null;

    private const string logFileName = "IGraphBuilderTest.log";

		public IGraphBuilderTest()
		{
		}

    public void DoTests()
    {
      this.graphBuilder = (IGraphBuilder) new FilterGraph();
      this.logFile = new FileStream(logFileName, FileMode.Create, FileAccess.Write);

      try
      {
        TestSetLogFile();

        ConfigGraph(this.graphBuilder);

        TestAddSourceFilter();
        TestConnect();
        TestRender();

        CleanUpGraph(this.graphBuilder);
        TestRenderFile();

        TestAbort();
        TestShouldOperationContinue();

      }
      finally
      {
        Marshal.ReleaseComObject(this.aviSplitter);
        Marshal.ReleaseComObject(this.sourceFilter);
        Marshal.ReleaseComObject(this.graphBuilder);

        if (this.logFile != null)
        {
          this.logFile.Flush();
          this.logFile.Close();
          ShowLogFile();
        }
      }
    }

    public void TestSetLogFile()
    {
      int hr = 0;

      // We should plan extended tests with this method to see 
      // if we could mix .Net text writing with GraphBuilder writings
      hr = this.graphBuilder.SetLogFile(this.logFile.Handle);
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IGraphBuilder.SetLogFile");
    }

    public void TestAddSourceFilter()
    {
      int hr = 0;

      // try to add a source filter
      hr = this.graphBuilder.AddSourceFilter("foo.avi", "Source filter", out this.sourceFilter);
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IGraphBuilder.AddSourceFilter");
    }

    public void TestConnect()
    {
      int hr = 0;
      IPin pinOut = null;
      IPin pinIn = null;

      // try to connect source filter with avi splitter
      pinOut = DsFindPin.ByDirection(this.sourceFilter, PinDirection.Output, 0);
      pinIn = DsFindPin.ByDirection(this.aviSplitter, PinDirection.Input, 0);

      hr = this.graphBuilder.Connect(pinOut, pinIn);
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IGraphBuilder.Connect");

      Marshal.ReleaseComObject(pinIn);
      Marshal.ReleaseComObject(pinOut);
    }

    public void TestRender()
    {
      int hr = 0;
      IPin pinOut = null;

      // try to render first pin of the avi splitter (video)
      pinOut = DsFindPin.ByDirection(this.aviSplitter, PinDirection.Output, 0);

      hr = this.graphBuilder.Render(pinOut);
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IGraphBuilder.Render");
    }

    public void TestRenderFile()
    {
      int hr = 0;

      // The same as above but in one method
      hr = this.graphBuilder.RenderFile("foo.avi", null);
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IGraphBuilder.RenderFile");
    }

    public void TestAbort()
    {
      int hr = 0;

      // can do nothing except test the return value
      hr = this.graphBuilder.Abort();
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IGraphBuilder.Abort");
    }

    public void TestShouldOperationContinue()
    {
      int hr = 0;

      // can do nothing except test the return value
      hr = this.graphBuilder.ShouldOperationContinue();
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0 || hr == 1, "IGraphBuilder.ShouldOperationContinue");
    }

    private void ConfigGraph(IGraphBuilder graphBuilder)
    {
      int hr = 0;

      this.aviSplitter = (IBaseFilter) new AviSplitter();

      hr = graphBuilder.AddFilter(this.aviSplitter, "AVI Splitter");
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IFilterGraph.AddFilter");
    }

    private void CleanUpGraph(IGraphBuilder graphBuilder)
    {
      int hr = 0;
      IEnumFilters enumFilters = null;
      IBaseFilter filter = null;
      int fetched = 0;

      hr = graphBuilder.EnumFilters(out enumFilters);
      Marshal.ThrowExceptionForHR(hr);

      while(enumFilters.Next(1, out filter, out fetched) == 0)
      {
        hr = graphBuilder.RemoveFilter(filter);
        Marshal.ThrowExceptionForHR(hr);
        Marshal.ReleaseComObject(filter);
        enumFilters.Reset();
      }

      Marshal.ReleaseComObject(enumFilters);
    }

    private void ShowLogFile()
    {
      // show the logfile in a notepad
      Process.Start(logFileName);
    }
  }
}
