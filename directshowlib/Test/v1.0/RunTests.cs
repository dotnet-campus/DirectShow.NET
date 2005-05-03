using System;

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

            IVideoWindowTest t1 = new IVideoWindowTest();
            t1.DoTests();

            IMediaSeekingTest t2 = new IMediaSeekingTest();
            t2.DoTests();
        }
	}
}
