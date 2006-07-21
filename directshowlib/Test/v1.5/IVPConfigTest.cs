using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.MultimediaStreaming;

namespace v1._5
{
    public class IVPConfigTest
    {
        IVPConfig m_ivc;

        public void DoTests()
        {
            Setup();

            TestDeci();
            TestScale();
        }

        private void TestDeci()
        {
            int hr;
            bool b;

            hr = m_ivc.IsVPDecimationAllowed(out b);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestScale()
        {
            int hr;
            AMVPSize s = new AMVPSize();
            s.dwHeight = 1;
            s.dwWidth = 9;

            hr = m_ivc.SetScalingFactors(ref s);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Setup()
        {
            IPin pPin = null;
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.LegacyAmFilterCategory);

            foreach (DsDevice d in devs)
            {
                if (d.Name == "InterVideo NonCSS Video Decoder for Hauppauge")
                {
                    Guid iid = typeof(IBaseFilter).GUID;
                    object o;
                    d.Mon.BindToObject(null, null, ref iid, out o);
                    IBaseFilter ibf = o as IBaseFilter;
                    pPin = DsFindPin.ByDirection(ibf, PinDirection.Output, 0);
                    break;
                }
            }

            if (pPin != null)
            {
                m_ivc = pPin as IVPConfig;
            }
        }
    }
}
