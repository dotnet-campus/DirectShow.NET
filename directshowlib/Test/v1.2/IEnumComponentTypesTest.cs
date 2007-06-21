using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;

namespace DirectShowLib.Test
{
    public class IEnumComponentTypesTest
    {
        IEnumComponentTypes m_pEnum;

        public IEnumComponentTypesTest()
        {
        }

        public void DoTests()
        {
            try
            {
                Config();

                TestClone();
                TestNext();
                TestReset();
                TestSkip();
            }
            finally
            {
                Marshal.ReleaseComObject(m_pEnum);
            }
        }

        private void Config()
        {
            int hr;
            object o;

            IComponentTypes compTypes = (IComponentTypes) new ComponentTypes();

            IComponentType icomp = (IComponentType)new ComponentType();

            for (int x=0; x < 7; x++)
            {
                hr = compTypes.Add(icomp, out o);
                DsError.ThrowExceptionForHR(hr);
            }

            hr = compTypes.EnumComponentTypes(out m_pEnum);
            DsError.ThrowExceptionForHR(hr);
        }

        private void TestClone()
        {
            int hr = 0;
            IEnumComponentTypes clone;

            hr = m_pEnum.Clone(out clone);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(clone != null, "Clone");

            Marshal.ReleaseComObject(clone);
        }

        private void TestNext()
        {
            int hr = 0;
            IComponentType[] ts = new IComponentType[5];
            int fetched;
            int validObj = 0;
            IntPtr ip = Marshal.AllocCoTaskMem(4);

            hr = m_pEnum.Next(ts.Length, ts, ip);
            fetched = Marshal.ReadInt32(ip);
            Marshal.FreeCoTaskMem(ip);

            DsError.ThrowExceptionForHR(hr);

            for(int i = 0; i < fetched; i++)
            {
                if (ts[i] != null)
                {
                    validObj++;
                    Marshal.ReleaseComObject(ts[i]);
                }
            }

            Debug.Assert(validObj == fetched, "IEnumTuningSpaces.Next");
        }

        private void TestReset()
        {
            int hr = 0;

            hr = m_pEnum.Reset();

            Debug.Assert(hr == 0, "IEnumTuningSpaces.Reset");
        }

        private void TestSkip()
        {
            int hr = 0;

            hr = m_pEnum.Skip(4);

            Debug.Assert(hr == 0, "IEnumTuningSpaces.Skip");
        }

    }
}
