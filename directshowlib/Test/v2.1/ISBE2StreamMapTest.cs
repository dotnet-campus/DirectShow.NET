using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;

namespace v2_1
{
    public class ISBE2StreamMapTest
    {
        const string FILENAME = @"c:\foo.stub";
        private ISBE2StreamMap m_sm;
        private ISBE2Crossbar m_cb;
        IMediaControl m_imc2;

        public ISBE2StreamMapTest()
        {
        }

        public void DoTests()
        {
            Config();

            TestEnum();
        }

        private void TestEnum()
        {
            int hr;
            ISBE2EnumStream ps;
            SBE2_StreamDesc[] sb = new SBE2_StreamDesc[1];

            hr = m_cb.EnableDefaultMode(CrossbarDefaultFlags.None);
            DsError.ThrowExceptionForHR(hr);

            IPin ppin = DsFindPin.ByDirection(m_cb as IBaseFilter, PinDirection.Output, 0);
            m_sm = ppin as ISBE2StreamMap;

            hr = m_sm.EnumMappedStreams(out ps);
            DsError.ThrowExceptionForHR(hr);

            hr = ps.Next(1, sb, IntPtr.Zero);

            hr = m_sm.UnmapStream(sb[0].StreamId);
            DsError.ThrowExceptionForHR(hr);

            hr = m_sm.MapStream(sb[0].StreamId);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            int hr;
            IFilterGraph gb = (IFilterGraph)new FilterGraph();
            int iDeviceIndex = 0;

            ICaptureGraphBuilder2 cgb = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            IMediaControl m_imc1 = gb as IMediaControl;
            hr = cgb.SetFiltergraph((IGraphBuilder)gb);

            DsDevice[] dev = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            IBaseFilter theDevice;
            hr = ((IFilterGraph2)gb).AddSourceFilterForMoniker(dev[iDeviceIndex].Mon, null, dev[iDeviceIndex].Name, out theDevice);

            IStreamBufferSink sbs = new SBE2Sink() as IStreamBufferSink;
            hr = gb.AddFilter((IBaseFilter)sbs, "StreamBufferSink");
            DsError.ThrowExceptionForHR(hr);

            hr = cgb.RenderStream(null, null, theDevice, null, sbs as IBaseFilter);
            DsError.ThrowExceptionForHR(hr);

            hr = sbs.LockProfile(FILENAME);
            DsError.ThrowExceptionForHR(hr);
            SetupGraph2();

            Marshal.ReleaseComObject(cgb);
            Marshal.ReleaseComObject(theDevice);

            m_imc1.Run();
            System.Threading.Thread.Sleep(1000);
            m_imc2.Run();
            System.Threading.Thread.Sleep(1000);
        }

        private void SetupGraph2()
        {
            int hr;

            // Get a ICaptureGraphBuilder2 to help build the graph
            ICaptureGraphBuilder2 icgb2 = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

            try
            {
                // Get the graphbuilder object
                IFilterGraph2 graphBuilder2 = (IFilterGraph2)new FilterGraph();
                m_imc2 = graphBuilder2 as IMediaControl;

                // Link the ICaptureGraphBuilder2 to the IFilterGraph2
                hr = icgb2.SetFiltergraph(graphBuilder2);
                DsError.ThrowExceptionForHR(hr);

                IBaseFilter streamBuffer = (IBaseFilter)new StreamBufferSource();

                hr = graphBuilder2.AddFilter(streamBuffer, "Stream buffer sink");
                DsError.ThrowExceptionForHR(hr);

                IFileSourceFilter sbfsf = (IFileSourceFilter)streamBuffer;

                hr = sbfsf.Load(FILENAME, null);
                DsError.ThrowExceptionForHR(hr);

                RenderPins(streamBuffer, icgb2);

                m_cb = streamBuffer as ISBE2Crossbar;
                ISBE2GlobalEvent2 ge2 = streamBuffer as ISBE2GlobalEvent2;
            }
            finally
            {
                if (icgb2 != null)
                {
                    Marshal.ReleaseComObject(icgb2);
                }
            }
        }

        private void RenderPins(IBaseFilter streamBuffer, ICaptureGraphBuilder2 icgb2)
        {
            int hr;
            IEnumPins iep;

            hr = streamBuffer.EnumPins(out iep);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                //hr = icgb2.RenderStream(null, null, pPin[0], null, null);
#if true
                IPin[] pPin = new IPin[1];

                // Walk each pin of the stream buffer source
                for (
                    hr = iep.Next(1, pPin, IntPtr.Zero);
                    hr == 0;
                    hr = iep.Next(1, pPin, IntPtr.Zero)
                    )
                {
                    try
                    {
                        AMMediaType[] amt = new AMMediaType[1];
                        IEnumMediaTypes pEnum;

                        hr = pPin[0].EnumMediaTypes(out pEnum);
                        DsError.ThrowExceptionForHR(hr);

                        try
                        {
                            // Grab the first media type
                            hr = pEnum.Next(1, amt, IntPtr.Zero);
                            DsError.ThrowExceptionForHR(hr);

                            try
                            {
                                // use the media type to render the stream
                                hr = icgb2.RenderStream(null, amt[0].majorType, pPin[0], null, null);
                                DsError.ThrowExceptionForHR(hr);
                            }
                            finally
                            {
                                DsUtils.FreeAMMediaType(amt[0]);
                            }
                        }
                        finally
                        {
                            Marshal.ReleaseComObject(pEnum);
                        }
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(pPin[0]);
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(iep);
            }
#endif
        }

    }
}
