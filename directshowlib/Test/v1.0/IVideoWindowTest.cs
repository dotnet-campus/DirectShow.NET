// Added enum for WindowStyle - couldn't find .net equiv - Used for Get/Set WindowStyle
// Added enum for WindowStyleEx - couldn't find .net equiv - Used for Get/Set WindowStyleEx
// Added enum for OABool.  Can't use bool (uses 1 instead of -1), no MarshalAs 
//    converts to -1. Used for "Get/Set AutoShow", "Get/Set BackgroundPalette", "Get/Set Visible"
//    "Get/Set FullScreenMode", SetWindowForeground, IsCursorHidden, HideCursor
// Added enum for WindowState - couldn't find .net equiv - Used for "Get/Set WindowsState"
// Left IntPtr for Owner, MessageDrain, & NotifyOwnerMessage since HWND has no .NET equiv.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace DirectShowLib.Test
{
	[ComVisible(false), ComImport,
		Guid("e436ebb5-524f-11ce-9f53-0020af0ba770")]
	public class AsyncReader
	{
	}

	[TestFixture]
	public class IVideoWindowTest
	{
		private const string g_TestFile = @"foo.avi";

		private IFilterGraph2 m_graphBuilder;
		private IVideoWindow m_ivw;

		public IVideoWindowTest()
		{
		}

		/// <summary>
		/// Test all IVideoWindow methods
		/// </summary>
		[Test]
		public void DoTests()
		{
			m_graphBuilder = BuildGraph(g_TestFile);
			m_ivw = m_graphBuilder as IVideoWindow;

			try
			{
				TestCaption();
				TestWindowStyle();
				TestWindowStyleEx();
				TestAutoShow();
				TestWindowState();
				TestBackgroundPalette();
				TestSetWindowForeground();
				TestVisible();
				TestLeft();
				TestWidth();
				TestTop();
				TestHeight();
				TestBorderColor();
				TestFullScreenMode();
				TestCursor();
				TestSetWindowPosition();
				TestGetIdealSize();
				TestOwner();
				TestMessageDrain();
				TestNotifyOwnerMessage();
			}
			finally
			{
				if (m_graphBuilder != null)
				{
					Marshal.ReleaseComObject(m_graphBuilder);
				}

				m_graphBuilder = null;
				m_ivw = null;
			}
		}

		////////////
		/// Test Get/Put Caption
		private void TestCaption()
		{
			int hr;
			string s;

			hr = m_ivw.put_Caption("Foo Bar");
			Marshal.ThrowExceptionForHR(hr);

			hr = m_ivw.get_Caption(out s);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(s == "Foo Bar", "Get/Set Caption");
		}

		////////////
		/// Test Get/Put WindowStyle
		private void TestWindowStyle()
		{
			int hr;
			WindowStyle ws1, ws2;

			// Read the current style
			hr = m_ivw.get_WindowStyle(out ws1);
			Marshal.ThrowExceptionForHR(hr);

			// Flip the caption bit
			ws1 ^= WindowStyle.Caption;

			// Write the changed style
			hr = m_ivw.put_WindowStyle(ws1);
			Marshal.ThrowExceptionForHR(hr);

			// Read the style
			hr = m_ivw.get_WindowStyle(out ws2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(ws1 == ws2, "Put/Get WindowStyle");
		}

		////////////
		/// Get/Put WindowStyleEx
		private void TestWindowStyleEx()
		{
			int hr;
			WindowStyleEx wsx1, wsx2;

			// Read the current WindowStyleEx
			hr = m_ivw.get_WindowStyleEx(out wsx1);
			Marshal.ThrowExceptionForHR(hr);

			// Flip the Layered bit
			wsx1 ^= WindowStyleEx.Layered;

			// Write the changed style
			hr = m_ivw.put_WindowStyleEx(wsx1);
			Marshal.ThrowExceptionForHR(hr);

			// Read the style
			hr = m_ivw.get_WindowStyleEx(out wsx2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(wsx1 == wsx2, "Put/Get WindowStyleEx");
		}

		////////////////
		/// Get/Put AutoShow
		private void TestAutoShow()
		{
			int hr;
			OABool autoShow1, autoShow2;

			// Read the current autoshow
			hr = m_ivw.get_AutoShow(out autoShow1);
			Marshal.ThrowExceptionForHR(hr);

			// reverse it and write it
			hr = m_ivw.put_AutoShow(~autoShow1);
			Marshal.ThrowExceptionForHR(hr);

			// Read the autoshow
			hr = m_ivw.get_AutoShow(out autoShow2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(autoShow1 != autoShow2, "Put/Get AutoShow");

			// Try it the other way

			// Read the current autoshow
			hr = m_ivw.get_AutoShow(out autoShow1);
			Marshal.ThrowExceptionForHR(hr);

			// reverse it and write it
			hr = m_ivw.put_AutoShow(~autoShow1);
			Marshal.ThrowExceptionForHR(hr);

			// Read the autoshow
			hr = m_ivw.get_AutoShow(out autoShow2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(autoShow1 != autoShow2, "Put/Get AutoShow");
		}

		////////////////
		/// Get/Put WindowState
		private void TestWindowState()
		{
			int hr;
			WindowState WindowState1, WindowState2;

			// Read WindowState
			hr = m_ivw.get_WindowState(out WindowState1);
			Marshal.ThrowExceptionForHR(hr);

			// Write new value
			hr = m_ivw.put_WindowState(WindowState.Show);
			Marshal.ThrowExceptionForHR(hr);

			// Read the value
			hr = m_ivw.get_WindowState(out WindowState2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(WindowState1 != WindowState2, "Put/Get WindowState");
		}

		////////////////
		/// Get/Put BackgroundPalette
		private void TestBackgroundPalette()
		{
			int hr;
			OABool BackgroundPalette1, BackgroundPalette2;

			// Read current setting
			hr = m_ivw.get_BackgroundPalette(out BackgroundPalette1);
			Marshal.ThrowExceptionForHR(hr);

			// Flip it
			hr = m_ivw.put_BackgroundPalette(~BackgroundPalette1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read it
			hr = m_ivw.get_BackgroundPalette(out BackgroundPalette2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(BackgroundPalette1 != BackgroundPalette2, "Put/Get BackgroundPalette");

			// Try it the other way

			// Read current setting
			hr = m_ivw.get_BackgroundPalette(out BackgroundPalette1);
			Marshal.ThrowExceptionForHR(hr);

			// Flip it
			hr = m_ivw.put_BackgroundPalette(~BackgroundPalette1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read it
			hr = m_ivw.get_BackgroundPalette(out BackgroundPalette2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(BackgroundPalette1 != BackgroundPalette2, "Put/Get BackgroundPalette");
		}

		////////////////
		/// Get/Put Visible
		private void TestVisible()
		{
			int hr;
			OABool Visible1, Visible2;

			// Read the current value
			hr = m_ivw.get_Visible(out Visible1);
			Marshal.ThrowExceptionForHR(hr);

			// Flip it
			hr = m_ivw.put_Visible(~Visible1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Visible(out Visible2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Visible1 != Visible2, "Put/Get Visible");

			// Try it the other way

			// Read the current value
			hr = m_ivw.get_Visible(out Visible1);
			Marshal.ThrowExceptionForHR(hr);

			// Flip it
			hr = m_ivw.put_Visible(~Visible1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Visible(out Visible2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Visible1 != Visible2, "Put/Get Visible");
		}

		////////////////
		/// Get/Put Left
		private void TestLeft()
		{
			int hr;
			int Left1, Left2;

			// Read the current value
			hr = m_ivw.get_Left(out Left1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_Left(Left1 + 1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Left(out Left2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Left1 + 1 == Left2, "Put/Get Left");

		}

		////////////////
		/// Get/Put Width
		private void TestWidth()
		{
			int hr;
			int Width1, Width2;

			// Read the current value
			hr = m_ivw.get_Width(out Width1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_Width(Width1 + 1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Width(out Width2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Width1 + 1 == Width2, "Put/Get Width");
		}

		////////////////
		/// Get/Put Top
		private void TestTop()
		{
			int hr;
			int Top1, Top2;

			// Read the current value
			hr = m_ivw.get_Top(out Top1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_Top(Top1 + 1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Top(out Top2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Top1 + 1 == Top2, "Put/Get Top");

		}

		////////////////
		/// Get/Put Height
		private void TestHeight()
		{
			int hr;
			int Height1, Height2;

			// Read the current value
			hr = m_ivw.get_Height(out Height1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_Height(Height1 + 1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Height(out Height2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Height1 + 1 == Height2, "Put/Get Height");
		}

		////////////////
		/// Get/Put BorderColor
		private void TestBorderColor()
		{
			int hr;
			int ColorRef1, ColorRef2;

			// Read the current value
			hr = m_ivw.get_BorderColor(out ColorRef1);
			Marshal.ThrowExceptionForHR(hr);

			ColorRef1++;

			// Change it
			hr = m_ivw.put_BorderColor(ColorRef1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_BorderColor(out ColorRef2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(ColorRef1 == ColorRef2, "Put/Get Border Color");
		}

		////////////////
		/// Get/Put FullScreenMode
		private void TestFullScreenMode()
		{
			int hr;
			OABool FullScreenMode1, FullScreenMode2;

			// Read the current value
			hr = m_ivw.get_FullScreenMode(out FullScreenMode1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_FullScreenMode(~FullScreenMode1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_FullScreenMode(out FullScreenMode2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(FullScreenMode1 != FullScreenMode2, "Put/Get FullScreenMode");

			// Try it the other way

			// Read the current value
			hr = m_ivw.get_FullScreenMode(out FullScreenMode1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_FullScreenMode(~FullScreenMode1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_FullScreenMode(out FullScreenMode2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(FullScreenMode1 != FullScreenMode2, "Put/Get FullScreenMode");
		}

		////////////////
		/// SetWindowForeground
		private void TestSetWindowForeground()
		{
			int hr;

			// No way to get it, so just set both possibilities
			hr = m_ivw.SetWindowForeground(OABool.True);
			Marshal.ThrowExceptionForHR(hr);

			hr = m_ivw.SetWindowForeground(OABool.False);
			Marshal.ThrowExceptionForHR(hr);
		}

		////////////////
		/// IsCursorHidden/HideCursor
		private void TestCursor()
		{
			int hr;
			OABool HideCursor1, HideCursor2;

			// Read the current value
			hr = m_ivw.IsCursorHidden(out HideCursor1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.HideCursor(~HideCursor1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.IsCursorHidden(out HideCursor2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(HideCursor1 != HideCursor2, "IsCursorHidden/HideCursor");

			// Try it the other way

			// Read the current value
			hr = m_ivw.IsCursorHidden(out HideCursor1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.HideCursor(~HideCursor1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.IsCursorHidden(out HideCursor2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(HideCursor1 != HideCursor2, "IsCursorHidden/HideCursor");
		}

		////////////////
		/// Get/Set WindowPosition
		private void TestSetWindowPosition()
		{
			int hr;
			int Left1, Left2;
			int Width1, Width2;
			int Top1, Top2;
			int Height1, Height2;

			// Read the current value
			hr = m_ivw.GetWindowPosition(out Left1, out Top1, out Width1, out Height1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.SetWindowPosition(Left1 + 1, Top1 + 1, Width1 + 1, Height1 + 1);
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.GetWindowPosition(out Left2, out Top2, out Width2, out Height2);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(Left1 + 1 == Left2, "GetWindowPosition/SetWindowPosition");
			Debug.Assert(Top1 + 1 == Top2, "GetWindowPosition/SetWindowPosition");
			Debug.Assert(Width1 + 1 == Width2, "GetWindowPosition/SetWindowPosition");
			Debug.Assert(Height1 + 1 == Height2, "GetWindowPosition/SetWindowPosition");

			////////////////

			// No set function.  Just read values
			hr = m_ivw.GetRestorePosition(out Left1, out Top1, out Width1, out Height1);
			Marshal.ThrowExceptionForHR(hr);

		}

		////////////////
		/// GetMinIdealImageSize GetMaxIdealImageSize
		private void TestGetIdealSize()
		{
			int hr;
			int Width1;
			int Height1;

			// The graph must be running to read these
			IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;

			hr = mediaCtrl.Run();
			Marshal.ThrowExceptionForHR(hr);

			// No particular way to check it.  Just read it
			hr = m_ivw.GetMinIdealImageSize(out Width1, out Height1);
			Marshal.ThrowExceptionForHR(hr);

			// No particular way to check it.  Just read it
			hr = m_ivw.GetMaxIdealImageSize(out Width1, out Height1);
			Marshal.ThrowExceptionForHR(hr);

			// Don't need to leave this running
			hr = mediaCtrl.Stop();
			Marshal.ThrowExceptionForHR(hr);
			mediaCtrl = null;
		}

		////////////////
		/// Get/Set owner
		private void TestOwner()
		{
			int hr;
			IntPtr owner1, owner2;

			// Read the current value
			hr = m_ivw.get_Owner(out owner1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_Owner((IntPtr) (owner1.ToInt32() + 1));
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_Owner(out owner2);
			Marshal.ThrowExceptionForHR(hr);

			// Put it back to null
			hr = m_ivw.put_Owner(IntPtr.Zero);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(owner1.ToInt32() + 1 == owner2.ToInt32(), "get/set Owner");

		}

		////////////////
		/// Get/Set MessageDrain
		private void TestMessageDrain()
		{
			int hr;
			IntPtr MessageDrain1, MessageDrain2;

			// Read the current value
			hr = m_ivw.get_MessageDrain(out MessageDrain1);
			Marshal.ThrowExceptionForHR(hr);

			// Change it
			hr = m_ivw.put_MessageDrain((IntPtr) (MessageDrain1.ToInt32() + 1));
			Marshal.ThrowExceptionForHR(hr);

			// Re-read
			hr = m_ivw.get_MessageDrain(out MessageDrain2);
			Marshal.ThrowExceptionForHR(hr);

			// Put it back to null
			hr = m_ivw.put_MessageDrain(IntPtr.Zero);
			Marshal.ThrowExceptionForHR(hr);

			// Make sure the value we set is what we just read
			Debug.Assert(MessageDrain1.ToInt32() + 1 == MessageDrain2.ToInt32(), "get/set MessageDrain");

		}

		////////////////
		/// GetMinIdealImageSize GetMaxIdealImageSize
		private void TestNotifyOwnerMessage()
		{
			int hr;

			// No way to check.  Just call it
			hr = m_ivw.NotifyOwnerMessage((IntPtr) 1, 1, IntPtr.Zero, IntPtr.Zero);
			Marshal.ThrowExceptionForHR(hr);
		}

		private IFilterGraph2 BuildGraph(string sFileName)
		{
			int hr;
			IBaseFilter ibfRenderer = null;
			IBaseFilter ibfAVISource = null;
			IPin IPinIn = null;
			IPin IPinOut = null;

			IFilterGraph2 graphBuilder = new FilterGraph() as IFilterGraph2;

			try
			{
				// Get the file source filter
				ibfAVISource = new AsyncReader() as IBaseFilter;

				// Add it to the graph
				hr = graphBuilder.AddFilter(ibfAVISource, "Ds.NET AsyncReader");
				Marshal.ThrowExceptionForHR(hr);

				// Set the file name
				IFileSourceFilter fsf = ibfAVISource as IFileSourceFilter;
				hr = fsf.Load(sFileName, null);
				Marshal.ThrowExceptionForHR(hr);

				IPinOut = DsFindPin.ByDirection(ibfAVISource, PinDirection.Output, 0);

				// Get the default video renderer
				ibfRenderer = (IBaseFilter) new VideoRendererDefault();

				// Add it to the graph
				hr = graphBuilder.AddFilter(ibfRenderer, "Ds.NET VideoRendererDefault");
				Marshal.ThrowExceptionForHR(hr);
				IPinIn = DsFindPin.ByDirection(ibfRenderer, PinDirection.Input, 0);

				hr = graphBuilder.Connect(IPinOut, IPinIn);
				Marshal.ThrowExceptionForHR(hr);
			}
			catch
			{
				Marshal.ReleaseComObject(graphBuilder);
				throw;
			}
			finally
			{
				Marshal.ReleaseComObject(ibfAVISource);
				Marshal.ReleaseComObject(ibfRenderer);
				Marshal.ReleaseComObject(IPinIn);
				Marshal.ReleaseComObject(IPinOut);
			}

			return graphBuilder;
		}
	}
}