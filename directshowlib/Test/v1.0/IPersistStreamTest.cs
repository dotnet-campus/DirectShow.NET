// Removed IStream and replaced it with UCOMIStream
// Save->UnmanagedType.Bool
// GetMaxSize->out

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using DirectShowLib.Dvd;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DirectShowLib.Test
{
  [TestFixture]
  public class IPersistStreamTest
  {
    // The drive containing testme.iso
    const string MyDisk = @"f:\video_ts";

    IDvdInfo2 m_idi2 = null;
    IDvdControl2 m_idc2 = null;
    DsROTEntry m_ROT = null;
    IMediaControl m_imc = null; // don't release
    IPersistStream m_ips = null;

    [DllImport("OLE32.DLL")]
    extern private static int CreateStreamOnHGlobal( 
      IntPtr hGlobalMemHandle, 
      bool fDeleteOnRelease, 
      out UCOMIStream pOutStm);

    public IPersistStreamTest()
    {
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
        AllowPlay();

        TestGetClassID();
        TestIsDirty();
        TestGetSizeMax();
        TestSaveLoad();
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
      IDvdState dss;

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

      hr = m_idi2.GetState(out dss);
      DsError.ThrowExceptionForHR(hr);

      m_ips = dss as IPersistStream;
    }


    void TestGetClassID()
    {
      int hr;
      Guid Clsid;

      hr = m_ips.GetClassID(out Clsid);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(Clsid == typeof(DVDState).GUID, "GetClassID");
    }

    void TestIsDirty()
    {
      int hr;

      hr = m_ips.IsDirty();

      // zero means it hasn't been saved (dirty)
      Debug.Assert(hr == 0, "IsDirty");
    }

    void TestSaveLoad()
    {
      int hr;
      UCOMIStream uis = null;
      long siz;
      hr = m_ips.GetSizeMax(out siz);

      // Create the stream to write to
      //hr = CreateStreamOnHGlobal(IntPtr.Zero, true, out uis);

      uis = new MyNullStream(siz);

      // false doesn't seem to work
      hr = m_ips.Save(uis, true);
      DsError.ThrowExceptionForHR(hr);

      // See if the dirty bit got cleared
      hr = m_ips.IsDirty();
      Debug.Assert(hr == 1, "dirty3");

      STATSTG p;
      uis.Stat(out p, 0);

      // Make sure something got written
      Debug.Assert(p.cbSize > 0, "Save");

      // Read it back
      uis.Seek(0, 0, IntPtr.Zero);

      hr = m_ips.Load(uis);
      DsError.ThrowExceptionForHR(hr);
    }

    void TestGetSizeMax()
    {
      int hr;
      long siz;

      hr = m_ips.GetSizeMax(out siz);
      DsError.ThrowExceptionForHR(hr);

      // The max size of a DvdState
      Debug.Assert(siz == 1036, "GetSizeMax");
    }

    // Not used - But there are things here that may be useful some day
#if false
        void TestSave2()
        {
            int hr;
            UCOMIStream uis = null;
            long siz;
            hr = m_ips.GetSizeMax(out siz);
            myIStorage iStore = null;

            hr = StgCreateDocfile(@"C:\foo.out", 
                //STGM.DIRECT|STGM.CREATE|STGM.READWRITE|STGM.SHARE_EXCLUSIVE,
                STGM.CREATE|STGM.WRITE|STGM.SHARE_EXCLUSIVE,
                0, 
                ref iStore);
            DsError.ThrowExceptionForHR(hr);
            uis = iStore as UCOMIStream;

            //hr = iStore.CreateStream("asdf", STGM.DIRECT|STGM.CREATE|STGM.READWRITE|STGM.SHARE_EXCLUSIVE, 0, 0, out uis);
            hr = iStore.CreateStream("asdf", STGM.CREATE|STGM.WRITE|STGM.SHARE_EXCLUSIVE, 0, 0, out uis);

            hr = OleSaveToStream(m_ips, uis);

            hr = StgCreateStorageEx(@"c:\foo.out", 
                (STGM)0,
                (STGFMT)0, 
                0, 
                IntPtr.Zero, 
                IntPtr.Zero, 
                typeof(myIStorage).GUID, 
                ref iStore);
            DsError.ThrowExceptionForHR(hr);

            string s = typeof(UCOMIStream).GUID.ToString();

            // Create the stream (with no initial memory allocated)
            IntPtr ip = Marshal.AllocCoTaskMem((int)siz);
            ip = GlobalAlloc(0, (int)siz);
            hr = CreateStreamOnHGlobal(ip, false, out uis);
            //uis.SetSize(siz * 2);

            byte[] b = new byte[3];
            b[0] = 65;
            b[1] = 66;
            b[2] = 67;

            uis.Write(b, b.Length, IntPtr.Zero);

            hr = m_ips.Save(uis, true);
            DsError.ThrowExceptionForHR(hr);
        }

        enum STGM
        {
            READ = 0x00000000, 
            WRITE = 0x00000001, 
            READWRITE = 0x00000002, 
            SHARE_DENY_NONE = 0x00000040, 
            SHARE_DENY_READ = 0x00000030, 
            SHARE_DENY_WRITE = 0x00000020, 
            SHARE_EXCLUSIVE = 0x00000010, 
            PRIORITY = 0x00040000, 
            CREATE = 0x00001000, 
            CONVERT = 0x00020000, 
            FAILIFTHERE = 0x00000000, 
            DIRECT = 0x00000000, 
            TRANSACTED = 0x00010000, 
            NOSCRATCH = 0x00100000, 
            NOSNAPSHOT = 0x00200000, 
            SIMPLE = 0x08000000, 
            DIRECT_SWMR = 0x00400000, 
            DELETEONRELEASE = 0x04000000
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
            Guid("0000000b-0000-0000-C000-000000000046")]
            interface myIStorage
        {
            int CreateStream(
                [MarshalAs(UnmanagedType.LPWStr)] string wcsName,
                STGM grfMode, //Access mode for the new stream
                int reserved1, //Reserved; must be zero
                int reserved2, //Reserved; must be zero
                out UCOMIStream stream //Pointer to output variable
                );

            int OpenStream(
                [MarshalAs(UnmanagedType.LPWStr)] string wcsName,
                IntPtr reserved1, //Reserved; must be NULL
                int grfMode, //Access mode for the new stream
                int reserved2, //Reserved; must be zero
                out UCOMIStream stream //Pointer to output variable
                );
        }

        [DllImport("Kernel32.DLL")]
        extern public static IntPtr GlobalAlloc(
            int uFlags,
            int dwBytes
            );


        [DllImport("ole32.dll", CharSet=CharSet.Unicode)]
        private static extern int StgCreateStorageEx(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            STGM accessMode, 
            STGFMT storageFileFormat, 
            int fileBuffering,
            IntPtr options, 
            IntPtr reserved, 
            [In, MarshalAs(UnmanagedType.LPStruct)] System.Guid riid,
            [In, Out, MarshalAs(UnmanagedType.Interface)] ref myIStorage propertySetStorage);

        [DllImport("ole32.dll", CharSet=CharSet.Unicode)]
        private static extern int StgCreateDocfile(
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            STGM grfMode,
                int reserved,
            [In, Out, MarshalAs(UnmanagedType.Interface)] ref myIStorage propertySetStorage
        );

        [DllImport("ole32.dll", CharSet=CharSet.Unicode)]
        private static extern int OleSaveToStream(
            IPersistStream pPStm,  //Pointer to the interface on the object 
            // to be saved
            UCOMIStream pStm           //Pointer to the destination stream to 
            // which the object is saved
            );

        enum STGFMT
        {
            STORAGE = 0, 
            FILE = 3, 
            ANY = 4, 
            DOCFILE = 5
        }

        void foo(UCOMIStream iStream)
        {
            // Load the Encrypted file into a stream.
            FileStream fsIn = new FileStream("C:\\test.pdf", FileMode.Open, FileAccess.Read);

            // Create a MemoryStream to hold the decrypted data.
            MemoryStream ms = new MemoryStream();	

            // Create a reader for the data.
            BinaryReader r = new BinaryReader(ms);

            // Get length of the file.
            int fileLen = Convert.ToInt32(ms.Length);
			
            // Create a buffer for the data.
            byte[] fileBytes = new byte[fileLen];

            // Read the data from Memory
            for (int i =0; i< fileLen; i++)
            {
                fileBytes[i] = r.ReadByte();
            }
			
            // declare the COM stream
            //UCOMIStream iStream;

            // Write the data from buffer into COM Stream
            iStream.Write(fileBytes, fileLen, System.IntPtr.Zero);
			
            // Set size of COM stream
            iStream.SetSize(fileLen);

        }
#endif
        
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

      return idgb;
    }
  }

  [ComVisible(true)]
  public class MyNullStream : UCOMIStream
  {
    MemoryStream memStream;
    BinaryWriter writer;
    int writeIndex = 0;
    BinaryReader reader;
    int readIndex = 0;

    public MyNullStream(long size)
    {
      memStream = new MemoryStream((int)size);
      writer = new BinaryWriter(memStream);
      reader = new BinaryReader(memStream);
    }

    ~MyNullStream()
    {
      memStream.Flush();
      memStream.Close();
    }

    #region Membres de UCOMIStream

    public void Commit(int grfCommitFlags)
    {
      Debug.WriteLine("in Commit");
      // TODO : ajoutez l'implémentation de Stream.Commit
    }

    public void Clone(out UCOMIStream ppstm)
    {
      Debug.WriteLine("in Clone");
      // TODO : ajoutez l'implémentation de Stream.Clone
      ppstm = null;
    }

    public void LockRegion(long libOffset, long cb, int dwLockType)
    {
      Debug.WriteLine("in LockRegion");
      // TODO : ajoutez l'implémentation de Stream.LockRegion
    }

    public void Seek(long dlibMove, int dwOrigin, System.IntPtr plibNewPosition)
    {
      // Ok it's cheating
      memStream.Seek(0, SeekOrigin.Begin);

      plibNewPosition = IntPtr.Zero;
      Marshal.ThrowExceptionForHR(0);
    }

    public void CopyTo(UCOMIStream pstm, long cb, System.IntPtr pcbRead, System.IntPtr pcbWritten)
    {
      Debug.WriteLine("in CopyTo");
      // TODO : ajoutez l'implémentation de Stream.CopyTo
    }

    public void Revert()
    {
      Debug.WriteLine("in Revert");
      // TODO : ajoutez l'implémentation de Stream.Revert
    }

    public void Write(byte[] pv, int cb, System.IntPtr pcbWritten)
    {
      try
      {
        writer.Write(pv, writeIndex, cb);
        writeIndex += cb;
      }
      catch
      {
      }

      if (pcbWritten != IntPtr.Zero)
        Marshal.WriteInt32(pcbWritten, cb);

      Marshal.ThrowExceptionForHR(0);
    }

    public void UnlockRegion(long libOffset, long cb, int dwLockType)
    {
      Debug.WriteLine("in UnlockRegion");
      // TODO : ajoutez l'implémentation de Stream.UnlockRegion
    }

    public void SetSize(long libNewSize)
    {
      Debug.WriteLine("in SetSize");
      // TODO : ajoutez l'implémentation de Stream.SetSize
    }

    public void Read(byte[] pv, int cb, System.IntPtr pcbRead)
    {
      try
      {
        reader.Read(pv, readIndex, cb);
        readIndex += cb;
      }
      catch
      {
      }

      if (pcbRead != IntPtr.Zero)
        Marshal.WriteInt32(pcbRead, cb);

      Marshal.ThrowExceptionForHR(0);
    }

    public void Stat(out STATSTG pstatstg, int grfStatFlag)
    {
      Debug.WriteLine("in Stat");
      pstatstg = new STATSTG();
      pstatstg.cbSize = writeIndex;
      Marshal.ThrowExceptionForHR(0);
    }

    #endregion
  }
}
