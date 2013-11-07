using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SupermarketEF.Model;
using SupermarketEF.Data;

namespace SupermarketDb.Client
{
    public static class XmlUtils
    {
        public static void CreateXml(string filePath)
        {
            using (SupermarketDBContext sqldb = new SupermarketDBContext())
            {
                string format = "dd-MMM-yyyy";

                XElement sales = new XElement("sales");
                foreach (var vendor in sqldb.Vendors.ToList().Where(
                    x=> x.Products.Any(y => y.SalesReports.Count > 0)))
                {
                    XElement elSale = new XElement("sale");
                    elSale.SetAttributeValue("vendor", vendor.VendorName.ToString());
                    foreach (var product in vendor.Products.ToList())
                    {
                        foreach (var report in product.SalesReports.ToList())
                        {
                            XElement summary = new XElement("summary");
                            summary.SetAttributeValue("date", report.ReportDate.ToString(format));
                            summary.SetAttributeValue("total-sum", report.Sum.ToString());
                            elSale.Add(summary);
                        }
                    }

                    sales.Add(elSale);
                }
                sales.Save(filePath);
            }
        }
    }
}
