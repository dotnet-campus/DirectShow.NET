using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IMPEG2ComponentTypeTest
	{
    private IMPEG2ComponentType mpeg2CompType = null;

    public IMPEG2ComponentTypeTest()
		{
		}

    public void DoTests()
    {
      mpeg2CompType = (IMPEG2ComponentType) new MPEG2ComponentType();

      try
      {
        TestStreamType();
      }
      finally
      {
        Marshal.ReleaseComObject(mpeg2CompType);
      }
    }

    private void TestStreamType()
    {
      int hr = 0;
      MPEG2StreamType streamType;

      hr = mpeg2CompType.put_StreamType(MPEG2StreamType.IsoIec13818_2_Video);
      DsError.ThrowExceptionForHR(hr);

      hr = mpeg2CompType.get_StreamType(out streamType);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(streamType == MPEG2StreamType.IsoIec13818_2_Video, "IMPEG2ComponentType.get_StreamType / put_StreamType");
    }
  }
}
