using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IMediaFilterTest
	{
    IMediaFilter filter = null;
    
    public IMediaFilterTest()
		{
		}

    public void DoTests()
    {

      try
      {
        // I decide to use the VMR9 filter because i love it !
        // This test could work with other filters with the notable exception of the 
        // VideoRendererDefault which is in fact a VMR7 on Windows XP...
        
        // All DirectShow filters must implement IBaseFilter...
        // which inherit from IMediaFilter...
        this.filter = (IMediaFilter) new VideoMixingRenderer9();

        TestRun();
        TestPause();
        TestStop();
        TestGetState();
        TestSyncSource();
      }
      finally
      {
        Marshal.ReleaseComObject(this.filter);
      }
    }

    public void TestStop()
    {
      int hr = 0;

      hr = this.filter.Stop();
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IMediaFilter.Stop");
    }

    public void TestPause()
    {
      int hr = 0;

      hr = this.filter.Pause();
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IMediaFilter.Pause");
    }

    public void TestRun()
    {
      int hr = 0;

      hr = this.filter.Run(0);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IMediaFilter.Run");
    }

    public void TestGetState()
    {
      int hr = 0;
      FilterState state;

      this.filter.Run(0);
      hr = this.filter.GetState(-1, out state);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(state == FilterState.Running, "IMediaFilter.GetState");

      this.filter.Stop();
      hr = this.filter.GetState(-1, out state);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(state == FilterState.Stopped, "IMediaFilter.GetState");
    }

    public void TestSyncSource()
    {
      int hr = 0;
      IReferenceClock systemClock = (IReferenceClock) new SystemClock();
      IReferenceClock readClock = null;

      // Try to assign the System Clock
      hr = this.filter.SetSyncSource(systemClock);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.filter.GetSyncSource(out readClock);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(readClock == systemClock, "IMediaFilter.GetSyncSource");

      // Try to assign no clock at all
      hr = this.filter.SetSyncSource(null);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.filter.GetSyncSource(out readClock);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(readClock == null, "IMediaFilter.GetSyncSource");

      // SetSyncSource method do an AddRef. Release all instances...
      while(Marshal.ReleaseComObject(systemClock) > 0);
    }
	}
}
