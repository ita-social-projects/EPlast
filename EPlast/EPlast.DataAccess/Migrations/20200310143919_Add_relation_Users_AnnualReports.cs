using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_relation_Users_AnnualReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_UserId",
                table: "AnnualReports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AspNetUsers_UserId",
                table: "AnnualReports");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_UserId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AnnualReports");
        }
    }
}
