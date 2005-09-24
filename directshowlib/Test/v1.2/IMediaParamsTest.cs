using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class IMediaParamsTest
    {
        IMediaParams m_imp;

        public IMediaParamsTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestParam();
                TestTime();
                TestAddEnvelope();
                TestFlushEnvelope();
            }
            finally
            {
                Marshal.ReleaseComObject(m_imp);
            }
        }

        private void TestFlushEnvelope()
        {
            int hr;

            hr = m_imp.FlushEnvelope(0, 0, long.MaxValue);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestAddEnvelope()
        {
            int hr;
            MPEnvelopeSegment [] pSeg = new MPEnvelopeSegment[2];
            MPData mp1 = new MPData();
            MPData mp2 = new MPData();

            mp1.vFloat = 40;
            mp2.vFloat = 60;

            pSeg[0].flags = MPFlags.Standard;
            pSeg[0].iCurve = MPCaps.Jump;
            pSeg[0].rtStart = 0;
            pSeg[0].rtEnd = 1000000;
            pSeg[0].valStart = mp1;
            pSeg[0].valEnd = mp2;

            pSeg[1].flags = MPFlags.Standard;
            pSeg[1].iCurve = MPCaps.Jump;
            pSeg[1].rtStart = 1000001;
            pSeg[1].rtEnd = 2000000;
            pSeg[1].valStart = mp2;
            pSeg[1].valEnd = mp1;

            hr = m_imp.AddEnvelope(0, 2, pSeg);
            DMOError.ThrowExceptionForHR(hr);
        }

        private void TestParam()
        {
            int hr;
            MPData pData = new MPData();

            pData.vFloat = 51;

            hr = m_imp.SetParam(0, pData);
            DMOError.ThrowExceptionForHR(hr);

            hr = m_imp.GetParam(0, out pData);
            DMOError.ThrowExceptionForHR(hr);

            Debug.Assert(pData.vFloat == 51, "GetParam");
        }

        private void TestTime()
        {
            int hr;

            hr = m_imp.SetTimeFormat(MediaParamTimeFormat.Reference, 33);
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

            m_imp = dmoWrapperFilter as IMediaParams;
        }
    }
}
