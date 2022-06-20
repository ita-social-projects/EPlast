using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class addnewfunctional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "RegionFollowers");

            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "RegionFollowers");

            migrationBuilder.DropColumn(
                name: "PostIndex",
                table: "RegionFollowers");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "OfficeNumber",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "PostIndex",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "RegionFollowers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Cities",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Cities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "RegionFollowers");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "RegionFollowers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "RegionFollowers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostIndex",
                table: "RegionFollowers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "Cities",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficeNumber",
                table: "Cities",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostIndex",
                table: "Cities",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Cities",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

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
                name: "UserPrecautionsTableObject",
                columns: table => new
                {
                    Count = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    PrecautionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reporter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
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
