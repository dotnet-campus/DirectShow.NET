using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRVideoStreamControl9Test
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9 = null;
    private IVMRVideoStreamControl9 streamControl = null;

    private DsROTEntry rot;
    
    public IVMRVideoStreamControl9Test()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();
        TestStreamActiveState();
      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(streamControl);
        Marshal.ReleaseComObject(vmr9);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      vmr9 = (IBaseFilter) new VideoMixingRenderer9();

      hr = graphBuilder.AddFilter(vmr9, "VMR9");
      DsError.ThrowExceptionForHR(hr);

      hr = (vmr9 as IVMRFilterConfig9).SetNumberOfStreams(1);
      DsError.ThrowExceptionForHR(hr);

      IPin pinIn = DsFindPin.ByDirection(vmr9, PinDirection.Input, 0);
      streamControl = (IVMRVideoStreamControl9) pinIn;

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      rot = new DsROTEntry(graphBuilder);

      // Run the graph to really connect VMR9 pins.
      // This sample doesn't work if the graph has not been run at least one time
      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    public void TestStreamActiveState()
    {
      int hr = 0;
      bool state = false;

      hr = streamControl.SetStreamActiveState(true);
      DsError.ThrowExceptionForHR(hr);

      hr = streamControl.GetStreamActiveState(out state);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(state == true, "IVMRVideoStreamControl9.GetStreamActiveState / SetStreamActiveState");

      hr = streamControl.SetStreamActiveState(false);
      DsError.ThrowExceptionForHR(hr);

      hr = streamControl.GetStreamActiveState(out state);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(state == false, "IVMRVideoStreamControl9.GetStreamActiveState / SetStreamActiveState");
    }

	}
}
