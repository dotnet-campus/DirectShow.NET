using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;
using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IComponentTest
    {
        private IComponent m_comp;

        public IComponentTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestLangID();
                TestDesc();
                TestStatus();
                TestType();
                TestClone();
            }
            finally
            {
                Marshal.ReleaseComObject(m_comp);
            }
        }

        private void TestLangID()
        {
            int hr;
            int lid;

            hr = m_comp.put_DescLangID(123);
            DsError.ThrowExceptionForHR(hr);

            hr = m_comp.get_DescLangID(out lid);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(lid == 123, "DescLangID");
        }

        private void TestDesc()
        {
            int hr;
            string sDesc;

            hr = m_comp.put_Description("moo");
            DsError.ThrowExceptionForHR(hr);

            hr = m_comp.get_Description(out sDesc);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sDesc == "moo", "Description");
        }

        private void TestStatus()
        {
            int hr;
            ComponentStatus stat;

            hr = m_comp.put_Status(ComponentStatus.Inactive);
            DsError.ThrowExceptionForHR(hr);

            hr = m_comp.get_Status(out stat);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(stat == ComponentStatus.Inactive, "Status");
        }

        private void TestType()
        {
            int hr;
            IComponentType iType, iType2;

            iType = (IComponentType)new ComponentType();

            hr = m_comp.put_Type(iType);
            DsError.ThrowExceptionForHR(hr);

            hr = m_comp.get_Type(out iType2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iType == iType2, "Type");
        }

        private void TestClone()
        {
            int hr;
            IComponent it;

            hr = m_comp.Clone(out it);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(it != null, "Clone");
        }

        private void Config()
        {
            m_comp = (IComponent) new Component();
        }
    }
}
