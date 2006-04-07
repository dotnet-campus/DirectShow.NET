/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

// This code is just to show a couple of ways you could use the 
// GenericSampleSourceFilter.  For a more details discussion, check out the
// readme.txt

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using DirectShowLib;
using GenericSampleSourceFilterClasses;

namespace DxPlay
{
	/// <summary>
	/// A class to construct a graph using the GenericSampleSourceFilter.
	/// </summary>
	internal class DxPlay : IDisposable, IGenericSampleCB
	{
		#region Definitions

		/// <summary>
		/// 100 ns - used by a number of DS methods
		/// </summary>
		private const long UNIT = 10000000;

		/// <summary>
		/// The Frames Per Second for the source filter to use
		/// </summary>
		private const long FPS =  UNIT / 25; // 25 fps

		#endregion

		#region Member variables

		/// <summary>
		/// Number of frames processed
		/// </summary>
		private int m_iFrameNumber = 0;

		// Event called when the graph stops
		public event EventHandler Completed = null;

		/// <summary>
		/// The class that retrieves the images
		/// </summary>
		private ImageHandler m_ImageHandler;

		/// <summary>
		/// graph builder interfaces
		/// </summary>
		private IFilterGraph2 m_graphBuilder;

		/// <summary>
		/// Another graph builder interface
		/// </summary>
		private IMediaControl m_mediaCtrl;

#if DEBUG
		/// <summary>
		/// Allow you to "Connect to remote graph" from GraphEdit
		/// </summary>
		DsROTEntry m_DsRot;
#endif

		#endregion

		/// <summary>
		/// Release everything
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			CloseInterfaces();
		}

		/// <summary>
		/// Alternate cleanup
		/// </summary>
		~DxPlay()
		{
			CloseInterfaces();
		}

		/// <summary>
		/// Play a video into a window using the GenericSampleSourceFilter as the video source
		/// </summary>
		/// <param name="sPath">Path for the ImageFromFiles class (if that's what we are using)
		/// to use to find images</param>
		/// <param name="hWin">Window to play the video in</param>
		public DxPlay(string sPath, Control hWin)
		{
			try
			{
				// pick one of our two image providers
				//m_ImageHandler = new ImageFromFiles(sPath);
				m_ImageHandler = new ImageFromPixels();

				// Set up the graph
				SetupGraph(hWin);
			}
			catch
			{
				Dispose();
				throw;
			}
		}


		/// <summary>
		/// Start playing
		/// </summary>
		public void Start()
		{
			// Create a new thread to process events
			Thread t;
			t = new Thread(new ThreadStart(EventWait));
			t.Name = "Media Event Thread";
			t.Start();

			int hr = m_mediaCtrl.Run();
			DsError.ThrowExceptionForHR( hr );
		}

		/// <summary>
		/// Stop the capture graph.
		/// </summary>
		public void Stop()
		{
			int hr;
			
			hr = ((IMediaEventSink)m_graphBuilder).Notify(EventCode.UserAbort, 0, 0);
			DsError.ThrowExceptionForHR( hr );

			hr = m_mediaCtrl.Stop();
			DsError.ThrowExceptionForHR( hr );
		}


		/// <summary>
		/// Build the filter graph
		/// </summary>
		/// <param name="hWin">Window to draw into</param>
		private void SetupGraph(Control hWin)
		{
			int hr;

			// Get the graphbuilder object
			m_graphBuilder = new FilterGraph() as IFilterGraph2;

			// Get a ICaptureGraphBuilder2 to help build the graph
			ICaptureGraphBuilder2 icgb2 = (ICaptureGraphBuilder2)new CaptureGraphBuilder2() ;

			try
			{
				// Link the ICaptureGraphBuilder2 to the IFilterGraph2
				hr = icgb2.SetFiltergraph(m_graphBuilder);
				DsError.ThrowExceptionForHR( hr );

#if DEBUG
				// Allows you to view the graph with GraphEdit File/Connect
				m_DsRot = new DsROTEntry(m_graphBuilder);
#endif

				// Our data source
				IBaseFilter ipsb = (IBaseFilter)new GenericSampleSourceFilter();

				try
				{
					// Get the pin from the filter so we can configure it
					IPin ipin = DsFindPin.ByDirection(ipsb, PinDirection.Output, 0);

					try
					{
						// Configure the pin using the provided BitmapInfo
						ConfigurePusher((IGenericSampleConfig)ipin);
					}
					finally
					{
						Marshal.ReleaseComObject(ipin);
					}

					// Add the filter to the graph
					hr = m_graphBuilder.AddFilter(ipsb, "GenericSampleSourceFilter");
					Marshal.ThrowExceptionForHR( hr );

					// Connect the filters together, use the default renderer
					hr = icgb2.RenderStream(null, null, ipsb, null, null);
					DsError.ThrowExceptionForHR( hr );
				}
				finally
				{
					Marshal.ReleaseComObject(ipsb);
				}

				// Configure the Video Window
				IVideoWindow videoWindow = m_graphBuilder as IVideoWindow;
				ConfigureVideoWindow(videoWindow, hWin);

				// Grab some other interfaces
				m_mediaCtrl = m_graphBuilder as IMediaControl;
			}
			finally
			{
				Marshal.ReleaseComObject(icgb2);
			}
		}

		/// <summary>
		/// Configure the GenericSampleSourceFilter
		/// </summary>
		/// <param name="ips">The interface to the GenericSampleSourceFilter</param>
		private void ConfigurePusher(IGenericSampleConfig ips)
		{
			int hr;
			BitmapInfoHeader bmi = m_ImageHandler.GetBMIH();

			// Send in the BitmapInfo struct and the desired FramesPerSecond
			hr = ips.SetMediaTypeFromBitmap(bmi, FPS);
			DsError.ThrowExceptionForHR(hr);

			// Specify the callback routine to call with each sample
			hr = ips.SetBitmapCB(this);
			DsError.ThrowExceptionForHR(hr);
		}

		/// <summary>
		/// Called by the GenericSampleSourceFilter.  This routine populates the MediaSample.
		/// 
		/// Note that the images used by this method MUST all be the same size (Height/
		/// Width/BitsPerPixel).
		/// </summary>
		/// <param name="pSample">Pointer to a sample</param>
		/// <returns>0 = success, 1 = end of stream, negative values for errors</returns>
		public int SampleCallback(IMediaSample pSample)
		{
			int hr;
			IntPtr pData;

			try
			{
				// Get the buffer into which we will copy the data
				hr = pSample.GetPointer(out pData);
				if (hr >= 0)
				{
					// Set TRUE on every sample for uncompressed frames
					hr = pSample.SetSyncPoint(true);
					if (hr >= 0)
					{
						// Find out the amount of space in the buffer
						int cbData = pSample.GetSize();

						// Calculate the start/end times based on the current frame number
						// and frame rate
						DsLong rtStart = new DsLong(m_iFrameNumber * FPS);
						DsLong rtStop  = new DsLong(rtStart + FPS);

						// Set the times into the sample
						hr = pSample.SetTime(rtStart, rtStop);
						if (hr >= 0)
						{
							// Get copy the data into the sample
							hr = m_ImageHandler.GetImage(m_iFrameNumber, pData, cbData);
							if (hr == 0) // 1 == End of stream
							{
								// increment the frame number for next time
								m_iFrameNumber++;
							}
						}
					}
				}
			}
			finally
			{
				// Release our pointer the the media sample.  THIS IS ESSENTIAL!  If
				// you don't do this, the graph will stop after about 2 samples.
				Marshal.ReleaseComObject(pSample);
			}

			return hr;
		}

		/// <summary>
		/// Configure the video window
		/// </summary>
		/// <param name="videoWindow">Interface of the video renderer</param>
		/// <param name="hWin">Handle of the window to draw into</param>
		private void ConfigureVideoWindow(IVideoWindow videoWindow, Control hWin)
		{
			int hr;

			// Set the output window
			hr = videoWindow.put_Owner( hWin.Handle );
			DsError.ThrowExceptionForHR( hr );

			// Set the window style
			hr = videoWindow.put_WindowStyle( (WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings) );
			DsError.ThrowExceptionForHR( hr );

			// Make the window visible
			hr = videoWindow.put_Visible( OABool.True );
			DsError.ThrowExceptionForHR( hr );

			// Position the playing location
			Rectangle rc = hWin.ClientRectangle;
			hr = videoWindow.SetWindowPosition( 0, 0, rc.Right, rc.Bottom );
			DsError.ThrowExceptionForHR( hr );
		}

		/// <summary>
		/// Shut down graph
		/// </summary>
		private void CloseInterfaces()
		{
			int hr;

			lock (this)
			{
				// Stop the graph
				if( m_mediaCtrl != null )
				{
					// Stop the graph
					hr = m_mediaCtrl.Stop();
					m_mediaCtrl = null;
				}

				if (m_ImageHandler != null)
				{
					m_ImageHandler.Dispose();
					m_ImageHandler = null;
				}

#if DEBUG
				if (m_DsRot != null)
				{
					m_DsRot.Dispose();
				}
#endif

				hr = ((IMediaEventSink)m_graphBuilder).Notify(EventCode.UserAbort, 0, 0);

				// Release the graph
				if (m_graphBuilder != null)
				{
					Marshal.ReleaseComObject(m_graphBuilder);
					m_graphBuilder = null;
				}
			}
			GC.Collect();
		}

		/// <summary>
		/// Called on a new thread to process events from the graph.  The thread
		/// exits when the graph finishes.
		/// </summary>
		private void EventWait()
		{
			// Returned when GetEvent is called but there are no events
			const int E_ABORT = unchecked((int)0x80004004);

			int hr;
			int p1, p2;
			EventCode ec;
			EventCode exitCode = 0;

			IMediaEvent pEvent = (IMediaEvent)m_graphBuilder;

			do
			{
				// Read the event
				for (
					hr = pEvent.GetEvent(out ec, out p1, out p2, 100);
					hr >= 0;
					hr = pEvent.GetEvent(out ec, out p1, out p2, 100)
					)
				{
					Debug.WriteLine(ec);
					switch(ec)
					{
							// If the clip is finished playing
						case EventCode.Complete:
						case EventCode.ErrorAbort:
						case EventCode.UserAbort:
							exitCode = ec;

							// Release any resources the message allocated
							hr = pEvent.FreeEventParams(ec, p1, p2);
							DsError.ThrowExceptionForHR(hr);
							break;

						default:
							// Release any resources the message allocated
							hr = pEvent.FreeEventParams(ec, p1, p2);
							DsError.ThrowExceptionForHR(hr);
							break;
					}
				}

				// If the error that exited the loop wasn't due to running out of events
				if (hr != E_ABORT)
				{
					DsError.ThrowExceptionForHR(hr);
				}
			} while (exitCode == 0);

			// Send an event saying we are complete
			if (Completed != null)
			{
				CompletedArgs ca = new CompletedArgs(exitCode);
				Completed(this, ca);
			}

		} // Exit the thread


		public class CompletedArgs : System.EventArgs
		{
			/// <summary>The result of the rendering</summary>
			/// <remarks>
			/// This code will be a member of DirectShowLib.EventCode.  Typically it 
			/// will be EventCode.Complete, EventCode.ErrorAbort or EventCode.UserAbort.
			/// </remarks>
			public EventCode Result;

			/// <summary>
			/// Used to construct an instace of the class.
			/// </summary>
			/// <param name="ec"></param>
			internal CompletedArgs(EventCode ec)
			{
				Result = ec;
			}
		}

	}

	// A generic class to support easily changing between my different sources of bitmaps.

	// Note: You DON'T have to use this class, or anything like it.  The key is the SampleCallback
	// routine.  How/where you get your bitmaps is ENTIRELY up to you.  Having SampleCallback call
	// members of this class was just the approach I used to isolate the bitmap handling.
	abstract internal class ImageHandler : IDisposable
	{
		abstract public void Dispose();
		abstract public BitmapInfoHeader GetBMIH();
		abstract public int GetImage(int iFrameNumber, IntPtr ip, int iSize);

	}

	/// <summary>
	/// Class to provide image data.  Note that the Bitmap class is very easy to use,
	/// but not terribly efficient.  If you aren't getting the performance you need,
	/// replacing that is a good place start.
	/// 
	/// Note that this class assumes that the images to show are all in the same
	/// directory, and are named 00000001.jpg, 00000002.jpg, etc
	/// 
	/// Also, make sure you read the comments on the ImageHandler class.
	/// </summary>
	internal class ImageFromFiles : ImageHandler
	{
		[DllImport("Kernel32.dll", EntryPoint="RtlMoveMemory")]
		private static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);

		/// <summary>
		/// How many frames to show the bitmap in.  Using 1 will return a new
		/// image for each frame.  Setting it to 5 would show the same image
		/// in 5 frames, etc.  So, if you are running at 5 FPS, and you set DIV
		/// to 5, each image will show for 1 second.
		/// </summary>
		private const int DIV = 1;

		#region Member Variables

		/// <summary>
		/// Contains the IntPtr to the raw data
		/// </summary>
		private BitmapData m_bmd;

		/// <summary>
		/// Needed to release the m_bmd member
		/// </summary>
		private Bitmap m_bmp;

		/// <summary>
		/// Path that contains the images
		/// </summary>
		private string m_sPath;

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sPath">The directory that contains the images.</param>
		public ImageFromFiles(string sPath)
		{
			m_sPath = sPath;
		}

		/// <summary>
		/// Dispose
		/// </summary>
		override public void Dispose()
		{
			// Release any outstanding bitmaps
			if (m_bmp != null)
			{
				m_bmp.UnlockBits(m_bmd);
				m_bmp = null;
				m_bmd = null;
			}
		}

		/// <summary>
		/// Create a BitmapInfoHeader struct and populate it
		/// using the first bitmap.
		/// </summary>
		/// <returns>The BitmapInfoHeader</returns>
		override public BitmapInfoHeader GetBMIH()
		{
			BitmapInfoHeader bmi = new BitmapInfoHeader();

			// Make sure we have an image to get the data from
			if (m_bmp == null)
			{
				IntPtr ip = IntPtr.Zero;
				GetImage(0, ip, 0);
			}

			// Build a BitmapInfo struct using the parms from the file
			bmi.Size = Marshal.SizeOf(typeof(BitmapInfoHeader));
			bmi.Width = m_bmd.Width;
			bmi.Height = m_bmd.Height * -1;
			bmi.Planes = 1;
			bmi.BitCount = 32;
			bmi.Compression = 0;
			bmi.ImageSize = (bmi.BitCount / 8) * bmi.Width * bmi.Height;
			bmi.XPelsPerMeter = 0;
			bmi.YPelsPerMeter = 0;
			bmi.ClrUsed = 0;
			bmi.ClrImportant = 0;

			return bmi;
		}

		/// <summary>
		/// Retrieve an image to show.  In this class I'm retrieving bitmaps 
		/// from files based on the current frame number.
		/// </summary>
		/// <param name="iFrameNumber">Frame number</param>
		/// <param name="ip">A pointer to the memory to populate with the bitmap data</param>
		/// <returns>0 on success and 1 on end of stream</returns>
		override public int GetImage(int iFrameNumber, IntPtr ip, int iSize)
		{
			int hr = 0;

			if (iFrameNumber % DIV == 0)
			{
				try
				{
					// Open the next image
					string sFileName = String.Format(@"{1}\{0:00000000}.jpg", iFrameNumber / DIV + 1, m_sPath);
					Bitmap bmp = new Bitmap(sFileName);
					Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);

					// Release the previous image
					if (m_bmd != null)
					{
						m_bmp.UnlockBits(m_bmd);
						m_bmp.Dispose();
					}

					// Store the pointers
					m_bmp = bmp;
					m_bmd = m_bmp.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

					// Only do the copy if we have a place to put the data
					if (ip != IntPtr.Zero)
					{
						// Copy from the bmd to the MediaSample
						CopyMemory(ip, m_bmd.Scan0, iSize);
					}
				}
				catch
				{
					// Presumably we ran out of files.  Terminate the stream
					hr = 1;
				}
			}

			return hr;
		}
	}

	/// <summary>
	/// Alternate class to provide image data.  
	/// 
	/// This class just generates pretty colored bitmaps.
	/// 
	/// Also, make sure you read the comments on the ImageHandler class.
	/// </summary>
	internal class ImageFromPixels : ImageHandler
	{
		/// <summary>
		/// How many frames to show the bitmap in.  Using 1 will return a new
		/// image for each frame.  Setting it to 5 would show the same image
		/// in 5 frames, etc.
		/// </summary>
		private const int DIV = 1;
		private const int HEIGHT = 240;
		private const int WIDTH = 320;
		private const int BPP = 24;

		// How many frames to return before returning End Of Stream
		private const int MAXFRAMES = 1000;

		#region Member Variables

		// Used to make the pretty picture
		private int m_g = 0;
		private int m_b = 0;

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public ImageFromPixels()
		{
		}

		/// <summary>
		/// Dispose
		/// </summary>
		override public void Dispose()
		{
		}

		/// <summary>
		/// Create a BitmapInfoHeader struct and populate it
		/// </summary>
		/// <returns>The BitmapInfoHeader</returns>
		override public BitmapInfoHeader GetBMIH()
		{
			BitmapInfoHeader bmi = new BitmapInfoHeader();

			// Build a BitmapInfo struct using the parms from the file
			bmi.Size = Marshal.SizeOf(typeof(BitmapInfoHeader));
			bmi.Width = WIDTH;
			bmi.Height = HEIGHT * -1;
			bmi.Planes = 1;
			bmi.BitCount = BPP;
			bmi.Compression = 0;
			bmi.ImageSize = (bmi.BitCount / 8) * bmi.Width * bmi.Height;
			bmi.XPelsPerMeter = 0;
			bmi.YPelsPerMeter = 0;
			bmi.ClrUsed = 0;
			bmi.ClrImportant = 0;

			return bmi;
		}


		/// <summary>
		/// Retrieve an image to show.  In this class I'm just generating bitmaps
		/// of random colors.
		/// 
		/// Using Marshal.WriteByte is *really* slow.  For decent performance, consider
		/// using pointers and unsafe code.
		/// </summary>
		/// <param name="iFrameNumber">Frame number</param>
		/// <param name="ip">A pointer to the memory to populate with the bitmap data</param>
		/// <returns>0 on success and 1 on end of stream</returns>
		override public int GetImage(int iFrameNumber, IntPtr ip, int iSize)
		{
			int hr = 0;

			if (iFrameNumber % DIV == 0)
			{

				if (iFrameNumber < MAXFRAMES)
				{
					byte r = (byte)((iFrameNumber * 2) % 255);
					byte g = (byte)((iFrameNumber * 2 + m_g) % 255);
					byte b = (byte)((iFrameNumber * 2 + m_b) % 255);

					m_g += 3;
					m_b += 7;

					for (int x=0; x < (HEIGHT * WIDTH * (BPP / 8)); x+=3)
					{
						Marshal.WriteByte(ip, x+0, r);
						Marshal.WriteByte(ip, x+1, g);
						Marshal.WriteByte(ip, x+2, b);
					}
				}
				else if (iFrameNumber == MAXFRAMES)
				{
					// The last frame will be black
					for (int x=0; x < (HEIGHT * WIDTH * (BPP / 8)); x++)
					{
						Marshal.WriteByte(ip, x, 0);
					}
				}
				else
				{
					hr = 1; // End of stream
				}
			}

			return hr;
		}
	}
}
