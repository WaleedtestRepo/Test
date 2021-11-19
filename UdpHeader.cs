using System;
using System.Collections.Generic;
using System.Text;

namespace asterixDataRecording
{
    class UdpHeader
    {

        Queue<byte> inputAsBytes;
        public int SourcePort { get; private set; }
        public int DestPort { get; private set; }

        public UdpHeader(Queue<byte> inputAsBytes)
        {
            this.inputAsBytes = inputAsBytes;

        }

        public void Parse()
        {
            byte byte1, byte2;
            byte1 = inputAsBytes.Dequeue();
            byte2 = inputAsBytes.Dequeue();
            int result = (byte1 << 8) | byte2;
            SourcePort = result;

            byte1 = inputAsBytes.Dequeue();
            byte2 = inputAsBytes.Dequeue();
            result = (byte1 << 8) | byte2;
            DestPort = result;
        }





    }
}
