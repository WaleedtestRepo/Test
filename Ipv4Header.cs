using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;

namespace asterixDataRecording
{
    class Ipv4Header
    {

        private Queue<byte> inputAsBytes;
        public String SourceAddr { get; private set; }
        public String DestAddr { get; private set; }
        public Ipv4Header(Queue<byte> inputAsBytes)
        {
            this.inputAsBytes = inputAsBytes;
        }
        public void Parse()
        {
            byte byte1, byte2, byte3, byte4;

            for (int i = 0; i < 12; i++)
            {
                inputAsBytes.Dequeue();
            }

            byte1 = inputAsBytes.Dequeue();
            byte2 = inputAsBytes.Dequeue();
            byte3 = inputAsBytes.Dequeue();
            byte4 = inputAsBytes.Dequeue();
            byte[] bytearray = new byte[] { byte1, byte2, byte3, byte4 };
            IPAddress ip = new IPAddress(bytearray);
            SourceAddr = ip.ToString();
            // Console.WriteLine(SourceAddr);

            byte1 = inputAsBytes.Dequeue();
            byte2 = inputAsBytes.Dequeue();
            byte3 = inputAsBytes.Dequeue();
            byte4 = inputAsBytes.Dequeue();

            bytearray = new byte[] { byte1, byte2, byte3, byte4 };

            ip = new IPAddress(bytearray);
            DestAddr = ip.ToString();
            // Console.WriteLine(DestAddr);
        }

    }
}
