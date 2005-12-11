/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Permissions;

using DirectShowLib;

namespace DirectShowLib.Utils
{
	public sealed class FilterGraphTools
	{
		private FilterGraphTools(){}

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static IBaseFilter AddFilterFromClsid(IGraphBuilder graphBuilder, Guid clsid, string name)
    {
      int hr = 0;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      Type type = Type.GetTypeFromCLSID(clsid);
      IBaseFilter filter = (IBaseFilter) Activator.CreateInstance(type);

      hr = graphBuilder.AddFilter(filter, name);
      DsError.ThrowExceptionForHR(hr);

      return filter;
    }

    public static IBaseFilter AddFilterByName(IGraphBuilder graphBuilder, Guid deviceCategory, string friendlyName)
    {
      int hr = 0;
      IBaseFilter filter = null;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      DsDevice[] devices = DsDevice.GetDevicesOfCat(deviceCategory);
      foreach (DsDevice device in devices)
      {
        if (!device.Name.Equals(friendlyName))
          continue;

        hr = (graphBuilder as IFilterGraph2).AddSourceFilterForMoniker(device.Mon, null, friendlyName, out filter);
        DsError.ThrowExceptionForHR(hr);

        break;
      }

      return filter;
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static IBaseFilter AddFilterByDevicePath(IGraphBuilder graphBuilder, string devicePath, string name)
    {
      int hr = 0;
      IBaseFilter filter;
      UCOMIBindCtx bindCtx;
      UCOMIMoniker moniker;
      int eaten;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      hr = NativeMethods.CreateBindCtx(0, out bindCtx);
      Marshal.ThrowExceptionForHR(hr);

      hr = NativeMethods.MkParseDisplayName(bindCtx, devicePath, out eaten, out moniker);
      Marshal.ThrowExceptionForHR(hr);

      hr = (graphBuilder as IFilterGraph2).AddSourceFilterForMoniker(moniker, bindCtx, name, out filter);
      DsError.ThrowExceptionForHR(hr);

      Marshal.ReleaseComObject(bindCtx);
      Marshal.ReleaseComObject(moniker);

      return filter;
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static IBaseFilter FindFilterByName(IGraphBuilder graphBuilder, string filterName)
    {
      int hr = 0;
      IBaseFilter filter = null;
      IEnumFilters enumFilters = null;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      hr = graphBuilder.EnumFilters(out enumFilters);
      if (hr == 0)
      {
        IBaseFilter[] filters = new IBaseFilter[1];
        int fetched;

        while(enumFilters.Next(filters.Length, filters, out fetched) == 0)
        {
          FilterInfo filterInfo;
          hr = filters[0].QueryFilterInfo(out filterInfo);

          if (filterInfo.achName.Equals(filterName))
          {
            filter = filters[0];
            break;
          }

          Marshal.ReleaseComObject(filters[0]);
        }
      }

      if (enumFilters != null)
        Marshal.ReleaseComObject(enumFilters);

      return filter;
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static IBaseFilter FindFilterByClsid(IGraphBuilder graphBuilder, Guid filterClsid)
    {
      int hr = 0;
      IBaseFilter filter = null;
      IEnumFilters enumFilters = null;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      hr = graphBuilder.EnumFilters(out enumFilters);
      if (hr == 0)
      {
        IBaseFilter[] filters = new IBaseFilter[1];
        int fetched;

        while(enumFilters.Next(filters.Length, filters, out fetched) == 0)
        {
          Guid clsid = Guid.Empty;

          hr = filters[0].GetClassID(out clsid);

          if (clsid == filterClsid)
          {
            filter = filters[0];
            break;
          }

          Marshal.ReleaseComObject(filters[0]);
        }
      }

      if (enumFilters != null)
        Marshal.ReleaseComObject(enumFilters);

      return filter;
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static void RenderPin(IGraphBuilder graphBuilder, IBaseFilter source, string pinName)
    {
      int hr = 0;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      IPin pin = DsFindPin.ByName(source, pinName);

      if (pin != null)
      {
        hr = graphBuilder.Render(pin);
        DsError.ThrowExceptionForHR(hr);

        Marshal.ReleaseComObject(pin);
      }
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static void DisconnectAllPins(IGraphBuilder graphBuilder)
    {
      int hr = 0;
      IEnumFilters enumFilters;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      hr = graphBuilder.EnumFilters(out enumFilters);
      DsError.ThrowExceptionForHR(hr);

      IBaseFilter[] filters = new IBaseFilter[1];
      int fetched;

      while(enumFilters.Next(filters.Length, filters, out fetched) == 0)
      {
        IEnumPins enumPins;
        IPin[] pins = new IPin[1];

        hr = filters[0].EnumPins(out enumPins);
        DsError.ThrowExceptionForHR(hr);

        while(enumPins.Next(pins.Length, pins, out fetched) == 0)
        {
          hr = pins[0].Disconnect();
          Marshal.ReleaseComObject(pins[0]);
        }

        Marshal.ReleaseComObject(enumPins);
        Marshal.ReleaseComObject(filters[0]);
      }

      Marshal.ReleaseComObject(enumFilters);
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static void RemoveAllFilters(IGraphBuilder graphBuilder)
    {
      int hr = 0;
      IEnumFilters enumFilters;
      ArrayList filtersArray = new ArrayList();

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      hr = graphBuilder.EnumFilters(out enumFilters);
      DsError.ThrowExceptionForHR(hr);

      IBaseFilter[] filters = new IBaseFilter[1];
      int fetched;

      while(enumFilters.Next(filters.Length, filters, out fetched) == 0)
      {
        filtersArray.Add(filters[0]);
      }

      foreach(IBaseFilter filter in filtersArray)
      {
        hr = graphBuilder.RemoveFilter(filter);
        Marshal.ReleaseComObject(filter);
      }
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static void SaveGraphFile(IGraphBuilder graphBuilder, string fileName)
    {
      int hr = 0;
      IStorage storage = null;
      UCOMIStream stream = null;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      try
      {
        hr = NativeMethods.StgCreateDocfile(
          fileName, 
          STGM.Create | STGM.Transacted | STGM.ReadWrite | STGM.ShareExclusive,
          0,
          out storage
          );

        Marshal.ThrowExceptionForHR(hr);

        hr = storage.CreateStream(
          @"ActiveMovieGraph",
          STGM.Write | STGM.Create | STGM.ShareExclusive,
          0,
          0,
          out stream
          );

        Marshal.ThrowExceptionForHR(hr);

        hr = (graphBuilder as IPersistStream).Save(stream, true);
        if (hr >= 0)
        {
          hr = storage.Commit(STGC.Default);
        }
      }
      finally
      {
        if (stream != null)
          Marshal.ReleaseComObject(stream);
        if (storage != null)
          Marshal.ReleaseComObject(storage);
      }
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static void LoadGraphFile(IGraphBuilder graphBuilder,string fileName)
    {
      int hr = 0;
      IStorage storage = null;
      UCOMIStream stream = null;

      if (graphBuilder == null)
        throw new ArgumentNullException("graphBuilder");

      try
      {
        if (NativeMethods.StgIsStorageFile(fileName) != 0)
          return;

        hr = NativeMethods.StgOpenStorage(
          fileName,
          null,
          STGM.Transacted | STGM.Read | STGM.ShareDenyWrite,
          IntPtr.Zero,
          0,
          out storage
          );

        Marshal.ThrowExceptionForHR(hr);

        hr = storage.OpenStream(
          @"ActiveMovieGraph",
          IntPtr.Zero,
          STGM.Read | STGM.ShareExclusive,
          0,
          out stream
          );

        if (hr >=0)
        {
          hr = (graphBuilder as IPersistStream).Load(stream);
        }
      }
      finally
      {
        if (stream != null)
          Marshal.ReleaseComObject(stream);
        if (storage != null)
          Marshal.ReleaseComObject(storage);
      }
    }

    public static bool SupportsPropertyPage(IBaseFilter filter)
    {
      return ((filter as ISpecifyPropertyPages) != null);
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static void ShowFilterPropertyPage(IBaseFilter filter, IntPtr parent)
    {
      int hr = 0;
      FilterInfo filterInfo;
      DsCAUUID caGuid;
      object[] objs;

      if (filter == null)
        throw new ArgumentNullException("filter");

      if (SupportsPropertyPage(filter))
      {
        hr = filter.QueryFilterInfo(out filterInfo);
        DsError.ThrowExceptionForHR(hr);

        hr = (filter as ISpecifyPropertyPages).GetPages(out caGuid);
        DsError.ThrowExceptionForHR(hr);

        objs = new object[1];
        objs[0] = filter;

        NativeMethods.OleCreatePropertyFrame(
          parent, 0, 0, 
          filterInfo.achName, 
          objs.Length, objs, 
          caGuid.cElems, caGuid.pElems, 
          0, 0, 
          IntPtr.Zero
          );

        Marshal.FreeCoTaskMem(caGuid.pElems);
        if (filterInfo.pGraph != null)
          Marshal.ReleaseComObject(filterInfo.pGraph);
      }
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static bool CheckIfThisComComponentIsInstalled(Guid clsid)
    {
      bool retval = false;

      try
      {
        Type type = Type.GetTypeFromCLSID(clsid);
        object o = Activator.CreateInstance(type);
        retval = true;
        Marshal.ReleaseComObject(o);
      }
      catch{}

      return retval;
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static bool IsVMR9Present()
    {
      return CheckIfThisComComponentIsInstalled(typeof(VideoMixingRenderer9).GUID);
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
    public static bool IsVMR7Present()
    {
      return CheckIfThisComComponentIsInstalled(typeof(VideoMixingRenderer).GUID);
    }

	}

  [Flags]
  internal enum STGM
  {
    Read = 0x00000000,
    Write = 0x00000001,
    ReadWrite = 0x00000002,
    ShareDenyNone = 0x00000040,
    ShareDenyRead = 0x00000030,
    ShareDenyWrite = 0x00000020,
    ShareExclusive = 0x00000010,
    Priority = 0x00040000,
    Create = 0x00001000,
    Convert = 0x00020000,
    FailIfThere = 0x00000000,
    Direct = 0x00000000,
    Transacted = 0x00010000,
    NoScratch = 0x00100000,
    NoSnapShot = 0x00200000,
    Simple = 0x08000000,
    DirectSWMR = 0x00400000,
    DeleteOnRelease = 0x04000000,
  }

  [Flags]
  internal enum STGC
  {
    Default        = 0,
    Overwrite      = 1,
    OnlyIfCurrent  = 2,
    DangerouslyCommitMerelyToDiskCache = 4,
    Consolidate    = 8
  }

  [Guid("0000000b-0000-0000-C000-000000000046"),
  InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  internal interface IStorage
  {
    [PreserveSig]
    int CreateStream(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] STGM grfMode,
      [In] int reserved1,
      [In] int reserved2,
      [Out] out UCOMIStream ppstm
      );

    [PreserveSig]
    int OpenStream(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] IntPtr reserved1,
      [In] STGM grfMode,
      [In] int reserved2,
      [Out] out UCOMIStream ppstm
      );

    [PreserveSig]
    int CreateStorage(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] STGM grfMode,
      [In] int reserved1,
      [In] int reserved2,
      [Out] out IStorage ppstg
      );

    [PreserveSig]
    int OpenStorage(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] IStorage pstgPriority,
      [In] STGM grfMode,
      [In] int snbExclude,
      [In] int reserved,
      [Out] out IStorage ppstg
      );

    [PreserveSig]
    int CopyTo(
      [In] int ciidExclude,
      [In] Guid[] rgiidExclude,
      [In] string[] snbExclude,
      [In] IStorage pstgDest
      );

    [PreserveSig]
    int MoveElementTo(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] IStorage pstgDest,
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName,
      [In] STGM grfFlags
      );

    [PreserveSig]
    int Commit([In] STGC grfCommitFlags);

    [PreserveSig]
    int Revert();

    [PreserveSig]
    int EnumElements(
      [In] int reserved1, 
      [In] IntPtr reserved2, 
      [In] int reserved3, 
      [Out, MarshalAs(UnmanagedType.Interface)] out object ppenum
      );

    [PreserveSig]
    int DestroyElement([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

    [PreserveSig]
    int RenameElement(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsOldName, 
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName
      );

    [PreserveSig]
    int SetElementTimes(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName, 
      [In] FILETIME pctime, 
      [In] FILETIME patime, 
      [In] FILETIME pmtime
      );

    [PreserveSig]
    int SetClass([In, MarshalAs(UnmanagedType.LPStruct)] Guid clsid);

    [PreserveSig]
    int SetStateBits(
      [In] int grfStateBits, 
      [In] int grfMask
      );

    [PreserveSig]
    int Stat(
      [Out] out STATSTG pStatStg, 
      [In] int grfStatFlag
      );
  }

  internal sealed class NativeMethods
  {
    private NativeMethods(){}

    [DllImport("ole32.dll")]
    public static extern int CreateBindCtx(int reserved, out UCOMIBindCtx ppbc);

    [DllImport("ole32.dll")]
    public static extern int MkParseDisplayName(UCOMIBindCtx pcb, [MarshalAs(UnmanagedType.LPWStr)] string szUserName, out int pchEaten, out UCOMIMoniker ppmk);

    [DllImport("olepro32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
    public static extern int OleCreatePropertyFrame(
      [In] IntPtr hwndOwner, 
      [In] int x, 
      [In] int y,
      [In, MarshalAs(UnmanagedType.LPWStr)] string lpszCaption, 
      [In] int cObjects,
      [In, MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.IUnknown)] object[] ppUnk,
      [In] int cPages,	
      [In] IntPtr pPageClsID, 
      [In] int lcid, 
      [In] int dwReserved, 
      [In] IntPtr pvReserved 
      );

    [DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
    public static extern int StgCreateDocfile(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] STGM grfMode,
      [In] int reserved,
      [Out] out IStorage ppstgOpen
      );

    [DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
    public static extern int StgIsStorageFile([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

    [DllImport("ole32.dll", CharSet=CharSet.Unicode, ExactSpelling=true)]
    public static extern int StgOpenStorage(
      [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
      [In] IStorage pstgPriority,
      [In] STGM grfMode,
      [In] IntPtr snbExclude,
      [In] int reserved,
      [Out] out IStorage ppstgOpen
      );

  }

}
