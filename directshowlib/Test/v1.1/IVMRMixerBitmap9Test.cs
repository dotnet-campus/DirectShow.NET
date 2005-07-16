using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IVMRMixerBitmap9Test
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9 = null;
    private IVMRMixerBitmap9 mixerBitmap = null;

    private Bitmap bmp = null;
    private Graphics g = null;
    private Color colorKey = Color.Magenta;
    private VMR9AlphaBitmap alphaBitmap;

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

    public IVMRMixerBitmap9Test()
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

      hr = graphBuilder.RenderFile(@"..\..\..\Resources\foo.avi", null);
      DsError.ThrowExceptionForHR(hr);

      mixerBitmap = (IVMRMixerBitmap9) vmr9;
    }

    private void GenerateBitmap()
    {
      bmp = new Bitmap(256, 256, PixelFormat.Format32bppArgb);
      g = Graphics.FromImage(bmp);

      // draw the background with the colorkey
      g.Clear(colorKey);

      g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

      FontFamily fontFamily = new FontFamily("Tahoma");
      System.Drawing.Font font = new System.Drawing.Font(fontFamily, 100, FontStyle.Regular, GraphicsUnit.Pixel);

      g.DrawString("Vmr9", font, Brushes.RoyalBlue, new PointF(0, 64));

      // for debug purpose
      // bmp.Save("vmr9.png");
    }

    private void RunGraph()
    {
      int hr = 0;

      hr = (vmr9 as IVideoWindow).put_Width(640);
      DsError.ThrowExceptionForHR(hr);

      hr = (vmr9 as IVideoWindow).put_Height(480);
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

      alphaBitmap = new VMR9AlphaBitmap();
      alphaBitmap.dwFlags = VMR9AlphaBitmapFlags.hDC | VMR9AlphaBitmapFlags.SrcColorKey | VMR9AlphaBitmapFlags.FilterMode;
      alphaBitmap.hdc = memDC;
      alphaBitmap.pDDS = IntPtr.Zero;
      alphaBitmap.fAlpha = 0.5f;
      alphaBitmap.rSrc = new DsRect(0, 0, 255, 255); // SetAlphaBitmap only accept the full size
      alphaBitmap.rDest = new NormalizedRect(0.0f, 0.0f, 1.0f, 1.0f);
      alphaBitmap.clrSrcKey = ColorTranslator.ToWin32(colorKey);
      alphaBitmap.dwFilterMode = VMRMixerPrefs.BiLinearFiltering;

      hr = mixerBitmap.SetAlphaBitmap(ref alphaBitmap);
      DsError.ThrowExceptionForHR(hr);

      DeleteObject(hBirmap);
      DeleteDC(memDC);
      g.ReleaseHdc(hdc);

      Debug.Assert(hr == 0, "IVMRMixerBitmap9.SetAlphaBitmap");
    }

    public void TestGetAlphaBitmapParameters()
    {
      int hr = 0;
      VMR9AlphaBitmap tmp;

      hr = mixerBitmap.GetAlphaBitmapParameters(out tmp);
      DsError.ThrowExceptionForHR(hr);

      // We could test other fields but this one is significative...
      Debug.Assert(tmp.clrSrcKey == ColorTranslator.ToWin32(colorKey), "IVMRMixerBitmap9.GetAlphaBitmapParameters");
    }

    public void TestUpdateAlphaBitmapParameters()
    {
      int hr = 0;

      // show the bitmap half smaller in the middle of the window
      // We don't change the image so no need to play again with GDI
      alphaBitmap = new VMR9AlphaBitmap();
      alphaBitmap.dwFlags = VMR9AlphaBitmapFlags.SrcColorKey | VMR9AlphaBitmapFlags.FilterMode;
      alphaBitmap.hdc = IntPtr.Zero;
      alphaBitmap.pDDS = IntPtr.Zero;
      alphaBitmap.fAlpha = 0.5f;
      alphaBitmap.rSrc = new DsRect(0, 0, 255, 255);
      alphaBitmap.rDest = new NormalizedRect(0.25f, 0.25f, 0.75f, 0.75f);
      alphaBitmap.clrSrcKey = ColorTranslator.ToWin32(colorKey);
      alphaBitmap.dwFilterMode = VMRMixerPrefs.BiLinearFiltering;

      hr = mixerBitmap.UpdateAlphaBitmapParameters(ref alphaBitmap);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRMixerBitmap9.UpdateAlphaBitmapParameters");
    }

	}
}
