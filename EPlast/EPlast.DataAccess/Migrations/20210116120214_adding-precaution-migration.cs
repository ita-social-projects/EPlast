using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class addingprecautionmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Precautions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Precautions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPrecautions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecautionId = table.Column<int>(nullable: false),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrecautions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrecautions_Precautions_PrecautionId",
                        column: x => x.PrecautionId,
                        principalTable: "Precautions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrecautions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPrecautions_PrecautionId",
                table: "UserPrecautions",
                column: "PrecautionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrecautions_UserId",
                table: "UserPrecautions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPrecautions");

            migrationBuilder.DropTable(
                name: "Precautions");
        }
    }
}
