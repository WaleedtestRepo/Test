using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace asterixDataRecording
{
    class ConvertToDataTables
    {
         private List<Ipv4Header> ipv4_Header_List;
         private List<UdpHeader> udp_Header_List;
         private List<UdpPayloadCat048> udpPayloadDataUnit_List;
         private List<string> dataUnitHex_List;
         private List<string> dataUnitCRC_List;
         private List<string> computedCRC_List;
         private List<bool> matchCRC_List = new List<bool>();

        public ConvertToDataTables(UdpPacketsFromFile packets)
        {
            this.ipv4_Header_List = packets.Ipv4_Header_List;
            this.udp_Header_List = packets.Udp_Header_List;
            this.udpPayloadDataUnit_List = packets.UdpPayloadDataUnit_List;
            this.dataUnitHex_List = packets.DataUnitHex_List;
            this.dataUnitCRC_List = packets.DataUnitCRC_List;
            this.computedCRC_List = packets.ComputedCRC_List;
            this.matchCRC_List = packets.MatchCRC_List;
        }

        public DataTable Convert()
        {
            //Getting Properties So They can be used as Columns
            PropertyDescriptorCollection properties_IpvHeader = TypeDescriptor.GetProperties(typeof(Ipv4Header));
            PropertyDescriptorCollection properties_UdpHeader = TypeDescriptor.GetProperties(typeof(UdpHeader));
            PropertyDescriptorCollection properties_UdpPayloadDataUnit = TypeDescriptor.GetProperties(typeof(UdpPayloadCat048));

            DataTable table = new DataTable();
            DataTable table_IpvHeader = new DataTable();
            DataTable table_UdpHeader = new DataTable();
            DataTable table_UdpPaylodDataUnit = new DataTable();
            DataTable table_DataUnitHex = new DataTable();
            DataTable table_DataUnitCRC = new DataTable();
            DataTable table_ComputedCRC = new DataTable();
            DataTable table_MatchCRC = new DataTable();

            table.Columns.Add("Line #", typeof(string));

            //Creating Data table for Line number could have been a primary key
            for (int i = 1; i < udpPayloadDataUnit_List.Count; i++)
            {
                table.Rows.Add(i);
            }

            //Creating Data table IPV4 HHeader 
            for (int i = 0; i < properties_IpvHeader.Count; i++)
            {
                PropertyDescriptor prop = properties_IpvHeader[i];
                table_IpvHeader.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] propertyValues_Ipv4Header = new object[properties_IpvHeader.Count];

            foreach (Ipv4Header item in ipv4_Header_List)
            {
                for (int i = 0; i < propertyValues_Ipv4Header.Length; i++)
                {
                    propertyValues_Ipv4Header[i] = properties_IpvHeader[i].GetValue(item);
                }
                table_IpvHeader.Rows.Add(propertyValues_Ipv4Header);
            }

            //Creating Data table for UDP Header 
            for (int i = 0; i < properties_UdpHeader.Count; i++)
            {
                PropertyDescriptor prop = properties_UdpHeader[i];
                table_UdpHeader.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] propertyValues_UdpHeader = new object[properties_UdpHeader.Count];

            foreach (UdpHeader item in udp_Header_List)
            {
                for (int i = 0; i < propertyValues_UdpHeader.Length; i++)
                {
                    propertyValues_UdpHeader[i] = properties_UdpHeader[i].GetValue(item);
                }
                table_UdpHeader.Rows.Add(propertyValues_UdpHeader);
            }

            //Creating Data table for UDP Payload  
            for (int i = 0; i < properties_UdpPayloadDataUnit.Count; i++)
            {
                PropertyDescriptor prop = properties_UdpPayloadDataUnit[i];
                table_UdpPaylodDataUnit.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] propertyValues_UdpPayloadDataUnit = new object[properties_UdpPayloadDataUnit.Count];

            foreach (UdpPayloadCat048 item in udpPayloadDataUnit_List)
            {
                for (int i = 0; i < propertyValues_UdpPayloadDataUnit.Length; i++)
                {
                    propertyValues_UdpPayloadDataUnit[i] = properties_UdpPayloadDataUnit[i].GetValue(item);
                }
                table_UdpPaylodDataUnit.Rows.Add(propertyValues_UdpPayloadDataUnit);
            }

            //Creating Data Table for Data Unit
            table_DataUnitHex.Columns.Add("Data Unit");
            for (int i = 0; i < dataUnitHex_List.Count; i++)
            {
                table_DataUnitHex.Rows.Add(dataUnitHex_List[i]);
            }

            //Creating Data Table for Data Unit CRC 
            table_DataUnitCRC.Columns.Add("Data Unit CRC");
            for (int i = 0; i < dataUnitCRC_List.Count; i++)
            {
                table_DataUnitCRC.Rows.Add(dataUnitCRC_List[i]);
            }

            //Creating Data Table for Computed CRC 
            table_ComputedCRC.Columns.Add("Computed CRC");
            for (int i = 0; i < computedCRC_List.Count; i++)
            {
                table_ComputedCRC.Rows.Add(computedCRC_List[i]);
            }

            //Creating Data Table to Match CRC
            table_MatchCRC.Columns.Add("CRC Match ?");
            for (int i = 0; i < matchCRC_List.Count; i++)
            {
                table_MatchCRC.Rows.Add(matchCRC_List[i]);
            }

            table.Columns.Add("SourceAddr");
            table.Columns.Add("DestAddr");

            table.Columns.Add("SourcePort");
            table.Columns.Add("DestPort");

            table.Columns.Add("Seq #");
            table.Columns.Add("Category");
            table.Columns.Add("SAC");
            table.Columns.Add("SIC");
            table.Columns.Add("Data Block Length (bytes)");
            table.Columns.Add("Data Unit Length (bytes)");
            table.Columns.Add("Data Unit");
            table.Columns.Add("Data Unit CRC");
            table.Columns.Add("Computed CRC");
            table.Columns.Add("CRC Match ?");
            table.Columns.Add("TYP");
            table.Columns.Add("RAB");
            table.Columns.Add("TST");
            table.Columns.Add("Range");
            table.Columns.Add("AzimuthACP");
            table.Columns.Add("AzimuthDeg");
            table.Columns.Add("Mode3a");
            table.Columns.Add("Mode3aV");
            table.Columns.Add("Mode3aG");
            table.Columns.Add("Mode3aL");
            table.Columns.Add("ModeC");
            table.Columns.Add("ModeCV");
            table.Columns.Add("ModeCG");
            table.Columns.Add("SRR");
            table.Columns.Add("SAM");

            //Merging DataTables 
            for (int i = 0; i < table.Rows.Count; i++)
            {
                //Ipv4 Header 
                table.Rows[i]["SourceAddr"] = table_IpvHeader.Rows[i]["SourceAddr"];
                table.Rows[i]["DestAddr"] = table_IpvHeader.Rows[i]["DestAddr"];

                //Udp Header 
                table.Rows[i]["SourcePort"] = table_UdpHeader.Rows[i]["SourcePort"];
                table.Rows[i]["DestPort"] = table_UdpHeader.Rows[i]["DestPort"];

                //UDP Payload Data Unit
                table.Rows[i]["Seq #"] = table_UdpPaylodDataUnit.Rows[i]["SequenceNumber"];
                table.Rows[i]["Category"] = table_UdpPaylodDataUnit.Rows[i]["Category"];
                table.Rows[i]["SAC"] = table_UdpPaylodDataUnit.Rows[i]["SAC"];
                table.Rows[i]["SIC"] = table_UdpPaylodDataUnit.Rows[i]["SIC"];
                table.Rows[i]["Data Block Length (bytes)"] = table_UdpPaylodDataUnit.Rows[i]["Length"];
                table.Rows[i]["Data Unit Length (bytes)"] = table_UdpPaylodDataUnit.Rows[i]["DataUnitLength"];

                //Data Unit in Hex
                table.Rows[i]["Data Unit"] = table_DataUnitHex.Rows[i]["Data Unit"];

                //Data Unit CRC  
                table.Rows[i]["Data Unit CRC"] = table_DataUnitCRC.Rows[i]["Data Unit CRC"];

                //Computer CRC
                table.Rows[i]["Computed CRC"] = table_ComputedCRC.Rows[i]["Computed CRC"];

                table.Rows[i]["CRC Match ?"] = table_MatchCRC.Rows[i]["CRC Match ?"];

                table.Rows[i]["TYP"] = table_UdpPaylodDataUnit.Rows[i]["TYP"];
                table.Rows[i]["RAB"] = table_UdpPaylodDataUnit.Rows[i]["RAB"];
                table.Rows[i]["TST"] = table_UdpPaylodDataUnit.Rows[i]["TST"];
                table.Rows[i]["Range"] = table_UdpPaylodDataUnit.Rows[i]["Range"];
                table.Rows[i]["AzimuthACP"] = table_UdpPaylodDataUnit.Rows[i]["AzimuthACP"];
                table.Rows[i]["AzimuthDeg"] = table_UdpPaylodDataUnit.Rows[i]["AzimuthDeg"];
                table.Rows[i]["Mode3a"] = table_UdpPaylodDataUnit.Rows[i]["Mode3a"];
                table.Rows[i]["Mode3aV"] = table_UdpPaylodDataUnit.Rows[i]["Mode3aV"];
                table.Rows[i]["Mode3aG"] = table_UdpPaylodDataUnit.Rows[i]["Mode3aG"];
                table.Rows[i]["Mode3aL"] = table_UdpPaylodDataUnit.Rows[i]["Mode3aL"];
                table.Rows[i]["ModeC"] = table_UdpPaylodDataUnit.Rows[i]["ModeC"];
                table.Rows[i]["ModeCV"] = table_UdpPaylodDataUnit.Rows[i]["ModeCV"];
                table.Rows[i]["ModeCG"] = table_UdpPaylodDataUnit.Rows[i]["ModeCG"];
                table.Rows[i]["SRR"] = table_UdpPaylodDataUnit.Rows[i]["SRR"];
                table.Rows[i]["SAM"] = table_UdpPaylodDataUnit.Rows[i]["SAM"];
            }

            return table;


        }

    }
}
