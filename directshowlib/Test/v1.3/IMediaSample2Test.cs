// Changed SetTime and SetMediaTime to use DsOptInt64
// Added comment to GetMediaType re DsUtils.FreeAMMediaType()

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DirectShowLib.Test
{
    public class IMediaSample2Test : ISampleGrabberCB
    {
        private const string g_TestFile = @"foo.avi";

        private IFilterGraph2 m_graphBuilder;

        // The IMediaSample to test
        private IMediaSample2 m_ims;

        // Avoid having a second sample overwrite the first
        private volatile bool m_GotOne = false;

        // Get the media type off the pin to use later
        private AMMediaType m_MediaType = null;

        // Since the tests are done in the callback, this is
        // used to signal when the tests are complete
        private ManualResetEvent m_TestComplete;

        public IMediaSample2Test()
        {
        }

        /// <summary>
        /// Test all IMediaSampleTest methods
        /// </summary>
        public void DoTests()
        {
            m_graphBuilder = BuildGraph(g_TestFile);

            // All the tests are called in ISampleGrabberCB.SampleCB, since
            // that's where we are when we get the IMediaSample2
            GetSample();

            // All done.  Release everything
            if (m_graphBuilder != null)
            {
                Marshal.ReleaseComObject(m_graphBuilder);
                m_graphBuilder = null;
            }

            if (m_MediaType != null)
            {
                DsUtils.FreeAMMediaType(m_MediaType);
                m_MediaType = null;
            }

            m_ims = null;
        }

        private void GetSample()
        {
            int hr;
            IMediaControl imc = m_graphBuilder as IMediaControl;

            // Use this to find out when the tests are finished
            m_TestComplete = new ManualResetEvent(false);

            // Start running the graph
            hr = imc.Run();
            Marshal.ThrowExceptionForHR(hr);

            // When the event is signaled, the tests are done
            bool bDone = m_TestComplete.WaitOne(5000, false);

            // If we timed out, we never ran the tests
            Debug.Assert(bDone, "Failed to get even one sample");

            // Clean up
            m_TestComplete.Close();
            m_TestComplete = null;
        }

        private void TestProperties()
        {
            int iSize = 48;
            int hr;
            IntPtr ip = Marshal.AllocCoTaskMem(iSize);
            AMSample2Properties stru = new AMSample2Properties();
            AMSample2Properties stru2 = new AMSample2Properties();
            AMMediaType amt = new AMMediaType();
            AMMediaType amt2;

            for (int x=0; x < iSize / 4; x++)
            {
                Marshal.WriteInt32(ip, x * 4, 0);
            }

            hr = m_ims.GetProperties(iSize, ip);
            DsError.ThrowExceptionForHR(hr);

            stru.cbData = Marshal.ReadInt32(ip);
            stru.dwTypeSpecificFlags = (AMVideoFlag)Marshal.ReadInt32(ip, 4);
            stru.dwSampleFlags = (AMSamplePropertyFlags)Marshal.ReadInt32(ip, 8);
            stru.lActual = Marshal.ReadInt32(ip, 12);
            stru.tStart = Marshal.ReadInt64(ip, 16);
            stru.tStop = Marshal.ReadInt64(ip, 24);
            stru.dwStreamId = Marshal.ReadInt32(ip, 32);
            IntPtr ip2 = Marshal.ReadIntPtr(ip, 36);
            if (ip2 != IntPtr.Zero)
            {
                Marshal.PtrToStructure(ip2, amt);
            }
            stru.pbBuffer = Marshal.ReadIntPtr(ip, 40);
            stru.cbBuffer = Marshal.ReadInt32(ip, 44);

            // An alternate approach
            Marshal.PtrToStructure(ip, stru2);
            if (stru2.pMediaType != IntPtr.Zero)
            {
                amt2 = new AMMediaType();
                Marshal.PtrToStructure(stru2.pMediaType, amt2);
            }

            Debug.Assert(stru.cbData == iSize, "GetProperties");

            hr = m_ims.SetProperties(iSize, ip);
            DsError.ThrowExceptionForHR(hr);
        }
        private IFilterGraph2 BuildGraph(string sFileName)
        {
            int hr;
            IBaseFilter ibfRenderer = null;
            IBaseFilter ibfAVISource = null;
            IPin IPinIn = null;
            IPin IPinOut = null;
            IPin iSampleIn = null;
            IPin iSampleOut = null;
            SampleGrabber sg = null;

            IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;

            try
            {
                // Get the file source filter
                ibfAVISource = new AsyncReader() as IBaseFilter;

                // Add it to the graph
                hr = graphBuilder.AddFilter(ibfAVISource, "Ds.NET AsyncReader");
                Marshal.ThrowExceptionForHR(hr);

                // Set the file name
                IFileSourceFilter fsf = ibfAVISource as IFileSourceFilter;
                hr = fsf.Load(sFileName, null);
                Marshal.ThrowExceptionForHR(hr);

                IPinOut = DsFindPin.ByDirection(ibfAVISource, PinDirection.Output, 0);

                // Get a SampleGrabber
                sg = new SampleGrabber();
                IBaseFilter grabFilt = sg as IBaseFilter;
                ISampleGrabber isg = sg as ISampleGrabber;

                // Add the sample grabber to the graph
                hr = graphBuilder.AddFilter(grabFilt, "Ds.NET SampleGrabber");
                Marshal.ThrowExceptionForHR(hr);

                iSampleIn = DsFindPin.ByDirection(grabFilt, PinDirection.Input, 0);
                iSampleOut = DsFindPin.ByDirection(grabFilt, PinDirection.Output, 0);

                // Get the default video renderer
                ibfRenderer = (IBaseFilter) new VideoRendererDefault();

                // Add it to the graph
                hr = graphBuilder.AddFilter(ibfRenderer, "Ds.NET VideoRendererDefault");
                Marshal.ThrowExceptionForHR(hr);
                IPinIn = DsFindPin.ByDirection(ibfRenderer, PinDirection.Input, 0);

                // Connect the file to the sample grabber
                hr = graphBuilder.Connect(IPinOut, iSampleIn);
                Marshal.ThrowExceptionForHR(hr);

                // Connect the sample grabber to the renderer
                hr = graphBuilder.Connect(iSampleOut, IPinIn);
                Marshal.ThrowExceptionForHR(hr);

                // Configure the sample grabber
                ConfigureSampleGrabber(isg);

                // Grab a copy of the mediatype being used.  Needed
                // in one of the tests
                m_MediaType = new AMMediaType();
                hr = IPinOut.ConnectionMediaType(m_MediaType);
                Marshal.ThrowExceptionForHR(hr);
            }
            catch
            {
                Marshal.ReleaseComObject(graphBuilder);
                throw;
            }
            finally
            {
                Marshal.ReleaseComObject(ibfRenderer);
                Marshal.ReleaseComObject(ibfAVISource);
                Marshal.ReleaseComObject(IPinIn);
                Marshal.ReleaseComObject(IPinOut);
                Marshal.ReleaseComObject(iSampleIn);
                Marshal.ReleaseComObject(iSampleOut);
                Marshal.ReleaseComObject(sg);
            }

            return graphBuilder;
        }

        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            int hr;

            // Call back on the SampleCB routine
            hr = sampGrabber.SetCallback( this, 0 );
            Marshal.ThrowExceptionForHR( hr );

            // Only one call
            hr = sampGrabber.SetOneShot(true);
            Marshal.ThrowExceptionForHR( hr );
        }

        int ISampleGrabberCB.SampleCB( double SampleTime, IMediaSample pSample )
        {
            // Make sure we only get called once
            if (!m_GotOne)
            {
                // copy the media sample to a member var
                m_GotOne = true;
                m_ims = pSample as IMediaSample2;

                // Call all the tests
                TestProperties();
                m_ims = null;

                // Set the completion event so DoTests can return
                m_TestComplete.Set();
            }

            Marshal.ReleaseComObject(pSample);

            return 0;
        }

        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.BufferCB( double SampleTime, IntPtr pBuffer, int BufferLen )
        {
            return 0;
        }
    }
}