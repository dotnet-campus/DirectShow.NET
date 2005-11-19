using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IVPManagerTest
    {
        IVPManager m_vpm;

        public IVPManagerTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestIndex();
            }
            finally
            {
                Marshal.ReleaseComObject(m_vpm);
            }
        }

        private void TestIndex()
        {
            int hr;
            int i;

            // Must have multiple monitors (and video cards?)
            // to use a value other than zero
            hr = m_vpm.SetVideoPortIndex(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_vpm.GetVideoPortIndex(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 0, "VideoPortIndex");
        }

        private void Configure()
        {
            m_vpm = (IVPManager)new VideoPortManager();
        }
    }
}
