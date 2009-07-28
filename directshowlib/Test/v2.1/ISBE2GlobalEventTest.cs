using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;
using DirectShowLib.BDA;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;

namespace v2_1
{
    public class ISBE2GlobalEventTest : IBroadcastEventEx
    {
        enum EventCmd
        {
            Init,
            Run,
            Event,
            Exit
        }

        class EventParams
        {
            public EventCmd ec;

            public Guid EventID;
            public int Param1;
            public int Param2;
            public int Param3;
            public int Param4;

            public EventParams(EventCmd c)
            {
                ec = c;
            }
        }

        private IBroadcastEventEx spBroadcastEvent;
        private int dwBroadcastEventCookie;
        private ISBE2GlobalEvent m_ge;
        private Thread m_thread;
        private IMediaControl m_mc;
        private Queue m_Q;
        private AutoResetEvent m_re;
        private bool m_bWorked;

        public ISBE2GlobalEventTest()
        {
            m_bWorked = false;
            m_re = new AutoResetEvent(false);
            m_Q = new Queue();
            m_thread = new Thread(ProcEvents);
            m_thread.Start();

            m_Q.Enqueue(new EventParams(EventCmd.Init));
            m_re.WaitOne();
        }

        public void DoTests()
        {
            m_Q.Enqueue(new EventParams(EventCmd.Run));
            m_re.WaitOne();

            System.Threading.Thread.Sleep(5000);

            m_mc.Stop();
            System.Threading.Thread.Sleep(1000);

            m_Q.Enqueue(new EventParams(EventCmd.Exit));
            m_re.WaitOne();

            Debug.Assert(m_bWorked == true);
        }

        private void ProcEvents()
        {
            bool bDone = false;
            int hr;
            bool sp;
            int i = 0;
            IntPtr ip = IntPtr.Zero;

            while (!bDone)
            {
                while (m_Q.Count == 0)
                {
                    Thread.Sleep(1);
                }

                EventParams ep = (EventParams)m_Q.Dequeue();

                switch (ep.ec)
                {
                    case EventCmd.Init:
                        Config2();
                        break;

                    case EventCmd.Run:
                        m_mc.Run();
                        break;

                    case EventCmd.Event:

                        Debug.WriteLine(ep.EventID);

                        if (ep.EventID == SBEEvent.StreamDescEvent)
                        {
                            if (!m_bWorked)
                            {
                                DVRStreamDesc sd = new DVRStreamDesc();

                                ip = IntPtr.Zero;
                                sp = false;
                                i = 0;

                                hr = m_ge.GetEvent(ep.EventID, ep.Param1, ep.Param2, ep.Param3, ep.Param4, out sp, ref i, ip);
                                Thread.Sleep(1);
                                ip = Marshal.AllocCoTaskMem(i);
                                try
                                {
                                    sp = false;
                                    hr = m_ge.GetEvent(ep.EventID, ep.Param1, ep.Param2, ep.Param3, ep.Param4, out sp, ref i, ip);
                                    Marshal.PtrToStructure(ip, sd);

                                    m_bWorked = hr == 0 && sd.guidFormatType == sd.MediaType.formatType && sd.guidFormatType != Guid.Empty;
                                }
                                finally
                                {
                                    Marshal.FreeCoTaskMem(ip);
                                }
                            }

                            break;
                        }
                        break;
                    case EventCmd.Exit:
                        bDone = true;
                        break;
                }
                m_re.Set();

            }
        }

        private void Config2()
        {
            int hr;
            IFilterGraph2 fg;
            ISBE2Crossbar iSBE2Crossbar;

            fg = new FilterGraph() as IFilterGraph2;
            IBaseFilter streamBuffer = (IBaseFilter)new StreamBufferSource();
            m_ge = streamBuffer as ISBE2GlobalEvent;
            m_mc = fg as IMediaControl;

            hr = fg.AddFilter(streamBuffer, "SBS");
            DsError.ThrowExceptionForHR(hr);

            IFileSourceFilter fs = streamBuffer as IFileSourceFilter;
            hr = fs.Load(@"C:\Users\Public\Recorded TV\Sample Media\win7_scenic-demoshort_raw.wtv", null);
            DsError.ThrowExceptionForHR(hr);

            iSBE2Crossbar = streamBuffer as ISBE2Crossbar;
            hr = iSBE2Crossbar.EnableDefaultMode(CrossbarDefaultFlags.None);
            DsError.ThrowExceptionForHR(hr);

            HookupGraphEventService(fg);
            RegisterForSBEGlobalEvents();
        }

        private int HookupGraphEventService(IFilterGraph pGraph)
        {
            object o;
            int hr;

            DirectShowLib.IServiceProvider spServiceProvider = (DirectShowLib.IServiceProvider)pGraph;

            hr = spServiceProvider.QueryService(typeof(BroadcastEventService).GUID,
                typeof(IBroadcastEventEx).GUID,
                out o);

            spBroadcastEvent = o as IBroadcastEventEx;

            if (hr < 0 || spBroadcastEvent == null)
            {
                // Create the Broadcast Event Service object.
                spBroadcastEvent = new BroadcastEventService() as IBroadcastEventEx;

                IRegisterServiceProvider spRegService = (IRegisterServiceProvider)pGraph;

                // Register the Broadcast Event Service object as a service.
                hr = spRegService.RegisterService(
                    typeof(BroadcastEventService).GUID,
                    spBroadcastEvent);
            }

            return hr;
        }

        // Establish the connection point to receive events.
        private int RegisterForSBEGlobalEvents()
        {
            IConnectionPoint spConnectionPoint = (IConnectionPoint)spBroadcastEvent;
            spConnectionPoint.Advise((IBroadcastEventEx)(this),
                out dwBroadcastEventCookie);
            return 0;
        }

        #region IBroadcastEventEx Members

        public int Fire(Guid EventID)
        {
            throw new NotImplementedException();
        }

        public int FireEx(Guid EventID, int Param1, int Param2, int Param3, int Param4)
        {
            EventParams ep = new EventParams(EventCmd.Event);

            ep.EventID = EventID;
            ep.Param1 = Param1;
            ep.Param2 = Param2;
            ep.Param3 = Param3;
            ep.Param4 = Param4;
            m_Q.Enqueue(ep);

            m_re.WaitOne();

            return 0;
        }

        #endregion
    }
}
