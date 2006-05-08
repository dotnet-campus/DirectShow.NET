using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32;

using DirectShowLib;

namespace v1._5
{

    public class IAMGraphBuilderCallbackTest
    {
        IFilterGraph2 m_ifg;

        public void DoTests()
        {
            Setup();

            TestSetGet();

            Marshal.ReleaseComObject(m_ifg);
        }

        private void TestSetGet()
        {
            int hr;

            hr = m_ifg.RenderFile("foo.avi", null);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(foo.bCreated, "Hit Created");
            Debug.Assert(foo.bSelected, "Hit Selected");
        }

        private void Setup()
        {
            int hr;

            m_ifg = new FilterGraph() as IFilterGraph2;
            IObjectWithSite iows = m_ifg as IObjectWithSite;

            hr = iows.SetSite(new foo());
            DsError.ThrowExceptionForHR(hr);
        }
    }



    [ComVisible(true), 
    Guid("78214A87-40F1-4439-ADDA-A744AC34271D"), 
    ClassInterface(ClassInterfaceType.None)]
    public class foo : IAMGraphBuilderCallback
    {
        static public bool bSelected = false;
        static public bool bCreated = false;

        #region IAMGraphBuilderCallback Members

        public int SelectedFilter(IMoniker pMon)
        {
            int hr;
            IBaseFilter ibf;
            object p;

            bSelected = true;

            // Get an instance of the filter
            Guid g = typeof(IBaseFilter).GUID;
            pMon.BindToObject(null, null, ref g, out p);
            ibf = (IBaseFilter)p;

            Guid cid;
            hr = ibf.GetClassID(out cid);

            // Get the class name of the filter
            string s = string.Format("CLSID\\{0}", cid.ToString("b"));
            RegistryKey r = Registry.ClassesRoot.OpenSubKey(s);
            object o = r.GetValue(null);

            // Print it out
            Debug.WriteLine(o.ToString());

            return 0;
        }

        public int CreatedFilter(IBaseFilter pFil)
        {
            bCreated = true;

            return 0;
        }

        #endregion

    }

}
