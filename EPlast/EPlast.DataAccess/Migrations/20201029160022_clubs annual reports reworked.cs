using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class clubsannualreportsreworked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports");

            migrationBuilder.RenameColumn(
                name: "ClubID",
                table: "ClubAnnualReports",
                newName: "ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_ClubAnnualReports_ClubID",
                table: "ClubAnnualReports",
                newName: "IX_ClubAnnualReports_ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubId",
                table: "ClubAnnualReports",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubId",
                table: "ClubAnnualReports");

            migrationBuilder.RenameColumn(
                name: "ClubId",
                table: "ClubAnnualReports",
                newName: "ClubID");

            migrationBuilder.RenameIndex(
                name: "IX_ClubAnnualReports_ClubId",
                table: "ClubAnnualReports",
                newName: "IX_ClubAnnualReports_ClubID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
