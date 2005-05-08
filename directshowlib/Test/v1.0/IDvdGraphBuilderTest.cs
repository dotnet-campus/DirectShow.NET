// Changed GetDvdInterface to use UnmanagedType.LPStruct so 
//    it is not necessary to use ref.

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using DirectShowLib.Dvd;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IDvdGraphBuilderTest
    {
        public IDvdGraphBuilderTest()
        {
        }

        /// <summary>
        /// Test all IDvdGraphBuilderTest methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            int hr;

            DvdGraphBuilder dgb = null;
            IDvdGraphBuilder idgb = null;
            IGraphBuilder gb = null;
            IBasicAudio iba = null;
            AMDvdRenderStatus drs;
            object obj;

            // Get a dvd graph object
            dgb = new DvdGraphBuilder();
            Debug.Assert(dgb != null, "new DvdGraphBuilder");

            try
            {
                // Get the IDvdGraphBuilder interface
                idgb = dgb as IDvdGraphBuilder;

                // Test RenderDvdVideoVolume
                hr = idgb.RenderDvdVideoVolume(null, AMDvdGraphFlags.HWDecPrefer, out drs);
                Marshal.ThrowExceptionForHR(hr);

                // If there is no dvd in the player, you get hr == S_FALSE (1)
                Debug.Assert(hr == 0 || (hr == 1 && drs.bDvdVolUnknown), "RenderDvdVideoVolume");

                // Get an IFilterGraph interface
                hr = idgb.GetFiltergraph(out gb);
                Marshal.ThrowExceptionForHR(hr);
                Debug.Assert(gb != null, "GetFiltergraph");

                // GetDvdInterface allows for retrieving one of a variety
                // of interfaces.  Try getting an IBasicAudio.
                hr = idgb.GetDvdInterface(typeof(IBasicAudio).GUID, out obj);
                Marshal.ThrowExceptionForHR(hr);

                // See if it is an IBasicAudio
                iba = obj as IBasicAudio;
                obj = null;
                Debug.Assert(iba != null);
            }
            finally
            {
                // Release everything
                if (dgb != null)
                {
                    Marshal.ReleaseComObject(dgb);
                    dgb = null;
                }
                if (gb != null)
                {
                    Marshal.ReleaseComObject(gb);
                    gb = null;
                }
                if (iba != null)
                {
                    Marshal.ReleaseComObject(iba);
                    iba = null;
                }
            }
        }
    }
}