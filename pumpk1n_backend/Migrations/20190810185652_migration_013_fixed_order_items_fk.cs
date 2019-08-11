using Microsoft.EntityFrameworkCore.Migrations;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_013_fixed_order_items_fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductInventory_ProductInventoryId",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductInventoryId",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductInventoryId",
                table: "OrderItem");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Product_ProductId",
                table: "OrderItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Product_ProductId",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductId",
                table: "OrderItem");

            migrationBuilder.AddColumn<long>(
                name: "ProductInventoryId",
                table: "OrderItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductInventoryId",
                table: "OrderItem",
                column: "ProductInventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductInventory_ProductInventoryId",
                table: "OrderItem",
                column: "ProductInventoryId",
                principalTable: "ProductInventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
