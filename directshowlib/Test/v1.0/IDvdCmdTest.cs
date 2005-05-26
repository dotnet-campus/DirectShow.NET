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
    public class IDvdCmdTest
    {
        // The drive containing testme.iso
        const string MyDisk = @"e:\video_ts";

        IDvdInfo2 m_idi2 = null;
        IDvdControl2 m_idc2 = null;
        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null; // don't release
        IMediaEventEx m_mediaEvent = null;

        public IDvdCmdTest()
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

                // These routines can be tested with testme.iso.  If you've got
                // nero, you can use NeroDrive to mount the iso file as a dvd drive.  Otherwise
                // burn the file to a dvd for testing.
                TestWaitForStart();
                TestWaitForEnd();
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
        }


        void TestWaitForStart()
        {
            int hr;
            IDvdCmd ppCmd = null;

            AllowPlay();
            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            // Only takes fraction of a second
            hr = ppCmd.WaitForStart();  
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);
        }

        void TestWaitForEnd()
        {
            int hr;
            IDvdCmd ppCmd = null;

            AllowPlay();
            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            // Only takes fraction of a second.  Does NOT wait for title to finish.  Only waits
            // for Graph to finish processing command.
            hr = ppCmd.WaitForEnd();  
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);
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

            m_mediaEvent = gb as IMediaEventEx;

            return idgb;
        }
    }
}