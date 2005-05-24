using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Description résumée de IVideoFrameStepTest.
	/// </summary>
	public class IVideoFrameStepTest
	{
    IGraphBuilder graphBuilder = null;
    IVideoFrameStep videoFrameStep = null;
    IBaseFilter vmr9 = null;
    
    public IVideoFrameStepTest()
		{
		}

    public void DoTests()
    {
      BuildGraph();

      this.videoFrameStep = (IVideoFrameStep) this.graphBuilder;

      try
      {
        TestCanStep();
        TestStep();
        TestCancelStep();
      }
      finally
      {
        Marshal.ReleaseComObject(this.vmr9);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    public void TestCanStep()
    {
      int hr = 0;

      // Try to see if graph can step one frame at time
      // It should work with foo.avi...
      hr = this.videoFrameStep.CanStep(0, null);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IVideoFrameStep.CanStep");

      // Try to see if graph can step every ten frames with VMR9 as step controler
      // It should work with foo.avi...
      hr = this.videoFrameStep.CanStep(10, this.vmr9);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IVideoFrameStep.CanStep");
    }

    public void TestStep()
    {
      int hr = 0;

      // Try to step one frame at time
      // This method start the graph
      hr = this.videoFrameStep.Step(1, null);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IVideoFrameStep.Step");

      // Try to step ten frame using VMR9 as step controler
      hr = this.videoFrameStep.Step(10, this.vmr9);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IVideoFrameStep.Step");
    }
  
    public void TestCancelStep()
    {
      int hr = 0;

      // Try to step 29*2 frames later (2 sec later)
      // This method is asynchronous and return dirctly
      hr = this.videoFrameStep.Step(29*2, null);
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IVideoFrameStep.CancelStep");

      // Try to cancel the step launch below
      hr = this.videoFrameStep.CancelStep();
      DsError.ThrowExceptionForHR(hr);
      Debug.Assert(hr == 0, "IVideoFrameStep.CancelStep");
    }

    private void BuildGraph()
    {
      int hr = 0;

      this.graphBuilder = (IGraphBuilder) new FilterGraph();
      this.vmr9 = (IBaseFilter) new VideoMixingRenderer9();
      this.graphBuilder.AddFilter(this.vmr9, "VMR9");

      hr = this.graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);
    }

	}
}
