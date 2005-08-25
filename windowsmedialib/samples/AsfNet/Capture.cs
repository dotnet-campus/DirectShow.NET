// Copyright 2005 - David Wohlferd david@LimeGreenSocks.com

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using WindowsMediaLib;

namespace AsfNet
{
	/// <summary> Summary description for MainForm. </summary>
	internal class Capture: IDisposable
	{
        #region Member variables

        public const int PortNum = 8080;
        public const int MaxClients = 5;

        /// <summary> graph builder interface. </summary>
		private IFilterGraph2 m_graphBuilder = null;
        private IMediaControl m_mediaCtrl = null;

        /// <summary> save this so we can close the port cleanly </summary>
        private IWMWriterNetworkSink m_pNetSink = null;

        /// <summary> Set by async routine when it captures an image </summary>
        private bool m_bRunning = false;

#if DEBUG
        DsROTEntry m_rot = null;
#endif

        #endregion

        /// <summary> release everything. </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            CloseInterfaces();
        }

        ~Capture()
        {
            Dispose();
        }

        /// <summary>
        /// Retrieve the url that should be connected to from media player.  From withing
        /// media player user File/Open URL and entry the string returned from this function.
        /// </summary>
        /// <returns></returns>
        public string GetURL()
        {
            int hr;
            int iSize = 0;

            // Call the function once to get the size
            hr = m_pNetSink.GetHostURL(null, ref iSize);
            WMError.ThrowExceptionForHR(hr);

            StringBuilder sRet = new StringBuilder(iSize, iSize);
            hr = m_pNetSink.GetHostURL(sRet, ref iSize);
            WMError.ThrowExceptionForHR(hr);

            // Trim off the trailing null
            return sRet.ToString().Substring(0, iSize - 1);
        }

        /// <summary>
        /// Create the graph using port PortNum
        /// </summary>
        /// <param name="iDeviceNum">Zero based device index from which to capture</param>
        /// <param name="szFileName">File gets created, but is never actually used</param>
        public Capture(int iDeviceNum, string szFileName)
        {
            DsDevice[] capDevices;

            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat( FilterCategory.VideoInputDevice );

            if (iDeviceNum + 1 > capDevices.Length)
            {
                throw new Exception("No video capture devices found at that index!");
            }

            try
            {
                // Set up the capture graph
                SetupGraph( capDevices[iDeviceNum], szFileName );
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        /// <summary>
        /// Start the capture graph
        /// </summary>
        public void Start()
        {
            if (!m_bRunning)
            {
                int hr = m_mediaCtrl.Run();
                DsError.ThrowExceptionForHR( hr );

                m_bRunning = true;
            }
        }
        /// <summary>
        /// Pause the capture graph - Running the graph takes up a lot of 
        /// resources.  Pause it when it isn't needed.
        /// </summary>
        public void Pause()
        {
            if (m_bRunning)
            {
                int hr = m_mediaCtrl.Pause();
                DsError.ThrowExceptionForHR( hr );

                m_bRunning = false;
            }
        }


        /// <summary> build the capture graph. </summary>
        private void SetupGraph(DsDevice dev, string szFileName)
		{
            int hr;

            IBaseFilter capFilter = null;
            IBaseFilter asfWriter = null;
		    ICaptureGraphBuilder2 capGraph = null;

            // Get the graphbuilder object
            m_graphBuilder = (IFilterGraph2)new FilterGraph();

            try
            {
                // Get the ICaptureGraphBuilder2
                capGraph = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();

                // Start building the graph
                hr = capGraph.SetFiltergraph( (IGraphBuilder)m_graphBuilder );
                DsError.ThrowExceptionForHR( hr );

#if DEBUG
                m_rot = new DsROTEntry( m_graphBuilder );
#endif

                // add the video input device
                hr = m_graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
                DsError.ThrowExceptionForHR( hr );

                // create and configure the asfWriter filter
                asfWriter = ConfigAsf(capGraph, szFileName);
                DoAnet(asfWriter);

                // Add the video device
                hr = m_graphBuilder.AddFilter( capFilter, "Ds.NET Video Capture Device" );
                DsError.ThrowExceptionForHR( hr );

                // Connect the capture device to the asf Writer
                hr = capGraph.RenderStream(null, null, capFilter, null, asfWriter);
                DsError.ThrowExceptionForHR( hr );

                // Use this to start/stop the graph
                m_mediaCtrl = m_graphBuilder as IMediaControl;
            }
            finally
            {
                if (capFilter != null)
                {
                    Marshal.ReleaseComObject(capFilter);
                    capFilter = null;
                }
                if (asfWriter != null)
                {
                    Marshal.ReleaseComObject(asfWriter);
                    asfWriter = null;
                }
                if (capGraph != null)
                {
                    Marshal.ReleaseComObject(capGraph);
                    capGraph = null;
                }
            }
        }

        IBaseFilter ConfigAsf(ICaptureGraphBuilder2 capGraph, string szFileName)
        {
            int hr;

            IBaseFilter asfWriter = null;
            IFileSinkFilter pTmpSink = null;

            // You *must* set an output file name.  Even though it never gets used
            hr = capGraph.SetOutputFileName( MediaSubType.Asf, szFileName, out asfWriter, out pTmpSink);
            DsError.ThrowExceptionForHR( hr );

            try
            {
                WindowsMediaLib.IConfigAsfWriter lConfig = asfWriter as WindowsMediaLib.IConfigAsfWriter;

                // Use one of the pre-defined system profiles.  I picked one with mucho
                // compression and no audio streams

                // Windows Media Video 8 for Dial-up Modem (No audio, 56 Kbps)
                Guid cat = new Guid(0x6E2A6955, 0x81DF, 0x4943, 0xBA, 0x50, 0x68, 0xA9, 0x86, 0xA7, 0x08, 0xF6);

                hr = lConfig.ConfigureFilterUsingProfileGuid(cat);
                DsError.ThrowExceptionForHR( hr );
            }
            finally
            {
                if (pTmpSink != null)
                {
                    Marshal.ReleaseComObject(pTmpSink);
                    pTmpSink = null;
                }
            }

            return asfWriter;
        }

        /// <summary>
        /// Get and configure the IWMWriterAdvanced
        /// </summary>
        /// <param name="asfWriter">IBaseFilter from which to get the IWMWriterAdvanced</param>
        void DoAnet(IBaseFilter asfWriter)
        {
            int hr;
            int dwPortNum = PortNum;

            // Get the IWMWriterAdvanced
            IWMWriterAdvanced pWriterAdvanced = GetWriterAdvanced(asfWriter);

            try
            {
                // Remove all the sinks from the writer
                RemoveAllSinks(pWriterAdvanced);

                // Say we are using live data
                hr = pWriterAdvanced.SetLiveSource(true);
                WMError.ThrowExceptionForHR( hr );

                // Create a network sink
                hr = WMUtils.WMCreateWriterNetworkSink(out m_pNetSink);
                WMError.ThrowExceptionForHR( hr );

                // Configure the network sink
                hr = m_pNetSink.SetNetworkProtocol(NetProtocol.HTTP);
                WMError.ThrowExceptionForHR( hr );

                // Configure the network sink
                hr = m_pNetSink.SetMaximumClients(MaxClients);
                WMError.ThrowExceptionForHR( hr );

                // Done configuring the network sink, open it
                hr = m_pNetSink.Open(ref dwPortNum);
                WMError.ThrowExceptionForHR( hr );
				
                // Add the network sink to the IWMWriterAdvanced
                hr = pWriterAdvanced.AddSink(m_pNetSink as IWMWriterSink);
                WMError.ThrowExceptionForHR( hr );
            }
            finally
            {
                if (pWriterAdvanced != null)
                {
                    Marshal.ReleaseComObject(pWriterAdvanced);
                    pWriterAdvanced = null;
                }
            }
        }

        private IWMWriterAdvanced GetWriterAdvanced(IBaseFilter asfWriter)
        {
            int hr;
            IWMWriterAdvanced pWriterAdvanced = null;

            // I don't understand why we can't just QueryInterface for a IWMWriterAdvanced, but
            // we just can't.  So, we use an IServiceProvider
            WindowsMediaLib.IServiceProvider pProvider = asfWriter as WindowsMediaLib.IServiceProvider;

            if (pProvider != null)
            {
                object opro;

                hr = pProvider.QueryService(typeof(IWMWriterAdvanced2).GUID, typeof(IWMWriterAdvanced2).GUID, out opro);
                WMError.ThrowExceptionForHR( hr );

                pWriterAdvanced = opro as IWMWriterAdvanced;
                if (pWriterAdvanced == null)
                {
                    throw new Exception("Can't get IWMWriterAdvanced");
                }
            } 

            return pWriterAdvanced;
        }

        private void RemoveAllSinks(IWMWriterAdvanced pWriterAdvanced)
        {
            IWMWriterSink ppSink;
            int iSinkCount;

            int hr = pWriterAdvanced.GetSinkCount(out iSinkCount);
            WMError.ThrowExceptionForHR( hr );

            for(int x=iSinkCount - 1; x >= 0; x--)
            {
                hr = pWriterAdvanced.GetSink(x, out ppSink);
                WMError.ThrowExceptionForHR( hr );

                hr = pWriterAdvanced.RemoveSink(ppSink);
                WMError.ThrowExceptionForHR( hr );
            }
        }

        /// <summary> Shut down capture </summary>
		private void CloseInterfaces()
		{
            int hr;

            try
			{
				if( m_mediaCtrl != null )
				{
                    // Stop the graph
                    hr = m_mediaCtrl.Stop();
                    m_mediaCtrl = null;
                    m_bRunning = false;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}

            if (m_pNetSink != null)
            {
                m_pNetSink.Close();
                Marshal.ReleaseComObject(m_pNetSink);
                m_pNetSink = null;
            }

#if DEBUG
            // Remove graph from the ROT
            if ( m_rot != null )
            {
                m_rot.Dispose();
                m_rot = null;
            }
#endif

            if (m_graphBuilder != null)
            {
                Marshal.ReleaseComObject(m_graphBuilder);
                m_graphBuilder = null;
            }
        }
    }
}
