using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_002_trimmed_down_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalUser");

            migrationBuilder.DropColumn(
                name: "ActivatedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "GoogleOAuthProfileId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordResetCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordResetCodeIssuedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordResetCodeUsedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserActivationCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserActivationCodeIssuedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserProfileCompleted",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActivatedDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleOAuthProfileId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetCode",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetCodeIssuedDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetCodeUsedDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhoneNumberConfirmedDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserActivationCode",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserActivationCodeIssuedDate",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserProfileCompleted",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "InternalUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true),
                    Nonce = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UserIsRoot = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalUser", x => x.Id);
                });
        }
    }
}
