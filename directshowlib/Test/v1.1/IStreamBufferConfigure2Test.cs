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
    public class IStreamBufferConfigTest2
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

        IStreamBufferConfigure2 m_isbc;

        public IStreamBufferConfigTest2()
        {
        }

        public void DoTests()
        {
            m_isbc = GetConfigure();

            try
            {
                TestMultiplexedPacketSize();
                TestFFTransitionRates();
            }
            finally
            {
                Marshal.ReleaseComObject(m_isbc);
            }
        }

        private void TestMultiplexedPacketSize()
        {
            int hr;
            int iSize;
            int iSize2;

            hr = m_isbc.GetMultiplexedPacketSize(out iSize);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.SetMultiplexedPacketSize(32767);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.GetMultiplexedPacketSize(out iSize2);
            DsError.ThrowExceptionForHR( hr );

            Debug.Assert(iSize2 == 32767, "SetMultiplexedPacketSize");

            hr = m_isbc.SetMultiplexedPacketSize(iSize);
            DsError.ThrowExceptionForHR( hr );
        }

        private void TestFFTransitionRates()
        {
            int hr;
            int iMin, iMax;
            int iMin2, iMax2;

            hr = m_isbc.GetFFTransitionRates(out iMin, out iMax);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.SetFFTransitionRates(7, 8);
            DsError.ThrowExceptionForHR( hr );

            hr = m_isbc.GetFFTransitionRates(out iMin2, out iMax2);
            DsError.ThrowExceptionForHR( hr );

            Debug.Assert(iMin2 == 7, "set min");
            Debug.Assert(iMax2 == 8, "set max");

            // According to the docs, these are the defaults.
            hr = m_isbc.SetFFTransitionRates(4, 6);
            DsError.ThrowExceptionForHR( hr );
        }


        private IStreamBufferConfigure2 GetConfigure()
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

            return (IStreamBufferConfigure2) sbc;
        }
    }
}
