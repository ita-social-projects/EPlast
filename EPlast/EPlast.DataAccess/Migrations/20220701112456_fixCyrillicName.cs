using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class fixCyrillicName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "СityURL",
                table: "RegionFollowers");

            migrationBuilder.AddColumn<string>(
                name: "CityURL",
                table: "RegionFollowers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityURL",
                table: "RegionFollowers");

            migrationBuilder.AddColumn<string>(
                name: "СityURL",
                table: "RegionFollowers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
