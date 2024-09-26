using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JwtAuthenticationBackend.Migrations
{
    /// <inheritdoc />
    public partial class ApplicatioUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af9b6a7b-47d9-40cb-8b04-29ed0907ae0e");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1e993afa-ee0c-4472-ae83-8b9f0c50cb8a", "f545ad10-4e3e-4220-9e5c-4809df8d993b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e993afa-ee0c-4472-ae83-8b9f0c50cb8a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f545ad10-4e3e-4220-9e5c-4809df8d993b");

            migrationBuilder.AddColumn<string>(
                name: "Expiration",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Token",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e993afa-ee0c-4472-ae83-8b9f0c50cb8a", "95d3f0ed-5b9f-4c73-a9d4-93aec13526ad", "Administrator", "ADMINISTRATOR" },
                    { "af9b6a7b-47d9-40cb-8b04-29ed0907ae0e", "854a9c9d-f18e-4530-98b1-f5a496b23e39", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f545ad10-4e3e-4220-9e5c-4809df8d993b", 0, "afee4c15-1140-4b28-88ec-ec1b296ec304", "admin@provider.com", true, false, null, "ADMIN@PROVIDER.COM", "ADMINISTRATOR", "AQAAAAIAAYagAAAAEGR+xw8HVnf2dExeT2JXNJ4x1Udp7LTExCqHc4c2VpPB5j1ay3r9j6FkjuQPzA0iEA==", "9111111111", true, "3c155b9f-7af7-4345-a70c-1a303b67f432", false, "Administrator" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1e993afa-ee0c-4472-ae83-8b9f0c50cb8a", "f545ad10-4e3e-4220-9e5c-4809df8d993b" });
        }
    }
}
