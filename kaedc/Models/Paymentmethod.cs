using System;
using System.Collections.Generic;

namespace kaedc.Models
{
    public partial class Paymentmethod
    {
        public Paymentmethod()
        {
            Kaedcuser = new HashSet<Kaedcuser>();
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public sbyte? IsActive { get; set; }
        public DateTime? Createdat { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updatedat { get; set; }
        public string Updatedby { get; set; }

        public ICollection<Kaedcuser> Kaedcuser { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
    }
}
