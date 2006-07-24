using System;
using System.Diagnostics;
using System.Text;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v1._5
{
  class IBDAComparableTest
  {
    private IBDAComparable obj1, obj2, obj3;

    public void DoTests()
    {
      Setup();

      TestHashEquivalent();
      TestCompareEquivalent();

      TestHashExact();
      TestCompareExact();
    }

    private void TestHashEquivalent()
    {
      int hr = 0;
      long result1, result2;

      // obj1 and obj2 are the same -> same results

      hr = obj1.HashEquivalent(BDACompFlags.ExcludeTSFromTR, out result1);
      DsError.ThrowExceptionForHR(hr);

      hr = obj2.HashEquivalent(BDACompFlags.ExcludeTSFromTR, out result2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(result1 == result2, "IBDAComparable.HashEquivalent");

      hr = obj1.HashEquivalentIncremental(0x12345678, BDACompFlags.IncludeLocatorInTR, out result1);
      DsError.ThrowExceptionForHR(hr);

      hr = obj2.HashEquivalentIncremental(0x12345678, BDACompFlags.IncludeLocatorInTR, out result2);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(result1 == result2, "IBDAComparable.HashEquivalentIncremental");
    }

    private void TestCompareEquivalent()
    {
      int hr = 0;
      int result = 0;

      hr = obj1.CompareEquivalent(obj2, BDACompFlags.NotDefined, out result);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(result == 0, "IBDAComparable.CompareEquivalent");

      (obj2 as IDVBTuneRequest).put_ONID(1234);
      // obj2 is now different from obj1

      hr = obj1.CompareEquivalent(obj2, BDACompFlags.NotDefined, out result);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(result != 0, "IBDAComparable.CompareEquivalent");
    }

    private void TestHashExact()
    {
      int hr = 0;
      long result1;

      hr = obj1.HashExact(out result1);
      //DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == unchecked((int)0x80004001), "IBDAComparable.HashExact"); // Not implemented with DVB & ATSC TuneRequests

      hr = obj1.HashExactIncremental(0x87654321, out result1);
      //DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == unchecked((int)0x80004001), "IBDAComparable.HashExactIncremental"); // Not implemented with DVB & ATSC TuneRequests
    }

    private void TestCompareExact()
    {
      int hr = 0;
      int result = 0;

      hr = obj1.CompareExact(obj3, out result);
      //DsError.ThrowExceptionForHR(hr);

      Debug.Assert(hr == unchecked((int)0x80004001), "IBDAComparable.CompareExact"); // Not implemented with DVB & ATSC TuneRequests
    }


    private void Setup()
    {
      obj1 = (IBDAComparable)new DVBTuneRequest();
      obj2 = (IBDAComparable)new DVBTuneRequest();
      obj3 = (IBDAComparable)new DVBTuneRequest();
    }
  }
}
