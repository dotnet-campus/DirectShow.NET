/************************************************************************

DirectShowLib - Provide access to DirectShow interfaces via .NET
Copyright (C) 2005
http://sourceforge.net/projects/directshownet/

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

**************************************************************************/

---------------------------------------------------------------------
CreateAsf

A .NET sample application for creating Asf or WMV files from a collection of bitmaps
  
---------------------------------------------------------------------

There are some useful comments at the top of CwmvFile.cs.

Description:

Not really much to say.  Create an instance of the class, then pass it bitmaps:

    // Profile id for "Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)"
    Guid g = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

    CwmvFile f = new CwmvFile("foo.asf", g, 2); // 2 FPS
    Bitmap b;

    for (int x=0; x < 100; x++)
    {
        b = new Bitmap(string.Format("C:\\pictures\\{0:00000000}.jpg", x));
        f.AppendNewFrame(b);
        b.Dispose();
    }
    f.Close();

