using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Set_NotNull_RegionID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionID",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAdministrations_AdminTypes_AdminTypeID",
                table: "RegionAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAdministrations_Regions_RegionID",
                table: "RegionAdministrations");

            migrationBuilder.RenameColumn(
                name: "RegionID",
                table: "RegionAdministrations",
                newName: "RegionId");

            migrationBuilder.RenameColumn(
                name: "AdminTypeID",
                table: "RegionAdministrations",
                newName: "AdminTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_RegionAdministrations_RegionID",
                table: "RegionAdministrations",
                newName: "IX_RegionAdministrations_RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_RegionAdministrations_AdminTypeID",
                table: "RegionAdministrations",
                newName: "IX_RegionAdministrations_AdminTypeId");

            migrationBuilder.RenameColumn(
                name: "RegionID",
                table: "Cities",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_RegionID",
                table: "Cities",
                newName: "IX_Cities_RegionId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RegionAdministrations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "RegionAdministrations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAdministrations_AdminTypes_AdminTypeId",
                table: "RegionAdministrations",
                column: "AdminTypeId",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAdministrations_Regions_RegionId",
                table: "RegionAdministrations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAdministrations_AdminTypes_AdminTypeId",
                table: "RegionAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_RegionAdministrations_Regions_RegionId",
                table: "RegionAdministrations");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "RegionAdministrations",
                newName: "RegionID");

            migrationBuilder.RenameColumn(
                name: "AdminTypeId",
                table: "RegionAdministrations",
                newName: "AdminTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_RegionAdministrations_RegionId",
                table: "RegionAdministrations",
                newName: "IX_RegionAdministrations_RegionID");

            migrationBuilder.RenameIndex(
                name: "IX_RegionAdministrations_AdminTypeId",
                table: "RegionAdministrations",
                newName: "IX_RegionAdministrations_AdminTypeID");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                table: "Cities",
                newName: "RegionID");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                newName: "IX_Cities_RegionID");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RegionAdministrations",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "RegionID",
                table: "RegionAdministrations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RegionID",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionID",
                table: "Cities",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAdministrations_AdminTypes_AdminTypeID",
                table: "RegionAdministrations",
                column: "AdminTypeID",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionAdministrations_Regions_RegionID",
                table: "RegionAdministrations",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
