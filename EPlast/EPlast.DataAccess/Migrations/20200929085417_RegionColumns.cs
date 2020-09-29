using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RegionColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostIndex",
                table: "Regions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Regions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PostIndex",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Regions");
        }
    }
}
