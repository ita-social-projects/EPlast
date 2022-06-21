using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class editpropertyaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "RegionFollowers");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RegionFollowers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Cities",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "RegionFollowers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "RegionFollowers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Cities",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }
    }
}
