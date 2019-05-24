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

        public decimal PrepaidSales { get; set; }
        public decimal PostpaidSales { get; set; }
        public decimal RefundSales { get; set; }
        public decimal CreditSales { get; set; }
        public decimal DebitSales { get; set; }
        public decimal TotalSales { get; set; }

        public decimal BrinqPrepaidProfit { get; set; }
        public decimal BrinqPostpaidProfit { get; set; }
        public decimal BrinqRefundProfit { get; set; }
        public decimal BrinqCreditProfit { get; set; }
        public decimal BrinqDebitProfit { get; set; }
        public decimal BrinqTotalProfit { get; set; }

        public decimal AgentPrepaidProfit { get; set; }
        public decimal AgentPostpaidProfit { get; set; }
        public decimal AgentRefundProfit { get; set; }
        public decimal AgentCreditProfit { get; set; }
        public decimal AgentDebitProfit { get; set; }
        public decimal AgentTotalProfit { get; set; }
    }
}
