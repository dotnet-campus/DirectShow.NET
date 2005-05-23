using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DirectShowLib.Test
{
  public class ISpecifyPropertyPagesTest
	{
    IBaseFilter filter = null;
    IGraphBuilder graphBuilder = null;
    
    public ISpecifyPropertyPagesTest()
		{
		}

    [DllImport("olepro32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
    private static extern int OleCreatePropertyFrame(
      [In] IntPtr hwndOwner, 
      [In] int x, 
      [In] int y,
      [In, MarshalAs(UnmanagedType.LPWStr)] string lpszCaption, 
      [In] int cObjects,
      [In, MarshalAs(UnmanagedType.IUnknown)] ref object ppUnk,
      [In] int cPages,	
      [In] IntPtr pPageClsID, 
      [In] int lcid, 
      [In] int dwReserved, 
      [In] IntPtr pvReserved 
      );

    public void DoTests()
    {
      this.filter = (IBaseFilter) new VideoMixingRenderer9();
      this.graphBuilder = (IGraphBuilder) new FilterGraph();
      this.graphBuilder.AddFilter(this.filter, "VMR9");

      try
      {
        TestGetPages();
      }
      finally
      {
        Marshal.ReleaseComObject(this.filter);
        Marshal.ReleaseComObject(this.graphBuilder);
      }
    }

    public void TestGetPages()
    {
      int hr  = 0;
      FilterInfo filterInfo;
      DsCAUUID pages = new DsCAUUID();

      try
      {
        // Just to retrieve the name of the filter
        hr = this.filter.QueryFilterInfo(out filterInfo);
        DsError.ThrowExceptionForHR(hr);

        // An exception is thrown if the cast is imposible
        ISpecifyPropertyPages propertyPages = (ISpecifyPropertyPages) this.filter;

        // Query PerperyPages Guid
        hr  = propertyPages.GetPages(out pages);
        DsError.ThrowExceptionForHR(hr);

        object obj = (object) this.filter;

        // Display the property window
        hr = OleCreatePropertyFrame(
          IntPtr.Zero, 
          0, 
          0,
          filterInfo.achName,
          1,
          ref obj,
          pages.cElems,
          pages.pElems,
          0,
          0,
          IntPtr.Zero
          );
        DsError.ThrowExceptionForHR(hr);
      }
      finally
      {
        // pElems memory must be freed
        if (pages.pElems != IntPtr.Zero)
          Marshal.FreeCoTaskMem(pages.pElems);
      }
    }
	}
}
