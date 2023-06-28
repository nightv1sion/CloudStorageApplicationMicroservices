using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Files.API.Migrations
{
    /// <inheritdoc />
    public partial class DirectoriesTableIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "directories_id_uindex",
                table: "Directories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "directories_id_uindex",
                table: "Directories");
        }
    }
}
