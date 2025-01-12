using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AbstarctModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Category",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Category",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Category",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Author",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Author",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Author",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "Author",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Author",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Author",
                newName: "LastName");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Book",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Author",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Author");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Category",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Category",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Category",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Author",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Author",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Author",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Author",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Author",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Author",
                newName: "last_name");
        }
    }
}
