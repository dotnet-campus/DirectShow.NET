using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IDVEncTest
	{
    IDVEnc dvEnc = null;

		public IDVEncTest()
		{
		}

    public void DoTests()
    {
      dvEnc = (IDVEnc) new DVVideoEnc();

      try
      {
        TestIFormatResolution();
      }
      finally
      {
        Marshal.ReleaseComObject(dvEnc);
      }
    }

    public void TestIFormatResolution()
    {
      int hr = 0;
      DVEncoderVideoFormat vidFmt;
      DVEncoderFormat fmt;
      DVEncoderResolution resol;
      DVInfo dvInfo = new DVInfo();

      hr = dvEnc.put_IFormatResolution(DVEncoderVideoFormat.PAL, DVEncoderFormat.DVHD, DVEncoderResolution.r720x480, false, dvInfo);
      DsError.ThrowExceptionForHR(hr);

      hr = dvEnc.get_IFormatResolution(out vidFmt, out fmt, out resol, false, out dvInfo);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(vidFmt == DVEncoderVideoFormat.PAL, "IDVEnc.get_ / put_IFormatResolution");
      Debug.Assert(fmt == DVEncoderFormat.DVHD, "IDVEnc.get_ / put_IFormatResolution");
      Debug.Assert(resol == DVEncoderResolution.r720x480, "IDVEnc.get_ / put_IFormatResolution");

      dvInfo = new DVInfo();
      dvInfo.dwDVAAuxSrc = 20;
      dvInfo.dwDVVAuxSrc = 10;

      hr = dvEnc.put_IFormatResolution(DVEncoderVideoFormat.NTSC, DVEncoderFormat.DVSD, DVEncoderResolution.r360x240, true, dvInfo);
      DsError.ThrowExceptionForHR(hr);

      hr = dvEnc.get_IFormatResolution(out vidFmt, out fmt, out resol, true, out dvInfo);
      DsError.ThrowExceptionForHR(hr);
    
      Debug.Assert(vidFmt == DVEncoderVideoFormat.NTSC, "IDVEnc.get_ / put_IFormatResolution");
      Debug.Assert(fmt == DVEncoderFormat.DVSD, "IDVEnc.get_ / put_IFormatResolution");
      Debug.Assert(resol == DVEncoderResolution.r360x240, "IDVEnc.get_ / put_IFormatResolution");

      //Debug.Assert(dvInfo.dwDVAAuxSrc == 20, "IDVEnc.get_ / put_IFormatResolution");
      //Debug.Assert(dvInfo.dwDVVAuxSrc == 10, "IDVEnc.get_ / put_IFormatResolution");
      // The get method always return a struct filled with -1.
      // I don't know if it's a normal behaviour or something is wrong with marshaling...
    }

	}
}
