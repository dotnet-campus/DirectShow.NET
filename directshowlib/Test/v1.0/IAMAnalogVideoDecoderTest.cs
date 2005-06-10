using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Drawing;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IAMAnalogVideoDecoderTest
    {
        const int E_PROP_ID_UNSUPPORTED = unchecked((int)0x80070490);

        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null;
        IAMVideoControl m_ivc = null;
        IPin m_IPinOut = null;
        IAMAnalogVideoDecoder m_avd = null;

        public IAMAnalogVideoDecoderTest()
        {
        }

        /// <summary>
        /// Test all methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            BuildGraph();

            try
            {
                TestAvailable();
                TestHorizontal();
                TestVCRHorizontal();
                TestTVFormat();
                TestOutput();
                TestLines();
            }
            finally
            {
                if (m_imc != null)
                {
                    m_imc.Stop();
                    Marshal.ReleaseComObject(m_imc);
                    m_imc = null;
                }
            }
        }

        void TestAvailable()
        {
            int hr;
            AnalogVideoStandard avs;

            hr = m_avd.get_AvailableTVFormats( out avs);
            DsError.ThrowExceptionForHR(hr);

            // There should be something
            Debug.Assert(avs > 0, "get_AvailableTVFormats");
        }

        void TestHorizontal()
        {
            int hr;
            bool locked;

            hr = m_avd.get_HorizontalLocked(out locked);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestVCRHorizontal()
        {
            int hr;
            bool vlock;

            hr = m_avd.put_VCRHorizontalLocking(false);
            DsError.ThrowExceptionForHR(hr);

            hr = m_avd.get_VCRHorizontalLocking(out vlock);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vlock == false, "VCRHorizontalLocking");

            hr = m_avd.put_VCRHorizontalLocking(true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_avd.get_VCRHorizontalLocking(out vlock);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vlock == true, "VCRHorizontalLocking2");
        }

        void TestTVFormat()
        {
            int hr;
            AnalogVideoStandard avs;

            hr = m_avd.put_TVFormat(AnalogVideoStandard.NTSC_M_J);
            DsError.ThrowExceptionForHR(hr);

            hr = m_avd.get_TVFormat(out avs);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(avs == AnalogVideoStandard.NTSC_M_J, "Get/Set TVFormat");
        }

        void TestLines()
        {
            int hr;
            int numline;

            hr = m_avd.get_NumberOfLines(out numline);
            DsError.ThrowExceptionForHR(hr);

            // Returns zero with my device
            //Debug.Assert(hr > 0, "NumberOfLines");
        }

        void TestOutput()
        {
            int hr;
            bool b;

            hr = m_avd.get_OutputEnable(out b);

            // My card doesn't support this
            if (hr != E_PROP_ID_UNSUPPORTED)
            {
                DsError.ThrowExceptionForHR(hr);

                hr = m_avd.put_OutputEnable(false);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        void BuildGraph()
        {
            int hr;
            IBaseFilter ppFilter;
            DsDevice [] devs;
            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;

            m_ROT = new DsROTEntry(graphBuilder);
            IFilterGraph2 ifg2 = graphBuilder as IFilterGraph2;

            devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = devs[0];

            hr = ifg2.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out ppFilter);
            DsError.ThrowExceptionForHR(hr);

            m_ivc = ppFilter as IAMVideoControl;

            m_IPinOut = DsFindPin.ByDirection(ppFilter, PinDirection.Output, 0);

            hr = ifg2.Render(m_IPinOut);
            DsError.ThrowExceptionForHR(hr);

            ICaptureGraphBuilder2 captureGraphBuilder = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = captureGraphBuilder.SetFiltergraph( graphBuilder );

            object o;
            hr = captureGraphBuilder.FindInterface(null, null, ppFilter, typeof(IAMAnalogVideoDecoder).GUID, out o );
            DsError.ThrowExceptionForHR(hr);

            m_avd = o as IAMAnalogVideoDecoder;

            m_imc = graphBuilder as IMediaControl;
            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);

        }
    }
}