using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    public partial class SeededDefaultUserandRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "18850188-64b9-494f-9a55-b099c587a0f5", "8d0a8218-fab1-4eba-b2e0-29c2e01efc82", "User", "USER" },
                    { "6c774d80-3f64-4ef6-b6b1-b4728e670b91", "c57cd692-83ad-4d5e-9bf3-751bfad8fa6f", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "39e52d14-3c85-4d59-8bb9-e770392debd4", 0, "a1b063e9-a5ed-47c4-90a6-816791042a88", "admin@bookstore.com", false, "system", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEPhi2uGJbMbqZjuxX+kguzZCZRv6HB0I1RbuQX96HLO395n71M8RhWbpBIsoozZmRQ==", null, false, "ff2f74ee-5356-45cb-8a63-a588dfc972cf", false, "admin@bookstore.com" },
                    { "f99d93e0-c8e1-445d-b58b-c71229875de1", 0, "5099f190-8cae-4cf2-a124-f3c4f66ba606", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEKosfpWHoBQJd77HeH8lu3p8NMe43IjvEY0ZenANY4weF49JF09Ys+kPzahasXww5Q==", null, false, "e0564966-5047-4c3d-a1e5-9ad7569fa3f8", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "18850188-64b9-494f-9a55-b099c587a0f5", "39e52d14-3c85-4d59-8bb9-e770392debd4" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "6c774d80-3f64-4ef6-b6b1-b4728e670b91", "f99d93e0-c8e1-445d-b58b-c71229875de1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "18850188-64b9-494f-9a55-b099c587a0f5", "39e52d14-3c85-4d59-8bb9-e770392debd4" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6c774d80-3f64-4ef6-b6b1-b4728e670b91", "f99d93e0-c8e1-445d-b58b-c71229875de1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18850188-64b9-494f-9a55-b099c587a0f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c774d80-3f64-4ef6-b6b1-b4728e670b91");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "39e52d14-3c85-4d59-8bb9-e770392debd4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f99d93e0-c8e1-445d-b58b-c71229875de1");
        }
    }
}
