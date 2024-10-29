using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserRoleManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mst_roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "mst_users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mst_users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_mst_users_mst_roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "mst_roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "mst_roles",
                columns: new[] { "RoleID", "CreatedBy", "CreatedDate", "IsActive", "RoleName" },
                values: new object[,]
                {
                    { 1, "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3272), true, "Super Admin" },
                    { 2, "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3275), true, "Admin" },
                    { 3, "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3277), true, "User" }
                });

            migrationBuilder.InsertData(
                table: "mst_users",
                columns: new[] { "UserID", "CreatedBy", "CreatedDate", "Email", "IsActive", "MobileNumber", "Password", "RoleID", "UserName" },
                values: new object[] { 1, "Super Admin", new DateTime(2024, 10, 25, 7, 7, 20, 495, DateTimeKind.Utc).AddTicks(3418), "superadmin@example.com", true, "1234567890", "d3535b78e24867f3c850fec1c8591b7a469b8f8683be64d835057cb9fd204aa1", 1, "superadmin" });

            migrationBuilder.CreateIndex(
                name: "IX_mst_users_RoleID",
                table: "mst_users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mst_users");

            migrationBuilder.DropTable(
                name: "mst_roles");
        }
    }
}
