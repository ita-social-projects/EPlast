using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RegionAnualReportStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RegionAnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnnualReportTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    CanManage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ClubAnnualReportTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    ClubName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    CanManage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DecisionTableObject",
                columns: table => new
                {
                    Total = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DecisionStatusType = table.Column<int>(nullable: false),
                    GoverningBody = table.Column<string>(nullable: true),
                    DecisionTarget = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RegionAnnualReportTableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RegionName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserDistinctionsTableObject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    DistinctionName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserPrecautionsTableObject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    PrecautionName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserTableObjects",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: true),
                    Index = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    ClubName = table.Column<string>(nullable: true),
                    PlastDegree = table.Column<string>(nullable: true),
                    Roles = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    UPUDegree = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "ClubAnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "DecisionTableObject");

            migrationBuilder.DropTable(
                name: "RegionAnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "UserDistinctionsTableObject");

            migrationBuilder.DropTable(
                name: "UserPrecautionsTableObject");

            migrationBuilder.DropTable(
                name: "UserTableObjects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RegionAnnualReports");
        }
    }
}
