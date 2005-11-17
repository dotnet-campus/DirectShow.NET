using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IAuxInTuningSpaceTest
	{
		public IAuxInTuningSpaceTest()
		{
        }

        public void DoTests()
        {
            IAuxInTuningSpace loc = new AuxInTuningSpace() as IAuxInTuningSpace;

            Debug.Assert(loc != null, "IAuxInTuningSpace");
        }
	}
}
