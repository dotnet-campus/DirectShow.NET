using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class IKsTopologyInfoTest
    {
        IKsTopologyInfo m_ks;
        KSTopologyConnection p;

        public void DoTests()
        {
            int hr;
            int i;
            Guid g;
            Guid g2 = new Guid("00000000-0000-0000-C000-000000000046");
            string s;
            object o;

            Configure();

            hr = m_ks.get_NumNodes(out i);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(i > 0, "NumNodes");

            hr = m_ks.get_NumConnections(out i);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(i > 0, "NumConnections");

            hr = m_ks.get_NumCategories(out i);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(i > 0, "NumCategories");

            hr = m_ks.get_NodeType(1, out g);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(i > 0, "Nodetype");

            hr = m_ks.get_Category(1, out g);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(i > 0, "Category");

            hr = m_ks.get_ConnectionInfo(2, out p);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(p.FromNode > 0, "ConnectionInfo");

            hr = m_ks.get_NodeName(1, IntPtr.Zero, 0, out i);
            Debug.Assert(hr == unchecked((int)0x800700ea), "NodeName"); // More data

            IntPtr ip = Marshal.AllocCoTaskMem(i);

            try
            {
                hr = m_ks.get_NodeName(1, ip, i, out i);
                s = Marshal.PtrToStringUni(ip);
                Debug.Assert(i > 0, "Nodename");
            }
            finally
            {
                Marshal.FreeCoTaskMem(ip);
            }

            hr = m_ks.CreateNodeInstance(1, g2, out o);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(o != null, "CreateNodeInstance");
        }

        private void Configure()
        {
            string s;
            DsDevice[] dd = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            dd[0].Mon.GetDisplayName(null, null, out s);

            m_ks = Marshal.BindToMoniker(s) as IKsTopologyInfo;
        }
    }
}

