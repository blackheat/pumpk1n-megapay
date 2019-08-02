using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_009_added_order_attributes_added_product_stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OutOfStock",
                table: "Product",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOutDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutOfStock",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CheckedOutDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "Order");
        }
    }
}
