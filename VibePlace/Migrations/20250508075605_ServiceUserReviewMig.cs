using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibePlace.Migrations
{
    /// <inheritdoc />
    public partial class ServiceUserReviewMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "review",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserServiceId",
                table: "review",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_UserServiceId",
                table: "review",
                column: "UserServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_review_UserServices_UserServiceId",
                table: "review",
                column: "UserServiceId",
                principalTable: "UserServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_review_UserServices_UserServiceId",
                table: "review");

            migrationBuilder.DropIndex(
                name: "IX_review_UserServiceId",
                table: "review");

            migrationBuilder.DropColumn(
                name: "UserServiceId",
                table: "review");

            migrationBuilder.AlterColumn<int>(
                name: "PlaceId",
                table: "review",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
