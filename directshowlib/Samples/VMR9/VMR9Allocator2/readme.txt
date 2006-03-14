---------------------------------------------------------------------
VMR9Allocator2 (second try)

While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  

---------------------------------------------------------------------

This sample is an alternative to the DirectShow VMR9Allocator program.
Minimal knowledge in Managed Direct3D is required to understand this sample!

The original VMR9Allocator have some drawbacks.

1) The render loop is in the PresentImage method. Most real-life Direct3D 
applications handle the render loop themselves and some developers found that's 
difficult to use VMR9Allocator in an existing engine.

2) The InitializeDevice method use IVMRSurfaceAllocatorNotify9.AllocateSurfaceHelper
to allocate the VMR9 surface. This method works well in C++ but in managed world, 
you have to deal with unmanaged surface and create the managed representation 
of this surface. All that complications make the source more unreadable.

What this sample is and what it is not!

The purpose of this sample is to illustrate how an allocator / presenter can be 
rewrite to just provide a texture to a client application. In this version, the 
presenter draw nothing!

In this sample, the A/P part is almost complete but have some limitations:
	- Only one pin connected to the VMR9 filter
	- Direct3D errors are not checked. 
	- In a real-life application, D3D resources should be handled by the 3D 
	engine's resources manager to ease in "Device Lost" situations.
	
The sample's Managed Direct3D engine is reduced to the minimal. This part absolutely 
don't have production quality. It don't handle Direct3D exceptions and resize 
events (You are warned).

What's wrong with AllocateSurfaceHelper?

Nothing but it use is not necessary if you don't want it.

The VMR9 filter call the InitializeDevice method once for each media types 
supported by the down-stream filter. So if you don't like the first one 
(or more likely because your hardware don't like it) it's perfectly valid to 
refuse it!

VMR9Allocator2 allocate a managed surface then retrieve its unmanaged pointer 
so source code is less tainted by unmanaged code pieces and it is also more 
readable.

Why YUV and RGB media types are handled differently?

This is a common hardware limitation. Most video boards (even the last ones)
don't allow to build a 3D texture from an YUV off screen surface. So YUV surface 
must be converted to a RGB surface before been use in a 3D engine. 

YUV to RGB conversions are generally very fast on DirectX 9 class hardware. 
During my tests i found that with my Geforce FX 5900, YUV rendering is as fast 
as RGB rendering even with the extra copy...

This sample uses the following DirectShow Interfaces:

	IGraphBuilder
	IBaseFilter
	IMediaControl
	IMediaEventEx
	IVMRFilterConfig9
	IVMRMixerControl9
	IVMRSurfaceAllocatorNotify9
	IVMRSurfaceAllocator9
	IVMRImagePresenter9

Managed DirectX must be installed to run this sample.
