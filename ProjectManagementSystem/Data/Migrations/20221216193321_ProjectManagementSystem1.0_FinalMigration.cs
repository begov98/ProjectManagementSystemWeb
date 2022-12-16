using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystem.Data.Migrations
{
    public partial class ProjectManagementSystem10_FinalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "59d73192-6892-4201-b245-f613b261cde8", "59d73192-6892-4201-b245-f613b261cde8", "Manager", "MANAGER" },
                    { "b3283af5-35f3-4262-9686-afd6ddb58011", "b3283af5-35f3-4262-9686-afd6ddb58011", "ProjectManager", "PROJECTMANAGER" },
                    { "b4cca62d-375b-4079-997e-65afde222f64", "b4cca62d-375b-4079-997e-65afde222f64", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "ProfilePicture", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4e9ba706-e6ce-4737-b8a1-572ace671f9c", 0, "d9ee4311-6172-485c-b11e-aaa1d8d35b47", "admin@pms.bg", true, false, null, "Leeroy", null, "PMSADMIN", "AQAAAAEAACcQAAAAEDki5AqWGACSaE0X0c/Jb4JEDRweO3BF+khl8+7GYD9wQ7Zsa+8h86GtYO9NzyfMbQ==", null, false, "Employee", new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "f1897cb9-bf5f-4067-bd3f-aeb2cc3a8bbd", "Jenkins", false, "pmsadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "59d73192-6892-4201-b245-f613b261cde8", "4e9ba706-e6ce-4737-b8a1-572ace671f9c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3283af5-35f3-4262-9686-afd6ddb58011");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4cca62d-375b-4079-997e-65afde222f64");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "59d73192-6892-4201-b245-f613b261cde8", "4e9ba706-e6ce-4737-b8a1-572ace671f9c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59d73192-6892-4201-b245-f613b261cde8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4e9ba706-e6ce-4737-b8a1-572ace671f9c");

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
    }
}
