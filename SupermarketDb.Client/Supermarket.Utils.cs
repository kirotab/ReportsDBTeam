using System;
using System.Linq;
using System.Data.OleDb;
using SupermarketEF.Data;
using SupermarketEF.Model;
using System.Data;
using System.IO;
using System.Threading;
using System.Globalization;
using Ionic.Zip;
namespace SupermarketDb.Client
{
    public static class Utils
    {
        public static void ExtractZipFIle(string zipfilePath, string unzippedDirectoryPath)
        {
            using (ZipFile zip = new ZipFile(zipfilePath))
            {
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract(unzippedDirectoryPath, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        public static void TestReportReader()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            SupermarketDBContext sqldb = new SupermarketDBContext();

            ExtractZipFIle(@"../Sample-Sales-Reports.zip", @"../Reports");
            ParseExcelDirectory(sqldb, @"../Reports/");
        }

        public static void ParseExcelFile(SupermarketDBContext sqlDb, string folderDate, string file)
        {
            string connectStr = @"Provider=Microsoft.ACE.OLEDB.12.0; " +
                                @"Data Source=" + file + "; " +
                                @"Extended Properties='Excel 12.0 Xml; HDR=YES'";

            OleDbConnection dbCon = new OleDbConnection(connectStr);
            string sqlQuery = "select * from [Sales$]";
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(sqlQuery, dbCon);
            DataSet currentData = new DataSet();
            myDataAdapter.Fill(currentData, "Sales");

            DataRowCollection excelRows = currentData.Tables["Sales"].Rows;
            string location = string.Empty;
            DateTime saleDate = DateTime.Parse(folderDate);

            location = excelRows[0][0].ToString();
            for (int row = 2; row < excelRows.Count - 1; row++)
            {
                int productId = 0;
                int quantity = 0;
                decimal unitPrice = 0;
                decimal sum = 0;

                int.TryParse(excelRows[row][0].ToString(), out productId);
                int.TryParse(excelRows[row][1].ToString(), out quantity);
                decimal.TryParse(excelRows[row][2].ToString(), out unitPrice);
                decimal.TryParse(excelRows[row][3].ToString(), out sum);

                if (productId != 0)
                {
                    SalesReport currentReport = new SalesReport
                    {
                        Location = location,
                        ProductId = productId,
                        Quantity = quantity,
                        Sum = sum,
                        UnitPrice = unitPrice,
                        ReportDate = saleDate
                    };

                    sqlDb.SalesReports.Add(currentReport);
                    sqlDb.SaveChanges();
                }
            }
        }

        public static void ParseExcelDirectory(SupermarketDBContext sqldb, string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            foreach (var subDirectory in dir.GetDirectories())
            {
                FileInfo[] files = subDirectory.GetFiles();
                //DateTime folderDate = DateTime.Parse(subDirectory.Name);

                foreach (var item in files)
                {
                    Console.WriteLine(item.FullName);
                    ParseExcelFile(sqldb, subDirectory.Name, item.FullName);
                    sqldb.SaveChanges();
                    Console.WriteLine();
                }
            }
        }
    }
}
