using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IDMOWrapperFilterTest
	{
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter fileSource = null;
    private IBaseFilter aviSplitter = null;
    private IBaseFilter dmoFilter = null;
    private IDMOWrapperFilter dmoWrapperFilter = null;
    private IBaseFilter directSoundDevice = null;

		public IDMOWrapperFilterTest()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();
      }
      finally
      {
        Marshal.ReleaseComObject(directSoundDevice);
        Marshal.ReleaseComObject(dmoFilter);
        Marshal.ReleaseComObject(aviSplitter);
        Marshal.ReleaseComObject(fileSource);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;
      Guid wavesReverbDMO = new Guid("87fc0268-9a55-4360-95aa-004a1d9de26c");
      IPin pinIn, pinOut;

      graphBuilder = (IFilterGraph2) new FilterGraph();

      // Add a source filter
      hr = graphBuilder.AddSourceFilter(@"..\..\..\Resources\foo.avi", "foo.avi", out fileSource);
      DsError.ThrowExceptionForHR(hr);

      // Add an Avi Splitter
      aviSplitter = (IBaseFilter) new AviSplitter();
      hr = graphBuilder.AddFilter(aviSplitter, "Splitter");
      DsError.ThrowExceptionForHR(hr);

      // Add a DMO Wrapper Filter
      dmoFilter = (IBaseFilter) new DMOWrapperFilter();
      dmoWrapperFilter = (IDMOWrapperFilter) dmoFilter;

      // IDMOWrapperFilter unique method is tested here
      //
      // Init this filter with WavesReverb DMO Object
      //
      hr = dmoWrapperFilter.Init(wavesReverbDMO, DMOCategory.AudioEffect);
      DsError.ThrowExceptionForHR(hr);

      // Add it to the Graph
      hr = graphBuilder.AddFilter(dmoFilter, "DMO Filter");
      DsError.ThrowExceptionForHR(hr);

      // Add an audio renderer
      directSoundDevice = (IBaseFilter) new DSoundRender();
      hr = graphBuilder.AddFilter(directSoundDevice, "DirectSound Device");
      DsError.ThrowExceptionForHR(hr);

      // Connect Source filter with Avi Splitter
      pinOut = DsFindPin.ByDirection(fileSource, PinDirection.Output, 0);
      pinIn = DsFindPin.ByDirection(aviSplitter, PinDirection.Input, 0);
      hr = graphBuilder.Connect(pinOut, pinIn);
      DsError.ThrowExceptionForHR(hr);
      Marshal.ReleaseComObject(pinOut);
      Marshal.ReleaseComObject(pinIn);

      // Connect Avi Splitter audio (2nd) pin with DMO Wrapper
      // Should add an audio codec between the 2 filters
      pinOut = DsFindPin.ByDirection(aviSplitter, PinDirection.Output, 1);
      pinIn = DsFindPin.ByDirection(dmoFilter, PinDirection.Input, 0);
      hr = graphBuilder.Connect(pinOut, pinIn);
      DsError.ThrowExceptionForHR(hr);
      Marshal.ReleaseComObject(pinOut);
      Marshal.ReleaseComObject(pinIn);

      // Connect DMO Wrapper with the audio renderer
      pinOut = DsFindPin.ByDirection(dmoFilter, PinDirection.Output, 0);
      pinIn = DsFindPin.ByDirection(directSoundDevice, PinDirection.Input, 0);
      hr = graphBuilder.Connect(pinOut, pinIn);
      DsError.ThrowExceptionForHR(hr);
      Marshal.ReleaseComObject(pinOut);
      Marshal.ReleaseComObject(pinIn);


      // Run this graph (just for fun)
      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);

      System.Threading.Thread.Sleep(4000);
    }
	}
}
