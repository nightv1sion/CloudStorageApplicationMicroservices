using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Files.Application.Common.Exceptions.Migrations
{
    /// <inheritdoc />
    public partial class FileUploadingStatusField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UploadingStatus",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadingStatus",
                table: "Files");
        }
    }
}
