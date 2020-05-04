using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class CascadedeleteUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserId",
                table: "ClubMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserId",
                table: "ClubMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserId",
                table: "ClubMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_AspNetUsers_UserId",
                table: "CityMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_AspNetUsers_UserId",
                table: "ClubMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
