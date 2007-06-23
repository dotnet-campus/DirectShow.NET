// SelectParentalCountry->byte[]
// PlayAtTimeInTitle -> lpstruct
// SelectKaraokeAudioPresentationMode -> DvdKaraokeDownMix
// SelectVideoModePreference -> DvdPreferredDisplayMode
// PlayPeriodInTitleAutoStop -> lpstruct
// GetCurrentUOPS -> ValidUOPFlag

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
    public class IDvdControl2Test : Form
    {
        const int WM_GRAPHNOTIFY = 0x8000 + 1;

        // The drive containing testme.iso
        const string MyDisk = @"e:\video_ts";

        // A drive with a dvd containing more streams, etc than testme.iso
        const string OtherDisk = @"d:\video_ts";

        IDvdInfo2 m_idi2 = null;
        IDvdControl2 m_idc2 = null;
        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null; // don't release
        IMediaEventEx m_mediaEvent = null;

        public IDvdControl2Test()
        {
            // Set the window properties.  The window isn't displayed, but does receive events.
            this.Text = "IDvdControl2Test window";
            this.ClientSize = new Size(800, 450); // 16/9 aspect ratio to see colored borders
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }
        // Media events are sent to use as windows messages.  Used to test GetCmdFromEvent
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                    // If this is a windows media message
                case WM_GRAPHNOTIFY:
                    EventCode eventCode;
                    IntPtr p1, p2;
                    int hr;

                    hr = m_mediaEvent.GetEvent(out eventCode, out p1, out p2, 0);
                    while (hr == 0)
                    {
                        switch(eventCode)
                        {
                            case EventCode.DvdCmdEnd:
                            {
                                IDvdCmd pCmdObj = null;

                                hr = this.m_idi2.GetCmdFromEvent(p1, out pCmdObj);
                                Debug.WriteLine(String.Format("{0} status1", hr));
                                // Handle the event.
                                Debug.WriteLine(eventCode);
                                break;
                            }
                            case EventCode.DvdCmdStart:
                            {
                                IDvdCmd pCmdObj = null;

                                hr = this.m_idi2.GetCmdFromEvent(p1, out pCmdObj);
                                Debug.WriteLine(String.Format("{0} status2", hr));
                                // Handle the event.
                                Debug.WriteLine(eventCode);
                                break;
                            }
                            default:
                            {
                                Debug.WriteLine(eventCode);
                                break;
                            }
                        }

                        // Release parms
                        m_mediaEvent.FreeEventParams(eventCode, p1, p2);

                        // check for additional events
                        hr = m_mediaEvent.GetEvent(out eventCode, out p1, out p2, 0);
                    }
                    break;

                    // All other messages
                default:
                    // unhandled window message
                    base.WndProc(ref m);
                    break;
            }
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
                TestPlayTitle();
                TestPlayChapterInTitle();
                TestPlayAtTimeInTitle();
                TestPlayAtTime();
                TestPlayChapter();
                TestPlayPrevChapter();
                TestPlayNextChapter();
                TestReplayChapter();
                TestPlayForwards();
                TestPlayBackwards();
                TestPlayPeriodInTitleAutoStop();
                TestPlayChaptersAutoStop();
                TestMenusAndButtons();
                TestSetGPRM();
                TestPauseResume();
                TestParentalLevel();
                TestLanguage();
                TestVideoModePref();

                /// These routines need a fancier dvd than I can make with my dvd software.  I've tested
                /// using "The Thomas Crown Affair".  Note that TestDirectory changes the drive from MyDisk
                /// to OtherDisk.
                TestDirectory();
                TestSelectAudioStream();
                TestSelectSubpictureStream();
                TestSelectAngle();
                TestDefaultSubpictureLanguage();
                TestDefaultAudioLanguage();
                TestKaraokeAudioPresentationMode();
                TestState();
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


        void TestPlayTitle()
        {
            int hr;
            IDvdCmd ppCmd = null;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 2, "PlayTitle");
        }

        void TestPlayChapterInTitle()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 1, "PlayChapterInTitle");
            Debug.Assert(pLocation.ChapterNum == 2, "PlayChapterInTitle2");
        }

        void TestPlayAtTimeInTitle()
        {
            int hr;
            DvdHMSFTimeCode pStartTime = new DvdHMSFTimeCode();
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            pStartTime.bHours = 0;
            pStartTime.bMinutes = 0;
            pStartTime.bSeconds = 1;
            pStartTime.bFrames = 0;

            hr = m_idc2.PlayAtTimeInTitle(2, pStartTime, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 2, "TestPlayAtTimeInTitle");
            Debug.Assert(pLocation.TimeCode.bMinutes == 0, "TestPlayAtTimeInTitle2");
            Debug.Assert(pLocation.TimeCode.bSeconds == 2, "TestPlayAtTimeInTitle3");
        }

        void TestPlayAtTime()
        {
            int hr;
            DvdHMSFTimeCode pStartTime = new DvdHMSFTimeCode();
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            pStartTime.bHours = 0;
            pStartTime.bMinutes = 0;
            pStartTime.bSeconds = 1;
            pStartTime.bFrames = 0;

            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idc2.PlayAtTime(pStartTime, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 2, "TestPlayAtTime");
            Debug.Assert(pLocation.TimeCode.bMinutes == 0, "TestPlayAtTime2");
            Debug.Assert(pLocation.TimeCode.bSeconds == 2, "TestPlayAtTime3");
        }

        void TestPlayChapter()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idc2.PlayChapter(1, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 1, "TestPlayChapter");
            Debug.Assert(pLocation.ChapterNum == 1, "TestPlayChapter2");
        }

        void TestPlayPrevChapter()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idc2.PlayPrevChapter(DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 1, "TestPlayPrevChapter");
            Debug.Assert(pLocation.ChapterNum == 1, "TestPlayPrevChapter2");
        }

        void TestPlayNextChapter()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 1, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idc2.PlayNextChapter(DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 1, "TestPlayNextChapter");
            Debug.Assert(pLocation.ChapterNum == 2, "TestPlayNextChapter2");
        }

        void TestReplayChapter()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idc2.ReplayChapter(DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TitleNum == 1, "TestReplayChapter");
            Debug.Assert(pLocation.ChapterNum == 2, "TestReplayChapter2");

            // Time should reset, but doesn't.  However, the chapter does restart.
            //Debug.Assert(pLocation.TimeCode.bSeconds == 0, "TestReplayChapter4");
        }

        void TestPlayForwards()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.PlayForwards(7.5, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(2000);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TimeCode.bSeconds >= 5 && pLocation.TimeCode.bSeconds <= 6, "TestPlayForwards");
        }

        void TestPlayBackwards()
        {
            int hr;
            IDvdCmd ppCmd = null;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.PlayBackwards(1, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            // S_False means it can't play backwards (I'm guessing)
            if (hr != 1)
            {
                Thread.Sleep(2000);

                hr = m_idi2.GetCurrentLocation(out pLocation);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(pLocation.TimeCode.bSeconds == 0, "TestPlayBackwards");
            }

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);
        }

        void TestPlayPeriodInTitleAutoStop()
        {
            int hr;
            DvdHMSFTimeCode pStartTime = new DvdHMSFTimeCode();
            DvdHMSFTimeCode pStopTime = new DvdHMSFTimeCode();
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            pStartTime.bHours = 0;
            pStartTime.bMinutes = 0;
            pStartTime.bSeconds = 1;
            pStartTime.bFrames = 0;

            pStopTime.bHours = 0;
            pStopTime.bMinutes = 0;
            pStopTime.bSeconds = 4;
            pStopTime.bFrames = 0;

            hr = m_idc2.PlayPeriodInTitleAutoStop(2, pStartTime, pStopTime, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(3100);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            Debug.Assert(hr == DsResults.E_DVDInvalidDomain, "TestPlayPeriodInTitleAutoStop");
        }

        void TestPlayChaptersAutoStop()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdPlaybackLocation2 pLocation;

            AllowPlay();
            hr = m_idc2.PlayChaptersAutoStop(1, 1, 1, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(7000);

            // Should fail since the playing should be finished
            hr = m_idi2.GetCurrentLocation(out pLocation);
            Debug.Assert(hr == DsResults.E_DVDInvalidDomain, "TestPlayPeriodInTitleAutoStop");
        }

        void TestMenusAndButtons()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdDomain dvdd;
            int pulButtonsAvailable;
            int pulCurrentButton;

            // Top menu
            hr = m_idc2.ShowMenu(DvdMenuId.Title, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);
            Application.DoEvents();

            hr = m_idi2.GetCurrentDomain(out dvdd);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dvdd == DvdDomain.VideoManagerMenu, "TestMenusAndButtons");

            hr = m_idc2.SelectButton(2);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);
            Application.DoEvents();

            hr = m_idi2.GetCurrentButton(out pulButtonsAvailable, out pulCurrentButton);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulCurrentButton == 2, "TestMenusAndButtons2");
            Debug.Assert(pulButtonsAvailable == 2, "TestMenusAndButtons2a");

            // Button 1 leads to a chapter menu with 2 items
            hr = m_idc2.SelectButton(1);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);
            Application.DoEvents();

            hr = m_idc2.ActivateButton();
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.ReturnFromSubmenu(DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.SelectRelativeButton(DvdRelativeButton.Lower);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idi2.GetCurrentButton(out pulButtonsAvailable, out pulCurrentButton);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulCurrentButton == 2, "TestMenusAndButtons2");

            hr = m_idc2.SelectAndActivateButton(1);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.SelectButton(2);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.SelectAtPosition(new Point(130, 130));
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idi2.GetCurrentButton(out pulButtonsAvailable, out pulCurrentButton);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulCurrentButton == 1, "TestMenusAndButtons2");

            hr = m_idc2.ReturnFromSubmenu(DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);

            hr = m_idc2.ActivateAtPosition(new Point(130, 130));
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(100);
        }

        void TestSetGPRM()
        {
            int hr;
            IDvdCmd ppCmd;
            GPRMArray pGRegisterArray;

            hr = m_idc2.SetGPRM(13, 13, DvdCmdFlags.None, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetAllGPRMs(out pGRegisterArray);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pGRegisterArray.registers[13] == 13, "SetGPRM");
        }

        void TestPauseResume()
        {
            int hr;
            IDvdCmd ppCmd;
            DvdDomain dvdd;
            DvdPlaybackLocation2 pLocation, pLocation2;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.Pause(true);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(3000);

            hr = m_idi2.GetCurrentLocation(out pLocation);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(3000);

            hr = m_idi2.GetCurrentLocation(out pLocation2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TimeCode.bSeconds == pLocation2.TimeCode.bSeconds, "TestPauseResume");

            hr = m_idc2.Pause(false);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idi2.GetCurrentLocation(out pLocation2);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLocation.TimeCode.bSeconds != pLocation2.TimeCode.bSeconds, "TestPauseResume2");

            hr = m_idc2.ShowMenu(DvdMenuId.Root, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idi2.GetCurrentDomain(out dvdd);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dvdd == DvdDomain.VideoManagerMenu, "TestPauseResume3");

            hr = m_idc2.Resume(DvdCmdFlags.None, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idi2.GetCurrentDomain(out dvdd);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dvdd == DvdDomain.Title, "TestPauseResume4");

            hr = m_idc2.StillOff();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);
        }

        void TestParentalLevel()
        {
            int hr;
            int pulParentalLevel;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SelectParentalLevel(3);
            DsError.ThrowExceptionForHR(hr);

            byte[] cc2 = new byte[2];
            cc2[0] = 67; //'C'
            cc2[1] = 65; //'A'
            hr = m_idc2.SelectParentalCountry(cc2);
            DsError.ThrowExceptionForHR(hr);

            byte[] cc = new byte[3];
            hr = m_idi2.GetPlayerParentalLevel(out pulParentalLevel, cc);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(cc[0] == 'C' && cc[1] == 'A', "TestParentalLevel");
            Debug.Assert(pulParentalLevel == 3, "TestParentalLevel2");
        }

        void TestLanguage()
        {
            int hr;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SelectDefaultMenuLanguage(0x0809);
            DsError.ThrowExceptionForHR(hr);

            // Apparently several languages all map back to us english.
        }

        void TestVideoModePref()
        {
            int hr;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SelectVideoModePreference(DvdPreferredDisplayMode.DisplayContentDefault);
            DsError.ThrowExceptionForHR(hr);
        }

        //////////////////////////////////////////////////////
        void TestDirectory()
        {
            int hr;
            int pulDActualSize;
            StringBuilder sb = new StringBuilder(255, 255);
            StringBuilder sb2 = new StringBuilder(255, 255);

            hr = m_imc.Stop();
            hr = m_idc2.SetOption(DvdOptionFlag.ResetOnStop, true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SetOption(DvdOptionFlag.NotifyParentalLevelChange, true);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetDVDDirectory(sb, 255, out pulDActualSize);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SetDVDDirectory(null);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_imc.Stop();
            DsError.ThrowExceptionForHR(hr);

            // Map to a different drive.  One that has multiple streams, angles, etc
            hr = m_idc2.SetDVDDirectory(OtherDisk);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetDVDDirectory(sb2, 255, out pulDActualSize);
            DsError.ThrowExceptionForHR(hr);

            StartGraph();
        }

        void TestDefaultAudioLanguage()
        {
            int hr;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(1000);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SelectDefaultAudioLanguage(1036, DvdAudioLangExt.NotSpecified);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestDefaultSubpictureLanguage()
        {
            int hr;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SelectDefaultSubpictureLanguage(1036, DvdSubPictureLangExt.NotSpecified);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestSelectAudioStream()
        {
            int hr;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(2, 1, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            int maxstreams, curstream;
            hr = m_idi2.GetCurrentAudio(out maxstreams, out curstream);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(curstream == 0, "TestSelectAudioStream");

            hr = m_idc2.SelectAudioStream(maxstreams - 1, DvdCmdFlags.None, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentAudio(out maxstreams, out curstream);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(curstream == maxstreams - 1, "TestSelectAudioStream2");
        }

        void TestSelectSubpictureStream()
        {
            int hr;
            IDvdCmd ppCmd;
            int avail, current;
            bool dis;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.SelectSubpictureStream(1, DvdCmdFlags.None, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SetSubpictureState(true, DvdCmdFlags.None, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentSubpicture(out avail, out current, out dis);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(current == 1, "TestSelectSubpictureStream");
        }

        void TestSelectAngle()
        {
            int hr;
            IDvdCmd ppCmd;
            int avail, current;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idi2.GetCurrentAngle(out avail, out current);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.SelectAngle(avail, DvdCmdFlags.None, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentAngle(out avail, out current);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(current == avail, "SelectAngle");

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);
        }

        void TestKaraokeAudioPresentationMode()
        {
            int hr;
            IDvdCmd ppCmd;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(1000);

            // Returns an error if not Karaoke disk
            hr = m_idc2.SelectKaraokeAudioPresentationMode(DvdKaraokeDownMix.Mix_0to0);
            //DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.AcceptParentalLevelChange(true);
            DsError.ThrowExceptionForHR(hr);
        }

        void TestState()
        {
            int hr;
            IDvdCmd ppCmd;
            IDvdState dss;

            AllowPlay();
            hr = m_idc2.PlayChapterInTitle(1, 2, DvdCmdFlags.Flush | DvdCmdFlags.SendEvents, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idi2.GetState(out dss);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(3000);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            // Returns a State corrupted error.  C++ returns the same thing.
            hr = m_idc2.SetState(dss, DvdCmdFlags.Block, out ppCmd);
            //DsError.ThrowExceptionForHR(hr);

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
            hr = m_mediaEvent.SetNotifyWindow(this.Handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            return idgb;
        }
    }
}