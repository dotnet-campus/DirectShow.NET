using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using DirectShowLib;
using DirectShowLib.DMO;

namespace DirectShowLib.Test
{
    public class IDMOVideoOutputOptimizationsTest
    {
        IDMOVideoOutputOptimizations m_voo;

        public IDMOVideoOutputOptimizationsTest()
        {
        }

        public void DoTests()
        {
            Configure();

            try
            {
                TestQuery();
                TestOpMode();
                TestSample();
            }
            finally
            {
                Marshal.ReleaseComObject(m_voo);
            }
        }

        private void TestQuery()
        {
            int hr;
            DMOVideoOutputStream q;

            hr = m_voo.QueryOperationModePreferences(0, out q);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(q == DMOVideoOutputStream.NeedsPreviousSample, "Query");
        }

        private void TestOpMode()
        {
            int hr;
            DMOVideoOutputStream q;

            hr = m_voo.SetOperationMode(0, DMOVideoOutputStream.NeedsPreviousSample);
            DsError.ThrowExceptionForHR(hr);

            hr = m_voo.GetCurrentOperationMode(0, out q);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(q == DMOVideoOutputStream.NeedsPreviousSample, "OpMode");
        }

        private void TestSample()
        {
            int hr;
            DMOVideoOutputStream q;

            hr = m_voo.GetCurrentSampleRequirements(0, out q);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(q == DMOVideoOutputStream.NeedsPreviousSample, "Sample");
        }

        private void Configure()
        {
            int hr;
            IDMOWrapperFilter iDMO = (IDMOWrapperFilter)new DMOWrapperFilter();
            Guid g = new Guid("03be3ac4-84b7-4e0e-a78d-d3524e60395a");

            hr = iDMO.Init(g, Guid.Empty);
            m_voo = (IDMOVideoOutputOptimizations)iDMO;
        }
    }
}
