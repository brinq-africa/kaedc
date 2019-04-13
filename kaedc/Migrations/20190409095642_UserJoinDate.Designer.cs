﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kaedc.Models;

namespace kaedc.Migrations
{
    [DbContext(typeof(Kaedc))]
    [Migration("20190409095642_UserJoinDate")]
    partial class UserJoinDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("kaedc.Models.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BankAccountName");

                    b.Property<string>("BankAccountNumber");

                    b.Property<string>("BankName");

                    b.Property<DateTime?>("Createdat");

                    b.Property<string>("Createdby");

                    b.Property<sbyte?>("IsDeleted");

                    b.Property<int>("KaedcUser");

                    b.Property<string>("KaedcUserNavigationId");

                    b.Property<DateTime?>("Updatedat");

                    b.Property<string>("Updatedby");

                    b.HasKey("Id");

                    b.HasIndex("KaedcUserNavigationId");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("kaedc.Models.Images", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Createdat");

                    b.Property<string>("Createdby");

                    b.Property<string>("Description");

                    b.Property<sbyte>("IsDeleted");

                    b.Property<int>("KaedcUser");

                    b.Property<string>("KaedcUserNavigationId");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("Updatedat");

                    b.Property<string>("Updatedby");

                    b.HasKey("Id");

                    b.HasIndex("KaedcUserNavigationId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("kaedc.Models.Kaedcuser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<decimal?>("BonusBalance");

                    b.Property<string>("BrinqaccountNumber");

                    b.Property<string>("CompanyAddress");

                    b.Property<string>("CompanyName");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("EncryptionKey");

                    b.Property<string>("Firstname");

                    b.Property<sbyte?>("IsActive");

                    b.Property<decimal?>("LoanBalance");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<decimal?>("MainBalance");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("PreferredPaymentMethod");

                    b.Property<int?>("PreferredPaymentMethodNavigationId");

                    b.Property<string>("Privatekey");

                    b.Property<string>("PublicKey");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("PreferredPaymentMethodNavigationId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("kaedc.Models.Paymentmethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("Createdat");

                    b.Property<sbyte?>("IsActive");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("Updatedat");

                    b.Property<string>("Updatedby");

                    b.HasKey("Id");

                    b.ToTable("Paymentmethod");
                });

            modelBuilder.Entity("kaedc.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AgentPercentage");

                    b.Property<int>("BrinqFullPercentage");

                    b.Property<int?>("ConvenienceFee");

                    b.Property<int>("CoordinatorPercentage");

                    b.Property<DateTime?>("Createdat");

                    b.Property<string>("Createdby");

                    b.Property<string>("Description");

                    b.Property<string>("Imageurl");

                    b.Property<sbyte>("IsActive");

                    b.Property<sbyte>("IsDeleted");

                    b.Property<decimal>("MaxSaleAmount");

                    b.Property<decimal>("MinimumSaleAmount");

                    b.Property<string>("Name");

                    b.Property<int>("ServiceCategoryId");

                    b.Property<int>("ServiceProviderPercentage");

                    b.Property<DateTime?>("Updatedat");

                    b.Property<string>("Updatedby");

                    b.Property<string>("Wallet");

                    b.Property<decimal>("WalletBalance");

                    b.HasKey("Id");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("kaedc.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal?>("AgentProfit");

                    b.Property<string>("Amount");

                    b.Property<string>("ApiUniqueReference");

                    b.Property<decimal?>("BrinqProfit");

                    b.Property<decimal?>("CoordinatorProfit");

                    b.Property<DateTime>("Datetime");

                    b.Property<string>("GatewayresponseCode");

                    b.Property<string>("GatewayresponseMessage");

                    b.Property<string>("Hash");

                    b.Property<int>("KaedcUser");

                    b.Property<string>("KaedcUserNavigationId");

                    b.Property<string>("MeterName");

                    b.Property<string>("Meternumber");

                    b.Property<string>("PayerIp");

                    b.Property<string>("PayersName");

                    b.Property<int>("PaymentMethodId");

                    b.Property<string>("PhcnUnique");

                    b.Property<string>("RecipientPhoneNumber");

                    b.Property<int>("ServiceId");

                    b.Property<string>("StatusMessage");

                    b.Property<int>("Statuscode");

                    b.Property<string>("Token");

                    b.Property<string>("TopUpValue");

                    b.Property<string>("TransactionReference");

                    b.HasKey("Id");

                    b.HasIndex("KaedcUserNavigationId");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("kaedc.Models.Bank", b =>
                {
                    b.HasOne("kaedc.Models.Kaedcuser", "KaedcUserNavigation")
                        .WithMany("Bank")
                        .HasForeignKey("KaedcUserNavigationId");
                });

            modelBuilder.Entity("kaedc.Models.Images", b =>
                {
                    b.HasOne("kaedc.Models.Kaedcuser", "KaedcUserNavigation")
                        .WithMany("Images")
                        .HasForeignKey("KaedcUserNavigationId");
                });

            modelBuilder.Entity("kaedc.Models.Kaedcuser", b =>
                {
                    b.HasOne("kaedc.Models.Paymentmethod", "PreferredPaymentMethodNavigation")
                        .WithMany("Kaedcuser")
                        .HasForeignKey("PreferredPaymentMethodNavigationId");
                });

            modelBuilder.Entity("kaedc.Models.Transaction", b =>
                {
                    b.HasOne("kaedc.Models.Kaedcuser", "KaedcUserNavigation")
                        .WithMany("Transaction")
                        .HasForeignKey("KaedcUserNavigationId");

                    b.HasOne("kaedc.Models.Paymentmethod", "PaymentMethod")
                        .WithMany("Transaction")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("kaedc.Models.Service", "Service")
                        .WithMany("Transaction")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("kaedc.Models.Kaedcuser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("kaedc.Models.Kaedcuser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("kaedc.Models.Kaedcuser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("kaedc.Models.Kaedcuser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
