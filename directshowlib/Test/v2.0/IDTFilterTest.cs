using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IDTFilterTest
    {
        IDTFilter m_ft;

        public void DoTests()
        {
            Configure();

            try
            {
                TestBlockUnratedDelay();
                TestBlockUnrated();
                TestBlockedRatingAttributes();
                TestCurRate();
                TestOK();
            }
            finally
            {
            }
        }

        private void TestOK()
        {
            int hr;
            int hr2;

            hr = m_ft.get_EvalRatObjOK(out hr2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr2 == 0, "get_EvalRatObjOK");
        }

        private void TestBlockedRatingAttributes()
        {
            int hr;
            BfEnTvRat_GenericAttributes b;

            hr = m_ft.get_BlockedRatingAttributes(EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_0, out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == BfEnTvRat_GenericAttributes.BfAttrNone, "get_BlockedRatingAttributes");

            hr = m_ft.put_BlockedRatingAttributes(EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_0, BfEnTvRat_GenericAttributes.BfIsAttr_2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ft.get_BlockedRatingAttributes(EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_0, out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == BfEnTvRat_GenericAttributes.BfIsAttr_2, "put_BlockUnRated");
        }

        private void TestCurRate()
        {
            int hr;
            EnTvRat_System s;
            EnTvRat_GenericLevel g;
            BfEnTvRat_GenericAttributes i;

            hr = m_ft.GetCurrRating(out s, out g, out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == EnTvRat_System.TvRat_SystemDontKnow, "GetCurrRating");
        }

        private void TestBlockUnrated()
        {
            int hr;
            bool b;

            hr = m_ft.get_BlockUnRated(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == false, "get_BlockUnRated");

            hr = m_ft.put_BlockUnRated(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ft.get_BlockUnRated(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "put_BlockUnRated");
        }

        private void TestBlockUnratedDelay()
        {
            int hr;
            int pb;

            hr = m_ft.get_BlockUnRatedDelay(out pb);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pb == 0, "get_BlockUnRatedDelay");

            hr = m_ft.put_BlockUnRatedDelay(1000);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ft.get_BlockUnRatedDelay(out pb);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pb == 1000, "put_BlockUnRatedDelay");
        }

        private void Configure()
        {
            m_ft = (IDTFilter)new DTFilter();
        }
    }
}
