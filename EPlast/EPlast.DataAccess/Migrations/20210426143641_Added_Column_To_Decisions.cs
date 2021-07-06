using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Added_Column_To_Decisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "GoverningBodyAdministrations",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Decesions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_UserId",
                table: "Decesions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_AspNetUsers_UserId",
                table: "Decesions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_AspNetUsers_UserId",
                table: "Decesions");

            migrationBuilder.DropIndex(
                name: "IX_Decesions_UserId",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Decesions");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GoverningBodyAdministrations",
                newName: "ID");
        }
    }
}
