using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IRenderEngine2Test
    {
        private IRenderEngine2 m_ire2;

        public IRenderEngine2Test()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestSetResizer();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ire2);
            }
        }

        private void TestSetResizer()
        {
            int hr;

            // Use the default DES resizer
            hr = m_ire2.SetResizerGUID(new Guid(0xF97B8A60, 0x31AD, 0x11CF, 0xB2, 0xDE, 0x00, 0xDD, 0x01, 0x10, 0x1B, 0x85));
            DESError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            m_ire2 = (IRenderEngine2)new RenderEngine();
        }
    }
}
