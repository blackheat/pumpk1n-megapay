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

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("HashedPassword");

                    b.Property<string>("Nonce");

                    b.Property<DateTime>("RegisteredDate");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Products.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Image");

                    b.Property<string>("LongDescription");

                    b.Property<string>("Name");

                    b.Property<string>("ShortDescription");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("pumpk1n_backend.Models.Entities.Suppliers.Supplier", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Telephone");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Supplier");
                });
#pragma warning restore 612, 618
        }
    }
}
