using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ApproveTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCityAdmin",
                table: "ConfirmedUsers");

            migrationBuilder.DropColumn(
                name: "isClubAdmin",
                table: "ConfirmedUsers");

            migrationBuilder.AddColumn<int>(
                name: "ApproveType",
                table: "ConfirmedUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveType",
                table: "ConfirmedUsers");

            migrationBuilder.AddColumn<bool>(
                name: "isCityAdmin",
                table: "ConfirmedUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isClubAdmin",
                table: "ConfirmedUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
