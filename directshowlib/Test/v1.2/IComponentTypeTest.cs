using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib.BDA;
using DirectShowLib;

namespace DirectShowLib.Test
{
    public class IComponentTypeTest
    {
        private IComponentType m_compType;

        public IComponentTypeTest()
        {
        }

        public void DoTests()
        {
            Config();

            try
            {
                Test__MediaFormatType();
                Test__MediaMajorType();
                Test__MediaSubType();
                TestMediaFormatType();
                TestMediaMajorType();
                TestMediaSubType();
                TestCategory();
                TestMediaType();
                TestClone();
            }
            finally
            {
                Marshal.ReleaseComObject(m_compType);
            }
        }

        private void Test__MediaFormatType()
        {
            int hr;
            Guid g;

            hr = m_compType.put__MediaFormatType(FormatType.WaveEx);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get__MediaFormatType(out g);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == FormatType.WaveEx, "__MediaFormatType");
        }

        private void Test__MediaMajorType()
        {
            int hr;
            Guid g;

            hr = m_compType.put__MediaMajorType(MediaType.URLStream);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get__MediaMajorType(out g);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaType.URLStream, "__MediaMajorType");
        }

        private void Test__MediaSubType()
        {
            int hr;
            Guid g;

            hr = m_compType.put__MediaSubType(MediaSubType.WAKE);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get__MediaSubType(out g);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(g == MediaSubType.WAKE, "__MediaSubType");
        }

        private void TestMediaFormatType()
        {
            int hr;
            string s = FormatType.AnalogVideo.ToString("B").ToUpper();
            string s2;

            hr = m_compType.put_MediaFormatType(s);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get_MediaFormatType(out s2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == s2, "MediaFormatType");
        }

        private void TestMediaMajorType()
        {
            int hr;
            string s = MediaType.Texts.ToString("B").ToUpper();
            string s2;

            hr = m_compType.put_MediaMajorType(s);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get_MediaMajorType(out s2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == s2, "MediaMajorType");
        }

        private void TestMediaSubType()
        {
            int hr;
            string s = MediaSubType.WebStream.ToString("B").ToUpper();
            string s2;

            hr = m_compType.put_MediaSubType(s);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get_MediaSubType(out s2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(s == s2, "MediaSubType");
        }

        private void TestCategory()
        {
            int hr;
            ComponentCategory c;

            hr = m_compType.put_Category(ComponentCategory.Data);
            DsError.ThrowExceptionForHR(hr);

            hr = m_compType.get_Category(out c);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(c == ComponentCategory.Data);
        }
        private void TestMediaType()
        {
            int hr;
            AMMediaType mt = new AMMediaType();

            hr = m_compType.get_MediaType(mt);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(mt.majorType == MediaType.Texts, "get_MediaType");

            hr = m_compType.put_MediaType(mt);
            DsError.ThrowExceptionForHR(hr);
        }
        private void TestClone()
        {
            int hr;
            IComponentType it;

            hr = m_compType.Clone(out it);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(it != null, "Clone");
        }

        private void Config()
        {
            m_compType = (IComponentType) new ComponentType();
        }
    }
}
