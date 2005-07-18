// Changed SetSIDs to use intptr array

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
    public class IStreamBufferInitializeTest
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

        IStreamBufferInitialize m_isbi;

        public IStreamBufferInitializeTest()
        {
        }

        public void DoTests()
        {
            m_isbi = (IStreamBufferInitialize) new StreamBufferConfig();

            try
            {
                TestSetHKEY();
                TestSetSIDs();
            }
            finally
            {
                Marshal.ReleaseComObject(m_isbi);
            }
        }

        private void TestSetHKEY()
        {
            int hr;
            RegResult dispo;
            IntPtr hkey;

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

            Debug.Assert(i == 0, "Error reading registry");

            hr = m_isbi.SetHKEY(hkey);
            DsError.ThrowExceptionForHR( hr );

            RegCloseKey(hkey);
        }

        private void TestSetSIDs()
        {
            int hr;
            int i;
            IntPtr pSID = IntPtr.Zero;
            IntPtr pDomain = IntPtr.Zero;
            int iSIDSize = 0;
            int iDomainSize = 0;
            int peUse;
            IntPtr [] ppp = new IntPtr[2];

            i = LookupAccountName(null, Environment.UserName, pSID, ref iSIDSize, pDomain, ref iDomainSize, out peUse);
            Debug.Assert(Marshal.GetLastWin32Error() == 122, "LookupAccountName1");

            try
            {
                pSID = Marshal.AllocCoTaskMem(iSIDSize);
                pDomain = Marshal.AllocCoTaskMem(iDomainSize);

                i = LookupAccountName(null, Environment.UserName, pSID, ref iSIDSize, pDomain, ref iDomainSize, out peUse);
                Debug.Assert(i == 1, "LookupAccountName2");

                ppp[0] = pSID;
                ppp[1] = pSID;

                hr = m_isbi.SetSIDs(2, ppp);
                DsError.ThrowExceptionForHR(hr);

                // There isn't a good programmatic way to check this
            }
            finally
            {
                // Do we need to use FreeSid?  Beats me.

                if (pSID != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pSID);
                }
                if (pDomain != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pDomain);
                }
            }
        }
    }
}

