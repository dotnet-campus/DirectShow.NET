using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Drawing;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IAMCrossbarTest
    {
        IAMCrossbar m_ixbar = null;

        public IAMCrossbarTest()
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
                TestPinCounts();
                TestCanRoute();
                TestIsRouted();
                TestCrossbarInfo();
            }
            finally
            {
            }
        }

        void TestPinCounts()
        {
            int hr;
            int opin, ipin;

            hr = m_ixbar.get_PinCounts(out opin, out ipin);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(opin > 0, "PinCount output");
            Debug.Assert(ipin > 0, "PinCount input");
        }

        void TestCanRoute()
        {
            int hr;
            int opin, ipin;

            hr = m_ixbar.get_PinCounts(out opin, out ipin);
            DsError.ThrowExceptionForHR(hr);

            for(int x=0; x < ipin; x++)
            {
                for(int y=0; y < opin; y++)
                {
                    hr = m_ixbar.CanRoute(y, x);
                    if (hr == 0)
                    {
                        // If we can route, try it
                        hr = m_ixbar.Route(y, x);
                        Debug.Assert(hr == 0);
                    }
                    else if (hr == 1)
                    {
                        // If we can't route, we should get S_FALSE
                        hr = m_ixbar.Route(y, x);
                        Debug.Assert(hr == 1);
                    }
                    else
                    {
                        DsError.ThrowExceptionForHR(hr);
                    }
                }
            }
        }

        void TestIsRouted()
        {
            int hr;
            int opin, ipin;
            int iIsRouted;

            hr = m_ixbar.get_PinCounts(out opin, out ipin);
            DsError.ThrowExceptionForHR(hr);

            for(int y=0; y < opin; y++)
            {
                hr = m_ixbar.get_IsRoutedTo(y, out iIsRouted);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(iIsRouted < ipin && iIsRouted >= 0, "IsRouted");
            }
        }

        void TestCrossbarInfo()
        {
            int hr;
            PhysicalConnectorType pctype;
            int opin, ipin;
            int iIsRelated;

            hr = m_ixbar.get_PinCounts(out opin, out ipin);
            DsError.ThrowExceptionForHR(hr);

            for(int x=0; x < ipin; x++)
            {
                hr = m_ixbar.get_CrossbarPinInfo(true, x, out iIsRelated, out pctype);
                DsError.ThrowExceptionForHR(hr);
            }

            for(int y=0; y < opin; y++)
            {
                hr = m_ixbar.get_CrossbarPinInfo(false, y, out iIsRelated, out pctype);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        void BuildGraph()
        {
            DsDevice [] devs;
            string s;

            devs = DsDevice.GetDevicesOfCat(FilterCategory.AMKSCrossbar);
            DsDevice dev = devs[0];

            dev.Mon.GetDisplayName(null, null, out s);
            m_ixbar = Marshal.BindToMoniker( s ) as IAMCrossbar;
        }
    }
}