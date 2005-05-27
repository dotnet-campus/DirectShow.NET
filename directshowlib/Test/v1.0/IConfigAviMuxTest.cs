// IConfigAviMux.GetOutputCompatibilityIndex -> out bool

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IConfigAviMuxTest
    {
        IConfigAviMux m_icam;

        public IConfigAviMuxTest()
        {
        }

        [Test]
        public void DoTests()
        {
            BuildGraph();

            try
            {
                TestMasterStream();
                TestCompatibility();
            }
            finally
            {
                Marshal.ReleaseComObject(m_icam);
            }
        }

        public void TestMasterStream()
        {
            int hr;
            int pstream;

            hr = m_icam.GetMasterStream(out pstream);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pstream == -1, "GetMasterStream");

            hr = m_icam.SetMasterStream(6);
            DsError.ThrowExceptionForHR(hr);

            hr = m_icam.GetMasterStream(out pstream);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pstream == 6, "GetMasterStream2");
        }

        void TestCompatibility()
        {
            int hr;
            bool bcompat;

            hr = m_icam.GetOutputCompatibilityIndex(out bcompat);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(bcompat == true, "TestCompatibility");

            hr = m_icam.SetOutputCompatibilityIndex(false);
            DsError.ThrowExceptionForHR(hr);

            hr = m_icam.GetOutputCompatibilityIndex(out bcompat);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(bcompat == false, "GetMasterStream2");
        }

        void BuildGraph()
        {
            int hr;
            IFileSinkFilter ppsink;
            IBaseFilter ppbf;

            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;

            DsROTEntry ds = new DsROTEntry(graphBuilder);

            // Get a ICaptureGraphBuilder2
            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();
            hr = icgb.SetFiltergraph( (IGraphBuilder)graphBuilder );
            DsError.ThrowExceptionForHR(hr);

            // Use the ICaptureGraphBuilder2 to add the avi mux
            hr = icgb.SetOutputFileName(MediaSubType.Avi, @"c:\foo.out", out ppbf, out ppsink);
            DsError.ThrowExceptionForHR(hr);

            // Save off the IConfigAviMux
            m_icam = ppbf as IConfigAviMux;
        }
    }
}