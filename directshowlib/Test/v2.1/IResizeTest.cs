using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.DES;

namespace v2_1
{
    public class IResizeTest
    {
        IResize m_rs;

        public IResizeTest()
        {
        }

        public void DoTests()
        {
            Config();
            TestSize();
            TestMT();

            Config2();
            TestInput();
        }

        private void TestSize()
        {
            int hr, h, w;
            ResizeFlags pf;

            hr = m_rs.put_Size(480, 640, ResizeFlags.Crop);
            DsError.ThrowExceptionForHR(hr);

            hr = m_rs.get_Size(out h, out w, out pf);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(h == 480 && w == 640 && pf == ResizeFlags.Crop);
        }

        private void TestInput()
        {
            int hr, h, w;

            hr = m_rs.get_InputSize(out h, out w);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(h > 0 && w > 0);
        }

        private void TestMT()
        {
            int hr;
            AMMediaType mt = new AMMediaType();

            hr = m_rs.get_MediaType(mt);
            DsError.ThrowExceptionForHR(hr);

            hr = m_rs.put_MediaType(mt);
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            object o;
            Guid g = typeof(IBaseFilter).GUID;
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.LegacyAmFilterCategory);
            foreach (DsDevice dev in devs)
            {
                if (dev.Name == "Stretch Video")
                {
                    dev.Mon.BindToObject(null, null, ref g, out o);

                    m_rs = o as IResize;
                    break;
                }
            }
        }

        private void Config2()
        {
            Guid g = typeof(IBaseFilter).GUID;
            DsDevice[] devs;
            IBaseFilter ibf, ibf2;
            object o;

            int hr;
            IFilterGraph2 ifg = new FilterGraph() as IFilterGraph2;

            ibf = m_rs as IBaseFilter;
            hr = ifg.AddFilter(ibf, "resizer");
            DsError.ThrowExceptionForHR(hr);

            devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            devs[1].Mon.BindToObject(null, null, ref g, out o);
            ibf2 = o as IBaseFilter;
            hr = ifg.AddFilter(ibf2, "camera");
            DsError.ThrowExceptionForHR(hr);

            ICaptureGraphBuilder2 cgb = new CaptureGraphBuilder2() as ICaptureGraphBuilder2;
            hr = cgb.SetFiltergraph(ifg);
            DsError.ThrowExceptionForHR(hr);

            hr = cgb.RenderStream(null, null, ibf2, null, ibf);
            DsError.ThrowExceptionForHR(hr);
        }
    }
}
