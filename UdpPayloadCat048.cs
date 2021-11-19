using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asterixDataRecording
{
    class UdpPayloadCat048
    {
        private Queue<byte> inputAsBytes;
        public int SequenceNumber { get; private set; }
        public byte Category { get; private set; }
        public int Length { get; private set; }
        public int DataUnitLength { get; private set; }
        public byte SIC { get; private set; }
        public byte SAC { get; private set; }
        public decimal TimeOFtheDay { get; private set; }
        public string TYP { get; private set; }
        public string RAB { get; private set; }
        public string TST { get; private set; }
        public double Range { get; private set; }
        public double AzimuthDeg { get; private set; }
        public double AzimuthACP { get; private set; }
        public double Mode3a { get; private set; }
        public string Mode3aV { get; private set; }
        public string Mode3aG { get; private set; }
        public string Mode3aL { get; private set; }
        public double ModeC { get; private set; }
        public string ModeCV { get; private set; }
        public string ModeCG { get; private set; }
        public double SRR { get; private set; }
        public double SAM { get; private set; }
        public UdpPayloadCat048(Queue<byte> inputAsBytes)
        {
            this.inputAsBytes = inputAsBytes;

        }
        public void Parse()
        {
            byte FSPEC, byte1, byte2, byte3, byte4;
            bool[] FRN = new bool[25];
            bool FX;

            //Getting the Sequence number
            //Write a method to Blockify
            byte1 = inputAsBytes.Dequeue();
            byte2 = inputAsBytes.Dequeue();
            SequenceNumber = CombineTwoBytes(byte1, byte2);
            // Console.WriteLine("Sequence Number: " + SequenceNumber);

            //Getting the Category number
            Category = inputAsBytes.Dequeue();
            // Console.WriteLine("Category: " + byte1);

            //Getting the Length 
            byte1 = inputAsBytes.Dequeue();
            byte2 = inputAsBytes.Dequeue();

            Length = CombineTwoBytes(byte1, byte2);
            DataUnitLength = Length + 6;
            //Console.WriteLine("Length: " + Length);

            FSPEC = inputAsBytes.Dequeue();

            FRN[1] = (FSPEC & 128) != 0;
            FRN[2] = (FSPEC & 64) != 0;
            FRN[3] = (FSPEC & 32) != 0;
            FRN[4] = (FSPEC & 16) != 0;
            FRN[5] = (FSPEC & 8) != 0;
            FRN[6] = (FSPEC & 4) != 0;
            FRN[7] = (FSPEC & 2) != 0;
            FX = (FSPEC & 1) != 0;

            if (FX)
            {
                FSPEC = inputAsBytes.Dequeue();

                FRN[8] = (FSPEC & 128) != 0;
                FRN[9] = (FSPEC & 64) != 0;
                FRN[10] = (FSPEC & 32) != 0;
                FRN[11] = (FSPEC & 16) != 0;
                FRN[12] = (FSPEC & 8) != 0;
                FRN[13] = (FSPEC & 4) != 0;
                FRN[14] = (FSPEC & 2) != 0;
                FX = (FSPEC & 1) != 0;
            }

            if (FX)
            {
                FSPEC = inputAsBytes.Dequeue();

                FRN[15] = (FSPEC & 128) != 0;
                FRN[16] = (FSPEC & 64) != 0;
                FRN[17] = (FSPEC & 32) != 0;
                FRN[18] = (FSPEC & 16) != 0;
                FRN[19] = (FSPEC & 8) != 0;
                FRN[20] = (FSPEC & 4) != 0;
                FRN[21] = (FSPEC & 2) != 0;
                FX = (FSPEC & 1) != 0;
            }

            if (FX)
            {
                FSPEC = inputAsBytes.Dequeue();
                FRN[22] = (FSPEC & 128) != 0;
                FRN[23] = (FSPEC & 64) != 0;
                FRN[24] = (FSPEC & 64) != 0;
                FX = (FSPEC & 1) != 0;
            }

            if (FRN[1])
            {
                SAC = inputAsBytes.Dequeue();
                SIC = inputAsBytes.Dequeue();             
            }

            if (FRN[2])
            {
                int result;
                byte1 = inputAsBytes.Dequeue();
                byte2 = inputAsBytes.Dequeue();
                byte3 = inputAsBytes.Dequeue();

                result = (byte1 << 8) | byte2;
                result = (result << 8) | byte3;

                TimeOFtheDay = (decimal)result / 128;
            }

            if (FRN[3])
            {

                byte1 = inputAsBytes.Dequeue();

                byte result = (byte)(byte1 >> 5 & 0x7);

                if (result == 0)
                {
                    TYP = "No Detection";
                }

                else if (result == 1)
                {
                    TYP = "Single PSR detection";
                    // Console.WriteLine(TYP);
                }

                else if (result == 2)
                {

                    TYP = "Single SSR Detection";
                    // Console.WriteLine(TYP);

                }

                else if (result == 3)
                {
                    TYP = "SSR + PSR dewtection";
                    // Console.WriteLine(TYP);
                }
              
                else if (result == 4)
                {
                    TYP = "Single Mode S ALl-CAll";
                    // Console.WriteLine(TYP);
                }
               
                else if (result == 5)
                {
                    TYP = "Single Mode S Roll-Call";
                    //Console.WriteLine(TYP);
                }
               
                else if (result == 6)
                {
                    TYP = "Mode S ALL-Call+ PSR";
                }

                else if (result == 7)
                {
                    TYP = "Mode S Roll-CAll +PSR";
                    // Console.WriteLine(TYP);
                }

                result = (byte)((byte1 >> 1) & 1);

                if (result == 0)
                {
                    RAB = "Report From aircraft transponder";
                    //Console.WriteLine(RAB);
                }
                else if (result == 1)
                {
                    RAB = "Report From field moniter";
                    //Console.WriteLine(RAB);

                }

                FX = (byte1 & 1) != 0;

                while (FX)
                {
                    byte1 = inputAsBytes.Dequeue();
                    byte result2 = (byte)((byte1 >> 7) & 1);
                    
                    if (result2 == 0)
                    {
                        TST = "Real Target Report";
                        // Console.WriteLine(TST);
                    }
                    else if (result2 == 1)
                    {
                        TST = "Test Target Report ";
                        // Console.WriteLine(TST);

                    }
                    FX = (byte1 & 1) != 0;
                }
            }

            if (FRN[4])
            {
                byte1 = inputAsBytes.Dequeue();
                byte2 = inputAsBytes.Dequeue();
                byte3 = inputAsBytes.Dequeue();
                byte4 = inputAsBytes.Dequeue();

                double result = (byte1 << 8) | byte2;

                Range = result / 256;
                // Console.WriteLine("Range: " + Range);

                int result2 = ((byte3 << 8) | byte4);

                AzimuthDeg = result2 * 0.0054931640625;
                AzimuthACP = (AzimuthDeg * 4096) / 360;

                // Console.WriteLine("Azimuth(Deg): " + AzimuthDeg);
                // Console.WriteLine("Azimuth(ACP): " + AzimuthACP);
            }

            if (FRN[5])
            {
                byte1 = inputAsBytes.Dequeue();
                byte2 = inputAsBytes.Dequeue();

                byte firstBit = (byte)(byte1 & 128);
                byte secondBit = (byte)(byte1 & 64);
                byte thirdBit = (byte)(byte1 & 32);


                if (firstBit == 0)
                {
                    Mode3aV = "Code Validated";
                    // Console.WriteLine("Mode3a V: " + Mode3aV);
                }

                else if (firstBit == 1)
                {
                    Mode3aV = "Code not Validated";
                    // Console.WriteLine("Mode3a V: " + Mode3aV);
                }

                if (secondBit == 0)
                {
                    Mode3aG = "Default";
                    // Console.WriteLine("Mode3a G: " + Mode3aG);
                }
                else if (secondBit == 1)
                {
                    Mode3aG = "Garbled Code";
                    // Console.WriteLine("Mode3a G: " + Mode3aG);
                }

                if (thirdBit == 0)
                {
                    Mode3aL = "Mode 3/A code derived from reply of transponder";
                    // Console.WriteLine("Mode3a L: " + Mode3aL);
                }
                else if (thirdBit == 1)
                {
                    Mode3aL = "MOde 3/A code not extracted dueing the last scan";
                    //Console.WriteLine("Mode3a G: " + Mode3aL);
                }

                int result = (byte1 << 8) | byte2;
                int result2 = result & 4095;
                String OctalRepresentation = (Convert.ToString(result2, 8));
                Mode3a = Convert.ToDouble(OctalRepresentation);
                // Console.WriteLine("Mode3a: " + Mode3a);
            }

            if (FRN[6])
            {
                byte1 = inputAsBytes.Dequeue();
                byte2 = inputAsBytes.Dequeue();
                byte firstBit = (byte)(byte1 & 128);
                byte secondBit = (byte)(byte1 & 64);

                if (firstBit == 0)
                {
                    ModeCV = "Code Validated";
                    //Console.WriteLine(ModeCV);
                }

                else if (firstBit == 1)
                {
                    ModeCV = "Code not Validated";
                    // Console.WriteLine(ModeCV);
                }

                if (secondBit == 0)
                {
                    ModeCG = "Default";
                    // Console.WriteLine(ModeCG);
                }

                else if (secondBit == 1)
                {
                    ModeCG = "Garbled Code";
                    // Console.WriteLine(ModeCG);

                }

                int result = (byte1 << 8) | byte2;
                int result2 = result & 16383;
                ModeC = (result2 * 100) / 4;
                //  Console.WriteLine("Mode c: " + ModeC);
            }

            if (FRN[7])
            {
                byte1 = inputAsBytes.Dequeue();

                bool[] Subfield = new bool[8];

                Subfield[1] = (byte1 & 128) != 0;
                Subfield[2] = (byte1 & 64) != 0;
                Subfield[3] = (byte1 & 32) != 0;
                Subfield[4] = (byte1 & 16) != 0;
                Subfield[5] = (byte1 & 8) != 0;
                Subfield[6] = (byte1 & 4) != 0;
                Subfield[7] = (byte1 & 2) != 0;

                if (Subfield[1])
                {
                    byte2 = inputAsBytes.Dequeue();
                }

                if (Subfield[2])
                {
                    byte2 = inputAsBytes.Dequeue();
                    SRR = byte2;
                    // Console.WriteLine("SRR: " + SRR);
                }

                if (Subfield[3])
                {
                    byte2 = inputAsBytes.Dequeue();
                    SAM = byte2;
                    // Console.WriteLine("SAM: " + SAM);
                }

                if (Subfield[4])
                {
                    byte2 = inputAsBytes.Dequeue();

                }

                if (Subfield[5])
                {
                    byte2 = inputAsBytes.Dequeue();
                }

                if (Subfield[6])
                {
                    byte2 = inputAsBytes.Dequeue();
                }

                if (Subfield[7])
                {
                    byte2 = inputAsBytes.Dequeue();

                }
            }
            // Console.WriteLine("=========================================================================================");
        }
        private int CombineTwoBytes(byte Byte1, byte Byte2)
        {
            int block = (Byte1 << 8) | Byte2; ;
            return block;
        }
    }
}
