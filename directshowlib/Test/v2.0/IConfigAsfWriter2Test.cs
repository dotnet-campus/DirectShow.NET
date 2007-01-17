using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;

namespace v2_0
{
    public class IConfigAsfWriter2Test
    {
        IConfigAsfWriter2 m_asfw;
        IFilterGraph2 m_FilterGraph;

        // This is a hacked and partial version of IWMProfile
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
            Guid("96406BDB-2B2B-11D3-B36B-00C04F6108FF")]
            public interface IWMProfile
        {
            [PreserveSig]
            int GetVersion(
                out int pdwVersion
                );

            [PreserveSig]
            int GetName(
                [Out] StringBuilder pwszName,
                ref int pcchName
                );
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestResetMultiPassState();
                TestParam();
                TestStreamNumFromPin();
            }
            finally
            {
                Marshal.ReleaseComObject(m_FilterGraph);
            }
        }

        private void TestStreamNumFromPin()
        {
            int hr;
            short pNum;
            IBaseFilter ibf = m_asfw as IBaseFilter;

            IPin pPin = DsFindPin.ByDirection(ibf, PinDirection.Input, 1);

            hr = m_asfw.StreamNumFromPin(pPin, out pNum);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pNum == 1, "StreamNumFromPin");
        }

        private void TestParam()
        {
            int hr;
            int p1;
            IntPtr p2 = IntPtr.Zero;

            hr = m_asfw.GetParam(ASFWriterConfig.MultiPass, out p1, p2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(p1 == 0, "GetParam");

            hr = m_asfw.SetParam(ASFWriterConfig.MultiPass, 1, 0);
            DsError.ThrowExceptionForHR(hr);

            hr = m_asfw.GetParam(ASFWriterConfig.MultiPass, out p1, p2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(p1 == 1, "SetParam");
        }

        private void TestResetMultiPassState()
        {
            int hr;

            hr = m_asfw.ResetMultiPassState();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            int hr;

            m_FilterGraph = (IFilterGraph2)new FilterGraph();

            m_asfw = (IConfigAsfWriter2)new WMAsfWriter();
            hr = m_FilterGraph.AddFilter((IBaseFilter)m_asfw, "asdf");
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
