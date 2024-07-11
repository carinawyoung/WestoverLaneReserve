﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WestoverLaneReserve.Data;

#nullable disable

namespace WestoverLaneReserve.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240711124946_InitialCreateV6")]
    partial class InitialCreateV6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("WestoverLaneReserve.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("WestoverLaneReserve.Models.LaneReservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("ReservationId");

                    b.ToTable("LaneReservation", (string)null);
                });

            modelBuilder.Entity("WestoverLaneReserve.Models.TimeSlotAvailability", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("Date")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("Time")
                        .HasColumnType("Time")
                        .HasColumnOrder(2);

                    b.Property<int>("LanesAvailable")
                        .HasColumnType("INTEGER");

                    b.HasKey("Date", "Time");

                    b.ToTable("TimeSlotAvailabilities");
                });
#pragma warning restore 612, 618
        }
    }
}
