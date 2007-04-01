using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.SBE;

namespace v2_0
{
  public class IStreamBufferConfigure3Test
  {
    #region API

    [DllImport("advapi32.dll", SetLastError = true)]
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

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int RegCloseKey(
        IntPtr hKey
        );

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int RegGetKeySecurity(
        IntPtr hKey,
        SecurityInformation si,
        IntPtr psid,
        ref int iSize
        );

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int LookupAccountName(
        string lpSystemName,
        string lpAccountName,
        IntPtr Sid,
        ref int cbSid,
        IntPtr ReferencedDomainName,
        ref int cchReferencedDomainName,
        out int peUse
        );

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int GetLengthSid(
        IntPtr pSid
        );

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int GetSidSubAuthorityCount(
        IntPtr pSid
        );

    [DllImport("advapi32.dll", SetLastError = true)]
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
      OwnerSecurityInformation = 0x00000001,
      GroupSecurityInformation = 0x00000002,
      DACLSecurityInformation = 0x00000004,
      SACLSecurityInformation = 0x00000008,

      ProtectedDACLSecurityInformation = unchecked((int)0x80000000),
      ProtectedSACLSecurityInformation = 0x40000000,
      UnprotectedDACLSecurityInformation = 0x20000000,
      UnprotectedSACLSecurityInformation = 0x10000000
    }

    [Flags]
    public enum RegSAM
    {
      QueryValue = (0x0001),
      SetValue = (0x0002),
      CreateSubKey = (0x0004),
      EnumerateSubKeys = (0x0008),
      Notify = (0x0010),
      CreateLink = (0x0020),
      WOW64_32Key = (0x0200),
      WOW64_64Key = (0x0100),
      WOW64_Res = (0x0300),
      Read = (0x00020019),
      Write = (0x00020006),
      Execute = (0x00020019),
      AllAccess = (0x000f003f)
    }

    public enum HKEY
    {
      ClassesRoot = unchecked((int)0x80000000),
      CurrentUser = unchecked((int)0x80000001),
      LocalMachine = unchecked((int)0x80000002),
      Users = unchecked((int)0x80000003),
      PerformanceData = unchecked((int)0x80000004),
      PerformanceText = unchecked((int)0x80000050),
      PerformanceNlsText = unchecked((int)0x80000060),
      CurrentConfig = unchecked((int)0x80000005),
      DynData = unchecked((int)0x80000006)
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

    private IStreamBufferConfigure3 sbConfig3 = null;

    public void DoTests()
    {
      Config();

      try
      {
        TestSetStartRecConfig();
        TestNamespace();
      }
      finally
      {
        Marshal.ReleaseComObject(sbConfig3);
      }
    }

    private void TestSetStartRecConfig()
    {
      int hr = 0;

      hr = sbConfig3.SetStartRecConfig(true);
      Debug.Assert(hr == 0, "IStreamBufferConfigure3.SetStartRecConfig failed");

      bool val = false;

      hr = sbConfig3.GetStartRecConfig(out val);
      Debug.Assert((hr == 0) && (val == true), "IStreamBufferConfigure3.GetStartRecConfig failed");

      hr = sbConfig3.SetStartRecConfig(false);
      Debug.Assert(hr == 0, "IStreamBufferConfigure3.SetStartRecConfig failed");

      hr = sbConfig3.GetStartRecConfig(out val);
      Debug.Assert((hr == 0) && (val == false), "IStreamBufferConfigure3.GetStartRecConfig failed");
    }

    private void TestNamespace()
    {
      int hr = 0;

      // this method only accept "Global" or null
      hr = sbConfig3.SetNamespace("azerty");
      Debug.Assert(hr != 0, "IStreamBufferConfigure3.SetNamespace failed");

      hr = sbConfig3.SetNamespace(null);
      Debug.Assert(hr == 0, "IStreamBufferConfigure3.SetNamespace failed");

      hr = sbConfig3.SetNamespace("Global");
      Debug.Assert(hr == 0, "IStreamBufferConfigure3.SetNamespace failed");

      string val;

      hr = sbConfig3.GetNamespace(out val);
      Debug.Assert((hr == 0) && (val.Equals("Global")), "IStreamBufferConfigure3.GetNamespace failed");
    }

    private void Config()
    {
      int hr = 0;
      RegResult dispo;
      IntPtr hkey;
      sbConfig3 = (IStreamBufferConfigure3)new StreamBufferConfig();

      IStreamBufferInitialize isbi = (IStreamBufferInitialize)sbConfig3;

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
      DsError.ThrowExceptionForHR(hr);

    }
  }
}
