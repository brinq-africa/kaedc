using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kaedc.Models
{
    public class ProductReportModel
    {
        public int PrepaidVolume { get; set; }
        public int PostpaidVolume { get; set; }
        public int RefundVolume { get; set; }
        public int CreditVolume { get; set; }
        public int DebitVolume { get; set; }
        public int TotalVolume { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double PrepaidSales { get; set; }
        public double PostpaidSales { get; set; }
        public double RefundSales { get; set; }
        public double CreditSales { get; set; }
        public double DebitSales { get; set; }
        public double TotalSales { get; set; }

        public decimal? BrinqPrepaidProfit { get; set; }
        public decimal? BrinqPostpaidProfit { get; set; }
        public decimal? BrinqTotalProfit { get; set; }

        public decimal? AgentPrepaidProfit { get; set; }
        public decimal? AgentPostpaidProfit { get; set; }
        public decimal? AgentTotalProfit { get; set; }
    }

    
}
