using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_012_modified_tokenbilling_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "UserTokenTransaction",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "UserTokenTransaction");
        }
    }
}
