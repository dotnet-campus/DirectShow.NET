// Change IsFormatSupported, IsUsingTimeFormat, SetTimeFormat, ConvertTimeFormat from ref to
//    MarshalAs(UnmanagedType.LPStruct) since parameters only go in.  Further this allows
//    using the read-only guids directly.
// Change ConverTypeFormat to use the GUID class instead of the Guid type. to allow for nulls
// Change SetPositions to use DsOptInt64 to allow for nulls, although I'm not really sure it's necessary

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	[TestFixture]
	public class IMediaSeekingTest
	{
		private const string g_TestFile = @"foo.avi";

		private IFilterGraph2 m_graphBuilder;
		private IMediaSeeking m_ims;

		public IMediaSeekingTest()
		{
		}

		/// <summary>
		/// Test all IMediaSeekingTest methods
		/// </summary>
		[Test]
		public void DoTests()
		{
			m_graphBuilder = BuildGraph(g_TestFile);
			m_ims = m_graphBuilder as IMediaSeeking;

			try
			{
                TestCaps();
                TestFormats();
                TestRate();
                TestPosition();
                TestPreRoll();
                TestSetPosition();
                TestConvert();
            }
			finally
			{
				if (m_graphBuilder != null)
				{
					Marshal.ReleaseComObject(m_graphBuilder);
				}

				m_graphBuilder = null;
				m_ims = null;
			}
		}

        /// <summary>
        /// Check caps related functions
        /// </summary>
        void TestCaps()
        {
            int hr;
            AMSeekingSeekingCapabilities pCapabilities, pCapabilities2;

            // Read the caps
            hr = m_ims.GetCapabilities(out pCapabilities);
            Marshal.ThrowExceptionForHR(hr);

            // Check to see if the caps just reported are all available
            // (they certainly should be!)
            pCapabilities2 = pCapabilities;
            hr = m_ims.CheckCapabilities(ref pCapabilities2);
            Marshal.ThrowExceptionForHR(hr);

            Debug.Assert(pCapabilities == pCapabilities2, "GetCapabilities, CheckCapabilities");
        }

        /// <summary>
        /// Check Format related functions
        /// </summary>
        void TestFormats()
        {
            int hr;
            Guid pFormat;

            // Query to see what the preferred time format is
            hr = m_ims.QueryPreferredFormat(out pFormat);
            Marshal.ThrowExceptionForHR(hr);

            // Is the preferred format supported?  Certainly hope so.
            hr = m_ims.IsFormatSupported(pFormat);
            Marshal.ThrowExceptionForHR(hr);

            // Might return and S_ code
            Debug.Assert(hr == 0);

            // Read the current time format
            hr = m_ims.GetTimeFormat(out pFormat);
            Marshal.ThrowExceptionForHR(hr);

            // See if the current format is the one we are using.
            // Better be.
            hr = m_ims.IsUsingTimeFormat(pFormat);
            Marshal.ThrowExceptionForHR(hr);

            // Try setting the format to the current value (the
            // only one we are sure is supported).
            hr = m_ims.SetTimeFormat(pFormat);
            Marshal.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Test Get/Set Rate
        /// </summary>
        void TestRate()
        {
            int hr;
            double dRate1, dRate2;

            // Read the current play rate
            hr = m_ims.GetRate(out dRate1);
            Marshal.ThrowExceptionForHR(hr);

            // Change the rate
            hr = m_ims.SetRate(dRate1 + 1);
            Marshal.ThrowExceptionForHR(hr);

            // Re-read the rate
            hr = m_ims.GetRate(out dRate2);
            Marshal.ThrowExceptionForHR(hr);

            // See if we got the right answer
            Debug.Assert(dRate1 + 1 == dRate2, "Get/Set Rate");
        }

        /// <summary>
        /// Test the functions that read the current position
        /// </summary>
        void TestPosition()
        {
            int hr;
            long pCurrent1, pStop1, pDuration1;
            long pCurrent2, pStop2;
            long pEarliest, pLatest;

            // Read the current play position
            hr = m_ims.GetCurrentPosition(out pCurrent1);
            Marshal.ThrowExceptionForHR(hr);

            // Read the current stop position
            hr = m_ims.GetStopPosition(out pStop1);
            Marshal.ThrowExceptionForHR(hr);

            // Read the duraton (probably related to StopPosition - Position
            hr = m_ims.GetDuration(out pDuration1);
            Marshal.ThrowExceptionForHR(hr);

            // Read both current and stop positions
            hr = m_ims.GetPositions(out pCurrent2, out pStop2);
            Marshal.ThrowExceptionForHR(hr);

            // Get the cached range of values
            hr = m_ims.GetAvailable(out pEarliest, out pLatest);
            Marshal.ThrowExceptionForHR(hr);

            // Since we aren't playing, current should be 0, stop &
            // duration should be the same (the length of the clip)
            Debug.Assert(pCurrent1 == 0, "CurrentPosition");
            Debug.Assert(pStop1 == pDuration1, "Stop, Duration");
            Debug.Assert(pCurrent1 == pCurrent2, "GetPositions");
            Debug.Assert(pStop1 == pStop2, "CurrentPosition stop");
            Debug.Assert(pEarliest == pCurrent1, "CurrentPosition stop");
            Debug.Assert(pLatest == pStop2, "CurrentPosition stop");
        }

        /// <summary>
        /// Test the SetPosition function
        /// </summary>
        void TestSetPosition()
        {
            int hr;
            DsLong pc;

            pc = new DsLong(1);

            // Move the start forward by one, use null
            // for stop position
            hr = m_ims.SetPositions(
                pc,
                AMSeekingSeekingFlags.AbsolutePositioning,
                null,
                AMSeekingSeekingFlags.NoPositioning);
            Marshal.ThrowExceptionForHR(hr);

            // Try setting the stop position to 1, and null the start
            hr = m_ims.SetPositions(
                null,
                AMSeekingSeekingFlags.NoPositioning,
                pc,
                AMSeekingSeekingFlags.AbsolutePositioning);
            Marshal.ThrowExceptionForHR(hr);
        }

        /// <summary>
        /// Test the PreRoll function
        /// </summary>
        void TestPreRoll()
        {
            int hr;
            long pllPreroll;

            hr = m_ims.GetPreroll(out pllPreroll);
            //Marshal.ThrowExceptionForHR(hr);

            // E_NOTIMPL for AsyncReader
            Debug.Assert(hr == -2147467263, "GetPreRoll");
        }

        /// <summary>
        /// Test the ConvertTimeFormat function
        /// </summary>
        void TestConvert()
        {
            int hr;
            long pTarget;
            const int iPos = 12345;

            // While ConvertTimeFormat supports nulls, it appears that
            // the AsyncReader doesn't implement them correctly.  Tests against
            // my video camera show null working correctly.
            hr = m_ims.ConvertTimeFormat(
                out pTarget,
                null,
                iPos,
                TimeFormat.Byte
                );
            //Marshal.ThrowExceptionForHR(hr);

            // Anything besides TimeFormat.Byte throws an error.
            hr = m_ims.ConvertTimeFormat(
                out pTarget,
                TimeFormat.Byte,
                iPos,
                TimeFormat.Byte
                );
            Marshal.ThrowExceptionForHR(hr);

            Debug.Assert(pTarget == iPos, "ConvertTimeFormat");
        }

        private IFilterGraph2 BuildGraph(string sFileName)
		{
			int hr;
			IBaseFilter ibfRenderer = null;
			IBaseFilter ibfAVISource = null;
			IPin IPinIn = null;
			IPin IPinOut = null;

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

				// Get the default video renderer
				ibfRenderer = (IBaseFilter) new VideoRendererDefault();

				// Add it to the graph
				hr = graphBuilder.AddFilter(ibfRenderer, "Ds.NET VideoRendererDefault");
				Marshal.ThrowExceptionForHR(hr);
				IPinIn = DsFindPin.ByDirection(ibfRenderer, PinDirection.Input, 0);

				hr = graphBuilder.Connect(IPinOut, IPinIn);
				Marshal.ThrowExceptionForHR(hr);
			}
			catch
			{
				Marshal.ReleaseComObject(graphBuilder);
				throw;
			}
			finally
			{
				Marshal.ReleaseComObject(ibfAVISource);
				Marshal.ReleaseComObject(ibfRenderer);
				Marshal.ReleaseComObject(IPinIn);
				Marshal.ReleaseComObject(IPinOut);
			}

			return graphBuilder;
		}
	}
}