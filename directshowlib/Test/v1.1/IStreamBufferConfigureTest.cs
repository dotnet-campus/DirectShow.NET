using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    /// <summary>
    /// Summary description for IStreamBufferConfigTest.
    /// </summary>
    public class IStreamBufferConfigTest
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

        IStreamBufferConfigure m_isbc;

        public IStreamBufferConfigTest()
        {
        }

        public void DoTests()
        {
            m_isbc = GetConfigure();

            try
            {
                TestBackingFileCount();
                TestDirectory();
                TestDuration();
            }
            finally
            {
                Marshal.ReleaseComObject(m_isbc);
            }
        }

        private void TestBackingFileCount()
        {
            int hr;
            int iMin, iMax;
            int iMin2, iMax2;

            // Min=4, max=6
            hr = m_isbc.GetBackingFileCount(out iMin, out iMax);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.SetBackingFileCount(7, 9);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.GetBackingFileCount(out iMin2, out iMax2);
            DsError.ThrowExceptionForHR( hr );

            Debug.Assert(iMin2 == 7, "set min");
            Debug.Assert(iMax2 == 9, "set max");

            hr = m_isbc.SetBackingFileCount(iMin, iMax);
            DsError.ThrowExceptionForHR( hr );
        }

        private void TestDuration()
        {
            int hr;
            int secs, secs2;

            // 300 secs
            hr = m_isbc.GetBackingFileDuration(out secs);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.SetBackingFileDuration(123);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.GetBackingFileDuration(out secs2);
            DsError.ThrowExceptionForHR( hr );

            Debug.Assert(secs2 == 123, "Set duration");

            hr = m_isbc.SetBackingFileDuration(secs);
            DsError.ThrowExceptionForHR( hr );
        }

        private void TestDirectory()
        {
            int hr;
            string dn;
            string dn2;

            // Defaults to a %temp%, under which it creates a hidden directory named TempSBE
            hr = m_isbc.GetDirectory(out dn);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.SetDirectory(@"c:\");
            DsError.ThrowExceptionForHR( hr );
            
            hr = m_isbc.GetDirectory(out dn2);
            DsError.ThrowExceptionForHR( hr );

            Debug.Assert(dn2 == @"c:\", "Set directory");

            hr = m_isbc.SetDirectory(dn);
            DsError.ThrowExceptionForHR( hr );
        }

        private IStreamBufferConfigure GetConfigure()
        {
            int hr;
            RegResult dispo;
            IntPtr hkey;
            StreamBufferConfig sbc = new StreamBufferConfig();

            IStreamBufferInitialize isbi = (IStreamBufferInitialize) sbc;

            // Default registry key is HKEY_CURRENT_USER\Software\Microsoft\DVR (which
            // is what we are using here)
            int i = RegCreateKeyEx(
                (IntPtr)HKEY.CurrentUser, 
                @"Software\Microsoft", 
                0, 
                null, 
                0, 
                RegSAM.Read | RegSAM.Write, 
                null, 
                out hkey, 
                out dispo
                );

            if (i != 0)
            {
                throw new Exception("Error reading registry");
            }

            hr = isbi.SetHKEY(hkey);
            DsError.ThrowExceptionForHR( hr );

            return (IStreamBufferConfigure) sbc;
        }
    }
}
