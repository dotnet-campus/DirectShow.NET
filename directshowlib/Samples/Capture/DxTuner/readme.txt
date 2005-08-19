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

This sample application shows how to change channels on a tv tuner.

There are several things you should know:

- The audio pins are not connected in this application.
- The tuner button is greyed out until the graph is constructed.  Because
of the way the UI is structured, that means the graph must be running
to change the channel.
- If your video is blank, try playing with the crossbar.  Make sure the "Video
Decoder Out" is connected to something that has a signal (Tuner in, Composite In, 
etc).
- If your video is jerky, this is probably due to the fact that this program is
saving the video to disk.  If you are not using compression, you are writing huge
amounts of data.  As a for-instance 640 x 480 x 16bpp x 30fps = ~20meg per sec.  If 
you are using compression, your compressor may be too slow for use with live capture.  
As a test, try using Intel Indeo for compressing.  Make sure you click the "Quick 
Compress" on the compressor's property page.
