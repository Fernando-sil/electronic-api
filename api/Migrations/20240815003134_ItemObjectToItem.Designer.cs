﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.DataContext;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240815003134_ItemObjectToItem")]
    partial class ItemObjectToItem
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BrandCategory", b =>
                {
                    b.Property<int>("BrandsId")
                        .HasColumnType("int");

                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.HasKey("BrandsId", "CategoriesId");

                    b.HasIndex("CategoriesId");

                    b.ToTable("BrandCategory");
                });

            modelBuilder.Entity("api.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BrandName = "Apple"
                        },
                        new
                        {
                            Id = 2,
                            BrandName = "Acer"
                        },
                        new
                        {
                            Id = 3,
                            BrandName = "Dell"
                        },
                        new
                        {
                            Id = 4,
                            BrandName = "HP"
                        },
                        new
                        {
                            Id = 5,
                            BrandName = "Asus"
                        },
                        new
                        {
                            Id = 6,
                            BrandName = "Microsoft"
                        });
                });

            modelBuilder.Entity("api.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatePurchasePaid")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPurchasePaid")
                        .HasColumnType("bit");

                    b.Property<decimal?>("Total")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("api.Entities.CartItem", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(0);

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(1);

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("SubTotal")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("ItemId", "CartId");

                    b.HasIndex("CartId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("api.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Computers"
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Laptops"
                        },
                        new
                        {
                            Id = 3,
                            CategoryName = "Tablets"
                        },
                        new
                        {
                            Id = 4,
                            CategoryName = "Headphones & Speakers"
                        },
                        new
                        {
                            Id = 5,
                            CategoryName = "Cellphones"
                        });
                });

            modelBuilder.Entity("api.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("BrandId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasPrecision(7, 2)
                        .HasColumnType("decimal(7,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<double?>("Score")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("api.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("api.Entities.Specification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Spec")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Specifications");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Spec = "Colour"
                        },
                        new
                        {
                            Id = 2,
                            Spec = "Form factor"
                        },
                        new
                        {
                            Id = 3,
                            Spec = "Wireless communication technology"
                        },
                        new
                        {
                            Id = 4,
                            Spec = "Special feature"
                        },
                        new
                        {
                            Id = 5,
                            Spec = "Noise control"
                        },
                        new
                        {
                            Id = 6,
                            Spec = "Headphone jack"
                        },
                        new
                        {
                            Id = 7,
                            Spec = "Ear placement"
                        },
                        new
                        {
                            Id = 8,
                            Spec = "Model name"
                        },
                        new
                        {
                            Id = 9,
                            Spec = "Screen size"
                        },
                        new
                        {
                            Id = 10,
                            Spec = "Hard disk size"
                        },
                        new
                        {
                            Id = 11,
                            Spec = "RAM memory installed size"
                        },
                        new
                        {
                            Id = 12,
                            Spec = "Operating system"
                        },
                        new
                        {
                            Id = 13,
                            Spec = "Graphics card description"
                        },
                        new
                        {
                            Id = 14,
                            Spec = "Maximum display resolution"
                        },
                        new
                        {
                            Id = 15,
                            Spec = "CPU speed"
                        },
                        new
                        {
                            Id = 16,
                            Spec = "Shape"
                        });
                });

            modelBuilder.Entity("api.Entities.SpecificationItem", b =>
                {
                    b.Property<int>("SpecificationId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnOrder(1);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SpecificationId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("SpecificationItems");
                });

            modelBuilder.Entity("api.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActivationCode")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ActivationCodeExpires")
                        .HasColumnType("datetime2");

                    b.Property<bool>("ConfirmedEmail")
                        .HasColumnType("bit");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoinedOn")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BrandCategory", b =>
                {
                    b.HasOne("api.Entities.Brand", null)
                        .WithMany()
                        .HasForeignKey("BrandsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.Entities.Cart", b =>
                {
                    b.HasOne("api.Entities.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("api.Entities.CartItem", b =>
                {
                    b.HasOne("api.Entities.Cart", null)
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Item", "Item")
                        .WithMany("CartItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("api.Entities.Item", b =>
                {
                    b.HasOne("api.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId");

                    b.HasOne("api.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("api.Entities.Rating", b =>
                {
                    b.HasOne("api.Entities.Item", "Item")
                        .WithMany("Ratings")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("api.Entities.SpecificationItem", b =>
                {
                    b.HasOne("api.Entities.Item", null)
                        .WithMany("ItemSpecifications")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Entities.Specification", null)
                        .WithMany("ItemSpecifications")
                        .HasForeignKey("SpecificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.Entities.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("api.Entities.Item", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("ItemSpecifications");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("api.Entities.Specification", b =>
                {
                    b.Navigation("ItemSpecifications");
                });

            modelBuilder.Entity("api.Entities.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
