using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class available : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableId",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Available",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPorosise = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statusi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Available", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_AvailableId",
                table: "Book",
                column: "AvailableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Available_AvailableId",
                table: "Book",
                column: "AvailableId",
                principalTable: "Available",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Available_AvailableId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Available");

            migrationBuilder.DropIndex(
                name: "IX_Book_AvailableId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "AvailableId",
                table: "Book");
        }
    }
}
