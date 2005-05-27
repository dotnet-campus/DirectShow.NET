// Added PinMedium to Misc.cs

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IKsPinTest
    {
        private IFilterGraph2 m_graphBuilder;

        private IPin m_IPinOut;

        // This is used to signal when the callbacks are called
        public IKsPinTest()
        {
        }

        /// <summary>
        /// Test all IKsPin methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            m_graphBuilder = BuildGraph();

            DumpPinMediumTest();

            // All done.  Release everything
            if (m_graphBuilder != null)
            {
                Marshal.ReleaseComObject(m_graphBuilder);
                m_graphBuilder = null;
            }
        }

        private void DumpPinMediumTest()
        {
            int hr;

            IntPtr ip = IntPtr.Zero;
            IKsPin pKsPin = m_IPinOut as IKsPin;
            KSMultipleItem pmi = new KSMultipleItem();

            // KsQueryMediums returns a KSMultipleItem immediately followed by
            // KSMultipleItem.Count instances of PinMedium
            hr = pKsPin.KsQueryMediums(out ip);
            Marshal.ThrowExceptionForHR(hr);

            try
            {
                // Read the KSMultipleItem
                Marshal.PtrToStructure(ip, pmi);
                Debug.Assert(pmi.Count > 0, "Medium count");

                if (pmi.Count > 0) 
                {
                    PinMedium pTemp = new PinMedium();
                    ip = (IntPtr)(ip.ToInt32() + Marshal.SizeOf(typeof(KSMultipleItem)));

                    for (int i = 0; i < pmi.Count; i++) 
                    {
                        // Read the PinMedium
                        Marshal.PtrToStructure(ip, pTemp);
                        Debug.WriteLine(string.Format("{0} ({1}) ({2})", pTemp.clsMedium, pTemp.dw1, pTemp.dw2));
                    }
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ip);
            }
        }

        private IFilterGraph2 BuildGraph()
        {
            int hr;
            IBaseFilter ibfAVISource = null;
            IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;

            try
            {
                ArrayList capDevices;

                // Get the collection of video devices
                capDevices = DsDevice.GetDevicesOfCat( FilterCategory.VideoInputDevice);
                if( capDevices.Count == 0 )
                {
                    throw new Exception("No video capture devices found!");
                }

                DsDevice dev = capDevices[0] as DsDevice;

                // Add it to the graph
                hr = graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, "Ds.NET CaptureDevice", out ibfAVISource);
                Marshal.ThrowExceptionForHR(hr);

                m_IPinOut = DsFindPin.ByDirection(ibfAVISource, PinDirection.Output, 0);

            }
            catch
            {
                Marshal.ReleaseComObject(graphBuilder);
                throw;
            }
            finally
            {
                Marshal.ReleaseComObject(ibfAVISource);
            }

            return graphBuilder;
        }

    }
}