using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.SBE;

namespace v2_1
{
    public class ISBE2FileScanTest
    {
        public ISBE2FileScanTest()
        {
        }

        public void DoTests()
        {
            SBE2FileScan fs = new SBE2FileScan();
            ISBE2FileScan ifs = fs as ISBE2FileScan;

            int hr = ifs.RepairFile(@"C:\Users\Public\Recorded TV\Sample Media\win7_scenic-demoshort_raw.wtv");
        }
    }
}
