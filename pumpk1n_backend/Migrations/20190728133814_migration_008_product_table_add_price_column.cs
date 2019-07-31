using Microsoft.EntityFrameworkCore.Migrations;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_008_product_table_add_price_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Product",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");
        }
    }
}
