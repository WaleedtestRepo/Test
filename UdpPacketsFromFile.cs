using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

//Reads Packets from file and adds them into List os String
namespace asterixDataRecording
{
    //This class Reads Packets from File in String

    class UdpPacketsFromFile
    {
        //Ether net Header and UDP Header Length is fixed 
        private const int ETHERNET_HEADER_LENGTH = 14;
        private const int UDP_HEADER_LENGTH = 8;
        //Ipv4 header length is not fixed its needs to be calculated (The length is in he second nibble of first byte)
        private int ipv4HeaderLength;
        private int udpHeaderLengthInReferencetoIpv4;

        private Queue<byte> ipv4Header_Byte;
        private Queue<byte> udpHeader_Byte;
        private Queue<byte> udpPayloadDataUnit_Byte;
       
        private List<Ipv4Header> ipv4_Header_List=new List<Ipv4Header>();
        private List<UdpHeader> udp_Header_List = new List<UdpHeader>();
        private List<UdpPayloadCat048> udpPayloadDataUnit_List = new List<UdpPayloadCat048>();
        private List<string> dataUnitHex_List = new List<string>();
        private List<string> dataUnitCRC_List = new List<string>();
        private List<string> computedCRC_List = new List<string>();
        private List<bool> matchCRC_List = new List<bool>();

        public List<Ipv4Header> Ipv4_Header_List
        {
            get { return ipv4_Header_List; }
        }
        public List<UdpHeader> Udp_Header_List
        {
            get { return udp_Header_List; }
        }
        public List<UdpPayloadCat048> UdpPayloadDataUnit_List
        {
            get { return udpPayloadDataUnit_List; }
        }
        public List<string> DataUnitHex_List
        {
            get { return dataUnitHex_List; }
        }
        public List<string> DataUnitCRC_List
        {
            get { return dataUnitCRC_List; }
        }
        public List<string> ComputedCRC_List
        {
            get { return computedCRC_List; }
        }
        public List<bool> MatchCRC_List
        {
            get { return matchCRC_List; }
        }

        public void ReadFromFile()
        {
            List<string> time = new List<string>();
            List<string> singlePacket = new List<string>();
            //ReadAllLines Throws exception if file is not found 
            List<string> lines = File.ReadAllLines(@"C:\Users\Waleed Maqsood\Desktop\test1.txt").ToList();
            lines.RemoveAll(s => s.Contains("+---------+---------------+----------+") || s.Equals(""));         

            for (int i = 0; i < lines.Count; i++)
            {

                if (i % 2 == 0)
                {
                    time.Add(lines[i]);

                }

            }

            for (int i = 0; i < lines.Count; i++)
            {
                singlePacket = new List<string>();
             
                if (i % 2 != 0)
                {

                    singlePacket = lines[i].Split("|").ToList();

                    String ethernetHeader_Hex = string.Empty;
                    String ipv4Header_Hex = string.Empty;
                    String udpHeader_Hex = string.Empty;
                    String udpPayloadDataUnit_Hex = string.Empty;
                    String dataUnitCRC_Hex = string.Empty;
                    String computedCRC_hex;

                    ipv4HeaderLength = CalculateIpv4HederLength(singlePacket);
                    int categoryLocation = 16 + ipv4HeaderLength + UDP_HEADER_LENGTH + 2;

                    //See if the Line Contains a byt indicating CAT 048
                    if (singlePacket[categoryLocation].Equals("30"))
                    {

                        for (int j = 2; j < ETHERNET_HEADER_LENGTH + 2; j++)
                        {
                            ethernetHeader_Hex += singlePacket[j];
                        }

                        ipv4HeaderLength += 16;


                        //ipv4 header can Range anywhere from 20 to 60 bytes 
                        for (int k = 16; k < ipv4HeaderLength; k++)
                        {
                            ipv4Header_Hex += singlePacket[k];
                        }

                        ipv4Header_Byte = convertToByte(ipv4Header_Hex);
                        Ipv4Header ipv4Header_Obj = new Ipv4Header(ipv4Header_Byte);
                        ipv4Header_Obj.Parse();
                        ipv4_Header_List.Add(ipv4Header_Obj);

                        //UDP header 8 bytes fixed
                        udpHeaderLengthInReferencetoIpv4 = ipv4HeaderLength + 8;
                        for (int x = ipv4HeaderLength; x < udpHeaderLengthInReferencetoIpv4; x++)
                        {
                            udpHeader_Hex += singlePacket[x];
                        }

                        udpHeader_Byte = convertToByte(udpHeader_Hex);
                        UdpHeader udpHeader_obj = new UdpHeader(udpHeader_Byte);
                        udpHeader_obj.Parse();
                        udp_Header_List.Add(udpHeader_obj);

                        int dataUnitLengthBeforeCRC = singlePacket.Count - 5;

                        //UDP Payload contains DataUnit(Includes Sequence number and CRC)
                        for (int y = udpHeaderLengthInReferencetoIpv4; y < singlePacket.Count; y++)
                        {
                            udpPayloadDataUnit_Hex += singlePacket[y];
                        }

                        //Getting the DatUnit in Hex form 
                        dataUnitHex_List.Add(udpPayloadDataUnit_Hex);

                        udpPayloadDataUnit_Byte = convertToByte(udpPayloadDataUnit_Hex);
                        UdpPayloadCat048 udpPayload_obj = new UdpPayloadCat048(udpPayloadDataUnit_Byte);
                        udpPayload_obj.Parse();
                        udpPayloadDataUnit_List.Add(udpPayload_obj);

                        //Calculating the CRC from the Data Unit
                        CRC crcObjec = new CRC(udpPayloadDataUnit_Hex);
                        computedCRC_hex = crcObjec.ComputeCrc();
                        computedCRC_List.Add(computedCRC_hex);

                        //Getting the Data Unit CRC
                        for (int y = dataUnitLengthBeforeCRC; y < singlePacket.Count; y++)
                        {
                            dataUnitCRC_Hex += singlePacket[y];
                        }

                        dataUnitCRC_List.Add(dataUnitCRC_Hex);

                        if (dataUnitCRC_Hex.Equals(computedCRC_hex))
                        {
                            matchCRC_List.Add(true);
                        }
                      
                        else
                        {

                            matchCRC_List.Add(false);
                        }

                    }
                }

            }
        }

        public static Queue<byte> convertToByte(String dataToBeConverted)
        {
            Queue<byte> datainQueuebytes = new Queue<byte>();

            byte[] inputAsBytes = Enumerable.Range(0, dataToBeConverted.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(dataToBeConverted.Substring(x, 2), 16))
                         .ToArray();

            datainQueuebytes = new Queue<byte>(inputAsBytes);



            return datainQueuebytes;
        }

        private int CalculateIpv4HederLength(List<string> singlePacket)
        {

            int ipv4HeaderLength;
            //The header Size along with Version of IPV4 is in the first field IHL
            String ipv4HeaderFirstField = singlePacket[16];
            //Spliiting the first field into Version and Length refer to IPV4 Header Structure
            char[] versionAndLengthField = ipv4HeaderFirstField.ToCharArray();
            // Length needs to be multiplied by 4 in order to get the IPV4 header field 
            ipv4HeaderLength = (Int32.Parse(versionAndLengthField[1].ToString()) * 4);

            return ipv4HeaderLength;


        }






    }

}