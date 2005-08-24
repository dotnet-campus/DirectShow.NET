using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Threading;

using DirectShowLib;
using DirectShowLib.SBE;

namespace DirectShowLib.Test
{
    public class IAMOpenProgressTest
    {
        volatile IAMOpenProgress m_iop;

        bool m_bQuery = false;
        bool m_bAbort = false;
        bool m_bLoaded = false;

        public IAMOpenProgressTest()
        {
        }

        public void DoTests()
        {
            Configure();
            Configure2();

            Debug.Assert(m_bAbort, "Abort");
            Debug.Assert(m_bQuery, "Query");
        }

        public void ThreadProc()
        {
            int hr;

            System.Threading.Thread.Sleep(500);

            hr = m_iop.AbortOperation();
            DsError.ThrowExceptionForHR(hr);
        }

        public void ThreadProc2()
        {
            int hr;
            long lTot, lCur;
            lTot = 3;
            lCur = 4;

            do
            {
                System.Threading.Thread.Sleep(10);
            } while (!m_bLoaded);

            System.Threading.Thread.Sleep(500);

            lTot = 3;
            lCur = 4;
            hr = m_iop.QueryProgress(out lTot, out lCur);

            m_bQuery = (lTot > 5 && lCur > 5);
        }

        private void Configure()
        {
            int hr;

            IFilterGraph2 filterGraph = (IFilterGraph2) new FilterGraph();

            URLReader u = new URLReader();
            m_iop = (IAMOpenProgress)u;
            IFileSourceFilter fsf = (IFileSourceFilter)u;
            IFileSourceFilter fsf2 = (IFileSourceFilter)u;

            hr = filterGraph.AddFilter((IBaseFilter)m_iop, "url");
            DsError.ThrowExceptionForHR(hr);

            ThreadStart o2 = new ThreadStart(this.ThreadProc);
            Thread thread;
            thread = new Thread( o2 );
            thread.Name="cancellor";
            thread.Start();

            hr = fsf.Load(@"http://192.168.1.77/DShow/foo.avi", null);

            m_bAbort = (hr == -2147467260); // Aborted

            Marshal.ReleaseComObject(u);
            Marshal.ReleaseComObject(filterGraph);
        }

        private void Configure2()
        {
            int hr;

            IFilterGraph2 filterGraph = (IFilterGraph2) new FilterGraph();

            URLReader u = new URLReader();
            m_iop = (IAMOpenProgress)u;

            IFileSourceFilter fsf = (IFileSourceFilter)u;

            hr = filterGraph.AddFilter((IBaseFilter)m_iop, "url");
            DsError.ThrowExceptionForHR(hr);

            ThreadStart o2 = new ThreadStart(this.ThreadProc2);
            Thread thread;
            thread = new Thread( o2 );
            thread.Name="cancellor2";
            thread.Start();

            hr = fsf.Load(@"http://www.LimeGreenSocks.com/test.avi", null);
            DsError.ThrowExceptionForHR(hr); // -2147467260

            m_bLoaded = true;

            ICaptureGraphBuilder2 icgb = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            hr = icgb.SetFiltergraph(filterGraph);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb.RenderStream(null, null, u, null, null);

            ((IMediaControl)filterGraph).Run();

            while (!m_bQuery)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }
    }
}
