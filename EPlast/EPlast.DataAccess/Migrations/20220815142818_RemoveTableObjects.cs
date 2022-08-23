using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RemoveTableObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "CityObjects");

            migrationBuilder.DropTable(
                name: "ClubAnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "DecisionTableObject");

            migrationBuilder.DropTable(
                name: "EducatorsStaffTableObjects");

            migrationBuilder.DropTable(
                name: "MethodicDocumentTableObjects");

            migrationBuilder.DropTable(
                name: "RegionAnnualReportTableObjects");

            migrationBuilder.DropTable(
                name: "RegionMembersInfoTableObjects");

            migrationBuilder.DropTable(
                name: "RegionNamesObjects");

            migrationBuilder.DropTable(
                name: "RegionObjects");

            migrationBuilder.DropTable(
                name: "UserDistinctionsTableObject");

            migrationBuilder.DropTable(
                name: "UserRenewalsTableObjects");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualReportTableObjects",
                columns: table => new
                {
                    CanManage = table.Column<bool>(type: "bit", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CityObjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityObjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClubAnnualReportTableObjects",
                columns: table => new
                {
                    CanManage = table.Column<bool>(type: "bit", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DecisionTableObject",
                columns: table => new
                {
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DecisionStatusType = table.Column<int>(type: "int", nullable: false),
                    DecisionTarget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoverningBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EducatorsStaffTableObjects",
                columns: table => new
                {
                    DateOfGranting = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    KadraName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KadraVykhovnykivTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberInRegister = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MethodicDocumentTableObjects",
                columns: table => new
                {
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoverningBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RegionAnnualReportTableObjects",
                columns: table => new
                {
                    CanManage = table.Column<bool>(type: "bit", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RegionMembersInfoTableObjects",
                columns: table => new
                {
                    CityAnnualReportId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfAdministrators = table.Column<int>(type: "int", nullable: true),
                    NumberOfBeneficiaries = table.Column<int>(type: "int", nullable: true),
                    NumberOfClubs = table.Column<int>(type: "int", nullable: true),
                    NumberOfHonoraryMembers = table.Column<int>(type: "int", nullable: true),
                    NumberOfIndependentGroups = table.Column<int>(type: "int", nullable: true),
                    NumberOfIndependentRiy = table.Column<int>(type: "int", nullable: true),
                    NumberOfNovatstva = table.Column<int>(type: "int", nullable: true),
                    NumberOfPlastpryiatMembers = table.Column<int>(type: "int", nullable: true),
                    NumberOfPtashata = table.Column<int>(type: "int", nullable: true),
                    NumberOfSeatsPtashat = table.Column<int>(type: "int", nullable: true),
                    NumberOfSeigneurMembers = table.Column<int>(type: "int", nullable: true),
                    NumberOfSeigneurSupporters = table.Column<int>(type: "int", nullable: true),
                    NumberOfSeniorPlastynMembers = table.Column<int>(type: "int", nullable: true),
                    NumberOfSeniorPlastynSupporters = table.Column<int>(type: "int", nullable: true),
                    NumberOfTeacherAdministrators = table.Column<int>(type: "int", nullable: true),
                    NumberOfTeachers = table.Column<int>(type: "int", nullable: true),
                    NumberOfUnatstvaMembers = table.Column<int>(type: "int", nullable: true),
                    NumberOfUnatstvaNoname = table.Column<int>(type: "int", nullable: true),
                    NumberOfUnatstvaProspectors = table.Column<int>(type: "int", nullable: true),
                    NumberOfUnatstvaSkobVirlyts = table.Column<int>(type: "int", nullable: true),
                    NumberOfUnatstvaSupporters = table.Column<int>(type: "int", nullable: true),
                    ReportStatus = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RegionNamesObjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionNamesObjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RegionObjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionObjects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserDistinctionsTableObject",
                columns: table => new
                {
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DistinctionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reporter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UserRenewalsTableObjects",
                columns: table => new
                {
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

        }
    }
}
