todo: 
remove sStartDir, FormLoad
readme
addav to sync to common

Step 1:

Construct the class.  The constructor has parameters for setting the Frames per Second, BitCount, and Height & Width of the output file.

Step 2:

Add the files with AddFile.  You can specify the video file, audio file, and start/stop points.

Step 3:

Choose one of the RenderTo* options.  Parameters are used to configure whichever rendering option you choose (see the specific rendering option).  Callback pointers can be passed to any of these functions.

Step 4:

Start the rendering with StartRendering()

Misc:

There are other functions that may be useful: Cancel(), GetXML(), and Dispose.  Note that Dispose should not be called within a Form.Dispose method.  See the method for details.

Events:


FileCompleted - Called when a file has finished processing.  Note that for RenderToWindow, sometimes frames get dropped, which can result missed events.


Completed - Called at the end of processing - contains final event code (Complete, UserAbort, Disk space problems, etc.

