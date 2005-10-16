#region license

/*
DirectShowLib - Provide access to DirectShow interfaces via .NET
Copyright (C) 2005
http://sourceforge.net/projects/directshownet/

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

#endregion

using System;
using System.Runtime.InteropServices;

namespace DirectShowLib.BDA
{

    #region Declarations

#if ALLOW_UNTESTED_INTERFACES
    /// <summary>
    /// From unnamed enum DISPID_TUNER_TS_*
    /// </summary>
    internal enum DispIDTuner
    {
        REMOVEITEM = -555,
        ADDITEM = -553,
        NEWENUM = -4,
        VALUE = 0,

        // DISPIDs for ITuningSpace interface
        TS_UNIQUENAME = 1,
        TS_FRIENDLYNAME = 2,
        TS_CLSID = 3,
        TS_NETWORKTYPE = 4,
        TS__NETWORKTYPE = 5,
        TS_CREATETUNEREQUEST = 6,
        TS_ENUMCATEGORYGUIDS = 7,
        TS_ENUMDEVICEMONIKERS = 8,
        TS_DEFAULTPREFERREDCOMPONENTTYPES = 9,
        TS_FREQMAP = 10,
        TS_DEFLOCATOR = 11,
        TS_CLONE = 12,

        // DISPIDs for ITuneRequest interface
        TR_TUNINGSPACE = 1,
        TR_COMPONENTS = 2,
        TR_CLONE = 3,
        TR_LOCATOR = 4,

        // DISPID for IComponentType interface
        CT_CATEGORY = 1,
        CT_MEDIAMAJORTYPE = 2,
        CT__MEDIAMAJORTYPE = 3,
        CT_MEDIASUBTYPE = 4,
        CT__MEDIASUBTYPE = 5,
        CT_MEDIAFORMATTYPE = 6,
        CT__MEDIAFORMATTYPE = 7,
        CT_MEDIATYPE = 8,
        CT_CLONE = 9,

        // DISPID for ILanguageComponentType interface
        LCT_LANGID = 100,

        // DISPID for IMPEG2ComponentType interface
        MP2CT_TYPE = 200,

        // DISPID for IATSCComponentType interface
        ATSCCT_FLAGS = 300,

        // DISPID for ILocator interface
        L_CARRFREQ = 1,
        L_INNERFECMETHOD = 2,
        L_INNERFECRATE = 3,
        L_OUTERFECMETHOD = 4,
        L_OUTERFECRATE = 5,
        L_MOD = 6,
        L_SYMRATE = 7,
        L_CLONE = 8,

        // DISPID for IATSCLocator interface
        L_ATSC_PHYS_CHANNEL = 201,
        L_ATSC_TSID = 202,

        // DISPID for IDVBTLocator interface
        L_DVBT_BANDWIDTH = 301,
        L_DVBT_LPINNERFECMETHOD = 302,
        L_DVBT_LPINNERFECRATE = 303,
        L_DVBT_GUARDINTERVAL = 304,
        L_DVBT_HALPHA = 305,
        L_DVBT_TRANSMISSIONMODE = 306,
        L_DVBT_INUSE = 307,

        // DISPID for IDVBSLocator interface
        L_DVBS_POLARISATION = 401,
        L_DVBS_WEST = 402,
        L_DVBS_ORBITAL = 403,
        L_DVBS_AZIMUTH = 404,
        L_DVBS_ELEVATION = 405,

        // DISPID for IDVBCLocator interface

        // DISPIDs for IComponent interface
        C_TYPE = 1,
        C_STATUS = 2,
        C_LANGID = 3,
        C_DESCRIPTION = 4,
        C_CLONE = 5,

        // DISPIDs for IMPEG2Component interface
        C_MP2_PID = 101,
        C_MP2_PCRPID = 102,
        C_MP2_PROGNO = 103,

        // DISPIDs for IDVBTuningSpace interface
        TS_DVB_SYSTEMTYPE = 101,

        // DISPIDs for IDVBTuningSpace2 interface
        TS_DVB2_NETWORK_ID = 102,

        // DISPIDs for IDVBSTuningSpace interface
        TS_DVBS_LOW_OSC_FREQ = 1001,
        TS_DVBS_HI_OSC_FREQ = 1002,
        TS_DVBS_LNB_SWITCH_FREQ = 1003,
        TS_DVBS_INPUT_RANGE = 1004,
        TS_DVBS_SPECTRAL_INVERSION = 1005,

        // DISPIDs for IAnalogRadioTuningSpace interface
        TS_AR_MINFREQUENCY = 101,
        TS_AR_MAXFREQUENCY = 102,
        TS_AR_STEP = 103,

        // DISPIDs for IAnalogTVTuningSpace interface
        TS_ATV_MINCHANNEL = 101,
        TS_ATV_MAXCHANNEL = 102,
        TS_ATV_INPUTTYPE = 103,
        TS_ATV_COUNTRYCODE = 104,

        // DISPIDs for IATSCTuningSpace interface
        TS_ATSC_MINMINORCHANNEL = 201,
        TS_ATSC_MAXMINORCHANNEL = 202,
        TS_ATSC_MINPHYSCHANNEL = 203,
        TS_ATSC_MAXPHYSCHANNEL = 204,

        // DISPID for IAnalogTVAudioComponent interface
        ATVAC_CHANNEL = 101,

        // DISPIDs for IAnalogTVDataComponent interface
        ATVDC_SYSTEM = 101,
        ATVDC_CONTENT = 102,

        // DISPID for IChannelTuneRequest interface
        CTR_CHANNEL = 101,

        // DISPID IATSCChannelTuneRequest
        ACTR_MINOR_CHANNEL = 201,

        // DISPIDs for IDVBComponent interface
        DVBTUNER_DVBC_ATTRIBUTESVALID = 101,
        DVBTUNER_DVBC_PID = 102,
        DVBTUNER_DVBC_TAG = 103,
        DVBTUNER_DVBC_COMPONENTTYPE = 104,

        // DISPIDs for IDVBTuneRequest interface
        DVBTUNER_ONID = 101,
        DVBTUNER_TSID = 102,
        DVBTUNER_SID = 103,

        // DISPIDs for IMPEG2TuneRequest interface
        MP2TUNER_TSID = 101,
        MP2TUNER_PROGNO = 102,

        // DISPIDs for IMPEG2TuneRequestFactory interface
        MP2TUNERFACTORY_CREATETUNEREQUEST = 1,
    }


    [ComImport, Guid("D02AAC50-027E-11d3-9D8E-00C04F72D980")]
    public class SystemTuningSpaces
    {
    }

    [ComImport, Guid("5FFDC5E6-B83A-4b55-B6E8-C69E765FE9DB")]
    public class TuningSpace
    {
    }

    [ComImport, Guid("A2E30750-6C3D-11d3-B653-00C04F79498E")]
    public class ATSCTuningSpace
    {
    }

    [ComImport, Guid("8A674B4C-1F63-11d3-B64C-00C04F79498E")]
    public class AnalogRadioTuningSpace
    {
    }

    [ComImport, Guid("F9769A06-7ACA-4e39-9CFB-97BB35F0E77E")]
    public class AuxInTuningSpace
    {
    }

    [ComImport, Guid("8A674B4D-1F63-11d3-B64C-00C04F79498E")]
    public class AnalogTVTuningSpace
    {
    }

    [ComImport, Guid("C6B14B32-76AA-4a86-A7AC-5C79AAF58DA7")]
    public class DVBTuningSpace
    {
    }

    [ComImport, Guid("B64016F3-C9A2-4066-96F0-BD9563314726")]
    public class DVBSTuningSpace
    {
    }

    [ComImport, Guid("A1A2B1C4-0E3A-11d3-9D8E-00C04F72D980")]
    public class ComponentTypes
    {
    }

    [ComImport, Guid("823535A0-0318-11d3-9D8E-00C04F72D980")]
    public class ComponentType
    {
    }

    [ComImport, Guid("1BE49F30-0E1B-11d3-9D8E-00C04F72D980")]
    public class LanguageComponentType
    {
    }

    [ComImport, Guid("418008F3-CF67-4668-9628-10DC52BE1D08")]
    public class MPEG2ComponentType
    {
    }

    [ComImport, Guid("A8DCF3D5-0780-4ef4-8A83-2CFFAACB8ACE")]
    public class ATSCComponentType
    {
    }

    [ComImport, Guid("809B6661-94C4-49e6-B6EC-3F0F862215AA")]
    public class Components
    {
    }

    [ComImport, Guid("59DC47A8-116C-11d3-9D8E-00C04F72D980")]
    public class Component
    {
    }

    [ComImport, Guid("055CB2D7-2969-45cd-914B-76890722F112")]
    public class MPEG2Component
    {
    }

    [ComImport, Guid("B46E0D38-AB35-4a06-A137-70576B01B39F")]
    public class TuneRequest
    {
    }

    [ComImport, Guid("0369B4E5-45B6-11d3-B650-00C04F79498E")]
    public class ChannelTuneRequest
    {
    }

    [ComImport, Guid("0369B4E6-45B6-11d3-B650-00C04F79498E")]
    public class ATSCChannelTuneRequest
    {
    }

    [ComImport, Guid("0955AC62-BF2E-4cba-A2B9-A63F772D46CF")]
    public class MPEG2TuneRequest
    {
    }

    [ComImport, Guid("2C63E4EB-4CEA-41b8-919C-E947EA19A77C")]
    public class MPEG2TuneRequestFactory
    {
    }

    [ComImport, Guid("0888C883-AC4F-4943-B516-2C38D9B34562")]
    public class Locator
    {
    }

    [ComImport, Guid("8872FF1B-98FA-4d7a-8D93-C9F1055F85BB")]
    public class ATSCLocator
    {
    }

    [ComImport, Guid("9CD64701-BDF3-4d14-8E03-F12983D86664")]
    public class DVBTLocator
    {
    }

    [ComImport, Guid("1DF7D126-4050-47f0-A7CF-4C4CA9241333")]
    public class DVBSLocator
    {
    }

    [ComImport, Guid("C531D9FD-9685-4028-8B68-6E1232079F1E")]
    public class DVBCLocator
    {
    }

    [ComImport, Guid("15D6504A-5494-499c-886C-973C9E53B9F1")]
    public class DVBTuneRequest
    {
    }

    [ComImport, Guid("8A674B49-1F63-11d3-B64C-00C04F79498E")]
    public class CreatePropBagOnRegKey
    {
    }

#endif

    #endregion

    #region Interfaces

#if ALLOW_UNTESTED_INTERFACES

    [Guid("359B3901-572C-4854-BB49-CDEF66606A25"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IRegisterTuner
    {
        int Register( 
            ITuner pTuner,
            IGraphBuilder pGraph
            );
        
        int Unregister();
    }
    

    [Guid("39DD45DA-2DA8-46BA-8A8A-87E2B73D983A"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAnalogRadioTuningSpace2 : IAnalogRadioTuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        #region IAnalogRadioTuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MINFREQUENCY)]
        new int get_MinFrequency([Out] out int MinFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MINFREQUENCY)]
        new int put_MinFrequency([In] int NewMinFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MAXFREQUENCY)]
        new int get_MaxFrequency([Out] out int MaxFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MAXFREQUENCY)]
        new int put_MaxFrequency([In] int NewMaxFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_STEP)]
        new int get_Step([Out] out int StepVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_STEP)]
        new int put_Step([In] int StepVal);

        #endregion

        int get_CountryCode( 
            out int CountryCodeVal
            );
        
        int put_CountryCode( 
            int NewCountryCodeVal
            );
    }


    [Guid("B10931ED-8BFE-4AB0-9DCE-E469C29A9729"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAuxInTuningSpace2 : IAuxInTuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        int get_CountryCode( 
            out int CountryCodeVal);
        
        int put_CountryCode( 
            int NewCountryCodeVal
            );
    }


    [Guid("B34505E0-2F0E-497b-80BC-D43F3B24ED7F"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBDAComparable
    {
        int CompareExact( 
            object CompareTo,
            out int Result
            );
        
        int CompareEquivalent( 
            object CompareTo,
            int dwFlags,
            out int Result
            );
        
        int HashExact( 
            out long Result
            );
        
        int HashExactIncremental( 
            long PartialResult,
            out long Result
            );
        
        int HashEquivalent( 
            int dwFlags,
            out long Result
            );
        
        int HashEquivalentIncremental( 
            long PartialResult,
            int dwFlags,
            out long Result
            );        
    }
    

    [Guid("901284E4-33FE-4b69-8D63-634A596F3756"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITuningSpaces
    {
        [PreserveSig]
        int get_Count([Out] out int Count);

        [PreserveSig, DispId((int) DispIDTuner.NEWENUM)]
        int get__NewEnum([Out] out UCOMIEnumVARIANT ppNewEnum);

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int get_Item(
            [In] object varIndex,
            [Out] out ITuningSpace TuningSpace
            );

        [PreserveSig]
        int get_EnumTuningSpaces([Out] out IEnumTuningSpaces NewEnum);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Tuning Space Container
    //////////////////////////////////////////////////////////////////////////////////////
    [CLSCompliant(false), // because of _TuningSpacesForCLSID
    Guid("5B692E84-E2F1-11d2-9493-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITuningSpaceContainer
    {
        [PreserveSig]
        int get_Count([Out] out int Count);

        [PreserveSig, DispId((int) DispIDTuner.NEWENUM)]
        int get__NewEnum([Out] out UCOMIEnumVARIANT ppNewEnum);

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int get_Item(
            [In] object varIndex,
            [Out] out ITuningSpace TuningSpace
            );

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int put_Item(
            [In] object varIndex,
            [In] ITuningSpace TuningSpace
            );

        [PreserveSig]
        int TuningSpacesForCLSID(
            [In, MarshalAs(UnmanagedType.BStr)] string SpaceCLSID,
            [Out] out ITuningSpaces NewColl
            );

        [PreserveSig] 
        int _TuningSpacesForCLSID(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid SpaceCLSID,
            [Out] out ITuningSpaces NewColl
            );

        [PreserveSig]
        int TuningSpacesForName(
            [In, MarshalAs(UnmanagedType.BStr)] string Name,
            [Out] out ITuningSpaces NewColl
            );

        [PreserveSig]
        int FindID(
            [In] ITuningSpace TuningSpace,
            [Out] out int ID
            );

        [PreserveSig, DispId((int) DispIDTuner.ADDITEM)]
        int Add(
            [In] ITuningSpace TuningSpace,
            [Out] out object NewIndex
            );

        [PreserveSig]
        int get_EnumTuningSpaces([Out] out IEnumTuningSpaces ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.REMOVEITEM)]
        int Remove([In] object Index);

        [PreserveSig]
        int get_MaxCount([Out] out int MaxCount);

        [PreserveSig]
        int put_MaxCount([In] int MaxCount);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Tuning Space Interfaces
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("061C6E30-E622-11d2-9493-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITuningSpace
    {
        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        int Clone([Out] out ITuningSpace NewTS);
    }


    [Guid("8B8EB248-FC2B-11d2-9D8C-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumTuningSpaces
    {
        int Next(
            [In] int celt,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] ITuningSpace[] rgelt,
            [Out] out int pceltFetched
            );

        int Skip([In] int celt);

        int Reset();

        int Clone([Out] out IEnumTuningSpaces ppEnum);
    }


    [Guid("ADA0B268-3B19-4e5b-ACC4-49F852BE13BA"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBTuningSpace : ITuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB_SYSTEMTYPE)]
        int get_SystemType([Out] out DVBSystemType SysType);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB_SYSTEMTYPE)]
        int put_SystemType([In] DVBSystemType SysType);
    }


    [Guid("843188B4-CE62-43db-966B-8145A094E040"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBTuningSpace2 : IDVBTuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        #region IDVBTuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB_SYSTEMTYPE)]
        new int get_SystemType([Out] out DVBSystemType SysType);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB_SYSTEMTYPE)]
        new int put_SystemType([In] DVBSystemType SysType);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB2_NETWORK_ID)]
        int get_NetworkID([Out] out int NetworkID);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB2_NETWORK_ID)]
        int put_NetworkID([In] int NetworkID);
    }


    [Guid("CDF7BE60-D954-42fd-A972-78971958E470"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBSTuningSpace : IDVBTuningSpace2
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        #region IDVBTuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB_SYSTEMTYPE)]
        new int get_SystemType([Out] out DVBSystemType SysType);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB_SYSTEMTYPE)]
        new int put_SystemType([In] DVBSystemType SysType);

        #endregion

        #region IDVBTuningSpace2 Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB2_NETWORK_ID)]
        new int get_NetworkID([Out] out int NetworkID);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVB2_NETWORK_ID)]
        new int put_NetworkID([In] int NetworkID);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_LOW_OSC_FREQ)]
        int get_LowOscillator([Out] out int LowOscillator);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_LOW_OSC_FREQ)]
        int put_LowOscillator([In] int LowOscillator);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_HI_OSC_FREQ)]
        int get_HighOscillator([Out] out int HighOscillator);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_HI_OSC_FREQ)]
        int put_HighOscillator([In] int HighOscillator);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_LNB_SWITCH_FREQ)]
        int get_LNBSwitch([Out] out int LNBSwitch);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_LNB_SWITCH_FREQ)]
        int put_LNBSwitch([In] int LNBSwitch);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_INPUT_RANGE)]
        int get_InputRange([Out, MarshalAs(UnmanagedType.BStr)] out string InputRange);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_INPUT_RANGE)]
        int put_InputRange([Out, MarshalAs(UnmanagedType.BStr)] string InputRange);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_INPUT_RANGE)]
        int put_LNBSwitch([In, MarshalAs(UnmanagedType.BStr)] string InputRange);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_SPECTRAL_INVERSION)]
        int get_SpectralInversion([Out] out SpectralInversion SpectralInversionVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DVBS_SPECTRAL_INVERSION)]
        int put_SpectralInversion([In] SpectralInversion SpectralInversionVal);
    }


    [Guid("E48244B8-7E17-4f76-A763-5090FF1E2F30"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAuxInTuningSpace : ITuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion
    }


    [Guid("2A6E293C-2595-11d3-B64C-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAnalogTVTuningSpace : ITuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MINCHANNEL)]
        int get_MinChannel([Out] out int MinChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MINCHANNEL)]
        int put_MinChannel([In] int NewMinChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MAXCHANNEL)]
        int get_MaxChannel([Out] out int MaxChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MAXCHANNEL)]
        int put_MaxChannel([In] int NewMaxChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        int get_InputType([Out] out TunerInputType InputTypeVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        int put_InputType([In] TunerInputType NewInputTypeVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        int get_CountryCode([Out] out int CountryCodeVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        int put_CountryCode([In] int NewCountryCodeVal);
    }


    [Guid("0369B4E2-45B6-11d3-B650-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IATSCTuningSpace : IAnalogTVTuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        #region IAnalogTVTuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MINCHANNEL)]
        new int get_MinChannel([Out] out int MinChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MINCHANNEL)]
        new int put_MinChannel([In] int NewMinChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MAXCHANNEL)]
        new int get_MaxChannel([Out] out int MaxChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_MAXCHANNEL)]
        new int put_MaxChannel([In] int NewMaxChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        new int get_InputType([Out] out TunerInputType InputTypeVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        new int put_InputType([In] TunerInputType NewInputTypeVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        new int get_CountryCode([Out] out int CountryCodeVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATV_INPUTTYPE)]
        new int put_CountryCode([In] int NewCountryCodeVal);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MINMINORCHANNEL)]
        int get_MinMinorChannel([Out] out int MinMinorChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MINMINORCHANNEL)]
        int put_MinMinorChannel([In] int NewMinMinorChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MAXMINORCHANNEL)]
        int get_MaxMinorChannel([Out] out int MaxMinorChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MAXMINORCHANNEL)]
        int put_MaxMinorChannel([In] int NewMaxMinorChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MINPHYSCHANNEL)]
        int get_MinPhysicalChannel([Out] out int MinPhysicalChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MINPHYSCHANNEL)]
        int put_MinPhysicalChannel([In] int NewMinPhysicalChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MAXPHYSCHANNEL)]
        int get_MaxPhysicalChannel([Out] out int MaxPhysicalChannelVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_ATSC_MAXPHYSCHANNEL)]
        int put_MaxPhysicalChannel([In] int NewMaxPhysicalChannelVal);
    }


    [Guid("2A6E293B-2595-11d3-B64C-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAnalogRadioTuningSpace : ITuningSpace
    {
        #region ITuningSpace Methods

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int get_UniqueName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_UNIQUENAME)]
        new int put_UniqueName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int get_FriendlyName([Out, MarshalAs(UnmanagedType.BStr)] out string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_FRIENDLYNAME)]
        new int put_FriendlyName([In, MarshalAs(UnmanagedType.BStr)] string Name);

        [PreserveSig, DispId((int) DispIDTuner.TS_CLSID)]
        new int get_CLSID([Out, MarshalAs(UnmanagedType.BStr)] out string SpaceCLSID);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int get_NetworkType([Out, MarshalAs(UnmanagedType.BStr)] out string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_NETWORKTYPE)]
        new int put_NetworkType([In, MarshalAs(UnmanagedType.BStr)] string NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int get__NetworkType([Out] out Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS__NETWORKTYPE)]
        new int put__NetworkType([In, MarshalAs(UnmanagedType.LPStruct)] Guid NetworkTypeGuid);

        [PreserveSig, DispId((int) DispIDTuner.TS_CREATETUNEREQUEST)]
        new int CreateTuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMCATEGORYGUIDS)]
        new int EnumCategoryGUIDs([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppEnum); // IEnumGUID** 

        [PreserveSig, DispId((int) DispIDTuner.TS_ENUMDEVICEMONIKERS)]
        new int EnumDeviceMonikers([Out] out UCOMIEnumMoniker ppEnum);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int get_DefaultPreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFAULTPREFERREDCOMPONENTTYPES)]
        new int put_DefaultPreferredComponentTypes([In] IComponentTypes NewComponentTypes);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int get_FrequencyMapping([Out, MarshalAs(UnmanagedType.BStr)] out string pMapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_FREQMAP)]
        new int put_FrequencyMapping([In, MarshalAs(UnmanagedType.BStr)] string Mapping);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int get_DefaultLocator([Out] out ILocator LocatorVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_DEFLOCATOR)]
        new int put_DefaultLocator([In] ILocator LocatorVal);

        [PreserveSig]
        new int Clone([Out] out ITuningSpace NewTS);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MINFREQUENCY)]
        int get_MinFrequency([Out] out int MinFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MINFREQUENCY)]
        int put_MinFrequency([In] int NewMinFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MAXFREQUENCY)]
        int get_MaxFrequency([Out] out int MaxFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_MAXFREQUENCY)]
        int put_MaxFrequency([In] int NewMaxFrequencyVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_STEP)]
        int get_Step([Out] out int StepVal);

        [PreserveSig, DispId((int) DispIDTuner.TS_AR_STEP)]
        int put_Step([In] int StepVal);
    }


    [Guid("07DDC146-FC3D-11d2-9D8C-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITuneRequest
    {
        [PreserveSig, DispId((int) DispIDTuner.TR_TUNINGSPACE)]
        int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig, DispId((int) DispIDTuner.TR_COMPONENTS)]
        int get_Components([Out] out IComponents Components);

        [PreserveSig, DispId((int) DispIDTuner.TR_CLONE)]
        int Clone([Out] out ITuneRequest NewTuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        int get_Locator([Out] out ILocator Locator);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        int put_Locator([In] ILocator Locator);
    }


    [Guid("0369B4E0-45B6-11d3-B650-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IChannelTuneRequest : ITuneRequest
    {
        #region ITuneRequest Methods

        [PreserveSig, DispId((int) DispIDTuner.TR_TUNINGSPACE)]
        new int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig, DispId((int) DispIDTuner.TR_COMPONENTS)]
        new int get_Components([Out] out IComponents Components);

        [PreserveSig, DispId((int) DispIDTuner.TR_CLONE)]
        new int Clone([Out] out ITuneRequest NewTuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int get_Locator([Out] out ILocator Locator);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int put_Locator([In] ILocator Locator);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.CTR_CHANNEL)]
        int get_Channel([Out] out int Channel);

        [PreserveSig, DispId((int) DispIDTuner.CTR_CHANNEL)]
        int put_Channel([In] int Channel);
    }


    [Guid("0369B4E1-45B6-11d3-B650-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IATSCChannelTuneRequest : IChannelTuneRequest
    {
        #region ITuneRequest Methods

        [PreserveSig, DispId((int) DispIDTuner.TR_TUNINGSPACE)]
        new int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig, DispId((int) DispIDTuner.TR_COMPONENTS)]
        new int get_Components([Out] out IComponents Components);

        [PreserveSig, DispId((int) DispIDTuner.TR_CLONE)]
        new int Clone([Out] out ITuneRequest NewTuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int get_Locator([Out] out ILocator Locator);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int put_Locator([In] ILocator Locator);

        #endregion

        #region IChannelTuneRequest Methods

        [PreserveSig, DispId((int) DispIDTuner.CTR_CHANNEL)]
        new int get_Channel([Out] out int Channel);

        [PreserveSig, DispId((int) DispIDTuner.CTR_CHANNEL)]
        new int put_Channel([In] int Channel);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.ACTR_MINOR_CHANNEL)]
        int get_MinorChannel([Out] out int MinorChannel);

        [PreserveSig, DispId((int) DispIDTuner.ACTR_MINOR_CHANNEL)]
        int put_MinorChannel([In] int MinorChannel);
    }


    [Guid("0D6F567E-A636-42bb-83BA-CE4C1704AFA2"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBTuneRequest : ITuneRequest
    {
        #region ITuneRequest Methods

        [PreserveSig, DispId((int) DispIDTuner.TR_TUNINGSPACE)]
        new int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig, DispId((int) DispIDTuner.TR_COMPONENTS)]
        new int get_Components([Out] out IComponents Components);

        [PreserveSig, DispId((int) DispIDTuner.TR_CLONE)]
        new int Clone([Out] out ITuneRequest NewTuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int get_Locator([Out] out ILocator Locator);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int put_Locator([In] ILocator Locator);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.DVBTUNER_ONID)]
        int get_ONID([Out] out int ONID);

        [PreserveSig, DispId((int) DispIDTuner.DVBTUNER_ONID)]
        int put_ONID([In] int ONID);

        [PreserveSig, DispId((int) DispIDTuner.DVBTUNER_TSID)]
        int get_TSID([Out] out int TSID);

        [PreserveSig, DispId((int) DispIDTuner.DVBTUNER_TSID)]
        int put_TSID([In] int TSID);

        [PreserveSig, DispId((int) DispIDTuner.DVBTUNER_SID)]
        int get_SID([Out] out int SID);

        [PreserveSig, DispId((int) DispIDTuner.DVBTUNER_SID)]
        int put_SID([In] int SID);
    }


    [Guid("EB7D987F-8A01-42ad-B8AE-574DEEE44D1A"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMPEG2TuneRequest : ITuneRequest
    {
        #region ITuneRequest Methods

        [PreserveSig, DispId((int) DispIDTuner.TR_TUNINGSPACE)]
        new int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig, DispId((int) DispIDTuner.TR_COMPONENTS)]
        new int get_Components([Out] out IComponents Components);

        [PreserveSig, DispId((int) DispIDTuner.TR_CLONE)]
        new int Clone([Out] out ITuneRequest NewTuneRequest);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int get_Locator([Out] out ILocator Locator);

        [PreserveSig, DispId((int) DispIDTuner.TR_LOCATOR)]
        new int put_Locator([In] ILocator Locator);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.MP2TUNER_TSID)]
        int get_TSID([Out] out int TSID);

        [PreserveSig, DispId((int) DispIDTuner.MP2TUNER_TSID)]
        int put_TSID([In] int TSID);

        [PreserveSig, DispId((int) DispIDTuner.MP2TUNER_PROGNO)]
        int get_ProgNo([Out] out int ProgNo);

        [PreserveSig, DispId((int) DispIDTuner.MP2TUNER_PROGNO)]
        int put_ProgNo([In] int ProgNo);
    }


    [Guid("14E11ABD-EE37-4893-9EA1-6964DE933E39"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMPEG2TuneRequestFactory
    {
        [PreserveSig, DispId((int) DispIDTuner.MP2TUNERFACTORY_CREATETUNEREQUEST)]
        int CreateTuneRequest(
            [In] ITuningSpace TuningSpace,
            [Out] out IMPEG2TuneRequest TuneRequest
            );
    }


    [Guid("1B9D5FC3-5BBC-4b6c-BB18-B9D10E3EEEBF"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IMPEG2TuneRequestSupport
    {
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Tuner Interfaces
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("28C52640-018A-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITuner
    {
        [PreserveSig]
        int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig]
        int put_TuningSpace([In] ITuningSpace TuningSpace);

        [PreserveSig]
        int EnumTuningSpaces([Out] out IEnumTuningSpaces ppEnum);

        [PreserveSig]
        int get_TuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig]
        int put_TuneRequest([In] ITuneRequest TuneRequest);

        [PreserveSig]
        int Validate([In] ITuneRequest TuneRequest);

        [PreserveSig]
        int get_PreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig]
        int put_PreferredComponentTypes([In] IComponentTypes ComponentTypes);

        [PreserveSig]
        int get_SignalStrength([Out] out int Strength);

        [PreserveSig]
        int TriggerSignalEvents([In] int Interval);
    }


    [Guid("1DFD0A5C-0284-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IScanningTuner : ITuner
    {
        #region ITuner Methods

        [PreserveSig]
        new int get_TuningSpace([Out] out ITuningSpace TuningSpace);

        [PreserveSig]
        new int put_TuningSpace([In] ITuningSpace TuningSpace);

        [PreserveSig]
        new int EnumTuningSpaces([Out] out IEnumTuningSpaces ppEnum);

        [PreserveSig]
        new int get_TuneRequest([Out] out ITuneRequest TuneRequest);

        [PreserveSig]
        new int put_TuneRequest([In] ITuneRequest TuneRequest);

        [PreserveSig]
        new int Validate([In] ITuneRequest TuneRequest);

        [PreserveSig]
        new int get_PreferredComponentTypes([Out] out IComponentTypes ComponentTypes);

        [PreserveSig]
        new int put_PreferredComponentTypes([In] IComponentTypes ComponentTypes);

        [PreserveSig]
        new int get_SignalStrength([Out] out int Strength);

        [PreserveSig]
        new int TriggerSignalEvents([In] int Interval);

        #endregion

        [PreserveSig]
        int SeekUp();

        [PreserveSig]
        int SeekDown();

        [PreserveSig]
        int ScanUp([In] int MillisecondsPause);

        [PreserveSig]
        int ScanDown([Out] out int MillisecondsPause);

        [PreserveSig]
        int AutoProgram();
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Component Type Interfaces
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("6A340DC0-0311-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComponentType
    {
        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        int get_Category([Out] out ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        int put_Category([In] ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        int get_MediaMajorType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        int put_MediaMajorType([In, MarshalAs(UnmanagedType.BStr)] string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        int get__MediaMajorType([Out] out Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        int put__MediaMajorType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        int get_MediaSubType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        int put_MediaSubType([In, MarshalAs(UnmanagedType.BStr)] string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        int get__MediaSubType([Out] out Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        int put__MediaSubType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        int get_MediaFormatType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        int put_MediaFormatType([In, MarshalAs(UnmanagedType.BStr)] string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        int get__MediaFormatType([Out] out Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        int put__MediaFormatType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        int get_MediaType([Out] out AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        int put_MediaType([In] AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_CLONE)]
        int Clone([Out] out IComponentType NewCT);
    }


    [Guid("B874C8BA-0FA2-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ILanguageComponentType : IComponentType
    {
        #region IComponentType Methods

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        new int get_Category([Out] out ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        new int put_Category([In] ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        new int get_MediaMajorType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        new int put_MediaMajorType([In, MarshalAs(UnmanagedType.BStr)] string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        new int get__MediaMajorType([Out] out Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        new int put__MediaMajorType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        new int get_MediaSubType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        new int put_MediaSubType([In, MarshalAs(UnmanagedType.BStr)] string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        new int get__MediaSubType([Out] out Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        new int put__MediaSubType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        new int get_MediaFormatType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        new int put_MediaFormatType([In, MarshalAs(UnmanagedType.BStr)] string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        new int get__MediaFormatType([Out] out Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        new int put__MediaFormatType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        new int get_MediaType([Out] out AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        new int put_MediaType([In] AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_CLONE)]
        new int Clone([Out] out IComponentType NewCT);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.LCT_LANGID)]
        int get_LangID([Out] out int LangID);

        [PreserveSig, DispId((int) DispIDTuner.LCT_LANGID)]
        int put_LangID([In] int LangID);
    }


    [Guid("2C073D84-B51C-48c9-AA9F-68971E1F6E38"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMPEG2ComponentType : ILanguageComponentType
    {
        #region IComponentType Methods

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        new int get_Category([Out] out ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        new int put_Category([In] ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        new int get_MediaMajorType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        new int put_MediaMajorType([In, MarshalAs(UnmanagedType.BStr)] string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        new int get__MediaMajorType([Out] out Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        new int put__MediaMajorType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        new int get_MediaSubType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        new int put_MediaSubType([In, MarshalAs(UnmanagedType.BStr)] string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        new int get__MediaSubType([Out] out Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        new int put__MediaSubType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        new int get_MediaFormatType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        new int put_MediaFormatType([In, MarshalAs(UnmanagedType.BStr)] string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        new int get__MediaFormatType([Out] out Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        new int put__MediaFormatType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        new int get_MediaType([Out] out AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        new int put_MediaType([In] AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_CLONE)]
        new int Clone([Out] out IComponentType NewCT);

        #endregion

        #region ILanguageComponentType Methods

        [PreserveSig, DispId((int) DispIDTuner.LCT_LANGID)]
        new int get_LangID([Out] out int LangID);

        [PreserveSig, DispId((int) DispIDTuner.LCT_LANGID)]
        new int put_LangID([In] int LangID);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.MP2CT_TYPE)]
        int get_StreamType([Out] out MPEG2StreamType MP2StreamType);

        [PreserveSig, DispId((int) DispIDTuner.MP2CT_TYPE)]
        int put_StreamType([In] MPEG2StreamType MP2StreamType);
    }


    [Guid("FC189E4D-7BD4-4125-B3B3-3A76A332CC96"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IATSCComponentType : IMPEG2ComponentType
    {
        #region IComponentType Methods

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        new int get_Category([Out] out ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_CATEGORY)]
        new int put_Category([In] ComponentCategory Category);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        new int get_MediaMajorType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAMAJORTYPE)]
        new int put_MediaMajorType([In, MarshalAs(UnmanagedType.BStr)] string MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        new int get__MediaMajorType([Out] out Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAMAJORTYPE)]
        new int put__MediaMajorType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaMajorType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        new int get_MediaSubType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIASUBTYPE)]
        new int put_MediaSubType([In, MarshalAs(UnmanagedType.BStr)] string MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        new int get__MediaSubType([Out] out Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIASUBTYPE)]
        new int put__MediaSubType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaSubType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        new int get_MediaFormatType([Out, MarshalAs(UnmanagedType.BStr)] out string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIAFORMATTYPE)]
        new int put_MediaFormatType([In, MarshalAs(UnmanagedType.BStr)] string MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        new int get__MediaFormatType([Out] out Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT__MEDIAFORMATTYPE)]
        new int put__MediaFormatType([In, MarshalAs(UnmanagedType.LPStruct)] Guid MediaFormatType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        new int get_MediaType([Out] out AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_MEDIATYPE)]
        new int put_MediaType([In] AMMediaType MediaType);

        [PreserveSig, DispId((int) DispIDTuner.CT_CLONE)]
        new int Clone([Out] out IComponentType NewCT);

        #endregion

        #region ILanguageComponentType Methods

        [PreserveSig, DispId((int) DispIDTuner.LCT_LANGID)]
        new int get_LangID([Out] out int LangID);

        [PreserveSig, DispId((int) DispIDTuner.LCT_LANGID)]
        new int put_LangID([In] int LangID);

        #endregion

        #region IMPEG2ComponentType Methods

        [PreserveSig, DispId((int) DispIDTuner.MP2CT_TYPE)]
        new int get_StreamType([Out] out MPEG2StreamType MP2StreamType);

        [PreserveSig, DispId((int) DispIDTuner.MP2CT_TYPE)]
        new int put_StreamType([In] MPEG2StreamType MP2StreamType);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.ATSCCT_FLAGS)]
        int get_Flags([Out] out int Flags);

        [PreserveSig, DispId((int) DispIDTuner.ATSCCT_FLAGS)]
        int put_Flags([In] int Flags);
    }


    [Guid("8A674B4A-1F63-11d3-B64C-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumComponentTypes
    {
        int Next(
            [In] int celt,
            [Out] out IComponentType rgelt,
            [Out] out int pceltFetched
            );

        int Skip([In] int celt);

        int Reset();

        int Clone([Out] out IEnumComponentTypes ppEnum);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Component Type Container
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("0DC13D4A-0313-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComponentTypes
    {
        [PreserveSig]
        int get_Count([Out] out int Count);

        [PreserveSig, DispId((int) DispIDTuner.NEWENUM)]
        int get__NewEnum([Out] out UCOMIEnumVARIANT ppNewEnum);

        [PreserveSig]
        int EnumComponentTypes([Out] out IEnumComponentTypes ppNewEnum);

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int get_Item(
            [In] object varIndex,
            [Out] out IEnumComponentTypes TuningSpace
            );

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int put_Item(
            [In] IComponentType ComponentType,
            [Out] out object NewIndex
            );

        [PreserveSig, DispId((int) DispIDTuner.ADDITEM)]
        int Add(
            [In] IComponentType ComponentType,
            [Out] out object NewIndex
            );

        [PreserveSig, DispId((int) DispIDTuner.REMOVEITEM)]
        int Remove([In] object Index);

        [PreserveSig]
        int Clone([Out] out IComponentTypes NewList);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Component Interfaces
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("1A5576FC-0E19-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComponent
    {
        [PreserveSig, DispId((int) DispIDTuner.C_TYPE)]
        int get_Type([Out] out IComponentType CT);

        [PreserveSig, DispId((int) DispIDTuner.C_TYPE)]
        int put_Type([In] IComponentType CT);

        [PreserveSig, DispId((int) DispIDTuner.C_LANGID)]
        int get_DescLangID([Out] out int LangID);

        [PreserveSig, DispId((int) DispIDTuner.C_LANGID)]
        int put_DescLangID([In] int LangID);

        [PreserveSig, DispId((int) DispIDTuner.C_STATUS)]
        int get_Status([Out] out ComponentStatus Status);

        [PreserveSig, DispId((int) DispIDTuner.C_STATUS)]
        int put_Status([In] ComponentStatus Status);

        [PreserveSig, DispId((int) DispIDTuner.C_DESCRIPTION)]
        int get_Description([Out, MarshalAs(UnmanagedType.BStr)] out string Description);

        [PreserveSig, DispId((int) DispIDTuner.C_DESCRIPTION)]
        int put_Description([In, MarshalAs(UnmanagedType.BStr)] string Description);

        [PreserveSig, DispId((int) DispIDTuner.C_CLONE)]
        int Clone([Out] out IComponent NewComponent);
    }


    [Guid("1493E353-1EB6-473c-802D-8E6B8EC9D2A9"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMPEG2Component : IComponent
    {
        #region IComponent Methods

        [PreserveSig, DispId((int) DispIDTuner.C_TYPE)]
        new int get_Type([Out] out IComponentType CT);

        [PreserveSig, DispId((int) DispIDTuner.C_TYPE)]
        new int put_Type([In] IComponentType CT);

        [PreserveSig, DispId((int) DispIDTuner.C_LANGID)]
        new int get_DescLangID([Out] out int LangID);

        [PreserveSig, DispId((int) DispIDTuner.C_LANGID)]
        new int put_DescLangID([In] int LangID);

        [PreserveSig, DispId((int) DispIDTuner.C_STATUS)]
        new int get_Status([Out] out ComponentStatus Status);

        [PreserveSig, DispId((int) DispIDTuner.C_STATUS)]
        new int put_Status([In] ComponentStatus Status);

        [PreserveSig, DispId((int) DispIDTuner.C_DESCRIPTION)]
        new int get_Description([Out, MarshalAs(UnmanagedType.BStr)] out string Description);

        [PreserveSig, DispId((int) DispIDTuner.C_DESCRIPTION)]
        new int put_Description([In, MarshalAs(UnmanagedType.BStr)] string Description);

        [PreserveSig, DispId((int) DispIDTuner.C_CLONE)]
        new int Clone([Out] out IComponent NewComponent);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.C_MP2_PID)]
        int get_PID([Out] out int PID);

        [PreserveSig, DispId((int) DispIDTuner.C_MP2_PID)]
        int put_PID([In] int PID);

        [PreserveSig, DispId((int) DispIDTuner.C_MP2_PCRPID)]
        int get_PCRPID([Out] out int PCRPID);

        [PreserveSig, DispId((int) DispIDTuner.C_MP2_PCRPID)]
        int put_PCRPID([In] int PCRPID);

        [PreserveSig, DispId((int) DispIDTuner.C_MP2_PROGNO)]
        int get_ProgramNumber([Out] out int ProgramNumber);

        [PreserveSig, DispId((int) DispIDTuner.C_MP2_PROGNO)]
        int put_ProgramNumber([In] int ProgramNumber);
    }


    [Guid("2A6E2939-2595-11d3-B64C-00C04F79498E"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumComponents
    {
        int Next(
            [In] int celt,
            [Out] out IComponent rgelt,
            [Out] out int pceltFetched
            );

        int Skip([In] int celt);

        int Reset();

        int Clone([Out] out IEnumComponents ppEnum);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Component Container
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("FCD01846-0E19-11d3-9D8E-00C04F72D980"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComponentsOld
    {
        [PreserveSig]
        int get_Count([Out] out int Count);

        [PreserveSig, DispId((int) DispIDTuner.NEWENUM)]
        int get__NewEnum([Out] out UCOMIEnumVARIANT ppNewEnum);

        [PreserveSig]
        int EnumComponents([Out] out IEnumComponents ppNewEnum);

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int get_Item(
            [In] object varIndex,
            [Out] out IComponent TuningSpace
            );

        [PreserveSig, DispId((int) DispIDTuner.ADDITEM)]
        int Add(
            [In] IComponent Component,
            [Out] out object NewIndex
            );

        [PreserveSig, DispId((int) DispIDTuner.REMOVEITEM)]
        int Remove([In] object Index);

        [PreserveSig]
        int Clone([Out] out IComponents NewList);
    }


    //////////////////////////////////////////////////////////////////////////////////////
    // Component Container
    //////////////////////////////////////////////////////////////////////////////////////
    [Guid("39A48091-FFFE-4182-A161-3FF802640E26"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComponents
    {
        [PreserveSig]
        int get_Count([Out] out int Count);

        [PreserveSig, DispId((int) DispIDTuner.NEWENUM)]
        int get__NewEnum([Out] out UCOMIEnumVARIANT ppNewEnum);

        [PreserveSig]
        int EnumComponents([Out] out IEnumComponents ppNewEnum);

        [PreserveSig, DispId((int) DispIDTuner.VALUE)]
        int get_Item(
            [In] object varIndex,
            [Out] out IComponent TuningSpace
            );

        [PreserveSig, DispId((int) DispIDTuner.ADDITEM)]
        int Add(
            [In] IComponent Component,
            [Out] out object NewIndex
            );

        [PreserveSig, DispId((int) DispIDTuner.REMOVEITEM)]
        int Remove([In] object Index);

        [PreserveSig]
        int Clone([Out] out IComponents NewList);

        [PreserveSig]
        int put_Item( 
            object Index,
            IComponent ppComponent
            );        
    }


    [Guid("286D7F89-760C-4F89-80C4-66841D2507AA"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ILocator
    {
        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        int get_CarrierFrequency([Out] out int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        int put_CarrierFrequency([In] int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        int get_InnerFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        int put_InnerFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        int get_InnerFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        int put_InnerFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        int get_OuterFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        int put_OuterFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        int get_OuterFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        int put_OuterFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        int get_Modulation([Out] out ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        int put_Modulation([In] ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        int get_SymbolRate([Out] out int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        int put_SymbolRate([In] int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_CLONE)]
        int Clone([Out] out ILocator NewLocator);
    }


    [Guid("BF8D986F-8C2B-4131-94D7-4D3D9FCC21EF"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IATSCLocator : ILocator
    {
        #region ILocator Methods
        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int get_CarrierFrequency([Out] out int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int put_CarrierFrequency([In] int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int get_InnerFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int put_InnerFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int get_InnerFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int put_InnerFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int get_OuterFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int put_OuterFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int get_OuterFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int put_OuterFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int get_Modulation([Out] out ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int put_Modulation([In] ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int get_SymbolRate([Out] out int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int put_SymbolRate([In] int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_CLONE)]
        new int Clone([Out] out ILocator NewLocator);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.L_ATSC_PHYS_CHANNEL)]
        int get_PhysicalChannel([Out] out int PhysicalChannel);

        [PreserveSig, DispId((int) DispIDTuner.L_ATSC_PHYS_CHANNEL)]
        int put_PhysicalChannel([In] int PhysicalChannel);

        [PreserveSig, DispId((int) DispIDTuner.L_ATSC_TSID)]
        int get_TSID([Out] out int TSID);

        [PreserveSig, DispId((int) DispIDTuner.L_ATSC_TSID)]
        int put_TSID([In] int TSID);
    }


    [Guid("8664DA16-DDA2-42ac-926A-C18F9127C302"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBTLocator : ILocator
    {
        #region ILocator Methods

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int get_CarrierFrequency([Out] out int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int put_CarrierFrequency([In] int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int get_InnerFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int put_InnerFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int get_InnerFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int put_InnerFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int get_OuterFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int put_OuterFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int get_OuterFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int put_OuterFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int get_Modulation([Out] out ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int put_Modulation([In] ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int get_SymbolRate([Out] out int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int put_SymbolRate([In] int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_CLONE)]
        new int Clone([Out] out ILocator NewLocator);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_BANDWIDTH)]
        int get_Bandwidth([Out] out int BandwidthVal);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_BANDWIDTH)]
        int put_Bandwidth([In] int BandwidthVal);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_LPINNERFECMETHOD)]
        int get_LPInnerFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_LPINNERFECMETHOD)]
        int put_LPInnerFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_LPINNERFECRATE)]
        int get_LPInnerFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_LPINNERFECRATE)]
        int put_LPInnerFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_HALPHA)]
        int get_HAlpha([Out] out HierarchyAlpha Alpha);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_HALPHA)]
        int put_HAlpha([In] HierarchyAlpha Alpha);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_GUARDINTERVAL)]
        int get_Guard([Out] out GuardInterval GI);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_GUARDINTERVAL)]
        int put_Guard([In] GuardInterval GI);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_TRANSMISSIONMODE)]
        int get_Mode([Out] out TransmissionMode mode);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_TRANSMISSIONMODE)]
        int put_Mode([In] TransmissionMode mode);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_INUSE)]
        int get_OtherFrequencyInUse([Out, MarshalAs(UnmanagedType.VariantBool)] out bool OtherFrequencyInUseVal);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBT_INUSE)]
        int put_OtherFrequencyInUse([In, MarshalAs(UnmanagedType.VariantBool)] bool OtherFrequencyInUseVal);
    }


    [Guid("3D7C353C-0D04-45f1-A742-F97CC1188DC8"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBSLocator : ILocator
    {
        #region ILocator Methods

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int get_CarrierFrequency([Out] out int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int put_CarrierFrequency([In] int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int get_InnerFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int put_InnerFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int get_InnerFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int put_InnerFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int get_OuterFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int put_OuterFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int get_OuterFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int put_OuterFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int get_Modulation([Out] out ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int put_Modulation([In] ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int get_SymbolRate([Out] out int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int put_SymbolRate([In] int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_CLONE)]
        new int Clone([Out] out ILocator NewLocator);

        #endregion

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_POLARISATION)]
        int get_SignalPolarisation([Out] out Polarisation PolarisationVal);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_POLARISATION)]
        int put_SignalPolarisation([In] Polarisation PolarisationVal);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_WEST)]
        int get_WestPosition([Out, MarshalAs(UnmanagedType.VariantBool)] out bool WestLongitude);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_WEST)]
        int put_WestPosition([In, MarshalAs(UnmanagedType.VariantBool)] bool WestLongitude);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_ORBITAL)]
        int get_OrbitalPosition([Out] out int longitude);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_ORBITAL)]
        int put_OrbitalPosition([In] int longitude);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_AZIMUTH)]
        int get_Azimuth([Out] out int Azimuth);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_AZIMUTH)]
        int put_Azimuth([In] int Azimuth);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_ELEVATION)]
        int get_Elevation([Out] out int Elevation);

        [PreserveSig, DispId((int) DispIDTuner.L_DVBS_ELEVATION)]
        int put_Elevation([In] int Elevation);
    }

    [Guid("6E42F36E-1DD2-43c4-9F78-69D25AE39034"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDVBCLocator : ILocator
    {
        #region ILocator Methods

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int get_CarrierFrequency([Out] out int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_CARRFREQ)]
        new int put_CarrierFrequency([In] int Frequency);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int get_InnerFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECMETHOD)]
        new int put_InnerFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int get_InnerFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_INNERFECRATE)]
        new int put_InnerFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int get_OuterFEC([Out] out FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECMETHOD)]
        new int put_OuterFEC([In] FECMethod FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int get_OuterFECRate([Out] out BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_OUTERFECRATE)]
        new int put_OuterFECRate([In] BinaryConvolutionCodeRate FEC);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int get_Modulation([Out] out ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_MOD)]
        new int put_Modulation([In] ModulationType Modulation);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int get_SymbolRate([Out] out int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_SYMRATE)]
        new int put_SymbolRate([In] int Rate);

        [PreserveSig, DispId((int) DispIDTuner.L_CLONE)]
        new int Clone([Out] out ILocator NewLocator);

        #endregion
    }

    ///////////////////////////////////////////////////////////////////////////////////////
    // utility interfaces
    ///////////////////////////////////////////////////////////////////////////////////////
    [Guid("3B21263F-26E8-489d-AAC4-924F7EFD9511"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IBroadcastEvent
    {
        [PreserveSig]
        int Fire(Guid EventID);
    }

#endif

    #endregion
}
