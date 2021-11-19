using System;
using System.Data;
using System.Diagnostics;

namespace asterixDataRecording
{
    class Main1
    {
        static void Main(string[] args)
        {
            UdpPacketsFromFile packetsFromFile = new UdpPacketsFromFile();

            packetsFromFile.ReadFromFile();

            ConvertToDataTables dataTable = new ConvertToDataTables(packetsFromFile);
            DataTable table=dataTable.Convert();

            PrintOnExcel print = new PrintOnExcel(table);
            print.WriteExcelFile();
             
            //TO test the performence of the code 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
        }
    }
}

