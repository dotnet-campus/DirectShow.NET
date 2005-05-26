using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using DirectShowLib.Dvd;
using System.Drawing;
using System.Windows.Forms;

namespace DirectShowLib.Test
{
    [TestFixture]
    public class IPersistStreamTest
    {
        // The drive containing testme.iso
        const string MyDisk = @"e:\video_ts";

        IDvdInfo2 m_idi2 = null;
        IDvdControl2 m_idc2 = null;
        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null; // don't release
        IPersistStream m_ips = null;

        public IPersistStreamTest()
        {
        }
        /// <summary>
        /// Test all IDvdControl2Test methods
        /// </summary>
        [Test]
        public void DoTests()
        {
            IDvdGraphBuilder idgb = GetDvdGraph();
            try
            {
                PopulateMembers(idgb);
                StartGraph();
                AllowPlay();

                TestGetClassID();
                TestIsDirty();
            }
            finally
            {
                if (m_ROT != null)
                {
                    m_ROT.Dispose();
                }
                if (idgb != null)
                {
                    Marshal.ReleaseComObject(idgb);
                    idgb = null;
                }
                if (m_idi2 != null)
                {
                    Marshal.ReleaseComObject(m_idi2);
                    m_idi2 = null;
                }
                if (m_idc2 != null)
                {
                    Marshal.ReleaseComObject(m_idc2);
                    m_idc2 = null;
                }
            }
        }

        // Grab off a couple of interface pointers
        void PopulateMembers(IDvdGraphBuilder idgb)
        {
            int hr;
            object obj;
            hr = idgb.GetDvdInterface(typeof(IDvdInfo2).GUID, out obj);
            DsError.ThrowExceptionForHR(hr);

            // Get the IDvdGraphBuilder interface
            m_idi2 = obj as IDvdInfo2;

            hr = idgb.GetDvdInterface(typeof(IDvdControl2).GUID, out obj);
            DsError.ThrowExceptionForHR(hr);

            // Get the IDvdGraphBuilder interface
            m_idc2 = obj as IDvdControl2;
        }

        // Start the dvd graph.  Wait til a menu appears
        void StartGraph()
        {
            int hr;
            DvdDomain dvdd;

            hr = m_imc.Run();
            DsError.ThrowExceptionForHR(hr);

            do
            {
                hr = m_idi2.GetCurrentDomain(out dvdd);
                DsError.ThrowExceptionForHR(hr);
                Application.DoEvents();
                Thread.Sleep(100);
            } while (dvdd != DvdDomain.VideoManagerMenu && dvdd != DvdDomain.VideoTitleSetMenu);

        }

        // Put us in a mode that allows for playing video
        void AllowPlay()
        {
            int hr;
            int buttonavail, curbutton;
            DvdDomain dvdd;
            IDvdState dss;

            // Keep clicking buttons until we start playing a title
            while ((hr = m_idi2.GetCurrentDomain(out dvdd)) == 0 &&
                ((dvdd == DvdDomain.VideoManagerMenu) || (dvdd == DvdDomain.VideoTitleSetMenu)))
            {
                hr = m_idi2.GetCurrentButton(out buttonavail, out curbutton);
                DsError.ThrowExceptionForHR(hr);

                if (curbutton > 0)
                {
                    hr = m_idc2.SelectAndActivateButton(1);
                    DsError.ThrowExceptionForHR(hr);
                }

                Thread.Sleep(500);
            }

            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetState(out dss);
            DsError.ThrowExceptionForHR(hr);

            m_ips = dss as IPersistStream;
       }


        void TestGetClassID()
        {
            int hr;
            Guid Clsid;

            hr = m_ips.GetClassID(out Clsid);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(Clsid == typeof(DVDState).GUID, "GetClassID");
        }

        void TestIsDirty()
        {
            int hr;

            hr = m_ips.IsDirty();

            // zero means it hasn't been saved (dirty)
            Debug.Assert(hr == 0, "IsDirty");
        }

        void TestSave()
        {
            int hr;

            //hr = m_ips.Save(
        }

        void TestLoad()
        {
        }

        void TestGetSizeMax()
        {
        }

        IDvdGraphBuilder GetDvdGraph()
        {
            int hr;
            DvdGraphBuilder dgb = null;
            IGraphBuilder gb = null;
            AMDvdRenderStatus drs;
            IDvdGraphBuilder idgb = null;

            // Get a dvd graph object
            dgb = new DvdGraphBuilder();
            Debug.Assert(dgb != null, "new DvdGraphBuilder");

            // Get the IDvdGraphBuilder interface
            idgb = dgb as IDvdGraphBuilder;

            hr = idgb.RenderDvdVideoVolume(MyDisk, AMDvdGraphFlags.HWDecPrefer, out drs);
            DsError.ThrowExceptionForHR(hr);

            // If there is no dvd in the player, you get hr == S_FALSE (1)
            Debug.Assert(hr == 0, "Can't find dvd");

            // Get an IFilterGraph interface
            hr = idgb.GetFiltergraph(out gb);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(gb != null, "GetFiltergraph");
            m_ROT = new DsROTEntry(gb);

            m_imc = gb as IMediaControl;

            return idgb;
        }
    }
}