using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class addedGoverningBodyAdminRoleToGoverningBodyAdminstrationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations");

            migrationBuilder.DropTable(
                name: "SectorAnnouncementImage");

            migrationBuilder.DropTable(
                name: "SectorAnnouncement");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "UserPrecautionsTableObject");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "UserPrecautionsTableObject");

            migrationBuilder.AlterColumn<int>(
                name: "GoverningBodyId",
                table: "GoverningBodyAdministrations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "GoverningBodyAdminRole",
                table: "GoverningBodyAdministrations",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations",
                column: "GoverningBodyId",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations");

            migrationBuilder.DropColumn(
                name: "GoverningBodyAdminRole",
                table: "GoverningBodyAdministrations");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "UserPrecautionsTableObject",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "UserPrecautionsTableObject",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "GoverningBodyId",
                table: "GoverningBodyAdministrations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SectorAnnouncement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectorAnnouncementId = table.Column<int>(type: "int", nullable: false)
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
                name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                table: "GoverningBodyAdministrations",
                column: "GoverningBodyId",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
