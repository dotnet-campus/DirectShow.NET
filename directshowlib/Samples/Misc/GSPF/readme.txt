/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

The Generic Sample Pull Filter - A way to implement a pull source filter in c#


Why I wrote this filter:
=======================

Most of the explanation of "why" you can find in the readme for GSSF.  This filter was created to
allow for pull filters, which GSSF can't handle.


How it works:
=============

Let's start by talking about how the MS File Source (Async) works.  It has a single output pin.  It knows nothing about
the contents of the files it processes.  When a filter (like the Avi Splitter) connects to its output pin, they query
that output pin to see if it supports IAsyncReader (and fail to connect to the pin if it doesn't).  Once they have
an IAsyncReader, they use it like a .NET programmer might use a BinaryReader.  If you look at the methods on 
IAsyncReader, you'll see basic file operations: Length, Read, and async reading (Request/WaitForNext).  Also, the 
Read method specifies where in the file to read (similar to using FilePosition).

As for where the data comes from, the other filters don't really care.  They just want to be able to say "Give
me x bytes starting at location y."  If you can do that, they don't care whether the data comes from an avi file on
disk, an binary field in a sql database, or a stream over the net.

So, with that in mind, let's talk about how this code works.  Start by taking a look in PullClasses.cs.  You see the class
to use to create an instance of the filter (GenericSamplePullFilter).  There is also an interface for configuring that
filter (IGenericPullConfig).  After creating an instance of the filter, you would use IGenericPullConfig to tell the
Media type you support (see ConfigurePuller in BuildGraph.cs for an example).

The next thing you need to do is to let the filter know what COM object it should use to support other interfaces.  This
COM object is where your c# code is going to live.  It shouldn't be necessary for you to make any modifications to the
GenericSamplePushFilter code (the c++ stuff).

While this sample supports IAsyncReader, you should be able to support (nearly) any DS interfaces here, as long as you
are working with the Pull model.  So, send the CLSID of the COM object you create to SetEmbedded (also shown in 
ConfigurePuller).

Following this, you can do any configuration of your COM object you need, such as telling it what file to process (or 
what TCP/IP port to use, or what sql database to read the data from, etc) unless you have this hard coded in your COM 
object (and shame on you if you do<g>).  I have created my own interface to do this (ISetFileName in Reader.cs).  Yes, 
I could have used IFileSinkFilter from the DS library.  However, I wanted to emphasize that you can create whatever 
interface you like.  You can change this to pass in a SQL Server name/Database name/Table Name/Key field/field name.  
Or a TCP address/Port #.  Or whatever you need to know how to configure your COM object.

So, now when anyone queries the output pin of this filter for any interface (other than IPin IGenericPullConfig, & 
IQualityControl), your COM object will be queried.  If you say you support IAsyncReader, filters will use your code.

Saying you support an interface is as easy as:

   public class AsyncRdr : IAsyncReader
   
Actually *supporting* the interface means writing code for each of the methods of that interface, to do what the 
specs for that interface.

For the curious, the reason I needed to do this as a COM object was this:

If someone does a QI against the pin, I could easily hand them any interface (it doesn't have to be a COM object).  
However what if they have your interface and want to QI back to the IPin?  COM says you have to support this (and 
GraphEdt complains bitterly, if obscurely, if you don't).  So, I use COM aggregation, which means I need a COM 
object, not just an interface.


How to use this filter:
======================

Before you can use the filter for the first time, you must register it with COM.  Copy the file to where you want to install
it and run "RegSvr32 PullSource.ax".  Also, since you need a COM object to use the filter, the c# COM object you write will 
need to be registered.  Visual Studio does this for you as part of compiling.  But it is something to keep in mind if you will
be distributing your code.

Steps to use the existing code (you can see this in the Renderer project):

1) Create an instance of the GenericSamplePullFilter class and add it to the graph.
2) Set a Media Type on the filter (using IGenericPullConfig::SetMediaType).  To use the Reader COM object, this
must be MediaType.Stream & either MediaSubType.Avi or MediaSubType.MPEG1System.
3) Set the COM object the filter should use to support additional interfaces (using IGenericPullConfig::SetEmbedded).  To
use the Reader COM object, this must be typeof(AsyncRdr).GUID.

What to change to have your avi come from somewhere else.

1) Make a copy of the Reader project.
2) CHANGE THE GUID on the AsyncRdr class.
3) Change/replace the ISetFileName interface so the parameters you need to set can be passed in.
4) Change the methods of the AsyncRdr class to read the data from your location.

It should NOT be necessary to change or re-build the GenericSamplePushFilter code.


Important:
==========

To support IAsyncReader, you MUST support random seeking within the data.  How random depends on what the downstream filter
chooses to do.  It may be that it only backs up once to re-read a header.  But it may not.  


A few things to know:
====================

- This filter is useless in GraphEdt.

- While the sample shows a pull filter that implements IAsyncReader, in theory you could implement any interfaces there, 
except: IQualityControl IGenericPullConfig, & IPin.  However, it will only support Pull filters.  If you need to do push 
filters, use the GSSF project.

- Building the filter: The source to the filter is included.  However, you will need to have the base class libraries.  MS ships
the code to the libraries in the platform sdk, but does not include the actual .Lib files, so you need to build them first.  Then 
update this project to point to it.

- The code in the cpp file is pretty clean.  If you are comfortable at all with c++ code, this really isn't so bad to read.  The 
main file of interest is PullSourceFilter.cpp which is only ~200 (well-commented) lines.

