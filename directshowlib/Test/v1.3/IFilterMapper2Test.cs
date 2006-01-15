using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using Microsoft.Win32;

namespace DirectShowLib.Test
{
	public class IFilterMapper2Test
	{
        private IFilterMapper2 m_fm2;

		public IFilterMapper2Test()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestCreateCategory();
                TestEnumMatchingFilters();
                TestUnregister();
                //TestRegister();
            }
            finally
            {
                Marshal.ReleaseComObject(m_fm2);
            }
        }

        private void TestCreateCategory()
        {
            int hr;
            string s = "{F76863EE-C75B-4578-8D85-6C4EF4BE7C7E}";
            Guid g = new Guid(s);

            hr = m_fm2.CreateCategory(g, Merit.DoNotUse, "Test Category");
            DsError.ThrowExceptionForHR(hr);

            // If CreateCategory fails, the DeleteSubKey will throw an exception
            RegistryKey reg = Registry.ClassesRoot.OpenSubKey(@"CLSID\{DA4E3DA0-D07D-11d0-BD50-00A0C911CE86}\Instance", true);
            reg.DeleteSubKey(s);
        }

        private void TestEnumMatchingFilters()
        {
            int hr;
            UCOMIEnumMoniker pEnum;
            Guid [] pInputType = new Guid[4];
            Guid [] pOutputType = new Guid[4];

            pInputType[0] = MediaType.ScriptCommand;
            pInputType[1] = MediaSubType.WAVE;
            pInputType[2] = MediaType.Video;
            pInputType[3] = MediaSubType.Null;

            pOutputType[0] = MediaType.ScriptCommand;
            pOutputType[1] = MediaSubType.WAVE;
            pOutputType[2] = MediaType.Video;
            pOutputType[3] = MediaSubType.RGB24;

            hr = m_fm2.EnumMatchingFilters(
                out pEnum, 
                0, 
                true, 
                Merit.Normal, 
                true, 
                0, 
                null, 
                null, 
                null,
                false,
                true,
                0,
                null,
                null,
                null);
            DsError.ThrowExceptionForHR(hr);

            int c1 = CountFilters(pEnum);
            Debug.Assert(c1 > 0);

            hr = m_fm2.EnumMatchingFilters(
                out pEnum, 
                0, 
                true, 
                Merit.Normal, 
                true, 
                1, 
                pInputType, 
                null, 
                null,
                false,
                true,
                1,
                pOutputType,
                null,
                null);
            DsError.ThrowExceptionForHR(hr);

            int c2 = CountFilters(pEnum);
            Debug.Assert(c2 == 0);

            hr = m_fm2.EnumMatchingFilters(
                out pEnum, 
                0, 
                true, 
                Merit.Normal, 
                true, 
                2, 
                pInputType, 
                null, 
                null,
                false,
                true,
                2,
                pOutputType,
                null,
                null);
            DsError.ThrowExceptionForHR(hr);

            int c3 = CountFilters(pEnum);
            Debug.Assert(c3 > 0);
        }

        private void TestUnregister()
        {
            int hr;

            // This is the guid for one of the sample filters I provide
            Guid filt = new Guid("{3893D412-BE20-46c0-89F0-8045881D553C}");

            // The is the "DirectShow Filters" category
            Guid cat = FilterCategory.LegacyAmFilterCategory;

            hr = m_fm2.UnregisterFilter(cat, null, filt);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestRegister()
        {
        }

        private int CountFilters(UCOMIEnumMoniker pEnum)
        {
            int hr;
            int c = 0;
            int junk;
            UCOMIMoniker [] mon = new UCOMIMoniker[1];

            do
            {
                hr = pEnum.Next(1, mon, out junk);
                c++;
            } while (hr == 0 && junk == 1);


            return c - 1;
        }

        private void Config()
        {
            m_fm2 = (IFilterMapper2)new FilterMapper2();
        }
	}
}
