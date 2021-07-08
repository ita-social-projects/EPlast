using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddGoverningBodySectorTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoverningBodySectors");

            migrationBuilder.CreateTable(
                name: "GoverningBodySectorDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectorDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoverningBodyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 18, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectors_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectorAdministrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    SectorId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    AdminTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectorAdministrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorAdministrations_GoverningBodySectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "GoverningBodySectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodySectorDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(maxLength: 120, nullable: false),
                    SectorDocumentTypeId = table.Column<int>(nullable: false),
                    SectorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodySectorDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorDocuments_GoverningBodySectorDocumentTypes_SectorDocumentTypeId",
                        column: x => x.SectorDocumentTypeId,
                        principalTable: "GoverningBodySectorDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodySectorDocuments_GoverningBodySectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "GoverningBodySectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorAdministrations_AdminTypeId",
                table: "GoverningBodySectorAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorAdministrations_SectorId",
                table: "GoverningBodySectorAdministrations",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorAdministrations_UserId",
                table: "GoverningBodySectorAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorDocuments_SectorDocumentTypeId",
                table: "GoverningBodySectorDocuments",
                column: "SectorDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectorDocuments_SectorId",
                table: "GoverningBodySectorDocuments",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodySectors_GoverningBodyId",
                table: "GoverningBodySectors",
                column: "GoverningBodyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoverningBodySectorAdministrations");

            migrationBuilder.DropTable(
                name: "GoverningBodySectorDocuments");

            migrationBuilder.DropTable(
                name: "GoverningBodySectorDocumentTypes");

            migrationBuilder.DropTable(
                name: "GoverningBodySectors");
        }
    }
}
