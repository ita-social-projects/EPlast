using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Rebuild_MembersStatistics_And_AnnualReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "PropertyList",
                table: "AnnualReports");

            migrationBuilder.RenameColumn(
                name: "SponsorFunds",
                table: "AnnualReports",
                newName: "SponsorshipFunds");

            migrationBuilder.RenameColumn(
                name: "PlastFunds",
                table: "AnnualReports",
                newName: "PublicFunds");

            migrationBuilder.RenameColumn(
                name: "GovernmentFunds",
                table: "AnnualReports",
                newName: "PlastSalary");

            migrationBuilder.AlterColumn<string>(
                name: "ImprovementNeeds",
                table: "AnnualReports",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "ListProperty",
                table: "AnnualReports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfAdministrators",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBeneficiaries",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfClubs",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfHonoraryMembers",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfIndependentGroups",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfIndependentRiy",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPlastpryiatMembers",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeatsInCity",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeatsPtashat",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTeacherAdministrators",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTeachers",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MembersStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    AnnualReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembersStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembersStatistics_AnnualReports_AnnualReportId",
                        column: x => x.AnnualReportId,
                        principalTable: "AnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembersStatistics_AnnualReportId",
                table: "MembersStatistics",
                column: "AnnualReportId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembersStatistics");

            migrationBuilder.DropColumn(
                name: "ListProperty",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfAdministrators",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfBeneficiaries",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfClubs",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfHonoraryMembers",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfIndependentGroups",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfIndependentRiy",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfPlastpryiatMembers",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfSeatsInCity",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfSeatsPtashat",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfTeacherAdministrators",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NumberOfTeachers",
                table: "AnnualReports");

            migrationBuilder.RenameColumn(
                name: "SponsorshipFunds",
                table: "AnnualReports",
                newName: "SponsorFunds");

            migrationBuilder.RenameColumn(
                name: "PublicFunds",
                table: "AnnualReports",
                newName: "PlastFunds");

            migrationBuilder.RenameColumn(
                name: "PlastSalary",
                table: "AnnualReports",
                newName: "GovernmentFunds");

            migrationBuilder.AlterColumn<string>(
                name: "ImprovementNeeds",
                table: "AnnualReports",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "AnnualReports",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PropertyList",
                table: "AnnualReports",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnnualReportId = table.Column<int>(nullable: false),
                    NumberOfBeneficiary = table.Column<int>(nullable: false),
                    NumberOfGnizd = table.Column<int>(nullable: false),
                    NumberOfGnizdForPtashat = table.Column<int>(nullable: false),
                    NumberOfHonoraryMembers = table.Column<int>(nullable: false),
                    NumberOfNovatstva = table.Column<int>(nullable: false),
                    NumberOfPlastpryiatMembers = table.Column<int>(nullable: false),
                    NumberOfPtashat = table.Column<int>(nullable: false),
                    NumberOfSamostiynyhRoiv = table.Column<int>(nullable: false),
                    NumberOfSenioryMembers = table.Column<int>(nullable: false),
                    NumberOfSenioryPryhylnyky = table.Column<int>(nullable: false),
                    NumberOfStarshiPlastunyMembers = table.Column<int>(nullable: false),
                    NumberOfStarshiPlastunyPryhylnyky = table.Column<int>(nullable: false),
                    NumberOfUnatstvaNeimenovani = table.Column<int>(nullable: false),
                    NumberOfUnatstvaPryhylnyky = table.Column<int>(nullable: false),
                    NumberOfUnatstvaRozviduvachi = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSkobyVirlytsi = table.Column<int>(nullable: false),
                    NumberOfUnatstvaUchasnyky = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statistics_AnnualReports_AnnualReportId",
                        column: x => x.AnnualReportId,
                        principalTable: "AnnualReports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_AnnualReportId",
                table: "Statistics",
                column: "AnnualReportId",
                unique: true);
        }
    }
}
