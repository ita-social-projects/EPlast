using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class addforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserId",
                table: "EventAdministration");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EventAdministration",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_EventAdministration_UserId",
                table: "EventAdministration",
                newName: "IX_EventAdministration_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserID",
                table: "EventAdministration",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserID",
                table: "EventAdministration");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "EventAdministration",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EventAdministration_UserID",
                table: "EventAdministration",
                newName: "IX_EventAdministration_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserId",
                table: "EventAdministration",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
