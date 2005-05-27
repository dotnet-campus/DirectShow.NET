// While I really wanted to find some alternative for the "ref" in put_Interleaving,
// DsOptInt64 was ugly, and custom marshaling only works with classes.

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IConfigInterleavingTest
    {
        IConfigInterleaving m_ici;

        public IConfigInterleavingTest()
        {
        }

        [Test]
        public void DoTests()
        {
            BuildGraph();

            try
            {
                InterleavingTest();
                ModeTest();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ici);
            }
        }

        void InterleavingTest()
        {
            int hr;
            long il, pr;

            hr = m_ici.get_Interleaving(out il, out pr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pr == 0, "Interleaving pr");
            Debug.Assert(il == 10000000, "Interleaving il");

            il ++;
            pr ++;

            hr = m_ici.put_Interleaving(ref il, ref pr);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ici.get_Interleaving(out il, out pr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pr == 1, "Interleaving pr2");
            Debug.Assert(il == 10000001, "Interleaving il2");
        }

        void ModeTest()
        {
            int hr;
            InterleavingMode pMode;

            hr = m_ici.get_Mode(out pMode);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pMode == InterleavingMode.None, "ModeTest");

            hr = m_ici.put_Mode(InterleavingMode.Full);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ici.get_Mode(out pMode);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pMode == InterleavingMode.Full, "ModeTest2");
        }

        void BuildGraph()
        {
            int hr;
            IBaseFilter ppbf;
            IFileSinkFilter ppsink;

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
            m_ici = ppbf as IConfigInterleaving;
        }
    }
}
