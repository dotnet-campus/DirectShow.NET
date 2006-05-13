using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;
using DirectShowLib.BDA;
using DirectShowLib.DES;
using DirectShowLib.Dvd;
using DirectShowLib.DMO;
using DirectShowLib.MultimediaStreaming;
using DirectShowLib.SBE;

namespace v1._5
{
    class Class1
    {
        [STAThread]
        static void Main(string[] args)
        {
#if false
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            Guid g = typeof(IBaseFilter).GUID;
            object o;
            devs[0].Mon.BindToObject(null, null, ref g, out o);
            IBaseFilter ibf = o as IBaseFilter;
            IPin pPin = DsFindPin.ByDirection(ibf, PinDirection.Output, 0);
            IEnumMediaTypes pEnum;
            int i = 0;
            AMMediaType[] mt = new AMMediaType[41];
            pPin.EnumMediaTypes(out pEnum);
            while (pEnum.Next(mt.Length, mt, out i) >= 0 && i > 0)
            {
                for (int x = 0; x < i; x++)
                {
                    Debug.WriteLine(DsToString.AMMediaTypeToString(mt[0]));
                    DsUtils.FreeAMMediaType(mt[0]);
                }
            }

            Debug.WriteLine("PinInfo\t" + Marshal.SizeOf(typeof(PinInfo)).ToString());
            Debug.WriteLine("AMMediaType\t" + Marshal.SizeOf(typeof(AMMediaType)).ToString());
            Debug.WriteLine("FilterInfo\t" + Marshal.SizeOf(typeof(FilterInfo)).ToString());
            Debug.WriteLine("AllocatorProperties\t" + Marshal.SizeOf(typeof(AllocatorProperties)).ToString());
            Debug.WriteLine("AMSample2Properties\t" + Marshal.SizeOf(typeof(AMSample2Properties)).ToString());
            Debug.WriteLine("AMStreamInfo\t" + Marshal.SizeOf(typeof(AMStreamInfo)).ToString());
            Debug.WriteLine("ColorKey\t" + Marshal.SizeOf(typeof(ColorKey)).ToString());
            Debug.WriteLine("RegPinMedium\t" + Marshal.SizeOf(typeof(RegPinMedium)).ToString());
            Debug.WriteLine("DVInfo\t" + Marshal.SizeOf(typeof(DVInfo)).ToString());
            Debug.WriteLine("VideoStreamConfigCaps\t" + Marshal.SizeOf(typeof(VideoStreamConfigCaps)).ToString());
            Debug.WriteLine("AudioStreamConfigCaps\t" + Marshal.SizeOf(typeof(AudioStreamConfigCaps)).ToString());
            Debug.WriteLine("BDANodeDescriptor\t" + Marshal.SizeOf(typeof(BDANodeDescriptor)).ToString());
            Debug.WriteLine("BDATemplateConnection\t" + Marshal.SizeOf(typeof(BDATemplateConnection)).ToString());
            Debug.WriteLine("SCompFmt0\t" + Marshal.SizeOf(typeof(SCompFmt0)).ToString());
            Debug.WriteLine("DexterParam\t" + Marshal.SizeOf(typeof(DexterParam)).ToString());
            Debug.WriteLine("DexterValue\t" + Marshal.SizeOf(typeof(DexterValue)).ToString());
            Debug.WriteLine("BitmapInfo\t" + Marshal.SizeOf(typeof(BitmapInfo)).ToString());
            Debug.WriteLine("BitmapInfoHeader\t" + Marshal.SizeOf(typeof(BitmapInfoHeader)).ToString());
            Debug.WriteLine("DDPixelFormat\t" + Marshal.SizeOf(typeof(DDPixelFormat)).ToString());
            Debug.WriteLine("NormalizedRect\t" + Marshal.SizeOf(typeof(NormalizedRect)).ToString());
            Debug.WriteLine("GPRMArray\t" + Marshal.SizeOf(typeof(GPRMArray)).ToString());
            Debug.WriteLine("SPRMArray\t" + Marshal.SizeOf(typeof(SPRMArray)).ToString());
            Debug.WriteLine("DvdHMSFTimeCode\t" + Marshal.SizeOf(typeof(DvdHMSFTimeCode)).ToString());
            Debug.WriteLine("DvdPlaybackLocation2\t" + Marshal.SizeOf(typeof(DvdPlaybackLocation2)).ToString());
            Debug.WriteLine("DvdAudioAttributes\t" + Marshal.SizeOf(typeof(DvdAudioAttributes)).ToString());
            Debug.WriteLine("DvdMUAMixingInfo\t" + Marshal.SizeOf(typeof(DvdMUAMixingInfo)).ToString());
            Debug.WriteLine("DvdMUACoeff\t" + Marshal.SizeOf(typeof(DvdMUACoeff)).ToString());
            Debug.WriteLine("DvdMultichannelAudioAttributes\t" + Marshal.SizeOf(typeof(DvdMultichannelAudioAttributes)).ToString());
            Debug.WriteLine("DvdKaraokeAttributes\t" + Marshal.SizeOf(typeof(DvdKaraokeAttributes)).ToString());
            Debug.WriteLine("DvdVideoAttributes\t" + Marshal.SizeOf(typeof(DvdVideoAttributes)).ToString());
            Debug.WriteLine("DvdSubpictureAttributes\t" + Marshal.SizeOf(typeof(DvdSubpictureAttributes)).ToString());
            Debug.WriteLine("DvdTitleAttributes\t" + Marshal.SizeOf(typeof(DvdTitleAttributes)).ToString());
            Debug.WriteLine("DvdMenuAttributes\t" + Marshal.SizeOf(typeof(DvdMenuAttributes)).ToString());
            Debug.WriteLine("DvdDecoderCaps\t" + Marshal.SizeOf(typeof(DvdDecoderCaps)).ToString());
            Debug.WriteLine("AMDvdRenderStatus\t" + Marshal.SizeOf(typeof(AMDvdRenderStatus)).ToString());
            Debug.WriteLine("DMOPartialMediatype\t" + Marshal.SizeOf(typeof(DMOPartialMediatype)).ToString());
            Debug.WriteLine("DMOOutputDataBuffer\t" + Marshal.SizeOf(typeof(DMOOutputDataBuffer)).ToString());
            Debug.WriteLine("MPData\t" + Marshal.SizeOf(typeof(MPData)).ToString());
            Debug.WriteLine("MPEnvelopeSegment\t" + Marshal.SizeOf(typeof(MPEnvelopeSegment)).ToString());
            Debug.WriteLine("ParamInfo\t" + Marshal.SizeOf(typeof(ParamInfo)).ToString());
            Debug.WriteLine("KSMultipleItem\t" + Marshal.SizeOf(typeof(KSMultipleItem)).ToString());
            Debug.WriteLine("DDColorControl\t" + Marshal.SizeOf(typeof(DDColorControl)).ToString());
            Debug.WriteLine("MPEGPacketList\t" + Marshal.SizeOf(typeof(MPEGPacketList)).ToString());
            Debug.WriteLine("DSMCCFilterOptions\t" + Marshal.SizeOf(typeof(DSMCCFilterOptions)).ToString());
            Debug.WriteLine("ATSCFilterOptions\t" + Marshal.SizeOf(typeof(ATSCFilterOptions)).ToString());
            Debug.WriteLine("MPEG2Filter\t" + Marshal.SizeOf(typeof(MPEG2Filter)).ToString());
            Debug.WriteLine("MPEGContextUnion\t" + Marshal.SizeOf(typeof(MPEGContextUnion)).ToString());
            Debug.WriteLine("BCSDeMux\t" + Marshal.SizeOf(typeof(BCSDeMux)).ToString());
            Debug.WriteLine("MPEGWinSock\t" + Marshal.SizeOf(typeof(MPEGWinSock)).ToString());
            Debug.WriteLine("MPEGContext\t" + Marshal.SizeOf(typeof(MPEGContext)).ToString());
            Debug.WriteLine("MPEGStreamBuffer\t" + Marshal.SizeOf(typeof(MPEGStreamBuffer)).ToString());
            Debug.WriteLine("WSTPage\t" + Marshal.SizeOf(typeof(WSTPage)).ToString());
            Debug.WriteLine("MPEG1WaveFormat\t" + Marshal.SizeOf(typeof(MPEG1WaveFormat)).ToString());
            Debug.WriteLine("VideoInfoHeader\t" + Marshal.SizeOf(typeof(VideoInfoHeader)).ToString());
            Debug.WriteLine("VideoInfoHeader2\t" + Marshal.SizeOf(typeof(VideoInfoHeader2)).ToString());
            Debug.WriteLine("WaveFormatEx\t" + Marshal.SizeOf(typeof(WaveFormatEx)).ToString());
            Debug.WriteLine("StreamBufferAttribute\t" + Marshal.SizeOf(typeof(StreamBufferAttribute)).ToString());
            Debug.WriteLine("SBEPinData\t" + Marshal.SizeOf(typeof(SBEPinData)).ToString());
            Debug.WriteLine("VMR9PresentationInfo\t" + Marshal.SizeOf(typeof(VMR9PresentationInfo)).ToString());
            Debug.WriteLine("VMR9AllocationInfo\t" + Marshal.SizeOf(typeof(VMR9AllocationInfo)).ToString());
            Debug.WriteLine("VMR9ProcAmpControl\t" + Marshal.SizeOf(typeof(VMR9ProcAmpControl)).ToString());
            Debug.WriteLine("VMR9MonitorInfo\t" + Marshal.SizeOf(typeof(VMR9MonitorInfo)).ToString());
            Debug.WriteLine("VMR9DeinterlaceCaps\t" + Marshal.SizeOf(typeof(VMR9DeinterlaceCaps)).ToString());
            Debug.WriteLine("VMR9VideoStreamInfo\t" + Marshal.SizeOf(typeof(VMR9VideoStreamInfo)).ToString());
            Debug.WriteLine("VMR9VideoDesc\t" + Marshal.SizeOf(typeof(VMR9VideoDesc)).ToString());
            Debug.WriteLine("VMR9Frequency\t" + Marshal.SizeOf(typeof(VMR9Frequency)).ToString());
            Debug.WriteLine("VMR9AlphaBitmap\t" + Marshal.SizeOf(typeof(VMR9AlphaBitmap)).ToString());
            Debug.WriteLine("VMR9ProcAmpControlRange\t" + Marshal.SizeOf(typeof(VMR9ProcAmpControlRange)).ToString());
            Debug.WriteLine("VMRAlphaBitmap\t" + Marshal.SizeOf(typeof(VMRAlphaBitmap)).ToString());
            Debug.WriteLine("VMRDeinterlaceCaps\t" + Marshal.SizeOf(typeof(VMRDeinterlaceCaps)).ToString());
            Debug.WriteLine("VMRFrequency\t" + Marshal.SizeOf(typeof(VMRFrequency)).ToString());
            Debug.WriteLine("VMRVideoDesc\t" + Marshal.SizeOf(typeof(VMRVideoDesc)).ToString());
            Debug.WriteLine("VMRVideoStreamInfo\t" + Marshal.SizeOf(typeof(VMRVideoStreamInfo)).ToString());
            Debug.WriteLine("DDColorKey\t" + Marshal.SizeOf(typeof(DDColorKey)).ToString());
            Debug.WriteLine("VMRMonitorInfo\t" + Marshal.SizeOf(typeof(VMRMonitorInfo)).ToString());
            Debug.WriteLine("VMRGuid\t" + Marshal.SizeOf(typeof(VMRGuid)).ToString());
#endif
            // IAMTimecodeReaderTest t1 = new IAMTimecodeReaderTest();
            // t1.DoTests();

            // IAMBufferNegotiationTest t2 = new IAMBufferNegotiationTest();
            // t2.DoTests();

            IObjectWithSiteTest t3 = new IObjectWithSiteTest();
            t3.DoTests();

            //IAMGraphBuilderCallbackTest t4 = new IAMGraphBuilderCallbackTest();
            //t4.DoTests();

            //IServiceProviderTest t5 = new IServiceProviderTest();
            //t5.DoTests();

            //IAMBufferNegotiationTest t6 = new IAMBufferNegotiationTest();
            //t6.DoTests();

            //IAMFilterGraphCallbackTest t7 = new IAMFilterGraphCallbackTest();
            //t7.DoTests();
        }
    }
}
