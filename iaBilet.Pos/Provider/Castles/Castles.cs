using iaBilet.Core.Lib;
using iaBilet.Pos.Utils;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Pos.Provider.Castles
{
    public class Castles : PosProvider
    {

        public static byte ACK = 0x06;
        public static byte STX = 0x02;
        public static byte ETX = 0x03;
        public static byte EOT = 0x04;
        public static byte NAK = 0x15;
        public static byte ZERO = 0x30;
        public static byte FS = 0x1C;
        public int Port { get; set; }
        public int BaudRate { get; set; }


        public byte[] ChargeCommand
        {

            get
            {
                int AmountAsInt = Convert.ToInt32(Amount * 100);
                byte[] etx = { ETX };
                byte[] stx = { STX };
                byte[] c = ByteUtils.Combine(
                   EMVData.HexStringToByteArray("F000"),
                   EMVData.HexStringToByteArray("0001"),
                   EMVData.HexStringToByteArray("F9"),
                   EMVData.HexStringToByteArray("F003"),
                   EMVData.HexStringToByteArray("0001"),
                   EMVData.HexStringToByteArray("A1"),
                   EMVData.HexStringToByteArray("F004"),
                   EMVData.HexStringToByteArray("000C"),
                   ByteUtils.ConvertStringToByteArray(AmountAsInt.ToString().PadLeft(12, '0'))
                );
                byte[] crc16 = EMVData.HexStringToByteArray(ByteUtils.CalculateCRC16(c).ToString("X4"));
                byte[] len = EMVData.HexStringToByteArray(c.Length.ToString("X").PadLeft(4, '0'));
                byte[] cmd = ByteUtils.Combine(stx, len, c, etx, crc16);
                Console.WriteLine("sending command = " + ByteUtils.ByteArrayToHexString(cmd));
                return cmd;
            }
        }

        public byte[] VoidCommand
        {
            get
            {
                int AmountAsInt = Convert.ToInt32(Amount * 100);
                byte[] etx = { ETX };
                byte[] stx = { STX };
                byte[] c = ByteUtils.Combine(
                   EMVData.HexStringToByteArray("F000"),
                   EMVData.HexStringToByteArray("0001"),
                   EMVData.HexStringToByteArray("F9"),
                   EMVData.HexStringToByteArray("F003"),
                   EMVData.HexStringToByteArray("0001"),
                   EMVData.HexStringToByteArray("A3"),
                   EMVData.HexStringToByteArray("F004"),
                   EMVData.HexStringToByteArray("000C"),
                   ByteUtils.ConvertStringToByteArray(AmountAsInt.ToString().PadLeft(12, '0'))
                );
                byte[] crc16 = EMVData.HexStringToByteArray(ByteUtils.CalculateCRC16(c).ToString("X4"));
                byte[] len = EMVData.HexStringToByteArray(c.Length.ToString("X").PadLeft(4, '0'));
                byte[] cmd = ByteUtils.Combine(stx, len, c, etx, crc16);
                Console.WriteLine("sending command = " + ByteUtils.ByteArrayToHexString(cmd));
                return cmd;
            }
        }
        private bool IsAcck(byte[] b)
        {
            byte[] ack = { ACK };
            return BitConverter.ToString(b) == BitConverter.ToString(ack);
        }
        private bool IsAcck(string hex)
        {
            byte[] ack = { ACK };
            return hex.ToLower() == BitConverter.ToString(ack).ToLower();
        }
        public string PortName
        {
            get => string.Format("COM{0}", Port);
        }

        public override Task Pay()
        {
            Log.WriteLine("PortName = " + PortName);
            Response = "";
            return Task.Run(async () =>
            {
                if (string.IsNullOrEmpty(PortName))
                {
                    TriggerResponseReceived(true, "Connection failed");
                    return;
                }

                byte[] ack = { 0x06 };

                SerialCommunication SerialCom = new SerialCommunication(PortName, BaudRate, Parity.None, 8, StopBits.One);
                bool connected = SerialCom.EnsureConnection();
                if (!connected)
                {
                    TriggerResponseReceived(true, "Connection failed");
                    return;
                }

                SerialCom.TimeoutSeconds = TimoutSeconds;
                string result = string.Empty;
                byte[] b = new byte[0];
                if (TransactionType == TransactionType.Sale)
                {
                    b = await SerialCom.GetBytes(ChargeCommand, 1);
                }
                if (TransactionType == TransactionType.Void)
                {
                    b = await SerialCom.GetBytes(VoidCommand, 1);
                }

                Response = BitConverter.ToString(b);

                if (IsAcck(b))
                {
                    TriggerResponseReceived();
                    byte[] res = await SerialCom.AwaitResult(ETX, 2);
                    Response = BitConverter.ToString(res);
                    TriggerResponseReceived();
                    byte[] f = await SerialCom.GetBytes(ack, 0);
                }
                else
                {
                    TriggerResponseReceived(true, "Pos Error");
                }

                SerialCom.CloseConnection();
            });
        }

        public override void TriggerResponseReceived(bool error = false, string message = "")
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                string CleanResponse = Response.Replace("-", "");
                CastlesResponse response = new CastlesResponse(CleanResponse);
                Log.WriteLine("respons is error" + response.IsError);

                if (error || response.IsError)
                {
                    response.IsError = true;
                    response.Message = response.Message ?? message;

                }

                UserMessage = response.Message;

                if (IsAcck(Response))
                {
                    response.IsHold = true;
                }

                if (!response.IsError && !response.IsHold)
                {
                    TransactionFinshedWithSuccess = true;
                }
                PosResponseEventArgs args = new PosResponseEventArgs(response, response.GetType());
                OnResponseReceived(args);
            });
        }
    }
}
