using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaBilet.Core.Lib
{
    public class ByteUtils
    {
        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public static byte[] AddByteToArrayAtBegining(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }
        public static byte[] AddByteToArray(byte[] bArray, byte newByte)
        {
            byte[] newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 0);
            newArray[bArray.Length] = newByte;
            return newArray;
        }

        public static byte[][] AddByteArrayToArray(byte[][] bArray, byte[] newByte)
        {
            byte[][] newArray = new byte[bArray.Length + 1][];
            bArray.CopyTo(newArray, 0);
            newArray[bArray.Length] = newByte;
            return newArray;
        }


        public static byte CalculateLRC(byte[] bytes)
        {
            byte LRC = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                LRC ^= bytes[i];
            }
            return LRC;
        }

        public static ushort CalculateCRC16(byte[] data)
        {
            ushort crc = 0x0000;
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                        crc = (ushort)((crc << 1) ^ 0x8005);
                    else
                        crc <<= 1;
                }
            }
            return crc;
        }

        public static byte[] ConvertIntToByteArray(int amount)
        {
            string asciitxt = amount.ToString();
            byte[] retval = System.Text.Encoding.ASCII.GetBytes(asciitxt);
            return retval;
        }

        public static byte[] ConvertStringToByteArray(string asciitxt)
        {
            byte[] retval = System.Text.Encoding.ASCII.GetBytes(asciitxt);
            return retval;
        }

        public static string ToString(byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        public static int ConvertHexToDec(string hexValue)
        {
            return Convert.ToInt32(hexValue, 16);
        }

        public static byte[] CovertHexToByteArray(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static string ConvertHexToString(string hexValue)
        {
            return System.Text.Encoding.ASCII.GetString(ByteUtils.CovertHexToByteArray(hexValue));
        }

        public static bool InByteArray(byte[] toFind, byte[] toSearch)
        {
            for (var i = 0; i + toFind.Length < toSearch.Length; i++)
            {
                var allSame = true;
                for (var j = 0; j < toFind.Length; j++)
                {
                    if (toSearch[i + j] != toFind[j])
                    {
                        allSame = false;
                        break;
                    }
                }

                if (allSame)
                {
                    return true;
                }
            }

            return false;
        }

        public static byte[][] Split(byte[] byteArray, byte separator)
        {
            byte[][] ret = new byte[0][];
            int j = 0;
            int lastFsIndex = -1;
            for (int i = 0; i < byteArray.Count(); i++)
            {
                bool split = byteArray[i] == separator || i == byteArray.Count();

                if (split)
                {
                    int len = lastFsIndex < 0 ? i : i - lastFsIndex - 1;
                    byte[] b = new byte[len];
                    Array.Copy(byteArray, lastFsIndex + 1, b, 0, len);
                    ret = AddByteArrayToArray(ret, byteArray);
                    lastFsIndex = i;
                }
            }
            return ret;
        }



        public static string ByteArrayToHexString(byte[] ba)
        {

            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string ToString7(byte[] bytes)
        {

            for (int i = 0; i < bytes.Length; i++)
            {
                byte[] strAsByte = new byte[] { (byte)bytes[i] };

                var bits = new BitArray(strAsByte);

                int count = 0;
                for (int k = 0; k < 8; k++)
                {
                    if (bits[k])
                    {
                        count++;
                    }
                }

            }
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static byte[] ByteTo7bitAsciiEvenParity(byte[] bytes)
        {



            for (int i = 0; i < bytes.Length; i++)
            {
                byte[] strAsByte = new byte[] { (byte)bytes[i] };


                var bits = new BitArray(strAsByte);
                int count = 0;

                for (int k = 0; k < 8; k++)
                {
                    if (bits[k])
                    {
                        count++;
                    }
                }

                if (count % 2 == 1)
                {
                    // Odd number of bits
                    bits[7] = true; // Set the left most bit as parity bit for even parity.
                }
                bits.CopyTo(strAsByte, 0);
                bytes[i] = strAsByte[0];

            }

            return bytes;

        }
        public static byte[] StringTo7bitAsciiEvenParity(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);

            return ByteUtils.ByteTo7bitAsciiEvenParity(bytes);

        }
    }
}
