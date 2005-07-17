using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

using DirectShowLib.Dvd;

namespace DirectShowLib.Test
{
	public class IVMRDeinterlaceControl9Test
	{
    // To Deinterlace, an interlaced video source is needed...
    // This is not the case of foo.avi and some low budget analog capture cards don't send 
    // VideoInfo2 format stream (see the GetVideoDesc method).
    // So i used the testme.iso file who need to be mounted with Nero DriveImage or something else.
    // Customize this constant to match your logical drive. 
    // This test should also works with a real DVD...
    const string dvdDrive = @"G:\VIDEO_TS";

    private IFilterGraph2 graphBuilder = null;
    private IBaseFilter dvdNavigator = null;
    private IBaseFilter vmr9 = null;
    private IBaseFilter dsRenderer = null;

    private IVMRDeinterlaceControl9 deinterlaceControl = null;

    private VMR9VideoDesc videoDesc;
    private Guid[] deinterlaceModes;
    
    public IVMRDeinterlaceControl9Test()
		{
		}

    public void DoTests()
    {
      try
      {
        BuildGraph();

        TestGetNumberOfDeinterlaceModes();
        TestGetActualDeinterlaceMode();
        TestDeinterlaceMode();
        TestGetDeinterlaceModeCaps();
        TestDeinterlacePrefs();
      }
      finally
      {
        Marshal.ReleaseComObject(vmr9);
        Marshal.ReleaseComObject(dsRenderer);
        Marshal.ReleaseComObject(dvdNavigator);
        Marshal.ReleaseComObject(graphBuilder);
      }
    }

    private void BuildGraph()
    {
      int hr = 0;

      // quick and dirty dvd graph building
      graphBuilder = (IFilterGraph2) new FilterGraph();
      dvdNavigator = (IBaseFilter) new DVDNavigator();
      vmr9 = (IBaseFilter) new VideoMixingRenderer9();
      dsRenderer = (IBaseFilter) new DSoundRender();

      hr = graphBuilder.AddFilter(dvdNavigator, "DVD Navigator");
      DsError.ThrowExceptionForHR(hr);

      hr = (dvdNavigator as IDvdControl2).SetDVDDirectory(dvdDrive);
      DsError.ThrowExceptionForHR(hr);

      hr = graphBuilder.AddFilter(vmr9, "VMR9");
      DsError.ThrowExceptionForHR(hr);

      // vmr9 is always in mixer mode

      deinterlaceControl = (IVMRDeinterlaceControl9) vmr9;

      hr = graphBuilder.AddFilter(dsRenderer, "Audio");
      DsError.ThrowExceptionForHR(hr);

      IPin pinOut = DsFindPin.ByDirection(dvdNavigator, PinDirection.Output, 0);
      IPin pinIn = DsFindPin.ByDirection(vmr9, PinDirection.Input, 0);

      hr = graphBuilder.Connect(pinOut, pinIn);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(pinOut);
      Marshal.ReleaseComObject(pinIn);

      pinOut = DsFindPin.ByDirection(dvdNavigator, PinDirection.Output, 1);
      pinIn = DsFindPin.ByDirection(dsRenderer, PinDirection.Input, 0);

      hr = graphBuilder.Connect(pinOut, pinIn);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(pinOut);
      Marshal.ReleaseComObject(pinIn);

      // Run graph
      hr = (graphBuilder as IMediaControl).Run();
      DsError.ThrowExceptionForHR(hr);
    }

    // this method is an implementation of the procedure describe in this page :
    // http://msdn.microsoft.com/library/en-us/directshow/htm/settingdeinterlacepreferences.asp?frame=true
    private VMR9VideoDesc GetVideoDesc9()
    {
      int hr = 0;
      AMMediaType mediaType = new AMMediaType();
      VMR9VideoDesc vDesc = new VMR9VideoDesc();
      vDesc.dwSize = Marshal.SizeOf(typeof(VMR9VideoDesc));

      IPin pinIn = DsFindPin.ByDirection(vmr9, PinDirection.Input, 0);

      hr = pinIn.ConnectionMediaType(mediaType);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(pinIn);

      if (mediaType.formatType == FormatType.VideoInfo2)
      {
        VideoInfoHeader2 videoHeader = (VideoInfoHeader2)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader2));
        if ((videoHeader.InterlaceFlags & AMInterlace.IsInterlaced) != 0)
        {
          vDesc.dwSampleWidth = videoHeader.BmiHeader.Width;
          vDesc.dwSampleHeight = videoHeader.BmiHeader.Height;
          vDesc.SampleFormat = ConvertInterlaceFlags(videoHeader.InterlaceFlags);
          vDesc.dwFourCC = videoHeader.BmiHeader.Compression;

          switch(videoHeader.AvgTimePerFrame)
          {
            case 166833:
            {
              vDesc.InputSampleFreq.dwNumerator = 60000;
              vDesc.InputSampleFreq.dwDenominator = 1001;
              break;
            }
            case 333667:
            {
              vDesc.InputSampleFreq.dwNumerator = 30000;
              vDesc.InputSampleFreq.dwDenominator = 1001;
              break;
            }
            case 333666: // this value is not define in the paper but is returned by testme.iso
            {
              vDesc.InputSampleFreq.dwNumerator = 30000;
              vDesc.InputSampleFreq.dwDenominator = 1001;
              break;
            }
            case 417188:
            {
              vDesc.InputSampleFreq.dwNumerator = 24000;
              vDesc.InputSampleFreq.dwDenominator = 1001;
              break;
            }
            case 200000:
            {
              vDesc.InputSampleFreq.dwNumerator = 50;
              vDesc.InputSampleFreq.dwDenominator = 1;
              break;
            }
            case 400000:
            {
              vDesc.InputSampleFreq.dwNumerator = 25;
              vDesc.InputSampleFreq.dwDenominator = 1;
              break;
            }
            case 416667:
            {
              vDesc.InputSampleFreq.dwNumerator = 24;
              vDesc.InputSampleFreq.dwDenominator = 1;
              break;
            }
            default:
            {
              throw new ApplicationException("Unknown AvgTimePerFrame : " + videoHeader.AvgTimePerFrame);
            }
          }

          // Video is interleaved
          vDesc.OutputFrameFreq.dwNumerator = vDesc.InputSampleFreq.dwNumerator * 2;
          vDesc.OutputFrameFreq.dwDenominator = vDesc.InputSampleFreq.dwDenominator;
        }
        else
          throw new ApplicationException("Only interlaced formats");
      }
      else
        throw new ApplicationException("Only VIDEOINFOHEADER2 formats can be interlaced");

      DsUtils.FreeAMMediaType(mediaType);
      return vDesc;
    }

    private VMR9SampleFormat ConvertInterlaceFlags(AMInterlace dwInterlaceFlags)
    {
      if ((dwInterlaceFlags & AMInterlace.IsInterlaced) != 0)
      {
        if ((dwInterlaceFlags & AMInterlace.OneFieldPerSample) != 0)
        {
          if ((dwInterlaceFlags & AMInterlace.Field1First) != 0)
            return VMR9SampleFormat.FieldSingleEven;
          else
            return VMR9SampleFormat.FieldSingleOdd;
        }
        else
        {
          if ((dwInterlaceFlags & AMInterlace.Field1First) != 0)
            return VMR9SampleFormat.FieldInterleavedEvenFirst;
          else
            return VMR9SampleFormat.FieldInterleavedOddFirst;
        }
      }
      else
        return VMR9SampleFormat.ProgressiveFrame;
    }

    public void TestGetNumberOfDeinterlaceModes()
    {
      int hr = 0;
      int numModes = 0;

      videoDesc = GetVideoDesc9();

      hr = deinterlaceControl.GetNumberOfDeinterlaceModes(ref videoDesc, ref numModes, null);
      DsError.ThrowExceptionForHR(hr);

      deinterlaceModes = new Guid[numModes];

      hr = deinterlaceControl.GetNumberOfDeinterlaceModes(ref videoDesc, ref numModes, deinterlaceModes);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == 0, "IVMRDeinterlaceControl9.GetNumberOfDeinterlaceModes");
    }

    public void TestGetActualDeinterlaceMode()
    {
      int hr = 0;
      Guid mode;
      bool foundInModesList = false;

      hr = deinterlaceControl.GetActualDeinterlaceMode(0, out mode);
      DsError.ThrowExceptionForHR(hr);

      // returned mode should be one of the previously returned
      foreach(Guid g in deinterlaceModes)
      {
        if (g == mode)
        {
          foundInModesList = true;
          break;
        }
      }

      Debug.Assert(foundInModesList, "IVMRDeinterlaceControl9.GetActualDeinterlaceMode");
    }

    public void TestDeinterlaceMode()
    {
      int hr = 0;
      Guid mode;

      // Try to activate the second best mode (assuming at least 2 are supported)
      hr = deinterlaceControl.SetDeinterlaceMode(0, deinterlaceModes[1]);
      DsError.ThrowExceptionForHR(hr);

      hr = deinterlaceControl.GetDeinterlaceMode(0, out mode);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(mode == deinterlaceModes[1], "IVMRDeinterlaceControl9.GetDeinterlaceMode / SetDeinterlaceMode");
    }

    public void TestGetDeinterlaceModeCaps()
    {
      int hr = 0;
      VMR9DeinterlaceCaps caps = new VMR9DeinterlaceCaps();
      caps.dwSize = Marshal.SizeOf(typeof(VMR9DeinterlaceCaps));

      // Get caps of the last mode.
      // Last mode is generaly BOB
      hr = deinterlaceControl.GetDeinterlaceModeCaps(deinterlaceModes[deinterlaceModes.Length - 1], ref videoDesc, ref caps);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert((hr == 0) && (caps.DeinterlaceTechnology == VMR9DeinterlaceTech.BOBVerticalStretch),"IVMRDeinterlaceControl9.GetDeinterlaceModeCaps");
    }

    public void TestDeinterlacePrefs()
    {
      int hr = 0;

      VMR9DeinterlacePrefs readPrefs;

      // My VMR9 don't like BOB...
      hr = deinterlaceControl.SetDeinterlacePrefs(VMR9DeinterlacePrefs.Weave);
      DsError.ThrowExceptionForHR(hr);

      hr = deinterlaceControl.GetDeinterlacePrefs(out readPrefs);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(readPrefs == VMR9DeinterlacePrefs.Weave, "IVMRDeinterlaceControl9.GetDeinterlacePrefs / SetDeinterlacePrefs");
    }

	}
}
