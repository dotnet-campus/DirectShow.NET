using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Test the IVMRWindowlessControl9 interface
	/// </summary>
	public class IVMRWindowlessControl9Test : Form
	{
    private const string testFile = @"foo.avi";
    
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter sourceFilter = null;
    private IBaseFilter vmr9Filter = null;
    private IVMRWindowlessControl9 vmrWndConfig = null;
    private IMediaControl mediaControl = null;

    private bool isPlaying = false;
    
    public IVMRWindowlessControl9Test()
		{
      this.Text = "IVMRWindowlessControl9Test window";
      this.ClientSize = new Size(800, 450); // 16/9 aspect ratio to see colored borders
      this.Paint += new PaintEventHandler(IVMRWindowlessControl9Test_Paint);
      this.Resize += new EventHandler(IVMRWindowlessControl9Test_Resize);
		}

    public void DoTests()
    {
      int hr = 0;
      IVMRFilterConfig9 vmrConfig = null;

      try
      {
        // Just build the graph with unconnected source filter and VMR9
        BuildGraph(testFile, out this.graphBuilder, out this.sourceFilter, out this.vmr9Filter);

        // VMR9 need to be placed in Windowless mode before connecting it to the rest of the graph
        vmrConfig = this.vmr9Filter as IVMRFilterConfig9;
        hr = vmrConfig.SetRenderingMode(VMR9Mode.Windowless);
        Marshal.ThrowExceptionForHR(hr);

        this.vmrWndConfig = this.vmr9Filter as IVMRWindowlessControl9;

        TestSetVideoClippingWindow();
        TestAspectRatioMode();
        TestBorderColor();
        TestGetMaxIdealVideoSize();
        TestGetMinIdealVideoSize();

        // Connect source filter with the VMR9
        ConnectGraph(ref this.graphBuilder, ref this.sourceFilter, ref this.vmr9Filter);

        // Run the graph to test other methods
        this.mediaControl = this.graphBuilder as IMediaControl;
        hr = this.mediaControl.Run();
        Marshal.ThrowExceptionForHR(hr);

        this.isPlaying = true;
        TestVideoPosition();

        Thread.Sleep(2000);
        TestGetCurrentImage();

        Thread.Sleep(2000);
        TestDisplayModeChanged();
        TestGetNativeVideoSize();
        TestRepaintVideo();
      }
      finally
      {
        if (this.mediaControl != null)
          this.mediaControl.Stop();
        Marshal.ReleaseComObject(this.vmr9Filter);
        Marshal.ReleaseComObject(this.sourceFilter);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    void TestDisplayModeChanged()
    {
      int hr = 0;

      hr = this.vmrWndConfig.DisplayModeChanged();
      Marshal.ThrowExceptionForHR(hr);

      // Can do nothing except calling this method
      Debug.Assert(hr == 0, "DisplayModeChanged");
    }

    void TestAspectRatioMode()
    {
      int hr = 0;
      VMR9AspectRatioMode mode;

      // default is VMR9AspectRatioMode.None
      hr = this.vmrWndConfig.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrWndConfig.GetAspectRatioMode(out mode);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(mode == VMR9AspectRatioMode.LetterBox, "Set / Get AspectRatioMode");
    }

    void TestBorderColor()
    {
      int hr = 0;
      int color = 0;

      // color format is 0x00bbggrr
      hr = this.vmrWndConfig.SetBorderColor(0x00cc9966);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrWndConfig.GetBorderColor(out color);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(color == 0x00cc9966, "Set / Get BorderColor");
    }

    void TestGetCurrentImage()
    {
      int hr = 0;
      IntPtr dib = IntPtr.Zero;

      hr = this.vmrWndConfig.GetCurrentImage(out dib);
      Marshal.ThrowExceptionForHR(hr);

      // too lazzy to save the bmp

      Marshal.FreeCoTaskMem(dib);
      Debug.Assert(dib != IntPtr.Zero, "GetCurrentImage");
    }

    void TestGetMaxIdealVideoSize()
    {
      int hr = 0;
      int width, height;

      hr = this.vmrWndConfig.GetMaxIdealVideoSize(out width, out height);
      Marshal.ThrowExceptionForHR(hr);

      // for me width and height are my screen resolution plus 1
      // Can do nothing except calling this method
      Debug.Assert(hr == 0, "GetMaxIdealVideoSize");
    }

    void TestGetMinIdealVideoSize()
    {
      int hr = 0;
      int width, height;

      hr = this.vmrWndConfig.GetMinIdealVideoSize(out width, out height);
      Marshal.ThrowExceptionForHR(hr);

      // for me width == height == 1
      Debug.Assert(hr == 0, "GetMinIdealVideoSize");
    }

    void TestGetNativeVideoSize()
    {
      int hr = 0;
      int width, height;
      int widthAR, heightAR;

      hr = this.vmrWndConfig.GetNativeVideoSize(out width, out height, out widthAR, out heightAR);
      Marshal.ThrowExceptionForHR(hr);

      // According to the documentation, widthAR should be 4 and heightAR should be 3
      // but i have widthAR = 320 & heightAR = 240 with foo.avi
      // I don't know where the error come from...

      // foo.avi is a 320x240 video
      Debug.Assert((width == 320) && (height == 240), "GetNativeVideoSize");
    }

    void TestVideoPosition()
    {
      this.IVMRWindowlessControl9Test_Resize(this, null);
    }

    void TestRepaintVideo()
    {
      // see IVMRWindowlessControl9Test_Paint...
    }

    void TestSetVideoClippingWindow()
    {
      int hr = 0;

      // if this method don't work we don't see the video...
      hr = this.vmrWndConfig.SetVideoClippingWindow(this.Handle);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "SetVideoClippingWindow");
    }

    private void BuildGraph(string sFileName, out IFilterGraph2 graphBuilder, out IBaseFilter sourceFilter, out IBaseFilter vmr9Filter)
    {
      int hr = 0;

      graphBuilder = new FilterGraph() as IFilterGraph2;
      vmr9Filter = null;

      try
      {
        hr = graphBuilder.AddSourceFilter(sFileName, sFileName, out sourceFilter);
        Marshal.ThrowExceptionForHR(hr);


        vmr9Filter = (IBaseFilter) new VideoMixingRenderer9();
        hr = graphBuilder.AddFilter(vmr9Filter, "VMR9");
        Marshal.ThrowExceptionForHR(hr);
      }
      catch
      {
        Marshal.ReleaseComObject(graphBuilder);
        throw;
      }
      finally
      {
      }
    }

    private void ConnectGraph(ref IFilterGraph2 graphBuilder, ref IBaseFilter sourceFilter, ref IBaseFilter vmr9Filter)
    {
      int hr = 0;
      IPin pinOut = null;

      try
      {
        pinOut = DsGetPin.ByDirection(sourceFilter, PinDirection.Output);

        hr = graphBuilder.RenderEx(pinOut, AMRenderExFlags.RenderToExistingRenderers, IntPtr.Zero);
        Marshal.ThrowExceptionForHR(hr);
      }
      finally
      {
        Marshal.ReleaseComObject(pinOut);
      }
    }

    private void IVMRWindowlessControl9Test_Paint(object sender, PaintEventArgs e)
    {
      int hr = 0;

      if (isPlaying)
      {
        if (this.vmrWndConfig != null)
        {
          IntPtr hDC = e.Graphics.GetHdc();
          hr = this.vmrWndConfig.RepaintVideo(this.Handle, hDC);
          e.Graphics.ReleaseHdc(hDC);
          Debug.Assert(hr != 0, "RepaintVideo");
        }
      }
    }

    private void IVMRWindowlessControl9Test_Resize(object sender, EventArgs e)
    {
      int hr = 0;
      Rectangle src, dst;

      if (isPlaying)
      {
        if (this.vmrWndConfig != null)
        {
          hr = this.vmrWndConfig.GetVideoPosition(out src, out dst);
          Debug.Assert(hr == 0, "GetVideoPosition");

          dst.Width = this.ClientSize.Width;
          dst.Height = this.ClientSize.Height;

          hr = this.vmrWndConfig.SetVideoPosition(ref src, ref dst);
          Debug.Assert(hr == 0, "SetVideoPosition");
        }
      }
    }


  }
}
