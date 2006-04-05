using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib.BDA;
using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IComponentTypesTest
    {
        private IComponentTypes m_compTypes;

        public IComponentTypesTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                TestAdd();
                TestCount();
                TestGetPut();
                TestClone();
                TestNewEnum();
                TestEnum();
                TestRemove();
            }
            finally
            {
                Marshal.ReleaseComObject(m_compTypes);
            }
        }

        private void TestAdd()
        {
            int hr;
            object o;

            IComponentType icomp = (IComponentType)new ComponentType();

            hr = m_compTypes.Add(icomp, out o);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestCount()
        {
            int hr;
            int i;

            hr = m_compTypes.get_Count(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1, "get_Count");
        }

        private void TestGetPut()
        {
            int hr;
            IComponentType ct;

            hr = m_compTypes.get_Item(0, out ct);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(ct != null, "get_Item");

            hr = m_compTypes.put_Item(0, ct);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestClone()
        {
            int hr;
            int i;
            IComponentTypes it;

            hr = m_compTypes.Clone(out it);
            DsError.ThrowExceptionForHR(hr);

            hr = it.get_Count(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 1, "clone");
        }

        private void TestNewEnum()
        {
            int hr;
            IEnumVARIANT pEnum;

            hr = m_compTypes.get__NewEnum(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum != null, "get__NewEnum");
        }

        private void TestEnum()
        {
            int hr;
            IEnumComponentTypes pEnum;

            hr = m_compTypes.EnumComponentTypes(out pEnum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pEnum != null, "EnumComponentTypes");
        }

        private void TestRemove()
        {
            int hr;
            int i;

            hr = m_compTypes.Remove(0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compTypes.get_Count(out i);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(i == 0, "Remove");
        }

        private void Config()
        {
            m_compTypes = (IComponentTypes) new ComponentTypes();
        }
    }
}
