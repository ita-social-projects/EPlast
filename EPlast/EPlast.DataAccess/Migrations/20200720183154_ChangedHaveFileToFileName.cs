using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangedHaveFileToFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HaveFile",
                table: "Decesions");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Decesions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Decesions");

            migrationBuilder.AddColumn<bool>(
                name: "HaveFile",
                table: "Decesions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
