using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IFileSinkFilter2Test
    {
        IFileSinkFilter2 m_ppsink;
        string FileName = @"c:\foo.out";

        public IFileSinkFilter2Test()
        {
        }

        [Test]
        public void DoTests()
        {
            BuildGraph();

            try
            {
                TestFileName();
                TestMode();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ppsink);
            }
        }

        void TestFileName()
        {
            int hr;
            string fn;
            AMMediaType pmt = new AMMediaType();

            hr = m_ppsink.GetCurFile(out fn, pmt);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(fn == FileName, "GetCurFile");
            Debug.Assert(pmt.majorType == MediaType.Stream, "GetCurFile type");

            hr = m_ppsink.SetFileName(@"c:\foo2.out", pmt);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ppsink.GetCurFile(out fn, pmt);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(fn == @"c:\foo2.out", "GetCurFile2");
        }

        void TestMode()
        {
            int hr;
            AMFileSinkFlags dwFlags;

            hr = m_ppsink.GetMode(out dwFlags);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dwFlags == AMFileSinkFlags.None, "TestMode");

            hr = m_ppsink.SetMode(AMFileSinkFlags.OverWrite);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ppsink.GetMode(out dwFlags);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dwFlags == AMFileSinkFlags.OverWrite, "TestMode");
        }

        void BuildGraph()
        {
            int hr;
            IBaseFilter ppbf, ppFilter;
            ICaptureGraphBuilder2 icgb2;
            IFileSinkFilter ppsink;
            ArrayList devs;

            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;
            icgb2 = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
            icgb2.SetFiltergraph(graphBuilder);

            DsROTEntry ds = new DsROTEntry(graphBuilder);
            IFilterGraph2 ifg2 = graphBuilder as IFilterGraph2;

            devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = (DsDevice)devs[0];

            hr = ifg2.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out ppFilter);
            DsError.ThrowExceptionForHR(hr);

            // Get a ICaptureGraphBuilder2
            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
            hr = icgb.SetFiltergraph( (IGraphBuilder)graphBuilder );
            DsError.ThrowExceptionForHR(hr);

            // Use the ICaptureGraphBuilder2 to add the avi mux
            hr = icgb.SetOutputFileName(MediaSubType.Avi, FileName, out ppbf, out ppsink);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb2.RenderStream(PinCategory.Capture, MediaType.Video, ppFilter, null, ppbf);
            DsError.ThrowExceptionForHR(hr);

            m_ppsink = ppsink as IFileSinkFilter2;
        }
    }
}