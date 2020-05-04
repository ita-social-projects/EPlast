using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ApproverConfirmedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvers_AspNetUsers_UserId",
                table: "Approvers");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmedUsers_AspNetUsers_UserId",
                table: "ConfirmedUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ConfirmedUsers",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmedUsers_UserId",
                table: "ConfirmedUsers",
                newName: "IX_ConfirmedUsers_UserID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Approvers",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Approvers_UserId",
                table: "Approvers",
                newName: "IX_Approvers_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvers_AspNetUsers_UserID",
                table: "Approvers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmedUsers_AspNetUsers_UserID",
                table: "ConfirmedUsers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Approvers_AspNetUsers_UserID",
                table: "Approvers");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmedUsers_AspNetUsers_UserID",
                table: "ConfirmedUsers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ConfirmedUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmedUsers_UserID",
                table: "ConfirmedUsers",
                newName: "IX_ConfirmedUsers_UserId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Approvers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Approvers_UserID",
                table: "Approvers",
                newName: "IX_Approvers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvers_AspNetUsers_UserId",
                table: "Approvers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmedUsers_AspNetUsers_UserId",
                table: "ConfirmedUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
