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
AsfNet

A .NET sample application for directing DirectShow capture graphs to
a network port to be read by Windows Media Player
  
---------------------------------------------------------------------

In addition to the Windows Media library, you'll also need DirectShowLib (located
at http://sourceforge.net/projects/directshownet/).

After starting this application, you should be able to connect to the output of 
your capture device by connecting to a TCP/IP port.  From Windows Media Player, go to 
File/Open URL and enter your machine name/ip (for example: http://192.168.0.2:8080).

