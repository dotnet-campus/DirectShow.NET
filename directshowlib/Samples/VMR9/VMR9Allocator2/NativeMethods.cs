/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DirectShowLib.Sample
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct Msg
  {
    public IntPtr hWnd;
    public int message;
    public IntPtr wParam;
    public IntPtr lParam;
    public uint time;
    public System.Drawing.Point p;
  }

  internal sealed class NativeMethods
  {
    [System.Security.SuppressUnmanagedCodeSecurity]
    [DllImport("User32.dll", CharSet=CharSet.Auto)]
    public static extern bool PeekMessage(out Msg msg, IntPtr hWnd, int messageFilterMin, int messageFilterMax, int flags);
  }
}
