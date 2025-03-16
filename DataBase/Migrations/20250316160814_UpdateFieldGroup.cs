using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBase.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Group",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Group",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Group_Member",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Group_Member",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Group_Level",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Group_Level",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Group_Member");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Group_Member");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Group_Level");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Group_Level");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Group",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Group",
                newName: "CreatedBy");
        }
    }
}
