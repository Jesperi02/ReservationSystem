﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReservationSystem22.Models;

#nullable disable

namespace ReservationSystem22.Migrations
{
    [DbContext(typeof(ReservationContext))]
    [Migration("20221117102556_Testi1711")]
    partial class Testi1711
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ReservationSystem22.Models.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TargetId")
                        .HasColumnType("bigint");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TargetId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("ReservationSystem22.Models.Item", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long>("accessCount")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ReservationSystem22.Models.Reservation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("TargetId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TargetId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("ReservationSystem22.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ReservationSystem22.Models.Image", b =>
                {
                    b.HasOne("ReservationSystem22.Models.Item", "Target")
                        .WithMany("Images")
                        .HasForeignKey("TargetId");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("ReservationSystem22.Models.Item", b =>
                {
                    b.HasOne("ReservationSystem22.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ReservationSystem22.Models.Reservation", b =>
                {
                    b.HasOne("ReservationSystem22.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("ReservationSystem22.Models.Item", "Target")
                        .WithMany()
                        .HasForeignKey("TargetId");

                    b.Navigation("Owner");

                    b.Navigation("Target");
                });

            modelBuilder.Entity("ReservationSystem22.Models.Item", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}