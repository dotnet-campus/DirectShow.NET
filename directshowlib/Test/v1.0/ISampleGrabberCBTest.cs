using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class ISampleGrabberCBTest : ISampleGrabberCB
    {
        private const string g_TestFile = @"foo.avi";

        private IFilterGraph2 m_graphBuilder;

        // This is used to signal when the callbacks are called
        private ManualResetEvent m_TestComplete;

        // Samplegrabber interface used to choose which callback to use
        private ISampleGrabber m_isg;

        // Used to store which callbacks have been called
        private int m_Called;

        public ISampleGrabberCBTest()
        {
        }

        /// <summary>
        /// Test all ISampleGrabberCBTest methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            m_graphBuilder = BuildGraph(g_TestFile);

            GetSample();

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
        }

        void GetSample()
        {
            int hr;
            bool bDone;
            IMediaControl imc = m_graphBuilder as IMediaControl;

            // Use this to find out when the tests are finished
            m_TestComplete = new ManualResetEvent(false);

            // Bitmap holding which callbacks were called
            m_Called = 0;

            for (int y=0; y < 2; y++)
            {
                m_TestComplete.Reset();

                // Call back on the specified routine
                hr = m_isg.SetCallback( this, y );
                Marshal.ThrowExceptionForHR( hr );

                // Start running the graph
                hr = imc.Run();
                Marshal.ThrowExceptionForHR(hr);

                // When the event is signaled, the test is done
                bDone = m_TestComplete.WaitOne(10000, false);

                // If we timed out, we failed
                Debug.Assert(bDone, "Failed to get even one sample");

                // Stop running the graph
                hr = imc.Stop();
                Marshal.ThrowExceptionForHR(hr);
            }

            Debug.Assert(m_Called == 3, "Called both");

            // Clean up
            m_TestComplete.Close();
            m_TestComplete = null;
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

                IPinOut = DsGetPin.ByDirection(ibfAVISource, PinDirection.Output);

                // Get a SampleGrabber
                sg = new SampleGrabber();
                IBaseFilter grabFilt = sg as IBaseFilter;
                m_isg = sg as ISampleGrabber;

                // Add the sample grabber to the graph
                hr = graphBuilder.AddFilter(grabFilt, "Ds.NET SampleGrabber");
                Marshal.ThrowExceptionForHR(hr);

                iSampleIn = DsGetPin.ByDirection(grabFilt, PinDirection.Input);
                iSampleOut = DsGetPin.ByDirection(grabFilt, PinDirection.Output);

                // Get the default video renderer
                ibfRenderer = (IBaseFilter) new VideoRendererDefault();

                // Add it to the graph
                hr = graphBuilder.AddFilter(ibfRenderer, "Ds.NET VideoRendererDefault");
                Marshal.ThrowExceptionForHR(hr);
                IPinIn = DsGetPin.ByDirection(ibfRenderer, PinDirection.Input);

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