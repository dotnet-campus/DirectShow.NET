using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IDigitalLocatorTest
    {
        public void DoTests()
        {
            object o = new ATSCLocator();
            IDigitalLocator idl = o as IDigitalLocator;

            Debug.Assert(idl != null, "IDigitalLocator");
        }
    }
}

