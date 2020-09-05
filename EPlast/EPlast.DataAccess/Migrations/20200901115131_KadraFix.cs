using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class KadraFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypeID",
                table: "KVs");

            migrationBuilder.DropForeignKey(
                name: "FK_KVs_AspNetUsers_UserId",
                table: "KVs");

            migrationBuilder.RenameColumn(
                name: "KadraVykhovnykivTypeID",
                table: "KVs",
                newName: "KadraVykhovnykivTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_KVs_KadraVykhovnykivTypeID",
                table: "KVs",
                newName: "IX_KVs_KadraVykhovnykivTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "KVs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KadraVykhovnykivTypeId",
                table: "KVs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypeId",
                table: "KVs",
                column: "KadraVykhovnykivTypeId",
                principalTable: "KVTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_AspNetUsers_UserId",
                table: "KVs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypeId",
                table: "KVs");

            migrationBuilder.DropForeignKey(
                name: "FK_KVs_AspNetUsers_UserId",
                table: "KVs");

            migrationBuilder.RenameColumn(
                name: "KadraVykhovnykivTypeId",
                table: "KVs",
                newName: "KadraVykhovnykivTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_KVs_KadraVykhovnykivTypeId",
                table: "KVs",
                newName: "IX_KVs_KadraVykhovnykivTypeID");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "KVs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "KadraVykhovnykivTypeID",
                table: "KVs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_KVTypes_KadraVykhovnykivTypeID",
                table: "KVs",
                column: "KadraVykhovnykivTypeID",
                principalTable: "KVTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_AspNetUsers_UserId",
                table: "KVs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
