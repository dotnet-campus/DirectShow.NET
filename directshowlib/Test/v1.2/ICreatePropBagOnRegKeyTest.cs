using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
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

	public class ICreatePropBagOnRegKeyTest
	{
    ICreatePropBagOnRegKey createPropBagOnRegKey;

		public ICreatePropBagOnRegKeyTest()
		{
		}

    public void DoTests()
    {
      try
      {
        createPropBagOnRegKey = (ICreatePropBagOnRegKey) new CreatePropBagOnRegKey();

        TestCreate();

      }
      finally
      {
        Marshal.ReleaseComObject(createPropBagOnRegKey);
      }
    }

    private void TestCreate()
    {
      int hr = 0;
      object propBag;

      hr = createPropBagOnRegKey.Create(
        (IntPtr)HKEY.CurrentUser,
        @"SOFTWARE\Microsoft",
        0,
        (int)(RegSAM.Read | RegSAM.Write),
        typeof(IPropertyBag).GUID,
        out propBag
        );
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(propBag is IPropertyBag, "ICreatePropBagOnRegKey.Create");

      Marshal.ReleaseComObject(propBag);
    }


	}
}
