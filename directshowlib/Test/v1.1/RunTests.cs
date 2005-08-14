using System;
using System.Windows.Forms;

namespace DirectShowLib.Test
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class Class1
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //IStreamBufferInitializeTest dw01 = new IStreamBufferInitializeTest();
            //dw01.DoTests();

            //IStreamBufferConfigTest dw02 = new IStreamBufferConfigTest();
            //dw02.DoTests();

            //IStreamBufferConfigTest2 dw03 = new IStreamBufferConfigTest2();
            //dw03.DoTests();

            //IStreamBufferDataCountersTest dw04 = new IStreamBufferDataCountersTest();
            //dw04.DoTests();

            //IStreamBufferSinkTest dw05 = new IStreamBufferSinkTest();
            //dw05.DoTests();

            //IStreamBufferSink2Test dw06 = new IStreamBufferSink2Test();
            //dw06.DoTests();

            //IStreamBufferSink3Test dw07 = new IStreamBufferSink3Test();
            //dw07.DoTests();

            //IStreamBufferRecordingAttributeTest dw08 = new IStreamBufferRecordingAttributeTest();
            //dw08.DoTests();

            //IEnumStreamBufferRecordingAttribTest dw09 = new IEnumStreamBufferRecordingAttribTest();
            //dw09.DoTests();

            //IStreamBufferRecordControlTest dw10 = new IStreamBufferRecordControlTest();
            //dw10.DoTests();

            //IStreamBufferRecCompTest dw11 = new IStreamBufferRecCompTest();
            //dw11.DoTests();

            //IStreamBufferSourceTest dw12 = new IStreamBufferSourceTest();
            //dw12.DoTests();

            //IStreamBufferMediaSeekingTest dw13 = new IStreamBufferMediaSeekingTest();
            //dw13.DoTests();

            //IStreamBufferMediaSeeking2Test dw14 = new IStreamBufferMediaSeeking2Test();
            //dw14.DoTests();

            //IAMGraphStreamsTest dw15 = new IAMGraphStreamsTest();
            //dw15.DoTests();

            //IQualPropTest dw16 = new IQualPropTest();
            //dw16.DoTests();

            //IAMVfwCaptureDialogsTest dw17 = new IAMVfwCaptureDialogsTest();
            //dw17.DoTests();

            //IAMMediaContentTest dw18 = new IAMMediaContentTest();
            //dw18.DoTests();

            //IAMMediaContent2Test dw19 = new IAMMediaContent2Test();
            //dw19.DoTests();

            //IConfigAsfWriterTest dw20 = new IConfigAsfWriterTest();
            //dw20.DoTests();

            IMixerPinConfigTest dw21 = new IMixerPinConfigTest();
            dw21.DoTests();

            //IDMOWrapperFilterTest en01 = new IDMOWrapperFilterTest();
            //en01.DoTests();

            //IDVEncTest en02 = new IDVEncTest();
            //en02.DoTests();

            //IVMRAspectRatioControlTest en03 = new IVMRAspectRatioControlTest();
            //en03.DoTests();

            //IVMRAspectRatioControl9Test en04 = new IVMRAspectRatioControl9Test();
            //en04.DoTests();

            //IVMRMixerControl9Test en05 = new IVMRMixerControl9Test();
            //en05.DoTests();

            //IVMRMixerControlTest en06 = new IVMRMixerControlTest();
            //en06.DoTests();

            //IVMRMonitorConfigTest en07 = new IVMRMonitorConfigTest();
            //en07.DoTests();

            //IVMRMonitorConfig9Test en08 = new IVMRMonitorConfig9Test();
            //en08.DoTests();

            //IVMRVideoStreamControlTest en09 = new IVMRVideoStreamControlTest();
            //en09.DoTests();

            //IVMRVideoStreamControl9Test en10 = new IVMRVideoStreamControl9Test();
            //en10.DoTests();

            //IVMRMixerBitmapTest en11 = new IVMRMixerBitmapTest();
            //en11.DoTests();

            //IVMRMixerBitmap9Test en12 = new IVMRMixerBitmap9Test();
            //en12.DoTests();

            //IVMRDeinterlaceControlTest en13 = new IVMRDeinterlaceControlTest();
            //en13.DoTests();

            //IVMRDeinterlaceControl9Test en14 = new IVMRDeinterlaceControl9Test();
            //en14.DoTests();

            //IVMRImagePresenterConfigTest en15 = new IVMRImagePresenterConfigTest();
            //en15.DoTests();

            //IVMRImagePresenterConfig9Test en16 = new IVMRImagePresenterConfig9Test();
            //en16.DoTests();

            //IVMRSurfaceAllocatorNotify9Test en17 = new IVMRSurfaceAllocatorNotify9Test();
            //en17.DoTests();

            //IVMRImageCompositor9Test en18 = new IVMRImageCompositor9Test();
            //en18.DoTests();

            //IVMRImageCompositorTest en19 = new IVMRImageCompositorTest();
            //en19.DoTests();

            //IVMRSurfaceAllocator9Test en20 = new IVMRSurfaceAllocator9Test();
            //en20.DoTests();

            IVMRSurfaceAllocatorEx9Test en21 = new IVMRSurfaceAllocatorEx9Test();
            en21.DoTests();
        }
    }
}
