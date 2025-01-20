using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class commentUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Coments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Coments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Coments_AspNetUsers_UserId",
                table: "Coments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
