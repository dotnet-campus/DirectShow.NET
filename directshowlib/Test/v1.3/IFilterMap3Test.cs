using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Summary description for IFilterMap3Test.
	/// </summary>
	public class IFilterMapper3Test
	{
        private IFilterMapper3 m_fm3;
		public IFilterMapper3Test()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestEnum();
            }
            finally
            {
                Marshal.ReleaseComObject(m_fm3);
            }
        }

        private void TestEnum()
        {
            int hr;
            ICreateDevEnum pEnum;

            hr = m_fm3.GetICreateDevEnum(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum != null, "GetICreateDevEnum");
        }

        private void Config()
        {
            m_fm3 = (IFilterMapper3) new FilterMapper2(); 
        }
	}
}
