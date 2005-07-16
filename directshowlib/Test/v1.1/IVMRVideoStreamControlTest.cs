using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRVideoStreamControlTest
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr = null;
    private IVMRVideoStreamControl streamControl = null;

    private DsROTEntry rot;
    
    public IVMRVideoStreamControlTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();
        TestColorKey();
        TestStreamActiveState();
      }
      finally
      {
        rot.Dispose();
        Marshal.ReleaseComObject(streamControl);
        Marshal.ReleaseComObject(vmr);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      graphBuilder = (IFilterGraph2) new FilterGraph();
      vmr = (IBaseFilter) new VideoMixingRenderer();

      hr = graphBuilder.AddFilter(vmr, "VMR");
      DsError.ThrowExceptionForHR(hr);

      hr = (vmr as IVMRFilterConfig).SetNumberOfStreams(1);
      DsError.ThrowExceptionForHR(hr);

      IPin pinIn = DsFindPin.ByDirection(vmr, PinDirection.Input, 0);
      streamControl = (IVMRVideoStreamControl) pinIn;

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      rot = new DsROTEntry(graphBuilder);

      // Run the graph to really connect VMR pins.
      // This sample doesn't work if the graph has not been run at least one time
      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    public void TestColorKey()
    {
      int hr = 0;
      DDColorKey colorToSet = new DDColorKey();
      colorToSet.dw1 = ColorTranslator.ToWin32(Color.BlueViolet);
      colorToSet.dw2 = ColorTranslator.ToWin32(Color.RoyalBlue);

      DDColorKey colorRead;

      hr = streamControl.SetColorKey(ref colorToSet);
      DsError.ThrowExceptionForHR(hr);

      hr = streamControl.GetColorKey(out colorRead);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((colorRead.dw1 == colorToSet.dw1) && (colorRead.dw2 == colorToSet.dw2), "IVMRVideoStreamControl.GetColorKey / SetColorKey");
    }

    public void TestStreamActiveState()
    {
      int hr = 0;
      bool state = false;

      hr = streamControl.SetStreamActiveState(true);
      DsError.ThrowExceptionForHR(hr);

      hr = streamControl.GetStreamActiveState(out state);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(state == true, "IVMRVideoStreamControl.GetStreamActiveState / SetStreamActiveState");

      hr = streamControl.SetStreamActiveState(false);
      DsError.ThrowExceptionForHR(hr);

      hr = streamControl.GetStreamActiveState(out state);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(state == false, "IVMRVideoStreamControl.GetStreamActiveState / SetStreamActiveState");
    }

	}
}
