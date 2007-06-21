using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class IEnumTuningSpacesTest
	{
    IEnumTuningSpaces enumTS;

		public IEnumTuningSpacesTest()
		{
		}

    public void DoTests()
    {
      try
      {
        Config();

        TestClone();
        TestNext();
        TestReset();
        TestSkip();
      }
      finally
      {
        Marshal.ReleaseComObject(enumTS);
      }
    }

    private void Config()
    {
      int hr = 0;
      ITuningSpaceContainer tsCont = (ITuningSpaceContainer) new SystemTuningSpaces();

      hr = tsCont.get_EnumTuningSpaces(out enumTS);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(tsCont);
    }

    private void TestClone()
    {
      int hr = 0;
      IEnumTuningSpaces clone;

      hr = enumTS.Clone(out clone);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(clone != null, "IEnumTuningSpaces.Clone");

      Marshal.ReleaseComObject(clone);
    }

    private void TestNext()
    {
      int hr = 0;
      ITuningSpace[] ts = new ITuningSpace[5];
      int fetched;
      int validObj = 0;
      IntPtr ip = Marshal.AllocCoTaskMem(4);

      hr = enumTS.Next(ts.Length, ts, ip);
      fetched = Marshal.ReadInt32(ip);
      Marshal.FreeCoTaskMem(ip);

      DsError.ThrowExceptionForHR(hr);

      for(int i = 0; i < fetched; i++)
      {
        if (ts[i] != null)
        {
          validObj++;
          Marshal.ReleaseComObject(ts[i]);
        }
      }

      Debug.Assert(validObj == fetched, "IEnumTuningSpaces.Next");
    }

    private void TestReset()
    {
      int hr = 0;

      hr = enumTS.Reset();

      Debug.Assert(hr == 0, "IEnumTuningSpaces.Reset");
    }

    private void TestSkip()
    {
      int hr = 0;

      hr = enumTS.Skip(4);

      Debug.Assert(hr == 0, "IEnumTuningSpaces.Skip");
    }

  }
}
