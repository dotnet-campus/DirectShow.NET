using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Summary description for IAMTimelineTrackTest.
	/// </summary>
	public class IAMTimelineTrackTest
	{
		private IAMTimeline m_pTimeline;
		private IAMTimelineTrack m_pTrack;
		private IAMTimelineObj m_pSourceObj1;
		private IAMTimelineObj m_pSourceObj2;

		public IAMTimelineTrackTest()
		{
		}

		public void DoTests()
		{
			InitTest();
			TestAreYouBlank();
			TestSrcAdd();
			TestGetSourcesCount();
			TestGetNextSrc();
			TestGetNextSrc2();
			TestGetNextSrcEx();
			TestGetSourceAtTime();
			TestGetSourceAtTime2();
			TestInsertSpace();
			TestInsertSpace2();
			TestZeroBetween();
			TestZeroBetween2();
			TestMoveEverythingByx2();
		}

		private void InitTest()
		{
			m_pTimeline = (IAMTimeline)new AMTimeline();
		}

		private void TestAreYouBlank() //def ok
		{
			bool ret = false;
			int hr = 0;

			IAMTimelineObj trackobj;
			hr = m_pTimeline.CreateEmptyNode(out trackobj, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			m_pTrack = (IAMTimelineTrack)trackobj;
			hr = m_pTrack.AreYouBlank(out ret);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(ret == true, "AreYouBlank");
		}
		private void TestSrcAdd()
		{
			int hr = 0;
			hr = m_pTimeline.CreateEmptyNode( out m_pSourceObj1, TimelineMajorType.Source);
			DESError.ThrowExceptionForHR(hr);
			m_pSourceObj1.SetStartStop(0, 100000000);

			hr = m_pTimeline.CreateEmptyNode( out m_pSourceObj2, TimelineMajorType.Source);
			DESError.ThrowExceptionForHR(hr);
			m_pSourceObj2.SetStartStop(100000000, 200000000);

			IAMTimelineSrc tlsrc = (IAMTimelineSrc)m_pSourceObj1;		

			hr = tlsrc.SetMediaName("foo.avi");
			tlsrc.SetMediaTimes(0, 100000000);
			DESError.ThrowExceptionForHR(hr);

			hr = m_pTrack.SrcAdd(m_pSourceObj1);
			DESError.ThrowExceptionForHR(hr);

			tlsrc = (IAMTimelineSrc)m_pSourceObj2;		
			hr = tlsrc.SetMediaName("foo.avi");
			tlsrc.SetMediaTimes(100000000, 200000000);
			DESError.ThrowExceptionForHR(hr);

			hr = m_pTrack.SrcAdd(m_pSourceObj2);
			DESError.ThrowExceptionForHR(hr);

			
			bool ret = true;
			m_pTrack.AreYouBlank(out ret);

			Debug.Assert(ret == false, "SrcAdd");

		}

		private void TestGetSourcesCount()
		{
			int sources = 0;
			int hr = m_pTrack.GetSourcesCount(out sources);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(sources == 2, "GetSourcesCount");
		}

		private void TestGetNextSrc()
		{
			long start = 0;
			IAMTimelineObj tlobj;
			int hr = m_pTrack.GetNextSrc(out tlobj, ref start);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj1, "GetNextSrc");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;

			start = 100000000;
			hr = m_pTrack.GetNextSrc(out tlobj, ref start);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj2, "GetNextSrc second source");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;

		}

		private void TestGetNextSrc2()
		{
			double start = 0;
			IAMTimelineObj tlobj;
			int hr = m_pTrack.GetNextSrc2(out tlobj, ref start);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj1, "GetNextSrc2");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;

			start = 10;
			hr = m_pTrack.GetNextSrc2(out tlobj, ref start);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj2, "GetNextSrc2 second source");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;
		}

		private void TestGetNextSrcEx()
		{
			IAMTimelineObj tlobj;
			int hr = m_pTrack.GetNextSrcEx(null, out tlobj);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj1, "GetNextSrcEx");

			Marshal.ReleaseComObject(tlobj);
			tlobj = null;

			hr = m_pTrack.GetNextSrcEx(m_pSourceObj1, out tlobj);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj2, "GetNextSrcEx");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;
		}

		private void TestGetSourceAtTime()
		{
			IAMTimelineObj tlobj;
			int hr = m_pTrack.GetSrcAtTime(out tlobj, 0, DexterFTrackSearchFlags.ExactlyAt);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj1, "GetSourceAtTime");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;
		
			hr = m_pTrack.GetSrcAtTime(out tlobj, 45898498, DexterFTrackSearchFlags.Forwards);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj2, "GetSourceAtTime");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;
		}

		private void TestGetSourceAtTime2()
		{
			IAMTimelineObj tlobj;
			int hr = m_pTrack.GetSrcAtTime2(out tlobj, 0, DexterFTrackSearchFlags.ExactlyAt);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj1, "GetSourceAtTime2");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;
		
			hr = m_pTrack.GetSrcAtTime2(out tlobj, 4.5898498, DexterFTrackSearchFlags.Forwards);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(tlobj == m_pSourceObj2, "GetSourceAtTime2");
			Marshal.ReleaseComObject(tlobj);
			tlobj = null;
		}

		private void TestInsertSpace()
		{
			long spacestart = 45898498;
			long spacestop = 50000000 + spacestart;
			int hr = m_pTrack.InsertSpace(spacestart, spacestop);
			DESError.ThrowExceptionForHR(hr);

			long start = 0, stop = 0;
			hr = m_pSourceObj1.GetStartStop(out start, out stop);
			DESError.ThrowExceptionForHR(hr);

			long objstop = 45898498;
			Debug.Assert(stop == objstop, "InsertSpace");
		}

		private void TestInsertSpace2()
		{
			double spacestart = 3.23;
			double spacestop = spacestart + 5;

			int hr = m_pTrack.InsertSpace2(spacestart, spacestop);
			DESError.ThrowExceptionForHR(hr);

			double start = 0, stop = 0;
			hr = m_pSourceObj1.GetStartStop2(out start, out stop);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(stop == spacestart, "TestInsertSpace2");


		}

		//Seems to work - not supported
		private void TestMoveEverythingByx2()
		{
			//using negative values produces inacurate results ...
			int hr = m_pTrack.MoveEverythingBy2(0.0F, 1.0F);
			DESError.ThrowExceptionForHR(hr);

			hr = m_pTrack.MoveEverythingBy(10000000, -10000000);
			DESError.ThrowExceptionForHR(hr);
		}


		private void TestZeroBetween()
		{
			long spacestart = 8198498;
			long spacestop = 50000000 + spacestart;
			int hr = m_pTrack.ZeroBetween(spacestart, spacestop);
			DESError.ThrowExceptionForHR(hr);

			long start = 0, stop = 0;
			hr = m_pSourceObj1.GetStartStop(out start, out stop);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(start == 0, "ZeroBetween");

			long objstop = 8198498;
			Debug.Assert(stop == objstop, "ZeroBetween");
		}

		private void TestZeroBetween2()
		{
			int hr = m_pTrack.ZeroBetween2(0, 30);
			DESError.ThrowExceptionForHR(hr);

			bool blank = false;

			hr = m_pTrack.AreYouBlank(out blank);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(blank == true, "ZeroBetween2");
		}
	}
}
