// changed GetVideoPaletteEntries to use int []

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	[TestFixture]
	public class IBasicVideo2Test
	{
		private const string g_TestFile = @"foo.avi";

		private IFilterGraph2 m_graphBuilder;
		private IBasicVideo2 m_ibv;
        private DsROTEntry m_dsrot;

		public IBasicVideo2Test()
		{
		}

		/// <summary>
		/// Test all IBasicVideo methods
		/// </summary>
		[Test]
		public void DoTests()
		{
			m_graphBuilder = BuildGraph(g_TestFile);
			m_ibv = m_graphBuilder as IBasicVideo2;

			try
			{
                TestAvgTime();
                TestBitErrorRate();
                TestBitRate();
                TestVideoDim();
                TestSourceLeft();
                TestSourceTop();
                TestSourceWidth();
                TestSourceHeight();
                TestDestinationWidth();
                TestDestinationLeft();
                TestDestinationTop();
                TestDestinationHeight();
                TestSourcePos();
                TestDestinationPos();
                TestGetImage();
                TestPalette();
                TestGetPreferredAspectRatio();
            }
			finally
			{
				if (m_graphBuilder != null)
				{
					Marshal.ReleaseComObject(m_graphBuilder);
				}

				m_graphBuilder = null;
				m_ibv = null;
                m_dsrot.Dispose();
			}
		}

        void TestAvgTime()
        {
            int hr;
            double tpf;

            hr = m_ibv.get_AvgTimePerFrame(out tpf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(tpf > 0, "get_AvgTimePerFrame");
        }

        void TestBitErrorRate()
        {
            int hr;
            int br;

            hr = m_ibv.get_BitErrorRate(out br);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(br == 0, "get_BitErrorRate");
        }

        void TestBitRate()
        {
            int hr;
            int br;

            hr = m_ibv.get_BitRate(out br);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(br == 0, "get_BitRate");
        }

        void TestVideoDim()
        {
            int hr;
            int vw, vh;

            hr = m_ibv.get_VideoWidth(out vw);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vw == 320, "get_VideoWidth");

            hr = m_ibv.get_VideoHeight(out vh);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vh == 240, "get_VideoHeight");

            hr = m_ibv.GetVideoSize(out vw, out vh);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vw == 320, "get_VideoWidth");
            Debug.Assert(vh == 240, "get_VideoHeight");
        }

        void TestDestinationLeft()
        {
            int hr;
            int sl;

            hr = m_ibv.put_DestinationLeft(4);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_DestinationLeft(out sl);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sl == 4, "DestinationLeft");
        }

        void TestDestinationTop()
        {
            int hr;
            int st;

            hr = m_ibv.put_DestinationTop(4);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_DestinationTop(out st);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(st == 4, "DestinationTop");
        }

        void TestDestinationWidth()
        {
            int hr;
            int sw;

            hr = m_ibv.IsUsingDefaultDestination();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr == 0, "IsUsingDefaultDestination");

            hr = m_ibv.put_DestinationWidth(200);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_DestinationWidth(out sw);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sw == 200, "DestinationWidth");

            hr = m_ibv.IsUsingDefaultDestination();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr == 1, "IsUsingDefaultDestination2");

            hr = m_ibv.SetDefaultDestinationPosition();
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_DestinationWidth(out sw);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sw == 320, "DestinationWidth2");
        }

        void TestDestinationHeight()
        {
            int hr;
            int sh;

            hr = m_ibv.put_DestinationHeight(210);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_DestinationHeight(out sh);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sh == 210, "DestinationHeight");
        }

        void TestSourceLeft()
        {
            int hr=0;
            int sl;

            hr = m_ibv.put_SourceLeft(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_SourceLeft(out sl);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sl == 0, "SourceLeft");
        }

        void TestSourceTop()
        {
            int hr;
            int st;

            hr = m_ibv.put_SourceTop(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_SourceTop(out st);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(st == 0, "SourceTop");
        }

        void TestSourceWidth()
        {
            int hr;
            int sw;

            hr = m_ibv.IsUsingDefaultSource();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr == 0, "IsUsingDefaultSource");

            hr = m_ibv.put_SourceWidth(200);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_SourceWidth(out sw);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sw == 200, "SourceWidth");

            hr = m_ibv.IsUsingDefaultSource();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(hr == 1, "IsUsingDefaultSource2");

            hr = m_ibv.SetDefaultSourcePosition();
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_SourceWidth(out sw);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sw == 320, "SourceWidth2");
        }

        void TestSourceHeight()
        {
            int hr;
            int sh;

            hr = m_ibv.put_SourceHeight(210);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.get_SourceHeight(out sh);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sh == 210, "SourceHeight");
        }


        void TestSourcePos()
        {
            int hr;
            int sl, st, sw, sh;

            hr = m_ibv.SetSourcePosition(4, 4, 200, 210);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.GetSourcePosition(out sl, out st, out sw, out sh);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sl == 4, "SetSourcePositionL");
            Debug.Assert(st == 4, "SetSourcePositiont");
            Debug.Assert(sw == 200, "SetSourcePositionw");
            Debug.Assert(sh == 210, "SetSourcePositionh");
        }

        void TestDestinationPos()
        {
            int hr;
            int sl, st, sw, sh;

            hr = m_ibv.SetDestinationPosition(4, 4, 200, 210);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ibv.GetDestinationPosition(out sl, out st, out sw, out sh);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sl == 4, "SetDestinationPositionL");
            Debug.Assert(st == 4, "SetDestinationPositiont");
            Debug.Assert(sw == 200, "SetDestinationPositionw");
            Debug.Assert(sh == 210, "SetDestinationPositionh");
        }

        void TestGetImage()
        {
            int hr;
            int iSize = 0;
            IntPtr ip;

            hr = m_ibv.GetCurrentImage(ref iSize, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            ip = Marshal.AllocCoTaskMem(iSize);

            hr = m_ibv.GetCurrentImage(ref iSize, ip);
            DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(ip);
        }

        void TestPalette()
        {
            int hr;
            int ret;
            int [] pal = new int[100];

            hr = m_ibv.GetVideoPaletteEntries(0, pal.Length, out ret, out pal);

            // I don't know how to get a palette to check this
            if (hr != DsResults.E_NoPaletteAvailable)
            {
                DsError.ThrowExceptionForHR(hr);
            }
        }

        void TestGetPreferredAspectRatio()
        {
            int hr;
            int x, y;

            hr = m_ibv.GetPreferredAspectRatio(out x, out y);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(x == 320, "aspectX");
            Debug.Assert(y == 240, "aspectY");
        }

		private IFilterGraph2 BuildGraph(string sFileName)
		{
			int hr;
			IBaseFilter ibfRenderer = null;
			IBaseFilter ibfAVISource = null;
			IPin IPinIn = null;
			IPin IPinOut = null;

			IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;
            m_dsrot = new DsROTEntry(graphBuilder);

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