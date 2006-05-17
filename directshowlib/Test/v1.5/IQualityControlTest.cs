using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
    public class IQualityControlTest : IQualityControl
    {
        IQualityControl m_qc;
        IMediaControl m_imc;
        bool m_bGotOne = false;

        public void DoTests()
        {
            Setup();

            TestEm();

            Marshal.ReleaseComObject(m_imc);
            Marshal.ReleaseComObject(m_qc);
        }

        private void TestEm()
        {
            int hr;

            hr = m_qc.SetSink(this);
            DsError.ThrowExceptionForHR(hr);

            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);

            System.Windows.Forms.MessageBox.Show("Wait a few seconds");
            Debug.Assert(m_bGotOne, "Didn't get one");
        }

        private void Setup()
        {
            int hr;
            IBaseFilter ibf;

            IFilterGraph2 ifg = new FilterGraph() as IFilterGraph2;
            m_imc = ifg as IMediaControl;
            DsROTEntry rot = new DsROTEntry(ifg);

            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            hr = ifg.AddSourceFilterForMoniker(devs[0].Mon, null, devs[0].Name, out ibf);

            ICaptureGraphBuilder2 icgb2 = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = icgb2.SetFiltergraph(ifg);
            DsError.ThrowExceptionForHR(hr);

            hr = icgb2.RenderStream(null, null, ibf, null, null);
            DsError.ThrowExceptionForHR(hr);

            IBaseFilter pFilter;
            hr = ifg.FindFilterByName("Video Renderer", out pFilter);

            m_qc = pFilter as IQualityControl;

            rot.Dispose();
        }

        #region IQualityControl Members

        int IQualityControl.Notify(IBaseFilter pSelf, Quality q)
        {
            m_bGotOne = true;

            Debug.Assert(pSelf != null, "IBaseFilter");
            Debug.Assert(q.Type == QualityMessageType.Famine);
            return 0;
        }

        int IQualityControl.SetSink(IQualityControl piqc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
