// IXDSToRat								2	"Applications do not use this interface."

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    public class IXDSToRatTest
	{
        IXDSToRat m_xtr;

        public void DoTests()
        {
            Config();

            TestStep();
        }

        private void TestStep()
        {
            int hr;
            EnTvRat_System pe = EnTvRat_System.Canadian_French;
            EnTvRat_GenericLevel gl = EnTvRat_GenericLevel.TvRat_3;
            BfEnTvRat_GenericAttributes ga = BfEnTvRat_GenericAttributes.BfIsAttr_2;

            hr = m_xtr.Init();
            DsError.ThrowExceptionForHR(hr);

            // Returns S_FALSE indicating there is no stream...
            hr = m_xtr.ParseXDSBytePair(1, 2, out pe, out gl, out ga);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_xtr = new XDSToRat() as IXDSToRat;
        }
	}
}
