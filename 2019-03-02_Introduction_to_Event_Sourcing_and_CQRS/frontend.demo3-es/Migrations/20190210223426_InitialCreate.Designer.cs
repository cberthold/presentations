﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using frontend.Data;

namespace frontend.Migrations
{
    [DbContext(typeof(BankAccountsContext))]
    [Migration("20190210223426_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("frontend.Data.Account", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName");

                    b.Property<decimal>("CurrentBalance");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("frontend.Data.Deposit", b =>
                {
                    b.Property<Guid>("DepositId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("Date");

                    b.HasKey("DepositId");

                    b.HasIndex("AccountId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("frontend.Data.Withdrawal", b =>
                {
                    b.Property<Guid>("WithdrawalId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("Date");

                    b.HasKey("WithdrawalId");

                    b.HasIndex("AccountId");

                    b.ToTable("Withdrawals");
                });

            modelBuilder.Entity("frontend.Data.Deposit", b =>
                {
                    b.HasOne("frontend.Data.Account", "Account")
                        .WithMany("Deposits")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("frontend.Data.Withdrawal", b =>
                {
                    b.HasOne("frontend.Data.Account", "Account")
                        .WithMany("Withdrawals")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
