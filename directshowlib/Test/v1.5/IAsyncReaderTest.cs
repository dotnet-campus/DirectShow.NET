using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

using DirectShowLib;

namespace v1._5
{
    public class IAsyncReaderTest
    {
        IAsyncReader m_iar;
        IMemAllocator m_pActual;

        public void DoTests()
        {
            Setup();

            TestAllocator();
            TestFlush();
            TestLength();
            TestSyncRead();
            TestWaitForNext();
        }

        private void TestAllocator()
        {
            int hr;
            AllocatorProperties pProps = new AllocatorProperties();
            AllocatorProperties pActualProps = new AllocatorProperties();

            pProps.cbAlign = 1;
            pProps.cbBuffer = 512 * 2;
            pProps.cbPrefix = 0;
            pProps.cBuffers = 5;

            hr = m_iar.RequestAllocator(null, pProps, out m_pActual);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pActual.SetProperties(pProps, pActualProps);
            DsError.ThrowExceptionForHR(hr);

            hr = m_pActual.Commit();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestWaitForNext()
        {
            int hr;
            IntPtr pUser;
            IMediaSample pSample;
            IMediaSample pSample2;

            hr = m_pActual.GetBuffer(out pSample, 0, 0, AMGBF.None);
            DsError.ThrowExceptionForHR(hr);

            hr = pSample.SetTime(0, 512 * 10000000L);
            DsError.ThrowExceptionForHR(hr);

            hr = pSample.SetActualDataLength(5);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iar.Request(pSample, (IntPtr)2);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iar.WaitForNext(1000, out pSample2, out pUser);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pUser == (IntPtr)2, "WaitForNext");
            Debug.Assert(pSample2.GetActualDataLength() == 512, "WaitForNext2");

            Marshal.ReleaseComObject(pSample);
        }

        private void TestSyncReadAligned()
        {
            int hr;
            IMediaSample pSample;

            hr = m_pActual.GetBuffer(out pSample, 0, 0, AMGBF.None);
            DsError.ThrowExceptionForHR(hr);

            hr = pSample.SetTime(0, 512 * 10000000L);
            DsError.ThrowExceptionForHR(hr);

            hr = m_iar.SyncReadAligned(pSample);
            DsError.ThrowExceptionForHR(hr);

            hr = pSample.GetActualDataLength();
            Debug.Assert(hr == 512, "SyncReadAligned");
            Marshal.ReleaseComObject(pSample);
        }

        private void TestSyncRead()
        {
            int hr;
            long lBuff;
            IntPtr pBuff = Marshal.AllocCoTaskMem(8);

            hr = m_iar.SyncRead(0, 8, pBuff);
            DsError.ThrowExceptionForHR(hr);

            lBuff = Marshal.ReadInt64(pBuff);
            Marshal.FreeCoTaskMem(pBuff);

            Debug.Assert(lBuff == 0x0012a09446464952, "SyncRead");
        }

        private void TestFlush()
        {
            int hr;

            hr = m_iar.BeginFlush();
            DsError.ThrowExceptionForHR(hr);

            hr = m_iar.EndFlush();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestLength()
        {
            int hr;
            long pTot, pAvail;

            hr = m_iar.Length(out pTot, out pAvail);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pTot == pAvail, "Length1");
            Debug.Assert(pTot == 1222656, "Length2");
        }

        private void Setup()
        {
            IFileSourceFilter ifsf = new AsyncReader() as IFileSourceFilter;

            int hr = ifsf.Load("foo.avi", null);
            IPin pPin = DsFindPin.ByDirection((IBaseFilter)ifsf, PinDirection.Output, 0);
            m_iar = pPin as IAsyncReader;
        }
    }
}
