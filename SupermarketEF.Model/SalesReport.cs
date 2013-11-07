﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupermarketEF;

namespace SupermarketEF.Model
{
    public class SalesReport
    {
        public int SalesReportId { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Sum { get; set; }

        public string Location { get; set; }

        public DateTime ReportDate { get; set; }

    }
}
