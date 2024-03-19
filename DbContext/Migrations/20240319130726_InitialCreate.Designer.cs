﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NBP.Context;

#nullable disable

namespace NBP.Context.Migrations
{
    [DbContext(typeof(NbpContext))]
    [Migration("20240319130726_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("NBP.Context.Models.ExchangeRateTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("timestamp");

                    b.Property<string>("TableType")
                        .IsRequired()
                        .HasColumnType("varchar(1)");

                    b.HasKey("Id");

                    b.ToTable("ExchangeRateTable");
                });

            modelBuilder.Entity("NBP.Context.Models.ExchangeRateValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<int>("ExchangeRateTableId")
                        .HasColumnType("integer");

                    b.Property<double>("Mid")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeRateTableId");

                    b.ToTable("ExchangeRateValue");
                });

            modelBuilder.Entity("NBP.Context.Models.ExchangeRateValue", b =>
                {
                    b.HasOne("NBP.Context.Models.ExchangeRateTable", "ExchangeRateTable")
                        .WithMany("ExchangeRates")
                        .HasForeignKey("ExchangeRateTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangeRateTable");
                });

            modelBuilder.Entity("NBP.Context.Models.ExchangeRateTable", b =>
                {
                    b.Navigation("ExchangeRates");
                });
#pragma warning restore 612, 618
        }
    }
}
