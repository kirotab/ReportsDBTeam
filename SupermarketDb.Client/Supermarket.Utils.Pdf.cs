using iTextSharp.text;
using iTextSharp.text.pdf;
using SupermarketEF.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDb.Client
{
    class PdfUtils
    {
        public static void CreatePdf(string path)
        {
            Document myDoc = new Document(PageSize.A4.Rotate());
            iTextSharp.text.pdf.PdfWriter.GetInstance(myDoc, new FileStream(path, FileMode.Create));
            myDoc.Open();
            PdfPTable table = new PdfPTable(5);

            table.TotalWidth = 800f;
            table.LockedWidth = true;
            Font font = new Font(Font.FontFamily.COURIER, 20f, 1);
            font.SetStyle("bold");

            PdfPCell cell = new PdfPCell(new Phrase("Aggregated Sales Report", font));
            cell.Colspan = 5;
            cell.Indent = 50;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 5;
            cell.BackgroundColor = new BaseColor(229, 228, 226);
            table.AddCell(cell);

            using (SupermarketDBContext sqlContext = new SupermarketDBContext())
            {
				//MAGIC QUERRY
                //var salesRep = ( from sr in sqlContext.SalesReports
                //                 join p in sqlContext.Products
                //                 on	sr.ProductId equals p.ProductId
                //                 select new { sr, p.ProductName }).Distinct().GroupBy(x => x.sr.ReportDate);

                var salesReports = sqlContext.SalesReports.GroupBy(x => x.ReportDate).ToList();
                decimal grandTotal = 0m;
                
                foreach (var reports in salesReports)
                {
					string format = "dd-MMM-yyyy";
                    var date = reports.First().ReportDate.ToString(format);
                    PdfPCell dateCell = new PdfPCell(
                        new Phrase(date, font));
                    
                    dateCell.Colspan = 5;
                    dateCell.Indent = 50;
                    dateCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dateCell.Padding = 5;
                    dateCell.BackgroundColor = new BaseColor(229, 228, 226);
                    table.AddCell(dateCell);


                    foreach (var report in reports)
                    {
                        string productId = report.Product.ProductName.ToString();
                        string quantity = report.Quantity.ToString();
                        string unitPrice = report.UnitPrice.ToString();
                        string location = report.Location.ToString();
                        string sum = report.Sum.ToString();

                        table.AddCell(productId);
                        table.AddCell(quantity);
                        table.AddCell(unitPrice);
                        table.AddCell(location);
                        table.AddCell(sum);

                    }
                    PdfPCell footrCell = new PdfPCell(new Phrase("Total sum for " + date));
                    footrCell.Colspan = 4;
                    footrCell.Indent = 50;
                    footrCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    footrCell.Padding = 5;
                    table.AddCell(footrCell);
                    //PdfPCell totalCell = new PdfPCell();
                    decimal reportSum = reports.Sum(x => x.Sum);
                    grandTotal += reportSum;
                    table.AddCell(reportSum.ToString());
                }

                PdfPCell grandFootrCell = new PdfPCell(new Phrase("Grand total:"));
                grandFootrCell.Colspan = 4;
                grandFootrCell.Indent = 50;
                grandFootrCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                grandFootrCell.Padding = 5;
                table.AddCell(grandFootrCell);

                table.AddCell(new PdfPCell(new Phrase(grandTotal.ToString(),font)));
                myDoc.Add(table);
            }

            myDoc.Close();
        }

    }
}
