using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExcelLibrary
{


    public class Document
    {
        public byte[] Content { get; set; }
        public string Name { get; set; }
    }


    public interface IExcelReader
    {
        List<Dictionary<string, object>> readFrom(Document document);
    }

    public class ExcelReader : IExcelReader
    {
        private ExcelDataSetConfiguration  excelDataSetConfiguration = new ExcelDataSetConfiguration()
            {
                UseColumnDataType = true,
                FilterSheet = (tableReader, sheetIndex) => true,
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    EmptyColumnNamePrefix = "Column",
                    UseHeaderRow = true,
                    FilterRow = (rowReader) => true,
                    FilterColumn = (rowReader, columnIndex) => true
                }
            };
        


        public List<Dictionary<string, object>> readFrom(Document document)
        {
            using var stream = new MemoryStream(document.Content);
            using var reader = document.Name.EndsWith(".csv")
                ? ExcelReaderFactory.CreateCsvReader(stream, getConfiCsv())
                : ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet(excelDataSetConfiguration);
                    
            var serializeObject = JsonConvert.SerializeObject(result, Formatting.Indented);
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(serializeObject);
            var sheet1 = deserializeObject.Values.First();


            var listColumns =
                sheet1
                    .Children<JObject>()
                    .Select(x => x.ToObject<Dictionary<string, object>>()).ToList();
            return listColumns;
        }

        private ExcelReaderConfiguration getConfiCsv()
        {
          return   new ExcelReaderConfiguration()
            {
                FallbackEncoding = Encoding.GetEncoding(1252),
                Password = "password",
                AutodetectSeparators = new char[] {',', ';', '\t', '|', '#'},
                LeaveOpen = false,
                AnalyzeInitialCsvRows = 0,
            };

        }
    }

    class Program
    {
        static void Main(string[] args)
        {System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            string fileName = "/home/enner/Downloads/prueba.csv";
            byte[] file = File.ReadAllBytes(fileName);
            var document = new Document() {Content = file,Name ="prueba.csv" };

            var excelReader = new ExcelReader();
            var readFrom = excelReader.readFrom(document);
        }
    }
}