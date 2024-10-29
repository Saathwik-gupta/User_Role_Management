using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserRoleManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class updated_newly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "mst_roles",
                keyColumn: "RoleID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "mst_roles",
                keyColumn: "RoleID",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "mst_roles",
                keyColumn: "RoleID",
                keyValue: 1,
                columns: new[] { "CreatedBy", "CreatedDate", "RoleName" },
                values: new object[] { "SuperAdmin", new DateTime(2024, 10, 28, 5, 36, 11, 644, DateTimeKind.Utc).AddTicks(3798), "SuperAdmin" });

            migrationBuilder.UpdateData(
                table: "mst_users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 5, 36, 11, 644, DateTimeKind.Utc).AddTicks(3999));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "mst_roles",
                keyColumn: "RoleID",
                keyValue: 1,
                columns: new[] { "CreatedBy", "CreatedDate", "RoleName" },
                values: new object[] { "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3272), "Super Admin" });

            migrationBuilder.InsertData(
                table: "mst_roles",
                columns: new[] { "RoleID", "CreatedBy", "CreatedDate", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { 2, "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3275), true, "Admin" },
                    { 3, "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3277), true, "User" }
                });

            migrationBuilder.UpdateData(
                table: "mst_users",
                keyColumn: "UserID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3418));
        }
    }
}
