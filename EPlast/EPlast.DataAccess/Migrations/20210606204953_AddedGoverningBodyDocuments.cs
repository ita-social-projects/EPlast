using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddedGoverningBodyDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoverningBodyDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoverningBodyDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    BlobName = table.Column<string>(maxLength: 64, nullable: false),
                    FileName = table.Column<string>(maxLength: 120, nullable: false),
                    GoverningBodyDocumentTypeId = table.Column<int>(nullable: false),
                    GoverningBodyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningBodyDocuments_GoverningBodyDocumentTypes_GoverningBodyDocumentTypeId",
                        column: x => x.GoverningBodyDocumentTypeId,
                        principalTable: "GoverningBodyDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodyDocuments_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyDocuments_GoverningBodyDocumentTypeId",
                table: "GoverningBodyDocuments",
                column: "GoverningBodyDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyDocuments_GoverningBodyId",
                table: "GoverningBodyDocuments",
                column: "GoverningBodyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoverningBodyDocuments");

            migrationBuilder.DropTable(
                name: "GoverningBodyDocumentTypes");
        }
    }
}
