using System;
using System.Collections.Generic;

namespace kaedc.Models
{
    public partial class Service
    {
        public Service()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public sbyte IsDeleted { get; set; }
        public sbyte IsActive { get; set; }
        public string Wallet { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal ServiceProviderPercentage { get; set; }
        public decimal BrinqFullPercentage { get; set; }
        public decimal CoordinatorPercentage { get; set; }
        public decimal AgentPercentage { get; set; }
        public decimal MinimumSaleAmount { get; set; }
        public decimal MaxSaleAmount { get; set; }
        public string Imageurl { get; set; }
        public DateTime? Createdat { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updatedat { get; set; }
        public string Updatedby { get; set; }
        public int ServiceCategoryId { get; set; }
        public int? ConvenienceFee { get; set; }

        public ICollection<Transaction> Transaction { get; set; }
    }
}
