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
            // This code, while complete, won't run on all machines.  The problem
            // is that not all audio cards support all these capabilities.  As a
            // result, some (or all) of the calls return E_NOTIMPLEMENTED.
            // IAMAudioInputMixerTest t3 = new IAMAudioInputMixerTest();
            // t3.DoTests();

            //IVideoWindowTest t1 = new IVideoWindowTest();
            //t1.DoTests();

            //IMediaSeekingTest t2 = new IMediaSeekingTest();
            //t2.DoTests();

            //IVMRFilterConfig9Test t4 = new IVMRFilterConfig9Test();
            //t4.DoTests();

            //IVMRWindowlessControl9Test t5 = new IVMRWindowlessControl9Test();
            //t5.Show();
            //t5.DoTests();

            //IMediaSampleTest t6 = new IMediaSampleTest();
            //t6.DoTests();

            //IDvdGraphBuilderTest t7 = new IDvdGraphBuilderTest();
            //t7.DoTests(); 

            //ISampleGrabberCBTest t8 = new ISampleGrabberCBTest();
            //t8.DoTests();

            //IKsPinTest t9 = new IKsPinTest();
            //t9.DoTests();

            //      IVMRFilterConfigTest t10 = new IVMRFilterConfigTest();
            //      t10.DoTests();

            //      IVMRWindowlessControlTest t11 = new IVMRWindowlessControlTest();
            //      t11.DoTests();

            //IPersistTest t12 = new IPersistTest();
            //t12.DoTests();

            //IMediaFilterTest t13 = new IMediaFilterTest();
            //t13.DoTests();

            //IBaseFilterTest t14 = new IBaseFilterTest();
            //t14.DoTests();

            //IBasicAudioTest t15 = new IBasicAudioTest();
            //t15.DoTests();

            //IFilterGraphTest t16 = new IFilterGraphTest();
            //t16.DoTests();

            //IGraphBuilderTest t17 = new IGraphBuilderTest();
            //t17.DoTests();
  
            //IFilterGraph2Test t18 = new IFilterGraph2Test();
            //t18.DoTests();

            //ISpecifyPropertyPagesTest t19 = new ISpecifyPropertyPagesTest();
            //t19.DoTests();

            //IDvdInfo2Test t20 = new IDvdInfo2Test();
            //t20.DoTests();

            //IDvdControl2Test t21 = new IDvdControl2Test();
            //t21.DoTests();

            //IVideoFrameStepTest t22 = new IVideoFrameStepTest();
            //t22.DoTests();

            //IDvdCmdTest dpw23 = new IDvdCmdTest();
            //dpw23.DoTests();

            //IDvdStateTest dpw24 = new IDvdStateTest();
            //dpw24.DoTests();

            //IPersistStreamTest dpw25 = new IPersistStreamTest();
            //dpw25.DoTests();

            //ICreateDevEnumTest dpw26 = new ICreateDevEnumTest();
            //dpw26.DoTests();

            //IConfigAviMuxTest dpw27 = new IConfigAviMuxTest();
            //dpw27.DoTests();

            //IConfigInterleavingTest dpw28 = new IConfigInterleavingTest();
            //dpw28.DoTests();

            //IFileSinkFilterTest dpw29 = new IFileSinkFilterTest();
            //dpw29.DoTests();

            //IFileSinkFilter2Test dpw30 = new IFileSinkFilter2Test();
            //dpw30.DoTests();

            //IFileSourceFilterTest en01 = new IFileSourceFilterTest();
            //en01.DoTests();

            //IBasicVideoTest dpw31 = new IBasicVideoTest();
            //dpw31.DoTests();   

            //IBasicVideo2Test dpw32 = new IBasicVideo2Test();
            //dpw32.DoTests();   

            IKsPropertySetTest dpw33 = new IKsPropertySetTest();
            dpw33.DoTests();
        }
    }
}
