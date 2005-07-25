using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IStreamBufferSinkTest
    {
        #region API

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int RegCreateKeyEx(
            IntPtr hKey, 
            string lpSubKey, 
            int Reserved, 
            string lpClass, 
            RegOption dwOptions, 
            RegSAM samDesired, 
            SecurityAttributes lpSecurityAttributes, 
            out IntPtr phkResult, 
            out RegResult lpdwDisposition);

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int RegCloseKey(
            IntPtr hKey
            );

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int RegGetKeySecurity(
            IntPtr hKey,
            SecurityInformation si,
            IntPtr psid,
            ref int iSize
            );

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int LookupAccountName(
            string lpSystemName,
            string lpAccountName,
            IntPtr Sid,
            ref int cbSid,
            IntPtr ReferencedDomainName,
            ref int cchReferencedDomainName,
            out int peUse
            );

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int GetLengthSid(
            IntPtr pSid
            );

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int GetSidSubAuthorityCount(
            IntPtr pSid
            );

        [DllImport("advapi32.dll", SetLastError=true)]
        static extern int IsValidSid(
            IntPtr pSid
            );

        [StructLayout(LayoutKind.Sequential)]
            public class SecurityAttributes
        {
            public int nLength = 0;
            public IntPtr lpSecurityDescriptor = IntPtr.Zero;
            public bool bInheritHandle = false;
        }

        [Flags]
            public enum SecurityInformation
        {
            None = 0,
            OwnerSecurityInformation =       0x00000001,
            GroupSecurityInformation =       0x00000002,
            DACLSecurityInformation =        0x00000004,
            SACLSecurityInformation =       0x00000008,

            ProtectedDACLSecurityInformation =      unchecked((int)0x80000000),
            ProtectedSACLSecurityInformation =      0x40000000,
            UnprotectedDACLSecurityInformation =    0x20000000,
            UnprotectedSACLSecurityInformation =  0x10000000
        }

        [Flags]
            public enum RegSAM
        {
            QueryValue =        (0x0001),
            SetValue =          (0x0002),
            CreateSubKey =      (0x0004),
            EnumerateSubKeys =  (0x0008),
            Notify =            (0x0010),
            CreateLink =        (0x0020),
            WOW64_32Key =       (0x0200),
            WOW64_64Key =       (0x0100),
            WOW64_Res =         (0x0300),
            Read =              (0x00020019),
            Write =             (0x00020006),
            Execute =           (0x00020019),
            AllAccess =         (0x000f003f)
        }

        public enum HKEY
        {
            ClassesRoot           = unchecked((int)0x80000000),
            CurrentUser           = unchecked((int)0x80000001),
            LocalMachine          = unchecked((int)0x80000002),
            Users                 = unchecked((int)0x80000003),
            PerformanceData       = unchecked((int)0x80000004),
            PerformanceText       = unchecked((int)0x80000050),
            PerformanceNlsText    = unchecked((int)0x80000060),
            CurrentConfig         = unchecked((int)0x80000005),
            DynData               = unchecked((int)0x80000006)
        }


        [Flags]
            public enum RegOption
        {
            NonVolatile = 0,
            Volatile = 1,
            CreateLink = 2,
            BackupRestore = 4,
            OpenLink = 8
        }

        public enum RegResult
        {
            CreatedNewKey = 0x00000001,
            OpenedExistingKey = 0x00000002
        }
        #endregion

        IStreamBufferSink m_isbc;
        IFilterGraph2 m_FilterGraph;

        public IStreamBufferSinkTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestLock();
                TestRecorder();
            }
            finally
            {
                Marshal.ReleaseComObject(m_isbc);
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestLock()
        {
            int hr;

            hr = m_isbc.IsProfileLocked();
            Debug.Assert(hr == 1, "Locked 1");

            hr = m_isbc.LockProfile("delme.prf");
            DsError.ThrowExceptionForHR(hr);

            hr = m_isbc.IsProfileLocked();
            Debug.Assert(hr == 0, "Locked 2");
        }

        private void TestRecorder()
        {
            const string FileName = "delme.out";
            int hr;
            object o;
            short c;

            File.Delete(FileName);

            hr = m_isbc.CreateRecorder("delme.out", RecordingType.Content, out o);
            DsError.ThrowExceptionForHR(hr);

            // Make sure we really got a recorder object
            IStreamBufferRecordingAttribute i = o as IStreamBufferRecordingAttribute;
            hr = i.GetAttributeCount(0, out c);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(c != 0, "CreateRecorder");
        }

        private void Configure()
        {
            // In order to lock a profile, you have to have at least one stream
            // connected to the sink. I connect a video thru the DVVideoEnc into 
            // the StreamBufferSink.
            int hr;
            IBaseFilter pFilter;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            StreamBufferSink sbk = new StreamBufferSink();
            m_FilterGraph = (IFilterGraph2)new FilterGraph();
            DsROTEntry  ds = new DsROTEntry(m_FilterGraph);

            hr = icgb.SetFiltergraph(m_FilterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = m_FilterGraph.AddFilter((IBaseFilter) sbk, "StreamBufferSink");
            DsError.ThrowExceptionForHR(hr);

            DsDevice [] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = m_FilterGraph.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out pFilter);
            DsError.ThrowExceptionForHR(hr);

            DVVideoEnc dve = new DVVideoEnc();
            IBaseFilter ibfVideoEnc = (IBaseFilter) dve;
            hr = m_FilterGraph.AddFilter( ibfVideoEnc, "dvenc" );
            DsError.ThrowExceptionForHR( hr );

            hr = icgb.RenderStream(null, null, pFilter, ibfVideoEnc, (IBaseFilter)sbk);
            DsError.ThrowExceptionForHR(hr);

            Marshal.ReleaseComObject(pFilter);
            Marshal.ReleaseComObject(icgb);

            m_isbc = (IStreamBufferSink)sbk;
        }
    }
}
