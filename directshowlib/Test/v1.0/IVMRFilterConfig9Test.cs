using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Description résumée de IVMRFilterConfig9Test.
	/// </summary>
  public class IVMRFilterConfig9Test
  {
    private const string testFile = @"foo.avi";
    
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr9Filter = null;
    private IVMRFilterConfig9 vmrConfig = null;

    public IVMRFilterConfig9Test()
    {
    }

    public void DoTests()
    {
      try
      {
        BuildGraph(testFile, out this.graphBuilder, out this.vmr9Filter);
        this.vmrConfig = this.vmr9Filter as IVMRFilterConfig9;

        TestNumberOfStreams();
        TestRenderingMode();
        TestRenderingPrefs();
        TestSetImageCompositor();
      }
      finally
      {
        Marshal.ReleaseComObject(this.vmr9Filter);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    public void TestNumberOfStreams()
    {
      int hr = 0;
      int streamNb = 0;

      hr = this.vmrConfig.SetNumberOfStreams(2);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrConfig.GetNumberOfStreams(out streamNb);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(streamNb == 2, "Set / Get NumberOfStreams");
    }

    public void TestRenderingMode()
    {
      int hr = 0;
      VMR9Mode mode;

      // default is VMR9Mode.Windowed
      hr = this.vmrConfig.SetRenderingMode(VMR9Mode.Windowless);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrConfig.GetRenderingMode(out mode);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(mode == VMR9Mode.Windowless, "Set / Get RenderingMode");
    }

    public void TestRenderingPrefs()
    {
      int hr = 0;
      VMR9RenderPrefs prefs;

      // default value is 0
      hr = this.vmrConfig.SetRenderingPrefs(VMR9RenderPrefs.DoNotRenderBorder);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrConfig.GetRenderingPrefs(out prefs);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(prefs == VMR9RenderPrefs.DoNotRenderBorder, "Set / Get RenderingPrefs");
    }

    public void TestSetImageCompositor()
    {
      int hr = 0;
      NullSampleCompositor compositor = new NullSampleCompositor();

      // VMR9 mixing mode have been activated in TestNumberOfStreams()
      hr = this.vmrConfig.SetImageCompositor(compositor);
      Marshal.ThrowExceptionForHR(hr);

      // Really don't know the validity of this test but it seem to work (ie hr == 0 for me)
      Debug.Assert(hr == 0, "SetImageCompositor");
    }

    private void BuildGraph(string sFileName, out IFilterGraph2 graphBuilder, out IBaseFilter vmr9Filter)
    {
      int hr = 0;
      IBaseFilter sourceFilter = null;

      graphBuilder = new FilterGraph() as IFilterGraph2;
      vmr9Filter = null;

      try
      {
        hr = graphBuilder.AddSourceFilter(sFileName, sFileName, out sourceFilter);
        Marshal.ThrowExceptionForHR(hr);

        vmr9Filter = (IBaseFilter) new VideoMixingRenderer9();
        hr = graphBuilder.AddFilter(vmr9Filter, "VMR9");
      }
      catch
      {
        Marshal.ReleaseComObject(graphBuilder);
        throw;
      }
      finally
      {
        Marshal.ReleaseComObject(sourceFilter);
      }
    }
  }

  [ComVisible(true), Guid("6A853B83-EAA3-456c-9DD7-AA7F8B40BD72")]
  public class NullSampleCompositor : IVMRImageCompositor9
  {
    #region Membres de IVMRImageCompositor9

    public int CompositeImage(IntPtr pD3DDevice, IntPtr pddsRenderTarget, AMMediaType pmtRenderTarget, long rtStart, long rtEnd, int dwClrBkGnd, VMR9VideoStreamInfo[] pVideoStreamInfo, int cStreams)
    {
      // TODO : ajoutez l'implémentation de NullCompositor.CompositeImage
      return 0;
    }

    public int TermCompositionDevice(IntPtr pD3DDevice)
    {
      // TODO : ajoutez l'implémentation de NullCompositor.TermCompositionDevice
      return 0;
    }

    public int SetStreamMediaType(int dwStrmID, AMMediaType pmt, bool fTexture)
    {
      // TODO : ajoutez l'implémentation de NullCompositor.SetStreamMediaType
      return 0;
    }

    public int InitCompositionDevice(IntPtr pD3DDevice)
    {
      // TODO : ajoutez l'implémentation de NullCompositor.InitCompositionDevice
      return 0;
    }

    #endregion
  }

}
