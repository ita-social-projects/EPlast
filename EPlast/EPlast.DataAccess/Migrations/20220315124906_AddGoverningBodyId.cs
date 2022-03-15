using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddGoverningBodyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoverningBodyId",
                table: "GoverningBodyAnnouncement",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SponsorshipFunds",
                table: "AnnualReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PublicFunds",
                table: "AnnualReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlastSalary",
                table: "AnnualReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContributionFunds",
                table: "AnnualReports",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncement_GoverningBodyId",
                table: "GoverningBodyAnnouncement",
                column: "GoverningBodyId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningBodyAnnouncement_Organization_GoverningBodyId",
                table: "GoverningBodyAnnouncement",
                column: "GoverningBodyId",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningBodyAnnouncement_Organization_GoverningBodyId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.DropIndex(
                name: "IX_GoverningBodyAnnouncement_GoverningBodyId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.DropColumn(
                name: "GoverningBodyId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.AlterColumn<int>(
                name: "SponsorshipFunds",
                table: "AnnualReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "PublicFunds",
                table: "AnnualReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "PlastSalary",
                table: "AnnualReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "ContributionFunds",
                table: "AnnualReports",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
