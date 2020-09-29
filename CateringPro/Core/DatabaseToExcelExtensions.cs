using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public static class DatabaseToExcelExtensions
    {
        public static ExcelMaterialize ExcelMaterialize(this DatabaseFacade database,string sqlQuery,string reportname)
        {
            return new ExcelMaterialize()
            {
                DatabaseFacade = database,
                SQLQuery = sqlQuery,
                ReportName = reportname
            };
        }
    }


    public class ExcelMaterialize
    {
        public DatabaseFacade DatabaseFacade { get; set; }
        public string SQLQuery { get; set; }
        public string ReportName { get; set; }
        public async Task<string> GetStreamAsync()
        {

            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQLQuery;
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {

                        return Export(reader);
                    }
                   // reader.Dispose();
                }
            }

            finally
            {
                conn.Close();
            }

        }
        private  string Export(DbDataReader reader)
        {
            
            string testpath = "c:\\Temp\\SavedDocument.xlsx";
            string temppath = Path.Combine("Temp", Guid.NewGuid().ToString());
            Directory.CreateDirectory("Temp");
            string templatepath = Path.Combine("Templates", $"{ReportName}.xlsx");
            using (MemoryStream ms = new MemoryStream()) {
                using (FileStream file = new FileStream(templatepath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);

                }
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(ms, true))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    var wsp = workbookPart.WorksheetParts;
                    // Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();

                    var sh = GetWorksheetPartByName(doc, "rawdata");
                    var existname = workbookPart.Workbook.DefinedNames.Elements<DefinedName>().FirstOrDefault(n => n.Name == "mydata");
                    if (existname != null)
                        workbookPart.Workbook.DefinedNames.RemoveChild(existname);
                    StringBuilder excelResult = new StringBuilder();
                    RemoveAllRows(sh);
                    WriteHeader(sh, reader);
                    WriteRowData(sh, reader);
                    WriteDefinedName(workbookPart, sh);
                    //var test = workbookPart.Workbook.DefinedNames;
                   // var dfname = new DefinedName() { Name = "mydata" };
                   // dfname.Text = String.Format("{0}!{1}:{2}", "rawdata", "$A$1", "$C$5");

                   //  workbookPart.Workbook.DefinedNames.Append(dfname);

                    File.Delete(temppath);
                    using (FileStream stream = new FileStream(temppath,
                                         FileMode.CreateNew, FileAccess.ReadWrite))
                    {

                        workbookPart.Workbook.Save();
                        doc.Close();
                        // doc.Save();
                        //ms.Position = 0;
                        ms.WriteTo(stream);
                    }

                }
            }
           // ms.Position = 0;
            return temppath;

        }
        private void WriteDefinedName(WorkbookPart workbookPart, WorksheetPart wssheatpart)
        {
            try
            {
                int rows = wssheatpart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Count();
                int cols = wssheatpart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().First().Elements<Cell>().Count();
                var dfname = new DefinedName() { Name = "mydata" };
                string colname = ColumnIndexToColumnLetter(cols);
                dfname.Text = $"rawdata!$A$1:${colname}${rows}";
                  

                workbookPart.Workbook.DefinedNames.Append(dfname);
            }
            catch(Exception ex)
            {

            }
        }
        private void WriteTestData(WorksheetPart wssheatpart)
        {
            SheetData firstChild = wssheatpart.Worksheet.GetFirstChild<SheetData>();



            for (uint i = 2; i < 100; i++)
            {
                Row row = FindRow(firstChild, i);
                for (int j = 0; j < 5; j++)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue("val" + i.ToString() + j.ToString());
                    row.AppendChild(cell);
                }
            }
        }
        public void WriteHeader(WorksheetPart wssheatpart, DbDataReader reader)
        {
            SheetData firstChild = wssheatpart.Worksheet.GetFirstChild<SheetData>();


            Row headerRow = FindRow(firstChild, 1);
            int fields = reader.FieldCount;
            headerRow.RemoveAllChildren();
            for (int i = 0; i < fields; i++)
            {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(reader.GetName(i));
                headerRow.AppendChild(cell);

            }

            /*
            for (int i = 0; i < 5; i++)
            {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue("col" + i.ToString());
                headerRow.AppendChild(cell);
            }
            */

        }
        public void WriteRowData(WorksheetPart wssheatpart, DbDataReader reader)
        {

            SheetData sheetData = wssheatpart.Worksheet.GetFirstChild<SheetData>();
            uint rowindex = 2;
            int fields = reader.FieldCount;
            Func<Type,CellValues?> typeselector = (t) =>
            {
               // if (t == typeof(DateTime))
               //          return CellValues.Date;
                if (t == typeof(decimal) || t== typeof(Int32))
                //       return null;
                   return CellValues.Number;
                if (t == typeof(bool))
                        return CellValues.Number;
                return CellValues.String;
            };
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            Func<DbDataReader, int, string> formatvalue = (reader, idx) =>
            {
                if (reader.GetFieldType(idx) == typeof(decimal))
                {
                    if (reader.IsDBNull(idx))
                        return "0.00";
                    return reader.GetDecimal(idx).ToString(nfi);
                }
                if (reader.GetFieldType(idx) == typeof(Int32))
                {
                    if (reader.IsDBNull(idx))
                        return "0";
                    return reader.GetInt32(idx).ToString();
                }
                if (reader.GetFieldType(idx) == typeof(bool))
                {
                    if (reader.IsDBNull(idx))
                        return "0";
                    return reader.GetBoolean(idx)?"1":"0";
                }
                if (reader.GetFieldType(idx) == typeof(DateTime))
                {
                    if (reader.IsDBNull(idx))
                        return "";
                    return reader.GetDateTime(idx).ToShortDateString();
                }
                return reader[idx].ToString();
            };
            while (reader.Read())
            {
                // Row row = FindRow(firstChild, rowindex); ///too long
                Row row = new Row();
                for (int i = 0; i < fields; i++)
                {
                    Cell cell = new Cell();
                    //if(reader.GetFieldType(i)!=typeof(decimal))
                    cell.DataType = typeselector(reader.GetFieldType(i)); //CellValues.String;
                    cell.CellValue = new CellValue(formatvalue(reader, i));//[new CellValue(reader[i].ToString());
                    row.AppendChild(cell);
                }
                sheetData.AppendChild(row);
                rowindex++;
            }
        }
        private static WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);
           
            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
           
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;

        }
        private static Cell GetCell(Worksheet worksheet, string columnName, uint rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null)
                return null;

            return row.Elements<Cell>().Where(c => string.Compare
                   (c.CellReference.Value, columnName +
                   rowIndex, true) == 0).First();
        }


        // Given a worksheet and a row index, return the row.
        private static Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        public static void RemoveAllRows(WorksheetPart wssheatpart)
        {
            SheetData firstChild = wssheatpart.Worksheet.GetFirstChild<SheetData>();
            firstChild.RemoveAllChildren();
           // sheetData.Elements<Row>()
        }
         public static Row FindRow(SheetData sheetData, uint rowIndex)
        {


            Row newChild = null;
            uint index = rowIndex;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex.Value == rowIndex).Count<Row>() != 0)
            {
                return sheetData.Elements<Row>().Where(r => r.RowIndex.Value == rowIndex).First();
            }
            newChild = new Row();
            newChild.RowIndex = index;
            int num = 0;
            foreach (Row row2 in sheetData.Elements<Row>())
            {
                if (row2.RowIndex.Value > rowIndex)
                {
                    sheetData.InsertAt<Row>(newChild, num);
                    return newChild;
                }
                num++;
            }
            sheetData.AppendChild(newChild);
            return newChild;
        }
        static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

    }

}