using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketEF.Model
{
    public class Measure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MeasureId { get; set; }

        public string MeasureName { get; set; }

    }
}
