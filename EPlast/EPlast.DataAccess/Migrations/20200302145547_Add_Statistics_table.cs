using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_Statistics_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NumberOfGnizd = table.Column<int>(nullable: false),
                    NumberOfGnizdForPtashat = table.Column<int>(nullable: false),
                    NumberOfSamostiynyhRoiv = table.Column<int>(nullable: false),
                    NumberOfPtashat = table.Column<int>(nullable: false),
                    NumberOfNovatstva = table.Column<int>(nullable: false),
                    NumberOfUnatstvaNeimenovani = table.Column<int>(nullable: false),
                    NumberOfUnatstvaPryhylnyky = table.Column<int>(nullable: false),
                    NumberOfUnatstvaUchasnyky = table.Column<int>(nullable: false),
                    NumberOfUnatstvaRozviduvachi = table.Column<int>(nullable: false),
                    NumberOfUnatstvaSkobyVirlytsi = table.Column<int>(nullable: false),
                    NumberOfStarshiPlastunyPryhylnyky = table.Column<int>(nullable: false),
                    NumberOfStarshiPlastunyMembers = table.Column<int>(nullable: false),
                    NumberOfSenioryPryhylnyky = table.Column<int>(nullable: false),
                    NumberOfSenioryMembers = table.Column<int>(nullable: false),
                    NumberOfBeneficiary = table.Column<int>(nullable: false),
                    NumberOfPlastpryiatMembers = table.Column<int>(nullable: false),
                    NumberOfHonoraryMembers = table.Column<int>(nullable: false),
                    AnnualReportId = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}
