// added "none" to CDef
// changed CreateClassEnumerator from ref Guid to guid
// changed CreateClassEnumerator from int to CDef
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    public class ICreateDevEnumTest
    {
        // This is used to signal when the callbacks are called
        public ICreateDevEnumTest()
        {
        }

        public void DoTests()
        {
            int hr;

            int x;
            CreateDevEnum cde = new CreateDevEnum();
            ICreateDevEnum devEnum = cde as ICreateDevEnum;
            IEnumMoniker em;
            IMoniker[] monikers = new IMoniker[5];
            IntPtr p = Marshal.AllocCoTaskMem(4);

            try
            {
                hr = devEnum.CreateClassEnumerator(FilterCategory.ActiveMovieCategories, out em, CDef.None);
                DsError.ThrowExceptionForHR(hr);

                hr = em.Next(monikers.Length, monikers, p);
                DsError.ThrowExceptionForHR(hr);

                x = Marshal.ReadInt32(p);
            }
            finally
            {
                Marshal.FreeCoTaskMem(p);
            }

            Debug.Assert(x > 0, "CreateClassEnumerator");
        }
    }
}