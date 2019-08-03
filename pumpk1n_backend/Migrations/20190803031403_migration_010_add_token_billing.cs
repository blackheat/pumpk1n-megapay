using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_010_add_token_billing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserTokenTransaction",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TokenBilling",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserTokenTransactionId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GatewayInvoiceId = table.Column<string>(nullable: true),
                    GatewayInvoiceReferenceLink = table.Column<string>(nullable: true),
                    ReceivedAmount = table.Column<double>(nullable: false),
                    InvoiceFullyPaid = table.Column<bool>(nullable: false),
                    GatewayStatus = table.Column<string>(nullable: true),
                    GatewayInvoiceSecret = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CancelledDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenBilling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenBilling_UserTokenTransaction_UserTokenTransactionId",
                        column: x => x.UserTokenTransactionId,
                        principalTable: "UserTokenTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenBilling_UserTokenTransactionId",
                table: "TokenBilling",
                column: "UserTokenTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenBilling");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserTokenTransaction");
        }
    }
}
