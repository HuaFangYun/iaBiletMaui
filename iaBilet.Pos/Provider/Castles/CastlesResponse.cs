using iaBilet.Core.Lib;
using iaBilet.Pos.Utils;
using iaBilet.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iaBilet.Pos.Provider.Castles
{
    public static class CastlesTag
    {
        public const string TAG_CMD = "F000";
        public const string TAG_RESP_TX_AMT = "F101";
        public const string TAG_RESP_TX_CBAMT = "F102";
        public const string TAG_RESP_TX_RCPNO = "F103";
        public const string TAG_RESP_TX_CODE = "F104";
        public const string TAG_RESP_TX_RRN = "F105";
        public const string TAG_RESP_TX_AUTH_CODE = "F106";
        public const string TAG_TERMINAL_NO = "F110";
        public const string TAG_MERCHANT_NO = "F111";
        public const string TAG_CARD_TYPE = "F112";
        public const string TAG_CARD_NUMBER = "F113";
        public const string TAG_CARD_HOLDER_NAME = "F114";
        public const string TAG_CURRENCY_CODE = "F115";
        public const string TAG_CARD_PRESENT_TYPE = "F116";
        public const string TAG_POS_PIN_ENTY_MODE = "F117";

    }
    public class CastlesResponse : PosResponse
    {
        public Dictionary<string, string> _currencies = new Dictionary<string, string>() {
            {"784","AED"}, {"971","AFN"}, {"8","ALL"}, {"51","AMD"}, {"532","ANG"}, {"973","AOA"}, {"32","ARS"}, {"36","AUD"}, {"533","AWG"}, {"944","AZN"}, {"977","BAM"}, {"52","BBD"}, {"50","BDT"}, {"975","BGN"}, {"48","BHD"}, {"108","BIF"}, {"60","BMD"}, {"96","BND"}, {"68","BOB"}, {"984","BOV"}, {"986","BRL"}, {"44","BSD"}, {"64","BTN"}, {"72","BWP"}, {"933","BYN"}, {"84","BZD"}, {"124","CAD"}, {"976","CDF"}, {"947","CHE"}, {"756","CHF"}, {"948","CHW"}, {"990","CLF"}, {"152","CLP"}, {"156","CNY"}, {"170","COP"}, {"970","COU"}, {"188","CRC"}, {"931","CUC"}, {"192","CUP"}, {"132","CVE"}, {"203","CZK"}, {"262","DJF"}, {"208","DKK"}, {"214","DOP"}, {"12","DZD"}, {"818","EGP"}, {"232","ERN"}, {"230","ETB"}, {"978","EUR"}, {"242","FJD"}, {"238","FKP"}, {"826","GBP"}, {"981","GEL"}, {"936","GHS"}, {"292","GIP"}, {"270","GMD"}, {"324","GNF"}, {"320","GTQ"}, {"328","GYD"}, {"344","HKD"}, {"340","HNL"}, {"191","HRK"}, {"332","HTG"}, {"348","HUF"}, {"360","IDR"}, {"376","ILS"}, {"356","INR"}, {"368","IQD"}, {"364","IRR"}, {"352","ISK"}, {"388","JMD"}, {"400","JOD"}, {"392","JPY"}, {"404","KES"}, {"417","KGS"}, {"116","KHR"}, {"174","KMF"}, {"408","KPW"}, {"410","KRW"}, {"414","KWD"}, {"136","KYD"}, {"398","KZT"}, {"418","LAK"}, {"422","LBP"}, {"144","LKR"}, {"430","LRD"}, {"426","LSL"}, {"434","LYD"}, {"504","MAD"}, {"498","MDL"}, {"969","MGA"}, {"807","MKD"}, {"104","MMK"}, {"496","MNT"}, {"446","MOP"}, {"929","MRU"}, {"480","MUR"}, {"462","MVR"}, {"454","MWK"}, {"484","MXN"}, {"979","MXV"}, {"458","MYR"}, {"943","MZN"}, {"516","NAD"}, {"566","NGN"}, {"558","NIO"}, {"578","NOK"}, {"524","NPR"}, {"554","NZD"}, {"512","OMR"}, {"590","PAB"}, {"604","PEN"}, {"598","PGK"}, {"608","PHP"}, {"586","PKR"}, {"985","PLN"}, {"600","PYG"}, {"634","QAR"}, {"946","RON"}, {"941","RSD"}, {"643","RUB"}, {"646","RWF"}, {"682","SAR"}, {"90","SBD"}, {"690","SCR"}, {"938","SDG"}, {"752","SEK"}, {"702","SGD"}, {"654","SHP"}, {"694","SLL"}, {"706","SOS"}, {"968","SRD"}, {"728","SSP"}, {"930","STN"}, {"222","SVC"}, {"760","SYP"}, {"748","SZL"}, {"764","THB"}, {"972","TJS"}, {"934","TMT"}, {"788","TND"}, {"776","TOP"}, {"949","TRY"}, {"780","TTD"}, {"901","TWD"}, {"834","TZS"}, {"980","UAH"}, {"800","UGX"}, {"840","USD"}, {"997","USN"}, {"940","UYI"}, {"858","UYU"}, {"927","UYW"}, {"860","UZS"}, {"928","VES"}, {"704","VND"}, {"548","VUV"}, {"882","WST"}, {"950","XAF"}, {"961","XAG"}, {"959","XAU"}, {"955","XBA"}, {"956","XBB"}, {"957","XBC"}, {"958","XBD"}, {"951","XCD"}, {"960","XDR"}, {"952","XOF"}, {"964","XPD"}, {"953","XPF"}, {"962","XPT"}, {"994","XSU"}, {"963","XTS"}, {"965","XUA"}, {"999","XXX"}, {"886","YER"}, {"710","ZAR"}, {"967","ZMW"}, {"932","ZWL"}
        };

        private Dictionary<string, string> Response = new Dictionary<string, string>() {
            {CastlesTag.TAG_CMD, ""},
            {CastlesTag.TAG_RESP_TX_AMT, ""},
            {CastlesTag.TAG_RESP_TX_CBAMT, ""},
            {CastlesTag.TAG_RESP_TX_RCPNO, ""},
            {CastlesTag.TAG_RESP_TX_CODE, ""},
            {CastlesTag.TAG_RESP_TX_RRN, ""},
            {CastlesTag.TAG_RESP_TX_AUTH_CODE, ""},
            {CastlesTag.TAG_TERMINAL_NO, ""},
            {CastlesTag.TAG_MERCHANT_NO, ""},
            {CastlesTag.TAG_CARD_TYPE, ""},
            {CastlesTag.TAG_CARD_NUMBER, ""},
            {CastlesTag.TAG_CARD_HOLDER_NAME, ""},
            {CastlesTag.TAG_CURRENCY_CODE, ""},
            {CastlesTag.TAG_CARD_PRESENT_TYPE, ""},
            {CastlesTag.TAG_POS_PIN_ENTY_MODE, ""},
        };

        public string CashBackAmount
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_RESP_TX_CBAMT]);
        }

        public string ReceiptNumber
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_RESP_TX_RCPNO]);
        }

        public string TransactionResponseCode
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_RESP_TX_CODE]);
        }

        public string TransactionReferenceNumber
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_RESP_TX_RRN]);
        }

        public string TransactionResponseAuthCode
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_RESP_TX_AUTH_CODE]);
        }

        public string TerminalNumber
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_TERMINAL_NO]);
        }

        public string MerchantNumber
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_MERCHANT_NO]);
        }
        public string CardType
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_CARD_TYPE]);
        }
        public string CardNumber
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_CARD_NUMBER]);
        }

        public string CardHolderName
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_CARD_HOLDER_NAME]);
        }

        public string CurrencyCode
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_CURRENCY_CODE]);
        }
        public string PresentType
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_CARD_PRESENT_TYPE]);

        }

        public string PinEntryMode
        {
            get => ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_POS_PIN_ENTY_MODE]);

        }
        public string StatusName
        {
            get
            {
                switch (TransactionResponseCode.ToUpper())
                {
                    case "00": return "APROVED"; break;
                    case "6A": return "CARD_ERROR"; break;
                    case "6B": return "USER_CANCELLED"; break;
                    case "6C": return "TIMEOUT"; break;
                    case "99": return "REJECTED"; break;
                    default: return "FAILED"; break;
                }
            }
        }

        public decimal Amount
        {
            get
            {
                string amount = ByteUtils.ConvertHexToString(Response[CastlesTag.TAG_RESP_TX_AMT]);
                if (amount.Length < 3)
                {
                    return 0;
                }
                amount = amount.Insert(amount.Length - 2, ".");

                string normalized = amount.TrimStart('0');
                if (normalized.Length == 0)
                {
                    return 0;
                }
                return Convert.ToDecimal(normalized);
            }
        }
        public string CurrencyName
        {
            get
            {
                string name = string.Empty;
                try
                {
                    name = _currencies[CurrencyCode];
                }
                catch (Exception ex)
                {

                }
                return name;
            }
        }

        public string CardIssuer
        {
            get
            {
                string issuer = "";
                try
                {
                    Dictionary<string, string> regexp = new Dictionary<string, string>() {
                    {"^4[0-9]", "Visa"}, //All Visa card numbers start with a 4.
                    {"^(5[1-5]|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)", "MasterCard"},//MasterCard numbers either start with the numbers 51 through 55 or with the numbers 2221 through 2720.
                    {"^(34|37)", "American Express"}, ////American Express card numbers start with 34 or 37
                    {"^(6011|65)", "Discover"}, //Discover card numbers begin with 6011 or 65. All have 16
                    {"^3(?:0[0-5]|[68][0-9])", "Diners Club"}, //Diners Club card numbers begin with 300 through 305, 36 or 38
                    {"^(2131|1800|35)","JCB" } //JCB cards beginning with 2131 or 1800 have 15 digits. JCB cards beginning with 35 have 16 digits
                };
                    foreach (KeyValuePair<string, string> entry in regexp)
                    {
                        Regex regex = new Regex(entry.Key);
                        if (regex.Match(this.CardNumber).Success)
                        {
                            return entry.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLine("Cannot detect card type. Message: " + ex.Message);
                }

                return issuer;
            }
        }
        public override string Message
        {
            get
            {
                if (IsHold)
                {

                    return AppResources.FollowTerminalInstruction;
                }
                switch (TransactionResponseCode.ToUpper())
                {
                    case "00": return IsError ? AppResources.TransactionFailed : AppResources.TransactionApproved; break;
                    case "6A": return AppResources.CardError; break;
                    case "6B": return AppResources.TransactionCancelledByUser; break;
                    case "6C": return AppResources.TimedOut; break;
                    case "99": return AppResources.TransactionRejected; break;
                    default: return AppResources.TransactionFailed; break;
                }
            }
        }

        public CastlesResponse(string response)
        {
            ProcessResponseString(response);
        }

        protected virtual void ProcessResponseString(string response)
        {
            if (response == null)
            {
                return;
            }
            HexResponse = response;
            byte[] ack = { 0x06 };
            if (response == ByteUtils.ByteArrayToHexString(ack))
            {
                IsHold = true;
                return;
            }
            IsHold = false;
            try
            {

                int len = response.Length;
                string etx1 = response.Substring(0, 2);
                string etx2 = response.Substring(2, 2);
                if (etx2 != "02")
                {
                    etx2 = ""; //unele mesaje vin c 2 ETX la inceput altele doar cu cu unul csfncsf
                }
                string mLen = response.Substring(etx1.Length + etx2.Length, 4);
                int offset = etx1.Length + etx2.Length + mLen.Length;
                int messageBodyLen = ByteUtils.ConvertHexToDec(mLen) * 2;
                string messageBody = response.Substring(offset, messageBodyLen);
                string messageCrc = response.Substring(offset + messageBodyLen + 2, 4);//mai adaugaum 2 pentru ca dupa mesaj uremaza ETX

                string calculatedCrc = ByteUtils.CalculateCRC16(EMVData.HexStringToByteArray(messageBody)).ToString("X4");
                Log.WriteLine("message body is " + messageBody);
                Log.WriteLine(string.Format("message crc = {0} calculatedCrc = {1}", messageCrc, calculatedCrc));
                while (messageBody.Length > 0)
                {
                    string tag = messageBody.Substring(0, 4);

                    int tagLen = ByteUtils.ConvertHexToDec(messageBody.Substring(4, 4));
                    string tagValue = messageBody.Substring(8, tagLen * 2);
                    int totalLen = 4 + 4 + (tagLen * 2);
                    Log.WriteLine(string.Format("T = {0} L = {1} V = {2}", tag, tagLen, tagValue));
                    Response[tag.ToUpper()] = tagValue;
                    messageBody = messageBody.Substring(totalLen, messageBody.Length - totalLen);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine("Cannot parse body message " + ex.Message);
            }

        }
    }
}
