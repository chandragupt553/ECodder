﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Data;

#nullable disable

namespace Project.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20240304055133_UpdatingCart")]
    partial class UpdatingCart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Project.Models.Cart", b =>
                {
                    b.Property<int>("PId")
                        .HasColumnType("int");

                    b.Property<int>("UId")
                        .HasColumnType("int");

                    b.Property<int>("Qty")
                        .HasColumnType("int");

                    b.HasKey("PId", "UId");

                    b.HasIndex("UId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("Project.Models.Customer", b =>
                {
                    b.Property<int>("UId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UId"));

                    b.Property<int>("MobileNumber")
                        .HasColumnType("int");

                    b.Property<string>("UEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UFName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ULName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Project.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int>("OrderStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderId");

                    b.HasIndex("OrderStatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Project.Models.OrderStatus", b =>
                {
                    b.Property<int>("OrderStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderStatusId"));

                    b.Property<string>("OStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderStatusId");

                    b.ToTable("OrderStatuses");
                });

            modelBuilder.Entity("Project.Models.PCategory", b =>
                {
                    b.Property<int>("CatgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CatgId"));

                    b.Property<string>("CatgName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CatgId");

                    b.ToTable("PCategories");
                });

            modelBuilder.Entity("Project.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PayTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<int>("UId")
                        .HasColumnType("int");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("Project.Models.Product", b =>
                {
                    b.Property<int>("PId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PId"));

                    b.Property<int>("CatgId")
                        .HasColumnType("int");

                    b.Property<string>("PName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("PId");

                    b.HasIndex("CatgId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Project.Models.Cart", b =>
                {
                    b.HasOne("Project.Models.Product", "productid")
                        .WithMany()
                        .HasForeignKey("PId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Project.Models.Customer", "customerid")
                        .WithMany()
                        .HasForeignKey("UId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customerid");

                    b.Navigation("productid");
                });

            modelBuilder.Entity("Project.Models.Order", b =>
                {
                    b.HasOne("Project.Models.OrderStatus", "OrderStatusid")
                        .WithMany()
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderStatusid");
                });

            modelBuilder.Entity("Project.Models.Payment", b =>
                {
                    b.HasOne("Project.Models.Order", "orderid")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Project.Models.Customer", "customerid")
                        .WithMany()
                        .HasForeignKey("UId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customerid");

                    b.Navigation("orderid");
                });

            modelBuilder.Entity("Project.Models.Product", b =>
                {
                    b.HasOne("Project.Models.PCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CatgId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
