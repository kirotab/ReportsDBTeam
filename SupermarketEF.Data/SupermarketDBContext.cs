using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SupermarketEF.Model;

namespace SupermarketEF.Data
{
    public class SupermarketDBContext : DbContext
    {
        public SupermarketDBContext()
            : base("SupermarketEFConn")
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Measure> Measures { get; set; }

        public DbSet<SalesReport> SalesReports { get; set; }
    }
}
