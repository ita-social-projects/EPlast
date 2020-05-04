using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_CityLegalStatuses_CityLegalStatusTypes_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityLegalStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLegalStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityLegalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityId = table.Column<int>(nullable: false),
                    CityLegalStatusTypeId = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityLegalStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityLegalStatuses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityLegalStatuses_CityLegalStatusTypes_CityLegalStatusTypeId",
                        column: x => x.CityLegalStatusTypeId,
                        principalTable: "CityLegalStatusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityLegalStatuses_CityId",
                table: "CityLegalStatuses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CityLegalStatuses_CityLegalStatusTypeId",
                table: "CityLegalStatuses",
                column: "CityLegalStatusTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityLegalStatuses");

            migrationBuilder.DropTable(
                name: "CityLegalStatusTypes");
        }
    }
}
