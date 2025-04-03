using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibePlace.Migrations
{
    /// <inheritdoc />
    public partial class ServicePlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_services_places_PlaceId",
                table: "services");

            migrationBuilder.DropIndex(
                name: "IX_services_PlaceId",
                table: "services");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "services");

            migrationBuilder.CreateTable(
                name: "ServiceToPlace",
                columns: table => new
                {
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    ServisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceToPlace", x => new { x.PlaceId, x.ServisId });
                    table.ForeignKey(
                        name: "FK_ServiceToPlace_places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceToPlace_services_ServisId",
                        column: x => x.ServisId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceToPlace_ServisId",
                table: "ServiceToPlace",
                column: "ServisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceToPlace");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_services_PlaceId",
                table: "services",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_services_places_PlaceId",
                table: "services",
                column: "PlaceId",
                principalTable: "places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
