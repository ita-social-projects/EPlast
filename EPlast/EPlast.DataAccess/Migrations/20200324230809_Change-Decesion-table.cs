using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangeDecesiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecesionStatus",
                table: "Decesions");

            migrationBuilder.AddColumn<int>(
                name: "DecesionStatusType",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HaveFile",
                table: "Decesions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecesionStatusType",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "HaveFile",
                table: "Decesions");

            migrationBuilder.AddColumn<int>(
                name: "DecesionStatus",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);
        }
    }
}
