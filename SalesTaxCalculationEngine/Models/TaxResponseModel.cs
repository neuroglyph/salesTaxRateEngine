using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxCalculationEngine.Models
{
    public class TaxResponseModel
    {
        public string County { get; set; }
        public double TransactionAmount { get; set; }
        public double SalesTaxTotal { get; set; }
    }
}
