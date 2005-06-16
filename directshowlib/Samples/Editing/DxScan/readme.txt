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

A sample application scanning a media file looking for black frames
 
To call it, you basically need:

   Capture cam;

   cam = new Capture(@"C:\foo.mpg");
   cam.Start();
    
   cam.m_Count is the number of frames
   cam.m_Blacks is how many black frames

Most of the work is done in ISampleGrabberCB.BufferCB.  See the comments 
there.  Also, the algorithm used to scan for black frames isn't as efficient
as it could be.  However, the samples gives you an idea of how this could be
done.