// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/vidcapstream/hh/vidcapstream/vidcapprop_bcfac64d-72fe-4b45-b3e4-1c4f557957d7.xml.asp

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IKsPropertySetTest
    {
        private readonly Guid PROPSETID_VIDCAP_TVAUDIO = new Guid(0x6a2e0650, 0x28e4, 0x11d0, 0xa1, 0x8c, 0x00, 0xa0, 0xc9, 0x11, 0x89, 0x56);
        IKsPropertySet m_ps;

        enum KSPROPERTY_VIDCAP_TVAUDIO
        {
            KSPROPERTY_TVAUDIO_CAPS,                            // R
            KSPROPERTY_TVAUDIO_MODE,                            // RW
            KSPROPERTY_TVAUDIO_CURRENTLY_AVAILABLE_MODES        // R
        }

        // This is used to signal when the callbacks are called
        public IKsPropertySetTest()
        {
        }

        [Test]
        public void DoTests()
        {
            BuildGraph();

            TestQuery();
            TestGet();
            TestPut();
        }

        void TestQuery()
        {
            int hr;
            KSPropertySupport pts;

            hr = m_ps.QuerySupported(PROPSETID_VIDCAP_TVAUDIO, (int)KSPROPERTY_VIDCAP_TVAUDIO.KSPROPERTY_TVAUDIO_MODE, out pts);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pts == (KSPropertySupport.Get | KSPropertySupport.Set), "QuerySupported");
    }

        void TestGet()
        {
            int hr;
            int ret;

            int iSize = 32; //Marshal.SizeOf(typeof(int));

            IntPtr ip1 = Marshal.AllocCoTaskMem(iSize);
            IntPtr ip2 = Marshal.AllocCoTaskMem(iSize);

            for (int x=0; x < iSize; x++)
            {
                Marshal.WriteByte(ip1, x, 65);
                Marshal.WriteByte(ip2, x, 65);
            }

            hr = m_ps.Get(
                PROPSETID_VIDCAP_TVAUDIO, 
                (int)KSPROPERTY_VIDCAP_TVAUDIO.KSPROPERTY_TVAUDIO_MODE,
                ip1, 
                iSize, 
                ip2, 
                iSize, 
                out ret);

            DsError.ThrowExceptionForHR(hr);

            int ival = Marshal.ReadInt32(ip2, 24);

            Debug.Assert(ival > 0, "Get");

            Marshal.FreeCoTaskMem(ip2);
            Marshal.FreeCoTaskMem(ip1);
        }

        void TestPut()
        {
            int hr;

            int iSize = 32; //Marshal.SizeOf(typeof(int));

            IntPtr ip1 = Marshal.AllocCoTaskMem(iSize);
            IntPtr ip2 = Marshal.AllocCoTaskMem(iSize);

            for (int x=0; x < iSize; x++)
            {
                Marshal.WriteByte(ip1, x, 0);
                Marshal.WriteByte(ip2, x, 0);
            }

            Marshal.WriteInt32(ip1, 24, 1);
            Marshal.WriteInt32(ip2, 24, 1);

            hr = m_ps.Set(
                PROPSETID_VIDCAP_TVAUDIO, 
                (int)KSPROPERTY_VIDCAP_TVAUDIO.KSPROPERTY_TVAUDIO_MODE,
                ip1, 
                iSize, 
                ip2, 
                iSize);

            DsError.ThrowExceptionForHR(hr);

            Marshal.FreeCoTaskMem(ip2);
            Marshal.FreeCoTaskMem(ip1);
        }

        void BuildGraph()
        {
            int hr;
            IBaseFilter ppFilter;

            ArrayList devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            DsDevice dev = devs[0] as DsDevice;

            IGraphBuilder graphBuilder = new FilterGraph() as IGraphBuilder;
            DsROTEntry ds = new DsROTEntry(graphBuilder);
            IFilterGraph2 ifg2 = graphBuilder as IFilterGraph2;

            hr = ifg2.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out ppFilter);
            DsError.ThrowExceptionForHR(hr);

            m_ps = ppFilter as IKsPropertySet;
        }
    }
}