using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddAnnualReportCreators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorFatherName",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorFirstName",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorLastName",
                table: "RegionAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorFatherName",
                table: "ClubAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorFirstName",
                table: "ClubAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "ClubAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorLastName",
                table: "ClubAnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorFatherName",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorFirstName",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorLastName",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionAnnualReports_CreatorId",
                table: "RegionAnnualReports",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAnnualReports_CreatorId",
                table: "ClubAnnualReports",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubAnnualReports_AspNetUsers_CreatorId",
                table: "ClubAnnualReports",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAnnualReports_AspNetUsers_CreatorId",
                table: "RegionAnnualReports",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAnnualReports_AspNetUsers_CreatorId",
                table: "ClubAnnualReports");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAnnualReports_AspNetUsers_CreatorId",
                table: "RegionAnnualReports");

            migrationBuilder.DropIndex(
                name: "IX_RegionAnnualReports_CreatorId",
                table: "RegionAnnualReports");

            migrationBuilder.DropIndex(
                name: "IX_ClubAnnualReports_CreatorId",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorFatherName",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorFirstName",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorLastName",
                table: "RegionAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorFatherName",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorFirstName",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorLastName",
                table: "ClubAnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorFatherName",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorFirstName",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "CreatorLastName",
                table: "AnnualReports");
        }
    }
}
