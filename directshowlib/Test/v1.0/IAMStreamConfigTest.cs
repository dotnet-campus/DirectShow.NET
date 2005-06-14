using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Drawing;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IAMStreamConfigTest
    {
        const int E_PROP_ID_UNSUPPORTED = unchecked((int)0x80070490);

        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null;
        IAMStreamConfig m_asc = null;

        public IAMStreamConfigTest()
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
                TestFormat();
                TestGetNum();
                TestStreamCaps();
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

        void TestFormat()
        {
            int hr;
            AMMediaType pmt;

            hr = m_asc.GetFormat(out pmt);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pmt.sampleSize > 0, "GetFormat");

            hr = m_asc.SetFormat(pmt);
            DsError.ThrowExceptionForHR(hr);
        }


        void TestGetNum()
        {
            int hr;
            int iCount, iSize;

            hr = m_asc.GetNumberOfCapabilities(out iCount, out iSize);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(iCount > 0, "GetFormat");
            Debug.Assert(iSize == Marshal.SizeOf(typeof(VideoStreamConfigCaps)) || 
                        iSize == Marshal.SizeOf(typeof(AudioStreamConfigCaps)), 
                        "GetNumberOfCaps");
        }


        void TestStreamCaps()
        {
            int hr;
            IntPtr pss;
            AMMediaType pmt;
            int iCount, iSize;

            hr = m_asc.GetNumberOfCapabilities(out iCount, out iSize);
            DsError.ThrowExceptionForHR(hr);

            pss = Marshal.AllocCoTaskMem(iCount * iSize);
            DsError.ThrowExceptionForHR(hr);

            if (iSize == Marshal.SizeOf(typeof(VideoStreamConfigCaps)))
            {
                for (int x=0; x < iCount; x++)
                {
                    hr = m_asc.GetStreamCaps(x, out pmt, pss);
                    DsError.ThrowExceptionForHR(hr);

                    VideoStreamConfigCaps vscc = (VideoStreamConfigCaps)Marshal.PtrToStructure(pss, typeof(VideoStreamConfigCaps));
                    pss = (IntPtr)(pss.ToInt64() + Marshal.SizeOf(typeof(VideoStreamConfigCaps)));
                }
            }
            else if (iSize == Marshal.SizeOf(typeof(AudioStreamConfigCaps)))
            {
                for (int x=0; x < iCount; x++)
                {
                    hr = m_asc.GetStreamCaps(x, out pmt, pss);
                    DsError.ThrowExceptionForHR(hr);

                    AudioStreamConfigCaps vscc = (AudioStreamConfigCaps)Marshal.PtrToStructure(pss, typeof(AudioStreamConfigCaps));
                    pss = (IntPtr)(pss.ToInt64() + Marshal.SizeOf(typeof(AudioStreamConfigCaps)));
                }
            }
            else
            {
                Debug.Assert(false, "GetStreamCaps");
            }

            Marshal.FreeCoTaskMem(pss);
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

            ICaptureGraphBuilder2 captureGraphBuilder = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = captureGraphBuilder.SetFiltergraph( graphBuilder );

            object o;
            hr = captureGraphBuilder.FindInterface(null, null, ppFilter, typeof(IAMStreamConfig).GUID, out o );
            DsError.ThrowExceptionForHR(hr);

            m_asc = o as IAMStreamConfig;

            //m_imc = graphBuilder as IMediaControl;
            //hr = m_imc.Run();
            //DsError.ThrowExceptionForHR(hr);

        }
    }
}