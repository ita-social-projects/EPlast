using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class addSectorAnnouncements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoverningBodyId",
                table: "GoverningBodyAnnouncement",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
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

            migrationBuilder.CreateTable(
                name: "SectorAnnouncement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    SectorId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorAnnouncement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorAnnouncement_GoverningBodySectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "GoverningBodySectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectorAnnouncement_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SectorAnnouncementImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(nullable: false),
                    SectorAnnouncementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorAnnouncementImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorAnnouncementImage_SectorAnnouncement_SectorAnnouncementId",
                        column: x => x.SectorAnnouncementId,
                        principalTable: "SectorAnnouncement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAnnouncement_GoverningBodyId",
                table: "GoverningBodyAnnouncement",
                column: "GoverningBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorAnnouncement_SectorId",
                table: "SectorAnnouncement",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorAnnouncement_UserId",
                table: "SectorAnnouncement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorAnnouncementImage_SectorAnnouncementId",
                table: "SectorAnnouncementImage",
                column: "SectorAnnouncementId");

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

            migrationBuilder.DropTable(
                name: "SectorAnnouncementImage");

            migrationBuilder.DropTable(
                name: "SectorAnnouncement");

            migrationBuilder.DropIndex(
                name: "IX_GoverningBodyAnnouncement_GoverningBodyId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.DropColumn(
                name: "GoverningBodyId",
                table: "GoverningBodyAnnouncement");

            migrationBuilder.DropColumn(
                name: "Title",
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
