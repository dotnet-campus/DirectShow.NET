using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;

namespace v1._5
{
    public class IServiceProviderTest
    {
        DirectShowLib.IServiceProvider m_isp;
        foo2 m_f;

        public void DoTests()
        {
            Setup();

            TestQuery();

            Marshal.ReleaseComObject(m_isp);
        }

        private void Setup()
        {
            int hr;
            m_f = new foo2();

            IRegisterServiceProvider rsp = new FilterGraph() as IRegisterServiceProvider;
            hr = rsp.RegisterService(typeof(foo2).GUID, m_f);
            DsError.ThrowExceptionForHR(hr);

            m_isp = rsp as DirectShowLib.IServiceProvider;
        }

        private void TestQuery()
        {
            int hr;
            object o;

            hr = m_isp.QueryService(typeof(foo2).GUID, typeof(IAMGraphBuilderCallback).GUID, out o);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(o == m_f);
        }
    }

    [ComVisible(true),
    Guid("78214A87-40F1-4439-ADDA-A744AC34271D"),
    ClassInterface(ClassInterfaceType.None)]
    public class foo2 : IAMGraphBuilderCallback
    {
        #region IAMGraphBuilderCallback Members

        public int SelectedFilter(System.Runtime.InteropServices.ComTypes.IMoniker pMon)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int CreatedFilter(IBaseFilter pFil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
