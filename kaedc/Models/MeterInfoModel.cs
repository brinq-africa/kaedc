using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class MeterInfoModel
    {
        public string MeterNumber { get; set; }
        public decimal Amount { get; set; }
        public string Service { get; set; }
        public string PhoneNumber { get; set; }
    }
}
