using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IBasicAudioTest
	{
    IFilterGraph2 graphBuilder = null;
    IBaseFilter filter = null;
    IBasicAudio audio = null;
    
		public IBasicAudioTest()
		{
		}

    public void DoTests()
    {
      try
      {
        // We need a stereo sound renderer...
        BuildGraph("foo.avi", out this.graphBuilder, out this.filter);

        this.audio = (IBasicAudio) this.filter;

        TestBalance();
        TestVolume();
      }
      finally
      {
        Marshal.ReleaseComObject(this.filter);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    public void TestBalance()
    {
      int hr = 0;
      int bal1 = 0, bal2 = 0;

      // set balance to full left
      hr = this.audio.put_Balance(-10000);
      Marshal.ThrowExceptionForHR(hr);

      // get this value
      hr = this.audio.get_Balance(out bal1);
      Marshal.ThrowExceptionForHR(hr);

      // set balance to full right
      hr = this.audio.put_Balance(+10000);
      Marshal.ThrowExceptionForHR(hr);

      // get this value
      hr = this.audio.get_Balance(out bal2);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(bal1 == -10000 && bal2 == +10000, "IBasicAudio.(get / put)_Balance");
    }

    public void TestVolume()
    {
      int hr = 0;

      int vol1 = 0, vol2 = 123456;

      // set volume to silence
      hr = this.audio.put_Volume(-10000);
      Marshal.ThrowExceptionForHR(hr);

      // get this value
      hr = this.audio.get_Volume(out vol1);
      Marshal.ThrowExceptionForHR(hr);

      // pomp up the volume
      hr = this.audio.put_Volume(0);
      Marshal.ThrowExceptionForHR(hr);

      // get this value
      hr = this.audio.get_Volume(out vol2);
      Marshal.ThrowExceptionForHR(hr);

      Debug.Assert(vol1 == -10000 && vol2 == 0, "IBasicAudio.(get / set)_Volume");
    }

    private void BuildGraph(string sFileName, out IFilterGraph2 graphBuilder, out IBaseFilter audioFilter)
    {
      int hr = 0;
      IBaseFilter sourceFilter = null;
      IPin pinOut = null;

      graphBuilder = new FilterGraph() as IFilterGraph2;
      audioFilter = null;

      try
      {
        hr = graphBuilder.AddSourceFilter(sFileName, sFileName, out sourceFilter);
        Marshal.ThrowExceptionForHR(hr);

        audioFilter = (IBaseFilter) new DSoundRender();
        hr = graphBuilder.AddFilter(audioFilter, "DirectSound Renderer");

        pinOut = DsFindPin.ByDirection(sourceFilter, PinDirection.Output, 0);

        hr = graphBuilder.RenderEx(pinOut, AMRenderExFlags.RenderToExistingRenderers, IntPtr.Zero);
        Marshal.ThrowExceptionForHR(hr);
      }
      catch
      {
        Marshal.ReleaseComObject(graphBuilder);
        throw;
      }
      finally
      {
        Marshal.ReleaseComObject(sourceFilter);
        Marshal.ReleaseComObject(pinOut);
      }
    }


  }
}
