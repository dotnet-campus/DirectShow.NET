using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IDVBCLocatorTest
	{
		public IDVBCLocatorTest()
		{
        }

        public void DoTests()
        {
            IDVBCLocator loc = new DVBCLocator() as IDVBCLocator;

            Debug.Assert(loc != null, "IDVBCLocator");
        }
	}
}
