using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_1
{
    public class IChannelIDTuneRequestTest
    {
        public IChannelIDTuneRequestTest() { }

        public void DoTests()
        {
            IBDACreateTuneRequestEx bdaCtrEx = new ChannelIDTuningSpace() as IBDACreateTuneRequestEx;
            Debug.Assert(bdaCtrEx != null);

            ITuneRequest tr;
            int hr = bdaCtrEx.CreateTuneRequestEx(typeof(IChannelIDTuneRequest).GUID, out tr);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(tr is IChannelIDTuneRequest);

            IChannelIDTuneRequest chIdTr = (IChannelIDTuneRequest)tr;

            hr = chIdTr.put_ChannelID("azerty");
            DsError.ThrowExceptionForHR(hr);

            string chId;
            hr = chIdTr.get_ChannelID(out chId);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(chId.Equals("azerty"));
        }

    }
}
