using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddClubRepotrTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubMemberHistory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    IsFollower = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMemberHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubMemberHistory_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubMemberHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportAdmins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    ClubAdministrationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportAdmins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportAdmins_ClubAdministrations_ClubAdministrationId",
                        column: x => x.ClubAdministrationId,
                        principalTable: "ClubAdministrations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubReportAdmins_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportPlastDegrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    PlastDegreeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportPlastDegrees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportPlastDegrees_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubReportPlastDegrees_PlastDegrees_PlastDegreeId",
                        column: x => x.PlastDegreeId,
                        principalTable: "PlastDegrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubReportPlastDegrees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubReportMember",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    ClubMemberHistoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportMember", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportMember_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubReportMember_ClubMemberHistory_ClubMemberHistoryId",
                        column: x => x.ClubMemberHistoryId,
                        principalTable: "ClubMemberHistory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberHistory_ClubId",
                table: "ClubMemberHistory",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMemberHistory_UserId",
                table: "ClubMemberHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportAdmins_ClubAdministrationId",
                table: "ClubReportAdmins",
                column: "ClubAdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportAdmins_ClubAnnualReportId",
                table: "ClubReportAdmins",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportMember_ClubAnnualReportId",
                table: "ClubReportMember",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportMember_ClubMemberHistoryId",
                table: "ClubReportMember",
                column: "ClubMemberHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportPlastDegrees_ClubAnnualReportId",
                table: "ClubReportPlastDegrees",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportPlastDegrees_PlastDegreeId",
                table: "ClubReportPlastDegrees",
                column: "PlastDegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportPlastDegrees_UserId",
                table: "ClubReportPlastDegrees",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubReportAdmins");

            migrationBuilder.DropTable(
                name: "ClubReportMember");

            migrationBuilder.DropTable(
                name: "ClubReportPlastDegrees");

            migrationBuilder.DropTable(
                name: "ClubMemberHistory");
        }
    }
}
