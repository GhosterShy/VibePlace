using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibePlace.Migrations
{
    /// <inheritdoc />
    public partial class ReviewLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "reviewLike",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviewLike", x => new { x.UserId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_reviewLike_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviewLike_review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "review",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_places_CityId",
                table: "places",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_reviewLike_ReviewId",
                table: "reviewLike",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_places_cities_CityId",
                table: "places",
                column: "CityId",
                principalTable: "cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_places_cities_CityId",
                table: "places");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "reviewLike");

            migrationBuilder.DropIndex(
                name: "IX_places_CityId",
                table: "places");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "places");
        }
    }
}
