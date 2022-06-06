using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RemovePrecautionTableObjectTotalAndCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
