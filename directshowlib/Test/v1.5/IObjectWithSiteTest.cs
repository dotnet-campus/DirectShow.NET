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

    public class IObjectWithSiteTest
    {
        IObjectWithSite m_iows;

        public void DoTests()
        {
            Setup();

            TestSetGet();
        }

        private void TestSetGet()
        {
            int hr;
            object o;

            hr = m_iows.GetSite(typeof(IAMGraphBuilderCallback).GUID, out o);
            Debug.Assert(hr < 0, "Should fail until we assign a value");

            foo1 f = new foo1();

            hr = m_iows.SetSite(f);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iows.GetSite(typeof(IAMGraphBuilderCallback).GUID, out o);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(o == f);
        }

        private void Setup()
        {
            m_iows = new FilterGraph() as IObjectWithSite;
        }
    }

    [ComVisible(true), 
    Guid("78214A87-40F1-4439-ADDA-A744AC34271D"), 
    ClassInterface(ClassInterfaceType.None)]
    public class foo1 : IAMGraphBuilderCallback
    {
        #region IAMGraphBuilderCallback Members

        public int SelectedFilter(IMoniker pMon)
        {
            return 0;
        }

        public int CreatedFilter(IBaseFilter pFil)
        {
            return 0;
        }

        #endregion

    }
}
