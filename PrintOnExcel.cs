
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace asterixDataRecording
{
    class PrintOnExcel
    {
        private DataTable table;
        public PrintOnExcel(DataTable table)
        {
            this.table = table;
        }     
        public void WriteExcelFile()
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(@"C:\Users\Waleed Maqsood\Desktop\AS\Text.xlsx", SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                List<String> columns = new List<string>();
                Row headerRow = new Row();
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
               
                sheets.Append(sheet);
                
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();


                        if (IsDouble(dsrow[col].ToString()))
                        {
                            cell.DataType = CellValues.Number;
                        }
                      
                        else
                        {
                            cell.DataType = CellValues.String;
                        }

                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                        sheetData.AppendChild(newRow);
                }

                        workbookPart.Workbook.Save();
            }
        }
        public static bool IsDouble(string s)
        {
            double dOutput = 0;
            if (Double.TryParse(s, out dOutput))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}





