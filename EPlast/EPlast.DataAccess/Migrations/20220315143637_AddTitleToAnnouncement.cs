using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddTitleToAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Titile",
                table: "SectorAnnouncement",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titile",
                table: "GoverningBodyAnnouncement",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Titile",
                table: "SectorAnnouncement");

            migrationBuilder.DropColumn(
                name: "Titile",
                table: "GoverningBodyAnnouncement");
        }
    }
}
