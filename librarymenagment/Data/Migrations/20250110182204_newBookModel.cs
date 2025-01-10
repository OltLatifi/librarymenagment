using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class newBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Book_BookId",
                table: "Author");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Book_BookId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_BookId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Author_BookId",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Author");

            migrationBuilder.AlterColumn<int>(
                name: "Copies",
                table: "Book",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorId",
                table: "Book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_CategoryId",
                table: "Book",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorId",
                table: "Book",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Category_CategoryId",
                table: "Book",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Author_AuthorId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_Category_CategoryId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_CategoryId",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Copies",
                table: "Book",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Author",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_BookId",
                table: "Category",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Author_BookId",
                table: "Author",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Book_BookId",
                table: "Author",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Book_BookId",
                table: "Category",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id");
        }
    }
}
