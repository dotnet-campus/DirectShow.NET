using System;
using System.Runtime.InteropServices;

namespace DirectShowLib.Sample
{
	internal sealed class NativeMethodes
	{
    // Graphics.GetHdc() have sereral "bugs" detailled here : 
    // http://support.microsoft.com/default.aspx?scid=kb;en-us;311221
    // (see case 2) So we have to play with old school GDI...
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

  }
}
