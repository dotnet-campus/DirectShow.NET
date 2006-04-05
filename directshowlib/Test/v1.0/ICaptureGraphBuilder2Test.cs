// FindInterface -> DsGuid
// RenderStream -> DsGuid
// ControlStream -> DsGuid, UnmanagedType.Interface, DsLong
// CopyCaptureFile -> bool
// FindPin -> UnmanagedType.Interface, DsGuid, UnmanagedType.Interface

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    class MyCopyProg : IAMCopyCaptureFileProgress
    {
        public bool WasCalled = false;
        public int Progress(int iProg)
        {
            WasCalled = true;
            Debug.WriteLine(iProg);
            return 0;
        }
    }

	public class ICaptureGraphBuilder2Test
	{
        private const string g_SourceTestFile = @"foo.avi";
        private const string g_DestTestFile = @"c:\foo.avi";

		private IFilterGraph2 m_graphBuilder;
        ICaptureGraphBuilder2 m_icgb2 = null;
        private DsROTEntry m_dsrot;
        IBaseFilter m_Source = null;
        IBaseFilter m_Multiplexer = null;

		public ICaptureGraphBuilder2Test()
		{
		}

		/// <summary>
		/// Test all IBasicVideo methods
		/// </summary>
		public void DoTests()
		{
			m_graphBuilder = BuildGraph(g_SourceTestFile);

			try
			{
                TestFilterGraph();
                TestAllocCapFile();
                TestOutputFileName();
                TestFindInterface();
                TestRenderStream();
                TestFindPin();
                TestCopyFile();
                TestControlStream();
            }
			finally
			{
				if (m_graphBuilder != null)
				{
					Marshal.ReleaseComObject(m_graphBuilder);
				}

				m_graphBuilder = null;
                m_dsrot.Dispose();
			}
		}

        void TestFilterGraph()
        {
            int hr;
            IGraphBuilder fg;

            hr = m_icgb2.SetFiltergraph(m_graphBuilder);
            DsError.ThrowExceptionForHR(hr);

            hr = m_icgb2.GetFiltergraph(out fg);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m_graphBuilder == fg, "Get/Set FilterGraph");
        }

        void TestOutputFileName()
        {
            int hr;
            IFileSinkFilter ppsink = null;

            hr = m_icgb2.SetOutputFileName(MediaSubType.Avi, g_DestTestFile, out m_Multiplexer, out ppsink);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestFindInterface()
        {
            int hr;
            object comObj;

            // Try it with nulls
            hr = m_icgb2.FindInterface( null, null, m_Multiplexer, typeof(IBaseFilter).GUID, out comObj );
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert((IBaseFilter)comObj != null, "FindInterface null");

            // Try it with values
            hr = m_icgb2.FindInterface( FindDirection.DownstreamOnly, MediaType.Stream, m_Multiplexer, typeof(IBaseFilter).GUID, out comObj );
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert((IBaseFilter)comObj != null, "FindInterface");
        }

        void TestAllocCapFile()
        {
            int hr;
            const int iSize = 2000000;

            File.Delete(g_DestTestFile);

            hr = m_icgb2.AllocCapFile(g_DestTestFile, iSize);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(new FileInfo(g_DestTestFile).Length == iSize + 11, "AllocCapFile");
        }

        void TestRenderStream()
        {
            int hr;
            IBaseFilter ifilt;

            // Try using nulls
            hr = m_icgb2.RenderStream(null, null, m_Source, null, m_Multiplexer);
            DsError.ThrowExceptionForHR(hr);

            hr = m_graphBuilder.FindFilterByName("AVI Splitter", out ifilt);
            DsError.ThrowExceptionForHR(hr);

            IPin ipin = DsFindPin.ByConnectionStatus(ifilt, PinConnectedStatus.Unconnected, 0);
            Guid g = DsUtils.GetPinCategory(ipin);

            // With this setup, there is no valid value for parm1
            hr = m_icgb2.RenderStream(null, MediaType.Audio, m_Source, null, m_Multiplexer);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestFindPin()
        {
            int hr;
            IPin ppin;

            // Try using nulls
            hr = m_icgb2.FindPin(m_Multiplexer, PinDirection.Input, null, null, true, 0, out ppin);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(ppin != null, "FindPin");

            // With this setup, there is no valid value for parm3
            hr = m_icgb2.FindPin(m_Multiplexer, PinDirection.Output, null, MediaType.Stream, false, 0, out ppin);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(ppin != null, "FindPin2");
        }

        void TestCopyFile()
        {
            int hr;
            const string tempfile = @"c:\moo.avi";
            MyCopyProg mcp = new MyCopyProg();
            IAMCopyCaptureFileProgress iccfp = mcp as IAMCopyCaptureFileProgress;
            IMediaControl imc = m_graphBuilder as IMediaControl;

            File.Delete(tempfile);

            // Run the graph, produce the output file
            hr = imc.Run();
            DsError.ThrowExceptionForHR(hr);

            // Wait for the play to finish
            Thread.Sleep(1000);

            hr = imc.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_icgb2.CopyCaptureFile(g_DestTestFile, tempfile, true, iccfp);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(File.Exists(tempfile), "CopyCaptureFile");
            File.Delete(tempfile);

            Debug.Assert(mcp.WasCalled, "IAMCopyCaptureFileProgress");
        }

        void TestControlStream()
        {
            int hr;

            hr = m_icgb2.ControlStream(PinCategory.Preview, null, null, null, null, 0, 0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_icgb2.ControlStream(PinCategory.Preview, MediaType.Stream, null, new DsLong(1), new DsLong(2), 3, 40);
            DsError.ThrowExceptionForHR(hr);
        }

        private IFilterGraph2 BuildGraph(string sFileName)
		{
			int hr;

			IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;
            m_dsrot = new DsROTEntry(graphBuilder);

			try
			{
				// Get the file source filter
				m_Source = new AsyncReader() as IBaseFilter;

				// Add it to the graph
				hr = graphBuilder.AddFilter(m_Source, "Ds.NET AsyncReader");
				Marshal.ThrowExceptionForHR(hr);

				// Set the file name
				IFileSourceFilter fsf = m_Source as IFileSourceFilter;
				hr = fsf.Load(sFileName, null);
				Marshal.ThrowExceptionForHR(hr);

                // Get the interface we are testing
                m_icgb2 = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
			}
			catch
			{
				Marshal.ReleaseComObject(graphBuilder);
				throw;
			}

			return graphBuilder;
		}
	}
}