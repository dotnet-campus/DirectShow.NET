/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

// This code is just to show a couple of ways you could use the 
// GenericSampleSourceFilter.  For a more details discussion, check out the
// readme.txt

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using DirectShowLib;
using GenericSampleSourceFilterClasses;
using rdr;

namespace DxPlayx
{
	/// <summary>
	/// A class to construct a graph using the GenericSampleSourceFilter.
	/// </summary>
    internal class DxPlay : IDisposable
    {
        #region Definitions

        /// <summary>
        /// 100 ns - used by a number of DS methods
        /// </summary>
        private const long UNIT = 10000000;

        /// <summary>
        /// The Frames Per Second for the source filter to use
        /// </summary>
        private const long FPS =  UNIT / 8; // 25 fps

        #endregion

        #region Member variables

        // Event called when the graph stops
        public event EventHandler Completed = null;

        /// <summary>
        /// graph builder interfaces
        /// </summary>
        private IFilterGraph2 m_graphBuilder;

        /// <summary>
        /// Another graph builder interface
        /// </summary>
        private IMediaControl m_mediaCtrl;

#if DEBUG
        /// <summary>
        /// Allow you to "Connect to remote graph" from GraphEdit
        /// </summary>
        DsROTEntry m_DsRot;
#endif

        #endregion

        /// <summary>
        /// Play a video using the GenericSampleSourceFilter as the video source
        /// </summary>
        /// <param name="sFileName">Avi file to play</param>
        public DxPlay(string sFileName)
        {
            try
            {
                // Set up the graph
                SetupGraph(sFileName);
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Release everything
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            CloseInterfaces();
        }

        /// <summary>
        /// Alternate cleanup
        /// </summary>
        ~DxPlay()
        {
            CloseInterfaces();
        }


        /// <summary>
        /// Start playing
        /// </summary>
        public void Start()
        {
            // Create a new thread to process events
            Thread t;
            t = new Thread(new ThreadStart(EventWait));
            t.Name = "Media Event Thread";
            t.Start();
            int hr = m_mediaCtrl.Run();
            DsError.ThrowExceptionForHR( hr );
        }

        /// <summary>
        /// Stop the capture graph.
        /// </summary>
        public void Stop()
        {
            int hr;
			
            hr = ((IMediaEventSink)m_graphBuilder).Notify(EventCode.UserAbort, 0, 0);
            DsError.ThrowExceptionForHR( hr );

            hr = m_mediaCtrl.Stop();
            DsError.ThrowExceptionForHR( hr );
        }


        /// <summary>
        /// Build the filter graph
        /// </summary>
        /// <param name="hWin">Window to draw into</param>
        private void SetupGraph(string sFileName)
        {
            int hr;

            // Get the graphbuilder object
            m_graphBuilder = new FilterGraph() as IFilterGraph2;

            // Get a ICaptureGraphBuilder2 to help build the graph
            ICaptureGraphBuilder2 icgb2 = (ICaptureGraphBuilder2)new CaptureGraphBuilder2() ;

            try
            {
                // Link the ICaptureGraphBuilder2 to the IFilterGraph2
                hr = icgb2.SetFiltergraph(m_graphBuilder);
                DsError.ThrowExceptionForHR( hr );

#if DEBUG
                // Allows you to view the graph with GraphEdit File/Connect
                m_DsRot = new DsROTEntry(m_graphBuilder);
#endif

                // Our graph source filter
                IBaseFilter ipsb = (IBaseFilter)new GenericSamplePullFilter();

                try
                {
                    // Get the output pin from the filter so we can configure it
                    IPin gssfOut = DsFindPin.ByDirection(ipsb, PinDirection.Output, 0);

                    // Configure the pin's media type and callback
                    string sExt = Path.GetExtension(sFileName);
                    ConfigurePuller((IGenericPullConfig)gssfOut, sExt);
                    
                    // Configure the file name
                    ConfigureRdr((ISetFileName)gssfOut, sFileName);

                    // Free the pin
                    Marshal.ReleaseComObject(gssfOut);

                    // Add the filter to the graph
                    hr = m_graphBuilder.AddFilter(ipsb, "GenericSamplePullFilter");
                    Marshal.ThrowExceptionForHR( hr );

                    // Build the rest of the graph, outputting to the default renderer
                    hr = icgb2.RenderStream(null, null, ipsb, null, null);
                    Marshal.ThrowExceptionForHR( hr );

                    // Connect any audio pin
                    hr = icgb2.RenderStream(null, MediaType.Audio, ipsb, null, null);
                    //Marshal.ThrowExceptionForHR( hr ); // Blindly assume any errors are due to no audio pin
                }
                finally
                {
                    Marshal.ReleaseComObject(ipsb);
                }

                // Grab some other interfaces
                m_mediaCtrl = m_graphBuilder as IMediaControl;
            }
            finally
            {
                Marshal.ReleaseComObject(icgb2);
            }
        }

        // Configure the Reader
        private void ConfigureRdr(ISetFileName pPin, string sFileName)
        {
            int hr;

            hr = pPin.SetFileName(sFileName);
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Configure the GenericSamplePullFilter
        /// </summary>
        /// <param name="ips">The interface to the GenericSampleSourceFilter</param>
        private void ConfigurePuller(IGenericPullConfig ips, string sExt)
        {
            int hr;
            AMMediaType amt = new AMMediaType();

            // IAsync uses a very basic MediaType
            amt.majorType = MediaType.Stream;

            // Looks like an avi file
            if (sExt.ToLower() == ".avi")
            {
                amt.subType = MediaSubType.Avi;
            }
            else
            {
                amt.subType = MediaSubType.MPEG1System;
            }
            amt.formatType = Guid.Empty;

            // Send in the MediaType
            hr = ips.SetMediaType(amt);
            DsError.ThrowExceptionForHR(hr);

            // Specify the embedded object to use.
            hr = ips.SetEmbedded(typeof(AsyncRdr).GUID);
            DsError.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Shut down graph
        /// </summary>
        private void CloseInterfaces()
        {
            int hr;

            lock (this)
            {
                // Stop the graph
                if( m_mediaCtrl != null )
                {
                    // Stop the graph
                    hr = m_mediaCtrl.Stop();
                    m_mediaCtrl = null;
                }

#if DEBUG
                if (m_DsRot != null)
                {
                    m_DsRot.Dispose();
                }
#endif

                hr = ((IMediaEventSink)m_graphBuilder).Notify(EventCode.UserAbort, 0, 0);

                // Release the graph
                if (m_graphBuilder != null)
                {
                    Marshal.ReleaseComObject(m_graphBuilder);
                    m_graphBuilder = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Called on a new thread to process events from the graph.  The thread
        /// exits when the graph finishes.
        /// </summary>
        private void EventWait()
        {
            // Returned when GetEvent is called but there are no events
            const int E_ABORT = unchecked((int)0x80004004);

            int hr;
            int p1, p2;
            EventCode ec;
            EventCode exitCode = 0;

            IMediaEvent pEvent = (IMediaEvent)m_graphBuilder;

            do
            {
                // Read the event
                for (
                    hr = pEvent.GetEvent(out ec, out p1, out p2, 100);
                    hr >= 0;
                    hr = pEvent.GetEvent(out ec, out p1, out p2, 100)
                    )
                {
                    Debug.WriteLine(ec);
                    switch(ec)
                    {
                            // If the clip is finished playing
                        case EventCode.Complete:
                        case EventCode.ErrorAbort:
                        case EventCode.UserAbort:
                            exitCode = ec;

                            // Release any resources the message allocated
                            hr = pEvent.FreeEventParams(ec, p1, p2);
                            DsError.ThrowExceptionForHR(hr);
                            break;

                        case EventCode.OleEvent:
                            string s1 = Marshal.PtrToStringBSTR(new IntPtr(p1));
                            string s2 = Marshal.PtrToStringBSTR(new IntPtr(p2));
                            string s3 = s2.Remove(s2.Length -1, 1);

                            Debug.WriteLine(string.Format("OleEvent Parms: {0} | {1}", s1, s3));
                            hr = pEvent.FreeEventParams(ec, p1, p2);
                            break;

                        default:
                            // Release any resources the message allocated
                            hr = pEvent.FreeEventParams(ec, p1, p2);
                            DsError.ThrowExceptionForHR(hr);
                            break;
                    }
                }

                // If the error that exited the loop wasn't due to running out of events
                if (hr != E_ABORT)
                {
                    DsError.ThrowExceptionForHR(hr);
                }
            } while (exitCode == 0);

            pEvent = null;

            // Send an event saying we are complete
            if (Completed != null)
            {
                CompletedArgs ca = new CompletedArgs(exitCode);
                Completed(this, ca);
            }

        } // Exit the thread


        public class CompletedArgs : System.EventArgs
        {
            /// <summary>The result of the rendering</summary>
            /// <remarks>
            /// This code will be a member of DirectShowLib.EventCode.  Typically it 
            /// will be EventCode.Complete, EventCode.ErrorAbort or EventCode.UserAbort.
            /// </remarks>
            public EventCode Result;

            /// <summary>
            /// Used to construct an instace of the class.
            /// </summary>
            /// <param name="ec"></param>
            internal CompletedArgs(EventCode ec)
            {
                Result = ec;
            }
        }
    }
}
