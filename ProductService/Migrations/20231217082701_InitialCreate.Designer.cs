﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductService.Data;

#nullable disable

namespace UserService.Migrations
{
    [DbContext(typeof(ProductServiceDBContext))]
    [Migration("20231217082701_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProductService.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Availability")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("InStock")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WarrantyMonths")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.HasIndex("ProductTypeID");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductService.Models.ProductType", b =>
                {
                    b.Property<int>("ProductTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ProductTypeID");

                    b.HasIndex("TypeName")
                        .IsUnique();

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("ProductService.Models.Product", b =>
                {
                    b.HasOne("ProductService.Models.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductType");
                });
#pragma warning restore 612, 618
        }
    }
}
