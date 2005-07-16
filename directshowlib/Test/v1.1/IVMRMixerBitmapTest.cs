using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRMixerBitmapTest
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr = null;
    private IVMRMixerBitmap mixerBitmap = null;

    private Bitmap bmp = null;
    private Graphics g = null;
    private Color colorKey = Color.Magenta;
    private VMRAlphaBitmap alphaBitmap;
    
    // Graphics.GetHdc() have sereral bugs detailled here : 
    // http://support.microsoft.com/default.aspx?scid=kb;en-us;311221
    // (see case 2) So we have to play with old school GDI...
    // Loss 4 hours on that...

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    public IVMRMixerBitmapTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();
        GenerateBitmap();

        TestSetAlphaBitmap();
        TestGetAlphaBitmapParameters();

        RunGraph();
        System.Threading.Thread.Sleep(2000);

        TestUpdateAlphaBitmapParameters();
        System.Threading.Thread.Sleep(2000);
      }
      finally
      {
        if (g != null)
          g.Dispose();

        if (bmp != null)
          bmp.Dispose();

        Marshal.ReleaseComObject(mixerBitmap);
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

      // To enable the VMR7's mixer and compositor
      hr = (vmr as IVMRFilterConfig).SetNumberOfStreams(1);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      mixerBitmap = (IVMRMixerBitmap) vmr;
    }

    private void GenerateBitmap()
    {

      bmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
      g = Graphics.FromImage(bmp);

      // draw the background with the colorkey
      g.Clear(colorKey);

      g.TextRenderingHint = TextRenderingHint.AntiAlias;

      FontFamily fontFamily = new FontFamily("Tahoma");
      Font font = new Font(fontFamily, 100, FontStyle.Regular, GraphicsUnit.Pixel);

      g.DrawString("Vmr7", font, Brushes.RoyalBlue, new PointF(0, 64));

      // for debug purpose
      // bmp.Save("vmr7.png");
    }

    private void RunGraph()
    {
      int hr = 0;

      hr = (vmr as IVideoWindow).put_Width(640);
      DsError.ThrowExceptionForHR(hr);

      hr = (vmr as IVideoWindow).put_Height(480);
      DsError.ThrowExceptionForHR(hr);

      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    public void TestSetAlphaBitmap()
    {
      int hr = 0;

      // GDI Stuff
      IntPtr hdc = g.GetHdc();
      IntPtr memDC = CreateCompatibleDC(hdc);
      IntPtr hBirmap = bmp.GetHbitmap();
      SelectObject(memDC, hBirmap);

      alphaBitmap = new VMRAlphaBitmap();
      alphaBitmap.dwFlags = VMRBitmap.Hdc | VMRBitmap.SRCColorKey;
      alphaBitmap.hdc = memDC;
      alphaBitmap.pDDS = IntPtr.Zero;
      alphaBitmap.fAlpha = 0.5f;
      alphaBitmap.rSrc = new DsRect(0, 0, 255, 255); // SetAlphaBitmap only accept the full size
      alphaBitmap.rDest = new NormalizedRect(0.0f, 0.0f, 1.0f, 1.0f);
      alphaBitmap.clrSrcKey = ColorTranslator.ToWin32(Color.Magenta);

      hr = mixerBitmap.SetAlphaBitmap(ref alphaBitmap);
      DsError.ThrowExceptionForHR(hr);

      DeleteObject(hBirmap);
      DeleteDC(memDC);
      g.ReleaseHdc(hdc);

      Debug.Assert(hr == 0, "IVMRMixerBitmap.SetAlphaBitmap");
    }

    public void TestGetAlphaBitmapParameters()
    {
      int hr = 0;
      VMRAlphaBitmap tmp;

      hr = mixerBitmap.GetAlphaBitmapParameters(out tmp);
      DsError.ThrowExceptionForHR(hr);

      // We could test other fields but this one is significative...
      Debug.Assert(tmp.clrSrcKey == ColorTranslator.ToWin32(colorKey), "IVMRMixerBitmap.GetAlphaBitmapParameters");
    }

    public void TestUpdateAlphaBitmapParameters()
    {
      int hr = 0;

      // show the bitmap half smaller in the middle of the window
      // We don't change the image so no need to play again with GDI
      alphaBitmap = new VMRAlphaBitmap();
      alphaBitmap.dwFlags = VMRBitmap.SRCColorKey;
      alphaBitmap.hdc = IntPtr.Zero;
      alphaBitmap.pDDS = IntPtr.Zero;
      alphaBitmap.fAlpha = 0.5f;
      alphaBitmap.rSrc = new DsRect(0, 0, 255, 255);
      alphaBitmap.rDest = new NormalizedRect(0.25f, 0.25f, 0.75f, 0.75f);
      alphaBitmap.clrSrcKey = ColorTranslator.ToWin32(Color.Magenta);

      hr = mixerBitmap.UpdateAlphaBitmapParameters(ref alphaBitmap);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMixerBitmap.UpdateAlphaBitmapParameters");
    }

  }
}
