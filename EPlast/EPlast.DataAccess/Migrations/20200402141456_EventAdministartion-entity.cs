using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class EventAdministartionentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EventAdministration",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventAdministration_UserId",
                table: "EventAdministration",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserId",
                table: "EventAdministration",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAdministration_AspNetUsers_UserId",
                table: "EventAdministration");

            migrationBuilder.DropIndex(
                name: "IX_EventAdministration_UserId",
                table: "EventAdministration");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EventAdministration");
        }
    }
}
