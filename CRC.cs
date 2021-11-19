using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace asterixDataRecording
{
    class CRC
    {
        private string dataUnit_hex;
        public CRC(string dataUnit_hex )
        {
            this.dataUnit_hex = dataUnit_hex;

        }
        public string ComputeCrc()
        {
            //Converting Hexadecimal String to Byte Array
            Byte[] inputAsBytes = Enumerable.Range(0, dataUnit_hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(dataUnit_hex.Substring(x, 2), 16))
                     .ToArray();

            UInt64 c0 = 0, c1 = 0, c2 = 0, c3 = 0;

            byte[] CRC = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                CRC[i] = inputAsBytes[inputAsBytes.Length - (4 - i)];
            }

            String d_crc = ConvertByteArrayTohex(CRC);

            for (int i = 0; i < inputAsBytes.Length - 4; i++)
            {
                c0 = (c0 + inputAsBytes[i]);
                c1 = (c1 + c0);
                c2 = (c2 + c1);
                c3 = (c3 + c2);
            }

            UInt64 x0 = (255 - ((c0 + c1 + c2 + c3) % 255)) % 255;
            UInt64 x1 = (c1 + 2 * c2 + 3 * c3) % 255;
            UInt64 x2 = (255 - ((c2 + 3 * c3) % 255)) % 255;
            UInt64 x3 = c3 % 255;

            byte[] resultofProcessing = new byte[4];
            resultofProcessing[0] = (byte)x0;
            resultofProcessing[1] = (byte)x1;
            resultofProcessing[2] = (byte)x2;
            resultofProcessing[3] = (byte)x3;

            //Converting Byte Array to Hex
            String c_crc = ConvertByteArrayTohex(resultofProcessing);
            return c_crc;
        }
        public static string ConvertByteArrayTohex(byte[] ba)
        {

            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();

        }
    }
}
