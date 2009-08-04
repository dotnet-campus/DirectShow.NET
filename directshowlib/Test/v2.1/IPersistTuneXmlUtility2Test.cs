using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DirectShowLib;
using DirectShowLib.BDA;

namespace v2_1
{
    /// <summary>
    /// This class test both IPersistTuneXmlUtility and IPersistTuneXmlUtility2
    /// </summary>
    public class IPersistTuneXmlUtility2Test
    {
        private IDVBTuneRequest dvbtr = null;
        private IPersistTuneXmlUtility2 xmlUtility;

        public IPersistTuneXmlUtility2Test() { }

        public void DoTests()
        {
            int hr = 0;

            Config();

            string savedTuneRequest;

            hr = xmlUtility.Serialize(dvbtr, out savedTuneRequest);
            DsError.ThrowExceptionForHR(hr);

            object o;

            // Only test the string version of the method but it's the more conveniant for a .Net usage.
            hr = xmlUtility.Deserialize(savedTuneRequest, out o);
            DsError.ThrowExceptionForHR(hr);
            Debug.Assert(o is IDVBTuneRequest);

            IDVBTuneRequest dvbtr2 = (IDVBTuneRequest)o;

            int onid, tsid, sid;

            dvbtr2.get_ONID(out onid);
            dvbtr2.get_TSID(out tsid);
            dvbtr2.get_SID(out sid);

            Debug.Assert((onid == 8442) && (tsid == 1) && (sid == 259));
        }

        private void Config()
        {
            xmlUtility = (IPersistTuneXmlUtility2)new PersistTuneXmlUtility();

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
        }

    }
}
