// Added enum for WindowStyle - couldn't find .net equiv - Used for Get/Set WindowStyle
// Added enum for WindowStyleEx - couldn't find .net equiv - Used for Get/Set WindowStyleEx
// Added enum for OABool.  Can't use bool (uses 1 instead of -1), no MarshalAs 
//    converts to -1. Used for "Get/Set AutoShow", "Get/Set BackgroundPalette", "Get/Set Visible"
//    "Get/Set FullScreenMode", SetWindowForeground, IsCursorHidden, HideCursor
// Added enum for WindowState - couldn't find .net equiv - Used for "Get/Set WindowsState"
// Added struct for ColorRef - couldn't use "Color", since the sizes don't match. - Used 
//    in "Get/Set BorderColor", 
// Left IntPtr for Owner, MessageDrain, & NotifyOwnerMessage since HWND has no .NET equiv.

using System;
using DirectShowLib;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TestClasses
{
    [ComVisible(false), ComImport,
    Guid("e436ebb5-524f-11ce-9f53-0020af0ba770")]
    public class AsyncReader
    {
    }

    public class TestIVideoWindow
	{
        public TestIVideoWindow()
        {
        }

        /// <summary>
        /// Test all IVideoWindow methods
        /// </summary>
        /// <param name="sFileName">The path+name of an existing AVI file.  Contents are not changed.</param>
        public void DoTest(string sFileName)
        {
            int hr;
            string s;
            WindowStyle ws1, ws2;
            WindowStyleEx wsx1, wsx2;
            OABool autoShow1, autoShow2;
            WindowState WindowState1, WindowState2;
            OABool BackgroundPalette1, BackgroundPalette2;
            OABool Visible1, Visible2;
            int Left1, Left2;
            int Top1, Top2;
            int Height1, Height2;
            int Width1, Width2;
            ColorRef ColorRef1, ColorRef2;
            OABool FullScreenMode1, FullScreenMode2;
            OABool HideCursor1, HideCursor2;
            IntPtr owner1, owner2;
            IntPtr MessageDrain1, MessageDrain2;

            IFilterGraph2 graphBuilder = BuildGraph(sFileName);

            try
            {
                IVideoWindow ivw = graphBuilder as IVideoWindow;

                ////////////
                /// Test Get/Put Caption
                hr = ivw.put_Caption("Foo Bar");
                Marshal.ThrowExceptionForHR(hr);

                hr = ivw.get_Caption(out s);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(s == "Foo Bar");

                ////////////
                /// Test Get/Put Caption

                // Read the current style
                hr = ivw.get_WindowStyle(out ws1);
                Marshal.ThrowExceptionForHR(hr);

                // Flip the caption bit
                ws1 ^= WindowStyle.Caption;

                // Write the changed style
                hr = ivw.put_WindowStyle(ws1);
                Marshal.ThrowExceptionForHR(hr);

                // Read the style
                hr = ivw.get_WindowStyle(out ws2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(ws1 == ws2, "Put/Get WindowStyle");

                ////////////
                /// Get/Put WindowStyleEx
                
                // Read the current WindowStyleEx
                hr = ivw.get_WindowStyleEx(out wsx1);
                Marshal.ThrowExceptionForHR(hr);

                // Flip the Layered bit
                wsx1 ^= WindowStyleEx.Layered;

                // Write the changed style
                hr = ivw.put_WindowStyleEx(wsx1);
                Marshal.ThrowExceptionForHR(hr);

                // Read the style
                hr = ivw.get_WindowStyleEx(out wsx2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(wsx1 == wsx2, "Put/Get WindowStyleEx");

                ////////////////
                /// Get/Put AutoShow

                // Read the current autoshow
                hr = ivw.get_AutoShow(out autoShow1);
                Marshal.ThrowExceptionForHR(hr);

                // reverse it and write it
                hr = ivw.put_AutoShow(~autoShow1);
                Marshal.ThrowExceptionForHR(hr);

                // Read the autoshow
                hr = ivw.get_AutoShow(out autoShow2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(autoShow1 != autoShow2, "Put/Get AutoShow");

                // Try it the other way

                // Read the current autoshow
                hr = ivw.get_AutoShow(out autoShow1);
                Marshal.ThrowExceptionForHR(hr);

                // reverse it and write it
                hr = ivw.put_AutoShow(~autoShow1);
                Marshal.ThrowExceptionForHR(hr);

                // Read the autoshow
                hr = ivw.get_AutoShow(out autoShow2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(autoShow1 != autoShow2, "Put/Get AutoShow");

                ////////////////

                // Read WindowState
                hr = ivw.get_WindowState(out WindowState1);
                Marshal.ThrowExceptionForHR(hr);

                // Write new value
                hr = ivw.put_WindowState(WindowState.Show);
                Marshal.ThrowExceptionForHR(hr);

                // Read the value
                hr = ivw.get_WindowState(out WindowState2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(WindowState1 != WindowState2, "Put/Get WindowState");

                ////////////////

                // Read current setting
                hr = ivw.get_BackgroundPalette(out BackgroundPalette1);
                Marshal.ThrowExceptionForHR(hr);

                // Flip it
                hr = ivw.put_BackgroundPalette(~BackgroundPalette1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read it
                hr = ivw.get_BackgroundPalette(out BackgroundPalette2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(BackgroundPalette1 != BackgroundPalette2, "Put/Get BackgroundPalette");

                // Try it the other way

                // Read current setting
                hr = ivw.get_BackgroundPalette(out BackgroundPalette1);
                Marshal.ThrowExceptionForHR(hr);

                // Flip it
                hr = ivw.put_BackgroundPalette(~BackgroundPalette1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read it
                hr = ivw.get_BackgroundPalette(out BackgroundPalette2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(BackgroundPalette1 != BackgroundPalette2, "Put/Get BackgroundPalette");

                ////////////////

                // Read the current value
                hr = ivw.get_Visible(out Visible1);
                Marshal.ThrowExceptionForHR(hr);

                // Flip it
                hr = ivw.put_Visible(~Visible1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Visible(out Visible2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Visible1 != Visible2, "Put/Get Visible");

                // Try it the other way

                // Read the current value
                hr = ivw.get_Visible(out Visible1);
                Marshal.ThrowExceptionForHR(hr);

                // Flip it
                hr = ivw.put_Visible(~Visible1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Visible(out Visible2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Visible1 != Visible2, "Put/Get Visible");

                ////////////////

                // Read the current value
                hr = ivw.get_Left(out Left1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_Left(Left1 + 1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Left(out Left2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Left1 + 1 == Left2, "Put/Get Left");

                ////////////////

                // Read the current value
                hr = ivw.get_Width(out Width1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_Width(Width1 + 1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Width(out Width2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Width1 + 1 == Width2, "Put/Get Width");

                ////////////////

                // Read the current value
                hr = ivw.get_Top(out Top1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_Top(Top1 + 1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Top(out Top2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Top1 + 1 == Top2, "Put/Get Top");

                ////////////////

                // Read the current value
                hr = ivw.get_Height(out Height1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_Height(Height1 + 1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Height(out Height2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Height1 + 1 == Height2, "Put/Get Height");

                ////////////////

                // Read the current value
                hr = ivw.get_BorderColor(out ColorRef1);
                Marshal.ThrowExceptionForHR(hr);

                ColorRef1.R++;
                ColorRef1.G++;
                ColorRef1.B++;

                // Change it
                hr = ivw.put_BorderColor(ColorRef1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_BorderColor(out ColorRef2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(ColorRef2.R == ColorRef2.R, "Put/Get Height R");
                Debug.Assert(ColorRef2.G == ColorRef2.G, "Put/Get Height G");
                Debug.Assert(ColorRef2.B == ColorRef2.B, "Put/Get Height B");

                ////////////////

                // Read the current value
                hr = ivw.get_FullScreenMode(out FullScreenMode1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_FullScreenMode(~FullScreenMode1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_FullScreenMode(out FullScreenMode2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(FullScreenMode1 != FullScreenMode2, "Put/Get FullScreenMode");

                // Try it the other way

                // Read the current value
                hr = ivw.get_FullScreenMode(out FullScreenMode1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_FullScreenMode(~FullScreenMode1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_FullScreenMode(out FullScreenMode2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(FullScreenMode1 != FullScreenMode2, "Put/Get FullScreenMode");

                ////////////////

                // No way to set it, so just set both possibilities
                hr = ivw.SetWindowForeground(OABool.True);
                Marshal.ThrowExceptionForHR(hr);

                hr = ivw.SetWindowForeground(OABool.False);
                Marshal.ThrowExceptionForHR(hr);

                ////////////////

                // Read the current value
                hr = ivw.IsCursorHidden(out HideCursor1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.HideCursor(~HideCursor1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.IsCursorHidden(out HideCursor2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(HideCursor1 != HideCursor2, "IsCursorHidden/HideCursor");

                // Try it the other way

                // Read the current value
                hr = ivw.IsCursorHidden(out HideCursor1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.HideCursor(~HideCursor1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.IsCursorHidden(out HideCursor2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(HideCursor1 != HideCursor2, "IsCursorHidden/HideCursor");

                ////////////////

                // Read the current value
                hr = ivw.GetWindowPosition(out Left1, out Top1, out Width1, out Height1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.SetWindowPosition(Left1 + 1, Top1 + 1, Width1 + 1, Height1 + 1);
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.GetWindowPosition(out Left2, out Top2, out Width2, out Height2);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(Left1 + 1 == Left2, "GetWindowPosition/SetWindowPosition");
                Debug.Assert(Top1 + 1 == Top2, "GetWindowPosition/SetWindowPosition");
                Debug.Assert(Width1 + 1 == Width2, "GetWindowPosition/SetWindowPosition");
                Debug.Assert(Height1 + 1 == Height2, "GetWindowPosition/SetWindowPosition");

                ////////////////
                
                // No set function.  Just read values
                hr = ivw.GetRestorePosition(out Left1, out Top1, out Width1, out Height1);
                Marshal.ThrowExceptionForHR(hr);

                ////////////////

                // The graph must be running to read these
                IMediaControl mediaCtrl = graphBuilder as IMediaControl;

                hr = mediaCtrl.Run();
                Marshal.ThrowExceptionForHR(hr);

                // No particular way to check it.  Just read it
                hr = ivw.GetMinIdealImageSize(out Width1, out Height1);
                Marshal.ThrowExceptionForHR(hr);
                
                // No particular way to check it.  Just read it
                hr = ivw.GetMaxIdealImageSize(out Width1, out Height1);
                Marshal.ThrowExceptionForHR(hr);

                // Don't need to leave this running
                hr = mediaCtrl.Stop();
                Marshal.ThrowExceptionForHR(hr);
                mediaCtrl = null;

                ////////////////

                // Read the current value
                hr = ivw.get_Owner(out owner1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_Owner((IntPtr)(owner1.ToInt32() + 1));
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_Owner(out owner2);
                Marshal.ThrowExceptionForHR(hr);

                // Put it back to null
                hr = ivw.put_Owner(IntPtr.Zero);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(owner1.ToInt32() + 1 == owner2.ToInt32(), "get/set Owner");

                ////////////////

                // Read the current value
                hr = ivw.get_MessageDrain(out MessageDrain1);
                Marshal.ThrowExceptionForHR(hr);

                // Change it
                hr = ivw.put_MessageDrain((IntPtr)(MessageDrain1.ToInt32() + 1));
                Marshal.ThrowExceptionForHR(hr);

                // Re-read
                hr = ivw.get_MessageDrain(out MessageDrain2);
                Marshal.ThrowExceptionForHR(hr);

                // Put it back to null
                hr = ivw.put_MessageDrain(IntPtr.Zero);
                Marshal.ThrowExceptionForHR(hr);

                // Make sure the value we set is what we just read
                Debug.Assert(MessageDrain1.ToInt32() + 1 == MessageDrain2.ToInt32(), "get/set MessageDrain");

                ////////////////

                // No way to check.  Just call it
                hr = ivw.NotifyOwnerMessage( (IntPtr)1, 1, 0, 0);
                Marshal.ThrowExceptionForHR(hr);
            }
            finally
            {
                Marshal.ReleaseComObject(graphBuilder);
            }
        }

        IFilterGraph2 BuildGraph(string sFileName)
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
                hr = graphBuilder.AddFilter( ibfAVISource, "Ds.NET AsyncReader" );
                Marshal.ThrowExceptionForHR( hr );

                // Set the file name
                IFileSourceFilter fsf = ibfAVISource as IFileSourceFilter;
                hr = fsf.Load(sFileName, null);
                IPinOut = DsGetPin.ByDirection(ibfAVISource, PinDirection.Output);

                // Get the default video renderer
                ibfRenderer = (IBaseFilter) new VideoRendererDefault();

                // Add it to the graph
                hr = graphBuilder.AddFilter( ibfRenderer, "Ds.NET VideoRendererDefault" );
                Marshal.ThrowExceptionForHR( hr );
                IPinIn = DsGetPin.ByDirection(ibfRenderer, PinDirection.Input);

                hr = graphBuilder.Connect(IPinOut, IPinIn);
                Marshal.ThrowExceptionForHR( hr );
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
