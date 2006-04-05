using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using DirectShowLib.BDA;
using DirectShowLib;

// This the test for the "new" version of IComponents

namespace DirectShowLib.Test
{
  public class IComponentsTest
  {
    private IComponentsNew m_comps;

    public IComponentsTest()
    {
    }

    public void DoTests()
    {
      Config();

      try
      {
        TestAdd();
        TestCount();
        TestGetPut();
        TestClone();
        TestNewEnum();
        TestEnum();
        TestRemove();
      }
      finally
      {
        Marshal.ReleaseComObject(m_comps);
      }
    }

    private void TestAdd()
    {
      int hr;
      object o;

      IComponent icomp = (IComponent)new Component();

      hr = m_comps.Add(icomp, out o);
      DsError.ThrowExceptionForHR(hr);
    }

    private void TestCount()
    {
      int hr;
      int i;

      hr = m_comps.get_Count(out i);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(i == 1, "get_Count");
    }

    private void TestGetPut()
    {
      int hr;
      IComponent ct;

      hr = m_comps.get_Item(0, out ct);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(ct != null, "get_Item");

      hr = m_comps.put_Item(0, ct);
      DsError.ThrowExceptionForHR(hr);
    }

    private void TestClone()
    {
      int hr;
      int i;
      IComponentsNew it;

      hr = m_comps.Clone(out it);
      DsError.ThrowExceptionForHR(hr);

      hr = it.get_Count(out i);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(i == 1, "clone");
    }

    private void TestNewEnum()
    {
      int hr;
      IEnumVARIANT pEnum;

      hr = m_comps.get__NewEnum(out pEnum);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(pEnum != null, "get__NewEnum");
    }

    private void TestEnum()
    {
      int hr;
      IEnumComponents pEnum;

      hr = m_comps.EnumComponents(out pEnum);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(pEnum != null, "EnumComponents");
    }

    private void TestRemove()
    {
      int hr;
      int i;

      hr = m_comps.Remove(0);
      DsError.ThrowExceptionForHR(hr);

      hr = m_comps.get_Count(out i);
      DsError.ThrowExceptionForHR(hr);

      Debug.Assert(i == 0, "Remove");
    }

    private void Config()
    {
      Components c = new Components();
      m_comps = (IComponentsNew) c;
    }
  }
}
