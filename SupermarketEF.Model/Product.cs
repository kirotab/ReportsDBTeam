using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketEF.Model
{
    public class Product
    {
        private ICollection<SalesReport> salesReport;

        public Product()
        {
            this.salesReport = new HashSet<SalesReport>();
        }

        public virtual ICollection<SalesReport> SalesReports
        {
            get { return this.salesReport; }
            set { this.salesReport = value; }
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public int MeasureId { get; set; }
        public virtual Measure Measure { get; set; }

        public decimal BasePrice { get; set; }
    }
}
