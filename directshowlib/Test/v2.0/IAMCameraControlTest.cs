using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;

namespace v2_0
{
    class IAMCameraControlTest
    {
        private IAMCameraControl m_icc;

        public void DoTests()
        {
            DoSetup();

            TestRange();
            TestGetSet();

            Marshal.ReleaseComObject(m_icc);
        }

        private void TestGetSet()
        {
            int hr;
            int iValue, iOldValue;
            CameraControlFlags iFlags, iOldFlags;

            hr = m_icc.Get(CameraControlProperty.Pan, out iOldValue, out iOldFlags);
            DsError.ThrowExceptionForHR(hr);

            hr = m_icc.Set(CameraControlProperty.Pan, 13, CameraControlFlags.Manual);
            DsError.ThrowExceptionForHR(hr);

            hr = m_icc.Get(CameraControlProperty.Pan, out iValue, out iFlags);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iValue == 13, "Get/Set");
            Debug.Assert(iFlags == CameraControlFlags.Manual, "Get/Set flags");

            hr = m_icc.Set(CameraControlProperty.Pan, iOldValue, iOldFlags);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestRange()
        {
            int hr;
            int pMin, pMax, pSteppingDelta, pDefault;
            CameraControlFlags pFlags;

            hr = m_icc.GetRange(CameraControlProperty.Pan, out pMin, out pMax, out pSteppingDelta, out pDefault, out pFlags);
            DsError.ThrowExceptionForHR(hr);

            // These values appropriate for Logitech QuickCam Pro 4000
            Debug.Assert(pMin == -100, "Min");
            Debug.Assert(pMax == 100, "Max");
            Debug.Assert(pSteppingDelta == 1, "Step");
            Debug.Assert(pDefault == 0, "Default");
            Debug.Assert((int)pFlags == ((int)(CameraControlFlags.Auto)|(int)(CameraControlFlags.Manual)), "Flags");
        }

        private void DoSetup()
        {
            m_icc = null;

            // Get the IAMCameraControl
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            // Get the graphbuilder object
            IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;
            // add the video input device
            IBaseFilter capFilter = null;
            int hr = graphBuilder.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out capFilter);
            DsError.ThrowExceptionForHR(hr);

            m_icc = capFilter as IAMCameraControl;

            Debug.Assert(m_icc != null);
        }
    }
}
