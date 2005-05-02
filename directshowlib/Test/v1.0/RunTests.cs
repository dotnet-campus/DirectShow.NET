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
            IAMAudioInputMixerTest t3 = new IAMAudioInputMixerTest();
            t3.DoTests();

            IVideoWindowTest t1 = new IVideoWindowTest();
            t1.DoTests();

            IMediaSeekingTest t2 = new IMediaSeekingTest();
            t2.DoTests();
        }
	}
}
