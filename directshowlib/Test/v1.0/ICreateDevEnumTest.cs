// added "none" to CDef
// changed CreateClassEnumerator from ref Guid to guid
// changed CreateClassEnumerator from int to CDef
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class ICreateDevEnumTest
    {
        // This is used to signal when the callbacks are called
        public ICreateDevEnumTest()
        {
        }

        [Test]
        public void DoTests()
        {
            int hr;

            CreateDevEnum cde = new CreateDevEnum();
            ICreateDevEnum devEnum = cde as ICreateDevEnum;
            UCOMIEnumMoniker em;
            UCOMIMoniker[] monikers = new UCOMIMoniker[1];
            int lFetched;

            hr = devEnum.CreateClassEnumerator(FilterCategory.VideoCompressorCategory, out em, CDef.DevmonDMO);
            DsError.ThrowExceptionForHR(hr);

            hr = em.Next(1, monikers, out lFetched);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(lFetched > 0, "CreateClassEnumerator");
        }
    }
}