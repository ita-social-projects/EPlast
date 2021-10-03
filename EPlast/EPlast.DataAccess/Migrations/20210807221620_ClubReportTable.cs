using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ClubReportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubAdminContacts",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "ClubContacts",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "ClubMembersSummary",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "ClubPage",
                table: "ClubAnnualReports");

            migrationBuilder.AddColumn<string>(
                name: "ClubURL",
                table: "ClubAnnualReports",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ClubAnnualReports",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ClubAnnualReports",
                maxLength: 18,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "ClubAnnualReports",
                maxLength: 60,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubURL",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "ClubAnnualReports");

            migrationBuilder.AddColumn<string>(
                name: "ClubAdminContacts",
                table: "ClubAnnualReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubContacts",
                table: "ClubAnnualReports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubMembersSummary",
                table: "ClubAnnualReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubPage",
                table: "ClubAnnualReports",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
