using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Oblast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "UserTableObjects");

            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "UserTableObjects",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "UserTableObjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "Oblast",
                table: "UserProfiles",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Oblast",
                table: "Cities",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "UserTableObjects");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "UserTableObjects");

            migrationBuilder.DropColumn(
                name: "Oblast",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Oblast",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "UserTableObjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
