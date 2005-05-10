using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Description résumée de IVMRFilterConfigTest.
	/// </summary>
  public class IVMRFilterConfigTest
  {
    private const string testFile = @"foo.avi";
    
    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter vmr7Filter = null;
    private IVMRFilterConfig vmrConfig = null;

    public IVMRFilterConfigTest()
    {
    }

    public void DoTests()
    {
      try
      {
        BuildGraph(testFile, out this.graphBuilder, out this.vmr7Filter);
        this.vmrConfig = this.vmr7Filter as IVMRFilterConfig;

        TestNumberOfStreams();
        TestRenderingMode();
        TestRenderingPrefs();
        TestSetImageCompositor();
      }
      finally
      {
        Marshal.ReleaseComObject(this.vmr7Filter);
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
      VMRMode mode;

      // default is VMR9Mode.Windowed
      hr = this.vmrConfig.SetRenderingMode(VMRMode.Windowless);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrConfig.GetRenderingMode(out mode);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(mode == VMRMode.Windowless, "Set / Get RenderingMode");
    }

    public void TestRenderingPrefs()
    {
      int hr = 0;
      VMRRenderPrefs prefs;

      // default value is 0
      hr = this.vmrConfig.SetRenderingPrefs(VMRRenderPrefs.DoNotRenderColorKeyAndBorder);
      Marshal.ThrowExceptionForHR(hr);

      hr = this.vmrConfig.GetRenderingPrefs(out prefs);
      Marshal.ThrowExceptionForHR(hr);

      // We should get what we set...
      Debug.Assert(prefs == VMRRenderPrefs.DoNotRenderColorKeyAndBorder, "Set / Get RenderingPrefs");
    }

    public void TestSetImageCompositor()
    {
      int hr = 0;
      NullSampleCompositor7 compositor = new NullSampleCompositor7();

      // VMR9 mixing mode have been activated in TestNumberOfStreams()
      hr = this.vmrConfig.SetImageCompositor(compositor);
      Marshal.ThrowExceptionForHR(hr);

      // Really don't know the validity of this test but it seem to work (ie hr == 0 for me)
      Debug.Assert(hr == 0, "SetImageCompositor");
    }

    private void BuildGraph(string sFileName, out IFilterGraph2 graphBuilder, out IBaseFilter vmr7Filter)
    {
      int hr = 0;
      IBaseFilter sourceFilter = null;

      graphBuilder = new FilterGraph() as IFilterGraph2;
      vmr7Filter = null;

      try
      {
        hr = graphBuilder.AddSourceFilter(sFileName, sFileName, out sourceFilter);
        Marshal.ThrowExceptionForHR(hr);

        vmr7Filter = (IBaseFilter) new VideoMixingRenderer();
        hr = graphBuilder.AddFilter(vmr7Filter, "VMR7");
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

    [ComVisible(true), Guid("BC49EDE1-DFFE-43ab-B8F7-2B8F6F26EF8E")]
    public class NullSampleCompositor7 : IVMRImageCompositor
    {
      #region Membres de IVMRImageCompositor

      public int CompositeImage(object pD3DDevice, object pddsRenderTarget, AMMediaType pmtRenderTarget, long rtStart, long rtEnd, int dwClrBkGnd, VMRVideoStreamInfo pVideoStreamInfo, int cStreams)
      {
        // TODO : ajoutez l'implémentation de NullSampleCompositor7.CompositeImage
        return 0;
      }

      public int TermCompositionTarget(object pD3DDevice, object pddsRenderTarget)
      {
        // TODO : ajoutez l'implémentation de NullSampleCompositor7.TermCompositionTarget
        return 0;
      }

      public int InitCompositionTarget(object pD3DDevice, object pddsRenderTarget)
      {
        // TODO : ajoutez l'implémentation de NullSampleCompositor7.InitCompositionTarget
        return 0;
      }

      public int SetStreamMediaType(int dwStrmID, AMMediaType pmt, bool fTexture)
      {
        // TODO : ajoutez l'implémentation de NullSampleCompositor7.SetStreamMediaType
        return 0;
      }

      #endregion

    }
  }

}
