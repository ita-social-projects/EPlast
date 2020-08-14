using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddTableForDestinctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distinctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distinctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDistinctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistinctionId = table.Column<int>(nullable: false),
                    Reporter = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDistinctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDistinctions_Distinctions_DistinctionId",
                        column: x => x.DistinctionId,
                        principalTable: "Distinctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDistinctions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDistinctions_DistinctionId",
                table: "UserDistinctions",
                column: "DistinctionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDistinctions_UserId",
                table: "UserDistinctions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDistinctions");

            migrationBuilder.DropTable(
                name: "Distinctions");
        }
    }
}
