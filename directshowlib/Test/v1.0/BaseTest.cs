// $Id: BaseTest.cs,v 1.1 2005-05-26 23:11:07 kawaic Exp $
// $Author: kawaic $
// $Revision: 1.1 $
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	/// <summary>
	/// BaseTestClass
	/// </summary>
	public class BaseTest
	{
		public BaseTest()
		{
		}

		public IFilterGraph2 BuildAviGraph(string sFileName)
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