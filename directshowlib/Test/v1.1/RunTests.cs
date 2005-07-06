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
            IDMOWrapperFilterTest en01 = new IDMOWrapperFilterTest();
            en01.DoTests();
        }
    }
}
