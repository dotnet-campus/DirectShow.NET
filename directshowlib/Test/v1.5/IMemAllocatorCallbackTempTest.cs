using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
    public class IMemAllocatorCallbackTempTest : IMemAllocatorNotifyCallbackTemp
    {
        IMemAllocatorCallbackTemp m_ma;
        bool m_bGotOne = false;

        public void DoTests()
        {
            Setup();

            TestCount();
            TestCB();
        }

        private void TestCount()
        {
            int hr;
            int i;

            hr = m_ma.GetFreeCount(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 24, "Buffer Counter");
        }

        private void TestCB()
        {
            int i;
            int hr;
            IMediaSample pBuff;

            hr = m_ma.GetBuffer(out pBuff, 1, 2, AMGBF.None);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pBuff.GetActualDataLength() == 1024, "Check size");

            hr = m_ma.GetFreeCount(out i);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ma.ReleaseBuffer(pBuff);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(m_bGotOne, "Callback");
        }

        private void Setup()
        {
            int hr;
            m_ma = new MemoryAllocator() as IMemAllocatorCallbackTemp;

            AllocatorProperties pProps = new AllocatorProperties();
            AllocatorProperties pActual = new AllocatorProperties();

            pProps.cbAlign = 1;
            pProps.cbBuffer = 1024;
            pProps.cbPrefix = 0;
            pProps.cBuffers = 24;

            hr = m_ma.SetProperties(pProps, pActual);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ma.SetNotify(this);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ma.Commit();
            DsError.ThrowExceptionForHR(hr);
        }

        #region IMemAllocatorNotifyCallbackTemp Members

        int IMemAllocatorNotifyCallbackTemp.NotifyRelease()
        {
            m_bGotOne = true;
            return 0;
        }

        #endregion
    }
}
