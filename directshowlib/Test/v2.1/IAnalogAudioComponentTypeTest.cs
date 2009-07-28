using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.SBE;

namespace v2_1
{
    public class IAnalogAudioComponentTypeTest
    {
        public IAnalogAudioComponentTypeTest()
        {
        }

        public void DoTests()
        {
            int hr;
            AnalogAudioComponentType dcts = new AnalogAudioComponentType();

            IAnalogAudioComponentType ts = dcts as IAnalogAudioComponentType;
            Debug.Assert(ts != null);

            TVAudioMode m;
            hr = ts.put_AnalogAudioMode(TVAudioMode.LangB);
            DsError.ThrowExceptionForHR(hr);

            hr = ts.get_AnalogAudioMode(out m);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m == TVAudioMode.LangB);
        }
    }
}
