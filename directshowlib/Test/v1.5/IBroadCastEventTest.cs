using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v1._5
{
    public class IBroadcastEventTest : IBroadcastEvent
	{
        private Guid SID_SBroadcastEventService = new Guid("0B3FFB92-0919-4934-9D5B-619C719D0202");
        IBroadcastEvent m_pService;
        bool m_Fired = false;
        IFilterGraph2 m_ifg;
        int m_Cookie;

        public void DoTests()
        {
            Config();

            try
            {
                int hr = m_pService.Fire(Guid.NewGuid()); // Can be any guid
                DsError.ThrowExceptionForHR(hr);

                // Wait for it
                System.Threading.Thread.Sleep(1000);

                // See if it fired
                Debug.Assert(m_Fired, "Didn't Fire");
            }
            finally
            {
                if (m_pService != null)
                {
                    IConnectionPoint icp = m_pService as IConnectionPoint;
                    icp.Unadvise(m_Cookie);
                    Marshal.ReleaseComObject(m_pService);
                }
                if (m_ifg != null)
                {
                    Marshal.ReleaseComObject(m_ifg);
                }
            }
        }

        private void Config()
        {
            m_ifg = new FilterGraph() as IFilterGraph2;

            RegisterService(m_ifg);
            RegisterClient(m_ifg);
        }

        private void RegisterService(IFilterGraph ifg)
        {
            int hr;

            // Get the registration interface from the graph
            IRegisterServiceProvider spRegService = ifg as IRegisterServiceProvider;

            // Create the Broadcast Event Service object.
            IBroadcastEvent spBroadcastEvent = new BroadcastEventService() as IBroadcastEvent;

            try
            {
                // Register the Broadcast Event Service object as a service.
                hr = spRegService.RegisterService(
                    SID_SBroadcastEventService,
                    spBroadcastEvent);
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
                Marshal.ReleaseComObject(spBroadcastEvent);
            }
        }

        private void RegisterClient(IFilterGraph ifg)
        {
            int hr;
            object o;
            DirectShowLib.IServiceProvider spServiceProvider;

            // Get the service provider interface
            spServiceProvider = ifg as DirectShowLib.IServiceProvider;
            hr = spServiceProvider.QueryService(SID_SBroadcastEventService, typeof(IBroadcastEvent).GUID, out o);
            m_pService = o as IBroadcastEvent;

            // Register ourselves as a client
            IConnectionPoint icp = m_pService as IConnectionPoint;
            icp.Advise(this, out m_Cookie);
        }

        #region IBroadcastEvent Members

        // The callback
        public int Fire(Guid EventID)
        {
            m_Fired = true;
            return 0;
        }

        #endregion
    }
}
