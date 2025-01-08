using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Author_Authorid",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_Authorid",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Authorid",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Author");

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "Author",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bio",
                table: "Author");

            migrationBuilder.AddColumn<int>(
                name: "Authorid",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Author",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Authorid",
                table: "Category",
                column: "Authorid");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Author_Authorid",
                table: "Category",
                column: "Authorid",
                principalTable: "Author",
                principalColumn: "id");
        }
    }
}
