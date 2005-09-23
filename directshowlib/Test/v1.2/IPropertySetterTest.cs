// The docs for IPropertySetter say:
//
// Usually an application needs to call only the IPropertySetter::ClearProps method 
// to clear existing properties, and the IPropertySetter::AddProp method to add new 
// properties. The other methods on this interface are called by other DES components.
//
// However, I have tried to call all the methods in an effort to be thorough.  Note
// that although it is tempting, I did not change GetProps to try to return structs
// and arrays.  This is because FreeProps is needed to free them, so none of the
// standard marshalers are likely to work correctly.  And I couldn't see creating
// a custom marshaler for accessing interal methods.  As you can see, IntPtr works
// fine.
//
// I have also not written tests for SetProps.  The docs are so vague, I don't
// understand what they mean, so I can't figure out how to call it.  However,
// a visual inspection of the interface appears correct.
//
// I have also not written tests for LoadXML.  In order to test this interface, you
// need a class which implements IXMLDocument (an undocumented interface), then from
// this, mystically create IXMLElement objects, which may under (unknown) circumstances
// be passed to LoadXml.  Again, a visual inspection suggest that the definition is
// correct.


using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Text;

using DirectShowLib;
using DirectShowLib.DES;

namespace DirectShowLib.Test
{
    public class IPropertySetterTest
    {
        IPropertySetter m_ips;

        public IPropertySetterTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestAddProps();
                TestGetProps();
                TestBlob();
                TestClone();
                TestPrintXML();
            }
            finally
            {
                Marshal.ReleaseComObject(m_ips);
            }
        }

        private void TestAddProps()
        {
            int hr;
            DexterValue [] dv;
            DexterParam dp = new DexterParam();

            dp.Name = "Vol";
            dp.nValues = 2;

            dv = new DexterValue[2];
            dv[0] = new DexterValue();
            dv[0].v = 0;
            dv[0].rt = 0;
            dv[0].dwInterp = Dexterf.Jump;

            dv[1] = new DexterValue();
            dv[1].v = 6;
            dv[1].rt = 3452345;
            dv[1].dwInterp = Dexterf.Jump;

            hr = m_ips.AddProp(dp, dv);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestGetProps()
        {
            int hr;
            int iCnt;
            DexterParam dp2 = new DexterParam();
            DexterValue dv2, dv3;
            IntPtr ip1 = IntPtr.Zero, ip2 = IntPtr.Zero;
            hr = m_ips.GetProps(out iCnt, out ip1, out ip2);
            DESError.ThrowExceptionForHR(hr);

            dp2 = (DexterParam)Marshal.PtrToStructure(ip1, typeof(DexterParam));
            dv2 = (DexterValue)Marshal.PtrToStructure(ip2, typeof(DexterValue));
            dv3 = (DexterValue)Marshal.PtrToStructure((IntPtr)(ip2.ToInt32() + Marshal.SizeOf(typeof(DexterValue))), typeof(DexterValue));

            Debug.Assert(dv3.rt == 3452345, "GetProps");

            hr = m_ips.FreeProps(iCnt, ip1, ip2);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestBlob()
        {
            int hr;
            IntPtr ip;
            int i;
            IntPtr ip2;
            int i2;

            hr = m_ips.SaveToBlob(out i, out ip);
            DESError.ThrowExceptionForHR(hr);

            hr = m_ips.ClearProps();

            hr = m_ips.SaveToBlob(out i2, out ip2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i2 == 4, "Mostly empty blob");

            Marshal.FreeCoTaskMem(ip2);

            hr = m_ips.LoadFromBlob(i, ip);
            DESError.ThrowExceptionForHR(hr);

            hr = m_ips.SaveToBlob(out i2, out ip2);
            DESError.ThrowExceptionForHR(hr);

            Debug.Assert(i == i2, "Save/Restore blob");
            Marshal.FreeCoTaskMem(ip);
            Marshal.FreeCoTaskMem(ip2);
        }

        private void TestClone()
        {
            int hr;
            IPropertySetter ps;

            hr = m_ips.CloneProps(out ps, 0, 100000000);
            DESError.ThrowExceptionForHR(hr);

            hr = ps.ClearProps();
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestPrintXML()
        {
            int hr;
            int i;
            StringBuilder sb = new StringBuilder(5000);

            hr = m_ips.PrintXML(sb, sb.Capacity, out i, 4);
            DESError.ThrowExceptionForHR(hr);
        }

        private void TestSetProps()
        {
            //int hr;
            
            //hr = m_ips.SetProps(x, 1000);
            //DESError.ThrowExceptionForHR(hr);
        }

        private void Configure()
        {
            m_ips = (IPropertySetter)new PropertySetter();
        }
    }
}
