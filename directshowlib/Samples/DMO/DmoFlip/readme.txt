There are several readme's with this project.  In addition to this file, there is a 
readme in both the DMO and the FormDmo folders.

The readme in the DmoForm folder talks about calling DMOs.  The readme in the DMO 
folder talks about the specific sample.  This readme talks in general about how to 
write a DMO.

///////////////////////////////////////////////////////////////////////////

START WITH THE MICROSOFT DOCS FOR DMOs!  In particular read about the
IMediaObject, IMediaParamInfo, IMediaParams, & DMO Wrapper Filter (if you
are using DirectShow graphs).  I do NOT intend to clutter up this file
and the source code repeating the descriptions of the interfaces, since 
they are already described adequately (for the most part), in MSDN.

When you read the MSDN docs about creating a DMO, they refer to a template that
you can use to make things easier.  I have translated this template into a C#
class.  To create a DMO, you can just create a child class, implement the necessary
abstract methods, and you should be good to go.  

Here is a more detailed description of the steps you need to take.  Note that you can
look at the sample code for examples of these steps.

1) Other than ripping out the rather lame logging, you shouldn't need to change
any code in IMediaObjectImpl.cs.  It is the initial entry point for all the
IMediaObject interfaces.  It performs parameter checking, makes sure the call
is appropriate for the current state, etc.  As needed it will make calls to
methods you will implement.

2) Create a class which implements the abstract IMediaObjectImpl class:

    [ComVisible(true), Guid("7EF28FD7-E88F-45bb-9CDD-8A62956F2D75"),
    ClassInterface(ClassInterfaceType.None)]
    public class DmoFlip : IMediaObjectImpl

3) Generate your own guid so the samples won't interfere with your code:
If you are running Dev Studio, go to Tools/Create Guid, choose "Registry 
Format", click "Copy", then paste into your code.

4) Create a constructor:

   public DmoFlip() : base(InputPinCount, OutputPinCount, ParamCount, TimeFormatFlags.Reference)
   
If you are planning to use this DMO with the DirectShow DMO Wrapper Filter, 
note that (up to and including DX v9.0), InputPinCount must be 1, 
and OutputPinCount must be > 0.  The ParamCount is the number of parameters 
your DMO supports, and can be zero.  In general, you should use 
TimeFormatFlags.Reference for the last paramter.

5) Create the COM register/unregister methods:

    [ComRegisterFunctionAttribute]
    static private void DoRegister(Type t)

    [ComUnregisterFunctionAttribute]
    static private void UnregisterFunction(Type t)

These tell the OS about your DMO.  They are called automatically during code
compilation.  If you are distributing your code, you will need to make sure
they get called during installation.  See the sample for things you need
to do during registration.

WARNING: If you use the "Register for COM Interop" compiler switch, the
compiler will attempt to register DirectShowLib.dll as well as your DMO.
Since DirectShowLib has no registration to perform, this generates an error.
That is why my sample uses pre/post build events to perform the registration.

6) Register the parameters your DMO supports using DefineParam.  Doing this once in
your constructor allows you to support IMediaParamInfo & IMediaParams.  You will also
need to use CalcValueForTime to find out what parameter value you should use at any 
given point during the stream.

7) Do everything else <g>.  Write the implementations for the abstract 
methods of IMediaObjectImpl, and any of the virtual methods that you need.  
See the sample code where there are comments with each method.  You can also 
look at the IMediaObjectImpl Class Template for C++ (documented as part of 
DirectShow) which served as an inspiration for much of the logic here.

If you aren't already knowledgeable about COM and writing multi-threaded apps, 
this is probably a good time to do a little research.  You may have multiple 
instances of your DMO running in the same process, in multiple processes, 
called on different threads, etc.  

As a simple example of the things you should be thinking of, the logging in 
(debug builds of) IMediaObjectImpl.cs opens a file as non-sharable.
However, if two applications try to instantiate your DMO, the second will fail, 
solely due to not being able to open the log file.
