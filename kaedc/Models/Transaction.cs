using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace kaedc.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int PaymentMethodId { get; set; }
        public double Amount { get; set; }
        public string KaedcUser { get; set; }
        public DateTime Datetime { get; set; }
        public string MeterName { get; set; }
        public string Meternumber { get; set; }
        public string TransactionReference { get; set; }
        public string Hash { get; set; }
        public string PayersName { get; set; }
        public string ApiUniqueReference { get; set; }
        public string GatewayresponseCode { get; set; }
        public string GatewayresponseMessage { get; set; }
        public int Statuscode { get; set; }
        public string StatusMessage { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public string Token { get; set; }
        public string PhcnUnique { get; set; }
        public string PayerIp { get; set; }
        public decimal? AgentProfit { get; set; }
        public decimal? CoordinatorProfit { get; set; }
        public decimal? BrinqProfit { get; set; }
        public string TopUpValue { get; set; }
        public string transactionsStatus { get; set; }

        public Kaedcuser KaedcUserNavigation { get; set; }
        public Paymentmethod PaymentMethod { get; set; }
        public Service Service { get; set; }
    }
}
