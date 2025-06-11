using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyCode.NetCoreBoilerplate.Core.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "employees");

            migrationBuilder.CreateTable(
                name: "departments",
                schema: "employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeptName = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    MangerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DeptId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.Id);
                    table.ForeignKey(
                        name: "employees_ibfk_1",
                        column: x => x.DeptId,
                        principalSchema: "employees",
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_MangerId",
                schema: "employees",
                table: "departments",
                column: "MangerId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_DeptId",
                schema: "employees",
                table: "employees",
                column: "DeptId");

            migrationBuilder.AddForeignKey(
                name: "departments_ibfk_1",
                schema: "employees",
                table: "departments",
                column: "MangerId",
                principalSchema: "employees",
                principalTable: "employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "departments_ibfk_1",
                schema: "employees",
                table: "departments");

            migrationBuilder.DropTable(
                name: "employees",
                schema: "employees");

            migrationBuilder.DropTable(
                name: "departments",
                schema: "employees");
        }
    }
}
