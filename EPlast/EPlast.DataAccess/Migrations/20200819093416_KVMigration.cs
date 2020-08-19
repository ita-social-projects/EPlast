using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class KVMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVs_KVTypes_kvTypesID",
                table: "KVs");

            migrationBuilder.DropColumn(
                name: "KVTypeID",
                table: "KVs");

            migrationBuilder.RenameColumn(
                name: "kvTypesID",
                table: "KVs",
                newName: "KVTypesID");

            migrationBuilder.RenameIndex(
                name: "IX_KVs_kvTypesID",
                table: "KVs",
                newName: "IX_KVs_KVTypesID");

            migrationBuilder.AlterColumn<int>(
                name: "KVTypesID",
                table: "KVs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_KVTypes_KVTypesID",
                table: "KVs",
                column: "KVTypesID",
                principalTable: "KVTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVs_KVTypes_KVTypesID",
                table: "KVs");

            migrationBuilder.RenameColumn(
                name: "KVTypesID",
                table: "KVs",
                newName: "kvTypesID");

            migrationBuilder.RenameIndex(
                name: "IX_KVs_KVTypesID",
                table: "KVs",
                newName: "IX_KVs_kvTypesID");

            migrationBuilder.AlterColumn<int>(
                name: "kvTypesID",
                table: "KVs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "KVTypeID",
                table: "KVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_KVs_KVTypes_kvTypesID",
                table: "KVs",
                column: "kvTypesID",
                principalTable: "KVTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
