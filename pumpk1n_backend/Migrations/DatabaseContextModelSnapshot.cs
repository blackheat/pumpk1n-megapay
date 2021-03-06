﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using pumpk1n_backend.Models.DatabaseContexts;

namespace pumpk1n_backend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Accounts.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Balance");

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("HashedPassword");

                    b.Property<string>("Nonce");

                    b.Property<DateTime>("RegisteredDate");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserType");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Orders.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Address");

                    b.Property<DateTime>("CancelledDate");

                    b.Property<DateTime>("CheckedOutDate");

                    b.Property<DateTime>("ConfirmedDate");

                    b.Property<long>("CustomerId");

                    b.Property<string>("CustomerName");

                    b.Property<string>("Notes");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Orders.OrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("OrderId");

                    b.Property<long>("ProductId");

                    b.Property<long>("Quantity");

                    b.Property<float>("SinglePrice");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Products.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<bool>("Deprecated");

                    b.Property<string>("Image");

                    b.Property<string>("LongDescription");

                    b.Property<string>("Name");

                    b.Property<bool>("OutOfStock");

                    b.Property<float>("Price");

                    b.Property<string>("ShortDescription");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Products.ProductInventory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CustomerId");

                    b.Property<DateTime>("ExportedDate");

                    b.Property<DateTime>("ImportedDate");

                    b.Property<long>("ProductId");

                    b.Property<string>("ProductUniqueIdentifier");

                    b.Property<long>("SupplierId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("ProductInventory");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Suppliers.Supplier", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<string>("Address");

                    b.Property<bool>("Deprecated");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Telephone");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Tokens.TokenBilling", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CancelledDate");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("GatewayInvoiceId");

                    b.Property<string>("GatewayInvoiceReferenceLink");

                    b.Property<string>("GatewayInvoiceSecret");

                    b.Property<string>("GatewayStatus");

                    b.Property<bool>("InvoiceFullyPaid");

                    b.Property<string>("Name");

                    b.Property<double>("ReceivedAmount");

                    b.Property<long>("UserTokenTransactionId");

                    b.HasKey("Id");

                    b.HasIndex("UserTokenTransactionId");

                    b.ToTable("TokenBilling");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Tokens.UserTokenTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedDate");

                    b.Property<float>("Amount");

                    b.Property<DateTime>("CancelledDate");

                    b.Property<DateTime>("ConfirmedDate");

                    b.Property<long>("CustomerId");

                    b.Property<bool>("IsPurchaseRequest");

                    b.Property<string>("Notes");

                    b.Property<int>("TransactionType");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("UserTokenTransaction");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Orders.Order", b =>
                {
                    b.HasOne("pumpk1n_backend.Models.Entities.Accounts.User", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Orders.OrderItem", b =>
                {
                    b.HasOne("pumpk1n_backend.Models.Entities.Orders.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("pumpk1n_backend.Models.Entities.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Products.ProductInventory", b =>
                {
                    b.HasOne("pumpk1n_backend.Models.Entities.Accounts.User", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("pumpk1n_backend.Models.Entities.Products.Product", "Product")
                        .WithMany("ProductInventories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("pumpk1n_backend.Models.Entities.Suppliers.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Tokens.TokenBilling", b =>
                {
                    b.HasOne("pumpk1n_backend.Models.Entities.Tokens.UserTokenTransaction", "UserTokenTransaction")
                        .WithMany("TokenBillings")
                        .HasForeignKey("UserTokenTransactionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Tokens.UserTokenTransaction", b =>
                {
                    b.HasOne("pumpk1n_backend.Models.Entities.Accounts.User", "Customer")
                        .WithMany("UserTokenTransactions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
