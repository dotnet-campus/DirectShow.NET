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

            IVMRMixerBitmapTest en11 = new IVMRMixerBitmapTest();
            en11.DoTests();

            IVMRMixerBitmap9Test en12 = new IVMRMixerBitmap9Test();
            en12.DoTests();
        }
    }
}
