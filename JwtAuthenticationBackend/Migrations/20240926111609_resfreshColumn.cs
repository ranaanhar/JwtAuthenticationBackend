using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JwtAuthenticationBackend.Migrations
{
    /// <inheritdoc />
    public partial class resfreshColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0133a14b-eb61-4758-bb70-bb39443e7265");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f1dc7e17-bc0a-4f51-8b8c-48f428eeee5a", "965f3dbf-9b36-4c18-9731-f7f7983d4f82" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1dc7e17-bc0a-4f51-8b8c-48f428eeee5a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "965f3dbf-9b36-4c18-9731-f7f7983d4f82");

            migrationBuilder.DropColumn(
                name: "Expiration",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "AspNetUsers",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiration",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "337dfa67-3502-468a-8a70-2578aecea406", "fe220800-00a0-4b19-8cce-8edfd8750852", "User", "USER" },
                    { "e1489453-9f71-4847-a74e-66affcba4f71", "a0b81f8a-c325-4b48-8fce-b43faf037d3c", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiration", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1f16564f-d6f0-4512-b769-ad0269c476f3", 0, "b5ef3ae8-c6fc-4818-9fb3-515705dcd238", "admin@provider.com", true, false, null, "ADMIN@PROVIDER.COM", "ADMINISTRATOR", "AQAAAAIAAYagAAAAEDTUkt6MyYh0dI+fIANI6ReSOMkrdW07EspmUnOqH815paXAf2q/3Kn7qEuLXR5nTA==", "9111111111", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3546044c-3912-4bda-b444-e6a095ca5a3c", false, "Administrator" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e1489453-9f71-4847-a74e-66affcba4f71", "1f16564f-d6f0-4512-b769-ad0269c476f3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "337dfa67-3502-468a-8a70-2578aecea406");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e1489453-9f71-4847-a74e-66affcba4f71", "1f16564f-d6f0-4512-b769-ad0269c476f3" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1489453-9f71-4847-a74e-66affcba4f71");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f16564f-d6f0-4512-b769-ad0269c476f3");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiration",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "AspNetUsers",
                newName: "Token");

            migrationBuilder.AddColumn<string>(
                name: "Expiration",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0133a14b-eb61-4758-bb70-bb39443e7265", "70b0cfbd-4c8d-4872-a04b-0cf1e5d0df4c", "User", "USER" },
                    { "f1dc7e17-bc0a-4f51-8b8c-48f428eeee5a", "3ebf29e5-2da9-4d72-bb80-612ee19cd07e", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "Expiration", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Token", "TwoFactorEnabled", "UserName" },
                values: new object[] { "965f3dbf-9b36-4c18-9731-f7f7983d4f82", 0, "8d4d3d32-ca46-4394-a9c0-b764b2d91e8f", "admin@provider.com", true, null, false, null, "ADMIN@PROVIDER.COM", "ADMINISTRATOR", "AQAAAAIAAYagAAAAEPZgiO12EPloweQ4fW/gSq+0+YO8RB1gplwh9kFIijnYI0qzAmWr87fPIV3J1mLPuw==", "9111111111", true, "52ca8aa6-b26d-44bd-a9a7-40bd53307748", null, false, "Administrator" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f1dc7e17-bc0a-4f51-8b8c-48f428eeee5a", "965f3dbf-9b36-4c18-9731-f7f7983d4f82" });
        }
    }
}
