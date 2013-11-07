using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupermarketDb.Data;
using System.Data.OleDb;
using SupermarketEF.Data;
using SupermarketEF.Model;
using System.Data.Entity;
using SupermarketEF.Data.Migrations;

namespace SupermarketDb.Client
{
    class Program
    {
        static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<
                    SupermarketDBContext, Configuration>());

            SeedDatabase();

            Utils.TestReportReader();
            PdfUtils.CreatePdf("../SalesReport.pdf");
            XmlUtils.CreateXml("../Sales-by-Vendors-report.xml");

            #region Test
            //SupermarketDbModel dbContextMySQL = new SupermarketDbModel();
            //SupermarketDBContext dbContextSQL = new SupermarketDBContext();
            //dbContextSQL.Measures.ToList();
            //dbContextSQL.SaveChanges();
            //dbContextSQL.SalesReports.ToList();
            #endregion
        }

        public static void SeedDatabase()
        {
            using (SupermarketDbModel mysql = new SupermarketDbModel())
            using (SupermarketDBContext sql = new SupermarketDBContext())
            {
                //sql.Database.Delete();

                foreach (var measure in mysql.Measures)
                {
                    if (!sql.Measures.Any(x=> x.MeasureId == measure.MeasureId))
                    {
                        SupermarketEF.Model.Measure measureToAdd = new SupermarketEF.Model.Measure
                        {
                            MeasureId = measure.MeasureId,
                            MeasureName = measure.MeasureName
                        };
                        sql.Measures.Add(measureToAdd);
                    }
                }

                sql.SaveChanges();

                foreach (var vendor in mysql.Vendors)
                {
                    if (!sql.Vendors.Any(x => x.VendorId == vendor.VendorId))
                    {
                        SupermarketEF.Model.Vendor vendorToAdd = new SupermarketEF.Model.Vendor
                        {
                            //Products = new HashSet<SupermarketEF.Model.Product>(),
                            VendorId = vendor.VendorId,
                            VendorName = vendor.VendorName
                        };

                        sql.Vendors.Add(vendorToAdd);
                    }

                    sql.SaveChanges();
                }

                foreach (var product in mysql.Products)
                {
                    if (!sql.Products.Any(x => x.ProductId == product.ProductId))
                    {
                        SupermarketEF.Model.Product productToAdd = new SupermarketEF.Model.Product
                        {
                            VendorId = product.VendorId,
                            ProductName = product.ProductName,
                            MeasureId = product.MeasureId,
                            BasePrice = product.BasePrice
                        };

                        sql.Products.Add(productToAdd);
                    }
                    sql.SaveChanges();
                }

            }
        }
    }
}
