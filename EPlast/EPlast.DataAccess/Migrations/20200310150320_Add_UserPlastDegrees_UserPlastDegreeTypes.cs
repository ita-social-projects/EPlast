using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_UserPlastDegrees_UserPlastDegreeTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPlastDegreeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlastDegreeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPlastDegrees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    UserPlastDegreeTypeId = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateFinish = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlastDegrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlastDegrees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPlastDegrees_UserPlastDegreeTypes_UserPlastDegreeTypeId",
                        column: x => x.UserPlastDegreeTypeId,
                        principalTable: "UserPlastDegreeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_UserId",
                table: "UserPlastDegrees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlastDegrees_UserPlastDegreeTypeId",
                table: "UserPlastDegrees",
                column: "UserPlastDegreeTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPlastDegrees");

            migrationBuilder.DropTable(
                name: "UserPlastDegreeTypes");
        }
    }
}
