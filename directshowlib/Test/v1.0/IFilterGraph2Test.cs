using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IFilterGraph2Test
	{
    private IFilterGraph2 filterGraph2 = null;
    private IBaseFilter sourceFilter = null;
    private IBaseFilter audioRenderer = null;

		public IFilterGraph2Test()
		{
		}

    public void DoTests()
    {
      this.filterGraph2 = (IFilterGraph2) new FilterGraph();
      DsROTEntry rot = new DsROTEntry(this.filterGraph2);

      try
      {
        ConfigGraph(this.filterGraph2);

        TestAddSourceFilterForMoniker();
        TestRenderEx();
        TestReconnectEx();
      }
      finally
      {
        rot.Dispose();

        Marshal.ReleaseComObject(this.audioRenderer);
        Marshal.ReleaseComObject(this.sourceFilter);
        Marshal.ReleaseComObject(this.filterGraph2);
      }
    }

    public void TestAddSourceFilterForMoniker()
    {
      int hr = 0;
      ICreateDevEnum devEnum = (ICreateDevEnum) new CreateDevEnum();
      UCOMIEnumMoniker enumMoniker = null;
      Guid filterCat = FilterCategory.AudioRendererCategory;
      UCOMIMoniker[] monikers = new UCOMIMoniker[1];
      int fetched = 0;

      // In this method, i try to add an Audio Renderer by browsing the 
      // AudioRenderer Filters Category...
      hr = devEnum.CreateClassEnumerator(ref filterCat, out enumMoniker, 0);
      Marshal.ThrowExceptionForHR(hr);
      Marshal.ReleaseComObject(devEnum);

      hr = enumMoniker.Next(1, monikers, out fetched);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.filterGraph2.AddSourceFilterForMoniker(monikers[0], null, "Audio Renderer from Moniker", out this.audioRenderer);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IFilterGraph2.AddSourceFilterForMoniker");

      Marshal.ReleaseComObject(enumMoniker);
      Marshal.ReleaseComObject(monikers[0]);
    }

    public void TestRenderEx()
    {
      int hr = 0;
      IPin pinOut = null;

      pinOut = DsFindPin.ByDirection(this.sourceFilter, PinDirection.Output, 0);

      // Render the output pin of the source filter to build the graph.
      // This method use the Audio Renderer we have added in TestAddSourceFilterForMoniker()
      hr = this.filterGraph2.RenderEx(pinOut, AMRenderExFlags.Zero, IntPtr.Zero);
      Marshal.ThrowExceptionForHR(hr);

      // hr == 0 if the graph is fully builded
      // hr == 262743	if the graph is partialy build (with AMRenderExFlags.RenderToExistingRenderers)
      Debug.Assert(hr >= 0, "IFilterGraph2.RenderEx");

      // Use GraphEdit to check the result...

      Marshal.ReleaseComObject(pinOut);
    }

    public void TestReconnectEx()
    {
      int hr = 0;
      IPin pinOut = null;

      pinOut = DsFindPin.ByDirection(this.sourceFilter, PinDirection.Output, 0);

      // Just trying to reconnect link between source filter and avi splitter
      hr = this.filterGraph2.ReconnectEx(pinOut, null);
      Marshal.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IFilterGraph2.ReconnectEx");

      // Use GraphEdit to check the result...
      // Avi Splitter should be deconnected from the two codecs

      Marshal.ReleaseComObject(pinOut);
    }

    private void ConfigGraph(IFilterGraph2 fg)
    {
      int hr = 0;

      hr = fg.AddSourceFilter("foo.avi", "Source Filter", out this.sourceFilter);
      Marshal.ThrowExceptionForHR(hr);

    }

  }
}
