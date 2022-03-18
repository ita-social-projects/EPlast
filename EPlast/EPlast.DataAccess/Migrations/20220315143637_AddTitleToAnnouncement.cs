using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddTitleToAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SectorAnnouncement",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "GoverningBodyAnnouncement",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "SectorAnnouncement");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "GoverningBodyAnnouncement");
        }
    }
}
