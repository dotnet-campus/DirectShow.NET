using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
    public class IMemAllocatorTest
    {
        IMemAllocator m_ma;

        public void DoTests()
        {
            Setup();

            TestProp();
            TestCommit();
            TestBuffer();
            TestDecommit();
        }

        private void TestBuffer()
        {
            int hr;
            IMediaSample pBuff;

            hr = m_ma.GetBuffer(out pBuff, 1, 2, AMGBF.None);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pBuff.GetActualDataLength() == 1024, "Check size");

            hr = m_ma.ReleaseBuffer(pBuff);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestDecommit()
        {
            int hr;

            hr = m_ma.Decommit();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestCommit()
        {
            int hr;

            hr = m_ma.Commit();
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestProp()
        {
            int hr;
            AllocatorProperties pProps = new AllocatorProperties();
            AllocatorProperties pProps2 = new AllocatorProperties();
            AllocatorProperties pActual = new AllocatorProperties();

            hr = m_ma.GetProperties(pProps);
            DsError.ThrowExceptionForHR(hr);

            pProps.cbAlign = 1;
            pProps.cbBuffer = 1024;
            pProps.cbPrefix = 0;
            pProps.cBuffers = 24;

            hr = m_ma.SetProperties(pProps, pActual);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ma.GetProperties(pProps2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pProps.cbBuffer == pProps2.cbBuffer, "Check Size");
            Debug.Assert(pProps.cbBuffer == pActual.cbBuffer, "Check Size");
        }

        private void Setup()
        {
            m_ma = new MemoryAllocator() as IMemAllocator;
        }
    }
}
