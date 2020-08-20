using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class KadraEntityAdding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KVTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KVs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    KVTypesID = table.Column<int>(nullable: false),
                    KadraVykhovnykivTypesID = table.Column<int>(nullable: true),
                    DateOfGranting = table.Column<DateTime>(nullable: false),
                    NumberInRegister = table.Column<int>(nullable: false),
                    BasisOfGranting = table.Column<string>(maxLength: 100, nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KVs_KVTypes_KadraVykhovnykivTypesID",
                        column: x => x.KadraVykhovnykivTypesID,
                        principalTable: "KVTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KVs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KVs_KadraVykhovnykivTypesID",
                table: "KVs",
                column: "KadraVykhovnykivTypesID");

            migrationBuilder.CreateIndex(
                name: "IX_KVs_UserId",
                table: "KVs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KVs");

            migrationBuilder.DropTable(
                name: "KVTypes");
        }
    }
}
