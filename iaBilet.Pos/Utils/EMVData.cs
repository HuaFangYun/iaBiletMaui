using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos.Utils
{
    public class EMVData
    {
        //lista completa aici https://www.eftlab.com/index.php/site-map/knowledge-base/145-emv-nfc-tags

        static string[] tags = { "84", "95", "9F12", "9F26", "9F27", "9F34" };
        string _tlvstr;
        private Dictionary<string, string> _data;

        public EMVData(string tlvstring)
        {
            this._tlvstr = tlvstring;
            this.Parse();
        }

        public Dictionary<string, string> data
        {
            get
            {
                return _data;
            }
        }

        //Dedicated File (DF) Name 84
        public string DF
        {
            get
            {
                return _data["84"];
            }
        }
        // Terminal Verification Results
        public string TVR
        {
            get
            {
                return _data["95"];
            }
        }
        //Application Preferred Name 9F12
        public string APN
        {
            get
            {
                return _data["9F12"];
            }
        }
        public string APNNAME
        {
            get
            {
                return EMVData.FromHexString(this.APN);
            }
        }
        //Application Cryptogram
        public string AC
        {
            get
            {
                return _data["9F26"];
            }
        }
        //Cryptogram Information Data
        public string CID
        {
            get
            {
                return _data["9F27"];
            }
        }

        //Cardholder Verification Method (CVM) Results
        public string CVM
        {
            get
            {
                return _data["9F34"];
            }
        }


        private void Parse()
        {
            _data = new Dictionary<string, string>();
            var bytes = new byte[_tlvstr.Length / 2];
            for (int i = 0; i < EMVData.tags.Length; i++)
            {
                string t = EMVData.tags[i];
                int pos = _tlvstr.IndexOf(t);
                if (pos < 0)
                {
                    continue;
                }
                int l = Int32.Parse(_tlvstr.Substring(pos + t.Length, 2), System.Globalization.NumberStyles.HexNumber);
                int statIndex = pos + t.Length + 2;
                int max = _tlvstr.Length - statIndex;
                if (max < l * 2)
                {
                    break;
                }
                string v = _tlvstr.Substring(pos + t.Length + 2, l * 2);
                _data[t] = v;
            }
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return Encoding.Default.GetString(bytes);
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
    }
}
