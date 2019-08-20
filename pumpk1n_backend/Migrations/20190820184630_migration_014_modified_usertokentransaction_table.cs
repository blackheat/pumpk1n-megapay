using Microsoft.EntityFrameworkCore.Migrations;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_014_modified_usertokentransaction_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPurchaseRequest",
                table: "UserTokenTransaction",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPurchaseRequest",
                table: "UserTokenTransaction");
        }
    }
}
