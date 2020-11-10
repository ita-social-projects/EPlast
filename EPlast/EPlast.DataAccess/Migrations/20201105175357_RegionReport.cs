using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RegionReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegionAnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfSeatsPtashat = table.Column<int>(nullable: false),
                    NumberOfPtashata = table.Column<int>(nullable: false),
                    NumberOfNovatstva = table.Column<int>(nullable: false),
                    NumberOfUnatstvaNoname = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSupporters = table.Column<int>(nullable: false),
                    NumberOfUnatstvaMembers = table.Column<int>(nullable: false),
                    NumberOfUnatstvaProspectors = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSkobVirlyts = table.Column<int>(nullable: false),
                    NumberOfSeniorPlastynSupporters = table.Column<int>(nullable: false),
                    NumberOfSeniorPlastynMembers = table.Column<int>(nullable: false),
                    NumberOfSeigneurSupporters = table.Column<int>(nullable: false),
                    NumberOfSeigneurMembers = table.Column<int>(nullable: false),
                    NumberOfIndependentRiy = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    NumberOfClubs = table.Column<int>(nullable: false),
                    NumberOfIndependentGroups = table.Column<int>(nullable: false),
                    NumberOfTeachers = table.Column<int>(nullable: false),
                    NumberOfAdministrators = table.Column<int>(nullable: false),
                    NumberOfTeacherAdministrators = table.Column<int>(nullable: false),
                    NumberOfBeneficiaries = table.Column<int>(nullable: false),
                    NumberOfPlastpryiatMembers = table.Column<int>(nullable: false),
                    NumberOfHonoraryMembers = table.Column<int>(nullable: false),
                    MembersStatisticId = table.Column<int>(nullable: true),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionAnnualReports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionAnnualReports_MembersStatistics_MembersStatisticId",
                        column: x => x.MembersStatisticId,
                        principalTable: "MembersStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegionAnnualReports_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegionAnnualReports_MembersStatisticId",
                table: "RegionAnnualReports",
                column: "MembersStatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAnnualReports_RegionId",
                table: "RegionAnnualReports",
                column: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionAnnualReports");
        }
    }
}
