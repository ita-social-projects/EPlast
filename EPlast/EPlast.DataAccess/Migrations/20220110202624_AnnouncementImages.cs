using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AnnouncementImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "GoverningBodyAnnouncementImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(nullable: false),
                    GoverningBodyAnnouncementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyAnnouncementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAnnouncementImages_GoverningBodyAnnouncement_GoverningBodyAnnouncementId",
                        column: x => x.GoverningBodyAnnouncementId,
                        principalTable: "GoverningBodyAnnouncement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoverningBodyAnnouncementImages");
        }
    }
}
