// GetCurrentActualFrameRate->out long
// GetFrameRateList->out IntPtr

// Here are all the different ways I tried declaring FrameRate to avoid the IntPtr
#if false
    // -------------------------------
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] long [] FrameRates1
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] long [] FrameRates2
    [MarshalAs(UnmanagedType.LPArray)] long [] FrameRates3
    [MarshalAs(UnmanagedType.LPArray, SizeConst=5)] long [] FrameRates4
    [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] long [] FrameRates5
    [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] long [] FrameRates6
    [Out, MarshalAs(UnmanagedType.LPArray)] long [] FrameRates7
    [Out, MarshalAs(UnmanagedType.LPArray, SizeConst=5)] long [] FrameRates8
    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] long [] FrameRatesa
    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] long [] FrameRatesb
    [In, Out, MarshalAs(UnmanagedType.LPArray)] long [] FrameRatesc
    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst=5)] long [] FrameRatesd
    // -------------------------------
    //Can not use SizeParamIndex for byref array parameters
    //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] out long [] FrameRates11
    //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] out long [] FrameRates12
    [MarshalAs(UnmanagedType.LPArray)] out long [] FrameRates13
    //Can not use SizeParamIndex for byref array parameters
    //[MarshalAs(UnmanagedType.LPArray, SizeConst=5)] out long [] FrameRates14
    //[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] out long [] FrameRates15
    [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] out long [] FrameRates16
    [Out, MarshalAs(UnmanagedType.LPArray)] out long [] FrameRates17
    //Can not use SizeParamIndex for byref array parameters
    //[Out, MarshalAs(UnmanagedType.LPArray, SizeConst=5)] out long [] FrameRates18
    // These won't compile
    //[In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] out long [] FrameRates1a
    //[In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] out long [] FrameRates1b
    //[In, Out, MarshalAs(UnmanagedType.LPArray)] out long [] FrameRates1c
    //[In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst=5)] out long [] FrameRates1d
    // -------------------------------
    //Can not use SizeParamIndex for byref array parameters
    //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] ref long [] FrameRates21
    //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] ref long [] FrameRates22
    [MarshalAs(UnmanagedType.LPArray)] ref long [] FrameRates23
    //[MarshalAs(UnmanagedType.LPArray, SizeConst=5)] ref long [] FrameRates24
    // These won't compile
    //[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] ref long [] FrameRates25
    //[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] ref long [] FrameRates26
    //[Out, MarshalAs(UnmanagedType.LPArray)] ref long [] FrameRates27
    //[Out, MarshalAs(UnmanagedType.LPArray, SizeConst=5)] ref long [] FrameRates28
    //Can not use SizeParamIndex for byref array parameters
    //[In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] ref long [] FrameRates2a
    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] ref long [] FrameRates2b
    [In, Out, MarshalAs(UnmanagedType.LPArray)] ref long [] FrameRates2c
    [In, Out, MarshalAs(UnmanagedType.LPArray, SizeConst=5)] ref long [] FrameRates2d
#endif

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Drawing;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IAMVideoControlTest
    {
        const int E_PROP_ID_UNSUPPORTED = unchecked((int)0x80070490);

        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null;
        IAMVideoControl m_ivc = null;
        IPin m_IPinOut = null;

        public IAMVideoControlTest()
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
                TestCaps();
                GetFrameRate();
                TestFrameRateList();
                TestGetMaxFrameRate();
                TestMode();
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

        void TestCaps()
        {
            int hr;
            VideoControlFlags vcf;

            hr = m_ivc.GetCaps(m_IPinOut, out vcf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vcf > 0, "GetCaps");
        }

        void GetFrameRate()
        {
            int hr;
            long fr;

            hr = m_ivc.GetCurrentActualFrameRate(m_IPinOut, out fr);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(fr > 0, "GetCurrentActualFrameRate");
        }

        void TestFrameRateList()
        {
            int hr;
            Size s = new Size(640, 480);
            int ls;
            IntPtr ip;

            hr = m_ivc.GetFrameRateList(m_IPinOut, 0, s, out ls, out ip);
            DsError.ThrowExceptionForHR(hr);

            for (int x=0; x < ls; x++)
            {
                long l = Marshal.ReadInt64(ip, x * 8);
                Debug.WriteLine(l);
            }
        }

        void TestGetMaxFrameRate()
        {
            int hr;
            Size s = new Size(640, 480);
            long ls;

            hr = m_ivc.GetMaxAvailableFrameRate(m_IPinOut, 0, s, out ls);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(ls > 0, "GetMaxAvailableFrameRate");
        }

        void TestMode()
        {
            int hr;
            VideoControlFlags vcf;

            hr = m_ivc.SetMode(m_IPinOut, VideoControlFlags.FlipHorizontal);
            DsError.ThrowExceptionForHR(hr);

            hr = m_ivc.GetMode(m_IPinOut, out vcf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(vcf == VideoControlFlags.FlipHorizontal, "Get/Set Mode");
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

            m_imc = graphBuilder as IMediaControl;
            //hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);
        }
    }
}