using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class clubsannualreportsinitialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports");

            migrationBuilder.AlterColumn<int>(
                name: "ClubID",
                table: "ClubAnnualReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports");

            migrationBuilder.AlterColumn<int>(
                name: "ClubID",
                table: "ClubAnnualReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_Clubs_ClubID",
                table: "ClubAnnualReports",
                column: "ClubID",
                principalTable: "Clubs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
