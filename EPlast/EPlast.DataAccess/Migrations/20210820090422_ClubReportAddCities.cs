using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ClubReportAddCities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubReportCities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubAnnualReportId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubReportCities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubReportCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubReportCities_ClubAnnualReports_ClubAnnualReportId",
                        column: x => x.ClubAnnualReportId,
                        principalTable: "ClubAnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubReportCities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportCities_CityId",
                table: "ClubReportCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportCities_ClubAnnualReportId",
                table: "ClubReportCities",
                column: "ClubAnnualReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubReportCities_UserId",
                table: "ClubReportCities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubReportCities");
        }
    }
}
