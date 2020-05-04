using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Drop_AnnualReportStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AnnualReportStatuses_AnnualReportStatusId",
                table: "AnnualReports");

            migrationBuilder.DropTable(
                name: "AnnualReportStatuses");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_AnnualReportStatusId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "AnnualReportStatusId",
                table: "AnnualReports");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AnnualReports");

            migrationBuilder.AddColumn<int>(
                name: "AnnualReportStatusId",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnnualReportStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualReportStatuses", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_AnnualReportStatusId",
                table: "AnnualReports",
                column: "AnnualReportStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AnnualReportStatuses_AnnualReportStatusId",
                table: "AnnualReports",
                column: "AnnualReportStatusId",
                principalTable: "AnnualReportStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
