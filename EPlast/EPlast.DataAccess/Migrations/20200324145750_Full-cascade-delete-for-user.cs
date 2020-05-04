using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Fullcascadedeleteforuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvers_AspNetUsers_UserId",
                table: "Approvers");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAdministrations_AspNetUsers_UserId",
                table: "RegionAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvers_AspNetUsers_UserId",
                table: "Approvers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAdministrations_AspNetUsers_UserId",
                table: "RegionAdministrations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvers_AspNetUsers_UserId",
                table: "Approvers");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAdministrations_AspNetUsers_UserId",
                table: "RegionAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvers_AspNetUsers_UserId",
                table: "Approvers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_AspNetUsers_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAdministrations_AspNetUsers_UserId",
                table: "RegionAdministrations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                table: "UnconfirmedCityMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
