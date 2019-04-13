using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace kaedc.Models
{
    public partial class Kaedcuser : IdentityUser
    {
        public Kaedcuser()
        {
            Bank = new HashSet<Bank>();
            Images = new HashSet<Images>();
            Transaction = new HashSet<Transaction>();
        }

        //public int Id { get; set; }
        public string BrinqaccountNumber { get; set; }
        public decimal? MainBalance { get; set; }
        public decimal? LoanBalance { get; set; }
        public decimal? BonusBalance { get; set; }
        public sbyte? IsActive { get; set; }
        public int PreferredPaymentMethod { get; set; }
        public string PublicKey { get; set; }
        public string Privatekey { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string EncryptionKey { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedAt { get; set; }

        public Paymentmethod PreferredPaymentMethodNavigation { get; set; }
        public ICollection<Bank> Bank { get; set; }
        public ICollection<Images> Images { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
    }
}
