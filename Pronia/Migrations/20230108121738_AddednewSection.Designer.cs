﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pronia.DAL;

#nullable disable

namespace Pronia.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230108121738_AddednewSection")]
    partial class AddednewSection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Pronia.Models.Banner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondaryTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Banners");
                });

            modelBuilder.Entity("Pronia.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Pronia.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Pronia.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Pronia.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Pronia.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Pronia.Models.ProductColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductColors");
                });

            modelBuilder.Entity("Pronia.Models.ProductImages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsCover")
                        .HasColumnType("bit");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("Pronia.Models.ProductInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AboutReturnRequest")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Guarantee")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Shipping")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("ProductInformations");
                });

            modelBuilder.Entity("Pronia.Models.ProductSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductSizes");
                });

            modelBuilder.Entity("Pronia.Models.ShippingArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ShippingAreas");
                });

            modelBuilder.Entity("Pronia.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("Pronia.Models.Slider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("PrimaryTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondaryTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sliders");
                });

            modelBuilder.Entity("Pronia.Models.ProductColor", b =>
                {
                    b.HasOne("Pronia.Models.Color", "Color")
                        .WithMany("Colors")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pronia.Models.Product", "Product")
                        .WithMany("ProductColors")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Pronia.Models.ProductImages", b =>
                {
                    b.HasOne("Pronia.Models.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Pronia.Models.ProductInformation", b =>
                {
                    b.HasOne("Pronia.Models.Product", "Product")
                        .WithOne("ProductInformations")
                        .HasForeignKey("Pronia.Models.ProductInformation", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Pronia.Models.ProductSize", b =>
                {
                    b.HasOne("Pronia.Models.Product", "Product")
                        .WithMany("ProductSizes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pronia.Models.Size", "Size")
                        .WithMany("Sizes")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("Pronia.Models.Color", b =>
                {
                    b.Navigation("Colors");
                });

            modelBuilder.Entity("Pronia.Models.Product", b =>
                {
                    b.Navigation("ProductColors");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductInformations");

                    b.Navigation("ProductSizes");
                });

            modelBuilder.Entity("Pronia.Models.Size", b =>
                {
                    b.Navigation("Sizes");
                });
#pragma warning restore 612, 618
        }
    }
}
