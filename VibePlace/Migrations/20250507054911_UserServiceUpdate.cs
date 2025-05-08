using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibePlace.Migrations
{
    /// <inheritdoc />
    public partial class UserServiceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "UserServices");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "UserServices",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "UserServices");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "UserServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
