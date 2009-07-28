using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_1
{
    public class IDigitalCableLocatorTest
    {
        public IDigitalCableLocatorTest()
        {
        }

        public void DoTests()
        {
            DigitalCableLocator dl = new DigitalCableLocator();

            IDigitalCableLocator dcl = dl as IDigitalCableLocator;

            Debug.Assert(dcl != null);
        }
    }
}
