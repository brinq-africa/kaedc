using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class AgentReportModel
    {
        public string Fullname { get; set; }

        public double TotalSales { get; set; }

        public double PrepaidTotalSales { get; set; }

        public double PostpaidTotalSales { get; set; }

        public decimal? PrepaidTotalProfit { get; set; }

        public decimal? PostpaidTotalProfit { get; set; }

        public string Location { get; set; }
    }
}
