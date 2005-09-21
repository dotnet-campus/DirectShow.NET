#region license

/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

#endregion

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.DMO;
using System.IO;
using System.Collections;

namespace MediaObjectTemplate
{
    abstract public class IMediaObjectImpl : IMediaObject, IMediaParamInfo, IMediaParams
    {
        #region Declarations

        // Used in IMediaParams to specify that an envelope change should
        // be applied to all parameters.
        private const int ALLPARAMS = -1;

        // COM return codes
        protected const int S_OK = 0;
        protected const int S_FALSE = 1;
        protected const int E_NOTIMPL    = unchecked((int)0x80004001);
        protected const int E_POINTER    = unchecked((int)0x80004003);
        protected const int E_INVALIDARG = unchecked((int)0x80070057);
        protected const int E_UNEXPECTED = unchecked((int)0x8000FFFF);

        // Info regarding a (input or output) pin
        private struct PinDef
        {
            public bool fTypeSet;
            public bool fIncomplete;
            public AMMediaType CurrentMediaType;
        }


        /// <summary>
        /// Used by ParamClass to specify which timeformats are supported
        /// </summary>
        [Flags]
        protected enum TimeFormatFlags
        {
            None = 0, // Unless you have zero parameters, you should never use this
            Reference = 1,
            Music = 2,
            Samples = 4
        }

        #endregion

        #region Internal Classes

        // This class holds all the information about the parameters
        // for this DMO.  Using this class allows IMediaObjectImpl to
        // automatically implement IMediaParamInfo & IMediaParams.  If
        // your DMO has no parameters, you do not need to create an instance
        // of this class.
        private class ParamClass
        {
            #region Member variables

            // The param returned to IMediaParamInfo::GetParamInfo
            public ParamInfo [] Parms;

            // The list of TimeFormats returned to IMediaParamInfo::GetSupportedTimeFormat
            public Guid [] TimeFormats;

            // The index into TimeFormats indicating the currently active format
            public int CurrentTimeFormat;

            // The TimeData for the current format (see IMediaParams::SetTimeFormat)
            public int TimeData;

            // The string text returned from IMediaParamInfo::GetParamText
            public string [] ParamText;

            // The envelopes holding the parameter info.  Note that even if parameter
            // info isn't sent with IMediaParams::AddEnvelope (ie thru IMediaParams::SetParam),
            // this implementation still creates and uses envelopes.  It just makes envelopes
            // that span the entire media length.
            public MPEnvelopes [] Envelopes;

            #endregion

            public ParamClass(int iParams, TimeFormatFlags iTimeFormats)
            {
                CurrentTimeFormat = 0;
                TimeData = 0;

                // Make room for the timeformats
                int iCount = CountBits((int)iTimeFormats);
                TimeFormats = new Guid[iCount];

                if ((iTimeFormats & TimeFormatFlags.Samples) > 0)
                {
                    TimeFormats[--iCount] = MediaParamTimeFormat.Samples;
                }
                if ((iTimeFormats & TimeFormatFlags.Music) > 0)
                {
                    TimeFormats[--iCount] = MediaParamTimeFormat.Music;
                }
                if ((iTimeFormats & TimeFormatFlags.Reference) > 0)
                {
                    TimeFormats[--iCount] = MediaParamTimeFormat.Reference;
                }
                Debug.Assert(iCount == 0, "Unrecognized time format specified");

                // Make room for the parameter info
                ParamText = new string[iParams];
                Parms = new ParamInfo[iParams];
                Envelopes = new MPEnvelopes[iParams];
            }

            public void DefineParam(int iParamNum, ParamInfo p, string sText)
            {
                Debug.Assert(iParamNum < Parms.Length && iParamNum >= 0, "Invalid parameter index");

                Parms[iParamNum] = p;
                Envelopes[iParamNum] = new MPEnvelopes(p.mpdNeutralValue, p.mopCaps, p.mpType, p.mpdMaxValue, p.mpdMaxValue);
                ParamText[iParamNum] = sText;
            }

        }


        /// <summary>
        /// The idea of envelopes (as I understand them) is to allow a parameter value
        /// to be applied to a range within the stream.  Generally speaking, there is a
        /// start time and stop time, a parameter start value and a parameter end value.  
        /// So, you could say "The volume should go from 0% to 100% over the first 4 
        /// seconds."  There is also a Curve parameter that controls how quickly the 
        /// parameter value should change: Linear, Square, Sine, Jump, etc.
        /// 
        /// Also note, the docs for IMediaParams::FlushEnvelope make no sense to me.
        /// Instead of trying to make sense of the crazyness they have written, I
        /// have done something simpler.  If you flush a range, that range gets set
        /// back to the NeutralVal for the parameter.
        /// 
        /// The assumption (based on comments in IMediaParams::SetTimeFormat) is that
        /// the times specified to the envelope are the same format as will be specified
        /// to ProcessInput.  If you are changing the format, call flush.
        /// </summary>
        private class MPEnvelopes
        {
            #region Declarations

            private const long MAX_TIME = 0x7FFFFFFFFFFFFFFF;
            protected const int S_OK = 0;
            protected const int E_INVALIDARG = unchecked((int)0x80070057);

            #endregion

            #region Member variables

            protected ArrayList m_Envelope;
            protected MPData m_DefaultValue;
            protected int m_ValidCaps;
            protected MPType m_DataType;
            protected MPData m_MinVal;
            protected MPData m_MaxVal;

            #endregion

            public MPEnvelopes(MPData iDefaultValue, MPCaps iValidCaps, MPType mt, MPData min, MPData max)
            {
                // Store the neutralValue, min/max value range, data type and supported curve types
                m_DefaultValue = iDefaultValue;
                m_ValidCaps = (int)iValidCaps;
                m_DataType = mt;
                m_MinVal = min;
                m_MaxVal = max;

                // Create an array to hold the segments (size = 3 was chosen arbitrarily)
                m_Envelope = new ArrayList(3);

                // Create one segment that spans all time containing the default values
                MPEnvelopeSegment e = new MPEnvelopeSegment();
                e.flags = MPFlags.BeginNeutralVal;
                e.iCurve = MPCaps.Jump;
                e.rtStart = 0;
                e.rtEnd = MAX_TIME;
                e.valStart = m_DefaultValue;
                e.valEnd = m_DefaultValue;

                m_Envelope.Add(e);
            }

            /// <summary>
            /// Add a segment to the envelope.  If this segment overlaps other segments,
            /// the other segments are deleted or shortened, and this segment is inserted
            /// </summary>
            /// <param name="mNew">The segment to add</param>
            /// <returns>HRESULT</returns>
            public int AddSegment(MPEnvelopeSegment mNew)
            {
                int hr;
                int y;
                MPEnvelopeSegment mOld;

                hr = ValidateEnvelopeSegment(mNew);
                if (hr >= 0)
                {
                    // Find the first record to modify.  There is always
                    // at least one record, and ValidateEnvelopeSegment ensures
                    // that the start time is less than the endtime of the last
                    // record (MAX_TIME)
                    y = 0;

                    do
                    {
                        mOld = (MPEnvelopeSegment)m_Envelope[y];

                        if (mNew.rtStart <= mOld.rtEnd)
                        {
                            break;
                        }
                        y++;

                    } while (true);

                    // Process the flags on mNew
                    UpdateSegment(y, ref mNew);

                    // Depending on how the new segment overlaps, adjust the
                    // other segments and add it
                    if (mNew.rtStart <= mOld.rtStart)
                    {
                        if (mNew.rtEnd >= mOld.rtEnd)
                        {
                            // Existing segment is completely replaced
                            m_Envelope.RemoveAt(y);
                            m_Envelope.Insert(y, mNew);

                            // Subsequent records may need to be deleted/adjusted
                            if (mNew.rtEnd > mOld.rtEnd)
                            {
                                DeleteRest(y+1, mNew.rtEnd);
                            }
                        }
                        else
                        {
                            // Existing segment is shortened from the left
                            mOld.rtStart = mNew.rtEnd + 1;
                            m_Envelope[y] = mOld;
                            m_Envelope.Insert(y, mNew);
                        }
                    }
                    else
                    {
                        if (mNew.rtEnd >= mOld.rtEnd)
                        {
                            // Existing segment is truncated
                            mOld.rtEnd = mNew.rtStart - 1;
                            m_Envelope[y] = mOld;
                            m_Envelope.Insert(y+1, mNew);

                            if (mNew.rtEnd > mOld.rtEnd)
                            {
                                DeleteRest(y+2, mNew.rtEnd);
                            }
                        }
                        else
                        {
                            // Split a segment
                            MPEnvelopeSegment mAppend = mOld;
                            mAppend.rtStart = mNew.rtEnd + 1;

                            mOld.rtEnd = mNew.rtStart - 1;
                            m_Envelope[y] = mOld;

                            m_Envelope.Insert(y+1, mNew);
                            m_Envelope.Insert(y+2, mAppend);
                        }
                    }
                }

                return hr;
            }

            /// <summary>
            /// Returns the Envelope the applies to a specific time.  Since there is a
            /// default segment that covers all possible times, this will always return 
            /// a value.
            /// </summary>
            /// <param name="rt">Time to check</param>
            /// <returns>The envelope at that segment</returns>
            public MPEnvelopeSegment FindEnvelopeForTime(long rt)
            {
                int x=0;
                MPEnvelopeSegment mRet;

                do
                {
                    mRet = (MPEnvelopeSegment)m_Envelope[x++];
                } while (rt > mRet.rtEnd);

                return mRet;
            }

            /// <summary>
            /// Calculate the parameter value at a specified time
            /// 
            /// While there are routines written for all the curve types, I'm not enough
            /// of a math whiz to feel comfortable that I got it right.  I stole the code
            /// from elsewhere and converted it to c#, so there's a chance I messed up.
            /// </summary>
            /// <param name="rt">Time at which to calculate</param>
            /// <returns>MPData value for that time based in the specified Curve</returns>
            public MPData CalcValueForTime(long rt)
            {
                long ir, ic;
                float p;
                MPData ret;
                MPEnvelopeSegment m = FindEnvelopeForTime(rt);
            
                switch(m.iCurve)
                {
                    case MPCaps.Jump:

                        // Not quite sure how I want to do this.  Consider an envelope
                        // that goes from time 0 to 10, and value 55 to 99.  Obviously
                        // at time 0, you return 55.  At times 1 thru 9, I assume you 
                        // also return 55 (although one could argue they should return
                        // 99).  At time 10, you return 99.

                        // If you never have a timestamp that exactly equals 10, you
                        // would never get the new value.  Seems odd.  Well, that's
                        // how I've written it, anyway.
                        if (rt < m.rtEnd)
                        {
                            ret = m.valStart;
                        }
                        else
                        {
                            ret = m.valEnd;
                        }
                        break;

                    case MPCaps.Linear:
                        ir = m.rtEnd - m.rtStart;
                        ic = rt - m.rtStart;
                        p = ic / ir;
                        ret = new MPData();

                        if (m_DataType == MPType.FLOAT)
                        {
                            ret.vFloat = (m.valEnd.vFloat - m.valStart.vFloat) * p;
                        }
                        else
                        {
                            ret.vInt = (int)((m.valEnd.vInt - m.valStart.vInt) * p);
                        }
                        break;

                    case MPCaps.InvSquare:
                        ir = m.rtEnd - m.rtStart;
                        ic = rt - m.rtStart;
                        p = ic / ir;
                        p = (float)Math.Sqrt(p);
                        ret = new MPData();

                        if (m_DataType == MPType.FLOAT)
                        {
                            ret.vFloat = (m.valEnd.vFloat - m.valStart.vFloat) * p;
                        }
                        else
                        {
                            ret.vInt = (int)((m.valEnd.vInt - m.valStart.vInt) * p);
                        }
                        break;

                    case MPCaps.Sine:
                        ir = m.rtEnd - m.rtStart;
                        ic = rt - m.rtStart;
                        p = ic / ir;
                        p = (float)((Math.Sin(p * Math.PI - (Math.PI/2)) + 1) / 2);
                        ret = new MPData();

                        if (m_DataType == MPType.FLOAT)
                        {
                            ret.vFloat = (m.valEnd.vFloat - m.valStart.vFloat) * p;
                        }
                        else
                        {
                            ret.vInt = (int)((m.valEnd.vInt - m.valStart.vInt) * p);
                        }
                        break;

                    case MPCaps.Square:
                        ir = m.rtEnd - m.rtStart;
                        ic = rt - m.rtStart;
                        p = ic / ir;
                        p = p * p;
                        ret = new MPData();

                        if (m_DataType == MPType.FLOAT)
                        {
                            ret.vFloat = (m.valEnd.vFloat - m.valStart.vFloat) * p;
                        }
                        else
                        {
                            ret.vInt = (int)((m.valEnd.vInt - m.valStart.vInt) * p);
                        }
                        break;

                    default:
                        Debug.Assert(false, "Invalid flag!");
                        ret = new MPData();
                        break;
                }

                return ret;
            }

            /// <summary>
            /// Make sure the envelope parameters are valid
            /// </summary>
            /// <param name="m">Envelope segment to check</param>
            /// <returns>E_INVALIDARG if parameters are incorrect, else S_OK</returns>
            public int ValidateEnvelopeSegment(MPEnvelopeSegment m)
            {
                int hr;

                // The start time cannot be MAX_TIME
                // The end time must be >= start time
                // The iCurve must be one of the recognized formats
                // The iCurve must be one of the supported formats
                // The iCurve can only be one format
                // The flags must be one of the recognized formats
                // The flags can only be one value

                if (
                    (m.rtStart == MAX_TIME) ||
                    (m.rtEnd < m.rtStart) ||
                    ((m.iCurve & (~(MPCaps.InvSquare | MPCaps.Jump | MPCaps.Linear | MPCaps.Sine | MPCaps.Square))) > 0) ||
                    ((((int)m.iCurve) & (~(m_ValidCaps))) > 0) ||
                    (IMediaObjectImpl.CountBits((int)m.iCurve) > 1) ||
                    ((m.flags & (~(MPFlags.BeginCurrentVal | MPFlags.BeginNeutralVal | MPFlags.Standard))) > 0) ||
                    (IMediaObjectImpl.CountBits((int)m.flags) > 1)
                    )
                {
                    hr = E_INVALIDARG;
                }
                else
                {
                    hr = S_OK;
                }

                return hr;
            }

#if DEBUG
            /// <summary>
            /// Debug utility to dump an envelope
            /// </summary>
            public void DumpEnvelope()
            {
                int c = m_Envelope.Count;
                Debug.WriteLine("----------------------------");

                for (int x=0; x < c; x++)
                {
                    MPEnvelopeSegment mOld = (MPEnvelopeSegment)m_Envelope[x];

                    Debug.WriteLine(string.Format("{0}: {1}-{2} {3}-{4}", x, mOld.rtStart, mOld.rtEnd, mOld.valStart.vInt, mOld.valEnd.vInt));
                }
            }
#endif

            /// <summary>
            /// Utility function called from AddEnvelope.  When adding an envelope segment,
            /// it could happen that the new segment overlaps multiple existing segments.  
            /// This routine walks starting at the specified segment.  It either deletes
            /// the segment or adjusts it until it reaches the ending time of the new
            /// segment.
            /// </summary>
            /// <param name="z">Starting segment</param>
            /// <param name="mNew">new segment</param>
            private void DeleteRest(int z, long rtEnd)
            {
                // Don't walk past the end of the array
                while (z < m_Envelope.Count)
                {
                    MPEnvelopeSegment mOld = (MPEnvelopeSegment)m_Envelope[z];

                    // Have we found the end time of the current segment?
                    if (mOld.rtEnd <= rtEnd)
                    {
                        // Nope.  Delete this segment.
                        m_Envelope.RemoveAt(z);
                    }
                    else
                    {
                        // Yes, this is the end.  Split the old segment
                        mOld.rtStart = rtEnd + 1;
                        m_Envelope[z] = mOld;
                        break;
                    }
                }
            }

            /// <summary>
            /// See if the specified parameter value falls within the allowable range
            /// </summary>
            /// <param name="m">Value to check</param>
            /// <returns>true if the parameter value is valid</returns>
            private bool CheckRange(MPData m)
            {
                bool bRet;

                switch (m_DataType)
                {
                    case MPType.BOOL:
                    case MPType.ENUM:
                    case MPType.INT:
                        bRet = (m.vInt >= m_MinVal.vInt && m.vInt <= m_MaxVal.vInt);
                        break;

                    case MPType.FLOAT:
                        bRet = (m.vFloat >= m_MinVal.vFloat && m.vFloat <= m_MaxVal.vFloat);
                        break;

                    default:
                        Debug.Assert(false, "Invalid Type");
                        bRet = false;
                        break;
                }

                return bRet;
            }

            private void UpdateSegment(int y, ref MPEnvelopeSegment m)
            {
                if ((m.flags & MPFlags.BeginNeutralVal) > 0)
                {
                    m.valStart = m_DefaultValue;
                }
                else if ((m.flags & MPFlags.BeginCurrentVal) > 0)
                {
                    if (y > 0)
                    {
                        MPEnvelopeSegment mOld = (MPEnvelopeSegment)m_Envelope[y-1];
                        m.valStart = mOld.valEnd;
                    }
                    else
                    {
                        m.valStart = m_DefaultValue;
                    }
                }
            }
        }

        #endregion

        #region API

        [DllImport("NTDll.dll", EntryPoint="RtlCompareMemory")]
        public static extern int CompareMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        #endregion

        #region Member variables

        // Keeps a count of the number of instances of this class have
        // been instantiated.  Only used by logging.
        static private int iInstanceCount = 0;

        // Status info about the input/output pins
        private PinDef [] m_InputInfo;
        private PinDef [] m_OutputInfo;

        // Have the types been set for all the input/output pins
        private bool m_fTypesSet;

        // All buffers are empty
        private bool m_fFlushed;

        // Has AllocateStreamingResources been successfully been called
        private bool m_fResourcesAllocated;

        // Count of the input/output pins
        private int m_NumInputs;
        private int m_NumOutputs;

        // The log file
        internal Logging m_Log;

        // Struct to support IMediaParamInfo & IMediaParams
        private ParamClass m_Params;

        #endregion

        #region Utility routines

        /// <summary>
        /// Count how many bits are set in an flag
        /// </summary>
        /// <param name="i">bitmap to check</param>
        /// <returns>Count of the number of bits set</returns>
        public static int CountBits(int i)
        {
            int iRet = 0;

            while (i > 0)
            {
                if ((i & 1) > 0)
                {
                    iRet++;
                }
                i >>= 1;
            }

            return iRet;
        }

        /// <summary>
        /// Check to see if two Media Types are exactly the same
        /// </summary>
        /// <param name="pmt1">Media type to compare</param>
        /// <param name="pmt2">Media type to compare</param>
        /// <returns>true if types are identical, else false</returns>
        static public bool TypesMatch(AMMediaType pmt1, AMMediaType pmt2)
        {
            if (pmt1.majorType  == pmt2.majorType &&
                pmt1.subType    == pmt2.subType &&
                pmt1.sampleSize == pmt2.sampleSize &&
                pmt1.formatType == pmt2.formatType &&
                pmt1.formatSize == pmt2.formatSize &&
                pmt1.formatSize == CompareMemory(pmt1.formatPtr, pmt2.formatPtr, pmt1.formatSize))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Make a clone of a media type
        /// </summary>
        /// <param name="pmt1">The AMMediaType to clone</param>
        /// <returns>The clone - must be freed with DsUtils.FreeMediaType when no longer needed or it will leak</returns>
        static protected AMMediaType MoCloneMediaType(AMMediaType pmt1)
        {
            AMMediaType pRet = new AMMediaType();
            DMOUtils.MoCopyMediaType(pRet, pmt1);

            return pRet;
        }

        /// <summary>
        /// Is the media type set for the specified input stream?
        /// </summary>
        /// <param name="ulInputStreamIndex">Zero based stream number to check</param>
        /// <returns>true if the stream type is set</returns>
        protected bool InputTypeSet(int ulInputStreamIndex)
        {
            Debug.Assert(ulInputStreamIndex < m_NumInputs);
            return m_InputInfo[ulInputStreamIndex].fTypeSet;
        }

        /// <summary>
        /// Is the media type set for the specified output stream?
        /// </summary>
        /// <param name="ulOutputStreamIndex">Zero based stream number to check</param>
        /// <returns>true if the stream type is set</returns>
        protected bool OutputTypeSet(int ulOutputStreamIndex)
        {
            Debug.Assert(ulOutputStreamIndex < m_NumOutputs);
            return m_OutputInfo[ulOutputStreamIndex].fTypeSet;
        }
        /// <summary>
        /// Get the AMMediaType for the specified Input stream
        /// </summary>
        /// <param name="ulInputStreamIndex">The stream to get the media type for</param>
        /// <returns>The media type for the stream, or null if not set</returns>
        protected AMMediaType InputType(int ulInputStreamIndex)
        {
            if (!InputTypeSet(ulInputStreamIndex)) 
            {
                return null;
            }
            return m_InputInfo[ulInputStreamIndex].CurrentMediaType;
        }
        /// <summary>
        /// Get the AMMediaType for the specified Output stream
        /// </summary>
        /// <param name="ulOutputStreamIndex">The stream to get the media type for</param>
        /// <returns>The media type for the stream, or null if not set</returns>
        protected AMMediaType OutputType(int ulOutputStreamIndex)
        {
            if (!OutputTypeSet(ulOutputStreamIndex)) 
            {
                return null;
            }
            return m_OutputInfo[ulOutputStreamIndex].CurrentMediaType;
        }

        /// <summary>
        /// Set m_fTypesSet by making sure types are set for all input and non-optional output streams.
        /// </summary>
        /// <returns>true if all types are set</returns>
        protected bool CheckAllTypesSet()
        {
            DMOOutputStreamInfo dwFlags;
            int dw;
            int hr;

            m_fTypesSet = false;
            for (dw = 0; dw < m_NumInputs; dw++) 
            {
                if (!InputTypeSet(dw)) 
                {
                    return false;
                }
            }

            for (dw = 0; dw < m_NumOutputs; dw++) 
            {
                if (!OutputTypeSet(dw)) 
                {
                    //  Check if it's optional
                    hr = InternalGetOutputStreamInfo(dw, out dwFlags);

                    Debug.Assert(0 == (dwFlags & ~(DMOOutputStreamInfo.WholeSamples |
                        DMOOutputStreamInfo.SingleSamplePerBuffer |
                        DMOOutputStreamInfo.FixedSampleSize |
                        DMOOutputStreamInfo.Discardable |
                        DMOOutputStreamInfo.Optional)));

                    if ((dwFlags & DMOOutputStreamInfo.Optional) > 0) 
                    {
                        return false;
                    }
                }
            }

            m_fTypesSet = true;

            return true;
        }

        /// <summary>
        /// Handle thrown exceptions in a consistent way.
        /// </summary>
        /// <param name="e">The exception that was thrown</param>
        /// <returns>HRESULT to return to COM</returns>
        private int CatFail(Exception e)
        {
            m_Log.Write(string.Format("Catastrophic failure: {0}", e.Message));

            int hr = Marshal.GetHRForException(e);
            if (hr >= 0)
            {
                hr = E_UNEXPECTED;
            }
            return hr;
        }

        /// <summary>
        /// Create a definition for a parameter that is accessible thru IMediaParamInfo
        /// & IMediaParams.
        /// </summary>
        /// <param name="iParamNum">zero based parameter number to set the definition for</param>
        /// <param name="p">Populated ParamInfo struct</param>
        /// <param name="sText">format string (described in MSDN under IMediaParamInfo::GetParamText)</param>
        protected void DefineParam(int iParamNum, ParamInfo p, string sText)
        {
            m_Params.DefineParam(iParamNum, p, sText);
        }

        protected MPData CalcValueForTime(int dwParam, long rtTimeStamp)
        {
            return m_Params.Envelopes[dwParam].CalcValueForTime(rtTimeStamp);
        }


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iInputs">Number of input streams</param>
        /// <param name="iOutputs">Number of output streams</param>
        protected IMediaObjectImpl(int iInputs, int iOutputs, int iParams, TimeFormatFlags iTimeFormats)
        {
            iInstanceCount++;
            m_Log = new Logging(iInstanceCount);
            m_Log.Write("-------------------------------\r\n");
            m_Log.Write(string.Format("bConstructor {0}\r\n", iInstanceCount));

            m_NumInputs = iInputs;
            m_NumOutputs = iOutputs;
            m_fTypesSet = false;
            m_fFlushed = true;
            m_fResourcesAllocated = false;
            m_InputInfo = new PinDef[m_NumInputs];
            m_OutputInfo = new PinDef[m_NumOutputs];

            // Protect ourselves from incorrectly written children
            m_Params = new ParamClass(iParams, iTimeFormats);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~IMediaObjectImpl() 
        {
            int dwCurrentType;

            m_Log.Write(string.Format("bDestructor {0}\r\n", iInstanceCount));

            for (dwCurrentType = 0; dwCurrentType < m_NumInputs; dwCurrentType++) 
            {
                if(InputTypeSet(dwCurrentType)) 
                {
                    DsUtils.FreeAMMediaType(m_InputInfo[dwCurrentType].CurrentMediaType);
                }
            }

            for (dwCurrentType = 0; dwCurrentType < m_NumOutputs; dwCurrentType++) 
            {
                if(OutputTypeSet(dwCurrentType)) 
                {
                    DsUtils.FreeAMMediaType(m_OutputInfo[dwCurrentType].CurrentMediaType);
                }
            }

            m_Log.Dispose();
        }


        #region Abstract methods

        // These methods MUST be implemented by any class that wants to inherit from IMediaObjectImpl

        // Reports whether the DMO supports the specified AMMediaType for input
        abstract protected int InternalCheckInputType(int dwInputStreamIndex, AMMediaType pmt);

        // Reports whether the DMO supports the specified AMMediaType for output
        abstract protected int InternalCheckOutputType(int dwOutputStreamIndex, AMMediaType pmt);

        // Reports the size and alignment needed for output buffers
        abstract protected int InternalGetOutputSizeInfo(int dwOutputStreamIndex, out int pcbSize, out int pcbAlignment);

        // Flushes all input/output buffers
        abstract protected int InternalFlush();

        // Examine the input buffers to make sure they can be processed.  Possibly begin the processing.
        abstract protected int InternalProcessInput(int dwInputStreamIndex, IMediaBuffer pBuffer, DMOInputDataBuffer dwFlags, long rtTimestamp, long rtTimelength);

        // Fill the output buffers with processed data from the input buffers
        abstract protected int InternalProcessOutput(DMOProcessOutput dwFlags, int cOutputBufferCount,  DMOOutputDataBuffer [] pOutputBuffers,  out int pdwStatus);

        // Report whether you are ready for more input buffers
        abstract protected int InternalAcceptingInput(int dwInputStreamIndex);

        #endregion

        #region Virtual Methods

        // Child classes MAY override these if they need additional/different processing

        // Assumes the stream has no time stamps or that the stream is stopped
        virtual protected long GetCurrentTime()
        {
            return 0;
        }

        // Assumes we don't need to allocate any addition resources to perform the processing
        virtual protected int InternalAllocateStreamingResources()
        {
            return S_OK;
        }

        // Assumes we don't have any resources that need to be freed
        virtual protected int InternalFreeStreamingResources()
        {
            return S_OK;
        }

        // Reports what we need to be able to process an input buffer
        virtual protected int InternalGetInputStreamInfo(int dwInputStreamIndex, out DMOInputStreamInfo pdwFlags)
        {
            pdwFlags = DMOInputStreamInfo.FixedSampleSize |
                DMOInputStreamInfo.SingleSamplePerBuffer |
                DMOInputStreamInfo.FixedSampleSize;

            return S_OK;
        }

        // Reports what we will provide for output buffers
        virtual protected int InternalGetOutputStreamInfo(int dwOutputStreamIndex, out DMOOutputStreamInfo pdwFlags)
        {
            pdwFlags = DMOOutputStreamInfo.WholeSamples |
                DMOOutputStreamInfo.SingleSamplePerBuffer |
                DMOOutputStreamInfo.FixedSampleSize;

            return S_OK;
        }

        // We don't enumerate our supported type.  Just try setting something and see if it works.
        virtual protected int InternalGetInputType(int dwInputStreamIndex, int dwTypeIndex,  out AMMediaType pmt)
        {
            pmt = null;
            return DMOResults.E_NoMoreItems;
        }

        // Assumes our output type is the same as our input type
        virtual protected int InternalGetOutputType(int dwOutputStreamIndex, int dwTypeIndex, out AMMediaType pmt)
        {
            int hr;

            if (InputTypeSet(dwOutputStreamIndex))
            {
                if (dwTypeIndex == 0)
                {
                    pmt = InputType(0);
                    hr = S_OK;
                }
                else
                {
                    pmt = null;
                    hr = DMOResults.E_NoMoreItems;
                }
            }
            else
            {
                pmt = null;
                hr = DMOResults.E_TypeNotSet;
            }

            return hr;
        }

        // Reports that we can accept any alignment, hold no lookahead buffers, and the
        // input buffer must be at least 1 byte long
        virtual protected int InternalGetInputSizeInfo(int dwInputStreamIndex, out int pcbSize, out int pcbMaxLookahead, out int pcbAlignment)
        {
            pcbSize = 1;
            pcbMaxLookahead = 0;
            pcbAlignment = 1;

            return S_OK;
        }

        // We don't support latency
        virtual protected int InternalGetInputMaxLatency(int dwInputStreamIndex, out long prtMaxLatency)
        {
            prtMaxLatency = 0;

            return E_NOTIMPL;
        }

        // We don't support latency
        virtual protected int InternalSetInputMaxLatency(int dwInputStreamIndex, long rtMaxLatency)
        {
            return E_NOTIMPL;
        }

        // No special processing is needed to support Discontinuities
        virtual protected int InternalDiscontinuity(int dwInputStreamIndex)
        {
            return S_OK;
        }


        #endregion

        #region IMediaObject methods

        public int GetStreamCount(out int pulNumberOfInputStreams, out int pulNumberOfOutputStreams)
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("GetStreamCount");

                    // Return the number of input/output streams
                    pulNumberOfInputStreams  = m_NumInputs;
                    pulNumberOfOutputStreams = m_NumOutputs;

                    m_Log.WriteNoTS(string.Format(" {0}:{1}\r\n", m_NumInputs, m_NumOutputs));

                    hr = S_OK;
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have these to make the compiler happy.  Should be safe since
                // GetStreamCount is doc'ed not to accept NULLs.
                pulNumberOfInputStreams  = 0;
                pulNumberOfOutputStreams = 0;
            }

            return hr;
        }

        public int GetInputStreamInfo(int ulStreamIndex, out DMOInputStreamInfo pdwFlags)
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pdwFlags = 0;
                    m_Log.Write("GetInputStreamInfo");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        // Call the internal function to get the value
                        hr = InternalGetInputStreamInfo(ulStreamIndex, out pdwFlags);

                        // Validate the value returned by the internal function
                        Debug.Assert(0 == (pdwFlags & ~(DMOInputStreamInfo.WholeSamples |
                            DMOInputStreamInfo.SingleSamplePerBuffer |
                            DMOInputStreamInfo.FixedSampleSize |
                            DMOInputStreamInfo.HoldsBuffers)));
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }

                    m_Log.WriteNoTS(string.Format(": {0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pdwFlags = 0;
            }

            return hr;
        }

        public int GetOutputStreamInfo(int ulStreamIndex, out DMOOutputStreamInfo pdwFlags)
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pdwFlags = 0;
                    m_Log.Write(string.Format("GetOutputStreamInfo({0})", ulStreamIndex));

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        // Call the internal function to get the value
                        hr = InternalGetOutputStreamInfo(ulStreamIndex, out pdwFlags);

                        // Validate the value returned by the internal function
                        Debug.Assert(0 == (pdwFlags & ~(DMOOutputStreamInfo.WholeSamples |
                            DMOOutputStreamInfo.SingleSamplePerBuffer |
                            DMOOutputStreamInfo.FixedSampleSize |
                            DMOOutputStreamInfo.Discardable |
                            DMOOutputStreamInfo.Optional)));
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }

                    m_Log.WriteNoTS(string.Format("= {1}: {0}\r\n", hr, pdwFlags));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pdwFlags = 0;
            }

            return hr;
        }

        public int GetInputType(int ulStreamIndex, int ulTypeIndex, AMMediaType pmt) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write(string.Format("GetInputType({0}, {1})", ulStreamIndex, ulTypeIndex));

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0 ) 
                    {
                        AMMediaType pmt2;

                        // Call the internal method to get the MediaType
                        hr = InternalGetInputType(ulStreamIndex, ulTypeIndex, out pmt2);

                        // If InternalGetInputType returned a value, and if the caller
                        // provided a place for it, copy it over
                        if (hr >= 0 && pmt != null)
                        {
                            DMOUtils.MoCopyMediaType(pmt, pmt2);
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                    m_Log.WriteNoTS(string.Format("= {0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int GetOutputType(int ulStreamIndex, int ulTypeIndex, AMMediaType pmt) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write(string.Format("GetOutputType({0}, {1})", ulStreamIndex, ulTypeIndex));

                    // Validate the stream number
                    if (ulStreamIndex < m_NumOutputs && ulStreamIndex >= 0) 
                    {
                        AMMediaType pmt2;

                        hr = InternalGetOutputType(ulStreamIndex, ulTypeIndex, out pmt2);

                        // If InternalGetInputType returned a value, and if the caller
                        // provided a place for it, copy it over
                        if (hr >= 0)
                        {
                            m_Log.WriteNoTS(string.Format(" out:{0}", DsToString.AMMediaTypeToString(pmt2)));
                            if (pmt != null)
                            {
                                DMOUtils.MoCopyMediaType(pmt, pmt2);
                            }
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }

                    m_Log.WriteNoTS(string.Format(": {0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int GetInputCurrentType(int ulStreamIndex, out AMMediaType pmt) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pmt = null;
                    m_Log.Write("GetInputCurrentType\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        // If there *is* a current type
                        if (InputTypeSet(ulStreamIndex))
                        {
                            pmt = MoCloneMediaType(m_InputInfo[ulStreamIndex].CurrentMediaType);
                            hr = S_OK;
                        }
                        else
                        {
                            hr = DMOResults.E_TypeNotSet;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pmt = null;
            }

            return hr;
        }

        public int GetOutputCurrentType(int ulStreamIndex, out AMMediaType pmt) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pmt = null;
                    m_Log.Write("GetOutputCurrentType\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumOutputs && ulStreamIndex >= 0) 
                    {
                        // If there *is* a current type
                        if (OutputTypeSet(ulStreamIndex))
                        {
                            pmt = MoCloneMediaType(m_OutputInfo[ulStreamIndex].CurrentMediaType);
                            hr = S_OK;
                        }
                        else
                        {
                            hr = DMOResults.E_TypeNotSet;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pmt = null;
            }

            return hr;
        }

        public int GetInputSizeInfo(int ulStreamIndex, out int pulSize, out int pcbMaxLookahead, out int pulAlignment) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pulSize = 0;
                    pcbMaxLookahead = 0;
                    pulAlignment = 0;

                    m_Log.Write("GetInputSizeInfo");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        // If there is a type set (otherwise, how can we report a size for it?)
                        if (InputTypeSet(ulStreamIndex)) 
                        {
                            hr = InternalGetInputSizeInfo(ulStreamIndex, out pulSize, out pcbMaxLookahead, out pulAlignment);
                        }
                        else
                        {
                            hr = DMOResults.E_TypeNotSet;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                    m_Log.WriteNoTS(string.Format("= {0} {1} {2} : {3}\r\n", pulSize, pcbMaxLookahead, pulAlignment, hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pulSize = 0;
                pcbMaxLookahead = 0;
                pulAlignment = 0;
            }

            return hr;
        }

        public int GetOutputSizeInfo(int ulStreamIndex, out int pulSize, out int pulAlignment) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pulSize = 0;
                    pulAlignment = 0;
                    m_Log.Write("GetOutputSizeInfo\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumOutputs && ulStreamIndex >= 0) 
                    {
                        // If there is a type set (otherwise, how can we report a size for it?)
                        if (m_fTypesSet && OutputTypeSet(ulStreamIndex)) 
                        {
                            hr = InternalGetOutputSizeInfo(ulStreamIndex, out pulSize, out pulAlignment);
                        }
                        else
                        {
                            hr = DMOResults.E_TypeNotSet;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pulSize = 0;
                pulAlignment = 0;
            }

            return hr;
        }

        public int SetInputType(int ulStreamIndex, AMMediaType pmt, DMOSetType dwFlags) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write(string.Format("SetInputType({0}, {1})", ulStreamIndex, dwFlags));

                    // Validate the stream number
                    if (ulStreamIndex >= m_NumInputs || ulStreamIndex < 0) 
                    {
                        return DMOResults.E_InvalidStreamIndex;
                    }

                    // Validate the flags
                    if ( (dwFlags & ~ (DMOSetType.Clear | DMOSetType.TestOnly)) > 0)
                    {
                        return E_INVALIDARG;
                    }

                    // If they requested to clear the current type
                    if ((dwFlags & DMOSetType.Clear) > 0)
                    {
                        DsUtils.FreeAMMediaType(m_InputInfo[ulStreamIndex].CurrentMediaType);
                        m_InputInfo[ulStreamIndex].fTypeSet = false;

                        // Re-check to see if all the non-optional streams still have types
                        if (!CheckAllTypesSet()) 
                        {
                            // We aren't in a runnable state anymore, flush things
                            Flush();
                            FreeStreamingResources();
                        }
                        hr = S_OK;
                    }
                    else
                    {
                        if (null != pmt)
                        {
                            m_Log.WriteNoTS(string.Format(" requested in: {0}", DsToString.AMMediaTypeToString(pmt)));

                            // Check to see if the type is already set
                            if (!InputTypeSet(ulStreamIndex))
                            {
                                // Check to see if we support the requested format
                                hr = InternalCheckInputType(ulStreamIndex, pmt);
                                if (hr >= 0)
                                {
                                    hr = S_OK;

                                    // If we weren't just being tested, actually set the type
                                    if ( (dwFlags & DMOSetType.TestOnly) == 0)
                                    {
                                        m_Log.WriteNoTS(string.Format(" input: {0}", DsToString.AMMediaTypeToString(pmt)));

                                        // Free any previous mediatype
                                        if (InputTypeSet(ulStreamIndex)) 
                                        {
                                            DsUtils.FreeAMMediaType(m_InputInfo[ulStreamIndex].CurrentMediaType);
                                        }
                                        m_InputInfo[ulStreamIndex].CurrentMediaType = MoCloneMediaType(pmt);
                                        m_InputInfo[ulStreamIndex].fTypeSet = true;

                                        // Re-check to see if all the non-optional streams still have types
                                        CheckAllTypesSet();
                                    }
                                }
                            }
                            else
                            {
                                // Type is set, so reject any type that's not identical
                                if (TypesMatch(pmt, InputType(ulStreamIndex)))
                                {
                                    hr = S_OK;
                                }
                                else
                                {
                                    hr = DMOResults.E_InvalidType;
                                }
                            }
                        }
                        else
                        {
                            hr = E_POINTER;
                        }
                    }

                    m_Log.WriteNoTS(string.Format(":{0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int SetOutputType(int ulStreamIndex, AMMediaType pmt, DMOSetType dwFlags) 
        {
            int hr = S_OK;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write(string.Format("SetOutputType({0}, {1})", ulStreamIndex, dwFlags));

                    // Validate the stream number
                    if (ulStreamIndex >= m_NumOutputs || ulStreamIndex < 0) 
                    {
                        return DMOResults.E_InvalidStreamIndex;
                    }

                    // Validate the flags
                    if ( (dwFlags & ~ (DMOSetType.Clear | DMOSetType.TestOnly)) > 0)
                    {
                        return E_INVALIDARG;
                    }

                    // If they requested to clear the current type
                    if ( (dwFlags & DMOSetType.Clear) > 0)
                    {
                        DsUtils.FreeAMMediaType(m_OutputInfo[ulStreamIndex].CurrentMediaType);
                        m_OutputInfo[ulStreamIndex].fTypeSet = false;

                        // Re-check to see if all the non-optional streams still have types
                        if (!CheckAllTypesSet()) 
                        {
                            // We aren't in a runnable state anymore, flush things
                            Flush();
                            FreeStreamingResources();
                        }
                        hr = S_OK;
                    }
                    else
                    {
                        if (null != pmt) 
                        {
                            m_Log.WriteNoTS(string.Format(" requested out: {0}", DsToString.AMMediaTypeToString(pmt)));

                            //  Check if the type is already set
                            if (!OutputTypeSet(ulStreamIndex))
                            {
                                // Check to see if we support the requested format
                                hr = InternalCheckOutputType(ulStreamIndex, pmt);
                                if (hr >= 0) 
                                {
                                    hr = S_OK;

                                    // If we weren't just being tested, actually set the type
                                    if ( (dwFlags & DMOSetType.TestOnly) == 0)
                                    {
                                        // Free any previous mediatype
                                        if (OutputTypeSet(ulStreamIndex)) 
                                        {
                                            DsUtils.FreeAMMediaType(m_OutputInfo[ulStreamIndex].CurrentMediaType);
                                        }
                                        m_OutputInfo[ulStreamIndex].CurrentMediaType = MoCloneMediaType(pmt);
                                        m_OutputInfo[ulStreamIndex].fTypeSet = true;

                                        // Re-check to see if all the non-optional streams still have types
                                        CheckAllTypesSet();
                                    }
                                }
                            }
                            else
                            {
                                // Type is set, so reject any type that's not identical
                                if (!TypesMatch(pmt, OutputType(ulStreamIndex)))
                                {
                                    hr = DMOResults.E_InvalidType;
                                }
                                else
                                {
                                    hr = S_OK;
                                }
                            }
                        }
                        else
                        {
                            hr = E_POINTER;
                        }
                    }

                    m_Log.WriteNoTS(string.Format(":{0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int GetInputStatus(int ulStreamIndex, out DMOInputStatusFlags pdwStatus) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    pdwStatus = DMOInputStatusFlags.None;
                    m_Log.Write("GetInputStatus\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        if (m_fTypesSet) 
                        {
                            hr = InternalAcceptingInput(ulStreamIndex);
                            if (hr == S_OK) 
                            {
                                pdwStatus |= DMOInputStatusFlags.AcceptData;
                            }
                            else if (hr == S_FALSE)
                            {
                                // Not an error, we just aren't accepting input right now
                                hr = S_OK;
                            }
                        }
                        else
                        {
                            hr = DMOResults.E_TypeNotSet;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pdwStatus = DMOInputStatusFlags.None;
            }

            return hr;
        }

        public int GetInputMaxLatency(int ulStreamIndex, out long prtLatency) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    prtLatency = 0;
                    m_Log.Write("GetInputMaxLatency\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        hr = InternalGetInputMaxLatency(ulStreamIndex, out prtLatency);
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                prtLatency = 0;
            }

            return hr;
        }

        public int SetInputMaxLatency(int ulStreamIndex, long rtLatency) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("SetInputMaxLatency\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        hr = InternalSetInputMaxLatency(ulStreamIndex, rtLatency);
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int Discontinuity(int ulStreamIndex) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("Discontinuity\r\n");

                    // Validate the stream number
                    if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                    {
                        if (m_fTypesSet) 
                        {
                            // If we are accepting input, tell the internal
                            // functions about the Discontinuity.
                            hr = InternalAcceptingInput(ulStreamIndex);
                            if (S_OK == hr)
                            {
                                hr = InternalDiscontinuity(ulStreamIndex);
                            }
                            else
                            {
                                // No one cares, just return an error
                                hr = DMOResults.E_NotAccepting;
                            }
                        }
                        else
                        {
                            hr = DMOResults.E_TypeNotSet;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_InvalidStreamIndex;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int Flush()
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("Flush()");

                    // Only call the internal flush if there is something to flush
                    if (m_fTypesSet && !m_fFlushed) 
                    {
                        hr = InternalFlush();
                    }
                    else
                    {
                        hr = S_OK;
                    }

                    m_fFlushed = true;
                    m_Log.WriteNoTS(string.Format(": {0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int AllocateStreamingResources() 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("AllocateStreamingResources");

                    // Don't allocate resources until all streams have a type
                    if (m_fTypesSet) 
                    {
                        // Only need to call it once
                        if (!m_fResourcesAllocated) 
                        {
                            hr = InternalAllocateStreamingResources();
                            if (hr >= 0) 
                            {
                                m_fResourcesAllocated = true;
                            }
                        }
                        else
                        {
                            hr = S_OK;
                        }
                    }
                    else
                    {
                        hr = DMOResults.E_TypeNotSet;
                    }

                    m_Log.WriteNoTS(string.Format(":{0}\r\n", hr));
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int FreeStreamingResources()
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("FreeStreamingResources\r\n");

                    if (m_fResourcesAllocated) 
                    {
                        m_fResourcesAllocated = false;
                        InternalFlush();
                        hr =  InternalFreeStreamingResources();
                        GC.Collect();
                    }
                    else
                    {
                        hr = S_OK;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int ProcessInput(
            int ulStreamIndex,
            IMediaBuffer pBuffer,
            DMOInputDataBuffer dwFlags,
            long rtTimestamp,
            long rtTimelength
            ) 
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write(string.Format("ProcessInput ({0}, {1}, {2}, {3})\r\n", ulStreamIndex, dwFlags, rtTimestamp, rtTimelength));

                    if (pBuffer != null) 
                    {
                        // Validate the stream number
                        if (ulStreamIndex < m_NumInputs && ulStreamIndex >= 0) 
                        {
                            // Validate flags
                            if ( (dwFlags & ~(DMOInputDataBuffer.SyncPoint |
                                DMOInputDataBuffer.Time |
                                DMOInputDataBuffer.TimeLength)) == 0)
                            {
                                //  Make sure all streams have media types set and resources are allocated
                                hr = AllocateStreamingResources();
                                if (hr >= 0) 
                                {
                                    // If we aren't accepting input, forget it
                                    if (InternalAcceptingInput(ulStreamIndex) == S_OK) 
                                    {
                                        m_fFlushed = false;

                                        hr = InternalProcessInput(
                                            ulStreamIndex,
                                            pBuffer,
                                            dwFlags,
                                            rtTimestamp,
                                            rtTimelength);
                                    }
                                    else
                                    {
                                        hr = DMOResults.E_NotAccepting;
                                    }
                                }
                            }
                            else
                            {
                                hr = E_INVALIDARG;
                            }
                        }
                        else
                        {
                            hr = DMOResults.E_InvalidStreamIndex;
                        }
                    }
                    else
                    {
                        hr =  E_POINTER;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }


        public int ProcessOutput(
            DMOProcessOutput dwFlags,
            int ulOutputBufferCount,
            DMOOutputDataBuffer [] pOutputBuffers,
            out int pdwStatus)
        {
            int hr;

            try
            {
                // Avoid multi-threaded access issues
                lock(this)
                {
                    m_Log.Write("ProcessOutput\r\n");
                    pdwStatus = 0;

                    // The number of buffers needs to exactly equal the number of streams
                    if (ulOutputBufferCount == m_NumOutputs && ( (dwFlags & ~DMOProcessOutput.DiscardWhenNoBuffer) == 0)) 
                    {
                        // If there are output streams, pOutputBuffers can't be null
                        if (m_NumOutputs > 0 || pOutputBuffers != null) 
                        {
                            hr = AllocateStreamingResources();
                            if (hr >= 0) 
                            {
                                // Init the status flags to zero
                                int dw;
                                for (dw = 0; dw < m_NumOutputs; dw++) 
                                {
                                    pOutputBuffers[dw].dwStatus = DMOOutputDataBufferFlags.None;
                                }

                                // Fill the buffers
                                hr = InternalProcessOutput(
                                    dwFlags,
                                    ulOutputBufferCount,
                                    pOutputBuffers,
                                    out pdwStatus);

                                // remember the DMO's incomplete status
                                for (dw = 0; dw < m_NumOutputs; dw++) 
                                {
                                    if ( (pOutputBuffers[dw].dwStatus & DMOOutputDataBufferFlags.InComplete) > 0)
                                    {
                                        m_OutputInfo[dw].fIncomplete = true;
                                    } 
                                    else 
                                    {
                                        m_OutputInfo[dw].fIncomplete = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            hr =  E_POINTER;
                        }
                    }
                    else
                    {
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pdwStatus = 0;
            }

            return hr;
        }

        public int Lock(bool lLock)
        {
            // Lock is doc'ed to limit access to multiple threads at the same time.  We
            // are doing that with the lock(this) in each entry point, so this is redundant.
            return S_OK;
        }


        #endregion

        #region IMediaParamInfo Members

        public int GetParamCount(out int pdwParams)
        {
            int hr;

            try
            {
                lock(this)
                {
                    m_Log.Write("GetParamCount\r\n");

                    pdwParams = m_Params.Parms.Length;
                    hr = S_OK;
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pdwParams = 0;
            }

            return hr;
        }

        public int GetSupportedTimeFormat(int dwFormatIndex, out Guid pguidTimeFormat)
        {
            int hr;

            try
            {
                m_Log.Write("GetSupportedTimeFormat\r\n");
                lock(this)
                {
                    // If the index is in range, return the specified timeformat
                    if (dwFormatIndex < m_Params.TimeFormats.Length)
                    {
                        pguidTimeFormat = m_Params.TimeFormats[dwFormatIndex];
                        hr = S_OK;
                    }
                    else
                    {
                        pguidTimeFormat = Guid.Empty;
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pguidTimeFormat = Guid.Empty;
            }

            return hr;
        }

        public int GetParamInfo(int dwParamIndex, out ParamInfo pInfo)
        {
            int hr;

            try
            {
                m_Log.Write("GetParamInfo\r\n");
                lock(this)
                {
                    // If the parameter is in range, return the ParamInfo
                    if (dwParamIndex < m_Params.Parms.Length)
                    {
                        pInfo = m_Params.Parms[dwParamIndex];
                        hr = S_OK;
                    }
                    else
                    {
                        pInfo = null;
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pInfo = null;
            }

            return hr;
        }

        public int GetCurrentTimeFormat(out Guid pguidTimeFormat, out int pTimeData)
        {
            int hr;

            try
            {
                m_Log.Write("GetCurrentTimeFormat\r\n");
                lock(this)
                {
                    // Return the current time format from the array
                    pguidTimeFormat = m_Params.TimeFormats[m_Params.CurrentTimeFormat];
                    pTimeData = m_Params.TimeData;

                    hr = S_OK;
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pguidTimeFormat = Guid.Empty;
                pTimeData = 0;
            }

            return hr;
        }

        public int GetParamText(int dwParamIndex, out string ppwchText)
        {
            int hr;

            try
            {
                m_Log.Write("GetParamText\r\n");
                lock(this)
                {
                    // If the parameter is in range, return the text
                    if (dwParamIndex < m_Params.ParamText.Length)
                    {
                        ppwchText = m_Params.ParamText[dwParamIndex];
                        hr = S_OK;
                    }
                    else
                    {
                        ppwchText = null;
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                ppwchText = null;
            }

            return hr;
        }

        public int GetNumTimeFormats(out int pdwNumTimeFormats)
        {
            int hr;

            try
            {
                m_Log.Write("GetNumTimeFormats\r\n");
                lock(this)
                {
                    pdwNumTimeFormats = m_Params.TimeFormats.Length;
                    hr = S_OK;
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pdwNumTimeFormats = 0;
            }

            return hr;
        }

        #endregion

        #region IMediaParams Members

        public int SetTimeFormat(Guid guidTimeFormat, int mpTimeData)
        {
            int hr = E_INVALIDARG;

            try
            {
                lock(this)
                {
                    m_Log.Write("SetTimeFormat\r\n");

                    // Scan the array of timeformats looking for a match
                    for (int x=0; x < m_Params.TimeFormats.Length; x++)
                    {
                        if (guidTimeFormat == m_Params.TimeFormats[x])
                        {
                            // Found one, grab the index & store the TimeData
                            m_Params.CurrentTimeFormat = x;
                            m_Params.TimeData = mpTimeData;
                            hr = S_OK;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int SetParam(int dwParamIndex, MPData value)
        {
            int hr;

            try
            {
                lock(this)
                {
                    m_Log.Write("SetParam\r\n");

                    // Check to see if the index is in range
                    if (dwParamIndex < m_Params.Parms.Length && dwParamIndex >= 0)
                    {
                        // Make an envelope that spans from current to end of stream
                        MPEnvelopeSegment m = new MPEnvelopeSegment();

                        m.flags = MPFlags.Standard;
                        m.iCurve = MPCaps.Jump;
                        m.rtStart = 0;
                        m.rtEnd = long.MaxValue;
                        m.valStart = value;
                        m.valEnd = value;

                        // Add the struct to the envelope
                        hr = m_Params.Envelopes[dwParamIndex].AddSegment(m);
                    }
                    else
                    {
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }


            return hr;
        }

        public int AddEnvelope(int dwParamIndex, int cSegments, MPEnvelopeSegment[] pEnvelopeSegments)
        {
            int hr;

            try
            {
                lock(this)
                {
                    m_Log.Write("AddEnvelope\r\n");

                    hr = S_OK;

                    if (dwParamIndex == ALLPARAMS)
                    {
                        // Add all the envelopes to all the parameters
                        for (int y = 0; y < m_Params.Parms.Length && hr >= 0; y++)
                        {
                            for (int x = 0; x < cSegments && hr >= 0; x++)
                            {
                                // Add the struct to the envelope
                                hr = m_Params.Envelopes[y].AddSegment(pEnvelopeSegments[x]);
                            }
                        }
                    }
                    else if (dwParamIndex < m_Params.Parms.Length && dwParamIndex >= 0)
                    {
                        // Add all the envelopes to the specified parameter
                        for (int x = 0; x < cSegments && hr >= 0; x++)
                        {
                            // Add the struct to the envelope
                            hr = m_Params.Envelopes[dwParamIndex].AddSegment(pEnvelopeSegments[x]);
                        }
                    }
                    else
                    {
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        public int GetParam(int dwParamIndex, out MPData pValue)
        {
            int hr;

            try
            {
                lock(this)
                {
                    m_Log.Write("GetParam\r\n");

                    // If the requested parameter is within range
                    if (dwParamIndex < m_Params.Parms.Length && dwParamIndex >= 0)
                    {
                        // Read the value
                        pValue = m_Params.Envelopes[dwParamIndex].CalcValueForTime(GetCurrentTime());
                        hr = S_OK;
                    }
                    else
                    {
                        pValue = new MPData();
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);

                // Have to have this to make the compiler happy.
                pValue = new MPData();
            }

            return hr;
        }

        public int FlushEnvelope(int dwParamIndex, long refTimeStart, long refTimeEnd)
        {
            int hr;

            try
            {
                lock(this)
                {
                    m_Log.Write("FlushEnvelope\r\n");

                    // If the time range makes sense
                    if ( (refTimeStart >= 0) && (refTimeEnd >= refTimeStart) )
                    {
                        if (dwParamIndex == ALLPARAMS)
                        {
                            hr = S_OK;

                            // Apply the flush to all parameters
                            for (int x=0; x < m_Params.Parms.Length && hr >= 0; x++)
                            {
                                MPEnvelopeSegment m = new MPEnvelopeSegment();

                                m.flags = MPFlags.Standard;
                                m.iCurve = MPCaps.Jump;
                                m.rtStart = refTimeStart;
                                m.rtEnd = refTimeEnd;
                                m.valStart = m_Params.Parms[x].mpdNeutralValue;
                                m.valEnd = m_Params.Parms[x].mpdNeutralValue;

                                hr = m_Params.Envelopes[dwParamIndex].AddSegment(m);
                            }
                        }
                        else if (dwParamIndex < m_Params.Parms.Length)
                        {
                            // Apply the flush to the specified parameter
                            MPEnvelopeSegment m = new MPEnvelopeSegment();

                            m.flags = MPFlags.Standard;
                            m.iCurve = MPCaps.Jump;
                            m.rtStart = refTimeStart;
                            m.rtEnd = refTimeEnd;
                            m.valStart = m_Params.Parms[dwParamIndex].mpdNeutralValue;
                            m.valEnd = m_Params.Parms[dwParamIndex].mpdNeutralValue;

                            hr = m_Params.Envelopes[dwParamIndex].AddSegment(m);
                        }
                        else
                        {
                            hr = E_INVALIDARG;
                        }
                    }
                    else
                    {
                        hr = E_INVALIDARG;
                    }
                }
            }
            catch (Exception e)
            {
                // Generic handling of all exceptions.  While .NET will turn exceptions into
                // HRESULTS "automatically", I prefer to have some place I can set a breakpoint.
                hr = CatFail(e);
            }

            return hr;
        }

        #endregion
    }


    /// <summary>
    /// Pretty lame logging class.  Note that during Release builds
    /// logging is disabled.
    /// </summary>
    internal class Logging : IDisposable
    {
#if DEBUG
        private static StreamWriter f = new StreamWriter(@"c:\mog.txt", true);
        private int m_Instance;
#endif
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i">Instance number.  Written as part of the header.</param>
        public Logging(int i)
        {
#if DEBUG
            m_Instance = i;
#endif
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Logging()
        {
#if DEBUG
            if (f != null)
            {
                try
                {
                    f.Close();
                    f = null;
                }
                catch {}
            }
#endif
        }

        /// <summary>
        /// Write to the log file without writing the header (Date/Time/Instance).
        /// </summary>
        /// <param name="s">String to write</param>
        public void WriteNoTS(string s)
        {
#if DEBUG
            try
            {
                if (f != null)
                {
                    f.Write(s); 
                    f.Flush();
                }
            }
            catch {}
#endif
        }

        /// <summary>
        /// Write an entry to the log file
        /// </summary>
        /// <param name="s">String to write</param>
        public void Write(string s)
        {
#if DEBUG
            try
            {
                if (f != null)
                {
                    f.Write(string.Format("{0} ({2})- {1}", DateTime.Now, s, m_Instance)); 
                    f.Flush();
                }
            }
            catch {}
#endif
        }
        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
#if DEBUG
            if (f != null)
            {
                try
                {
                    f.Close();
                    f = null;
                    GC.SuppressFinalize(this);
                }
                catch {}
            }
#endif
        }


        #endregion
    }
}
