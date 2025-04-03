using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibePlace.Migrations
{
    /// <inheritdoc />
    public partial class Services : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLink",
                table: "places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "places",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "addDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLink",
                table: "places");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "places");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "places");

            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "addDate",
                table: "AspNetUsers");
        }
    }
}
