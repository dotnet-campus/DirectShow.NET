using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

using MSXML2;

namespace v2_1
{
    public class IPersistTuneXmlTest
    {
        /// This sample require the MSXML2 Com library
        private IDVBTuneRequest dvbtr = null;
        private IPersistTuneXml ptXml = null;

        public IPersistTuneXmlTest() { }

        public void DoTests()
        {
            int hr = 0;

            Config();

            object xmlNode;

            // Save the tune request
            hr = ptXml.Save(out xmlNode);
            DsError.ThrowExceptionForHR(hr);

            Debug.Assert(xmlNode is IXMLDOMNode);

            // Change the ONID in the saved string
            string conf = (xmlNode as IXMLDOMNode).xml;
            conf = conf.Replace("8442", "2448");

            // Load the modified setting
            hr = ptXml.Load(conf);
            DsError.ThrowExceptionForHR(hr);

            int onid;

            hr = dvbtr.get_ONID(out onid);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(onid == 2448);

            // As documented, this method doesn't seem to do anything
            hr = ptXml.InitNew();
            DsError.ThrowExceptionForHR(hr);
        }

        private void Config()
        {
            IBDACreateTuneRequestEx bdaCtrEx = new DVBTuningSpace() as IBDACreateTuneRequestEx;
            Debug.Assert(bdaCtrEx != null);

            ITuneRequest tr;

            int hr = bdaCtrEx.CreateTuneRequestEx(typeof(IDVBTuneRequest).GUID, out tr);
            DsError.ThrowExceptionForHR(hr);

            dvbtr = (IDVBTuneRequest)tr;

            // Tune request for France 2...
            dvbtr.put_ONID(8442);
            dvbtr.put_TSID(1);
            dvbtr.put_SID(259);

            ptXml = (IPersistTuneXml)tr;
        }

    }
}
