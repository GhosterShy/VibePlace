using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibePlace.Migrations
{
    /// <inheritdoc />
    public partial class MakeRatingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingPlace_AspNetUsers_UserId",
                table: "RatingPlace");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingPlace_places_PlaceId",
                table: "RatingPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingPlace",
                table: "RatingPlace");

            migrationBuilder.RenameTable(
                name: "RatingPlace",
                newName: "ratings");

            migrationBuilder.RenameIndex(
                name: "IX_RatingPlace_UserId",
                table: "ratings",
                newName: "IX_ratings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RatingPlace_PlaceId",
                table: "ratings",
                newName: "IX_ratings_PlaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ratings",
                table: "ratings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_AspNetUsers_UserId",
                table: "ratings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_places_PlaceId",
                table: "ratings",
                column: "PlaceId",
                principalTable: "places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ratings_AspNetUsers_UserId",
                table: "ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_ratings_places_PlaceId",
                table: "ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ratings",
                table: "ratings");

            migrationBuilder.RenameTable(
                name: "ratings",
                newName: "RatingPlace");

            migrationBuilder.RenameIndex(
                name: "IX_ratings_UserId",
                table: "RatingPlace",
                newName: "IX_RatingPlace_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ratings_PlaceId",
                table: "RatingPlace",
                newName: "IX_RatingPlace_PlaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingPlace",
                table: "RatingPlace",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingPlace_AspNetUsers_UserId",
                table: "RatingPlace",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingPlace_places_PlaceId",
                table: "RatingPlace",
                column: "PlaceId",
                principalTable: "places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
