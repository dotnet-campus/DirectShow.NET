// $Id: IMediaControlTest.cs,v 1.1 2005-05-26 23:11:07 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.1 $
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
	}
}