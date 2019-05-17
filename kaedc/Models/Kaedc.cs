using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace kaedc.Models
{
    public partial class Kaedc : IdentityDbContext<Kaedcuser>
    {
        public Kaedc()
        {
        }

        public Kaedc(DbContextOptions<Kaedc> options)
            : base(options)
        {
        }

        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Kaedcuser> Kaedcuser { get; set; }
        public virtual DbSet<Paymentmethod> Paymentmethod { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseMySql("Server=74.208.128.248;User Id=brinq;Password=N_wjo881;Database=kaedc; default command timeout=120;");
                optionsBuilder.UseMySql("Server=localhost;User Id=root;Password=root;Database=kaedc; default command timeout=120;");
            }
        }

    }
}
