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

            IMediaSampleTest t6 = new IMediaSampleTest();
            t6.DoTests();
 
        }
	}
}
