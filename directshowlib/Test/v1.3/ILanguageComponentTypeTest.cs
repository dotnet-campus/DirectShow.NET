using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
	public class ILanguageComponentTypeTest
	{
    private ILanguageComponentType langCompType = null;

		public ILanguageComponentTypeTest()
		{
		}

    public void DoTests()
    {
      langCompType = (ILanguageComponentType) new LanguageComponentType();

      try
      {
        TestLangID();
      }
      finally
      {
        Marshal.ReleaseComObject(langCompType);
      }
    }

    private void TestLangID()
    {
      int hr = 0;
      int lcid = 0;

      hr = langCompType.put_LangID(0x40C);
      DsError.ThrowExceptionForHR(hr);

      hr = langCompType.get_LangID(out lcid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(lcid == 0x40C, "ILanguageComponentType.get_LangID / put_LangID");
    }

	}
}
