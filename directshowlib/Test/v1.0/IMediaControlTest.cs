// $Id: IMediaControlTest.cs,v 1.2 2005-06-01 18:53:14 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.2 $
using System.Runtime.InteropServices;
using DirectShowLib;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	/// <summary>
	/// Summary description for IMediaControlTest.
	/// </summary>
	[TestFixture]
	public class IMediaControlTest : BaseTest
	{
		private const string testfile = @"foo.avi";

		[Test]
		public void TestRunStop()
		{
			IFilterGraph2 filterGraph = BuildAviGraph(testfile);
			Assert.IsNotNull(filterGraph);

			IMediaControl mediaCtrl = filterGraph as IMediaControl;
			int hr;

			hr = mediaCtrl.Run();
			DsError.ThrowExceptionForHR(hr);
			hr = mediaCtrl.Stop();
			DsError.ThrowExceptionForHR(hr);

			Marshal.ReleaseComObject(filterGraph);
		}

		public void TestPause()
		{
			IFilterGraph2 filterGraph = BuildAviGraph(testfile);
			Assert.IsNotNull(filterGraph);

			IMediaControl mediaCtrl = filterGraph as IMediaControl;
			int hr;

			hr = mediaCtrl.Run();
			DsError.ThrowExceptionForHR(hr);
			hr = mediaCtrl.Pause();
			DsError.ThrowExceptionForHR(hr);

			Marshal.ReleaseComObject(filterGraph);
		}

		public void TestStopWhenReady()
		{
			IFilterGraph2 filterGraph = BuildAviGraph(testfile);
			Assert.IsNotNull(filterGraph);

			IMediaControl mediaCtrl = filterGraph as IMediaControl;
			int hr;

			hr = mediaCtrl.Run();
			DsError.ThrowExceptionForHR(hr);
			hr = mediaCtrl.StopWhenReady();
			DsError.ThrowExceptionForHR(hr);

			Marshal.ReleaseComObject(filterGraph);
		}

		public void TestRenderFile()
		{
			IFilterGraph2 filterGraph = BuildAviGraph(testfile);
			Assert.IsNotNull(filterGraph);

			IMediaControl mediaCtrl = filterGraph as IMediaControl;
			int hr;

			hr = mediaCtrl.RenderFile(testfile);
			DsError.ThrowExceptionForHR(hr);
		}

		public void TestGetState()
		{
			IFilterGraph2 filterGraph = BuildAviGraph(testfile);
			Assert.IsNotNull(filterGraph);

			IMediaControl mediaCtrl = filterGraph as IMediaControl;
			int hr;

			FilterState fs;
			hr = mediaCtrl.GetState(1000, out fs);
			DsError.ThrowExceptionForHR(hr);
			Assert.IsNotNull(fs);

		}

		public void TestGetFilterCollectionAndAddSourceFilter()
		{
			IFilterGraph2 filterGraph = BuildAviGraph(testfile);
			Assert.IsNotNull(filterGraph);

			IMediaControl mediaCtrl = filterGraph as IMediaControl;
			int hr;

			
			//hr = mediaCtrl.get_FilterCollection()
		}
	}
}