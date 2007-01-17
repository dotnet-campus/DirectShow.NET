using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IEvalRatTest
    {
        IEvalRat m_er;

        public void DoTests()
        {
            Configure();

            try
            {
                TestBlockUnrated();
                TestBlockedRatingAttributes();
                TestMost();
                TestRating();
            }
            finally
            {
            }
        }

        private void TestBlockUnrated()
        {
            int hr;
            bool b;

            hr = m_er.get_BlockUnRated(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == false, "get_BlockUnRated");

            hr = m_er.put_BlockUnRated(!b);
            DsError.ThrowExceptionForHR(hr);

            hr = m_er.get_BlockUnRated(out b);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(b == true, "put_BlockUnRated");
        }

        private void TestBlockedRatingAttributes()
        {
            int hr;
            BfEnTvRat_GenericAttributes a;

            hr = m_er.get_BlockedRatingAttributes(EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_0, out a);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(a == BfEnTvRat_GenericAttributes.BfAttrNone, "get_BlockedRatingAttributes");

            hr = m_er.put_BlockedRatingAttributes(EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_0, BfEnTvRat_GenericAttributes.BfIsAttr_1);
            DsError.ThrowExceptionForHR(hr);

            hr = m_er.get_BlockedRatingAttributes(EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_0, out a);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(a == BfEnTvRat_GenericAttributes.BfIsAttr_1, "put_BlockedRatingAttributes");
        }

        private void TestMost()
        {
            int hr;
            EnTvRat_System s;
            EnTvRat_GenericLevel g;
            BfEnTvRat_GenericAttributes a;

            hr = m_er.MostRestrictiveRating(
                EnTvRat_System.MPAA, EnTvRat_GenericLevel.TvRat_3, BfEnTvRat_GenericAttributes.BfIsAttr_4, 
                EnTvRat_System.Canadian_English, EnTvRat_GenericLevel.TvRat_6, BfEnTvRat_GenericAttributes.BfIsAttr_6, 
                out s, out g, out a);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == EnTvRat_System.Canadian_English);
        }

        private void TestRating()
        {
            int hr;

            hr = m_er.TestRating(
                EnTvRat_System.MPAA, 
                EnTvRat_GenericLevel.TvRat_5, 
                BfEnTvRat_GenericAttributes.BfIsAttr_5);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_er = (IEvalRat)new EvalRat();
        }
    }
}
