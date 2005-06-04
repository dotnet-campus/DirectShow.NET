using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class ISampleGrabberTest : ISampleGrabberCB
    {
        private const string g_TestFile = @"foo.avi";

        private IFilterGraph2 m_graphBuilder;

        // This is used to signal when the callbacks are called
        private ManualResetEvent m_TestComplete;

        // Samplegrabber interface used to choose which callback to use
        private ISampleGrabber m_isg;

        // Used to store which callbacks have been called
        private int m_Called;

        IMediaControl m_imc = null;

        public ISampleGrabberTest()
        {
        }

        /// <summary>
        /// Test all ISampleGrabberCBTest methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            m_graphBuilder = BuildGraph(g_TestFile);
            // Use this to find out when the tests are finished
            m_TestComplete = new ManualResetEvent(false);

            TestSetCallback();
            TestGetConnectedMediaType();
            TestSamples();
            TestOneShot();

            // All done.  Release everything
            if (m_graphBuilder != null)
            {
                Marshal.ReleaseComObject(m_graphBuilder);
                m_graphBuilder = null;
            }

            if (m_isg != null)
            {
                Marshal.ReleaseComObject(m_isg);
                m_isg = null;
            }

            if (m_TestComplete != null)
            {
                m_TestComplete.Close();
                m_TestComplete = null;
            }
        }

        void TestSetCallback()
        {
            int hr;
            bool bDone;
            m_imc = m_graphBuilder as IMediaControl;

            // Bitmap holding which callbacks were called
            m_Called = 0;

            for (int y=0; y < 2; y++)
            {
                m_TestComplete.Reset();

                // Call back on the specified routine
                hr = m_isg.SetCallback( this, y );
                Marshal.ThrowExceptionForHR( hr );

                // Start running the graph
                hr = m_imc.Run();
                Marshal.ThrowExceptionForHR(hr);

                // When the event is signaled, the test is done
                bDone = m_TestComplete.WaitOne(10000, false);

                // If we timed out, we failed
                Debug.Assert(bDone, "Failed to get even one sample");

                // Stop running the graph
                hr = m_imc.Stop();
                Marshal.ThrowExceptionForHR(hr);
            }

            Debug.Assert(m_Called == 3, "Called both");
        }

        void TestGetConnectedMediaType()
        {
            int hr;
            AMMediaType pmt = new AMMediaType();

            hr = m_isg.GetConnectedMediaType(pmt);
            DsError.ThrowExceptionForHR(hr);

            // Media type is due to using a video file
            Debug.Assert(pmt.majorType == MediaType.Video, "GetConnectedMediaType");

            // Subtype comes from the SetMediaType in BuildGraph
            Debug.Assert(pmt.subType == MediaSubType.RGB24, "SetMediaType");

            DsUtils.FreeAMMediaType(pmt);
        }
        void TestSamples()
        {
            int hr;
            int iSize = 0;
            IMediaSample pSample;

            hr = m_isg.SetBufferSamples(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imc.Run();
            Marshal.ThrowExceptionForHR(hr);

            // Wait a moment for the graph to start running
            Thread.Sleep(500);

            // Get a buffer (needs SetBufferSamples(true)
            hr = m_isg.GetCurrentBuffer(ref iSize, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iSize > 0, "GetCurrentBuffer");

            hr = m_isg.GetCurrentSample(out pSample);
            // E_NOTIMPL - Nothing to test
            if (hr != -2147467263)
            {
                DsError.ThrowExceptionForHR(hr);
            }

            hr = m_imc.Stop();
            Marshal.ThrowExceptionForHR(hr);
        }

        void TestOneShot()
        {
            int hr;
            bool bDone;

            m_TestComplete.Reset();

            hr = m_isg.SetOneShot(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imc.Run();
            Marshal.ThrowExceptionForHR(hr);

            // Get one sample
            bDone = m_TestComplete.WaitOne(10000, false);
            Thread.Sleep(100);
            m_TestComplete.Reset();

            // Should be able to get at least one
            Debug.Assert(bDone, "SetOneShot1");

            // Try to get another
            bDone = m_TestComplete.WaitOne(1000, false);

            // Should fail since we said "OneShot"
            // You may need to comment out the call to SetMediaType in
            // BuildGraph for this to work correctly.
            Debug.Assert(!bDone, "SetOneShot2");

            hr = m_imc.Stop();
            Marshal.ThrowExceptionForHR(hr);
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
                m_isg = sg as ISampleGrabber;

                // Set the media type to Video/RBG24
                AMMediaType media;
                media = new AMMediaType();
                media.majorType	= MediaType.Video;
                media.subType	= MediaSubType.RGB24;
                media.formatType = FormatType.VideoInfo;
                hr = m_isg.SetMediaType( media );
                Marshal.ThrowExceptionForHR(hr);

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
            }

            return graphBuilder;
        }

        int ISampleGrabberCB.SampleCB( double SampleTime, IMediaSample pSample )
        {
            // SampleCB was called
            m_Called |= 1;

            // Simple test to see if parms are correct
            Debug.Assert(pSample.GetActualDataLength() > 0, "SampleCB size");

            // Set the completion event so DoTests can return
            m_TestComplete.Set();

            Marshal.ReleaseComObject(pSample);

            return 0;
        }

        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.BufferCB( double SampleTime, IntPtr pBuffer, int BufferLen )
        {
            // BufferCB was called
            m_Called |= 2;

            // Simple test to see if parms are correct
            Debug.Assert(BufferLen > 0);

            // Set the completion event so DoTests can return
            m_TestComplete.Set();

            return 0;
        }
    }
}