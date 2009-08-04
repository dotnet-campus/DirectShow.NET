using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.SBE;

namespace v2_1
{
    public class IDigitalCableTuningSpaceTest
    {
        public IDigitalCableTuningSpaceTest()
        {
        }

        public void DoTests()
        {
            DigitalCableTuningSpace dcts = new DigitalCableTuningSpace();

            IDigitalCableTuningSpace ts = dcts as IDigitalCableTuningSpace;
            Debug.Assert(ts != null);

            int maxSrcId, minSrcId, maxMajorChannel, minMajorChannel;

            int hr = ts.put_MaxSourceID(5000);
            DsError.ThrowExceptionForHR(hr);

            hr = ts.get_MaxSourceID(out maxSrcId);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(maxSrcId == 5000);

            hr = ts.put_MinSourceID(1000);
            DsError.ThrowExceptionForHR(hr);

            hr = ts.get_MinSourceID(out minSrcId);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(minSrcId == 1000);

            hr = ts.put_MaxMajorChannel(3000);
            DsError.ThrowExceptionForHR(hr);

            hr = ts.get_MaxMajorChannel(out maxMajorChannel);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(maxMajorChannel == 3000);

            hr = ts.put_MinMajorChannel(1000);
            DsError.ThrowExceptionForHR(hr);

            hr = ts.get_MinMajorChannel(out minMajorChannel);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(minMajorChannel == 1000);
        }
    }
}
