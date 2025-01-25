using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace librarymenagment.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Statusi",
                table: "Available",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Available",
                newName: "Statusi");
        }
    }
}
