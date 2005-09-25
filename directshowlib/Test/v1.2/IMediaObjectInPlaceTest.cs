using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class IMediaObjectInPlaceTest
    {
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);

        IMediaObject m_imo;
        IMediaObjectInPlace m_imoip;

        public IMediaObjectInPlaceTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestLatency();
                TestClone();
                TestProcess();
            }
            finally
            {
                Marshal.ReleaseComObject(m_imo);
            }
        }

        private void TestProcess()
        {
            const int iSize = 44100;

            int hr;
            int iCurSize;
            IntPtr ip;
            AMMediaType pmt = new AMMediaType();
            DMOOutputDataBuffer [] pOutBuf = new DMOOutputDataBuffer[1];

            hr = m_imo.GetInputType(0, 0, pmt);
            hr = m_imo.SetInputType(0, pmt, DMOSetType.None);

            hr = m_imo.GetOutputType(0, 0, pmt);
            hr = m_imo.SetOutputType(0, pmt, DMOSetType.None);

            hr = m_imo.AllocateStreamingResources();
            DMOError.ThrowExceptionForHR(hr);

            DoBuff d = new DoBuff(iSize);
            d.SetLength(iSize);
            d.GetBufferAndLength(out ip, out iCurSize);

            hr = m_imoip.Process(iSize, ip, 0, DMOInplaceProcess.Normal);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestLatency()
        {
            int hr;
            long l;

            hr = m_imoip.GetLatency(out l);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestClone()
        {
            int hr;
            long l;
            IMediaObjectInPlace pMO;

            hr = m_imoip.Clone(out pMO);
            DMOError.ThrowExceptionForHR(hr);

            hr = pMO.GetLatency(out l);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            int hr;

            DMOWrapperFilter dmoFilter = new DMOWrapperFilter();
            IDMOWrapperFilter dmoWrapperFilter = (IDMOWrapperFilter) dmoFilter;

            // Chorus - {efe6629c-81f7-4281-bd91-c9d604a95af6}
            // DmoFlip - {7EF28FD7-E88F-45bb-9CDD-8A62956F2D75}
            //hr = dmoWrapperFilter.Init(new Guid("{7EF28FD7-E88F-45bb-9CDD-8A62956F2D75}"), DMOCategory.AudioEffect);
            hr = dmoWrapperFilter.Init(new Guid("{efe6629c-81f7-4281-bd91-c9d604a95af6}"), DMOCategory.AudioEffect);
            DMOError.ThrowExceptionForHR(hr);

            m_imo = dmoWrapperFilter as IMediaObject;
            m_imoip = dmoWrapperFilter as IMediaObjectInPlace;
        }
    }
}
