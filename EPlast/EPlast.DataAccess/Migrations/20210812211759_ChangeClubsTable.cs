using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangeClubsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "PostIndex",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Clubs");

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "Clubs",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "Clubs");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Clubs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "Clubs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostIndex",
                table: "Clubs",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Clubs",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);
        }
    }
}
