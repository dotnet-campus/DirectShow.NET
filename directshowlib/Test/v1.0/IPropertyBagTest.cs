// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemruntimeinteropservicesexcepinfoclasstopic.asp

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IPropertyBagTest
    {
        IPropertyBag m_ipb;

        public IPropertyBagTest()
        {
        }

        [Test]
        public void DoTests()
        {
            BuildGraph();

            TestReadWrite();
        }

        void TestReadWrite()
        {
            int hr;
            object o = "moo";
            object o2 = "zoo";

            hr = m_ipb.Write("name", ref o);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ipb.Read("name", out o2, null);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert((string)o2 == (string)o, "Read/Write");
        }

        void BuildGraph()
        {
            IBaseFilter ppFilter;
            
            ppFilter = new AviDest() as IBaseFilter;

            IPin ipin = DsFindPin.ByDirection(ppFilter, PinDirection.Input, 0);
            m_ipb = ipin as IPropertyBag;
        }
    }
}