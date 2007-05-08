// The only filter I know that supports this interface returns E_NOTIMPL for all methods

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IXDSCodecTest
    {
        IXDSCodec m_xc;

        public void DoTests()
        {
            Configure();
            TestToRat();
            TestSubstream();
            TestLastError();
            TestRat();
            TestPacket();
            TestLicense();
        }

        private void TestToRat()
        {
            int hr, hr2;
            
            // Not implemented (or doc'ed)
            hr = m_xc.get_XDSToRatObjOK(out hr2);
            //DsError.ThrowExceptionForHR(hr);
            //DsError.ThrowExceptionForHR(hr2);
        }

        private void TestSubstream()
        {
            int hr;
            int iMask;

            // Not implemented
            hr = m_xc.put_CCSubstreamService(3);
            //DsError.ThrowExceptionForHR(hr);

            // Not implemented
            hr = m_xc.get_CCSubstreamService(out iMask);
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestLastError()
        {
            int hr;

            // Not implemeted
            hr = m_xc.GetLastErrorCode();
            //DsError.ThrowExceptionForHR(hr);
        }

        private void TestRat()
        {
            int hr;
            int pRat;
            int pSeq;
            int pCall;
            long pStart;
            long pTime;

            hr = m_xc.GetContentAdvisoryRating(
                out pRat,
                out pSeq,
                out pCall,
                out pStart,
                out pTime);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pSeq == 1, "GetContentAdvisoryRating");
        }

        private void TestPacket()
        {
            int hr;
            int pPkt, pType;
            string pBstr;
            int pSeq, pCall;
            long pStart, pEnd;

            // Not Implemented
            hr = m_xc.GetXDSPacket(
                out pPkt,
                out pType,
                out pBstr,
                out pSeq,
                out pCall,
                out pStart,
                out pEnd);
        }

        private void TestLicense()
        {
            int hr;
            ProtType pType = ProtType.Free;
            int pDate;

            // Not implemented
            hr = m_xc.GetCurrLicenseExpDate(pType, out pDate);
        }

        private void Configure()
        {
            // BDA MPEG2 Transport Information Filter
            Guid g = new Guid("{fc772ab0-0c7f-11d3-8ff2-00a0c9224cf4}");
            Type type = Type.GetTypeFromCLSID(g);
            m_xc = Activator.CreateInstance(type) as IXDSCodec;
        }
    }
}

