using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;
using DirectShowLib.Dvd;

namespace DirectShowLib.Test
{
    public class IAMDecoderCapsTest
    {
        // The drive containing testme.iso
        const string MyDisk = @"e:\video_ts";

        IAMDecoderCaps m_idc;

        public IAMDecoderCapsTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestGetCaps();
            }
            finally
            {
                Marshal.ReleaseComObject(m_idc);
            }
        }

        private void TestGetCaps()
        {
            int hr;
            DecoderCap d;

            hr = m_idc.GetDecoderCaps(AMQueryDecoder.VMRSupport, out d);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            int hr;
            IGraphBuilder gb = null;
            AMDvdRenderStatus drs;
            IDvdGraphBuilder idgb = null;
            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            object o;
            IBaseFilter pFilter;

            // Get the IDvdGraphBuilder interface
            idgb = (IDvdGraphBuilder)new DvdGraphBuilder();

            hr = idgb.RenderDvdVideoVolume(MyDisk, AMDvdGraphFlags.HWDecPrefer, out drs);
            DsError.ThrowExceptionForHR(hr);

            // If there is no dvd in the player, you get hr == S_FALSE (1)
            Debug.Assert(hr == 0, "Can't find dvd");

            // Get an IFilterGraph interface
            hr = idgb.GetFiltergraph(out gb);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.SetFiltergraph(gb);
            DsError.ThrowExceptionForHR(hr);

            hr = gb.FindFilterByName("DVD Navigator", out pFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.FindInterface(null, null, pFilter, typeof(IAMDecoderCaps).GUID, out o);
            DsError.ThrowExceptionForHR(hr);

            m_idc = (IAMDecoderCaps)o;
        }
    }
}
