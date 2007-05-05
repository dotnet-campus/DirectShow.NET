// The only filter I know that supports this interface returns E_NOTIMPL for all methods

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_0
{
    class ICCSubStreamFilteringTest
    {
        ICCSubStreamFiltering m_cc;

        public void DoTests()
        {
            int hr;
            CCSubstreamService p;

            Configure();

            hr = m_cc.put_SubstreamTypes(CCSubstreamService.CC3);
            DsError.ThrowExceptionForHR(hr);

            hr = m_cc.get_SubstreamTypes(out p);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(p == CCSubstreamService.CC3, "SubstreamTypes");
        }

        private void Configure()
        {
            IBaseFilter ibf;

            // VBI Codec
            Guid g = new Guid("{370a1d5d-ddeb-418c-81cd-189e0d4fa443}");
            Type type = Type.GetTypeFromCLSID(g);
            ibf = Activator.CreateInstance(type) as IBaseFilter;

            IPin iPin = DsFindPin.ByDirection(ibf, PinDirection.Output, 3);
            m_cc = iPin as ICCSubStreamFiltering;
        }
    }
}

