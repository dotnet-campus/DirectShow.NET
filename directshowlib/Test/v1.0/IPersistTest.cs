using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DirectShowLib.Test
{
	public class IPersistTest
	{
		public IPersistTest()
		{
		}
    
    public void DoTests()
    {
      IBaseFilter filter = null;

      try
      {
        Guid typeGuid;
        IPersist persist = null;
        Guid readGuid;

        // I decide to use the VMR9 filter because i love it !
        // This test could work with other filters with the notable exception of the 
        // VideoRendererDefault which is in fact a VMR7 on Windows XP...
        
        filter = (IBaseFilter) new VideoMixingRenderer9();
        typeGuid = (typeof(VideoMixingRenderer9).GUID);

        // All DirectShow filters must implement IBaseFilter...
        // which inherit from IMediaFilter...
        // which inherit from IPersist
        
        persist = (IPersist) filter;
        persist.GetClassID(out readGuid);

        Debug.Assert(readGuid == typeGuid, "IPersist.GetClassID");
      }
      finally
      {
        Marshal.ReleaseComObject(filter);
      }
    }
  }
}
