using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RegionAnnualReportAddedQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Characteristic",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChurchCooperation",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fundraising",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImportantNeeds",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvolvementOfVolunteers",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProblemSituations",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicFunding",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialProjects",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateOfPreparation",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusOfStrategy",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuccessStories",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainedNeeds",
                table: "RegionAnnualReports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Characteristic",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "ChurchCooperation",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "Fundraising",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "ImportantNeeds",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "InvolvementOfVolunteers",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "ProblemSituations",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "PublicFunding",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "SocialProjects",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "StateOfPreparation",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "StatusOfStrategy",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "SuccessStories",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "TrainedNeeds",
                table: "RegionAnnualReports");
        }
    }
}
