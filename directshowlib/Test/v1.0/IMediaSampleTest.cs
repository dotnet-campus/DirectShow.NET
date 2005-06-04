// Changed SetTime and SetMediaTime to use DsOptInt64
// Added comment to GetMediaType re DsUtils.FreeAMMediaType()

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IMediaSampleTest : ISampleGrabberCB
    {
        private const string g_TestFile = @"foo.avi";

        private IFilterGraph2 m_graphBuilder;

        // The IMediaSample to test
        private IMediaSample m_ims;

        // Avoid having a second sample overwrite the first
        private volatile bool m_GotOne = false;

        // Get the media type off the pin to use later
        private AMMediaType m_MediaType = null;

        // Since the tests are done in the callback, this is
        // used to signal when the tests are complete
        private ManualResetEvent m_TestComplete;

        public IMediaSampleTest()
        {
        }

        /// <summary>
        /// Test all IMediaSampleTest methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            m_graphBuilder = BuildGraph(g_TestFile);

            // All the tests are called in ISampleGrabberCB.SampleCB, since
            // that's where we are when we get the IMediaSample
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

        void TestLength()
        {
            int hr;
            int iSize, iLen, iLen2;

            iSize = m_ims.GetSize();
            iLen = m_ims.GetActualDataLength();

            // Since we aren't using compression, size and datalen should
            // be the same.
            Debug.Assert(iSize == iLen, "GetSize, GetActualDatalen");

            // Change the datalen
		    hr = m_ims.SetActualDataLength(iLen - 1);
            Marshal.ThrowExceptionForHR(hr);

            // Make sure the change, changed
            iLen2 = m_ims.GetActualDataLength();
            Debug.Assert(iLen - 1 == iLen2, "Get/Set ActualDataLen");
        }

        void TestSyncPoint()
        {
            int hr;
            int s1, s2;

            // Read the value
            s1 = m_ims.IsSyncPoint();

            // Change the value
            hr = m_ims.SetSyncPoint(s1 != 0);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            s2 = m_ims.IsSyncPoint();

            // Check to see if the change took
            Debug.Assert(s1 != s2, "Get/Set SyncPoint");

            // put the original value back
            hr = m_ims.SetSyncPoint(s1 == 0);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            s2 = m_ims.IsSyncPoint();

            // Check to see if the change took
            Debug.Assert(s1 == s2, "Get/Set SyncPoint2");
        }

        void TestPreroll()
        {
            int hr;
            int s1, s2;

            // Read the value
            s1 = m_ims.IsPreroll();

            // Change the value
            hr = m_ims.SetPreroll(s1 != 0);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            s2 = m_ims.IsPreroll();

            // Check to see if the change took
            Debug.Assert(s1 != s2, "Get/Set Preroll");

            // put the original value back
            hr = m_ims.SetPreroll(s1 == 0);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            s2 = m_ims.IsPreroll();

            // Check to see if the change took
            Debug.Assert(s1 == s2, "Get/Set Preroll2");
        }

        void TestDiscontinuity()
        {
            int hr;
            int s1, s2;

            // Read the value
            s1 = m_ims.IsDiscontinuity();

            // Change the value
            hr = m_ims.SetDiscontinuity(s1 != 0);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            s2 = m_ims.IsDiscontinuity();

            // Check to see if the change took
            Debug.Assert(s1 != s2, "Get/Set Discontinuity");

            // put the original value back
            hr = m_ims.SetDiscontinuity(s1 == 0);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            s2 = m_ims.IsDiscontinuity();

            // Check to see if the change took
            Debug.Assert(s1 == s2, "Get/Set Discontinuity2");
        }

        void TestTime()
        {
            int hr;
            long TimeStart1, TimeEnd1;
            long TimeStart2, TimeEnd2;

            // Read the value
            hr = m_ims.GetTime(out TimeStart1, out TimeEnd1);
            Marshal.ThrowExceptionForHR(hr);

            // Change the value
            hr = m_ims.SetTime(new DsLong(TimeStart1 + 1), new DsLong(TimeEnd1 - 1));
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            hr = m_ims.GetTime(out TimeStart2, out TimeEnd2);
            Marshal.ThrowExceptionForHR(hr);

            // Check to see if the change took
            Debug.Assert(TimeStart1 + 1 == TimeStart2, "Get/Set time start");
            Debug.Assert(TimeEnd1 - 1 == TimeEnd2, "Get/Set time End");

            // Try using nulls
            hr = m_ims.SetTime(null, null);
            Marshal.ThrowExceptionForHR(hr);

            // Read the time, should fail (because of the nulls)
            hr = m_ims.GetTime(out TimeStart1, out TimeEnd1);
            Debug.Assert(hr == DsResults.E_SampleTimeNotSet, "Get/Set time null");

            // Put it back to where it started
            hr = m_ims.SetTime(new DsLong(TimeStart1), new DsLong(TimeEnd1));
            Marshal.ThrowExceptionForHR(hr);
        }

        void TestMediaTime()
        {
            int hr;
            long TimeStart1, TimeEnd1;
            long TimeStart2, TimeEnd2;

            // Read the value
            hr = m_ims.GetMediaTime(out TimeStart1, out TimeEnd1);
            Marshal.ThrowExceptionForHR(hr);

            // Change the value
            hr = m_ims.SetMediaTime(new DsLong(TimeStart1 + 1), new DsLong(TimeEnd1 - 1));
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            hr = m_ims.GetMediaTime(out TimeStart2, out TimeEnd2);
            Marshal.ThrowExceptionForHR(hr);

            // Check to see if the change took
            Debug.Assert(TimeStart1 + 1 == TimeStart2, "Get/Set MediaTime start");
            Debug.Assert(TimeEnd1 - 1 == TimeEnd2, "Get/Set MediaTime End");

            // Try using nulls
            hr = m_ims.SetMediaTime(null, null);
            Marshal.ThrowExceptionForHR(hr);

            // Read the time, should fail (because of the nulls)
            hr = m_ims.GetMediaTime(out TimeStart1, out TimeEnd1);
            Debug.Assert(hr == DsResults.E_MediaTimeNotSet, "Get/Set MediaTime null");

            // Put it back to where it started
            hr = m_ims.SetMediaTime(new DsLong(TimeStart1), new DsLong(TimeEnd1));
            Marshal.ThrowExceptionForHR(hr);
        }

        void TestMediaType()
        {
            int hr;
            AMMediaType mt1 = new AMMediaType();

            // Set the media type to the original value we 
            // got off the pin.  If we don't do this, the GetMediatype
            // will just return null, indicating that the mediatype
            // hasn't changed.
            hr = m_ims.SetMediaType(m_MediaType);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the value
            hr = m_ims.GetMediaType(out mt1);
            Marshal.ThrowExceptionForHR(hr);

            // Check to see if it re-read correctly.
            Debug.Assert(m_MediaType.majorType == mt1.majorType, "Major type");
            Debug.Assert(m_MediaType.subType == mt1.subType, "Sub type");

            DsUtils.FreeAMMediaType(mt1);
            mt1 = null;
        }

        void TestPointer()
        {
            int hr;
            IntPtr ppBuffer;

            // No particular test, just see if it will read.
            hr = m_ims.GetPointer(out ppBuffer);
            Marshal.ThrowExceptionForHR(hr);

            Debug.Assert(ppBuffer != IntPtr.Zero, "GetPointer");
        }

        void GetSample()
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
                m_ims = pSample;

                // Call all the tests
                TestLength();
                TestSyncPoint();
                TestPreroll();
                TestDiscontinuity();
                TestTime();
                TestMediaTime();
                TestMediaType();
                TestPointer();
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