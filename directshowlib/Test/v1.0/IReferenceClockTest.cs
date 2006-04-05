using System;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;
using System.ComponentModel;

namespace DirectShowLib.Test
{
    public class IReferenceClockTest
    {
        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null;
        IReferenceClock m_irc = null;

        public IReferenceClockTest()
        {
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        public void DoTests()
        {
            try
            {
                BuildGraph();

                TestGetTime();
                TestAdviseTime();
                TestAdvisePeriodic(); // Includes Unadvise
            }
            finally
            {
                if (m_ROT != null)
                {
                    m_ROT.Dispose();
                }
                if (m_imc != null)
                {
                    Marshal.ReleaseComObject(m_imc);
                }
                if (m_irc != null)
                {
                    Marshal.ReleaseComObject(m_irc);
                }
            }
        }

        void TestGetTime()
        {
            int hr;
            long ltime = 0;

            hr = m_irc.GetTime(out ltime);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(ltime > 0, "GetTime");
        }

        void TestAdviseTime()
        {
            int hr;
            long stime, stime2;
            int cookie;
            ManualResetEvent mre = new ManualResetEvent(false);

            // Get the current time
            hr = m_irc.GetTime(out stime);
            DsError.ThrowExceptionForHR(hr);

            // Ask for a notification
            hr = m_irc.AdviseTime(stime, 10000000, mre.SafeWaitHandle.DangerousGetHandle(), out cookie);
            DsError.ThrowExceptionForHR(hr);

            // Wait for the notification
            bool bRet = mre.WaitOne(1100, true);

            // Make sure we got it
            Debug.Assert(bRet, "AdviseTime1");

            // Get the new time
            hr = m_irc.GetTime(out stime2);
            DsError.ThrowExceptionForHR(hr);

            // Make sure some time has elapsed
            Debug.Assert(stime2 - stime >= 10000000, "AdviseTime");
        }

        void TestAdvisePeriodic()
        {
            int hr;
            long stime, stime2;
            int cookie;
            bool bRet;
            Semaphore mre = new Semaphore(1);

            // Get the current time
            hr = m_irc.GetTime(out stime);
            DsError.ThrowExceptionForHR(hr);

            // Ask for periodic notification
            hr = m_irc.AdvisePeriodic(stime + 10000000, 10000000, mre.SafeWaitHandle.DangerousGetHandle(), out cookie);
            DsError.ThrowExceptionForHR(hr);

            // Make sure we get some notifications
            for (int x=0; x < 5; x++)
            {
                bRet = mre.WaitOne(1100, true);
                Debug.Assert(bRet, "AdvisePeriodic");
            }

            // Get the new time
            hr = m_irc.GetTime(out stime2);
            DsError.ThrowExceptionForHR(hr);

            // Make sure time has elapsed
            Debug.Assert(stime2 - stime >= 40000000, "AdvisePeriodic");

            // Now turn the advise off
            hr = m_irc.Unadvise(cookie);
            DsError.ThrowExceptionForHR(hr);

            // Make sure it turns off
            bRet = mre.WaitOne(1100, true);
            Debug.Assert(!bRet, "Unadvise");
        }

        void BuildGraph()
        {
            int hr;
            IBaseFilter ppFilter;
            DsDevice [] devs;
            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;

            m_ROT = new DsROTEntry(graphBuilder);
            IFilterGraph2 ifg2 = graphBuilder as IFilterGraph2;

            devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = devs[0];

            hr = ifg2.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out ppFilter);
            DsError.ThrowExceptionForHR(hr);

            m_imc = graphBuilder as IMediaControl;
            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);

            hr = ppFilter.GetSyncSource(out m_irc);
            DsError.ThrowExceptionForHR(hr);

        }

        // Class to use semaphores
        internal sealed class Semaphore : WaitHandle
        {
            public Semaphore( int maxCount )
            {
                SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(CreateSemaphore(IntPtr.Zero, maxCount, maxCount, null), true);
                if ( SafeWaitHandle.IsInvalid )
                    throw new Win32Exception( Marshal.GetLastWin32Error() );      
            }

            public Semaphore( int initialCount, int maxCount )
            {
                SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(CreateSemaphore(IntPtr.Zero, initialCount, maxCount, null), true);

                if (SafeWaitHandle.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());      
            }

            public Semaphore( int maxCount, string name )
            {
                SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(CreateSemaphore(IntPtr.Zero, maxCount, maxCount, name), true);

                if (SafeWaitHandle.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            public Semaphore( int initialCount, int maxCount, string name )
            {
                SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(CreateSemaphore(IntPtr.Zero, initialCount, maxCount, name), true);

                if (SafeWaitHandle.IsInvalid)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            public int ReleaseSemaphore()
            {
                IntPtr previousCount = new IntPtr();

                if ( !ReleaseSemaphore( SafeWaitHandle.DangerousGetHandle(), 1, out previousCount ) )
                    throw new Win32Exception( Marshal.GetLastWin32Error() );

                return previousCount.ToInt32();
            }

            public int ReleaseSemaphore( int count )
            {
                IntPtr previousCount = new IntPtr();

                if ( !ReleaseSemaphore( SafeWaitHandle.DangerousGetHandle(), count, out previousCount ) )
                    throw new Win32Exception( Marshal.GetLastWin32Error() );

                return previousCount.ToInt32();
            }

            [DllImport("Kernel32.dll", SetLastError = true)]
            private static extern IntPtr CreateSemaphore( IntPtr lpSemaphoreAttributes,
                int lInitialCount, int lMaximumCount, string lpName );

            [DllImport("Kernel32.dll", SetLastError = true)]
            private static extern bool ReleaseSemaphore( IntPtr hSemaphore, int lReleaseCount,
                out IntPtr lpPreviousCount );

        }

    }
}