using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRoleManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "mst_roles",
                keyColumn: "RoleID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 6, 39, 18, 686, DateTimeKind.Utc).AddTicks(2667));

            migrationBuilder.UpdateData(
                table: "mst_users",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "SuperAdmin", new DateTime(2024, 10, 28, 6, 39, 18, 686, DateTimeKind.Utc).AddTicks(2831) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "mst_roles",
                keyColumn: "RoleID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 10, 28, 5, 36, 11, 644, DateTimeKind.Utc).AddTicks(3798));

            migrationBuilder.UpdateData(
                table: "mst_users",
                keyColumn: "UserID",
                keyValue: 1,
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "Super Admin", new DateTime(2024, 10, 28, 5, 36, 11, 644, DateTimeKind.Utc).AddTicks(3999) });
        }
    }
}
