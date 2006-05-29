using System;
using System.Diagnostics;

using DirectShowLib;

namespace v1._5
{
#if ALLOW_UNTESTED_INTERFACES
    class IResourceManagerTest : IResourceConsumer
    {
        private IResourceManager m_rm;
        private int[] m_tokens = new int[2];

        public void DoTests()
        {
            Setup();

            TestRegister();
            TestGroup();
            TestRequest();
        }

        private void TestRegister()
        {
            int hr;

            hr = m_rm.Register("res1", 5, out m_tokens[0]);
            DsError.ThrowExceptionForHR(hr);

            hr = m_rm.Register("res2", 5, out m_tokens[1]);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestGroup()
        {
            int hr;
            int groupToken;

            hr = 0; // m_rm.RegisterGroup("Group1", 2, m_tokens, out groupToken);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestRequest()
        {
            int hr;

            hr = m_rm.RequestResource(m_tokens[0], this, this);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Setup()
        {
            m_rm = new FilterGraphNoThread() as IResourceManager;
        }

        #region IResourceConsumer Members

        public int AcquireResource(int idResource)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ReleaseResource(int idResource)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
#endif
}
