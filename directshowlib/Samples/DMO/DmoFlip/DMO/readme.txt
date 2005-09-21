DMOFlip - A sample DMO that flips video on either the x or y axis (or both).

The DMO in this directory is based on the abstract class contained in IMediaObjectImpl.cs.  
The IMediaObjectImpl class is intended to be completely generic, and can be used to create 
other DMOs.  See the readme in the parent directory for descriptions of the steps.

DMOFlip is designed to work with the DMOWrapper Filter, meaning it can be added to DirectShow 
graphs.  You can find it in GraphEdit under "DMO Video Effects", and the FormDMO sample shows
how to call it programmatically.

The DMOFlip sample takes RGB24 or RGB32 video, and (optionally) flips it.  The flipping mode 
is controlled by the DMOFlip parameter, and can be set using either IMediaParams::SetParam 
or IMediaParams::AddEnvelope.  Valid values for FlipMode:

    FlipMode:
        None = 0,
        FlipY = 1,
        FlipX = 2,
        FlipY + FlipX = 3

Note that while the DoFlip routine might benefit (performance-wise) from using unsafe mode, 
I haven't done so in order to keep the sample simple.  This is a sample, not production code.
