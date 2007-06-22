#if ALLOW_UNTESTED_INTERFACES

// Can't test GetParameterRange

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib;

namespace v2_0
{
    class ICodecAPITest
    {
        [DllImport("OLE32.DLL")]
        extern private static int CreateStreamOnHGlobal(
          IntPtr hGlobalMemHandle,
          bool fDeleteOnRelease,
          out IStream pOutStm);

        private ICodecAPI m_ica;

        public void DoTests()
        {
            DoSetup();
            TestIsSupported();
            TestIsModifiable();

            DoSetup2();
            TestGetParameterRange();
            TestSave();

            Marshal.ReleaseComObject(m_ica);
        }

        private void TestGetParameterRange()
        {
            int hr;
            object p2 = 1; // new object();
            object p3 = 2; // new object();
            object p4 = 3; // new object();

            //IntPtr ip2 = Marshal.AllocCoTaskMem(20);
            //IntPtr ip3 = Marshal.AllocCoTaskMem(20);
            //IntPtr ip4 = Marshal.AllocCoTaskMem(20);

            //IntPtr ip5 = Marshal.AllocCoTaskMem(20);
            //IntPtr ip6 = Marshal.AllocCoTaskMem(20);
            //IntPtr ip7 = Marshal.AllocCoTaskMem(20);

            //Marshal.WriteInt64(ip5, 0);
            //Marshal.WriteInt64(ip6, 0);
            //Marshal.WriteInt64(ip7, 0);

            //Marshal.WriteIntPtr(ip2, ip5);
            //Marshal.WriteIntPtr(ip3, ip6);
            //Marshal.WriteIntPtr(ip4, ip7);
            //long l2 = Marshal.ReadInt64(ip2);
            //long l3 = Marshal.ReadInt64(ip3);
            //long l4 = Marshal.ReadInt64(ip4);

            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_AVDecMmcssClass, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_AllSettings, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_AllSettings, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_AudioEncoder, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_ChangeLists, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_CurrentChangeList, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_SetAllDefaults, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_SupportsEvents, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.CODECAPI_VideoEncoder, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.DroppedFrames, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.ENCAPIPARAM_BitRate, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.ENCAPIPARAM_BitRateMode, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.ENCAPIPARAM_PeakBitRate, out p2, out p3, out p4);
            hr = m_ica.GetParameterRange(PropSetID.ENCAPIPARAM_SAP_MODE, out p2, out p3, out p4);

            //l2 = Marshal.ReadInt64(ip2);
            //l3 = Marshal.ReadInt64(ip3);
            //l4 = Marshal.ReadInt64(ip4);

            //l2 = Marshal.ReadInt64(ip5);
            //l3 = Marshal.ReadInt64(ip6);
            //l4 = Marshal.ReadInt64(ip7);

            DsError.ThrowExceptionForHR(hr);
        }

        private void TestSave()
        {
            int hr;
            IStream uis = null;
            //long siz = 512;

            // Create the stream to write to
            hr = CreateStreamOnHGlobal(IntPtr.Zero, true, out uis);

            // false doesn't seem to work
            hr = m_ica.GetAllSettings(uis);
            if (hr >= 0) 
            { 
                Debug.WriteLine("TestSave"); 
            }
        }

        private void TestIsSupported()
        {
            int hr;
            Guid g = new Guid();

            hr = m_ica.IsSupported(g);
            Debug.Assert(hr == unchecked((int)0x80004001), "IsSupported");

            hr = m_ica.IsSupported(PropSetID.CODECAPI_AVDecMmcssClass);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestIsModifiable()
        {
            int hr;
            Guid g = new Guid();

            hr = m_ica.IsModifiable(g);
            Debug.Assert(hr == 1, "TestIsModifiable");

            hr = m_ica.IsModifiable(PropSetID.CODECAPI_AVDecMmcssClass);
            Debug.Assert(hr == 0, "TestIsModifiable");
        }

        private void DoSetup2()
        {
            Guid g = new Guid("{42150CD9-CA9A-4EA5-9939-30EE037F6E74}");
            Type type = Type.GetTypeFromCLSID(g);
            m_ica = Activator.CreateInstance(type) as ICodecAPI;
        }

        private void DoSetup()
        {
            DsDevice [] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.LegacyAmFilterCategory);

            foreach (DsDevice dev in capDevices)
            {
                string s;

                dev.Mon.GetDisplayName(null, null, out s);

                try
                {
                    m_ica = Marshal.BindToMoniker(s) as ICodecAPI;
                    if (dev.Name == "Microsoft MPEG-1/DD Audio Decoder")
                    {
                        break;
                    }
                }
                catch { }
            }
        }
    }
}
#if asdf
        int GetParameterRange(
            [In] Guid Api,
            [Out] out object ValueMin,
            [Out] out object ValueMax,
            [Out] out object SteppingDelta
            );

        int GetParameterValues(
            [In] Guid Api,
            [Out] out object[] Values,
            [Out] out int ValuesCount
            );

        int GetDefaultValue(
            [In] Guid Api,
            [Out] out object Value
            );

        int GetValue(
            [In] Guid Api,
            [Out] out object Value
            );

        int SetValue(
            [In] Guid Api,
            [In] object Value
            );

        int RegisterForEvent(
            [In] Guid Api,
            [In] IntPtr userData
            );

        int UnregisterForEvent([In] Guid Api);

        int SetAllDefaults();

        int SetValueWithNotify(
            [In] Guid Api,
            [In] object Value,
            [Out] out Guid[] ChangedParam,
            [Out] int ChangedParamCount
            );

        int SetAllDefaultsWithNotify(
            [Out] out Guid[] ChangedParam,
            [Out] out int ChangedParamCount
            );

        int GetAllSettings([In] IStream pStream);

        int SetAllSettings([In] IStream pStream);

            [In] IStream pStream,
            [Out] out Guid[] ChangedParam,
            [Out] out int ChangedParamCount
            );
#endif
#endif
