using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddCompanyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoverningBodyAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    GoverningBodyId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    AdminTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningBodyAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAdministrations_AdminTypes_AdminTypeId",
                        column: x => x.AdminTypeId,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAdministrations_Organization_GoverningBodyId",
                        column: x => x.GoverningBodyId,
                        principalTable: "Organization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoverningBodyAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAdministrations_AdminTypeId",
                table: "GoverningBodyAdministrations",
                column: "AdminTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAdministrations_GoverningBodyId",
                table: "GoverningBodyAdministrations",
                column: "GoverningBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningBodyAdministrations_UserId",
                table: "GoverningBodyAdministrations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoverningBodyAdministrations");
        }
    }
}
