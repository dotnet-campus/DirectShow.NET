using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace DirectShowLib.Test
{
    [ComVisible(true), Guid("fc4801a1-2ba9-11cf-a229-00aa003d7352"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBindHost
    {
        int CreateMoniker( 
            /* [in] */ string szName,
            /* [in] */ UCOMIBindCtx pBC,
            /* [out] */ out UCOMIMoniker ppmk,
            /* [in] */ int dwReserved);
        
        int MonikerBindToStorage( 
            /* [in] */ UCOMIMoniker pMk,
            /* [in] */ UCOMIBindCtx pBC,
            /* [in] */ IntPtr pBSC,
            /* [in] */ [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            /* [out] */ out object ppvObj);
        
        int MonikerBindToObject( 
            /* [in] */ UCOMIMoniker pMk,
            /* [in] */ UCOMIBindCtx pBC,
            /* [in] */ IntPtr pBSC,
            /* [in] */ [MarshalAs(UnmanagedType.LPStruct)] Guid  riid,
            /* [out] */ out object ppvObj);
    }


    
    [ComVisible(true), Guid("6d5140c1-7436-11ce-8034-00aa006009fa"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMyServiceProvider
    {
        [PreserveSig]
        int QueryService( 
            [MarshalAs(UnmanagedType.LPStruct)] Guid guidService,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out object ppvObject);
    }

    [
    ComImport(),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("FC5971A5-C2D2-4442-9778-E924303A3399")
    ]
    public interface IMyObjectWithSite
    {
        [PreserveSig]
        int SetSite ([MarshalAs(UnmanagedType.IUnknown)]object site);
        [PreserveSig]
        int GetSite (ref Guid guid, out IntPtr ppvSite);
    }

    [Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectWithSite
    {
        [PreserveSig]
        int SetSite( 
            [MarshalAs(UnmanagedType.Interface)] object pUnkSite);
        
        [PreserveSig]
        int GetSite( 
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out object ppvSite);
    }

    public class IAMFilterGraphCallbackTest : IAMFilterGraphCallback, IServiceProvider, IObjectWithSite, IMyObjectWithSite, IMyServiceProvider, IBindHost
	{
        private IAMClockSlave m_cs;

		public IAMFilterGraphCallbackTest()
		{
        }

        public void DoTests()
        {
            Config();

            try
            {
            }
            finally
            {
                Marshal.ReleaseComObject(m_cs);
            }
        }
#if false
To use this interface, implement the interface in your application or client object. Query the Filter 
        Graph Manager for the IObjectWithSite interface and call the IObjectWithSite::SetSite method with 
        a pointer to your implementation of the interface. The Filter Graph Manager calls the methods on this 
        interface while it builds the graph, which gives the client the opportunity to modify the graph-building process.

#endif
        private void Config()
        {
            int hr;
            IBaseFilter pFilter;
            object o;

            IFilterGraph ifg = new FilterGraph() as IFilterGraph;

            IObjectWithSite iows = ifg as IObjectWithSite;
            hr = iows.SetSite(this);
            hr = iows.GetSite(typeof(IAMFilterGraphCallback).GUID, out o);
            hr = ((IGraphBuilder)ifg).AddSourceFilter("DirectShowLib.dll", null, out pFilter);
            IPin pPin = DsFindPin.ByDirection(pFilter, PinDirection.Output, 0);
            hr = ((IGraphBuilder)ifg).Render(pPin);
            //hr = ((IGraphBuilder)ifg).RenderFile("DirectShowLib.dll", null);
            DsError.ThrowExceptionForHR(hr);
        }

        #region IAMFilterGraphCallback Members

        public int UnableToRender(IPin pPin)
        {
            Debug.WriteLine("asdf");
            return 0;
        }

        #endregion

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.GetService implementation
            return null;
        }

        #endregion

        #region IObjectWithSite Members

        public int SetSite(object pUnkSite)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.SetSite implementation
            return 0;
        }

        public int GetSite(Guid riid, out object ppvSite)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.GetSite implementation
            ppvSite = null;
            return 0;
        }

        #endregion

        #region IMyObjectWithSite Members

        int DirectShowLib.Test.IMyObjectWithSite.GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.DirectShowLib.Test.IMyObjectWithSite.GetSite implementation
            ppvSite = new IntPtr ();
            return 0;
        }

        int DirectShowLib.Test.IMyObjectWithSite.SetSite(object o)
        {
            return 0;
        }
        #endregion

        #region IMyServiceProvider Members

        public int QueryService(Guid guidService, Guid riid, out object ppvObject)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.QueryService implementation
            ppvObject = null;
            return 0;
        }

        #endregion

        #region IBindHost Members

        public int CreateMoniker(string szName, UCOMIBindCtx pBC, out UCOMIMoniker ppmk, int dwReserved)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.CreateMoniker implementation
            ppmk = null;
            return 0;
        }

        public int MonikerBindToStorage(UCOMIMoniker pMk, UCOMIBindCtx pBC, IntPtr pBSC, Guid riid, out object ppvObj)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.MonikerBindToStorage implementation
            ppvObj = null;
            return 0;
        }

        public int MonikerBindToObject(UCOMIMoniker pMk, UCOMIBindCtx pBC, IntPtr pBSC, Guid riid, out object ppvObj)
        {
            // TODO:  Add IAMFilterGraphCallbackTest.MonikerBindToObject implementation
            ppvObj = null;
            return 0;
        }

        #endregion
    }
}
