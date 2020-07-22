using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class IsApprovedCityMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CityMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CityMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
