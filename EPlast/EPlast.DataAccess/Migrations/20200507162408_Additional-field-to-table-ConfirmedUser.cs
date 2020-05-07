using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AdditionalfieldtotableConfirmedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCityAdmin",
                table: "ConfirmedUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isClubAdmin",
                table: "ConfirmedUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCityAdmin",
                table: "ConfirmedUsers");

            migrationBuilder.DropColumn(
                name: "isClubAdmin",
                table: "ConfirmedUsers");
        }
    }
}
