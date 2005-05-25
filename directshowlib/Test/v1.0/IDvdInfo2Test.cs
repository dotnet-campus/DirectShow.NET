// These methods are tested in IDvdControl2:
//      GetCurrentLocation
//      GetCurrentButton
//      GetAllGPRMs
//      GetPlayerParentalLevel
//      GetState
//      GetCmdFromEvent

// Changes to IDvdInfo2:
// GetPlayerParentalLevel -> LPArray, SizeConst
// GetKaraokeAttributes -> custom marshaler
// GetTitleAttributes -> custom marshaler
// GetCurrentUOPS -> ValidUOPFlag
// GetDVDTextStringAsNative -> string
// GetDVDTextStringAsUnicode -> stringbuilder
// GetTitleParentalLevels -> DvdParentalLevel
// GetDVDDirectory -> stringbuilder
// GetMenuLanguages -> LPArray

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
    public class IDvdInfo2Test
    {
        // The drive containing testme.iso
        const string MyDisk = @"e:\video_ts";

        // A drive with a dvd containing more streams, etc than testme.iso
        const string OtherDisk = @"d:\video_ts";

        IDvdInfo2 m_idi2 = null;
        IDvdControl2 m_idc2 = null;
        DsROTEntry m_ROT = null;
        IMediaControl m_imc = null; // don't release

        public IDvdInfo2Test()
        {
        }

        /// <summary>
        /// Test all IDvdInfo2Test methods
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
                TestGetCurrentDomain();
                TestGetTotalTitleTime();
                TestGetCurrent();
                TestDiskInfo();
                TestTitle();
                TestButton();
                TestMisc();

                /// These routines need a fancier dvd than I can make with my dvd software.  I've tested
                /// using "The Thomas Crown Affair".  Note that TestDirectory changes the drive from MyDisk
                /// to OtherDisk.

                // Map to a different drive.  One that has multiple streams, angles, etc
                int hr;
                hr = m_imc.Stop();
                DsError.ThrowExceptionForHR(hr);
                hr = m_idc2.SetDVDDirectory(OtherDisk);
                DsError.ThrowExceptionForHR(hr);
                StartGraph();

                TestStrings();
                TestMenuLang();
                TestAudio();
                TestSubPic();
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

        void TestGetCurrentDomain()
        {
            int hr;
            DvdDomain dvdd;

            hr = m_idi2.GetCurrentDomain(out dvdd);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dvdd == DvdDomain.VideoManagerMenu, "GetCurrentDomain");

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentDomain(out dvdd);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dvdd == DvdDomain.Stop, "GetCurrentDomain2");
        }

        void TestGetTotalTitleTime()
        {
            int hr;
            IDvdCmd ppCmd = null;
            DvdHMSFTimeCode dhtc = new DvdHMSFTimeCode();
            DvdTimeCodeFlags dtcf = new DvdTimeCodeFlags();

            AllowPlay();

            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Thread.Sleep(500);

            hr = m_idi2.GetTotalTitleTime(dhtc, out dtcf);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(dhtc.bFrames == 13, "GetTotalTitleTime");
            Debug.Assert(dhtc.bSeconds == 12, "GetTotalTitleTime2");
            Debug.Assert(dtcf == DvdTimeCodeFlags.FPS30, "GetTotalTitleTime3");
        }

        void TestGetCurrent()
        {
            int hr;
            IDvdCmd ppCmd = null;
            int pulAnglesAvailable;
            int pulCurrentAngle;
            int pulStreamsAvailable;
            int pulCurrentStream;
            int pulSubAvailable;
            int pulCurrentSub;
            bool pbIsDisabled;
            ValidUOPFlag pulUOPs;

            AllowPlay();
            hr = m_idc2.PlayTitle(2, DvdCmdFlags.Flush, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentAngle(out pulAnglesAvailable, out pulCurrentAngle);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentAudio(out pulStreamsAvailable, out pulCurrentStream);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentSubpicture(out pulSubAvailable, out pulCurrentSub, out pbIsDisabled);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idi2.GetCurrentUOPS(out pulUOPs);
            DsError.ThrowExceptionForHR(hr);

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulAnglesAvailable == 1, "GetCurrentAngle");
            Debug.Assert(pulCurrentAngle == 1, "GetCurrentAngle2");

            Debug.Assert(pulStreamsAvailable == 1, "GetCurrentAudio");
            Debug.Assert(pulCurrentStream == 0, "GetCurrentAudio2");

            Debug.Assert(pulSubAvailable == 0, "GetCurrentSubpicture");
            //Debug.Assert(pulCurrentSub == 1, "GetCurrentSubpicture2");
            Debug.Assert(pbIsDisabled, "GetCurrentSubpicture3");

            Debug.Assert(pulUOPs == 0);
        }


        void TestStrings()
        {
            int hr;
            int pulwActualSize;
            int pulActualSize;
            DvdTextStringType pType;
            DvdTextStringType pwType;
            int pulNumOfLangs;
            int pulNumOfStrings;
            int pLangCode;
            DvdTextCharSet pbCharacterSet;

            hr = m_idi2.GetDVDTextNumberOfLanguages(out pulNumOfLangs);
            DsError.ThrowExceptionForHR(hr);

            if (pulNumOfLangs > 0)
            {
                hr = m_idi2.GetDVDTextLanguageInfo(0, out pulNumOfStrings, out pLangCode, out pbCharacterSet);
                DsError.ThrowExceptionForHR(hr);

                StringBuilder sb1 = new StringBuilder(255, 255);
                hr = m_idi2.GetDVDTextStringAsNative(0, pulNumOfStrings - 1, sb1, sb1.Capacity, out pulActualSize, out pType);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(sb1.Length > 0, "GetDVDTextStringAsNative");

                StringBuilder sb2 = new StringBuilder(255, 255);
                hr = m_idi2.GetDVDTextStringAsUnicode(0, pulNumOfStrings - 1, sb2, sb2.Capacity, out pulwActualSize, out pwType);
                DsError.ThrowExceptionForHR(hr);

                Debug.Assert(sb2.Length > 0, "GetDVDTextStringAsUnicode");
            }
        }

        void TestMenuLang()
        {
            int hr;

            int pulActualLanguages;
            hr = m_idi2.GetMenuLanguages(null, 0, out pulActualLanguages);
            DsError.ThrowExceptionForHR(hr);

            int [] pLanguages = new int[pulActualLanguages + 1];
            hr = m_idi2.GetMenuLanguages(pLanguages, pLanguages.Length, out pulActualLanguages);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pLanguages[0] == 1033, "GetMenuLanguages");

            int pMLLanguage;
            hr = m_idi2.GetDefaultMenuLanguage(out pMLLanguage);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pMLLanguage == 1033, "GetDefaultMenuLanguage");
        }

        void TestAudio()
        {
            int hr;

            int pLanguage;
            hr = m_idi2.GetAudioLanguage(0, out pLanguage);
            DsError.ThrowExceptionForHR(hr);
            DvdAudioAttributes pAATR;

            Debug.Assert(pLanguage == 1033, "TestAudio");

            int pALLanguage;
            DvdAudioLangExt pAudioExtension;
            hr = m_idi2.GetDefaultAudioLanguage(out pALLanguage, out pAudioExtension);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pALLanguage == -1, "TestAudio2");

            hr = m_idi2.GetAudioAttributes(0, out pAATR);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pAATR.dwFrequency == 48000, "TestAudio3");

            bool pbEnabled;
            hr = m_idi2.IsAudioStreamEnabled(0, out pbEnabled);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pbEnabled, "TestAudio4");
        }

        void TestSubPic()
        {
            int hr;
            int pSLanguage;
            DvdSubpictureAttributes pSATR;

            hr = m_idi2.GetSubpictureLanguage(0, out pSLanguage);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pSLanguage == 1033, "TestSubPic");

            int pSPLLanguage;
            DvdSubPictureLangExt pSubpictureExtension;
            hr = m_idi2.GetDefaultSubpictureLanguage(out pSPLLanguage, out pSubpictureExtension);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pSPLLanguage == -1, "TestSubPic2");

            hr = m_idi2.GetSubpictureAttributes(1, out pSATR);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pSATR.Language == 1036, "TestSubPic3");

            bool pbSSEEnabled;
            hr = m_idi2.IsSubpictureStreamEnabled(1, out pbSSEEnabled);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pbSSEEnabled == false, "TestSubPic4");
        }

        void TestDiskInfo()
        {
            int hr;
            int pulNumOfVolumes;
            int pulVolume;
            DvdDiscSide pSide;
            int pulNumOfTitles;
            int pulDActualSize;

            hr = m_idi2.GetDVDVolumeInfo(out pulNumOfVolumes, out pulVolume, out pSide, out pulNumOfTitles);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulNumOfTitles == 2, "TestDiskInfo");

            StringBuilder sb = new StringBuilder(255, 255);
            hr = m_idi2.GetDVDDirectory(sb, sb.Capacity, out pulDActualSize);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(sb.ToString() == MyDisk, "TestDiskInfo2");

            long l1;
            hr = m_idi2.GetDiscID(sb.ToString(), out l1);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(l1 == 6633412489990086489, "TestDiskInfo3");

            DvdMenuAttributes pMATR = new DvdMenuAttributes();
            hr = m_idi2.GetVMGAttributes(out pMATR);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pMATR.VideoAttributes.sourceResolutionX == 720, "TestDiskInfo4");
        }

        void TestTitle()
        {
            int hr;
            DvdParentalLevel pulTParentalLevels;
            int pulNumOfChapters;

            hr = m_idi2.GetNumberOfChapters(1, out pulNumOfChapters);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulNumOfChapters == 2, "TestTitle");

            hr = m_idi2.GetTitleParentalLevels(1, out pulTParentalLevels);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulTParentalLevels ==
                (DvdParentalLevel.Level1 | DvdParentalLevel.Level2 | DvdParentalLevel.Level3 |
                DvdParentalLevel.Level4 | DvdParentalLevel.Level5 | DvdParentalLevel.Level6 |
                DvdParentalLevel.Level7 | DvdParentalLevel.Level8),
                "TestTitle2");

            DvdDecoderCaps pCaps = new DvdDecoderCaps();
            pCaps.dwSize = Marshal.SizeOf(pCaps);
            hr = m_idi2.GetDecoderCaps(ref pCaps);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pCaps.dwAudioCaps ==
                (DvdAudioCaps.AC3 | DvdAudioCaps.MPEG2 | DvdAudioCaps.DTS | DvdAudioCaps.SDDS),
                "TestTitle3");

            DvdMenuAttributes pMenu = new DvdMenuAttributes();
            DvdTitleAttributes pTitle = new DvdTitleAttributes();

            hr = m_idi2.GetTitleAttributes(1, out pMenu, pTitle);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pMenu.VideoAttributes.sourceResolutionX == 720, "TestDiskInfo4");
            Debug.Assert(pTitle.ulNumberOfAudioStreams == 1, "TestTitle5");
        }

        void TestButton()
        {
            int hr;
            int pulButtonIndex;
            IDvdCmd ppCmd = null;

            hr = m_idc2.ShowMenu(DvdMenuId.Title, DvdCmdFlags.Flush, out ppCmd);
            DsError.ThrowExceptionForHR(hr);

            Rectangle pRect;
            hr = m_idi2.GetButtonRect(1, out pRect);
            //ThrowExceptionForHR(hr);

            hr = m_idi2.GetButtonAtPosition(new Point(130, 130), out pulButtonIndex);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pulButtonIndex == 1, "TestButton");
        }

        void TestMisc()
        {
            int hr;

            DvdVideoAttributes pVATR;
            SPRMArray pRegisterArray;

            hr = m_idi2.GetAllSPRMs(out pRegisterArray);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pRegisterArray.registers[0] == 25966, "GetAllSPRMs");

            hr = m_idi2.GetCurrentVideoAttributes(out pVATR);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(pVATR.sourceResolutionX == 720, "GetCurrentVideoAttributes");

            // This won't work for non-karaoke disks
            DvdKaraokeAttributes pKATR = new DvdKaraokeAttributes();
            hr = m_idi2.GetKaraokeAttributes(0, pKATR);
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

            // Test RenderDvdVideoVolume
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

            Thread.Sleep(500);
        }

        // Get past all the menus to a point where titles can be played.
        void AllowPlay()
        {
            int hr;
            int buttonavail, curbutton;
            IDvdCmd ppCmd = null;
            DvdDomain dvdd;

            hr = m_idi2.GetCurrentDomain(out dvdd);
            DsError.ThrowExceptionForHR(hr);

            if (dvdd == DvdDomain.Stop)
            {
                hr = m_idc2.Resume(DvdCmdFlags.Flush, out ppCmd);
                if (hr < 0)
                {
                    hr = m_idc2.ShowMenu(DvdMenuId.Title, DvdCmdFlags.Flush, out ppCmd);
                    DsError.ThrowExceptionForHR(hr);
                }
            }

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

            hr = m_idc2.Stop();
            DsError.ThrowExceptionForHR(hr);

        }

    }
}