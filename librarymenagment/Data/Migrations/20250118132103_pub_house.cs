using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class pub_house : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublishingHouseId",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublishingHouseId",
                table: "Author",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PublishingHouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoundedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishingHouses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_PublishingHouseId",
                table: "Book",
                column: "PublishingHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Author_PublishingHouseId",
                table: "Author",
                column: "PublishingHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_PublishingHouses_PublishingHouseId",
                table: "Author",
                column: "PublishingHouseId",
                principalTable: "PublishingHouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_PublishingHouses_PublishingHouseId",
                table: "Book",
                column: "PublishingHouseId",
                principalTable: "PublishingHouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_PublishingHouses_PublishingHouseId",
                table: "Author");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_PublishingHouses_PublishingHouseId",
                table: "Book");

            migrationBuilder.DropTable(
                name: "PublishingHouses");

            migrationBuilder.DropIndex(
                name: "IX_Book_PublishingHouseId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Author_PublishingHouseId",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "PublishingHouseId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PublishingHouseId",
                table: "Author");
        }
    }
}
