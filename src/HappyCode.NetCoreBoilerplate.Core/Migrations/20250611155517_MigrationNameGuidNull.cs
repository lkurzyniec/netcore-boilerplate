using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyCode.NetCoreBoilerplate.Core.Migrations
{
    /// <inheritdoc />
    public partial class MigrationNameGuidNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "DeptId",
                schema: "employees",
                table: "employees",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "DeptId",
                schema: "employees",
                table: "employees",
                type: "uniqueidentifier",
                maxLength: 4,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
