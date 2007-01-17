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
            TestSave();

            Marshal.ReleaseComObject(m_ica);
        }

        private void TestSave()
        {
            int hr;
            IStream uis = null;
            //long siz = 512;

            // Create the stream to write to
            hr = CreateStreamOnHGlobal(IntPtr.Zero, true, out uis);

            // false doesn't seem to work
            hr = m_ica.SetAllSettings(uis);
            DsError.ThrowExceptionForHR(hr);

        }

        private void TestIsSupported()
        {
            int hr;
            Guid g = new Guid("138130AF-A79B-45D5-B4AA-87697457BA87");
            Guid g2 = new Guid("374ac4df-7c98-4257-b13d-36087dbee458");

            hr = m_ica.IsSupported(g);
            hr = m_ica.IsSupported(g2);
            DsError.ThrowExceptionForHR(hr);
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
                    if (m_ica != null)
                    {
                        Debug.WriteLine(dev.Name);
                        break;
                    }
                }
                catch { }
            }
        }
    }
}
#if asdf
    public interface ICodecAPI
    {
        [PreserveSig]
        int IsSupported([In] Guid Api);

        [PreserveSig]
        int IsModifiable([In] Guid Api);

        [PreserveSig]
        int GetParameterRange(
            [In] Guid Api,
            [Out] out object ValueMin,
            [Out] out object ValueMax,
            [Out] out object SteppingDelta
            );

        [PreserveSig]
        int GetParameterValues(
            [In] Guid Api,
            [Out] out object[] Values,
            [Out] out int ValuesCount
            );

        [PreserveSig]
        int GetDefaultValue(
            [In] Guid Api,
            [Out] out object Value
            );

        [PreserveSig]
        int GetValue(
            [In] Guid Api,
            [Out] out object Value
            );

        [PreserveSig]
        int SetValue(
            [In] Guid Api,
            [In] object Value
            );

        [PreserveSig]
        int RegisterForEvent(
            [In] Guid Api,
            [In] IntPtr userData
            );

        [PreserveSig]
        int UnregisterForEvent([In] Guid Api);

        [PreserveSig]
        int SetAllDefaults();

        [PreserveSig]
        int SetValueWithNotify(
            [In] Guid Api,
            [In] object Value,
            [Out] out Guid[] ChangedParam,
            [Out] int ChangedParamCount
            );

        [PreserveSig]
        int SetAllDefaultsWithNotify(
            [Out] out Guid[] ChangedParam,
            [Out] out int ChangedParamCount
            );

        [PreserveSig]
#if USING_NET11
        int GetAllSettings([In] UCOMIStream pStream);
#else
        int GetAllSettings([In] IStream pStream);
#endif

        [PreserveSig]
#if USING_NET11
        int SetAllSettings([In] UCOMIStream pStream);
#else
        int SetAllSettings([In] IStream pStream);
#endif

        [PreserveSig]
        int SetAllSettingsWithNotify(
#if USING_NET11
            [In] UCOMIStream pStream,
#else
            [In] IStream pStream,
#endif
            [Out] out Guid[] ChangedParam,
            [Out] out int ChangedParamCount
            );
#endif