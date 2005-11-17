using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IMPEG2TuneRequestSupportTest
	{
		public IMPEG2TuneRequestSupportTest()
		{
        }

        public void DoTests()
        {
            IMPEG2TuneRequestSupport loc = new ATSCTuningSpace() as IMPEG2TuneRequestSupport;

            Debug.Assert(loc != null, "IMPEG2TuneRequestSupport");
        }
	}
}
