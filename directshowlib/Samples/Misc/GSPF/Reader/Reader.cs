/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;

using DirectShowLib;

namespace rdr
{
    #region Local Interfaces

    [Guid("A6AD4353-4690-438e-AAB6-3F345C221D6F")]
    public interface ISetFileName
    {
        [PreserveSig]
        int SetFileName(string s);

        [PreserveSig]
        int GetFileName(out string s);
    }

    #endregion


	[ComVisible(true), 
    Guid("6F7837E1-AAB1-4286-8238-D979F85CF3A7"), 
    ClassInterface(ClassInterfaceType.None)]
	public class AsyncRdr : IAsyncReader, ISetFileName
	{
        #region Defines

		private const long UNIT = 10000000;

        private const int S_Ok = 0;
        private const int S_False = 1;
        private const int E_NotImpl = unchecked((int)0x80004001);
        private const int E_Fail = unchecked((int) 0x80004005);
        private const int E_BufferTooSmall = unchecked ((int)0x8007007a);

        #endregion

        #region Members

        private volatile bool m_GraphIsFlushing;
        private int m_ThreadsWaiting;
        private Queue m_Requests;
        private string m_sFileName;
        private FileStream m_fs;
        private BinaryReader m_br;
        private IMemAllocator m_MemAlloc;
        private AutoResetEvent m_Wait;

        #endregion

		public AsyncRdr()
		{
            m_Wait = new AutoResetEvent(false);

            m_GraphIsFlushing = false;

            // Just used in the destructor to wait for everything
            // to exit.
            m_ThreadsWaiting = 0;

            // Make space for caching requests.  This holds results of calls
            // made to Request that haven't yet been retrieve by a matching
            // call to WaitForNext.  I haven't seen it get higher than 1,
            // but I suppose it could.  Queue is supposed to auto-grow if
            // needed.
            m_Requests = Queue.Synchronized(new Queue(5));
		}

        ~AsyncRdr()
        {
            // Make sure any waiting threads get released
            int iRetries = 0;

            BeginFlush();
            while ( (m_ThreadsWaiting > 0) && (iRetries < 10) )
            {
                System.Threading.Thread.Sleep(1);
                iRetries++;
            }

            Debug.Assert(m_ThreadsWaiting == 0, "Threads still active");

            // Release the m_MemAlloc.  No, we never actually used
            // it, but bad things happen if we don't hang on to it.
            if (m_MemAlloc != null)
            {
                Marshal.ReleaseComObject(m_MemAlloc);
                m_MemAlloc = null;
            }

            // Close the file
            if (m_br != null)
            {
                m_br.Close();
                m_br = null;
                m_fs = null;
            }

            m_Wait.Close();

            m_Wait = null;
            m_Requests = null;
            m_sFileName = null;
        }


		public int RequestAllocator(IMemAllocator pPreferred, AllocatorProperties pProps, out IMemAllocator ppActual)
		{
            int hr;

            Debug.WriteLine(string.Format("RequestAllocator {0} Align: {1} Buffer: {2} #: {3}", 
                pPreferred != null, pProps.cbAlign, pProps.cbBuffer, pProps.cBuffers));
            Debug.Assert(pPreferred != null, "No allocator provided");

            // Need to set it to something.  Let's hope for the best.
            ppActual = pPreferred;

            lock(this) // Protect the m_ variables
            {
                // Release any previous allocator
                if ( (m_MemAlloc != null) && (m_MemAlloc != pPreferred) )
                {
                    Marshal.ReleaseComObject(m_MemAlloc);
                    m_MemAlloc = null;
                }

                if (pPreferred != null)
                {
                    m_MemAlloc = pPreferred;
                    hr = S_Ok;
                }
                else
                {
                    // Todo - In theory, we should provide our own allocator if one isn't offered.
                    // In practice, no one accepts an allocator if I offer one.
                    hr = E_Fail;
                }
            }

            return hr;
		}

		public int Request(IMediaSample pSample, IntPtr dwUser)
		{
            int hr;

            Debug.Write(string.Format("Request {0} user: {1}", pSample != null, dwUser));

            MediaHolder mh = new MediaHolder(pSample, dwUser);

            lock(this) // Protect the m_ variables
            {
                if (!m_GraphIsFlushing)
                {
                    // Now that we have populated everything, wait for the splitter to call the
                    // WaitForNext() method to retrieve the data
                    m_Requests.Enqueue(mh);

                    // Let the waiting thread in WaitForNext know something is ready for
                    // processing
                    m_Wait.Set();

                    hr = S_Ok;
                }
                else
                {
                    hr = DsResults.E_WrongState;
                }
            }

            Debug.WriteLine(hr.ToString());

            return hr;
		}

		public int WaitForNext(int dwTimeout, out IMediaSample ppSample, out IntPtr pdwUser)
		{
            int hr;

            Debug.WriteLine(string.Format("WaitForNext {0}", dwTimeout));

            // In case something goes wrong
            ppSample = null;
            pdwUser = IntPtr.Zero;

            if (!m_GraphIsFlushing)
            {
                // Count threads we have waiting
                Interlocked.Increment(ref m_ThreadsWaiting);

                bool bWait = m_Wait.WaitOne(dwTimeout, false);

                lock(this)
                {
                    // If we found one before timing out, send it back
                    if (bWait && !m_GraphIsFlushing)
                    {
                        MediaHolder mh = m_Requests.Dequeue() as MediaHolder;
                        ppSample = mh.Sample;
                        pdwUser = mh.User;

                        hr = PopulateMediaSample(ppSample);
                    }
                    else
                    {
                        hr = DsResults.E_Timeout;
                    }

                    // If there is another request, reset the event.  Also
                    // if the graph is flushing, allow the other threads
                    // to exit
                    if ( (m_Requests.Count > 0) || (m_GraphIsFlushing) )
                    {
                        m_Wait.Set();
                    }
                }

                // Count threads we have waiting
                Interlocked.Decrement(ref m_ThreadsWaiting);
            }
            else
            {
                hr = DsResults.E_WrongState;
            }

            return hr;
		}

		public int SyncReadAligned(IMediaSample pSample)
		{
            int hr;

            Debug.Write("SyncReadAligned");

            if (!m_GraphIsFlushing)
            {
                hr = PopulateMediaSample(pSample);
            }
            else
            {
                hr = DsResults.E_WrongState;
            }

            Marshal.ReleaseComObject(pSample);

            Debug.WriteLine(string.Format("={0}", hr));

            return hr;
		}

		public int SyncRead(long llPosition, int lLength, IntPtr pBuffer)
		{
            Debug.Write(string.Format("SyncRead pos:{0} len: {1} ", llPosition, lLength));

            int hr;
            int iBytesRead;

            hr = SyncReadInternal(llPosition, lLength, pBuffer, out iBytesRead);

            Debug.WriteLine(string.Format(" Read: {0}={1}", iBytesRead, hr));

            return hr;
		}

		public int Length(out long pTotal, out long pAvailable)
		{
            long lSize = m_fs.Length;

			Debug.WriteLine(string.Format("Length {0}", lSize));

            pTotal = lSize;
			pAvailable = lSize;

			return 0;
		}

		public int BeginFlush()
		{
			Debug.WriteLine("BeginFlush");

            lock(this)
            {
                // Set this to true to let any threads that are blocked in WaitForNext() 
                // to exit.
                m_Wait.Set();

                // 
                m_GraphIsFlushing = true;
                m_Requests.Clear();
            }

			return 0;
		}

		public int EndFlush()
		{
			Debug.WriteLine("EndFlush");

            lock(this)
            {
                m_Wait.Reset();
                m_GraphIsFlushing = false;
            }

			return 0;
        }


        private int SyncReadInternal(long llPosition, int lLength, IntPtr pBuffer, out int iBytesRead)
        {
            int hr;

            // No file name set
            if (m_br != null)
            {
                Byte [] b = new byte[lLength];

                lock(this)
                {
                    // Move to the requested point in the file
                    m_fs.Position = llPosition;

                    // Read the specified number of bytes
                    iBytesRead = m_br.Read(b, 0, lLength);
                }

                // Copy the read data into the buffer.  Too bad BinaryReader doesn't have an overload
                // that takes an IntPtr
                Marshal.Copy(b, 0, pBuffer, lLength);

                // A simpleminded check for reading past the eof
                if (lLength + llPosition <= m_fs.Length)
                {
                    hr = S_Ok;
                }
                else
                {
                    hr = S_False;
                }
            }
            else
            {
                iBytesRead = 0;
                hr = E_Fail;
            }

            return hr;
        }

        private int PopulateMediaSample(IMediaSample pSample)
        {
            int hr;
            long tStart, tStop;

            // Read the file offsets they are seeking
            hr = pSample.GetTime(out tStart, out tStop);
            if(hr >= 0)
            {
                IntPtr pBuffer = IntPtr.Zero;

                // Turn the sample time into a file position and length
                long llPos = tStart / UNIT;
                int lLength = (int) ((tStop - tStart) / UNIT);
                int iBufferSize = pSample.GetSize();

                Debug.Write(string.Format(" pos: {0} len: {1} size: {2}", llPos, lLength, iBufferSize));

                // Make sure the buffer they passed it big enough to hold
                // all the data they requested
                if (iBufferSize >= lLength)
                {
                    // This is where we store the data
                    hr = pSample.GetPointer(out pBuffer);
                }
                else
                {
                    hr = E_BufferTooSmall;
                }
 
                if (hr >= 0)
                {
                    int iBytesRead;

                    // Read the data into the buffer
                    hr = SyncReadInternal(llPos, lLength, pBuffer, out iBytesRead);
                    if (hr >= 0)
                    {
                        // How much of the buffer we used.
                        pSample.SetActualDataLength(iBytesRead);
                        Debug.Write(string.Format(" read: {0}", iBytesRead));
                    }
                }
            }

            return hr;
        }


        #region ISetFileName Members

        public int SetFileName(string s)
        {
            int hr = S_Ok;

            lock(this)
            {
                // Close any previous file
                if (m_br != null)
                {
                    m_br.Close();
                    m_br = null;
                    m_fs = null;
                    m_sFileName = null;
                }

                // Open a new file
                try
                {
                    m_fs = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.Read);
                    m_br = new BinaryReader(m_fs);
                    m_sFileName = s;
                }
                catch
                {
                    m_fs = null;
                    m_br = null;
                    m_sFileName = null;
                    hr = E_Fail;
                }
            }

            return hr;
        }

        public int GetFileName(out string s)
        {
            s = m_sFileName;
            return 0;
        }


        #endregion
    }


    // Wrapper to hold the details of a request
	internal class MediaHolder
	{
		private IMediaSample m_Sample;
		private IntPtr m_User;

		public MediaHolder(IMediaSample pSample, IntPtr dwUser)
		{
			m_Sample = pSample;
			m_User = dwUser;
		}

		public IMediaSample Sample
		{
			get
			{
				return m_Sample;
			}
		}

		public IntPtr User
		{
			get
			{
				return m_User;
			}
        }
    }
}
