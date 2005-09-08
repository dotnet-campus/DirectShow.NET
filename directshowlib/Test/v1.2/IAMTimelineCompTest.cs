using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;
namespace DirectShowLib.Test
{
	/// <summary>
	/// Summary description for IAMTimelineCompTest.
	/// </summary>
	/// 

	public class IAMTimelineCompTest
	{
		private IAMTimeline		m_timeline;
		
		private IAMTimelineObj	m_timelineobj, 
								m_timelinetrackobj,
								m_timelinetrackobj2;

		private IAMTimelineComp m_timelinecomp;

		public IAMTimelineCompTest()
		{
			SetupTimeline();
		}

		public void DoTests()
		{
			TestVTrackInsBefore(); //The other tests rely on this test.
			TestVTrackGetCount();
			TestGetcountOfType();
			TestGetNextVTrack();
			TestGetVTrack();
			TestVTrackSwapPriorities();
			TestGetRecursiveLayerOfType();
			TestGetRecursiveLayerOfTypeI();
		}
		public void TestGetRecursiveLayerOfTypeI()
		{
			IAMTimelineObj vtrack;
			int layer = 1;
			int hr = m_timelinecomp.GetRecursiveLayerOfTypeI(out vtrack, ref layer, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(vtrack == m_timelinetrackobj2, "TestGetRecursiveLayerOfType");
			Marshal.ReleaseComObject(vtrack);

			layer = 0;
			hr = m_timelinecomp.GetRecursiveLayerOfTypeI(out vtrack, ref layer, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(vtrack == m_timelinetrackobj, "TestGetRecursiveLayerOfType");
			Marshal.ReleaseComObject(vtrack);
		}

		public void TestGetRecursiveLayerOfType()
		{
			IAMTimelineObj vtrack;
			int hr = m_timelinecomp.GetRecursiveLayerOfType(out vtrack, 1, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(vtrack == m_timelinetrackobj2, "TestGetRecursiveLayerOfType");
			Marshal.ReleaseComObject(vtrack);

			hr = m_timelinecomp.GetRecursiveLayerOfType(out vtrack, 0, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(vtrack == m_timelinetrackobj, "TestGetRecursiveLayerOfType");
			Marshal.ReleaseComObject(vtrack);
		}

		public void TestVTrackSwapPriorities()
		{
			//Swap
			int hr = m_timelinecomp.VTrackSwapPriorities(0, 1);
			DESError.ThrowExceptionForHR(hr);

			//Check the swap
			IAMTimelineObj vtrack0, vtrack1;

			hr = m_timelinecomp.GetVTrack(out vtrack0, 0);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(m_timelinetrackobj2 == vtrack0, "TestGetVTrack");

			hr = m_timelinecomp.GetVTrack(out vtrack1, 1);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(m_timelinetrackobj == vtrack1, "TestGetVTrack");

			Marshal.ReleaseComObject(vtrack0);
			Marshal.ReleaseComObject(vtrack1);

			//Swap back
			m_timelinecomp.VTrackSwapPriorities(1, 0);

			//Check the tracks are back to initial priorities
			hr = m_timelinecomp.GetVTrack(out vtrack0, 0);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(m_timelinetrackobj == vtrack0, "TestGetVTrack");

			hr = m_timelinecomp.GetVTrack(out vtrack1, 1);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(m_timelinetrackobj2 == vtrack1, "TestGetVTrack");

			Marshal.ReleaseComObject(vtrack0);
			Marshal.ReleaseComObject(vtrack1);

		}

		public void TestGetNextVTrack()
		{
			IAMTimelineObj firstvirtualtrack;
			int hr = m_timelinecomp.GetNextVTrack(null, out firstvirtualtrack);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(m_timelinetrackobj == firstvirtualtrack, "TestGetNextVTrack");

			IAMTimelineObj nextvirtualtrack;
			hr = m_timelinecomp.GetNextVTrack(firstvirtualtrack, out nextvirtualtrack);
			DESError.ThrowExceptionForHR(hr);
		
			Debug.Assert(m_timelinetrackobj2 == nextvirtualtrack, "TestGetNextVTrack");

			Marshal.ReleaseComObject(firstvirtualtrack);
			Marshal.ReleaseComObject(nextvirtualtrack);
		}

		public void TestGetVTrack()
		{
			IAMTimelineObj vtrack0, vtrack1;

			int hr = m_timelinecomp.GetVTrack(out vtrack0, 0);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(m_timelinetrackobj == vtrack0, "TestGetVTrack");

			hr = m_timelinecomp.GetVTrack(out vtrack1, 1);
			DESError.ThrowExceptionForHR(hr);
			Debug.Assert(m_timelinetrackobj2 == vtrack1, "TestGetVTrack");

			Marshal.ReleaseComObject(vtrack0);
			Marshal.ReleaseComObject(vtrack1);
		}

		public void TestVTrackGetCount()
		{
			int count;
			m_timelinecomp.VTrackGetCount(out count);
			Debug.Assert(count == 2, "TestVTrackGetCount");
		}

		public void TestVTrackInsBefore()
		{
			m_timelinecomp = m_timelineobj as IAMTimelineComp;
			int hr = m_timelinecomp.VTrackInsBefore(m_timelinetrackobj, 0);
			DESError.ThrowExceptionForHR(hr);		

			hr = m_timelinecomp.VTrackInsBefore(m_timelinetrackobj2, 1);
			DESError.ThrowExceptionForHR(hr);		
		}

		public void SetupTimeline()
		{
			m_timeline = (IAMTimeline)new AMTimeline();
			int hr = m_timeline.CreateEmptyNode(out m_timelineobj, TimelineMajorType.Composite);
			DESError.ThrowExceptionForHR(hr);
			hr = m_timeline.CreateEmptyNode(out m_timelinetrackobj, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			hr = m_timeline.CreateEmptyNode(out m_timelinetrackobj2, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);
		}

		public void TestGetcountOfType()
		{
			int pval = 0;
			int pvalwithcomps = 0;

			int hr = m_timelinecomp.GetCountOfType(out pval, out pvalwithcomps, TimelineMajorType.Track);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(pvalwithcomps == 3, "TestGetcountOfType");
			Debug.Assert(pval == 2, "TestGetcountOfType");

			hr = m_timelinecomp.GetCountOfType(out pval, out pvalwithcomps, TimelineMajorType.Composite);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(pvalwithcomps == 1, "TestGetcountOfType");
			Debug.Assert(pval == 0, "TestGetcountOfType");
		
			hr = m_timelinecomp.GetCountOfType(out pval, out pvalwithcomps, TimelineMajorType.Effect);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(pvalwithcomps == 0, "TestGetcountOfType");
			Debug.Assert(pval == 0, "TestGetcountOfType");

			hr = m_timelinecomp.GetCountOfType(out pval, out pvalwithcomps, TimelineMajorType.Group);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(pvalwithcomps == 0, "TestGetcountOfType");
			Debug.Assert(pval == 0, "TestGetcountOfType");

			hr = m_timelinecomp.GetCountOfType(out pval, out pvalwithcomps, TimelineMajorType.Source);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(pvalwithcomps == 0, "TestGetcountOfType");
			Debug.Assert(pval == 0, "TestGetcountOfType");

			hr = m_timelinecomp.GetCountOfType(out pval, out pvalwithcomps, TimelineMajorType.Transition);
			DESError.ThrowExceptionForHR(hr);

			Debug.Assert(pvalwithcomps == 0, "TestGetcountOfType");
			Debug.Assert(pval == 0, "TestGetcountOfType");

		}
	}
}
