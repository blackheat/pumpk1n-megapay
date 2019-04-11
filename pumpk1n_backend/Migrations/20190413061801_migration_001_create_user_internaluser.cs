using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace pumpk1n_backend.Migrations
{
    public partial class migration_001_create_user_internaluser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Nonce = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true),
                    UserIsRoot = table.Column<bool>(nullable: false),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    GoogleOAuthProfileId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmedDate = table.Column<DateTime>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Nonce = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UserProfileCompleted = table.Column<bool>(nullable: false),
                    UserActivationCode = table.Column<string>(nullable: true),
                    UserActivationCodeIssuedDate = table.Column<DateTime>(nullable: true),
                    ActivatedDate = table.Column<DateTime>(nullable: true),
                    PasswordResetCode = table.Column<string>(nullable: true),
                    PasswordResetCodeIssuedDate = table.Column<DateTime>(nullable: true),
                    PasswordResetCodeUsedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalUser");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
