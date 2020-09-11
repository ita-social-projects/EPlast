using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class KVLastFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypesID",
                table: "KVs");

            migrationBuilder.DropIndex(
                name: "IX_KVs_KadraVykhovnykivTypesID",
                table: "KVs");

            migrationBuilder.DropColumn(
                name: "KVTypesID",
                table: "KVs");

            migrationBuilder.DropColumn(
                name: "KadraVykhovnykivTypesID",
                table: "KVs");

            migrationBuilder.AddColumn<int>(
                name: "KadraVykhovnykivTypeId",
                table: "KVs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KVs_KadraVykhovnykivTypeId",
                table: "KVs",
                column: "KadraVykhovnykivTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypeId",
                table: "KVs",
                column: "KadraVykhovnykivTypeId",
                principalTable: "KVTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypeId",
                table: "KVs");

            migrationBuilder.DropIndex(
                name: "IX_KVs_KadraVykhovnykivTypeId",
                table: "KVs");

            migrationBuilder.DropColumn(
                name: "KadraVykhovnykivTypeId",
                table: "KVs");

            migrationBuilder.AddColumn<int>(
                name: "KVTypesID",
                table: "KVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KadraVykhovnykivTypesID",
                table: "KVs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KVs_KadraVykhovnykivTypesID",
                table: "KVs",
                column: "KadraVykhovnykivTypesID");

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypesID",
                table: "KVs",
                column: "KadraVykhovnykivTypesID",
                principalTable: "KVTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
