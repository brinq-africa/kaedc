using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class VendModel
    {
        public string MeterNumber { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Service { get; set; }
    }
}
