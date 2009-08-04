using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_1
{
    public class IBDACreateTuneRequestExTest
    {
        public IBDACreateTuneRequestExTest()
        {
        }

        public void DoTests()
        {
            IBDACreateTuneRequestEx bdaCtrEx = new DVBTuningSpace() as IBDACreateTuneRequestEx;
            Debug.Assert(bdaCtrEx != null);

            ITuneRequest tr;

            int hr = bdaCtrEx.CreateTuneRequestEx(typeof(IDVBTuneRequest).GUID, out tr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(tr is IDVBTuneRequest);
        }

    }
}
