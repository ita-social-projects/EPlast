using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class clubannualreportcontextcreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubAnnualReportID",
                table: "ClubMembers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubAnnualReportID",
                table: "ClubAdministrations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClubAnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false),
                    CurrentClubMembers = table.Column<int>(nullable: false),
                    CurrentClubFollowers = table.Column<int>(nullable: false),
                    ClubEnteredMembersCount = table.Column<int>(nullable: false),
                    ClubLeftMembersCount = table.Column<int>(nullable: false),
                    ClubCenters = table.Column<string>(maxLength: 200, nullable: true),
                    ClubContacts = table.Column<string>(maxLength: 200, nullable: true),
                    ClubPage = table.Column<string>(maxLength: 200, nullable: true),
                    KbUSPWishes = table.Column<string>(maxLength: 500, nullable: true),
                    ClubID = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAnnualReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubAnnualReports_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_ClubAnnualReportID",
                table: "ClubMembers",
                column: "ClubAnnualReportID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubAnnualReportID",
                table: "ClubAdministrations",
                column: "ClubAnnualReportID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAnnualReports_ClubID",
                table: "ClubAnnualReports",
                column: "ClubID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubAdministrations_ClubAnnualReports_ClubAnnualReportID",
                table: "ClubAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClubMembers_ClubAnnualReports_ClubAnnualReportID",
                table: "ClubMembers");

            migrationBuilder.DropTable(
                name: "ClubAnnualReports");

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
        }
    }
}
