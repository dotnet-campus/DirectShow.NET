using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DirectShowLib.Test
{
  public class IVMRWindowlessControlTest : Form
	{
    private const string testFile = @"foo.avi";
    
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter sourceFilter = null;
    private IBaseFilter vmr7Filter = null;
    private IVMRWindowlessControl vmrWndConfig = null;
    private IMediaControl mediaControl = null;

    private bool isPlaying = false;

    private System.Windows.Forms.Timer timer; 
  
    public IVMRWindowlessControlTest()
		{
      this.Text = "IVMRWindowlessControlTest window";
      this.ClientSize = new Size(800, 450); // 16/9 aspect ratio to see colored borders
      this.Paint += new PaintEventHandler(IVMRWindowlessControlTest_Paint);
      this.Resize += new System.EventHandler(IVMRWindowlessControlTest_Resize);

      this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
    }

    public void DoTests()
    {
      int hr = 0;
      IVMRFilterConfig vmrConfig = null;
      DsROTEntry ROT = null;

      this.Show();

      try
      {
        // Just build the graph with unconnected source filter and VMR7
        BuildGraph(testFile, out this.graphBuilder, out this.sourceFilter, out this.vmr7Filter);

        // VMR7 need to be placed in Windowless mode before connecting it to the rest of the graph
        vmrConfig = this.vmr7Filter as IVMRFilterConfig;
        hr = vmrConfig.SetRenderingMode(VMRMode.Windowless);
        Marshal.ThrowExceptionForHR(hr);

        this.vmrWndConfig = this.vmr7Filter as IVMRWindowlessControl;

        TestSetVideoClippingWindow();
        TestAspectRatioMode();
        TestBorderColor();
        TestColorKey();
        TestGetMaxIdealVideoSize();
        TestGetMinIdealVideoSize();

        // Connect source filter with the VMR7
        ConnectGraph(ref this.graphBuilder, ref this.sourceFilter, ref this.vmr7Filter);
        ROT = new DsROTEntry(this.graphBuilder);

        // Run the graph to test other methods
        this.mediaControl = this.graphBuilder as IMediaControl;
        hr = this.mediaControl.Run();
        Marshal.ThrowExceptionForHR(hr);

        this.timer = new System.Windows.Forms.Timer();
        this.timer.Interval = 2000;
        this.timer.Enabled = true;
        this.timer.Tick += new EventHandler(DoTestsPart2);
        this.timer.Start();

        this.isPlaying = true;
        TestVideoPosition();

        Application.Run(this);

      }
      finally
      {
          if (ROT != null)
          {
              ROT.Dispose();
          }
        if (this.mediaControl != null)
          this.mediaControl.Stop();
        Marshal.ReleaseComObject(this.vmr7Filter);
        Marshal.ReleaseComObject(this.sourceFilter);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    private void DoTestsPart2(object sender, EventArgs e)
    {
      // do this only one time
      this.timer.Enabled = false;

      TestDisplayModeChanged();
      TestGetCurrentImage();
      TestGetNativeVideoSize();

      // wait and close the window...
      Thread.Sleep(2000);
      this.Close();
    }


    void TestSetVideoClippingWindow()
    {
      int hr = 0;

      // if this method don't work we don't see the video...
      hr = this.vmrWndConfig.SetVideoClippingWindow(this.Handle);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "SetVideoClippingWindow");
    }

    void TestAspectRatioMode()
    {
      int hr = 0;
      VMRAspectRatioMode mode;

      // default is VMRAspectRatioMode.None
      hr = this.vmrWndConfig.SetAspectRatioMode(VMRAspectRatioMode.LetterBox);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrWndConfig.GetAspectRatioMode(out mode);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(mode == VMRAspectRatioMode.LetterBox, "Set / Get AspectRatioMode");
    }

    void TestBorderColor()
    {
      int hr = 0;
      int color1 = ColorTranslator.ToWin32(Color.BlueViolet);
      int color2 = 0;

      // color format is 0x00bbggrr
      hr = this.vmrWndConfig.SetBorderColor(color1);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrWndConfig.GetBorderColor(out color2);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(color2 == color1, "Set / Get BorderColor");
    }

    void TestColorKey()
    {
      int hr = 0;
      int color1 = ColorTranslator.ToWin32(Color.Violet);
      int color2 = 0;

      // color format is 0x00bbggrr
      hr = this.vmrWndConfig.SetColorKey(color1);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrWndConfig.GetColorKey(out color2);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(color2 == color1, "Set / Get BorderColor");
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

    void TestVideoPosition()
    {
      this.IVMRWindowlessControlTest_Resize(this, null);
    }

    void TestRepaintVideo()
    {
      // see IVMRWindowlessControl9Test_Paint...
    }

    void TestDisplayModeChanged()
    {
      int hr = 0;

      hr = this.vmrWndConfig.DisplayModeChanged();
      Marshal.ThrowExceptionForHR(hr);

      // Can do nothing except calling this method
      Debug.Assert(hr == 0, "DisplayModeChanged");
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


    private void BuildGraph(string sFileName, out IFilterGraph2 graphBuilder, out IBaseFilter sourceFilter, out IBaseFilter vmr7Filter)
    {
      int hr = 0;

      graphBuilder = new FilterGraph() as IFilterGraph2;
      vmr7Filter = null;

      try
      {
        hr = graphBuilder.AddSourceFilter(sFileName, sFileName, out sourceFilter);
        Marshal.ThrowExceptionForHR(hr);


        vmr7Filter = (IBaseFilter) new VideoMixingRenderer();
        hr = graphBuilder.AddFilter(vmr7Filter, "VMR7");
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

    private void ConnectGraph(ref IFilterGraph2 graphBuilder, ref IBaseFilter sourceFilter, ref IBaseFilter vmr7Filter)
    {
      int hr = 0;
      IPin pinOut = null;

      try
      {
        pinOut = DsFindPin.ByDirection(sourceFilter, PinDirection.Output, 0);

        hr = graphBuilder.RenderEx(pinOut, AMRenderExFlags.RenderToExistingRenderers, IntPtr.Zero);
        Marshal.ThrowExceptionForHR(hr);
      }
      finally
      {
        Marshal.ReleaseComObject(pinOut);
      }
    }

    private void IVMRWindowlessControlTest_Paint(object sender, PaintEventArgs e)
    {
      int hr = 0;

      if (isPlaying)
      {
        if (this.vmrWndConfig != null)
        {
          Trace.WriteLine("Dans Paint");
          IntPtr hDC = e.Graphics.GetHdc();
          hr = this.vmrWndConfig.RepaintVideo(this.Handle, hDC);
          e.Graphics.ReleaseHdc(hDC);
          Debug.Assert(hr == 0, "RepaintVideo");
        }
      }
    }

    private void IVMRWindowlessControlTest_Resize(object sender, System.EventArgs e)
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
