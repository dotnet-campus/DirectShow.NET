using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class MoTest
    {
        public MoTest()
        {
        }

        public void DoTests()
        {
            TestInit();
            TestCopy();
            TestRegister();
            TestName();
            TestTypes();
        }

        private void TestName()
        {
            int hr;
            StringBuilder sb = new StringBuilder(80);

            hr = DMOUtils.DMOGetName(new Guid("{efe6629c-81f7-4281-bd91-c9d604a95af6}"), sb);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(sb.ToString() == "Chorus", "DMOGetName");
        }

        private void TestTypes()
        {
            int hr;
            int i, o;

            DMOPartialMediatype [] pInTypes = new DMOPartialMediatype[2];
            DMOPartialMediatype [] pOutTypes = new DMOPartialMediatype[2];

            hr = DMOUtils.DMOGetTypes(
                new Guid("{efe6629c-81f7-4281-bd91-c9d604a95af6}"),
                2,
                out i,
                pInTypes,
                2,
                out o,
                pOutTypes
                );

            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(i == o && i == 1, "DMOGetTypes");
            Debug.Assert(pInTypes[0].type == MediaType.Audio && pInTypes[0].subtype == MediaSubType.PCM, "DMOGetTypes2");
            Debug.Assert(pOutTypes[0].type == MediaType.Audio && pOutTypes[0].subtype == MediaSubType.PCM, "DMOGetTypes3");
        }

        private void TestInit()
        {
            int hr;
            AMMediaType pmt = new AMMediaType();

            hr = DMOUtils.MoInitMediaType(pmt, 30);
            Debug.Assert(hr == 0 && pmt.formatPtr != IntPtr.Zero && pmt.formatSize == 30, "MoInitMediaType");
            DsUtils.FreeAMMediaType(pmt);
        }

        private void TestCopy()
        {
            int hr;

            AMMediaType pmt1 = new AMMediaType();
            AMMediaType pmt2 = new AMMediaType();
            FilterGraph f = new FilterGraph();
            IntPtr ip = Marshal.GetIUnknownForObject(f);

            pmt1.fixedSizeSamples = true;
            pmt1.formatPtr = Marshal.AllocCoTaskMem(8);
            Marshal.WriteInt64(pmt1.formatPtr, long.MaxValue);
            pmt1.formatSize = 8;
            pmt1.formatType = FormatType.DvInfo;
            pmt1.majorType = MediaType.AuxLine21Data;
            pmt1.sampleSize = 65432;
            pmt1.subType = MediaSubType.AIFF;
            pmt1.temporalCompression = true;
            pmt1.unkPtr = ip;

            hr = DMOUtils.MoCopyMediaType(pmt2, pmt1);

            Debug.Assert(hr == 0 &&
                pmt2.fixedSizeSamples == true &&
                pmt2.formatPtr != pmt1.formatPtr &&
                Marshal.ReadInt64(pmt2.formatPtr) == long.MaxValue &&
                pmt2.formatSize == 8 &&
                pmt2.formatType == FormatType.DvInfo &&
                pmt2.majorType == MediaType.AuxLine21Data &&
                pmt2.sampleSize == 65432 &&
                pmt2.subType == MediaSubType.AIFF &&
                pmt2.temporalCompression == true &&
                pmt2.unkPtr == ip, "MoCopyMediaType");

            DsUtils.FreeAMMediaType(pmt1);
            DsUtils.FreeAMMediaType(pmt2);
        }

        private void TestRegister()
        {
            int hr;
            IEnumDMO idmo;
            Guid g = Guid.NewGuid();
            Guid g2 = Guid.NewGuid();

            Debug.WriteLine(g);
            Debug.WriteLine(g2);

            DMOPartialMediatype [] pIn = new DMOPartialMediatype[2];
            pIn[0] = new DMOPartialMediatype();
            pIn[0].type = g2;
            pIn[0].subtype = MediaSubType.RGB24;

            pIn[1] = new DMOPartialMediatype();
            pIn[1].type = g2;
            pIn[1].subtype = MediaSubType.RGB32;

            DMOPartialMediatype [] pOut = new DMOPartialMediatype[2];
            pOut[0] = new DMOPartialMediatype();
            pOut[0].type = g2;
            pOut[0].subtype = MediaSubType.RGB24;

            pOut[1] = new DMOPartialMediatype();
            pOut[1].type = g2;
            pOut[1].subtype = MediaSubType.RGB32;

            hr = DMOUtils.DMORegister("asdffdsa", g, DMOCategory.VideoEffect, DMORegisterFlags.None,
                pIn.Length,
                pIn,
                pOut.Length,
                pOut
                );

            Debug.Assert(hr == 0, "DMORegister");

            DMOPartialMediatype [] tIn = new DMOPartialMediatype[1];
            tIn[0].type = g2;
            tIn[0].subtype = MediaSubType.RGB32;

            hr = DMOUtils.DMOEnum(Guid.Empty, DMOEnumerator.IncludeKeyed, tIn.Length, tIn, 0, null, out idmo);
            DMOError.ThrowExceptionForHR(hr);

            int iCnt1 = CountEm(idmo);

            Debug.Assert(iCnt1 == 1, "DMORegister");

            hr = DMOUtils.DMOUnregister(g, DMOCategory.VideoEffect);
            DMOError.ThrowExceptionForHR(hr);

            int iCnt2 = CountEm(idmo);

            Debug.Assert(iCnt2 == 0, "DMOUnregister");
        }

        private int CountEm(IEnumDMO idmo)
        {
            int hr;
            int iCnt = 0;
            Guid [] g = new Guid[1];
            string [] sn = new string[1];

            do
            {
                hr = idmo.Next(1, g, sn, IntPtr.Zero);
            } while (hr == 0 && iCnt++ < 100000);

            DMOError.ThrowExceptionForHR(hr);

            return iCnt;
        }

    }
}

