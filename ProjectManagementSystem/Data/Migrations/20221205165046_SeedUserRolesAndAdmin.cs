using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class SeedUserRolesAndAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c3b7db5-c6a5-4541-a40c-c311eeac28a4", "1c3b7db5-c6a5-4541-a40c-c311eeac28a4", "Manager", "MANAGER" },
                    { "528ab890-6ba2-4144-83bf-c806cdb19eb1", "528ab890-6ba2-4144-83bf-c806cdb19eb1", "ProjectManager", "PROJECTMANAGER" },
                    { "e8eab2c6-5f73-403c-a252-3bb6fa039091", "e8eab2c6-5f73-403c-a252-3bb6fa039091", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "ProfilePicture", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3dd00985-a76e-4049-b41d-012bbb8a2fc2", 0, "7bf9f060-4fe9-428f-97d7-9a386b7f5135", "admin@pms.bg", true, false, null, "Leeroy", null, "PMSADMIN", "AQAAAAEAACcQAAAAELNiGwHoNOEWzD9DQG+4SkR+ou/zpAWvZKPQtlS7Mp+Kp1YIpJuVmy8XIGgH0fV3nw==", null, false, "Employee", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "3ddce88a-c08a-49ca-bca4-9f26b7d4b2f7", "Jenkins", false, "pmsadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1c3b7db5-c6a5-4541-a40c-c311eeac28a4", "3dd00985-a76e-4049-b41d-012bbb8a2fc2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "528ab890-6ba2-4144-83bf-c806cdb19eb1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8eab2c6-5f73-403c-a252-3bb6fa039091");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1c3b7db5-c6a5-4541-a40c-c311eeac28a4", "3dd00985-a76e-4049-b41d-012bbb8a2fc2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c3b7db5-c6a5-4541-a40c-c311eeac28a4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3dd00985-a76e-4049-b41d-012bbb8a2fc2");
        }
    }
}
