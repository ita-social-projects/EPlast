using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Addednewpropertiestoclubsannualreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_ClubAnnualReports_ClubAnnualReportID",
                table: "ClubAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_ClubAnnualReports_ClubAnnualReportID",
                table: "ClubMembers");

            migrationBuilder.DropIndex(
                name: "IX_ClubMembers_ClubAnnualReportID",
                table: "ClubMembers");

            migrationBuilder.DropIndex(
                name: "IX_ClubAdministrations_ClubAnnualReportID",
                table: "ClubAdministrations");

            migrationBuilder.DropColumn(
                name: "ClubAnnualReportID",
                table: "ClubMembers");

            migrationBuilder.DropColumn(
                name: "ClubAnnualReportID",
                table: "ClubAdministrations");

            migrationBuilder.AlterColumn<string>(
                name: "ForWhom",
                table: "Events",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ClubMembersSummary",
                table: "ClubAnnualReports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubMembersSummary",
                table: "ClubAnnualReports");

            migrationBuilder.AlterColumn<string>(
                name: "ForWhom",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "ClubAnnualReportID",
                table: "ClubMembers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubAnnualReportID",
                table: "ClubAdministrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_ClubAnnualReportID",
                table: "ClubMembers",
                column: "ClubAnnualReportID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubAnnualReportID",
                table: "ClubAdministrations",
                column: "ClubAnnualReportID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAdministrations_ClubAnnualReports_ClubAnnualReportID",
                table: "ClubAdministrations",
                column: "ClubAnnualReportID",
                principalTable: "ClubAnnualReports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubMembers_ClubAnnualReports_ClubAnnualReportID",
                table: "ClubMembers",
                column: "ClubAnnualReportID",
                principalTable: "ClubAnnualReports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
