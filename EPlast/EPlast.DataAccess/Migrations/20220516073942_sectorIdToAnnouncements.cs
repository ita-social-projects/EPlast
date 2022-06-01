using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class sectorIdToAnnouncements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "GoverningBodyAnnouncement",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncement_SectorId",
                table: "GoverningBodyAnnouncement",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningBodyAnnouncement_GoverningBodySectors_SectorId",
                table: "GoverningBodyAnnouncement",
                column: "SectorId",
                principalTable: "GoverningBodySectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningBodyAnnouncement_GoverningBodySectors_SectorId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.DropIndex(
                name: "IX_GoverningBodyAnnouncement_SectorId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "GoverningBodyAnnouncement");
        }
    }
}
